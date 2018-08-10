import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import Board, { BoardState } from '../types/domain/Board';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@aspnet/signalr';
import Game from '../types/domain/Game';
import Player from '../types/Player';

interface PlayState {
    Hub: HubConnection,
    Me: Player,
    Board: BoardState,
    EnemyBoard: BoardState
    Opponent: string;
}

export class Play extends React.Component<RouteComponentProps<{}>, PlayState> {

    constructor() {
        super();
        this.state = {
            Hub: new HubConnectionBuilder().withUrl('/gameHub', { logger: LogLevel.Debug }).build(),
            Me: { id: '', name: '' },
            Board: { width: 0, height: 0, ships: [], shots: [] },
            EnemyBoard: { width: 0, height: 0, ships: [], shots: [] },
            Opponent: ''
        };

        let setStateFromGame = (game: Game) => {
            this.setState((prevState) => {
                let isPlayerA: boolean = prevState.Me.id === game.playerA;
                return {
                    Board: isPlayerA ? game.boardA : game.boardB,
                    EnemyBoard: isPlayerA ? game.boardB : game.boardA,
                    Opponent: isPlayerA ? game.playerb : game.playerA
                }
            });
        }

        this.state.Hub.on('Initialize', (game: Game, me: Player) => {
            let isPlayerA: boolean = me.id === game.playerA;
            this.setState({ Me: me });
            setStateFromGame(game);
        });

        this.state.Hub.on('GameUpdate', (game: Game) => {
            setStateFromGame(game);
        });
    }

    componentDidMount() {
        this.state.Hub
            .start()
            .then(() => {
                console.log('Connection started!');
                var gameId: string = this.props.location.pathname.split('/')[2];
                this.state.Hub.invoke('GetGameInfo', gameId);
            })
            .catch(err => console.log('Error while establishing connection :('));
    }

    public render() {
        return <div>
            <h1>Game on!</h1>

            <p>Your board:</p>
            <Board board={this.state.Board} />
            <p>Enemy board:</p>
            <Board board={this.state.EnemyBoard} />

            <h2>State:</h2>
            <div>{JSON.stringify(this.state)}</div>

        </div>;
    }

}
