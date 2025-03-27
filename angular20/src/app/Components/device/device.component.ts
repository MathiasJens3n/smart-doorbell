import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceService } from '../../Services/device.service';
import { Device } from '../../Interfaces/device';

@Component({
  selector: 'app-device',
  standalone: false,
  templateUrl: './device.component.html',
  styleUrl: './device.component.css'
})
export class DeviceComponent implements OnInit {
  DeviceArray: Device[] = []; // Store devices
  paginatedDevices: Device[] = []; // Devices to display on the current page
  currentPage: number = 1; // Current page
  pageSize: number = 10; // Number of items per page
  totalPages: number = 0; // Total number of pages

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {
    this.deviceService.GetDevices().subscribe({
      next: (device) => {
        // Process the device data and calculate pagination
        this.DeviceArray = device.map(device => ({
          id: device.id,
          name: device.name,
          registration_Code: device.registrationcode
        }));
        this.totalPages = Math.ceil(this.DeviceArray.length / this.pageSize);
        this.updatePaginatedDevices(); // Update the displayed devices
        console.log('Devices fetched:', this.DeviceArray);
      },
      error: (error) => {
        console.error('Error fetching devices:', error);
      }
    });
  }

  // Update paginated devices based on the current page
  updatePaginatedDevices(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedDevices = this.DeviceArray.slice(startIndex, endIndex);
  }

  // Go to the previous page
  goToPreviousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedDevices();
    }
  }

  // Go to the next page
  goToNextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedDevices();
    }
  }

  // Navigate to gallery
  Gallery() {
    this.router.navigate(['/gallery']);
  }
}