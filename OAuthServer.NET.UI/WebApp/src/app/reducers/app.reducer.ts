import * as AppActions from './app.actions';

export function newState(state: any, newData: any) {
    return Object.assign([], state, newData);
}

export function clientsReducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_CLIENTS:
            return newState(state, action.clients);
        case AppActions.CREATE_CLIENT:
            let addClientNewState = state.concat((<AppActions.CreateClient>action).client);
            addClientNewState = addClientNewState.sort((a, b) => {
                if (a.clientName > b.clientName) {
                    return 1;
                } else {
                    return -1;
                }
            });
            return newState(state, addClientNewState);
        case AppActions.UPDATE_CLIENT:
            const updateClientNewState = (<AppActions.UpdateClient>action).client;
            const clientToModify = state.find(x => x.id === updateClientNewState.id);
            const indexOfUpdate = state.indexOf(clientToModify);
            return [
                ...state.slice(0, indexOfUpdate),
                updateClientNewState,
                ...state.slice(indexOfUpdate + 1, state.length)
            ];
        case AppActions.REMOVE_CLIENT:
            const idOfDelete = (<AppActions.RemoveClient>action).id;
            const clientToDelete = state.find(x => x.id === idOfDelete);
            const indexOfDelete = state.indexOf(clientToDelete);
            return [
                ...state.slice(0, indexOfDelete),
                ...state.slice(indexOfDelete + 1, state.length)
            ]
        case AppActions.CLEAR_CLIENTS:
            return [];
        default:
            return state;
    }
}

export function clientsPostLogoutRedirectURIsreducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_CLIENTPOSTLOGOUTREDIRECTURIS:
            return newState(state, action.clientPostLogoutRedirectURIs);
        case AppActions.CREATE_CLIENTPOSTLOGOUTREDIRECTURI:
            let addClientPostLogoutRedirectURINewState = state.concat((<AppActions.CreateClientPostLogoutRedirectURI>action).clientPostLogoutRedirectURI);
            addClientPostLogoutRedirectURINewState = addClientPostLogoutRedirectURINewState.sort((a, b) => {
                if (a.postLogoutRedirectURI > b.postLogoutRedirectURI) {
                    return 1;
                } else {
                    return -1;
                }
            });
            return newState(state, addClientPostLogoutRedirectURINewState);
        case AppActions.UPDATE_CLIENTPOSTLOGOUTREDIRECTURI:
            const updateClientPostLogoutRedirectURINewState = (<AppActions.UpdateClientPostLogoutRedirectURI>action).clientPostLogoutRedirectURI;
            const postLogoutRedirectURIToModify = state.find(x => x.id === updateClientPostLogoutRedirectURINewState.id);
            const indexOfUpdate = state.indexOf(postLogoutRedirectURIToModify);
            return [
                ...state.slice(0, indexOfUpdate),
                updateClientPostLogoutRedirectURINewState,
                ...state.slice(indexOfUpdate + 1, state.length)
            ];
        case AppActions.REMOVE_CLIENTPOSTLOGOUTREDIRECTURI:
            const idOfDelete = (<AppActions.RemoveClientPostLogoutRedirectURI>action).id;
            const postLogoutRedirectURIToDelete = state.find(x => x.id === idOfDelete);
            const indexOfDelete = state.indexOf(postLogoutRedirectURIToDelete);
            return [
                ...state.slice(0, indexOfDelete),
                ...state.slice(indexOfDelete + 1, state.length)
            ]
        case AppActions.CLEAR_CLIENTPOSTLOGOUTREDIRECTURIS:
            return [];
        default:
            return state;
    }
}

export function clientsRedirectURIsreducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_CLIENTREDIRECTURIS:
            return newState(state, action.clientRedirectURIs);
        case AppActions.CREATE_CLIENTREDIRECTURI:
            let addClientRedirectURINewState = state.concat((<AppActions.CreateClientRedirectURI>action).clientRedirectURI);
            addClientRedirectURINewState = addClientRedirectURINewState.sort((a, b) => {
                if (a.redirectURI > b.redirectURI) {
                    return 1;
                } else {
                    return -1;
                }
            });
            return newState(state, addClientRedirectURINewState);
        case AppActions.UPDATE_CLIENTREDIRECTURI:
            const updateClientRedirectURINewState = (<AppActions.UpdateClientRedirectURI>action).clientRedirectURI;
            const redirectURIToModify = state.find(x => x.id === updateClientRedirectURINewState.id);
            const indexOfUpdate = state.indexOf(redirectURIToModify);
            return [
                ...state.slice(0, indexOfUpdate),
                updateClientRedirectURINewState,
                ...state.slice(indexOfUpdate + 1, state.length)
            ];
        case AppActions.REMOVE_CLIENTREDIRECTURI:
            const idOfDelete = (<AppActions.RemoveClientRedirectURI>action).id;
            const redirectURIToDelete = state.find(x => x.id === idOfDelete);
            const indexOfDelete = state.indexOf(redirectURIToDelete);
            return [
                ...state.slice(0, indexOfDelete),
                ...state.slice(indexOfDelete + 1, state.length)
            ]
        case AppActions.CLEAR_CLIENTREDIRECTURIS:
            return [];
        default:
            return state;
    }
}

