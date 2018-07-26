import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface GameState {
    players: Array<string>;
}

export class Game extends React.Component<RouteComponentProps<{}>, GameState> {
    constructor() {
        super();
        this.state = { players: [] };
    }

    public render() {
        return <div>
            <h1>Game on!</h1>

            <p>This is a simple example of a React component.</p>

            <p>Current player count: <strong>{ this.state.players.length }</strong></p>

        </div>;
    }

}
