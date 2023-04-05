import { Component } from '@angular/core';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.css']
})
export class ProfileSettingsComponent {

  wanted: number = 1;

  click(element: EventTarget | null) {
    if(element == null)
      return;
    
    let active = document.querySelector(".profile-settings-menu-active-item") as HTMLDivElement;

    if(active != null)
      active.className = "profile-settings-menu-item";

    if ((element as HTMLDivElement).className == "profile-settings-menu-item")
      (element as HTMLDivElement).className = "profile-settings-menu-active-item";
  }

  handler(event: MouseEvent) {
    this.click(event.target);
  }
}
