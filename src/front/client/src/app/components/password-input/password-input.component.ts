import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  styleUrls: ['./password-input.component.css']
})
export class PasswordInputComponent {
  data: any = {
    token: '',
    newPassword: '',
    confirmedNewPassword: ''
  }

  constructor(private authService: AuthService, private router: Router) { }

  restartPassword()
  {
    this.data.token = this.router.url.split('/')[2].replace("verification?token=","");
    this.authService.resetPassword(this.data).subscribe((result: any) => {
      this.router.navigateByUrl('');
    }, (error: any) => {
      console.log(error);
    })
  }
  
}
