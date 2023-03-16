import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

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

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit()
  {
    this.verifyToken();
  }

  verifyToken()
  {
    this.data.token = this.router.url.split('/')[2].replace("verification?token=","");
    this.password.token = this.data.token;
    console.log(this.data);
    this.authService.verifyToken(this.data).subscribe((result: any) => {
      document.getElementById('verify-prosumer-mail')!.innerText=result.body.data;
    }, (error: any) => {
      console.log(error)
    });
  }

  verify()
  {
    this.authService.verify(this.password).subscribe((result: any) => {
      this.router.navigateByUrl('/');
    }, (error: any) => {
      console.log(error)
    })
  }
}