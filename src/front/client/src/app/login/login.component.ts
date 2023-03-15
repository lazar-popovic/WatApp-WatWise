import { Component } from '@angular/core';
import { AuthService } from '../services/auth-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  login: any = {
    username : '',
    password : ''
  };

  constructor(private authService: AuthService, private route: Router) { }

  logIn() {
      this.authService.loginProsumer(this.login).subscribe((result: any) => {
        console.log(result.status);
        localStorage.setItem("token",result.token);
        this.route.navigateByUrl('/prosumer/overview');
      },(error: any) => {
        console.log(error.error.errors)
      })
  }
}
