import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService } from '../../Service/device.service';
import { Device } from '../../Interfaces/device';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrl: './device.component.css'
})
export class DeviceComponent implements OnInit {;
  // Array of devices
  DeviceArray: Device[] = [];

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    // Fetch images and store them in ImageArray
    this.deviceService.GetDevices().subscribe({
      next: (device) => {
        // Assuming 'images' is an array of Image objects returned from the API
        this.DeviceArray = device.map(device => ({
          id: device.id,
          name: device.name,
          registration_Code: device.registrationcode
        }));
        console.log('Images fetched:', this.DeviceArray);
      },
      error: (error) => {
        console.error('Error fetching images:', error);
      }
    });
  }
}