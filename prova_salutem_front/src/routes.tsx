import React from 'react';
import { Switch, Route, BrowserRouter } from 'react-router-dom';

// Pages
import Customer from './pages/Customer';
import Dashboard from './pages/Dashboard';
import Seller from './pages/Seller';

const Routes: React.FC = () => {
  return (
    <BrowserRouter>
      <Switch>
        <Route exact path="/" component={Dashboard} />
        <Route path="/customer" component={Customer} />
        <Route path="/seller" component={Seller} />
      </Switch>
    </BrowserRouter>
  )
}

export default Routes;