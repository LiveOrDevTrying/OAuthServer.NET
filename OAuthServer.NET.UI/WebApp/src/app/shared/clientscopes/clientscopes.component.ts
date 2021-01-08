import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Store } from '@ngrx/store';
import { ConfirmationmodalComponent, HttpService } from 'src/app';
import { BaseComponent } from 'src/app/base.component';
import { ClientscopemodalComponent } from '../modals/clientscopemodal/clientscopemodal.component';

@Component({
  selector: 'app-clientscopes',
  templateUrl: './clientscopes.component.html',
  styleUrls: ['./clientscopes.component.scss']
})
export class ClientscopesComponent extends BaseComponent {
  @Input() clientId: string;
  displayedColumns: string[] = ['scopeName', 'editScope', 'deleteScope'];
  dataSource = new MatTableDataSource<IClientScope>();

  constructor(protected store: Store<IAppState>,
    protected http: HttpService,
    protected matDialog: MatDialog) {
      super(store, http, matDialog);
    }

  assignScopes(clientScopes: IClientScope[]) {
    super.assignScopes(clientScopes);
    const selectedScopes = clientScopes.filter(x => x.clientId === this.clientId);
    this.dataSource.data = selectedScopes;
  }

  addScope() {
    const clientScopeModalData: IClientScopeModalData = {
      isConfirmed: false,
      scopeName: undefined,
      existingScopes: this.clientsScopes.filter(x => x.clientId === this.clientId),
      isCreate: true
    };

    const clientScopeCreateModal = this.matDialog.open(ClientscopemodalComponent, { width: '250px', data: clientScopeModalData });

    clientScopeCreateModal.afterClosed().subscribe((result: IClientScopeModalData) => {
      if (result !== undefined) {
        const scope : IClientScopeCreateRequest = {
          clientId: this.clientId,
          scopeName: result.scopeName,
        }
        this.http.createClientScope(scope);
      }
    });
  }

  editScope(scopeName: string) {
    const selectedScopes = this.clientsScopes.filter(x => x.clientId === this.clientId);
    const selectedScope = selectedScopes.find(x => x.scopeName === scopeName);

    const clientScopeModalData: IClientScopeModalData = {
      isConfirmed: false,
      scopeName: selectedScope.scopeName,
      existingScopes: selectedScopes,
      isCreate: false
    };

    const clientScopeEditModal = this.matDialog.open(ClientscopemodalComponent, { width: '250px', data: clientScopeModalData });

    clientScopeEditModal.afterClosed().subscribe((result: IClientScopeModalData) => {
      if (result !== undefined) {
        const request: IClientScopeUpdateRequest = {
          scopeName: result.scopeName,
          id: selectedScope.id,
          clientId: this.clientId
        }

        this.http.updateClientScope(request);
      }
    });
  }

  deleteScope(scopeName: string) {
    const selectedScopes = this.clientsScopes.filter(x => x.clientId === this.clientId);
    const selectedScope = selectedScopes.find(x => x.scopeName === scopeName);

    const confirmationModalData: IConfirmationModalData = {
      isConfirmed: false,
      title: 'Confirm Delete',
      message: `Are you sure you would like to delete the scope ${scopeName}?`,
      cancelMessage: 'Cancel',
      confirmMessage: 'Confirm'
    };

    const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });

    confirmationModal.afterClosed().subscribe((result: IConfirmationModalData) => {
      if (result !== undefined) {
        this.http.removeClientScope(selectedScope.id);
      }
    });
  }
}
