import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';  // Ensure this routing module is imported
import { AppComponent } from './app.component';
import { CreateAccountComponent } from './Components/create-account/create-account.component';
import { LoginComponent } from './Components/login/login.component';
import { ImageGalleryComponent } from './Components/image-gallery/image-gallery.component';
import { DeviceComponent } from './Components/device/device.component'; // Declare components here
import { provideHttpClient } from '@angular/common/http';

//firebase shit
import { environment } from '../environments/environment';
import { ServiceWorkerModule } from '@angular/service-worker';
import { provideFirebaseApp, initializeApp } from '@angular/fire/app';
import { provideMessaging, getMessaging } from '@angular/fire/messaging';

@NgModule({
  declarations: [
    AppComponent,
    CreateAccountComponent,
    LoginComponent,
    ImageGalleryComponent,
    DeviceComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,  // Import the routing module
    ReactiveFormsModule, 
    ServiceWorkerModule.register('ngsw-worker.js', {
    enabled: !isDevMode(),

}),
    
  ],
  providers: [
    provideHttpClient(),
    provideFirebaseApp(() => initializeApp(environment.firebase)),
    provideMessaging(() => getMessaging())
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
