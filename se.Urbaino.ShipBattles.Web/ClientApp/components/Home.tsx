import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { HubConnection, HubConnectionBuilder, HttpTransportType, LogLevel } from '@aspnet/signalr';

interface HomeState {
    Hub: HubConnection,
    Nick: string,
    Message: string
}

export class Home extends React.Component<RouteComponentProps<{}>, HomeState> {
    constructor() {
        super();
        this.state = {
            Hub: new HubConnectionBuilder().withUrl('/hub', {logger: LogLevel.Debug}).build(),
            Nick: '',
            Message: ''
        };
    }
    componentDidMount() {
        this.state.Hub.on('RecieveMessage', (receivedMessage) => {
            const text = `You are now known as: ${receivedMessage}`;
            this.setState({ Message: text });
        });

        this.state.Hub
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));
    }

    sendMessage = () => {
        this.state.Hub
            .invoke('SendMessage', this.state.Nick)
            .catch(err => console.error(err));
    };

    public render() {
        return <div>
            <h1>Hello!</h1>
            <p>Welcome to Ship Battles! Please enter your username:</p>
            <input type='text' onChange={(e) => this.setState({ Nick: e.target.value })} />
            <button onClick={() => this.state.Hub.invoke('SendMessage', this.state.Nick)}>Send</button>
            <span>{this.state.Message}</span>
        </div>;
    }
}
