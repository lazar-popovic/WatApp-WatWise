import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-profile-view-settings',
  templateUrl: './profile-view-settings.component.html',
  styleUrls: ['./profile-view-settings.component.css']
})
export class ProfileViewSettingsComponent {
  
  @Output() profileDataEvent = new EventEmitter<any>();
  
  data: any = {
    email: '',
    firstname: '',
    lastname: ''
  }

  emitData()
  {
    this.profileDataEvent.emit(this.data);
  }
}
