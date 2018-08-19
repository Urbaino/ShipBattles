import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import Player from '../types/Player';
import GameSummary from '../types/GameSummary';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalR';
import Challenge from '../types/Challenge';

interface LobbyState {
    Hub: HubConnection,
    Me: Player,
    Players: Array<Player>,
    Games: Array<GameSummary>,
    Challenge?: Challenge
}

export class Lobby extends React.Component<RouteComponentProps<{}>, LobbyState> {
    constructor() {
        super();
        this.state = {
            Hub: new HubConnectionBuilder().withUrl('/lobbyHub', { logger: LogLevel.Debug }).build(),
            Me: { id: '', name: '' },
            Players: [],
            Games: []
        };

        this.state.Hub.on('Initialize', (me: Player, players: Array<Player>, games: Array<GameSummary>) => {
            this.setState(() => { return { Me: me, Players: players, Games: games } });
        });

        this.state.Hub.on('NewPlayer', (player) => {
            this.setState((prevState) => {
                var playersNew = prevState.Players.concat(player);
                return { Players: playersNew };
            });
        })

        this.state.Hub.on('PlayerDisconnected', (player) => {
            this.setState((prevState) => {
                var playersNew = prevState.Players.slice(0);
                var playerIndex = playersNew.indexOf(player);
                return { Players: playersNew.splice(playerIndex, 1) };
            });
        });

        this.state.Hub.on('ChallengeReceived', (newChallenge: Challenge, challengeState: string) => {
            this.setState(() => {
                newChallenge.state = challengeState;
                return { Challenge: newChallenge }
            });
        });

        this.state.Hub.on('GameOn', (gameId: string) => {
            this.redirectToGame(gameId);
        });

        this.state.Hub.on('NoGame', () => {
            this.setState(() => { return { Challenge: undefined } });
        });
    }

    componentDidMount() {
        this.state.Hub
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));
    }

    sendChallenge = (player: string) => {
        this.state.Hub.invoke('ChallengePlayer', player);
        this.setState(() => {
            return { Challenge: { playerB: player } }
        });
    }

    acceptChallenge = () => {
        if (this.state.Challenge === undefined) return;
        this.state.Hub.invoke('AcceptChallenge', this.state.Challenge.state);
    }

    declineChallenge = () => {
        if (this.state.Challenge === undefined) return;
        this.state.Hub.invoke('DeclineChallenge', this.state.Challenge.state);
        this.setState(() => { return { Challenge: undefined } });
    }

    redirectToGame = (gameId: string) => {
        window.location.href = `play/${gameId}`;
    }

    public render() {
        return <div>
            <h1>Welcome {this.state.Me.name}</h1>

            {this.state.Challenge && this.state.Challenge.playerA && <div className="modal" onClick={this.declineChallenge}>
                <div className="modal-content">
                    <p>You've been challenged by {this.state.Challenge.playerA}!</p>
                    <span>
                        <button onClick={this.acceptChallenge}>Accept</button>
                        <button onClick={this.declineChallenge}>Decline</button>
                    </span>
                </div>
            </div>}

            {this.state.Challenge && !this.state.Challenge.playerA && <div>
                Waiting for challenge reply from {this.state.Challenge.playerB}...
            </div>}

            <h2>Currently online players:</h2>
            {this.state.Players.map((player, i) => {
                return <button key={i} onClick={() => this.sendChallenge(player.name)}>{player.name}</button>
            })}

            <h2>Games in progress:</h2>
            {this.state.Games.filter(game => game.resultIsVictory == undefined).map((game, i) => {
                return <button key={i} onClick={() => this.redirectToGame(game.id)}>{game.opponentName} - {game.timestamp.toString()}</button>
            })}

            <h2>History:</h2>
            {this.state.Games.filter(game => game.resultIsVictory != undefined).map((game, i) => {
                return <button key={i} onClick={() => this.redirectToGame(game.id)}>{game.opponentName}</button>
            })}

        </div>;
    }

}
