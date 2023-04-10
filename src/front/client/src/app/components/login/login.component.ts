import { Component } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import { ToastrNotifService} from "../../services/toastr-notif.service";
import {JWTService} from "../../services/jwt.service";

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

  constructor(private authService: AuthService, private route: Router, private toastrNotifService: ToastrNotifService, private jwtService: JWTService) { }

  logIn() {
      this.authService.login(this.login).subscribe((result: any) => {
        if( result.body.success) {
          localStorage.setItem("token", result.body.data.token);
          this.jwtService.setToken();
          console.log( this.jwtService.roleId);
          this.toastrNotifService.showSuccess("Login Successful!");
          if( this.jwtService.roleId == 3) {
            this.route.navigate(['/prosumer/overview']);
          }
          else {
            this.route.navigate(['/dso/overview']);
          }
        }
        else {
          this.toastrNotifService.showErrors( result.body.errors);
        }
      },(error: any) => {
        console.log(error.error.errors)
      })
  }
}
