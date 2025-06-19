import { Routes } from '@angular/router';
import { UserForm } from './user-form/user-form';
import { Users } from './users/users';

export const routes: Routes = [
  { path: 'add-user', component: UserForm },
  {
    path: 'users',
    component: Users,
  },
];