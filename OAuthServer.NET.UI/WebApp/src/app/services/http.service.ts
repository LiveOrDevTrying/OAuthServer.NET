import { HttpClient, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { ClearClientCORSOrigins, ClearClientPostLogoutRedirectURIs, ClearClientRedirectURIs, ClearClients, ClearClientScopes, ClearGrants, CreateClient, CreateClientCORSOrigin, CreateClientPostLogoutRedirectURI, CreateClientRedirectURI, CreateClientScope, CreateGrant, RemoveClient, RemoveClientCORSOrigin, RemoveClientPostLogoutRedirectURI, RemoveClientRedirectURI, RemoveClientScope, RemoveGrant, SetClientCORSOrigins, SetClientPostLogoutRedirectURIs, SetClientRedirectURIs, SetClients, SetClientScopes, SetGrants, UpdateClient, UpdateClientCORSOrigin, UpdateClientPostLogoutRedirectURI, UpdateClientRedirectURI, UpdateClientScope, UpdateGrant } from '../reducers';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private _hasPayloadBeenRecevied: boolean;

  constructor(private httpClient: HttpClient,
    private store: Store<IAppState>) {}

  login(login: ILogin): Observable<any> {
    return this.httpClient.post<ILogin>('/login', login)
  }

  requestPayload() {
    if (!this._hasPayloadBeenRecevied) {
      this.httpClient.get<IAppState>('/payload')
      .subscribe((appState: IAppState) => {
        this._hasPayloadBeenRecevied = true;
        this.store.dispatch(new SetClients(appState.clients));
        this.store.dispatch(new SetGrants(appState.grants));
        this.store.dispatch(new SetClientPostLogoutRedirectURIs(appState.clientsPostLogoutRedirectURIs));
        this.store.dispatch(new SetClientRedirectURIs(appState.clientsRedirectURIs));
        this.store.dispatch(new SetClientCORSOrigins(appState.clientsCORSOrigins));
        this.store.dispatch(new SetClientScopes(appState.clientsScopes));
      });
    }
  }

  clearPayload() {
    this._hasPayloadBeenRecevied = false;
    this.store.dispatch(new ClearClients());
    this.store.dispatch(new ClearGrants());
    this.store.dispatch(new ClearClientPostLogoutRedirectURIs());
    this.store.dispatch(new ClearClientRedirectURIs());
    this.store.dispatch(new ClearClientCORSOrigins());
    this.store.dispatch(new ClearClientScopes());
  }

  createClientImplicit(request: IClientImplicitCreateRequest) {
    this.httpClient.post(`/clients/implicit`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new CreateClient(client));
      });
  }

  updateClientImplicit(request: IClientImplicitUpdateRequest) {
    this.httpClient.put(`/clients/implicit/${request.id}`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new UpdateClient(client));
      });
  }

  createClientAuthorizationCode(request: IClientAuthorizationCodeCreateRequest) {
    this.httpClient.post(`/clients/authorizationcode`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new CreateClient(client));
      });
  }

  updateClientAuthorizationCode(request: IClientAuthorizationCodeUpdateRequest) {
    this.httpClient.put(`/clients/authorizationcode/${request.id}`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new UpdateClient(client));
      });
  }

  createClientClientCredentials(request: IClientClientCredentialsCreateRequest) {
    this.httpClient.post(`/clients/clientcredentials`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new CreateClient(client));
      });
  }

  updateClientClientCredentials(request: IClientClientCredentialsUpdateRequest) {
    this.httpClient.put(`/clients/clientcredentials/${request.id}`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new UpdateClient(client));
      });
  }

  createClientROPassword(request: IClientResourceOwnerPasswordCreateRequest) {
    this.httpClient.post(`/clients/ropassword`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new CreateClient(client));
      });
  }

  updateClientROPassword(request: IClientResourceOwnerPasswordUpdateRequest) {
    this.httpClient.put(`/clients/ropassword/${request.id}`, request)
      .subscribe((client: IClient) => {
        this.store.dispatch(new UpdateClient(client));
      });
  }

  removeClient(id: string) {
    this.httpClient.delete(`/clients/${id}`, {observe: 'response'})
    .subscribe((result: HttpResponseBase) => {
      if (result.status === 204) {
        this.store.dispatch(new RemoveClient(id));
      }
    });
  }

  createClientScope(request: IClientScopeCreateRequest) {
    this.httpClient.post(`/clients/scopes/`, request)
      .subscribe((clientScope: IClientScope) => {
        this.store.dispatch(new CreateClientScope(clientScope));
      })
  }

  updateClientScope(request: IClientScopeUpdateRequest) {
    this.httpClient.put(`/clients/scopes/${request.id}`, request)
      .subscribe((clientScope: IClientScope) => {
        this.store.dispatch(new UpdateClientScope(clientScope));
      })
  }

  removeClientScope(id: string) {
    this.httpClient.delete(`/clients/scopes/${id}`, {observe: 'response'})
    .subscribe((result: HttpResponseBase) => {
      if (result.status === 204) {
        this.store.dispatch(new RemoveClientScope(id));
      }
    });
  }

  createClientCORSOrigin(request: IClientCORSOriginCreateRequest) {
    this.httpClient.post(`/clients/corsOrigins/`, request)
      .subscribe((clientCORSOrigin: IClientCORSOrigin) => {
        this.store.dispatch(new CreateClientCORSOrigin(clientCORSOrigin));
      })
  }

  updateClientCORSOrigin(request: IClientCORSOriginUpdateRequest) {
    this.httpClient.put(`/clients/corsOrigins/${request.id}`, request)
      .subscribe((clientCORSOrigin: IClientCORSOrigin) => {
        this.store.dispatch(new UpdateClientCORSOrigin(clientCORSOrigin));
      })
  }

  removeClientCORSOrigin(id: string) {
    this.httpClient.delete(`/clients/corsOrigins/${id}`, {observe: 'response'})
    .subscribe((result: HttpResponseBase) => {
      if (result.status === 204) {
        this.store.dispatch(new RemoveClientCORSOrigin(id));
      }
    });
  }

  createClientRedirectURI(request: IClientRedirectURICreateRequest) {
    this.httpClient.post(`/clients/redirectURIs/`, request)
      .subscribe((clientRedirectURI: IClientRedirectURI) => {
        this.store.dispatch(new CreateClientRedirectURI(clientRedirectURI));
      })
  }

  updateClientRedirectURI(request: IClientRedirectURIUpdateRequest) {
    this.httpClient.put(`/clients/redirectURIs/${request.id}`, request)
      .subscribe((clientRedirectURI: IClientRedirectURI) => {
        this.store.dispatch(new UpdateClientRedirectURI(clientRedirectURI));
      })
  }

  removeClientRedirectURI(id: string) {
    this.httpClient.delete(`/clients/redirectURIs/${id}`, {observe: 'response'})
    .subscribe((result: HttpResponseBase) => {
      if (result.status === 204) {
        this.store.dispatch(new RemoveClientRedirectURI(id));
      }
    });
  }

  createClientPostLogoutRedirectURI(request: IClientPostLogoutRedirectURICreateRequest) {
    this.httpClient.post(`/clients/postLogoutRedirectURIs/`, request)
      .subscribe((clientPostLogoutRedirectURI: IClientPostLogoutRedirectURI) => {
        this.store.dispatch(new CreateClientPostLogoutRedirectURI(clientPostLogoutRedirectURI));
      })
  }

  updateClientPostLogoutRedirectURI(request: IClientPostLogoutRedirectURIUpdateRequest) {
    this.httpClient.put(`/clients/postLogoutRedirectURIs/${request.id}`, request)
      .subscribe((clientPostLogoutRedirectURI: IClientPostLogoutRedirectURI) => {
        this.store.dispatch(new UpdateClientPostLogoutRedirectURI(clientPostLogoutRedirectURI));
      })
  }

  removeClientPostLogoutRedirectURI(id: string) {
    this.httpClient.delete(`/clients/postLogoutRedirectURIs/${id}`, {observe: 'response'})
    .subscribe((result: HttpResponseBase) => {
      if (result.status === 204) {
        this.store.dispatch(new RemoveClientPostLogoutRedirectURI(id));
      }
    });
  }

  createGrant(grant: IGrant) {
    this.httpClient.post<IGrant>('/grants', grant)
      .subscribe((grant: IGrant) => {
        if (grant) {
          this.store.dispatch(new CreateGrant(grant));
        }
      });
  }

  updateGrant(grant: IGrant) {
    this.httpClient.put<IGrant>(`/grants/${grant.id}`, grant, {observe: 'response'})
      .subscribe((result: HttpResponseBase) => {
        if (result.status === 204) {
          this.store.dispatch(new UpdateGrant(grant));
        }
      });
  }

  removeGrant(id: string) {
    this.httpClient.delete(`/grants/${id}`, {observe: 'response'})
      .subscribe((result: HttpResponseBase) => {
        if (result.status === 204) {
          this.store.dispatch(new RemoveGrant(id));
        }
      });
  }
}
