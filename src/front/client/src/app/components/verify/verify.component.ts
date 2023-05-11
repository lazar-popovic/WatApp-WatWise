import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth-service.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.css']
})
export class VerifyComponent implements OnInit {
  password: any = {
    token: '',
    password1 : '',
    password2 : ''
  };

  login: any = {
    email: '',
    password: ''
  };

  data: any = {
    token: '',
  }

  busy: Subscription | undefined;

  constructor(private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService, private jwtService: JWTService) { }

  ngOnInit()
  {
    this.verifyToken();
  }

  verifyToken()
  {
    this.data.token = this.router.url.split('/')[1].replace("verification?token=","");
    this.password.token = this.data.token;
    this.authService.verifyToken(this.data).subscribe((result: any) => {
      if( result.body.success) {
        document.getElementById('verify-prosumer-mail')!.innerText = this.login.email = result.body.data.message;
        this.toastrNotifService.showSuccess("Token is valid!");
      } else {
        this.toastrNotifService.showErrors(["Token is invalid!"]);
        this.router.navigateByUrl('/');
      }
    }, (error: any) => {
      console.log(error)
    });
  }

  verify()
  {
    this.busy = this.authService.verify(this.password).subscribe((result: any) => {
      if( result.body.success) {
        this.login.password = this.password.password1;
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.authService.login(this.login).subscribe((result: any) => {
          if( result.body.success) {
            localStorage.setItem("token", result.body.data.token);
            this.jwtService.setToken();
            if( this.jwtService.roleId == 3) {
              this.router.navigateByUrl('/prosumer/overview');
            }
            else {
              this.router.navigateByUrl('/dso/overview');
            }
          }
        }, (error : any) => {
          console.log(error)
        })
        this.router.navigateByUrl('/');
      } else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    }, (error: any) => {
      console.log(error)
    })
  }
}
