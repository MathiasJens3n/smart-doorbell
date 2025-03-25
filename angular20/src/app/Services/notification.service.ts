import { Injectable, inject } from '@angular/core';
import { Messaging } from '@angular/fire/messaging';
import { getToken, onMessage } from 'firebase/messaging';
import { NgZone } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private messaging = inject(Messaging); // Properly inject Messaging service
  private ngZone = inject(NgZone); // Inject Angular Zone to run Firebase calls inside Angular context

  constructor() {}

  async requestPermission() {
    try {
      const permission = await Notification.requestPermission();
      if (permission !== 'granted') {
        console.warn('Permission not granted for notifications');
        return;
      }

      // Run Firebase API call inside Angular Zone to avoid hydration issues
      this.ngZone.runOutsideAngular(async () => {
        try {
          const token = await getToken(this.messaging, {
            vapidKey:
              'BBis51v6rrBSrcgi-TFOTHRYBHQJ131ruKEcLkEPY3NMzwX5ni7mXuKWOegnbMFDgZi1Ny_HL_QaGhR6GDYaLWA',
          });

          if (token) {
            console.log('FCM Token:', token);
            sessionStorage.setItem('FCM', token);
            // TODO: Send token to backend
          } else {
            console.warn('No registration token available.');
          }
        } catch (err) {
          console.error('An error occurred while retrieving token.', err);
        }
      });
    } catch (err) {
      console.error('Error requesting notification permission:', err);
    }
  }

  listenToMessages() {
    this.ngZone.runOutsideAngular(() => {
      onMessage(this.messaging, (payload) => {
        console.log('Message received in foreground:', payload);
        // Show a toast, badge, or notification UI
      });
    });
  }
}
