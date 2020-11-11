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
}`;

const ActionsWrapper = styled.div`{
    position: absolute;
    right: 0;
    bottom: 0;
}`;

interface Props {
    list: CardList;

    onCreateCard: Function;
    onDeleteCard: Function;
    onEditCard: Function;
}

interface State {
    list: CardList;
    mouseOver: number;
}

export default class CardListComponent extends React.Component<Props, State> {
    constructor(props: any) {
        super(props);

        this.state = {
            list: props.list,
            mouseOver: -1
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

    render() {
        return <ListEntryWrapper>
            <Paper elevation={3} style={{ padding: "0.5rem" }}>
                <Typography variant="h5">{this.state.list.name}</Typography>
                <HorizontalLine />
                {
                    this.state.list.content.map((card: Card, index: number) => {
                        return <CardWrapper key={index + card.title}
                            onMouseEnter={(() => {
                                this.setState({
                                    mouseOver: index
                                });
                            }).bind(this)}
                            onMouseLeave={(() => {
                                this.setState({
                                    mouseOver: -1
                                });
                            }).bind(this)}>
                            <MaterialCard>
                                <CardContent>
                                    <Typography variant="h5" component="h2">{card.title}</Typography>
                                    <Typography color="textSecondary">{card.description}</Typography>
                                </CardContent>
                                <ActionsWrapper style={{ opacity: (this.state.mouseOver == index ? 1 : 0) }}>
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
