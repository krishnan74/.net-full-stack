import { Routes } from '@angular/router';
import { TeacherList } from './features/teacher/components/teacher-list/teacher-list';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { AuthComponent } from './features/auth/auth';
import { Quiz } from './features/quiz/quiz';
import { Landing } from './features/landing/landing';
import { QuizList } from './features/quiz/components/quiz-list/quiz-list';
import { ProfileComponent } from './features/profile/profile';
import { DashboardComponent } from './features/dashboard/dashboard';

export const routes: Routes = [
  {
    path: '',
    component: Landing,
  },
  {
    path: 'teachers',
    component: TeacherList,
  },
  {
    path: 'quiz/:id',
    component: Quiz,
  },
  {
    path: 'quizzes',
    component: QuizList,
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
  { path: 'profile', component: ProfileComponent },
  { path: 'dashboard', component: DashboardComponent },
];
