import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface LobbyState {
    players: Array<string>;
}

export class Lobby extends React.Component<RouteComponentProps<{}>, LobbyState> {
    constructor() {
        super();
        this.state = { players: [] };
    }

    public render() {
        return <div>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p>Current count: <strong>{ this.state.players.length }</strong></p>

        </div>;
    }

}
