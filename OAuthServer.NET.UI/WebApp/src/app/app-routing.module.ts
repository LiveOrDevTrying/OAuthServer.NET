import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientauthorizationcodeComponent } from './components/clientauthorizationcode/clientauthorizationcode.component';
import { ClientclientcredentialsComponent } from './components/clientclientcredentials/clientclientcredentials.component';
import { ClientimplicitComponent } from './components/clientimplicit/clientimplicit.component';
import { ClientresourceownerpasswordComponent } from './components/clientresourceownerpassword/clientresourceownerpassword.component';
import { ClientsComponent } from './components/clients/clients.component';
import { GrantsComponent } from './components/grants/grants.component';
import { GrantComponent } from './components/grant/grant.component';
import { ClientComponent } from './components/client/client.component';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { AuthGuard } from './auth/authGuard';


const routes: Routes = [
  { path: '', component: ClientsComponent, canActivate: [AuthGuard] },
  { path: 'grants', component: GrantsComponent, canActivate: [AuthGuard] },
  { path: 'grant/:id', component: GrantComponent, canActivate: [AuthGuard] },
  { path: 'grant', component: GrantComponent, canActivate: [AuthGuard] },
  { path: 'client/authorizationcode/:id', component: ClientauthorizationcodeComponent, canActivate: [AuthGuard] },
  { path: 'client/authorizationcode', component: ClientauthorizationcodeComponent, canActivate: [AuthGuard] },
  { path: 'client/implicit/:id', component: ClientimplicitComponent, canActivate: [AuthGuard] },
  { path: 'client/implicit', component: ClientimplicitComponent, canActivate: [AuthGuard] },
  { path: 'client/ropassword/:id', component: ClientresourceownerpasswordComponent, canActivate: [AuthGuard] },
  { path: 'client/ropassword', component: ClientresourceownerpasswordComponent, canActivate: [AuthGuard] },
  { path: 'client/clientcredentials/:id', component: ClientclientcredentialsComponent, canActivate: [AuthGuard] },
  { path: 'client/clientcredentials', component: ClientclientcredentialsComponent, canActivate: [AuthGuard] },
  { path: 'client/other/:id', component: ClientComponent, canActivate: [AuthGuard] },
  { path: 'client/other', component: ClientComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
