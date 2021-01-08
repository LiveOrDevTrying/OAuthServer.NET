import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Store } from '@ngrx/store';
import { ConfirmationmodalComponent, HttpService } from 'src/app';
import { BaseComponent } from 'src/app/base.component';
import { ClientcorsoriginmodalComponent } from '../modals/clientcorsoriginmodal/clientcorsoriginmodal.component';

@Component({
  selector: 'app-clientcorsorigins',
  templateUrl: './clientcorsorigins.component.html',
  styleUrls: ['./clientcorsorigins.component.scss']
})
export class ClientcorsoriginsComponent extends BaseComponent {
  @Input() clientId: string;
  displayedColumns: string[] = ['originURI', 'editCORSOrigin', 'deleteCORSOrigin'];
  dataSource = new MatTableDataSource<IClientCORSOrigin>();

  constructor(protected store: Store<IAppState>,
    protected http: HttpService,
    protected matDialog: MatDialog) {
      super(store, http, matDialog);
    }

    assignCORSOrigins(clientCORSOrigins: IClientCORSOrigin[]) {
      super.assignCORSOrigins(clientCORSOrigins);
      const selectedCORSOrigins = clientCORSOrigins.filter(x => x.clientId === this.clientId);
      this.dataSource.data = selectedCORSOrigins;
    }

    addCORSOrigin() {
      const clientCORSModalData: IClientCORSOriginModalData = {
        isConfirmed: false,
        originURI: undefined,
        existingCORSOrigins: this.clientsCORSOrigins.filter(x => x.clientId === this.clientId),
        isCreate: true
      };
  
      const clientCORSOriginCreateModal = this.matDialog.open(ClientcorsoriginmodalComponent, { width: '250px', data: clientCORSModalData });
  
      clientCORSOriginCreateModal.afterClosed().subscribe((result: IClientCORSOriginModalData) => {
        if (result !== undefined) {
          const request : IClientCORSOriginCreateRequest = {
            clientId: this.clientId,
            originURI: result.originURI,
          }

          this.http.createClientCORSOrigin(request);
        }
      });
    }
  
    editCORSOrigin(originURI: string) {
      const selectedCORSOrigins = this.clientsCORSOrigins.filter(x => x.clientId === this.clientId);
      const selectedCORSOrigin = selectedCORSOrigins.find(x => x.originURI === originURI);
  
      const clientCORSModalData: IClientCORSOriginModalData = {
        isConfirmed: false,
        originURI: selectedCORSOrigin.originURI,
        existingCORSOrigins: selectedCORSOrigins,
        isCreate: false
      };
  
      const clientCORSEditModal = this.matDialog.open(ClientcorsoriginmodalComponent, { width: '250px', data: clientCORSModalData });
  
      clientCORSEditModal.afterClosed().subscribe((result: IClientCORSOriginModalData) => {
        if (result !== undefined) {
          const request: IClientCORSOriginUpdateRequest = {
            originURI: result.originURI,
            id: selectedCORSOrigin.id,
            clientId: this.clientId
          }

          this.http.updateClientCORSOrigin(request);
        }
      });
    }
  
    deleteCORSOrigin(originURI: string) {
      const selectedCORSOrigins = this.clientsCORSOrigins.filter(x => x.clientId === this.clientId);
      const selectedOrigin = selectedCORSOrigins.find(x => x.originURI === originURI);
  
      const confirmationModalData: IConfirmationModalData = {
        isConfirmed: false,
        title: 'Confirm Delete',
        message: `Are you sure you would like to delete the CORS Origin ${originURI}?`,
        cancelMessage: 'Cancel',
        confirmMessage: 'Confirm'
      };
  
      const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });
  
      confirmationModal.afterClosed().subscribe((result: IConfirmationModalData) => {
        if (result !== undefined) {
          this.http.removeClientCORSOrigin(selectedOrigin.id);
        }
      });
    }
}
