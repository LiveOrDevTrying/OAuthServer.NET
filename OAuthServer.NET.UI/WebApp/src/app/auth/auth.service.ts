import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _authenticationChanged = new BehaviorSubject<boolean>(false);
  
  constructor() { }

  getToken(): string {
    if (!this.isAuthenticated()) {
      return '';
    }
    
    const json = JSON.parse(window.localStorage['token']);
    return json.access_token;
  }

  setToken(token: IToken) {
    window.localStorage['token'] = JSON.stringify(token);

    const tokenExpiration: Date = new Date();
    tokenExpiration.setSeconds((new Date().getSeconds() + token.expires_in));
    window.localStorage['expirationdate'] = tokenExpiration;
    this._authenticationChanged.next(this.isAuthenticated());

  }

  clearToken() {
    window.localStorage['token'] = undefined;
    this._authenticationChanged.next(this.isAuthenticated());
  }

  isAuthenticationChanged(): Observable<boolean> {
    return this._authenticationChanged.asObservable();
  }

  isAuthenticated(): boolean {
    return (window.localStorage['token'] !== undefined &&
      window.localStorage['token'] !== null &&
      window.localStorage['token'] !== '' &&
      window.localStorage['token'] !== 'undefined' &&
      window.localStorage['token'] !== 'null' &&
      Date.parse(window.localStorage['expirationdate']) > new Date().getSeconds());
  }
}
