import React from 'react';

import styled from "styled-components";
import { Paper, Typography, IconButton, CardContent } from '@material-ui/core';
import { CardList, Card } from '../api';
import AddIcon from '@material-ui/icons/Add';
import MaterialCard from '@material-ui/core/Card';
import { Delete, Edit } from '@material-ui/icons';

const ListEntryWrapper = styled.div`{
    flex: 0 0 300px;
    padding: 0.5rem;
}`;

const HorizontalLine = styled.div`{
    width: 100%;
    height: 1px;
    background-color: #dddddd;
    margin-top: 0.25rem;
    margin-bottom: 0.25rem;
}`;

const CardWrapper = styled.div`{
    width: 100%;
    padding-top: 0.25rem;
    padding-bottom: 0.25rem;
    position: relative;
    cursor: grab;
}`;

const ActionsWrapper = styled.div`{
    position: absolute;
    right: 0;
    bottom: 0;
}`;

const ListActionsWrapper = styled.div`{
    position: relative;
    height: 48px;
    display: flex;
}`;

const ListActions = styled.div`{
    position: absolute;
    right: 0;
    top: 0;
}`;

interface Props {
    list: CardList;

    onCreateCard: Function;
    onDeleteCard: Function;
    onEditCard: Function;

    onDeleteList: Function;

    onDragStart: Function;
    onDragDrop: Function;
}

interface State {
    list: CardList;
    mouseOverCard: number;
    mouseOverList: boolean;
}

export default class CardListComponent extends React.Component<Props, State> {
    constructor(props: any) {
        super(props);

        this.state = {
            list: props.list,
            mouseOverCard: -1,
            mouseOverList: false
        };
    }

    handleNewContent(newContent: Card[]) {
        const list = this.state.list;
        list.content = newContent;
        this.setState({
            list: list
        });
    }

    handleCreateCard() {
        this.props.onCreateCard(this.state.list.id).then(this.handleNewContent.bind(this));
    }

    handleDeleteCard(index: number) {
        this.props.onDeleteCard(this.state.list.id, index).then(this.handleNewContent.bind(this));
    }

    handleEditCard(index: number, card: Card) {
        this.props.onEditCard(this.state.list.id, index, card).then(this.handleNewContent.bind(this));
    }

    handleDeleteList() {
        this.props.onDeleteList(this.state.list.id);
    }

    onDragStart(cardIndex: number) {
        this.props.onDragStart(this.state.list.id, cardIndex);
    }

    onDragOver(e: any) {
        e.preventDefault();
    }

    onDragDropList(e: any) {
        e.preventDefault();
        e.stopPropagation();
        this.props.onDragDrop(this.state.list.id, 0);
    }

    onDragDropCard(index: number) {
        this.props.onDragDrop(this.state.list.id, index);
    }

    render() {
        return <ListEntryWrapper onDragOver={this.onDragOver.bind(this)} onDrop={this.onDragDropList.bind(this)}>
            <Paper elevation={3} style={{ padding: "0.5rem" }}
                onMouseEnter={(() => {
                    this.setState({
                        mouseOverList: true
                    });
                }).bind(this)}
                onMouseLeave={(() => {
                    this.setState({
                        mouseOverList: false
                    });
                }).bind(this)}>
                <ListActionsWrapper>
                    <Typography style={{display: "flex", alignItems: "center"}} variant="h5">{this.state.list.name}</Typography>
                    <ListActions style={{ opacity: (this.state.mouseOverList && this.state.mouseOverCard == -1 ? 1 : 0) }}>
                        <IconButton onClick={this.handleDeleteList.bind(this)}>
                            <Delete />
                        </IconButton>
                    </ListActions>
                </ListActionsWrapper>
                <HorizontalLine />
                {
                    this.state.list.content.map((card: Card, index: number) => {
                        return <CardWrapper draggable key={index + card.title} 
                            onDragOver={this.onDragOver.bind(this)}
                            onDrop={((e: any) => {
                                e.preventDefault();
                                e.stopPropagation();
                                this.onDragDropCard(index);
                            }).bind(this)}
                            onDragStart={((e: any) => {
                                this.onDragStart(index);
                            }).bind(this)}
                            onMouseEnter={(() => {
                                this.setState({
                                    mouseOverCard: index
                                });
                            }).bind(this)}
                            onMouseLeave={(() => {
                                this.setState({
                                    mouseOverCard: -1
                                });
                            }).bind(this)}>
                            <MaterialCard>
                                <CardContent>
                                    <Typography variant="h5" component="h2">{card.title}</Typography>
                                    <Typography color="textSecondary">{card.description}</Typography>
                                </CardContent>
                                <ActionsWrapper style={{ opacity: (this.state.mouseOverCard == index ? 1 : 0) }}>
                                    <IconButton onClick={(() => { this.handleEditCard(index, card); }).bind(this)}>
                                        <Edit />
                                    </IconButton>
                                    <IconButton onClick={(() => { this.handleDeleteCard(index); }).bind(this)}>
                                        <Delete />
                                    </IconButton>
                                </ActionsWrapper>
                            </MaterialCard>
                        </CardWrapper>;
                    })
                }
                <HorizontalLine />
                <Paper elevation={3} style={{ cursor: "pointer" }} onClick={this.handleCreateCard.bind(this)}>
                    <IconButton>
                        <AddIcon />
                        <Typography variant="body1">Új kártya...</Typography>
                    </IconButton>
                </Paper>
            </Paper>
        </ListEntryWrapper>;
    }
}
