import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observer } from 'rxjs';
import { Router } from '@angular/router';
import { ImageGalleryService } from '../../Service/image-gallery.service';
import { Account } from '../../Interfaces/account';
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

  constructor(private imageGalleryService: ImageGalleryService) {}

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

    // Fetch images and store them in ImageArray
    this.imageGalleryService.GetImages().subscribe({
      next: (images) => {
        // Assuming 'images' is an array of Image objects returned from the API
        this.ImageArray = images.map(image => ({
          id: image.id,
          data: image.data,
          insert_Date: image.insert_Date,
          user_Id: image.user_Id
        }));
        console.log('Images fetched:', this.ImageArray);
      },
      error: (error) => {
        console.error('Error fetching images:', error);
      }
    });
  }
}