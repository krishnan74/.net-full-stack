import { Routes } from '@angular/router';
import { TeacherList } from './features/teacher/components/teacher-list/teacher-list';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { AuthComponent } from './features/auth/auth';
import { Quiz } from './features/quiz/quiz';

export const routes: Routes = [
  {
    path: 'teachers',
    component: TeacherList,
  },
  {
    path: 'quiz/:id',
    component: Quiz,
  },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: 'login', component: LoginComponent },
      {
        path: 'register',
        component: RegisterComponent,
      },
    ],
  },
];
