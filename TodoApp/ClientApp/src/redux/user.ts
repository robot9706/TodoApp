const ACTION_LOGIN = "ACTION_LOGIN";
const ACTION_LOGOUT = "ACTION_LOGOUT";

export const onUserLogin = (name: string, fullname: string) => ({
    type: ACTION_LOGIN,
    data: {
        name: name,
        fullname: fullname
    }
});

export const onUserLogout = () => ({
    type: ACTION_LOGOUT,
});

const initialUserState = {
    loggedIn: false,
    name: null,
    fullname: null
};

export const userReducer = (state = initialUserState, action: any) => {
    switch (action.type) {
        case ACTION_LOGIN:
            return {
                loggedIn: true,
                name: action.data.name,
                fullname: action.data.fullname
            };
        case ACTION_LOGOUT:
            return {
                loggedIn: false,
                name: null,
                fullname: null
            };
        default:
            return state
    }
};