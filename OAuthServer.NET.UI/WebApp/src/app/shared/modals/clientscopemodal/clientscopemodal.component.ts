import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-clientscopemodal',
  templateUrl: './clientscopemodal.component.html',
  styleUrls: ['./clientscopemodal.component.scss']
})
export class ClientscopemodalComponent {
  existingClientScopes: IClientScope[] = [];
  scopeName: string;
  isCreate: boolean;

  data = this.dataInstance;

  constructor(private dialogRef: MatDialogRef<ClientscopemodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IClientScopeModalData) { 
      this.existingClientScopes = dataInstance.existingScopes;
      this.isCreate = dataInstance.isCreate;
      this.scopeName = dataInstance.scopeName;
    }

  ngOnInit() {
  }

  onNoClick() {
    this.dialogRef.close();
  }

  onSaveClick() {
    this.data.scopeName = this.scopeName;
    this.data.isConfirmed = true;
    this.dialogRef.close(this.data);
  }

  get isValidForSave(): boolean {
    return this.scopeName && 
      this.scopeName !== '' && 
      this.existingClientScopes.filter(x => x.scopeName.toLowerCase() === this.scopeName.toLowerCase()).length == 0;
  }
}
