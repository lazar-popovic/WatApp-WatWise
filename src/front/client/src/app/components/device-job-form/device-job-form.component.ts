import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DeviceSchedulerService } from 'src/app/services/device-scheduler.service';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-device-job-form',
  templateUrl: './device-job-form.component.html',
  styleUrls: ['./device-job-form.component.css']
})
export class DeviceJobFormComponent implements OnInit {

  @Input() deviceId: number = 0;
  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  newJob = {
    deviceId: 0,
    startDate: "",
    endDate: "",
    turn: true,
    repeat: true,
  }

  devices = [] as any[];

  busyAddDeviceJob: Subscription | undefined;

  constructor( private deviceService: DeviceService,
               private jwtService: JWTService,
               private toastrService: ToastrNotifService,
               private route: Router,
               private deviceScheduler: DeviceSchedulerService) { }

  ngOnInit(): void {
    this.getDevices();
  }

  getDevices() {
    this.deviceService.getDevicesIdAndNameByUserId( this.jwtService.userId).subscribe( (result:any) => {
      if( result.success) {
        this.devices = result.data;
        if( this.devices.length > 0) {
          this.newJob.deviceId = this.devices[0].id;
          if( this.deviceId > 0) {
            this.newJob.deviceId = this.deviceId;
            (document.querySelector('#device') as HTMLInputElement).disabled = true;
          }
        }
      }
    }, ( error: any) => {
      console.log( error);
    });
  }

  addDeviceJob() {
    this.newJob.startDate = (document.querySelector('#start-date') as HTMLInputElement).value + ":00.000Z";
    this.newJob.endDate = (document.querySelector('#end-date') as HTMLInputElement).value + ":00.000Z";
    console.log( this.newJob);

    this.busyAddDeviceJob = this.deviceScheduler.insertDeviceJob( this.newJob).subscribe( (result:any) => {
      this.toastrService.showSuccess( "Device job successfully scheduled!");
      this.route.navigateByUrl("prosumer/device/"+this.newJob.deviceId);
    }, (error:any) => {
      this.toastrService.showErrors(["Error while creating job. Check all fields!"]);
      console.log( error);
    })
  }

  refresh() {
    this.output.emit(false);
    console.log( false);
  }
}
