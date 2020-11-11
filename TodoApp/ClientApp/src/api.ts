import axios from 'axios';
import { store } from './redux/redux';

export interface AuthUser {
    username: string;
    password: string;
}

export interface AuthResult {
    username: string;
    fullname: string;
}

export interface RegisterUser {
    username: string;
    fullname: string;
    password: string;
}

export interface CreateTable {
    name: string;
}

export interface Card {
    title: string;
    description: string;
}

export interface CardList {
    id: string;
    name: string;
    content: Card[];
}

export interface CreateList {
    name: string;
}

const errorHandler = (err: any) => {
    return {
        ok: false
    };
}

const successHandler = (res: any) => {
    return {
        ok: (res.status == 200)
    };
};

const dataHandler = (res: any) => {
    return {
        ok: true,
        data: res.data
    };
}

const apiGet = (url: string): any => {
    return axios.get(process.env.REACT_APP_BACKEND + url, { withCredentials: true });
}

const apiPost = (url: string, postData: any): any => {
    return axios.post(process.env.REACT_APP_BACKEND + url, postData, { withCredentials: true });
}

export const apiLogin = (user: AuthUser): Promise<AuthResult> => {
    return apiPost("/user/login", user).then(dataHandler).catch(errorHandler);
}

export const apiRegister = (user: RegisterUser): Promise<any> => {
    return apiPost("/user/register", user).then(dataHandler).catch(errorHandler);
}

export const apiLogout = (): Promise<any> => {
    return apiGet("/user/logout").then(successHandler).catch(errorHandler);
}

export const apiGetTables = (): Promise<any> => {
    return apiGet("/table/all").then(dataHandler).catch(errorHandler);
}

export const apiCreateTable = (data: CreateTable): Promise<any> => {
    return apiPost("/table/create", data).then(successHandler).catch(errorHandler);
}

export const apiDeleteTable = (id: string): Promise<any> => {
    return apiGet(`/table/${id}/delete`).then(successHandler).catch(errorHandler);
}

export const apiGetTableContent = (id: string): Promise<any> => {
    return apiGet(`/table/${id}/all`).then(dataHandler).catch(errorHandler);
}

export const apiCreateList = (tableId: string, data: CreateList): Promise<any> => {
    return apiPost(`/table/${tableId}/createList`, data).then(dataHandler).catch(errorHandler);
}

export const apiCreateCard = (tableId: string, listId: string, card: Card): Promise<any> => {
    return apiPost(`/table/${tableId}/${listId}/create`, card).then(dataHandler).catch(errorHandler);
}