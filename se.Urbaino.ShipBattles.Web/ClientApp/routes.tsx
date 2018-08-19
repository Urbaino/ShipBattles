import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Lobby } from './components/Lobby';
import { Play } from './components/Play';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/lobby' component={Lobby} />
    <Route path='/play' component={Play} />
</Layout>;
