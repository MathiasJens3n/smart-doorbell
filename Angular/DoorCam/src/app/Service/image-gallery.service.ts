import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../Interfaces/account';

@Injectable({
  providedIn: 'root'
})
export class ImageGalleryService {
  url: string = "http://localhost:5005";
  endpoint: string = "user"; // API endpoint

  constructor(private httpClient: HttpClient) {}

  Getuser(): Observable<Account> {
    // Retrieve the JWT token from sessionStorage
    const token = sessionStorage.getItem('Token');
    
    // Check if the token exists
    if (!token) {
      throw new Error('Token not found in sessionStorage');
    }

    // Set up headers with the Authorization Bearer token
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Send a GET request with the Authorization header
    return this.httpClient.get<Account>(`${this.url}/${this.endpoint}`, { headers });
  }
}
