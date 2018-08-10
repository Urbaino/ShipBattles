import Ship from "./Ship";
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
    board.shots.forEach(ship => { // TODO: Hantera dictionary istället
        items[ship.Coordinates.X][ship.Coordinates.Y] = CellState.Ship;
    });

    board.shots.forEach(shot => { // TODO: Hantera dictionary istället
        var cell = items[shot.Coordinates.X][shot.Coordinates.Y];
        var value: CellState;
        if (cell === undefined) {
            value = CellState.Shot;
        }
        else {
            value = CellState.ShotShip;
        }
        items[shot.Coordinates.X][shot.Coordinates.Y] = value;
    });


    let rows = [];
    for (let i = 0; i < board.height; i++) {
        let cols = [];
        for (let j = 0; j < board.width; j++) {
            let icon: string = '';
            switch (items[i][j]) {
                case CellState.Ship:
                    icon = 'S';
                    break;
                case CellState.Shot:
                    icon = 'x';
                    break;
                case CellState.ShotShip:
                    icon = 'X';
                    break;
                default:
                    break;
            }
            cols.push(<td>{icon}</td>);
        }
        rows.push(<tr>{cols}</tr>);
    }

    return (
        <table>
            <tbody>
                {rows}
            </tbody>
        </table>
    );
} 
