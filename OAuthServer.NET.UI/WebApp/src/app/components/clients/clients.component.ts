import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BaseComponent } from '../../base.component';
import { HttpService } from '../../services/http.service';
import { ClientcreatemodalComponent } from '../../shared/modals/clientcreatemodal/clientcreatemodal.component';
import { ConfirmationmodalComponent } from '../../shared/modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent extends BaseComponent {
  displayedColumns: string[] = ['clientName', 'grantType', 'clientId', 'countPostLogoutRedirectURIs', 'countRedirectURIs', 'removeClient'];
  dataSource = new MatTableDataSource<IClient>();

  constructor(protected store: Store<IAppState>,
    protected router: Router,
    protected http: HttpService,
    protected matDialog: MatDialog) { 
      super(store, http, matDialog);
    }

  ngOnInit() {
    super.ngOnInit();
    this.httpService.requestPayload();
  }

  assignClients(clients: IClient[]) {
    super.assignClients(clients);
    this.dataSource.data = this.clients;
  }

  navigateToClient(clientId: string) {
    const client = this.clients.find(x => x.id == clientId);
    const grant = this.getGrant(client.grantId);

    switch (grant.grantName) {
      case 'Authorization Code':
        this.router.navigate(['/client/authorizationcode', clientId]);
        break;
      case 'Implicit':
        this.router.navigate(['/client/implicit', clientId]);
        break;
      case 'Resource Owner Password':
        this.router.navigate(['/client/ropassword', clientId]);
        break;
      case 'Client Credentials':
        this.router.navigate(['/client/clientcredentials', clientId]);
        break;
      default:
        this.router.navigate(['/client']);
        break;
    }
  }

  createClient() {
    // Confirmation box
    const confirmationModalData: IClientCreateModalData = {
      isConfirmed: false,
      grantId: undefined,
      grants: this.grants
    };

    const clientCreateModal = this.matDialog.open(ClientcreatemodalComponent, { width: '350px', data: confirmationModalData });

    clientCreateModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        const grant = this.getGrant(result.grantId);

        switch (grant.grantName) {
          case 'Authorization Code':
            this.router.navigate(['/client/authorizationcode']);
            break;
          case 'Implicit':
            this.router.navigate(['/client/implicit']);
            break;
          case 'Resource Owner Password':
            this.router.navigate(['/client/ropassword']);
            break;
          case 'Client Credentials':
            this.router.navigate(['/client/clientcredentials']);
            break;
          default:
            this.router.navigate(['/client']);
            break;
        }
      }
    })
  }

  removeClient(id: string) {
    // Confirmation box
    const confirmationModalData: IConfirmationModalData = {
      title: 'Confirm Delete',
      message: 'Are you sure you would like to delete this Client?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.http.removeClient(id);
      }
    })
  }
}
