import { Component } from '@angular/core';
import { Device } from '../../Models/device';
import { OnInit } from '@angular/core';
import {DeviceService} from "../../services/device.service";
import {JWTService} from "../../services/jwt.service";
import {ToastrNotifService} from "../../services/toastr-notif.service";



@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit
{

    showDevices : boolean = true;

    devices: Device[] = [];
    newDevice = {
      userId: 0,
      name: "",
      deviceTypeId: 0
    }
    category: number = -1;
    types: any[] = [];
   constructor( private deviceService: DeviceService, private jwtService: JWTService, private toastrService: ToastrNotifService) { }

    ngOnInit(): void {
      const savedDevices = localStorage.getItem('devices');
      if (savedDevices) {
        this.devices = JSON.parse(savedDevices);
      }
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
}

