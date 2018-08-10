import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import Board, { BoardState } from '../types/domain/Board';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@aspnet/signalr';
import Player from '../types/Player';
import { GameState } from '../types/domain/GameState';
import Game from '../types/domain/Game';
import Ship, { Direction } from '../types/domain/Ship';
import Coordinate from '../types/domain/Coordinate';
import Shot from '../types/domain/Shot';

interface PlayState {
    hub: HubConnection,
    me: Player,
    gameId: string,
    gameState: GameState,
    board: BoardState,
    enemyBoard: BoardState,
    opponent: string,
    shipToPlace: Ship
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
            opponent: '',
            shipToPlace: { origin: { x: 0, y: 0 }, length: 4, heading: Direction.North }
        };

        let setStateFromPlayState = (game: PlayState) => {
            this.setState((prevState) => {
                var newShipToPlace: Ship;
                switch (game.gameState) {
                    case GameState.PlaceShip4:
                        newShipToPlace = { origin: { x: 0, y: 0 }, length: 4, heading: Direction.North };
                        break;
                    case GameState.PlaceShip3:
                        newShipToPlace = { origin: { x: 0, y: 0 }, length: 3, heading: Direction.North };
                        break;
                    case GameState.PlaceShip2:
                        newShipToPlace = { origin: { x: 0, y: 0 }, length: 2, heading: Direction.North };
                        break;
                    case GameState.PlaceShip1:
                        newShipToPlace = { origin: { x: 0, y: 0 }, length: 1, heading: Direction.North };
                        break;
                    default:
                        newShipToPlace = prevState.shipToPlace;
                };

                return {
                    me: game.me,
                    gameId: game.gameId,
                    gameState: game.gameState,
                    board: game.board,
                    enemyBoard: game.enemyBoard,
                    opponent: game.opponent,
                    shipToPlace: newShipToPlace
                }
            });
        }

        this.state.hub.on('GameUpdate', (game: PlayState) => {
            setStateFromPlayState(game);
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
        let shot: Shot = { coordinates: { x: x, y: y } };
        this.state.hub.invoke('Fire', this.state.gameId, shot);
    }

    placeShip = (x: number, y: number) => {
        let ship: Ship = { origin: { x: x, y: y }, length: this.state.shipToPlace.length, heading: this.state.shipToPlace.heading };
        this.state.hub.invoke('PlaceShip', this.state.gameId, ship);
    }

    public render() {
        return <div>

            {this.renderHeader()}

            {this.renderInstruction()}

            <p>Your board:</p>
            <Board board={this.state.board} clickCallback={this.placeShip} />
            <p>{this.state.opponent}'s board:</p>
            <Board board={this.state.enemyBoard} clickCallback={this.fire} />

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
        else
            return this.renderShipToPlace();
    }

    renderShipToPlace() {
        switch (this.state.gameState) {
            case GameState.PlaceShip4:
            case GameState.PlaceShip3:
            case GameState.PlaceShip2:
            case GameState.PlaceShip1:
                break;
            default:
                return false;
        }

        let length = this.state.shipToPlace.length;
        let rows = [];
        for (let i = -length; i <= length; i++) {
            let cols = [];
            for (let j = -length; j <= length; j++) {
                let isShip: boolean = false;
                switch (this.state.shipToPlace.heading) {
                    case Direction.North:
                        isShip = (j == 0 && i <= 0 && i > -length);
                        break;
                    case Direction.East:
                        isShip = (i == 0 && j >= 0 && j < length);
                        break;
                    case Direction.West:
                        isShip = (i == 0 && j <= 0 && j > -length);
                        break;
                    case Direction.South:
                        isShip = (j == 0 && i >= 0 && i < length);
                        break;
                }

                let stateSetCallback = (direction: Direction) => {
                    this.setState((prevState) => {
                        let newShipToPlace: Ship = {
                            heading: direction,
                            origin: prevState.shipToPlace.origin,
                            length: prevState.shipToPlace.length
                        };
                        return { shipToPlace: newShipToPlace };
                    })
                };
                let content;
                if (j === 0 && i === -length) {
                    content = <button className='dirChange' onClick={() => stateSetCallback(Direction.North)}></button>;
                };
                if (j === 0 && i === length) {
                    content = <button className='dirChange' onClick={() => stateSetCallback(Direction.South)}></button>;
                };
                if (i === 0 && j === -length) {
                    content = <button className='dirChange' onClick={() => stateSetCallback(Direction.West)}></button>;
                };
                if (i === 0 && j === length) {
                    content = <button className='dirChange' onClick={() => stateSetCallback(Direction.East)}></button>;
                };

                cols.push(<td key={j} className={`boardColumn ${isShip ? 'ship' : ''}`}>{content}</td>);
            }
            rows.push(<tr key={i} className="boardRow">{cols}</tr>);
        }

        return (
            <table className="boardTable" >
                <tbody>
                    {rows}
                </tbody>
            </table>
        );
    }
}
