import { Component } from '@angular/core';
import { Device } from '../../Models/device';
import { OnInit } from '@angular/core';
import {DeviceService} from "../../services/device.service";
import {JWTService} from "../../services/jwt.service";
import {ToastrNotifService} from "../../services/toastr-notif.service";
import {Router} from "@angular/router";



@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit
{
    showDevices : boolean = true;

    newDevice = {
      userId: 0,
      name: "",
      deviceTypeId: 0
    }
    category: number = -1;
    types: any[] = [];

    userDevices = {
      consumers: [] as any[],
      storages: [] as any[],
      producers: [] as any[]
    }
   constructor( private deviceService: DeviceService, private jwtService: JWTService, private toastrService: ToastrNotifService, private route: Router) { }

    ngOnInit(): void {
      console.log( this.jwtService.roleId + " " + this.jwtService.userId);
      this.deviceService.getDevicesByUserId( this.jwtService.userId).subscribe(
        result => {
          if( result.success) {
            this.userDevices.consumers = result.data.filter((obj:any) => obj.category === -1).map((obj:any) => obj.devices)[0];
            this.userDevices.producers = result.data.filter((obj:any) => obj.category === 1).map((obj:any) => obj.devices)[0];
            this.userDevices.storages = result.data.filter((obj:any) => obj.category === 0).map((obj:any) => obj.devices)[0];
          }
          else {
            console.log( result.errors);
          }
        }, error => {
          console.log( error);
        }
      );
      this.fillTypes();
    }

    fillTypes() {
      this.deviceService.getDeviceTypesByCategory( this.category).subscribe(
        result => {
          this.types = result.data;
          this.newDevice.deviceTypeId = 0;
          this.newDevice.deviceTypeId = this.types[0].id;
        }, error => {
          console.log( error.errors);
        }
      )
    }

    addDevice()
    {
      this.newDevice.userId = this.jwtService.userId;
      this.deviceService.insertDevice( this.newDevice).subscribe(
        result => {
          if( result.body.success)
          {
            this.toastrService.showSuccess( result.body.data);
          }
          else
          {
            this.toastrService.showErrors( result.body.errors);
          }
        }
      )
    }

    refresh() {
      this.route.navigateByUrl('/prosumer/devices');
    }
}

