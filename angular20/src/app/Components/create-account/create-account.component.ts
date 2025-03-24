import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { CreateAccountService } from '../../Service/create-account.service';
import { Account } from '../../Interfaces/account';

@Component({
  selector: 'app-create-account',
  standalone: false,
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.css'
})
export class CreateAccountComponent {
  accountForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private createAccountService: CreateAccountService, private router: Router) { }

  ngOnInit(): void {
    this.accountForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      password2: ['', Validators.required]
    }, { validator: this.passwordMatchValidator }); // Apply custom validator for password matching
  }

  // Custom validator to check if passwords match
  passwordMatchValidator(control: AbstractControl) {
    const password = control.get('password')?.value;
    const password2 = control.get('password2')?.value;

    if (password !== password2) {
      control.get('password2')?.setErrors({ mismatch: true });
    } else {
      control.get('password2')?.setErrors(null);
    }
  }

  onSubmit() {
    if (this.accountForm.invalid) {
      return;
    }

    const username = this.accountForm.controls['username'].value;
    const password = this.accountForm.controls['password'].value;
    const password2 = this.accountForm.controls['password2'].value;

    // Check if passwords match before sending request
    if (password !== password2) {
      this.accountForm.controls['password2'].setErrors({ mismatch: true });
      return;
    }

    const self = this; // Store a reference to the component context

    const observer: Observer<Account> = {
      next(response: Account) {
        // Handle successful account response here
        console.log('account successful', response);

        // Trigger navigation to protected route after successful account
        self.router.navigate(['/login']);
      },
      error(error: any) {
        // Handle account error here
        console.error('account error', error);
      },
      complete() {
      }
    };

    this.createAccountService.AddUser(username, password).subscribe(observer);
  }

  Login() {
    this.router.navigate(['/login']);
  }
}