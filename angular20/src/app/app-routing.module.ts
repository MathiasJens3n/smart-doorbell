import { NgModule } from '@angular/core';
import { RouterModule, Routes, CanActivateFn, Router } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { CreateAccountComponent } from './Components/create-account/create-account.component';
import { ImageGalleryComponent } from './Components/image-gallery/image-gallery.component';
import { DeviceComponent } from './Components/device/device.component';

export const checkAuth: CanActivateFn = () => {
  return !!sessionStorage.getItem('Token'); // Returns true if 'Token' exists, otherwise false
};


const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'create-account', component: CreateAccountComponent },
  {
    path: 'components',
    canActivate: [checkAuth],  // Use function to protect routes
    children: [
      { path: 'gallery', component: ImageGalleryComponent },
      { path: 'device', component: DeviceComponent },
    ],
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },  // Default to login if no route is matched
  { path: '**', redirectTo: 'login' },  // Redirect invalid routes to login
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
