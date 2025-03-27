// firebase-messaging-sw.js
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging-compat.js');

firebase.initializeApp({
  apiKey: 'AIzaSyAP6PcXz0D61h9BLZ0YnHF7AbxQpAKtiyk',
  authDomain: 'smart-doorbell-8cbc8.firebaseapp.com',
  projectId: 'smart-doorbell-8cbc8',
  storageBucket: 'smart-doorbell-8cbc8.firebasestorage.app',
  messagingSenderId: '285763276979',
  appId: '1:285763276979:web:5b72114ea4cd53dff660c6',
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function(payload) {
  console.log('[firebase-messaging-sw.js] Received background message ', payload);
  const notificationTitle = payload.notification.title;
  const notificationOptions = {
    body: payload.notification.body,
    icon: '/assets/icons/icon-72x72.png'
  };

  self.registration.showNotification(notificationTitle, notificationOptions);
});

