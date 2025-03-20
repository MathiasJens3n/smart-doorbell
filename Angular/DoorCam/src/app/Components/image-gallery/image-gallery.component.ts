import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ImageGalleryService } from '../../Service/image-gallery.service';
import { Image } from '../../Interfaces/image';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.css']
})
export class ImageGalleryComponent implements OnInit {
  username: string = '';
  registration_Code: string = '';

  // Array of Image objects (with id and data)
  ImageArray: Image[] = [];
  sanitizedImageMap: { [key: string]: SafeUrl } = {};  //  Store sanitized images

  constructor(private imageGalleryService: ImageGalleryService, private router: Router, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
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
}