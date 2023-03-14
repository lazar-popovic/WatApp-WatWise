import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  imagePath: string = 'assets/logo.png';
  login: any = {
    username : '',
    password : ''
  };

  imageUrl: SafeResourceUrl;
  constructor(private sanitizer: DomSanitizer, private authService: AuthService) {
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.imagePath);

  }

  logIn() {
      this.authService.login(this.login).subscribe((result: any) => {
        console.log(result);
      });
  }

  onSubmit() {
   
  }
}
