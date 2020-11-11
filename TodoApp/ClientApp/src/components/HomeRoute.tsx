import React from 'react';
import { connect } from "react-redux";
import styled from "styled-components";
import LoginCard from './LoginCard';
import TableList from './TableList';

const mapStateToProps = (store: any) => {
    return {
        isLoggedIn: store.user.loggedIn
    };
};

const mapDispatchToProps = (dispatch: any) => {
    return { };
};

const PageWrapper = styled.div`{
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
}`;

type ActualProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>;
class HomeRoute extends React.Component<ActualProps> {
    constructor(props: any) {
        super(props);
    }
    render() {
        if (!this.props.isLoggedIn) {
            return <PageWrapper>
                <LoginCard />
            </PageWrapper>;
        }

        return <TableList />;
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(HomeRoute);