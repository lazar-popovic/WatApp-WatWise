import { Component } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import { ToastrNotifService} from "../../services/toastr-notif.service";

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

  constructor(private authService: AuthService, private route: Router, private toastrNotifService: ToastrNotifService) { }

  logIn() {
      this.authService.login(this.login).subscribe((result: any) => {
        if( result.body.success) {
          localStorage.setItem("token", result.body.data.token);
          this.route.navigate(['/prosumer/overview']);
        }
        else {
          this.toastrNotifService.showErrors( result.body.errors);
        }
      },(error: any) => {
        console.log(error.error.errors)
      })
  }
}
