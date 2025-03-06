import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { CreateAccountService } from '../../Service/create-account.service';
import { Account } from '../../Interfaces/account';


@Component({
  selector: 'app-create-account',
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
    });
  }

  onSubmit() {
    if (this.accountForm.invalid) {
      return;
    }

    const username = this.accountForm.controls['username'].value;
    const password = this.accountForm.controls['password'].value;
    const password2 = this.accountForm.controls['password2'].value;

    const self = this; // Store a reference to the component context

    const observer: Observer<Account> = {
      next(response: Account) {
        // Handle successful account response here
        console.log('account successful', response);

        // Trigger navigation to protected route after successful account
        self.router.navigate(['/Components/']);
      },
      error(error: any) {
        // Handle account error here
        console.error('account error', error);
      },
      complete() {
      }
    };

    this.createAccountService.postAccount(username, password).subscribe(observer);

  }
}
