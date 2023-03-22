import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

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

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit()
  {
    this.verifyToken();
  }

  verifyToken()
  {
    this.token.token = this.data.token = this.router.url.split('/')[2].replace("reset-password?token=","");
    this.authService.verifyToken(this.data).subscribe((result: any) => {
      document.getElementById('password-input-mail')!.innerText=result.body.data.message;
    }, (error: any) => {
      console.log(error)
    });
  }

  restartPassword()
  {
    this.authService.resetPassword(this.data).subscribe((result: any) => {
      this.router.navigateByUrl('');
    }, (error: any) => {
      console.log(error);
    })
  }
  
}
