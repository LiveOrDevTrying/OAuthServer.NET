import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from 'src/app';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string;
  password: string;
  message: string;

  constructor(private http: HttpService,
    private authService: AuthService,
    private router: Router) { }

  login() {
    const request: ILogin = {
      username: this.username,
      password: this.password
    }

    this.http.login(request)
      .subscribe((response: IToken) => {
        if (response) {
          this.authService.setToken(response);
          this.router.navigate(['/']);
        } else {
          this.message = "Not a successful login. Please try again.";
        }
      });
  }
}
