import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  url: string = "https://37.27.27.136";
  endpoint1: string = "device"; // API endpoint for user

  constructor(private httpClient: HttpClient) {}

  // This will return the list of images
  GetDevices(): Observable<any[]> {
    const token = sessionStorage.getItem('Token');
    if (!token) {
      throw new Error('Token not found in sessionStorage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Send a GET request to fetch images
    return this.httpClient.get<any[]>(`${this.url}/${this.endpoint1}`, { headers });
  }
}