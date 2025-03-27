import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { LoginService } from '../../Services/login.service';
import { Login } from '../../Interfaces/login';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private loginServices: LoginService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    const username = this.loginForm.controls['username'].value;
    const password = this.loginForm.controls['password'].value;

    const self = this; // Store a reference to the component context

    const observer: Observer<Login> = {
      next(response: Login) {
        // Handle successful login response here
        console.log('Login successful', response);
        self.router.navigate(['/components/gallery']);

      },
      error(error: any) {
        // Handle login error here
        console.error('Login error', error);
        // Optionally, show a login error message here
      },
      complete() {}
    };

    this.loginServices.Login(username, password).subscribe(observer);
  }

  Create_account() {
    this.router.navigate(['/create-account']);
  }
}
