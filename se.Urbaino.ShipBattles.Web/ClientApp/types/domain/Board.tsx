import Ship, { Direction } from "./Ship";
import ShotModel from "./Shot";
import * as React from "react";
import { RouteComponentProps } from "react-router";

interface BoardProps {
    board: BoardState,
    clickCallback: Function
}

export interface BoardState {
    ships: Array<Ship>,
    shots: Array<ShotModel>,
    width: number,
    height: number
}

export default function Board(props: BoardProps) {
    enum CellState { Shot, ShotShip };

    var cellWidth = 25;
    var cellPadding = 3;

    var board: BoardState = props.board;

    let ships: JSX.Element[] = [];
    board.ships.forEach((ship: Ship) => {
        var x = (ship.origin.x * cellWidth) + cellPadding;
        var y = (ship.origin.y * cellWidth) + cellPadding;
        var shipWidth: number = 0;
        var shipHeight: number = 0;
        switch (ship.heading) {
            case Direction.North:
                y -= (ship.length - 1) * cellWidth;
            case Direction.South:
                shipWidth = cellWidth - 2 * cellPadding;
                shipHeight = (ship.length * cellWidth) - 2 * cellPadding;
                break;
            case Direction.West:
                x -= (ship.length - 1) * cellWidth;
            case Direction.East:
                shipHeight = cellWidth - 2 * cellPadding;
                shipWidth = (ship.length * cellWidth) - 2 * cellPadding;
                break;
        }
        ships.push(<rect key={`${ship.origin.x};${ship.origin.y}-ship`} x={x} y={y} width={shipWidth} height={shipHeight} className={`ship`}></rect>);
    });

    let items: CellState[][] = [];
    for (let i = 0; i < board.width; i++) {
        items[i] = [];
    }

    board.shots.forEach((shot: ShotModel) => {
        var value: CellState = shot.hit ? CellState.ShotShip : CellState.Shot;
        items[shot.coordinates.x][shot.coordinates.y] = value;
    });

    let cells = [];
    for (let y = 0; y < board.height; y++) {
        for (let x = 0; x < board.width; x++) {
            let state: string = '';
            switch (items[x][y]) {
                case CellState.Shot:
                    state = 'shot';
                    break;
                case CellState.ShotShip:
                    state = 'shotShip';
                    break;
                default:
                    break;
            }
            cells.push(<rect key={`${x};${y}`} x={x * cellWidth} y={y * cellWidth} height={cellWidth} width={cellWidth} className={`boardCell ${state}`} onClick={() => props.clickCallback(x, y)}></rect>);
        }
    }

    return (
        <svg className="board">
            {cells}
            {ships}
        </svg>
    );
} 
