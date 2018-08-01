import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import Board from '../types/domain/Board';

interface GameState {
    Board: Board,
    EnemyBoard: Board
    Opponent: string;
}

export class Game extends React.Component<RouteComponentProps<{}>, GameState> {
    constructor() {
        super();
        // this.state = { Board: {}, EnemyBoard: {}, Opponent: '' };
    }

    public render() {
        return <div>
            <h1>Game on!</h1>

            <p>This is a simple example of a React component.</p>

            <p>Your board: <strong>{this.state.Board}</strong></p>
            <p>Enemy board: <strong>{this.state.EnemyBoard}</strong></p>

        </div>;
    }

}
