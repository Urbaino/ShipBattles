import Ship, { Direction } from "./Ship";
import ShotModel from "./Shot";
import * as React from "react";
import { RouteComponentProps } from "react-router";

interface BoardProps {
    board : BoardState,
    clickCallback : Function
}

export interface BoardState {
    ships: Array<Ship>,
    shots: Array<ShotModel>,
    width: number,
    height: number
}

export default function Board(props: BoardProps) {
    enum CellState { Ship, Shot, ShotShip };

    var board: BoardState = props.board;

    let items: CellState[][] = [];
    for (let i = 0; i < board.width; i++) {
        items[i] = [];
    }

    board.ships.forEach((ship: Ship) => { // NOTE: Dictionary would be nice
        var x = ship.origin.x;
        var y = ship.origin.y;
        for (let length = 0; length < ship.length; length++) {
            items[x][y] = CellState.Ship;
            switch (ship.heading) {
                case Direction.North:
                    y -= 1;
                    break;
                case Direction.East:
                    x += 1;
                    break;
                case Direction.West:
                    x -= 1;
                    break;
                case Direction.South:
                    y += 1;
                    break;
            }
        }
    });

    board.shots.forEach((shot: ShotModel) => {
        var value: CellState = shot.hit ? CellState.ShotShip : CellState.Shot;
        items[shot.coordinates.x][shot.coordinates.y] = value;
    });


    let cells = [];
    for (let y = 0; y < board.height; y++) {
        for (let x = 0; x < board.width; x++) {
            let state: string = '';
            switch (items[x][y]) {
                case CellState.Ship:
                    state = 'ship';
                    break;
                case CellState.Shot:
                    state = 'shot';
                    break;
                case CellState.ShotShip:
                    state = 'shotShip';
                    break;
                default:
                    break;
            }
            cells.push(<rect key={`${x};${y}`} x={x*25} y={y*25} className={`boardCell ${state}`} onClick={() => props.clickCallback(x, y)}></rect>);
        }
    }

    return (
        <svg className="board">
            {cells}
        </svg>
    );
} 
