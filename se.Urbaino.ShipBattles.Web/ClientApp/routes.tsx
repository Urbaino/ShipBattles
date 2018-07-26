import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Game } from './components/Game';
import { Lobby } from './components/Lobby';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/lobby' component={Lobby} />
    <Route path='/game' component={Game} />
    <Route path='/counter' component={Counter} />
    <Route path='/fetchdata' component={FetchData} />
</Layout>;
