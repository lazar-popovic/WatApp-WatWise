import { Component } from '@angular/core';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {

  showProfileForm = true;
  dsoShow = true;
  user: User;
  constructor() {
    this.user = new User();
  }

}
