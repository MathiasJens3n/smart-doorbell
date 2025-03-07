import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { ImageGalleryService } from '../../Service/image-gallery.service';
import { Account } from '../../Interfaces/account';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.css']
})
export class ImageGalleryComponent implements OnInit {
  username: string = ''; // Declare and initialize the username
  registration_Code: string = ''; // Declare and initialize the registration_Code
  
  // Example array for images, you can replace null with actual URLs
  ImageArray = [
    null, null, null, null, null,
    null, null, null, null, null,
    null, null, null, null, null,
    null, null, null, null, null,
    null, null, null, null, null
  ];

  constructor(
    private formBuilder: FormBuilder,
    private imageGalleryService: ImageGalleryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Create an observer to handle the response of the createAccountService
    const observer: Observer<Account> = {
      next: (response: Account) => {      
        // Save the username and registration code from the response
        this.username = response.username;
        this.registration_Code = response.registration_Code;
        
        // Optionally, log the variables to check
        console.log('Username:', this.username);
        console.log('Registration Code:', this.registration_Code);
      },
      error: (error: any) => {
        // Handle account error here
        console.error('Account error', error);
      },
      complete: () => {
        // Do something after the observable completes if needed
      }
    };
    
    // Subscribe to the service that fetches the account details
    this.imageGalleryService.Getuser().subscribe(observer);
  }
}
