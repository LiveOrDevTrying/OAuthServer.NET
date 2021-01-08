import { Action } from '@ngrx/store';

export const SET_CLIENTS = '[App] Set Clients';
export const CREATE_CLIENT = '[App] Create Client';
export const UPDATE_CLIENT = '[App] Update Client';
export const REMOVE_CLIENT = '[App] Remove Client';
export const CLEAR_CLIENTS = '[App] Clear Clients';

export const SET_GRANTS = '[App] Set Grants';
export const CREATE_GRANT = '[App] Create Grant';
export const UPDATE_GRANT = '[App] Update Grant';
export const REMOVE_GRANT = '[App] Remove Grant';
export const CLEAR_GRANTS = '[App] Clear Grants';

export const SET_CLIENTPOSTLOGOUTREDIRECTURIS = '[App] Set Client Post Logout Redirect URIs';
export const CREATE_CLIENTPOSTLOGOUTREDIRECTURI = '[App] Create Client Post Logout Redirect URI';
export const UPDATE_CLIENTPOSTLOGOUTREDIRECTURI = '[App] Update Client Post Logout Redirect URI';
export const REMOVE_CLIENTPOSTLOGOUTREDIRECTURI = '[App] Remove Client Post Logout Redirect URI';
export const CLEAR_CLIENTPOSTLOGOUTREDIRECTURIS = '[App] Clear Client Post Logout Redirect URIs';

export const SET_CLIENTREDIRECTURIS = '[App] Set Client Redirect URIs';
export const CREATE_CLIENTREDIRECTURI = '[App] Create Client Redirect URI';
export const UPDATE_CLIENTREDIRECTURI = '[App] Update Client Redirect URI';
export const REMOVE_CLIENTREDIRECTURI = '[App] Remove Client Redirect URI';
export const CLEAR_CLIENTREDIRECTURIS = '[App] Clear Client Redirect URIs';

export const SET_CLIENTCORSORIGINS = '[App] Set Client CORS Origins';
export const CREATE_CLIENTCORSORIGIN = '[App] Create Client CORS Origin';
export const UPDATE_CLIENTCORSORIGIN = '[App] Update Client CORS Origin';
export const REMOVE_CLIENTCORSORIGIN = '[App] Remove Client CORS Origin';
export const CLEAR_CLIENTCORSORIGINS = '[App] Clear Client CORS Origins';

export const SET_CLIENTSCOPES = '[App] Set Client Scopes';
export const CREATE_CLIENTSCOPE = '[App] Create Client Scope';
export const UPDATE_CLIENTSCOPE = '[App] Update Client Scope';
export const REMOVE_CLIENTSCOPE = '[App] Remove Client Scope';
export const CLEAR_CLIENTSCOPES = '[App] Clear Client Scopes';

export class SetClients implements Action {
    readonly type = SET_CLIENTS;

    constructor(public clients: IClient[]) {
    }
}
export class CreateClient implements Action {
    readonly type = CREATE_CLIENT;

    constructor(public client: IClient) {
    }
}
export class UpdateClient implements Action {
    readonly type = UPDATE_CLIENT;

    constructor(public client: IClient) {
    }
}
export class RemoveClient implements Action {
    readonly type = REMOVE_CLIENT;

    constructor(public id: string) {
    }
}

export class ClearClients implements Action {
    readonly type = CLEAR_CLIENTS;

    constructor() {
    }
}

export class SetGrants implements Action {
    readonly type = SET_GRANTS;

    constructor(public grants: IGrant[]) {
    }
}
export class CreateGrant implements Action {
    readonly type = CREATE_GRANT;

    constructor(public grant: IGrant) {
    }
}
export class UpdateGrant implements Action {
    readonly type = UPDATE_GRANT;

    constructor(public grant: IGrant) {
    }
}
export class RemoveGrant implements Action {
    readonly type = REMOVE_GRANT;

    constructor(public id: string) {
    }
}
export class ClearGrants implements Action {
    readonly type = CLEAR_GRANTS;

    constructor() {
    }
}

export class SetClientPostLogoutRedirectURIs implements Action {
    readonly type = SET_CLIENTPOSTLOGOUTREDIRECTURIS;

    constructor(public clientPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[]) {
    }
}
export class CreateClientPostLogoutRedirectURI implements Action {
    readonly type = CREATE_CLIENTPOSTLOGOUTREDIRECTURI;

