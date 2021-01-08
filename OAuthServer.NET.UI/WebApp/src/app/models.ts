interface ILogin {
    username: string,
    password: string
}

interface IToken {
    access_token: string,
    expires_in: number,
    refresh_token: string,
    state: string,
    token_type: string,
}

interface IAppState {
    clients: IClient[],
    grants: IGrant[],
    clientsPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[],
    clientsRedirectURIs: IClientRedirectURI[],
    clientsCORSOrigins: IClientCORSOrigin[],
    clientsScopes: IClientScope[]
}

interface IBaseInterface {
    id: string,
}

interface IClient extends IBaseInterface {
    clientName: string,
    clientId: string,
    clientSecret: string,
    tokenExpirationMin: number,
    issueRefreshTokens: boolean,
    refreshTokenExpirationDays: number,
    issuerURI: string,
    audience: string,
    allowRememberLogin: boolean,
    enableLocalLogin: boolean,
    enableExternalLogin: boolean,
    validateIssuer: boolean,
    validateAudience: boolean,
    validateIssuerSigningKey: boolean,
    validateCORS: boolean,
    grantId: string
}

interface IClientCreateRequest {
    grantId: string,
    clientName: string,
    clientId: string,
    tokenExpirationMin: number,
    issuerURI: string,
    audience: string,
}

interface IClientImplicitCreateRequest extends IClientCreateRequest {
    allowRememberLogin: boolean;
    enableLocalLogin: boolean;
    enableExternalLogin: boolean;
    validateIssuer: boolean;
    validateAudience: boolean;
    validateCORS: boolean;
}

interface IClientImplicitUpdateRequest extends IClientImplicitCreateRequest {
    id: string;
}

interface IClientAuthorizationCodeCreateRequest extends IClientCreateRequest {
    clientSecret: string,
    issueRefreshTokens: boolean,
    refreshTokenExpirationDays: number,
    allowRememberLogin: boolean,
    enableLocalLogin: boolean,
    enableExternalLogin: boolean,
    validateIssuer: boolean,
    validateAudience: boolean,
    validateIssuerSigningKey: boolean,
    validateCORS: boolean,
}

interface IClientAuthorizationCodeUpdateRequest extends IClientAuthorizationCodeCreateRequest {
    id: string
}

interface IClientResourceOwnerPasswordCreateRequest extends IClientCreateRequest {
    clientSecret: string,
    issueRefreshTokens: boolean,
    refreshTokenExpirationDays: number,
    validateIssuer: boolean,
    validateAudience: boolean,
    validateIssuerSigningKey: boolean,
    validateCORS: boolean,
}

interface IClientResourceOwnerPasswordUpdateRequest extends IClientResourceOwnerPasswordCreateRequest {
    id: string;
}

interface IClientClientCredentialsCreateRequest extends IClientCreateRequest {
    clientSecret: string,
    issueRefreshTokens: boolean,
    refreshTokenExpirationDays: number,
    validateIssuer: boolean,
    validateAudience: boolean,
    validateIssuerSigningKey: boolean,
    validateCORS: boolean,
}

interface IClientClientCredentialsUpdateRequest extends IClientClientCredentialsCreateRequest {
    id: string
}

interface IClientScopeCreateRequest {
    scopeName: string
    clientId: string;
}

interface IClientScopeUpdateRequest extends IClientScopeCreateRequest{
    id: string
}

interface IClientCORSOriginCreateRequest {
    originURI: string;
    clientId: string;
}

interface IClientCORSOriginUpdateRequest extends IClientCORSOriginCreateRequest {
    id: string;
}

interface IClientRedirectURICreateRequest {
    redirectURI: string;
    clientId: string;
}

interface IClientRedirectURIUpdateRequest extends IClientRedirectURICreateRequest {
    id: string;
}

interface IClientPostLogoutRedirectURICreateRequest {
    postLogoutRedirectURI : string
    clientId: string;
}

interface IClientPostLogoutRedirectURIUpdateRequest extends IClientPostLogoutRedirectURICreateRequest {
    id: string;
}

interface IClientPostLogoutRedirectURI extends IBaseInterface {
    clientId: string,
    postLogoutRedirectURI: string
}

interface IClientRedirectURI extends IBaseInterface {
    clientId: string,
    redirectURI: string
}

interface IClientCORSOrigin extends IBaseInterface {
    clientId: string,
    originURI: string
}

interface IClientScope extends IBaseInterface {
    clientId: string,
    scopeName: string
}

interface IGrant extends IBaseInterface {
    grantName: string,
    authorizeResponseType: string,
    tokenGrantType: string
}

interface IConfirmationModalData {
    title: string,
    message: string,
    isConfirmed: boolean,
    cancelMessage: string,
    confirmMessage: string
}
interface IClientCreateModalData {
    grantId: string,
    isConfirmed: boolean,
    grants: IGrant[]
}
interface IClientScopeModalData {
    scopeName: string,
    isConfirmed: boolean,
    isCreate: boolean,
    existingScopes: IClientScope[],
}
interface IClientCORSOriginModalData {
    originURI: string,
    isConfirmed: boolean,
    isCreate: boolean,
    existingCORSOrigins: IClientCORSOrigin[],
}
interface IClientRedirectURIModalData {
    redirectURI: string,
    isConfirmed: boolean,
    isCreate: boolean,
    existingRedirectURIs: IClientRedirectURI[],
}
interface IClientPostLogoutRedirectURIModalData {
    postLogoutRedirectURI: string,
    isConfirmed: boolean,
    isCreate: boolean,
    existingPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[],
}