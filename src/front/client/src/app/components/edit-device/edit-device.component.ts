import { Component, Input, OnInit } from '@angular/core';
import { DeviceService } from 'src/app/services/device.service';

@Component({
  selector: 'app-edit-device',
  templateUrl: './edit-device.component.html',
  styleUrls: ['./edit-device.component.css']
})
export class EditDeviceComponent implements OnInit{
  @Input() id: number = 0;
  @Input() privacyStatus: boolean = false;

  device: any = {
    name: '',
    status: false
  }

  constructor(private deviceService: DeviceService) { }

  ngOnInit(): void {
    if(this.privacyStatus == true)
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is visible to DSO."
    else 
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is not visible to DSO."
  }

  status(event: boolean) {
    this.device.status = event;
    if(this.privacyStatus == true) {
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is visible to DSO.";
    } else if(this.privacyStatus == false) {
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is not visible to DSO.";
    }
  }

  saveChanges() {г
    this.deviceService.updateDevice(this.id, this.device);
  }
}
