import { Component } from '@angular/core';
import { Device } from '../../Models/device';
import { OnInit } from '@angular/core';



@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit
{

    showDevices : boolean = true;

    devices: Device[] = [];
    newDevice: Device = new Device('',0,'', '');

   constructor() { }

    /*
    devices: Device[] = [
      { id: 1, name: "Fridge", type: "Type 1", category: "consumer" },
      { id: 2, name: "Device 2", type: "Type 2", category: "prosumer" },
      { id: 3, name: "Device 3", type: "Type 3", category: "storage" }
    ];+
    */

    ngOnInit(): void {
      const savedDevices = localStorage.getItem('devices');
      if (savedDevices) {
        this.devices = JSON.parse(savedDevices);
      }
    }

    addDevice() 
    {
      console.log(this.newDevice.category);
      this.newDevice.id = this.devices.length + 1;
      this.newDevice = new Device(this.newDevice.name, this.newDevice.consumption,this.newDevice.type, this.newDevice.category);
      this.devices.push(this.newDevice);
     
      localStorage.setItem('devices', JSON.stringify(this.devices));
      this.newDevice = new Device('',0,'','');
      this.showDevices = true;
    }

    removeDevice(device: Device) {
      const index = this.devices.indexOf(device);
      if (index !== -1) {
        this.devices.splice(index, 1);
        localStorage.setItem('devices', JSON.stringify(this.devices));
      }
    }
}

