import { Component } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  login: any = {
    email : '',
    password : ''
  };

  constructor(private authService: AuthService, private route: Router) { }

  logIn() {
      this.authService.login(this.login).subscribe((result: any) => {
        localStorage.setItem("token", result.body.data.token);
        this.route.navigateByUrl('/prosumer/overview');
      },(error: any) => {
        console.log(error.error.errors)
      })
  }
}
