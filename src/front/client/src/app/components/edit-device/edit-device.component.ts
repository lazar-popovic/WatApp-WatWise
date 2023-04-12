import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';

@Component({
  selector: 'app-edit-device',
  templateUrl: './edit-device.component.html',
  styleUrls: ['./edit-device.component.css']
})
export class EditDeviceComponent implements OnInit{
  @Input() id: number = 0;
  @Input() privacyStatus: boolean = false;
  @Output() exitStatusEvent = new EventEmitter<boolean>();

  device: any = {
    name: '',
    dataShare: false
  }

  constructor(private deviceService: DeviceService, private router: Router) { }

  ngOnInit(): void {
    if(this.privacyStatus == true)
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is visible to DSO."
    else
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is not visible to DSO."
  }

  status(event: boolean) {
    this.device.dataShare = event;
    if(this.privacyStatus == true) {
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is visible to DSO.";
    } else if(this.privacyStatus == false) {
      (document.querySelector('#slider-text') as HTMLDivElement).innerText = "Device is not visible to DSO.";
    }
  }

  saveChanges() {
    this.deviceService.updateDevice(this.id, this.device).subscribe((result: any) => {
      this.router.navigateByUrl('/prosumer/devices');
    }, (error: any) => {
      console.log(error);
    })
  }

  hideForm() {
    this.exitStatusEvent.emit(true);
  }
}
