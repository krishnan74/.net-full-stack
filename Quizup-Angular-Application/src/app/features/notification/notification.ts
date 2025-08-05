import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalr.service';
import { CommonModule } from '@angular/common';
import { QuizNotificationModel } from './models/notification.model';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.html',
  imports: [CommonModule],
  styleUrls: ['./notification.css'],
})
export class NotificationComponent implements OnInit {
  notifications: QuizNotificationModel[] = [];

  constructor(
    private signalRService: SignalRService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.signalRService.notifications$.subscribe({
      next: (notifications) => {
        const previousCount = this.notifications.length;
        this.notifications = notifications;

        // Show toast for new notifications
        if (notifications.length > previousCount) {
          const newNotifications = notifications.slice(previousCount);
          newNotifications.forEach((notification) => {
            this.toastService.showInfo(
              notification.type === 'start' ? 'Quiz Started' : 'Quiz Ended',
              notification.title
            );
          });
        }
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.toastService.showError('Loading Notifications', error);
      },
    });
  }
}
