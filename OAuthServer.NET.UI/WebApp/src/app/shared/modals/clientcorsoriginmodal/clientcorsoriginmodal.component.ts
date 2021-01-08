import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-clientcorsoriginmodal',
  templateUrl: './clientcorsoriginmodal.component.html',
  styleUrls: ['./clientcorsoriginmodal.component.scss']
})
export class ClientcorsoriginmodalComponent {
  existingCORSOrigins: IClientCORSOrigin[] = [];
  originURI: string;
  isCreate: boolean;

  data = this.dataInstance;

  constructor(private dialogRef: MatDialogRef<ClientcorsoriginmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IClientCORSOriginModalData) { 
      this.existingCORSOrigins = dataInstance.existingCORSOrigins;
      this.isCreate = dataInstance.isCreate;
      this.originURI = dataInstance.originURI;
    }

  onNoClick() {
    this.dialogRef.close();
  }

  onSaveClick() {
    this.data.originURI = this.originURI;
    this.data.isConfirmed = true;
    this.dialogRef.close(this.data);
  }

  get isValidForSave(): boolean {
    return this.originURI && 
      this.originURI !== '' && 
      this.existingCORSOrigins.filter(x => x.originURI.toLowerCase() === this.originURI.toLowerCase()).length == 0;
  }

}
