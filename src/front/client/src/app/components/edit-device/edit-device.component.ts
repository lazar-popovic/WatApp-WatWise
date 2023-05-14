import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DeviceService } from 'src/app/services/device.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-edit-device',
  templateUrl: './edit-device.component.html',
  styleUrls: ['./edit-device.component.css']
})
export class EditDeviceComponent implements OnInit{
  @Input() id: number = 0;
  @Input() privacyStatus: boolean = false;
  @Output() exitStatusEvent = new EventEmitter<boolean>();

  @Input() device: any = {
    name: '',
    dataShare: false,
    dsoControl: false
  }

  busy: Subscription | undefined;

  constructor(private deviceService: DeviceService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  ngOnInit(): void {
  }

  saveChanges() {
    console.log( this.device);
    this.busy = this.deviceService.updateDevice(this.id, this.device).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess(result.body.data);
        window.location.reload();
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    }, (error: any) => {
      console.log(error);
    })
  }

  hideForm() {
    this.exitStatusEvent.emit(true);
  }
}
