import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Device } from 'src/app/Models/device';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-devices-info',
  templateUrl: './devices-info.component.html',
  styleUrls: ['./devices-info.component.css']
})
export class DevicesInfoComponent implements OnInit{
  consumeShow: boolean = false;
  prosumeShow: boolean = false;
  storageShow: boolean = false;

  consumeDevices: Device[] = [];
  prosumeDevices: Device[] = [];
  storageDevices: Device[] = [];



 constructor( private deviceService: DeviceService, private jwtService: JWTService, private toastrService: ToastrNotifService, private route: Router) { }

  ngOnInit(): void {
    this.deviceService.getDevicesByUserId( this.jwtService.userId).subscribe(
      result => {
        if( result.success) {
          console.log(result.data);
          for(let device of result.data[0].devices) {
            let deviceIns = new Device();
            deviceIns.id = device.id;
            deviceIns.name = device.name;
            deviceIns.activityStatus = device.activityStatus;
            deviceIns.usage = device.value;
            if(device.id == 1) {
              this.consumeDevices.push(deviceIns)
            } else if(device.id == 2) {
              this.prosumeDevices.push(deviceIns);
            } else {
              this.storageDevices.push(deviceIns);
            }
          }
        }
        else {
          console.log( result.errors);
        }
      }, error => {
        console.log( error);
      }
    );
  }
  refresh() {
    this.route.navigateByUrl('/prosumer/devices');
  }

  toDevice( id: number) {
    this.route.navigate(['/prosumer/device',id]);
  }
}
