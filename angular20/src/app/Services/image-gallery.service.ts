import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../Interfaces/account';

@Injectable({
  providedIn: 'root'
})
export class ImageGalleryService {
  url: string = "http://localhost:5000";
  endpoint1: string = "user"; // API endpoint for user
  endpoint2: string = "image"; // Add an endpoint for images

  constructor(private httpClient: HttpClient) {}

  Getuser(): Observable<Account> {
    const token = sessionStorage.getItem('Token');
    if (!token) {
      throw new Error('Token not found in sessionStorage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.httpClient.get<Account>(`${this.url}/${this.endpoint1}`, { headers });
  }

  // This will return the list of images
  GetImages(): Observable<any[]> {
    const token = sessionStorage.getItem('Token');
    if (!token) {
      throw new Error('Token not found in sessionStorage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Send a GET request to fetch images
    return this.httpClient.get<any[]>(`${this.url}/${this.endpoint2}`, { headers });
  }


  UpdateUser(username: string, password: string): Observable<Account> {
    const AccountData = { username, password };

    const token = sessionStorage.getItem('Token');
    if (!token) {
      throw new Error('Token not found in sessionStorage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    // Send a put so the api run user creation aka account.
    return this.httpClient.put<Account>(`${this.url}/${this.endpoint1}`, AccountData, { headers })

  }
}