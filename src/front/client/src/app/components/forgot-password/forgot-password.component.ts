import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  mail: any = {
    email: ''
  };

  constructor(private authService: AuthService, private router: Router) { }

  sendForm()
  {
    this.authService.forgotPassword(this.mail).subscribe((result: any) => {
      this.router.navigateByUrl('forgot-password-reset');
    },(error: any) => {
      console.log(error);
    })
  }
}
