import React from 'react';
import { connect } from "react-redux";

import styled, { ThemeConsumer } from "styled-components";
import { TextField, Paper, Typography, Button, CircularProgress, Fab, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, IconButton } from '@material-ui/core';
import { apiCreateList, apiGetTableContent, Card, CardList, CreateList, apiCreateCard } from '../api';
import AddIcon from '@material-ui/icons/Add';
import { history } from '../App';
import { withRouter } from 'react-router';
import PromptTextDialog from "./PromptTextDialog";
import CardEditDialog from './CardEditDialog';
import CardListComponent from './CardListComponent';

const mapStateToProps = (store: any) => {
    return {
        isLoggedIn: store.user.loggedIn
    };
};

const mapDispatchToProps = (dispatch: any) => {
    return {
    };
};

const FullPageCenter = styled.div`{
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
}`;

const ListsWrapper = styled.div`{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: row;
    background-color: ##007FDB;
}`;

const ListEntryWrapper = styled.div`{
    flex: 0 0 300px;
    padding: 0.5rem;
}`;

const FabPlace = styled.div`{
    position: absolute;
    right: 0;
    bottom: 0;
    margin: 1rem;
}`;

const HorizontalLine = styled.div`{
    width: 100%;
    height: 1px;
    background-color: #dddddd;
}`;

interface State {
    loading: boolean;
    lists: CardList[];
}

type ActualProps = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & any;
class TableRoute extends React.Component<ActualProps, State> {
    createListDialogRef: any;

    editCardDialogRef: any;
    editSelectedList: string = "";
    editCardIndex?: number;
    editCardResolve?: Function;

    constructor(props: any) {
        super(props);

        this.state = {
            loading: true,
            lists: []
        };
    }

    getTableId() {
        return (this.props as any).location.state.tableId;
    }

    componentWillMount() {
        if (!this.props.isLoggedIn) {
            history.push("/");
            return;
        }

        const tableId = this.getTableId();
        if (tableId === undefined) {
            history.push("/");
            return;
        }

        apiGetTableContent(tableId).then((result: any) => {
            if (!result) {
                history.push("/");
                return;
            }

            const lists: CardList[] = result.data;

            this.setState({
                loading: false,
                lists: lists
            });
        })
    }

    handleFab() {
        this.createListDialogRef.doOpen();
    }

    onCreateList(name: string) {
        const newList: CreateList = {
            name: name
        };
        apiCreateList(this.getTableId(), newList).then(result => {
            if (!result.ok) {
                history.push("/");
                return;
            }

            this.setState({
                lists: [ ...this.state.lists, result.data ]
            });
        });
    }

    handleCreateCard(listId: string) {
        this.editCardIndex = undefined;
        this.editSelectedList = listId;
        this.editCardDialogRef.doOpen(null);
    }

    onCardEditDone(card: Card) {
        if (this.editCardIndex == undefined) {
            apiCreateCard(this.getTableId(), this.editSelectedList, card).then(result => {
                if (result.ok) {
                    if (this.editCardResolve != undefined) {
                        this.editCardResolve(result.data.content);
                    }
                } else {
                    history.push("/");
                }
            })
        }
    }

    render() {
        if (this.state.loading) {
            return <FullPageCenter>
                <CircularProgress />
            </FullPageCenter>;
        }

        return <>
            <ListsWrapper>
                {
                    this.state.lists.map((list: CardList, index: number) => {
                        return <CardListComponent key={index} list={list} onCreateCard={((listId: string) => {
                            return new Promise((resolve, reject) => {
                                this.handleCreateCard(listId);
                                this.editCardResolve = resolve;
                            });
                        }).bind(this)} />;
                    })
                }
            </ListsWrapper>
            <PromptTextDialog title={"Új tábla"} text={"Adja meg az új tábla nevét:"} done={this.onCreateList.bind(this)} ref={r => this.createListDialogRef = r} />
            <CardEditDialog done={this.onCardEditDone.bind(this)} ref={r => this.editCardDialogRef = r} />
            <FabPlace>
                <Fab color="primary" onClick={this.handleFab.bind(this)}>
                    <AddIcon />
                </Fab>
            </FabPlace>
        </>;
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TableRoute));