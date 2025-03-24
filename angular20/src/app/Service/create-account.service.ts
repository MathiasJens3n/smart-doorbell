import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../Interfaces/account';

@Injectable({
  providedIn: 'root'
})
export class CreateAccountService {
  url: string = "http://localhost:5005/auth";
  endpoint: string = "register"; // API endpoint

  constructor(private httpClient: HttpClient) {}

  AddUser(username: string, password: string): Observable<Account> {
    const AccountData = { username, password };

    // Send a POST so the api run user creation aka account.
    return this.httpClient.post<Account>(`${this.url}/${this.endpoint}`, AccountData)

  }
}