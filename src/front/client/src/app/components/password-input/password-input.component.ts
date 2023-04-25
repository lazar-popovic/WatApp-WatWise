import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';
import {ToastrNotifService} from "../../services/toastr-notif.service";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  styleUrls: ['./password-input.component.css']
})
export class PasswordInputComponent implements OnInit {
  data: any = {
    token: '',
    newPassword: '',
    confirmedNewPassword: ''
  }

  token: any = {
    token: ''
  }

  busy: Subscription | undefined;

  constructor(private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  ngOnInit()
  {
    this.verifyToken();
  }

  verifyToken()
  {
    this.token.token = this.data.token = this.router.url.split('/')[2].replace("reset-password?token=","");
    this.authService.verifyToken(this.data).subscribe((result: any) => {
      if( result.body.success) {
        document.getElementById('password-input-mail')!.innerText=result.body.data.message;
        this.toastrNotifService.showSuccess( result.body.data.message);
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
        this.router.navigateByUrl('');
      }
    }, (error: any) => {
      console.log(error)
    });
  }

  restartPassword()
  {
    this.busy = this.authService.resetPassword(this.data).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl('');
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    }, (error: any) => {
      console.log(error);
    })
  }

}
