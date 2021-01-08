import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BaseIdComponent } from '../../baseId.component';
import { HttpService } from '../../services/http.service';

@Component({
  selector: 'app-grant',
  templateUrl: './grant.component.html',
  styleUrls: ['./grant.component.scss']
})
export class GrantComponent extends BaseIdComponent {
  id: string;
  grantName: string;
  authorizeResponseType: string;
  tokenGrantType: string;
  
  constructor(protected route: ActivatedRoute,
    protected store: Store<IAppState>,
    protected http: HttpService,
    protected router: Router,
    protected matDialog: MatDialog) {
      super(store, http, matDialog, route);
     }

  assignGrants(grants: IGrant[]) {
    super.assignGrants(grants);

    if (this.id && this.id !== '') {
      const grant = grants.find(x => x.id == this.id);
      this.grantName = grant.grantName;
      this.authorizeResponseType = grant.authorizeResponseType;
      this.tokenGrantType = grant.tokenGrantType;
    }
  }

  createGrant() {
    const request: IGrant = {
      id: this.id,
      grantName: this.grantName,
      authorizeResponseType: this.authorizeResponseType,
      tokenGrantType: this.tokenGrantType
    };

    if (this.id && this.id !== '') {
      this.http.updateGrant(request);
    } else {
      this.http.createGrant(request);
    } 

    this.router.navigate(['/grants']);
  }
}
