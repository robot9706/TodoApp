import * as React from 'react';
import { Router, Switch, Redirect, Route } from "react-router-dom";
import { Provider } from 'react-redux'
import { store } from './redux/redux';
import { createBrowserHistory } from 'history';
import styled from "styled-components";

export const history = createBrowserHistory();

const NoMatch = () => <Redirect to="/" />

const PageWrapper = styled.div`{
    display: flex;
    flex-flow: column;
    height: 100%;
}`;

const PageHeader = styled.div`{
    flex: 0 0 64px;
}`;

const Page = styled.div`{
    flex: 1 1 auto;
    overflow-y: scroll;
}`;

export default class App extends React.Component {
    render() {
        return <Provider store={store}>
            <Router history={history}>
                <PageWrapper>
                    <PageHeader>

                    </PageHeader>
                    <Page>
                        <Switch>
                            <Route exact component={NoMatch} />
                        </Switch>
                    </Page>
                </PageWrapper>
            </Router>
        </Provider>;
    }
}
