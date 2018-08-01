import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface HistoryState {
    games: Array<string>;
}

export class History extends React.Component<RouteComponentProps<{}>, HistoryState> {
    constructor() {
        super();
        this.state = { games: [] };
    }

    public render() {
        return <div>
            <h1>Game history</h1>

            <p>These are the games you've played</p>

            <p>Current count: <strong>{ this.state.games.length }</strong></p>

        </div>;
    }

}
