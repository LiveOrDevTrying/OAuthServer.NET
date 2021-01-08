import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-clientpostlogoutredirecturimodal',
  templateUrl: './clientpostlogoutredirecturimodal.component.html',
  styleUrls: ['./clientpostlogoutredirecturimodal.component.scss']
})
export class ClientpostlogoutredirecturimodalComponent {
  existingPostLogoutRedirectURIs: IClientPostLogoutRedirectURI[] = [];
  postLogoutRedirectURI: string;
  isCreate: boolean;

  data = this.dataInstance;

  constructor(private dialogRef: MatDialogRef<ClientpostlogoutredirecturimodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IClientPostLogoutRedirectURIModalData) { 
      this.existingPostLogoutRedirectURIs = dataInstance.existingPostLogoutRedirectURIs;
      this.isCreate = dataInstance.isCreate;
      this.postLogoutRedirectURI = dataInstance.postLogoutRedirectURI;
    }

  onNoClick() {
    this.dialogRef.close();
  }

  onSaveClick() {
    this.data.postLogoutRedirectURI = this.postLogoutRedirectURI;
    this.data.isConfirmed = true;
    this.dialogRef.close(this.data);
  }

  get isValidForSave(): boolean {
    return this.postLogoutRedirectURI && 
      this.postLogoutRedirectURI !== '' && 
      this.existingPostLogoutRedirectURIs.filter(x => x.postLogoutRedirectURI.toLowerCase() === this.postLogoutRedirectURI.toLowerCase()).length == 0;
  }
}