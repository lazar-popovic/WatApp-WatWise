import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth-service.service';
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

  data: any = {
    token: '',
  }

  busy: Subscription | undefined;

  constructor(private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  ngOnInit()
  {
    this.verifyToken();
  }

  verifyToken()
  {
    this.data.token = this.router.url.split('/')[1].replace("verification?token=","");
    this.password.token = this.data.token;
    console.log(this.data);
    this.authService.verifyToken(this.data).subscribe((result: any) => {
      if( result.body.success) {
        document.getElementById('verify-prosumer-mail')!.innerText=result.body.data.message;
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
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl('/');
      } else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    }, (error: any) => {
      console.log(error)
    })
  }
}
