import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import Board, { BoardState } from '../types/domain/Board';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@aspnet/signalr';
import Player from '../types/Player';
import { GameState } from '../types/domain/GameState';
import Game from '../types/domain/Game';
import Ship, { Direction } from '../types/domain/Ship';
import Coordinate from '../types/domain/Coordinate';
import ShotModel from '../types/domain/Shot';

interface PlayState {
    hub: HubConnection,
    me: Player,
    gameId: string,
    gameState: GameState,
    board: BoardState,
    enemyBoard: BoardState,
    opponent: string,
    shipToPlace?: Ship,
    message?: string
}

export class Play extends React.Component<RouteComponentProps<{}>, PlayState> {

    constructor() {
        super();
        this.state = {
            hub: new HubConnectionBuilder().withUrl('/gameHub', { logger: LogLevel.Debug }).build(),
            me: { id: '', name: '' },
            gameId: '',
            gameState: GameState.NotYourTurn,
            board: { width: 0, height: 0, ships: [], shots: [] },
            enemyBoard: { width: 0, height: 0, ships: [], shots: [] },
            opponent: ''
        };

        let setStateFromPlayState = (game: PlayState) => {
            this.setState((prevState) => {
                var newShipToPlace: Ship | undefined;
                var newHeading: Direction = prevState.shipToPlace ? prevState.shipToPlace.heading : Direction.North;
                var newOrigin: Coordinate = prevState.shipToPlace ? prevState.shipToPlace.origin : { x: 0, y: 0 };
                switch (game.gameState) {
                    case GameState.PlaceShip4:
                        newShipToPlace = { origin: newOrigin, length: 4, heading: newHeading };
                        break;
                    case GameState.PlaceShip3:
                        newShipToPlace = { origin: newOrigin, length: 3, heading: newHeading };
                        break;
                    case GameState.PlaceShip2:
                        newShipToPlace = { origin: newOrigin, length: 2, heading: newHeading };
                        break;
                    case GameState.PlaceShip1:
                        newShipToPlace = { origin: newOrigin, length: 1, heading: newHeading };
                        break;
                    default:
                        newShipToPlace = undefined;
                };

                return {
                    me: game.me,
                    gameId: game.gameId,
                    gameState: game.gameState,
                    board: game.board,
                    enemyBoard: game.enemyBoard,
                    opponent: game.opponent,
                    shipToPlace: newShipToPlace,
                    message: undefined
                }
            });
        }

        this.state.hub.on('GameUpdate', (game: PlayState) => {
            setStateFromPlayState(game);
        });

        this.state.hub.on('Message', (msg: string) => {
            this.setState((prevstate) => {
                return { message: msg };
            });
        });
    }

    componentDidMount() {
        this.state.hub
            .start()
            .then(() => {
                console.log('Connection started!');
                var gameId: string = this.props.location.pathname.split('/')[2];
                this.state.hub.invoke('GetGameInfo', gameId);
            })
            .catch(err => console.log('Error while establishing connection :('));
    }

    fire = (x: number, y: number) => {
        let shot: Coordinate = { x: x, y: y };
        this.setState((prevstate) => { return { message: undefined }; });
        this.state.hub.invoke('Fire', this.state.gameId, shot);
    }

    placeShip = (x: number, y: number) => {
        if (!this.state.shipToPlace) return;

        let ship: Ship = { origin: { x: x, y: y }, length: this.state.shipToPlace.length, heading: this.state.shipToPlace.heading };
        this.setState((prevstate) => { return { message: undefined }; });
        this.state.hub.invoke('PlaceShip', this.state.gameId, ship);
    }

    rotateShip = () => {
        this.setState((prevState) => {
            if (!prevState.shipToPlace) return;
            var newHeading: Direction;
            switch (prevState.shipToPlace.heading) {
                case Direction.North:
                    newHeading = Direction.East;
                    break;
                case Direction.East:
                    newHeading = Direction.South;
                    break;
                case Direction.South:
                    newHeading = Direction.West;
                    break;
                case Direction.West:
                    newHeading = Direction.North;
                    break;
                default:
                    throw "Invalid direction";
            }
            return {
                shipToPlace: {
                    origin: { x: prevState.shipToPlace.origin.x, y: prevState.shipToPlace.origin.y },
                    length: prevState.shipToPlace.length,
                    heading: newHeading
                }
            }
        });
        return false; // Hide context menu
    }

    previewShip = (x: number, y: number) => {
        this.setState((prevState) => {
            if (!prevState.shipToPlace) return;

            return {
                shipToPlace: {
                    origin: { x: x, y: y },
                    length: prevState.shipToPlace.length,
                    heading: prevState.shipToPlace.heading
                }
            }
        });
    }

    public render() {
        return <div>

            {this.renderHeader()}

            <div className="errorMessage">
                {this.state.message && <span>
                    {this.state.message}
                </span>}
            </div>

            <div className="info">
                {this.renderInstruction()}
            </div>

            <div className="boards">
                <span className="playerBoard boardHolder">
                    <p>Your board:</p>
                    <Board board={this.state.board} clickCallback={this.placeShip} mouseOverCallback={this.previewShip} shipToPlace={this.state.shipToPlace} rightClickCallback={this.rotateShip} />
                </span>
                <span className="opponentBoard boardHolder">
                    <p>{this.state.opponent}'s board:</p>
                    <Board board={this.state.enemyBoard} clickCallback={this.fire} />
                </span>
            </div>

        </div>;
    }

    renderHeader() {
        if (this.state.gameState === GameState.Win)
            return <h1>You won!</h1>;
        else if (this.state.gameState === GameState.Lose)
            return <h1>You lost.</h1>;
        else
            return <h1>Game on, {this.state.me.name}!</h1>;
    }

    renderInstruction() {
        if (this.state.gameState === GameState.ReadyToPlay)
            return <p>Waiting for {this.state.opponent} to place ships.</p>;
        if (this.state.gameState === GameState.Fire)
            return <p>Your turn to fire!</p>;
        if (this.state.gameState === GameState.NotYourTurn)
            return <p>Waiting for {this.state.opponent} to fire.</p>;
            if (this.state.gameState === GameState.Lose || this.state.gameState === GameState.Win)
            return <p>Well played!</p>;
        else
            return <p>Please place your ships. Right click to rotate.</p>;
    }

}
