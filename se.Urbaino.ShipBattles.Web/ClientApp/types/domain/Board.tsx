import Ship, { Direction } from "./Ship";
import Shot from "./Shot";
import * as React from "react";
import { RouteComponentProps } from "react-router";

export interface BoardState {
    ships: Array<Ship>,
    shots: Array<Shot>,
    width: number,
    height: number
}

export default function Board(props: any) {
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

    board.shots.forEach((shot: Shot) => { // TODO: Dictionary would be nice
        var cell = items[shot.coordinates.x][shot.coordinates.y];
        var value: CellState;
        if (cell === undefined) {
            value = CellState.Shot;
        }
        else {
            value = CellState.ShotShip;
        }
        items[shot.coordinates.x][shot.coordinates.y] = value;
    });


    let rows = [];
    for (let y = 0; y < board.height; y++) {
        let cols = [];
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
            cols.push(<td key={x} className={`boardColumn ${state}`} onClick={() => props.clickCallback(x, y)}></td>);
        }
        rows.push(<tr key={y} className="boardRow">{cols}</tr>);
    }

    return (
        <table className="boardTable">
            <tbody>
                {rows}
            </tbody>
        </table>
    );
} 
