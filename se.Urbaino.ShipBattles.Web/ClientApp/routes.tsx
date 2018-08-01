import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { History } from './components/History';
import { Lobby } from './components/Lobby';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/lobby' component={Lobby} />
    <Route path='/history' component={History} />
</Layout>;
