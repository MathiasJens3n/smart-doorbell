import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Login } from '../Interfaces/login';
  
@Injectable({
  providedIn: 'root'
})
export class LoginService {
  url: string = "http://localhost:5005/auth";
  endpointLogin: string = "login"; // API endpoint

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
}