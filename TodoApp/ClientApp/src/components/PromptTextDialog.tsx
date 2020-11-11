import React from 'react';

import { TextField, Button, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions } from '@material-ui/core';

interface DialogProps {
    done: Function;
    text: string;
    title: string;
}

interface DialogState {
    open: boolean;
    name: string;
}

export default class PromptTextDialog extends React.Component<DialogProps, DialogState> {
    constructor(props: any) {
        super(props);

        this.state = {
            open: false,
            name: ""
        };
    }

    doOpen() {
        this.setState({
            open: true,
            name: ""
        });
    }

    handleClose() {
        this.setState({
            open: false,
            name: ""
        });
    }

    handleDone() {
        this.props.done(this.state.name);

        this.setState({
            open: false,
            name: ""
        });
    }

    render() {
        return <Dialog onClose={this.handleClose.bind(this)} open={this.state.open} maxWidth={"md"}>
            <DialogTitle>{this.props.title}</DialogTitle>
            <DialogContent>
                <DialogContentText>{this.props.text}</DialogContentText>
                <TextField autoFocus margin="dense" id="tname" type="text" fullWidth label="Név" value={this.state.name} onChange={(e) => this.setState({ name: e.target.value })} />
            </DialogContent>
            <DialogActions>
                <Button onClick={this.handleClose.bind(this)} color="primary">Mégse</Button>
                <Button onClick={this.handleDone.bind(this)} color="primary" disabled={this.state.name.length == 0}>Létrehozás</Button>
            </DialogActions>
        </Dialog>;
    }
}