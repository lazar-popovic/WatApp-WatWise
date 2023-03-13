import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']

  
})
export class LoginComponent {

  imagePath: string = 'assets/logo.png';
  
  imageUrl: SafeResourceUrl;
  constructor(private sanitizer: DomSanitizer) {
    this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.imagePath);
  }

  onSubmit() {
   
  }
}
