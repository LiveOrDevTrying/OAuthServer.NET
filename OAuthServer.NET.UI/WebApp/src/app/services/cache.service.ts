import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private _grantId: string;

  constructor() { }

  storeGrantId(grantId: string) {
    this._grantId = grantId;
  }

  getGrantId(): string {
    return this._grantId;
  }
}
