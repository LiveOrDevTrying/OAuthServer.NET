import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ClientsComponent } from './components/clients/clients.component';
import { StoreModule } from '@ngrx/store';
import { clientsReducer, grantsReducer, clientsPostLogoutRedirectURIsreducer, clientsCORSOriginsreducer, clientsRedirectURIsreducer, clientsScopesreducer } from './reducers/index';
import { GrantsComponent } from './components/grants/grants.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';
import { ClientauthorizationcodeComponent } from './components/clientauthorizationcode/clientauthorizationcode.component';
import { ClientimplicitComponent } from './components/clientimplicit/clientimplicit.component';
import { ClientresourceownerpasswordComponent } from './components/clientresourceownerpassword/clientresourceownerpassword.component';
import { ClientclientcredentialsComponent } from './components/clientclientcredentials/clientclientcredentials.component';
import { ConfirmationmodalComponent } from './shared/modals/confirmationmodal/confirmationmodal.component';
import { GrantComponent } from './components/grant/grant.component';
import { FormsModule } from '@angular/forms';
import { ClientscopesComponent } from './shared/clientscopes/clientscopes.component';
import { ClientcreatemodalComponent } from './shared/modals/clientcreatemodal/clientcreatemodal.component';
import { ClientComponent } from './components/client/client.component';
import { ClientscopemodalComponent } from './shared/modals/clientscopemodal/clientscopemodal.component';
import { ClientcorsoriginmodalComponent } from './shared/modals/clientcorsoriginmodal/clientcorsoriginmodal.component';
import { ClientcorsoriginsComponent } from './shared/clientcorsorigins/clientcorsorigins.component';
import { ClientredirecturisComponent } from './shared/clientredirecturis/clientredirecturis.component';
import { ClientredirecturimodalComponent } from './shared/modals/clientredirecturimodal/clientredirecturimodal.component';
import { ClientpostlogoutredirecturimodalComponent } from './shared/modals/clientpostlogoutredirecturimodal/clientpostlogoutredirecturimodal.component';
import { ClientpostlogoutredirecturisComponent } from './shared/clientpostlogoutredirecturis/clientpostlogoutredirecturis.component';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { AuthGuard } from './auth/authGuard';
import { Httpinterceptor } from './interceptors/httpInterceptor';
@NgModule({
  declarations: [
    AppComponent,
    ClientsComponent,
    ClientauthorizationcodeComponent,
    ClientimplicitComponent,
    ClientresourceownerpasswordComponent,
    ClientclientcredentialsComponent,
    ConfirmationmodalComponent,
    GrantsComponent,
    GrantComponent,
    ClientscopesComponent,
    ClientcreatemodalComponent,
    ClientComponent,
    ClientscopemodalComponent,
    ClientcorsoriginmodalComponent,
    ClientcorsoriginsComponent,
    ClientredirecturisComponent,
    ClientredirecturimodalComponent,
    ClientpostlogoutredirecturimodalComponent,
    ClientpostlogoutredirecturisComponent,
    LoginComponent,
    LogoutComponent
  ],
  entryComponents: [
    ConfirmationmodalComponent,
    ClientcreatemodalComponent,
    ClientscopemodalComponent,
    ClientcorsoriginmodalComponent,
    ClientredirecturimodalComponent,
    ClientpostlogoutredirecturimodalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    StoreModule.forRoot({ 
      clients: clientsReducer,
      grants: grantsReducer,
      clientsPostLogoutRedirectURIs: clientsPostLogoutRedirectURIsreducer,
      clientsRedirectURIs: clientsRedirectURIsreducer,
      clientsCORSOrigins: clientsCORSOriginsreducer,
      clientsScopes: clientsScopesreducer
    }),
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: Httpinterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