export function clientsCORSOriginsreducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_CLIENTCORSORIGINS:
            return newState(state, action.clientCORSOrigins);
        case AppActions.CREATE_CLIENTCORSORIGIN:
                let addClientCORSOriginNewState = state.concat((<AppActions.CreateClientCORSOrigin>action).clientCORSOrigin);
                addClientCORSOriginNewState = addClientCORSOriginNewState.sort((a, b) => {
                    if (a.originURI > b.originURI) {
                        return 1;
                    } else {
                        return -1;
                    }
                });
                return newState(state, addClientCORSOriginNewState);
            case AppActions.UPDATE_CLIENTCORSORIGIN:
                const updateClientCORSOriginNewState = (<AppActions.UpdateClientCORSOrigin>action).clientCORSOrigin;
                const corsOriginToModify = state.find(x => x.id === updateClientCORSOriginNewState.id);
                const indexOfUpdate = state.indexOf(corsOriginToModify);
                return [
                    ...state.slice(0, indexOfUpdate),
                    updateClientCORSOriginNewState,
                    ...state.slice(indexOfUpdate + 1, state.length)
                ];
            case AppActions.REMOVE_CLIENTCORSORIGIN:
                const idOfDelete = (<AppActions.RemoveClientCORSOrigin>action).id;
                const corsOriginToDelete = state.find(x => x.id === idOfDelete);
                const indexOfDelete = state.indexOf(corsOriginToDelete);
                return [
                    ...state.slice(0, indexOfDelete),
                    ...state.slice(indexOfDelete + 1, state.length)
                ]
            case AppActions.CLEAR_CLIENTCORSORIGINS:
                return [];
            default:
                return state;
    }
}

export function clientsScopesreducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_CLIENTSCOPES:
            return newState(state, action.clientScopes);
        case AppActions.CREATE_CLIENTSCOPE:
            let addClientScopeState = state.concat((<AppActions.CreateClientScope>action).clientScope);
            addClientScopeState = addClientScopeState.sort((a, b) => {
                if (a.scopeName > b.scopeName) {
                    return 1;
                } else {
                    return -1;
                }
            });
            return newState(state, addClientScopeState);
        case AppActions.UPDATE_CLIENTSCOPE:
            const updateClientScopeNewState = (<AppActions.UpdateClientScope>action).clientScope;
            const clientScopeToModify = state.find(x => x.id === updateClientScopeNewState.id);
            const indexOfUpdate = state.indexOf(clientScopeToModify);
            return [
                ...state.slice(0, indexOfUpdate),
                updateClientScopeNewState,
                ...state.slice(indexOfUpdate + 1, state.length)
            ];
        case AppActions.REMOVE_CLIENTSCOPE:
            const idOfDelete = (<AppActions.RemoveClientScope>action).id;
            const clientScopeToDelete = state.find(x => x.id === idOfDelete);
            const indexOfDelete = state.indexOf(clientScopeToDelete);
            return [
                ...state.slice(0, indexOfDelete),
                ...state.slice(indexOfDelete + 1, state.length)
            ]
        case AppActions.CLEAR_CLIENTSCOPES:
            return [];
        default:
            return state;
    }
}

export function grantsReducer(state: any = [], action: AppActions.AllAppActions) {
    switch (action.type) {
        case AppActions.SET_GRANTS:
            return newState(state, action.grants);
        case AppActions.CREATE_GRANT:
            let addGrantNewState = state.concat((<AppActions.CreateGrant>action).grant);
            addGrantNewState = addGrantNewState.sort((a, b) => {
                if (a.grantName > b.grantName) {
                    return 1;
                } else {
                    return -1;
                }
            });
            return newState(state, addGrantNewState);
        case AppActions.UPDATE_GRANT:
            const updateGrantNewState = (<AppActions.UpdateGrant>action).grant;
            const grantToModify = state.find(x => x.id === updateGrantNewState.id);
            const indexOfUpdate = state.indexOf(grantToModify);
            return [
                ...state.slice(0, indexOfUpdate),
                updateGrantNewState,
                ...state.slice(indexOfUpdate + 1, state.length)
            ];
        case AppActions.REMOVE_GRANT:
            const idOfDelete = (<AppActions.RemoveGrant>action).id;
            const grantToDelete = state.find(x => x.id === idOfDelete);
            const indexOfDelete = state.indexOf(grantToDelete);
            return [
                ...state.slice(0, indexOfDelete),
                ...state.slice(indexOfDelete + 1, state.length)
            ]
        case AppActions.CLEAR_GRANTS:
            return [];
        default:
            return state;
    }
}