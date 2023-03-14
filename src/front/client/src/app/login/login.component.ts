import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  imagePath: string = 'assets/logo.png';
  login: any = {
    username : '',
    password : ''
  };

  imageUrl: SafeResourceUrl;
  constructor(private sanitizer: DomSanitizer, private authService: AuthService, private route: Router) {
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.imagePath);

  }

  logIn() {
      this.authService.login(this.login).subscribe((result: any) => {
        console.log(result.status);
        localStorage.setItem("token",result.token);
        this.route.navigateByUrl('/overview');
      },(error: any) => {
        console.log("Error: email or password not valid.")   
      })
  }
}
