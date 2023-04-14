import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService } from 'src/app/services/device.service';

@Component({
  selector: 'app-delete-device',
  templateUrl: './delete-device.component.html',
  styleUrls: ['./delete-device.component.css']
})
export class DeleteDeviceComponent {
  @Output() exitStatusEvent = new EventEmitter<boolean>();
  @Input() id: number = 0;

  constructor(private deviceService: DeviceService, private router: Router) { }

  deleteDevice() {
    this.deviceService.deleteDevice(this.id).subscribe((result: any) => {
      this.router.navigateByUrl('prosumers/devices');
    }, (error: any) => {
      console.log(error);
  });
  }

  hideForm() {
    this.exitStatusEvent.emit(true);
  }
}
