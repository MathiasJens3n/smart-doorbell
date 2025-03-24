import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ImageGalleryService } from '../../Service/image-gallery.service';
import { Image } from '../../Interfaces/image';
import { Account } from '../../Interfaces/account';

@Component({
  selector: 'app-image-gallery',
  standalone: false,
  templateUrl: './image-gallery.component.html',
  styleUrl: './image-gallery.component.css'
})
export class ImageGalleryComponent implements OnInit {
  updateform!: FormGroup;
  username: string = '';
  registration_Code: string = '';
  isEditing: boolean = false; // Toggle state for editing mode

  // Array of Image objects (with id and data)
  ImageArray: Image[] = [];
  sanitizedImageMap: { [key: string]: SafeUrl } = {};  //  Store sanitized images

  constructor(private formBuilder: FormBuilder, private imageGalleryService: ImageGalleryService, private router: Router, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.updateform = this.formBuilder.group(
      {
        username: ['', Validators.required],
        password: ['', Validators.required],
        password2: ['', Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );


    // Fetch user information (if needed)
    this.imageGalleryService.Getuser().subscribe({
      next: (response) => {
        this.username = response.username;
        this.registration_Code = response.registration_Code;
      },
      error: (error) => {
        console.error('Error fetching user:', error);
      }
    });

    // Fetch images
    this.imageGalleryService.GetImages().subscribe({
      next: (images) => {
        this.ImageArray = images;

        // Sanitize each Base64 image URL
        this.ImageArray.forEach(image => {
          if (image.data) {
            this.sanitizedImageMap[image.id] = this.sanitizer.bypassSecurityTrustUrl(
              `data:image/jpeg;base64,${image.data}`
            );
          }
        });

        console.log('Sanitized Image Map:', this.sanitizedImageMap);
      },
      error: (error) => console.error('Error fetching images:', error)
    });
  }

  Device() {
    this.router.navigate(['/components/device']);
  }

  Edit_toggle(enable: boolean) {
    this.isEditing = enable;
    if (!enable) {
      this.updateform.reset({ username: this.username, password: '', password2: '' });
    }
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
      if (this.updateform.invalid) {
        return;
      }
  
      const username = this.updateform.controls['username'].value;
      const password = this.updateform.controls['password'].value;
      const password2 = this.updateform.controls['password2'].value;
  
      // Check if passwords match before sending request
      if (password !== password2) {
        this.updateform.controls['password2'].setErrors({ mismatch: true });
        return;
      }
  
      const self = this; // Store a reference to the component context
  
      const observer: Observer<Account> = {
        next(response: Account) {
          // Handle successful account response here
          console.log('account successful', response);

          window.location.reload();
        },
        error(error: any) {
          // Handle account error here
          console.error('account error', error);
        },
        complete() {
        }
      };
  
      this.imageGalleryService.UpdateUser(username, password).subscribe(observer);
    }
}