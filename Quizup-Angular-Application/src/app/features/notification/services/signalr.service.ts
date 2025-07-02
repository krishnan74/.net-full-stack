import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { QuizNotificationModel } from '../models/notification.model';

@Injectable()
export class SignalRService {
  private connection: signalR.HubConnection;
  private notificationsSubject = new BehaviorSubject<any[]>([]);
  notifications$ = this.notificationsSubject.asObservable();

  constructor() {
    console.log('Initializing SignalR Service');
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5166/quizNotificationHub', {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on('NotifyStartQuiz', (quiz) => {
      console.log('Quiz started:', quiz);
      this.addNotification(quiz);
    });

    this.connection.on('NotifyEndQuiz', (quiz) => {
      console.log('Quiz ended:', quiz);
      this.addNotification(quiz);
    });

    this.connection
      .start()
      .then(() => console.log('SignalR connection established'))
      .catch((err) => console.error('SignalR connection failed:', err));
  }

  private addNotification(notification: QuizNotificationModel) {
    const currentNotifications = this.notificationsSubject.value;
    this.notificationsSubject.next([...currentNotifications, notification]);
  }

  joinClassGroup(classGroupName: string): void {
    this.connection
      .invoke('JoinClassGroup', classGroupName)
      .then(() => console.log(`Joined class group: ${classGroupName}`))
      .catch((err) =>
        console.error(`Failed to join class group: ${classGroupName}`, err)
      );
  }
}
