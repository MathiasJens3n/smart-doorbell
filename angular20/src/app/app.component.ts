import { Component, OnInit } from '@angular/core';
import { NotificationService } from './Services/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css',
  template: `<h1>Firebase Notifications</h1>`
})
export class AppComponent implements OnInit {
  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.notificationService.requestPermission();
    this.notificationService.listenToMessages();
  }
}
