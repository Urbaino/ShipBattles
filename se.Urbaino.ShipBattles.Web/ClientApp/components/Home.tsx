import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { HubConnection, HubConnectionBuilder, HttpTransportType, LogLevel } from '@aspnet/signalr';

interface HomeState {
    Nick: string
    Message: string
}

export class Home extends React.Component<RouteComponentProps<{}>, HomeState> {
    constructor() {
        super();
        this.state = {
            Nick: '',
            Message: ''
        };
    }

    signIn = () => {
        window.location.href = `login/${this.state.Nick}`;
    };

    setName = (nick: string) => {
        this.setState(() => {
            return {Nick: nick};
        });
    }

    public render() {
        return <div>
            <h1>Hello!</h1>
            <p>Welcome to Ship Battles! Please enter your username:</p>
            <input type='text' onChange={(e) => this.setName(e.target.value)} />
            <button onClick={this.signIn}>Send</button>
            <span>{this.state.Message}</span>
        </div>;
    }
}
