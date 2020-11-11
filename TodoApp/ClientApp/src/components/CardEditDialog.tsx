import React from 'react';

import { TextField, Button, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions } from '@material-ui/core';
import { Card } from '../api';

interface DialogProps {
    done: Function;
    card: Card;
}

interface DialogState {
    open: boolean;
    isNew: boolean;
    title: string;
    desc: string;
}

export default class CardEditDialog extends React.Component<DialogProps & any, DialogState> {
    constructor(props: any) {
        super(props);

        this.state = {
            open: false,
            isNew: true,
            title: "",
            desc: ""
        };
    }

    doOpen(card: Card) {
        this.setState({
            open: true,
            isNew: card == null,
            title: card ? card.title : "",
            desc: card ? card.description : ""
        });
    }

    handleClose() {
        this.setState({
            open: false,
            title: "",
            desc: ""
        });
    }

    handleDone() {
        const newCard: Card = {
            title: this.state.title,
            description: this.state.desc
        };

        this.props.done(newCard);

        this.setState({
            open: false,
            title: "",
            desc: ""
        });
    }

    render() {
        return <Dialog onClose={this.handleClose.bind(this)} open={this.state.open} maxWidth={"md"}>
            <DialogTitle>{ this.state.isNew ? "Új kártya létrehozása" : "Kártya szerkesztése" }</DialogTitle>
            <DialogContent>
                <DialogContentText>Kártya neve:</DialogContentText>
                <TextField autoFocus margin="dense" id="tname" type="text" fullWidth label="Név" value={this.state.title} onChange={(e) => this.setState({ title: e.target.value })} />

                <DialogContentText>Kártya leírása:</DialogContentText>
                <TextField variant="outlined" multiline rows={5} margin="dense" id="tdesc" type="text" fullWidth label="Név" value={this.state.desc} onChange={(e) => this.setState({ desc: e.target.value })} />
            </DialogContent>
            <DialogActions>
                <Button onClick={this.handleClose.bind(this)} color="primary">Mégse</Button>
                <Button onClick={this.handleDone.bind(this)} color="primary">{ this.state.isNew ? "Új" : "Mentés" }</Button>
            </DialogActions>
        </Dialog>;
    }
}