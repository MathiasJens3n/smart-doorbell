import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Login } from '../Interfaces/login';
import { Fcm } from '../Interfaces/fcm';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  url: string = "https://37.27.27.136/auth";
  endpointLogin: string = "login"; // API endpoint
  url2: string = "https://37.27.27.136/user";
  endpoint2: string = "register-fcm-token"; // API endpoint

  constructor(private httpClient: HttpClient) {}

  Login(username: string, password: string): Observable<Login> {
    const loginData = { username, password };

    // Send a POST request to the API for login
    return this.httpClient.post<Login>(`${this.url}/${this.endpointLogin}`, loginData)
      .pipe(
        // Handle the response and store the JWT token in session or local storage
        tap((response: any) => {
          if (response.token) {
            // Store the JWT token in session storage
            sessionStorage.setItem('Token', response.token);
          }
        })
      );
  }

  // Register the FCM token after a successful login
  registerFCMn(): Observable<Fcm> {
    const jwt = sessionStorage.getItem('Token');
    if (!jwt) {
      throw new Error('Token not found in sessionStorage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${jwt}`
    });

    const token = sessionStorage.getItem('FCM');
    if (!token) {
      throw new Error('FCM token not found in sessionStorage');
    }

    const fcmtoken = { token };

    // Send a POST request to register the FCM token
    return this.httpClient.post<Fcm>(`${this.url2}/${this.endpoint2}`, fcmtoken, { headers });
  }
}
