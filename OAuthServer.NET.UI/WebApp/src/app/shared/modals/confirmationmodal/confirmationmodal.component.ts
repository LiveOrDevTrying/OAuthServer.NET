import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-confirmationmodal',
  templateUrl: './confirmationmodal.component.html',
  styleUrls: ['./confirmationmodal.component.scss']
})
export class ConfirmationmodalComponent implements OnInit {
  data = this.dataInstance;

  constructor(private dialogRef: MatDialogRef<ConfirmationmodalComponent>,
    @Inject(MAT_DIALOG_DATA) private dataInstance: IConfirmationModalData) { }

  ngOnInit() {
  }

  onNoClick() {
    this.dialogRef.close();
  }
}
