import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Store } from '@ngrx/store';
import { ConfirmationmodalComponent, HttpService } from 'src/app';
import { BaseComponent } from 'src/app/base.component';
import { ClientredirecturimodalComponent } from '../modals/clientredirecturimodal/clientredirecturimodal.component';

@Component({
  selector: 'app-clientredirecturis',
  templateUrl: './clientredirecturis.component.html',
  styleUrls: ['./clientredirecturis.component.scss']
})
export class ClientredirecturisComponent extends BaseComponent {
  @Input() clientId: string;
  displayedColumns: string[] = ['redirectURI', 'editRedirectURI', 'deleteRedirectURI'];
  dataSource = new MatTableDataSource<IClientRedirectURI>();

  constructor(protected store: Store<IAppState>,
    protected http: HttpService,
    protected matDialog: MatDialog) {
      super(store, http, matDialog);
    }

    assignRedirectURIs(redirectURIs: IClientRedirectURI[]) {
      super.assignRedirectURIs(redirectURIs);
      const selectedClientRedirectURIs = redirectURIs.filter(x => x.clientId === this.clientId);
      this.dataSource.data = selectedClientRedirectURIs;
    }

    addRedirectURI() {
      const clientRedirectModalData: IClientRedirectURIModalData = {
        isConfirmed: false,
        redirectURI: undefined,
        existingRedirectURIs: this.clientsRedirectURIs.filter(x => x.clientId === this.clientId),
        isCreate: true
      };
  
      const clientRedirectCreateModal = this.matDialog.open(ClientredirecturimodalComponent, { width: '250px', data: clientRedirectModalData });
  
      clientRedirectCreateModal.afterClosed().subscribe((result: IClientRedirectURIModalData) => {
        if (result !== undefined) {
          const request : IClientRedirectURICreateRequest = {
            clientId: this.clientId,
            redirectURI: result.redirectURI,
          }

          this.http.createClientRedirectURI(request);
        }
      });
    }
  
    editRedirectURI(redirectURI: string) {
      const selectedClientRedirectURIs = this.clientsRedirectURIs.filter(x => x.clientId === this.clientId);
      const selectedRedirectURI = selectedClientRedirectURIs.find(x => x.redirectURI === redirectURI);
  
      const clientRedirectModalData: IClientRedirectURIModalData = {
        isConfirmed: false,
        redirectURI: selectedRedirectURI.redirectURI,
        existingRedirectURIs: selectedClientRedirectURIs,
        isCreate: false
      };
  
      const clientRedirectURIEditModal = this.matDialog.open(ClientredirecturimodalComponent, { width: '250px', data: clientRedirectModalData });
  
      clientRedirectURIEditModal.afterClosed().subscribe((result: IClientRedirectURIModalData) => {
        if (result !== undefined) {
          const request: IClientRedirectURIUpdateRequest = {
            redirectURI: result.redirectURI,
            id: selectedRedirectURI.id,
            clientId: this.clientId
          }

          this.http.updateClientRedirectURI(request);
        }
      });
    }
  
    deleteRedirectURI(redirectURI: string) {
      const selectedClientRedirectURIs = this.clientsRedirectURIs.filter(x => x.clientId === this.clientId);
      const selectedRedirectURI = selectedClientRedirectURIs.find(x => x.redirectURI === redirectURI);
  
      const confirmationModalData: IConfirmationModalData = {
        isConfirmed: false,
        title: 'Confirm Delete',
        message: `Are you sure you would like to delete the Redirect URI ${redirectURI}?`,
        cancelMessage: 'Cancel',
        confirmMessage: 'Confirm'
      };
  
      const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });
  
      confirmationModal.afterClosed().subscribe((result: IConfirmationModalData) => {
        if (result !== undefined) {
          this.http.removeClientRedirectURI(selectedRedirectURI.id);
        }
      });
    }
}
