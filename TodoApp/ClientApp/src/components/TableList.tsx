import React from 'react';

import styled from "styled-components";
import { Paper, Typography, Button, CircularProgress, Fab, Card, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, IconButton } from '@material-ui/core';
import { apiGetTables, CreateTable, apiCreateTable, apiDeleteTable } from '../api';
import AddIcon from '@material-ui/icons/Add';
import DeleteIcon from '@material-ui/icons/Delete';
import { history } from '../App';
import PromptTextDialog from "./PromptTextDialog";

const FullPage = styled.div`{
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
}`;

const FabPlace = styled.div`{
    position: absolute;
    right: 0;
    bottom: 0;
    margin: 1rem;
}`;

const TableListWrapper = styled.div`{
    min-width: 800px;
    max-width: 1000px;
    margin: 1rem;
}`;

const TableListRow = styled.div`{
    width: 100%;
}`;

const TableElement = styled.div`{
    height: 58px;
    display: inline-block;
    width: calc(50% - 10px);
    margin: 5px;
    cursor: pointer;
}`;

const TablePaperContent = styled.div`{
    padding: 5px;
    padding-left: 15px;
    display: flex;
}`;

interface TableRef {
    id: string;
    name: string;
}

interface State {
    loading: boolean;
    tables: TableRef[];
}

export default class TableList extends React.Component<{}, State> {
    createTableDialogRef: any;

    constructor(props: any) {
        super(props);

        this.state = {
            loading: true,
            tables: []
        };
    }

    componentWillMount() {
        apiGetTables().then(result => {
            this.setState({
                loading: false,
                tables: result.data
            });
        });
    }

    handleFab() {
        this.createTableDialogRef.doOpen();
    }

    onCreateTable(name: string) {
        const data: CreateTable = {
            name: name
        };
        apiCreateTable(data).then(result => {
            this.componentWillMount();
        });
    }

    renderTableCard(table: TableRef) {
        if (table == null) {
            return null;
        }

        return <TableElement>
            <Paper style={{ height: "100%" }} elevation={3} onClick={(() => {
                history.push("/table", {
                    tableId: table.id
                });
            }).bind(this)}>
                <TablePaperContent>
                    <Typography variant="body1" style={{flex: "1 1 auto", display: "flex", alignItems: "center"}}>{table.name}</Typography>
                    <IconButton onClick={(() => {
                        apiDeleteTable(table.id).then(result =>{
                            if (result.ok) {
                                this.componentWillMount();
                            }
                        });
                    }).bind(this)}><DeleteIcon /></IconButton>
                </TablePaperContent>
            </Paper>
        </TableElement>;
    }

    renderList() {
        const rowData = [];
        for (let i = 0; i < this.state.tables.length; i += 2) {
            if (i + 1 < this.state.tables.length) {
                rowData.push([this.state.tables[i], this.state.tables[i + 1]]);
            } else {
                rowData.push([this.state.tables[i]]);
            }
        }

        return <>
            {
                rowData.map((row: TableRef[], index: number) => {
                    return <TableListRow key={index}>
                        {
                            row.length > 0 ? this.renderTableCard(row[0]) : null
                        }
                        {
                            row.length > 1 ? this.renderTableCard(row[1]) : null
                        }
                    </TableListRow>;
                })
            }
        </>;
    }

    render() {
        return <FullPage>
            <FabPlace>
                <Fab color="primary" onClick={this.handleFab.bind(this)}>
                    <AddIcon />
                </Fab>
            </FabPlace>
            <PromptTextDialog title={"Új tábla"} text={"Adja meg az új tábla nevét:"} done={this.onCreateTable.bind(this)} ref={r => this.createTableDialogRef = r} />
            <TableListWrapper>
                <Paper style={{ padding: "0.5rem" }} elevation={3}>
                    <Typography variant="h5">Táblák</Typography>
                    {
                        this.state.loading ?
                            <CircularProgress /> :
                            this.renderList()
                    }
                </Paper>
            </TableListWrapper>
        </FullPage>;
    }
};
