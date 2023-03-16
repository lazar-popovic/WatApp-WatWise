import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-login-dso',
  templateUrl: './login-dso.component.html',
  styleUrls: ['./login-dso.component.css']
})
export class LoginDSOComponent {

  login: any = {
    username : '',
    password : ''
  };

  constructor(private authService: AuthService, private route: Router) { }

  logIn() {
      this.authService.loginProsumer(this.login).subscribe((result: any) => {
        console.log(result.status);
        localStorage.setItem("token",result.token);
        this.route.navigateByUrl('/dso/overview');
      },(error: any) => {
        console.log(error.error.errors)
      })
  }
}
