import { Component } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { Store } from '@ngrx/store';
import { BaseComponent } from '../../base.component';
import { HttpService } from '../../services/http.service';
import { ConfirmationmodalComponent } from '../../shared/modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-grants',
  templateUrl: './grants.component.html',
  styleUrls: ['./grants.component.scss']
})
export class GrantsComponent extends BaseComponent {
  displayedColumns: string[] = ['grantName', 'authorizeResponseType', 'tokenGrantType', 'numClientApplications', 'removeGrant'];
  dataSource = new MatTableDataSource<IGrant>();

  constructor(protected store: Store<IAppState>,
    protected httpService: HttpService,
    protected matDialog: MatDialog)  {
    super(store, httpService, matDialog);
  }

  assignGrants(grants: IGrant[]) {
    super.assignGrants(grants);
    this.dataSource.data = this.grants;
  }

  removeGrant(id: string) {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Confirm Delete',
      message: 'Are you sure you would like to delete this grant type?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.matDialog.open(ConfirmationmodalComponent, { width: '250px', data: confirmationModalData });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.httpService.removeGrant(id);
      }
    });
  }
}
