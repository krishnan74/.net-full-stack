import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalr.service';
import { CommonModule } from '@angular/common';
import { QuizNotificationModel } from './models/notification.model';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.html',
  imports: [CommonModule],
  styleUrls: ['./notification.css'],
})
export class NotificationComponent implements OnInit {
  notifications: QuizNotificationModel[] = [];

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.notifications$.subscribe((notifications) => {
      this.notifications = notifications;
    });
  }
}
