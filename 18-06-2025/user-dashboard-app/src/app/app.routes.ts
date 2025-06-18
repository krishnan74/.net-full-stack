import { Routes } from '@angular/router';
import { UserForm } from './user-form/user-form';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
  { path: 'add-user', component: UserForm },
  {
    path: 'dashboard',
    component: Dashboard,
  },
];
