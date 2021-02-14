import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _authenticationChanged = new BehaviorSubject<boolean>(false);
  token: IToken;
  tokenExpiration: Date;

  constructor() { }

  getToken(): string {
    if (!this.isAuthenticated()) {
      return '';
    }

    if (!this.token) {
      return '';
    }

    return this.token.access_token;
    //const json = JSON.parse(window.localStorage['token']);
    //return json.access_token;
  }

  setToken(token: IToken) {
    this.token = token;

    //window.localStorage['token'] = JSON.stringify(token);

    this.tokenExpiration = new Date();
    this.tokenExpiration.setSeconds((new Date().getSeconds() + token.expires_in));
    //window.localStorage['expirationdate'] = tokenExpiration;
    this._authenticationChanged.next(this.isAuthenticated());

  }

  clearToken() {
    this.token = undefined;
    //window.localStorage['token'] = undefined;
    this._authenticationChanged.next(this.isAuthenticated());
  }

  isAuthenticationChanged(): Observable<boolean> {
    return this._authenticationChanged.asObservable();
  }

  isAuthenticated(): boolean {
    if (this.token !== undefined) {
      console.log(this.tokenExpiration);
      console.log(new Date());
      console.log(this.tokenExpiration > new Date());
      //this.tokenExpiration.getSeconds() > new Date().getSeconds());
      return true;
    }

    //return (window.localStorage['token'] !== undefined &&
    //  window.localStorage['token'] !== null &&
    //  window.localStorage['token'] !== '' &&
    //  window.localStorage['token'] !== 'undefined' &&
    //  window.localStorage['token'] !== 'null' &&
    //  Date.parse(window.localStorage['expirationdate']) > new Date().getSeconds());
  }
}
