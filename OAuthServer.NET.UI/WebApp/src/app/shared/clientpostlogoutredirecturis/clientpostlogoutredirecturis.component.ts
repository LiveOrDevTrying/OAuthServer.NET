import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Store } from '@ngrx/store';
import { ConfirmationmodalComponent, HttpService } from 'src/app';
import { BaseComponent } from 'src/app/base.component';
import { ClientpostlogoutredirecturimodalComponent } from '../modals/clientpostlogoutredirecturimodal/clientpostlogoutredirecturimodal.component';

@Component({
  selector: 'app-clientpostlogoutredirecturis',
  templateUrl: './clientpostlogoutredirecturis.component.html',
  styleUrls: ['./clientpostlogoutredirecturis.component.scss']
})
export class ClientpostlogoutredirecturisComponent extends BaseComponent{
  @Input() clientId: string;
  displayedColumns: string[] = ['postLogoutRedirectURI', 'editPostLogoutRedirectURI', 'deletePostLogoutRedirectURI'];
  dataSource = new MatTableDataSource<IClientPostLogoutRedirectURI>();

  constructor(protected store: Store<IAppState>,
    protected http: HttpService,
    protected matDialog: MatDialog) {
      super(store, http, matDialog);
    }

    assignPostLogoutRedirectURIs(postLogoutRedirectURIs: IClientPostLogoutRedirectURI[]) {
      super.assignPostLogoutRedirectURIs(postLogoutRedirectURIs);
      const selectedClientPostLogoutRedirectURIs = postLogoutRedirectURIs.filter(x => x.clientId === this.clientId);
      this.dataSource.data = selectedClientPostLogoutRedirectURIs;
    }

    addPostLogoutRedirectURI() {
      const clientPostLogoutRedirectModalData: IClientPostLogoutRedirectURIModalData = {
        isConfirmed: false,
        postLogoutRedirectURI: undefined,
        existingPostLogoutRedirectURIs: this.clientsPostLogoutRedirectURIs.filter(x => x.clientId == this.clientId),
        isCreate: true
      };
  
      const clientPostLogoutRedirectCreateModal = this.matDialog.open(ClientpostlogoutredirecturimodalComponent, { width: '250px', data: clientPostLogoutRedirectModalData });
  
      clientPostLogoutRedirectCreateModal.afterClosed().subscribe((result: IClientPostLogoutRedirectURIModalData) => {
        if (result !== undefined) {
          const request : IClientPostLogoutRedirectURICreateRequest = {
            clientId: this.clientId,
            postLogoutRedirectURI: result.postLogoutRedirectURI,
          }

          this.http.createClientPostLogoutRedirectURI(request);
        }
      });
    }
  
    editPostLogoutRedirectURI(postLogoutRedirectURI: string) {
      const selectedClientPostLogoutRedirectURIs = this.clientsPostLogoutRedirectURIs.filter(x => x.clientId === this.clientId);
      const selectedPostLogoutRedirectURI = selectedClientPostLogoutRedirectURIs.find(x => x.postLogoutRedirectURI === postLogoutRedirectURI);
  
      const clientPostLogoutRedirectModalData: IClientPostLogoutRedirectURIModalData = {
        isConfirmed: false,
        postLogoutRedirectURI: selectedPostLogoutRedirectURI.postLogoutRedirectURI,
        existingPostLogoutRedirectURIs: selectedClientPostLogoutRedirectURIs,
        isCreate: false
      };
  
      const clientPostLogoutRedirectURIEditModal = this.matDialog.open(ClientpostlogoutredirecturimodalComponent, { width: '250px', data: clientPostLogoutRedirectModalData });
  
      clientPostLogoutRedirectURIEditModal.afterClosed().subscribe((result: IClientPostLogoutRedirectURIModalData) => {
        if (result !== undefined) {
          const request: IClientPostLogoutRedirectURIUpdateRequest = {
            postLogoutRedirectURI: result.postLogoutRedirectURI,
            id: selectedPostLogoutRedirectURI.id,
            clientId: this.clientId
          };

          this.http.updateClientPostLogoutRedirectURI(request);
        }
      });
    }
  
    deletePostLogoutRedirectURI(postLogoutRedirectURI: string) {
      const selectedClientPostLogoutRedirectURIs = this.clientsPostLogoutRedirectURIs.filter(x => x.clientId === this.clientId);
      const selectedPostLogoutRedirectURI = selectedClientPostLogoutRedirectURIs.find(x => x.postLogoutRedirectURI === postLogoutRedirectURI);
  
      const confirmationModalData: IConfirmationModalData = {
        isConfirmed: false,
        title: 'Confirm Delete',
        message: `Are you sure you would like to delete the Post Logout Redirect URI ${postLogoutRedirectURI}?`,
        cancelMessage: 'Cancel',
        confirmMessage: 'Confirm'
      };
  
      const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });
  
      confirmationModal.afterClosed().subscribe((result: IConfirmationModalData) => {
        if (result !== undefined) {
          this.http.removeClientPostLogoutRedirectURI(selectedPostLogoutRedirectURI.id);
        }
      });
    }
}
