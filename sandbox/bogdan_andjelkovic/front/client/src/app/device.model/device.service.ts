import { Injectable } from '@angular/core';
import {Device} from "./device";

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  devices: any;

  constructor() { }

  addDevice(device: any) {
    this.devices.push(device);
  }

  getDevices() {
    return this.devices;
  }
}
