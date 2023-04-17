import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DeviceService } from 'src/app/services/device.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-delete-device',
  templateUrl: './delete-device.component.html',
  styleUrls: ['./delete-device.component.css']
})
export class DeleteDeviceComponent {
  @Output() exitStatusEvent = new EventEmitter<boolean>();
  @Input() id: number = 0;

  busy: Subscription | undefined;

  constructor(private deviceService: DeviceService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  deleteDevice() {
    this.busy = this.deviceService.deleteDevice(this.id).subscribe((result: any) => {
      console.log( result);
      if( result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data);
        this.router.navigate(['/prosumer/devices']);
      }
    }, (error: any) => {
      console.log(error);
  });
  }

  hideForm() {
    this.exitStatusEvent.emit(true);
  }
}