    constructor(public clientPostLogoutRedirectURI: IClientPostLogoutRedirectURI) {
    }
}
export class UpdateClientPostLogoutRedirectURI implements Action {
    readonly type = UPDATE_CLIENTPOSTLOGOUTREDIRECTURI;

    constructor(public clientPostLogoutRedirectURI: IClientPostLogoutRedirectURI) {
    }
}
export class RemoveClientPostLogoutRedirectURI implements Action {
    readonly type = REMOVE_CLIENTPOSTLOGOUTREDIRECTURI;

    constructor(public id: string) {
    }
}
export class ClearClientPostLogoutRedirectURIs implements Action {
    readonly type = CLEAR_CLIENTPOSTLOGOUTREDIRECTURIS;

    constructor() {
    }
}

export class SetClientRedirectURIs implements Action {
    readonly type = SET_CLIENTREDIRECTURIS;

    constructor(public clientRedirectURIs: IClientRedirectURI[]) {
    }
}
export class CreateClientRedirectURI implements Action {
    readonly type = CREATE_CLIENTREDIRECTURI;

    constructor(public clientRedirectURI: IClientRedirectURI) {
    }
}
export class UpdateClientRedirectURI implements Action {
    readonly type = UPDATE_CLIENTREDIRECTURI;

    constructor(public clientRedirectURI: IClientRedirectURI) {
    }
}
export class RemoveClientRedirectURI implements Action {
    readonly type = REMOVE_CLIENTREDIRECTURI;

    constructor(public id: string) {
    }
}
export class ClearClientRedirectURIs implements Action {
    readonly type = CLEAR_CLIENTREDIRECTURIS;

    constructor() {
    }
}

export class SetClientCORSOrigins implements Action {
    readonly type = SET_CLIENTCORSORIGINS;

    constructor(public clientCORSOrigins: IClientCORSOrigin[]) {
    }
}
export class CreateClientCORSOrigin implements Action {
    readonly type = CREATE_CLIENTCORSORIGIN;

    constructor(public clientCORSOrigin: IClientCORSOrigin) {
    }
}
export class UpdateClientCORSOrigin implements Action {
    readonly type = UPDATE_CLIENTCORSORIGIN;

    constructor(public clientCORSOrigin: IClientCORSOrigin) {
    }
}
export class RemoveClientCORSOrigin implements Action {
    readonly type = REMOVE_CLIENTCORSORIGIN;

    constructor(public id: string) {
    }
}
export class ClearClientCORSOrigins implements Action {
    readonly type = CLEAR_CLIENTCORSORIGINS;

    constructor() {
    }
}

export class SetClientScopes implements Action {
    readonly type = SET_CLIENTSCOPES;

    constructor(public clientScopes: IClientScope[]) {
    }
}
export class CreateClientScope implements Action {
    readonly type = CREATE_CLIENTSCOPE;

    constructor(public clientScope: IClientScope) {
    }
}
export class UpdateClientScope implements Action {
    readonly type = UPDATE_CLIENTSCOPE;

    constructor(public clientScope: IClientScope) {
    }
}
export class RemoveClientScope implements Action {
    readonly type = REMOVE_CLIENTSCOPE;

    constructor(public id: string) {
    }
}

export class ClearClientScopes implements Action {
    readonly type = CLEAR_CLIENTSCOPES;

    constructor() {
    }
}

export type AllAppActions
    = SetClients
    | CreateClient
    | UpdateClient
    | RemoveClient
    | ClearClients
    | SetGrants
    | CreateGrant
    | UpdateGrant
    | RemoveGrant
    | ClearGrants
    | SetClientPostLogoutRedirectURIs
    | CreateClientPostLogoutRedirectURI
    | UpdateClientPostLogoutRedirectURI
    | RemoveClientPostLogoutRedirectURI
    | ClearClientPostLogoutRedirectURIs
    | SetClientRedirectURIs
    | CreateClientRedirectURI
    | UpdateClientRedirectURI
    | RemoveClientRedirectURI
    | ClearClientRedirectURIs
    | SetClientCORSOrigins
    | CreateClientCORSOrigin
    | UpdateClientCORSOrigin
    | RemoveClientCORSOrigin
    | ClearClientCORSOrigins
    | SetClientScopes
    | CreateClientScope
    | UpdateClientScope
    | RemoveClientScope
    | ClearClientScopes;