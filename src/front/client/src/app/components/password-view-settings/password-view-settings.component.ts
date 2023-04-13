import { Component, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-password-view-settings',
  templateUrl: './password-view-settings.component.html',
  styleUrls: ['./password-view-settings.component.css']
})
export class PasswordViewSettingsComponent {
  @Output() dataEvent = new EventEmitter<any>();
  data: any = {
    oldPassword: '',
    newPassword: '',
    confirmedPassword: ''
  }

  emitData()
  {
    this.dataEvent.emit(this.data);
  }
}
