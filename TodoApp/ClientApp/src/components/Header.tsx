import React from 'react';

import styled from "styled-components";
import { connect } from "react-redux";
import { onUserLogout } from '../redux/user';

import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import LogOutIcon from '@material-ui/icons/SubdirectoryArrowRight';
import { withRouter } from 'react-router';

import { history } from '../App';
import { apiLogout } from '../api';
import { IconButton } from '@material-ui/core';
import { ArrowBack } from '@material-ui/icons';

const mapStateToProps = (store: any) => {
    return {
        isLoggedIn: store.user.loggedIn,
        name: store.user.fullname
    };
};

const mapDispatchToProps = (dispatch: any) => {
    return {
        logout: () => {
            dispatch(onUserLogout());
        }
    };
};

const ActionsContainer = styled.div`{
    display: flex;
    align-items: center;
}`;

const LogoutButton = styled.div`{
    cursor: pointer;
    margin-left: 0.5rem;
}`;

const LogoText = styled(Typography)`{
    cursor: pointer;
}`;

const LoginWrapper = styled.div`{
    flex: 0 0 auto;
}`;

const BackButton = styled(IconButton)`{
    margin-right: 1rem;
}`;

type ActualProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & any;
class AppHeader extends React.Component<ActualProps> {
    constructor(props: any) {
        super(props);
    }

    handleLogout() {
        apiLogout().then((() => {
            this.props.logout();
            history.push("/");
        }).bind(this));
    }

    render() {
        return (
            <LoginWrapper>
                <AppBar position="static">
                    <Toolbar>
                        {
                            (this.props as any).location.pathname.length > 1 ?
                            <BackButton edge="start" color="inherit" onClick={(() => {
                                history.goBack();
                            }).bind(this)}>
                                <ArrowBack />
                            </BackButton>
                            : null
                        }
                        <LogoText variant="h6" style={{ flexGrow: 1 }} onClick={() => {
                            history.push("/");
                        }}>Todo App</LogoText>
                        {
                            this.props.isLoggedIn ? (
                                <ActionsContainer>
                                    <Typography variant="h6">{this.props.name}</Typography>
                                    <LogoutButton onClick={this.handleLogout.bind(this)}>
                                        <LogOutIcon />
                                    </LogoutButton>
                                </ActionsContainer>
                            ) : null
                        }
                    </Toolbar>
                </AppBar>
            </LoginWrapper>
        );
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(AppHeader));