import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogActions, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Store } from '@ngrx/store';
import { HttpService } from 'src/app';
import { BaseComponent } from 'src/app/base.component';
import { CacheService } from 'src/app/services/cache.service';

@Component({
  selector: 'app-clientcreatemodal',
  templateUrl: './clientcreatemodal.component.html',
  styleUrls: ['./clientcreatemodal.component.scss']
})
export class ClientcreatemodalComponent {
  grantId: string;
  grants: IGrant[] = [];

  data = this.dataInstance;

  constructor(private dialogRef: MatDialogRef<ClientcreatemodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IClientCreateModalData,
    private cacheService: CacheService) {
      this.grants = dataInstance.grants;
   }

   onNoClick() {
     this.dialogRef.close();
   }

   onSaveClick() {
     this.data.grantId = this.grantId;
     this.data.isConfirmed = true;

     this.cacheService.storeGrantId(this.grantId);

     this.dialogRef.close(this.data);
   }

   get isValidForSave(): boolean {
    return this.grantId && this.grantId !== '';
  }
}
