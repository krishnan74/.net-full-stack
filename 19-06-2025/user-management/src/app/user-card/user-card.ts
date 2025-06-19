import { Component, Input } from '@angular/core';
import { UserModel } from '../features/users/user.model';

@Component({
  selector: 'app-user-card',
  imports: [],
  templateUrl: './user-card.html',
  styleUrl: './user-card.css',
})
export class UserCard {
  @Input() user: UserModel | null = new UserModel();
}