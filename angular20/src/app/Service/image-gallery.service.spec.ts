import { TestBed } from '@angular/core/testing';

import { ImageGalleryService } from './image-gallery.service';

describe('ImageGalleryService', () => {
  let service: ImageGalleryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImageGalleryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
