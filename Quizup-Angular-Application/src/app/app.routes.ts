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
import { NotificationComponent } from './features/notification/notification';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { StudentsListComponent } from './features/admin/students-list/students-list.component';
import { TeachersListComponent } from './features/admin/teachers-list/teachers-list.component';
import { SubjectsListComponent } from './features/admin/subjects-list/subjects-list.component';
import { ClassesListComponent } from './features/admin/classes-list/classes-list.component';

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
    path: 'quiz/:quizId/attempt/:submissionId',
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
  { path: 'notifications', component: NotificationComponent },
  {
    path: 'admin',
    children: [
      { path: '', component: AdminDashboardComponent },
      { path: 'students', component: StudentsListComponent },
      { path: 'teachers', component: TeachersListComponent },
      { path: 'subjects', component: SubjectsListComponent },
      { path: 'classes', component: ClassesListComponent },
    ],
  },
];
