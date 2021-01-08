import { OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { HttpService } from './services/http.service';

export abstract class BaseComponent implements OnInit, OnDestroy {
  grants: IGrant[] = [];
  clients: IClient[] = [];
  clientsPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[] = [];
  clientsRedirectURIs: IClientRedirectURI[] = [];
  clientsScopes: IClientScope[] = [];
  clientsCORSOrigins: IClientCORSOrigin[] = [];

  $grantsSubscription: Subscription;
  $clientsSubscription: Subscription;
  $clientsPostLogoutRedirectURIsSubscription: Subscription;
  $clientsRedirectURIsSubscription: Subscription;
  $clientsScopesSubscription: Subscription;
  $clientsCORSOriginsSubscription: Subscription;

  constructor(protected store: Store<IAppState>,
    protected httpService: HttpService,
    protected matDialog: MatDialog) { }

  ngOnInit() {
    this.$grantsSubscription = this.store
      .select(x => x.grants)
      .subscribe((grants: IGrant[]) => {
        this.assignGrants(grants);
    });

    this.$clientsSubscription = this.store
      .select(x => x.clients) 
      .subscribe((clients: IClient[]) => {
        this.assignClients(clients);
      });

    this.$clientsPostLogoutRedirectURIsSubscription = this.store
      .select(x => x.clientsPostLogoutRedirectURIs)
      .subscribe((clientsPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[]) => {
        this.assignPostLogoutRedirectURIs(clientsPostLogoutRedirectURIs);
      });

    this.$clientsRedirectURIsSubscription = this.store
      .select(x => x.clientsRedirectURIs)
      .subscribe((clientsRedirectURIs: IClientRedirectURI[]) => {
        this.assignRedirectURIs(clientsRedirectURIs);
      });

    this.$clientsScopesSubscription = this.store
      .select(x => x.clientsScopes)
      .subscribe((clientScopes: IClientScope[]) => {
        this.assignScopes(clientScopes);
      });

    this.$clientsCORSOriginsSubscription = this.store
      .select(x => x.clientsCORSOrigins)
      .subscribe((clientCORSOrigins: IClientCORSOrigin[]) => {
        this.assignCORSOrigins(clientCORSOrigins);
      });
  }

  assignGrants(grants: IGrant[]) {
    this.grants = grants;
  }

  assignClients(clients: IClient[]) {
    this.clients = clients;
  }

  assignPostLogoutRedirectURIs(clientsPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[]) {
    this.clientsPostLogoutRedirectURIs = clientsPostLogoutRedirectURIs;
  }

  assignRedirectURIs(clientsRedirectURIs: IClientRedirectURI[]) {
    this.clientsRedirectURIs = clientsRedirectURIs;
  }

  assignScopes(clientsScopes: IClientScope[]) {
    this.clientsScopes = clientsScopes;
  }

  assignCORSOrigins(clientCORSOrigins: IClientCORSOrigin[]) {
    this.clientsCORSOrigins = clientCORSOrigins;
  }

  getGrant(grantId: string) : IGrant {
    return this.grants.find(x => x.id == grantId);
  }

  getClientsPostLogoutRedirectURIs(clientId: string): IClientPostLogoutRedirectURI[] {
    return this.clientsPostLogoutRedirectURIs.filter(x => x.clientId == clientId);
  }

  getClientsRedirectURIs(clientId: string): IClientRedirectURI[] {
    return this.clientsRedirectURIs.filter(x => x.clientId == clientId);
  }

  getNumberClientApplications(grantId: string): number {
    return this.clients.filter(x => x.grantId == grantId).length;
  }

  ngOnDestroy() {
    this.$grantsSubscription.unsubscribe();
    this.$clientsSubscription.unsubscribe();
    this.$clientsPostLogoutRedirectURIsSubscription.unsubscribe();
    this.$clientsRedirectURIsSubscription.unsubscribe();
    this.$clientsScopesSubscription.unsubscribe();
    this.$clientsCORSOriginsSubscription.unsubscribe();
  }
}
