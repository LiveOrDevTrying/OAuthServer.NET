import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BaseIdComponent } from 'src/app/baseId.component';
import { CacheService } from 'src/app/services/cache.service';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-clientclientcredentials',
  templateUrl: './clientclientcredentials.component.html',
  styleUrls: ['./clientclientcredentials.component.scss']
})
export class ClientclientcredentialsComponent extends BaseIdComponent {
  grantId: string;
  clientName: string;
  clientId: string;
  clientSecret: string;
  tokenExpirationMin: number;
  issuerURI: string;
  audience: string;
  validateIssuer: boolean = true;
  validateAudience: boolean = true
  validateCORS: boolean = true;
  validateSigningKey: boolean = true;
  issueRefreshTokens: boolean = true;
  refreshTokenExpirationDays: number = 7;

  constructor(protected store: Store<IAppState>,
    protected http: HttpService,
    protected matDialog: MatDialog,
    protected route: ActivatedRoute,
    protected router: Router,
    protected cacheService: CacheService) {
      super(store, http, matDialog, route);
      this.grantId = cacheService.getGrantId();
  }

  assignClients(clients: IClient[]) {
    super.assignClients(clients);

    let client = this.clients.find(x => x.id == this.id);

    if (!client) {
      client = this.clients.find(x => x.clientName === this.clientName);
    }

    if (client) {
      this.id = client.id;
      this.grantId = client.grantId;
      this.clientName = client.clientName;
      this.clientId = client.clientId;
      this.clientSecret = client.clientSecret;
      this.tokenExpirationMin = client.tokenExpirationMin;
      this.issuerURI = client.issuerURI;
      this.audience = client.audience;
      this.validateIssuer = client.validateIssuer;
      this.validateAudience = client.validateAudience;
      this.validateCORS = client.validateCORS;
      this.validateSigningKey = client.validateIssuerSigningKey;
      this.issueRefreshTokens = client.issueRefreshTokens;
      this.refreshTokenExpirationDays = client.refreshTokenExpirationDays;
    }
  }

  createClient() {
    // Todo: Validate all data is correct

    if (this.id && this.id !== '') {
      const request: IClientClientCredentialsUpdateRequest = {
        validateAudience: this.validateAudience,
        validateCORS: this.validateCORS,
        validateIssuer: this.validateIssuer,
        clientId: this.clientId,
        clientName: this.clientName,
        grantId: this.grantId,
        tokenExpirationMin: this.tokenExpirationMin,
        issuerURI: this.issuerURI,
        audience: this.audience,
        id: this.id,
        clientSecret: this.clientSecret,
        issueRefreshTokens: this.issueRefreshTokens,
        refreshTokenExpirationDays: this.refreshTokenExpirationDays,
        validateIssuerSigningKey: this.validateSigningKey
      }

      this.http.updateClientClientCredentials(request);
      this.router.navigate(['/']);
    } else {
      const request: IClientClientCredentialsCreateRequest = {
        validateAudience: this.validateAudience,
        validateCORS: this.validateCORS,
        validateIssuer: this.validateIssuer,
        clientId: this.clientId,
        clientName: this.clientName,
        grantId: this.grantId,
        tokenExpirationMin: this.tokenExpirationMin,
        issuerURI: this.issuerURI,
        audience: this.audience,
        clientSecret: this.clientSecret,
        issueRefreshTokens: this.issueRefreshTokens,
        refreshTokenExpirationDays: this.refreshTokenExpirationDays,
        validateIssuerSigningKey: this.validateSigningKey
      }

      this.http.createClientClientCredentials(request);
    }
  }
}
