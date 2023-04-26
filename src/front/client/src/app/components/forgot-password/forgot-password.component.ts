import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';
import {ToastrNotifService} from "../../services/toastr-notif.service";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  mail: any = {
    email: ''
  };

  busy: Subscription | undefined;

  constructor(private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  sendForm()
  {
    this.busy = this.authService.forgotPassword(this.mail).subscribe((result: any) => {
      if( result.body.success)
      {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.router.navigateByUrl("");
      }
      else
      {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    },(error: any) => {
      console.log(error);
    })
  }
}
