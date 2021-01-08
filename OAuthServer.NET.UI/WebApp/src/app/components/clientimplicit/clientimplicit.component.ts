import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BaseIdComponent } from '../../baseId.component';
import { CacheService } from '../../services/cache.service';
import { HttpService } from '../../services/http.service';

@Component({
  selector: 'app-clientimplicit',
  templateUrl: './clientimplicit.component.html',
  styleUrls: ['./clientimplicit.component.scss']
})
export class ClientimplicitComponent extends BaseIdComponent {
  grantId: string;
  clientName: string;
  clientId: string;
  tokenExpirationMin: number;
  issuerURI: string;
  audience: string;
  allowRememberLogin: boolean = true;
  enableExternalLogin: boolean = true;
  enableLocalLogin: boolean = true;
  validateIssuer: boolean = true;
  validateAudience: boolean = true
  validateCORS: boolean = true;

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
      this.tokenExpirationMin = client.tokenExpirationMin;
      this.issuerURI = client.issuerURI;
      this.audience = client.audience;
      this.allowRememberLogin = client.allowRememberLogin;
      this.enableExternalLogin = client.enableExternalLogin;
      this.enableLocalLogin = client.enableLocalLogin;
      this.validateIssuer = client.validateIssuer;
      this.validateAudience = client.validateAudience;
      this.validateCORS = client.validateCORS;
    }
  }

  createClient() {
    // Todo: Validate all data is correct

    if (this.id && this.id !== '') {
      const request: IClientImplicitUpdateRequest = {
        allowRememberLogin: this.allowRememberLogin,
        enableLocalLogin: this.enableLocalLogin,
        enableExternalLogin: this.enableExternalLogin,
        validateAudience: this.validateAudience,
        validateCORS: this.validateCORS,
        validateIssuer: this.validateIssuer,
        clientId: this.clientId,
        clientName: this.clientName,
        grantId: this.grantId,
        tokenExpirationMin: this.tokenExpirationMin,
        issuerURI: this.issuerURI,
        audience: this.audience,
        id: this.id
      }

      this.http.updateClientImplicit(request);
      this.router.navigate(['/']);
    } else {
      const request: IClientImplicitCreateRequest = {
        allowRememberLogin: this.allowRememberLogin,
        enableLocalLogin: this.enableLocalLogin,
        enableExternalLogin: this.enableExternalLogin,
        validateAudience: this.validateAudience,
        validateCORS: this.validateCORS,
        validateIssuer: this.validateIssuer,
        clientId: this.clientId,
        clientName: this.clientName,
        grantId: this.grantId,
        tokenExpirationMin: this.tokenExpirationMin,
        issuerURI: this.issuerURI,
        audience: this.audience
      }

      this.http.createClientImplicit(request);
    }
  }
}
