import { Routes } from '@angular/router';
import { TeacherList } from './features/teacher/components/teacher-list/teacher-list';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { AuthComponent } from './features/auth/auth';
import { QuizAttemptPage } from './features/quiz/pages/quiz-attempt/quiz-attempt-page';
import { Landing } from './features/landing/landing';
import { ProfileComponent } from './features/profile/profile';
import { DashboardComponent } from './features/dashboard/dashboard';
import { NotificationComponent } from './features/notification/notification';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { StudentsListComponent } from './features/admin/students-list/students-list.component';
import { TeachersListComponent } from './features/admin/teachers-list/teachers-list.component';
import { SubjectsListComponent } from './features/admin/subjects-list/subjects-list.component';
import { ClassesListComponent } from './features/admin/classes-list/classes-list.component';
import { QuizComponent } from './features/quiz/quiz';
import { QuizCreatePage } from './features/quiz/pages/create/quiz-create-page';
import { QuizExplorePage } from './features/quiz/pages/explore/quiz-explore-page';
import { QuizSubmissionPage } from './features/quiz/pages/quiz-submission-detail/quiz-submission-page';
import { QuizDetailPage } from './features/quiz/pages/quiz-detail/quiz-detail-page';
import { QuizSubmissionsPage } from './features/quiz/pages/quiz-submissions/quiz-submissions-page';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: Landing,
    data: { requiresAuth: false },
  },
  {
    path: 'quiz',
    component: QuizComponent,
    canActivate: [AuthGuard],
    data: { requiresAuth: true },
    children: [
      {
        path: ':quizId/attempt/:submissionId',
        component: QuizAttemptPage,
        data: { requiresAuth: true, allowedRoles: ['Student'] },
      },
      {
        path: 'create',
        component: QuizCreatePage,
        data: { requiresAuth: true, allowedRoles: ['Teacher'] },
      },
      {
        path: 'explore',
        component: QuizExplorePage,
        data: { requiresAuth: true },
      },
      {
        path: ':quizId/submission/:submissionId',
        component: QuizSubmissionPage,
        data: { requiresAuth: true },
      },
      {
        path: ':quizId/submissions',
        component: QuizSubmissionsPage,
        data: { requiresAuth: true, allowedRoles: ['Teacher'] },
      },
      {
        path: ':quizId',
        component: QuizDetailPage,
        data: { requiresAuth: true, allowedRoles: ['Teacher'] },
      },
    ],
  },
  {
    path: 'auth',
    component: AuthComponent,
    data: { requiresAuth: false },
    children: [
      { path: 'login', component: LoginComponent },
      {
        path: 'register',
        component: RegisterComponent,
      },
    ],
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard],
    data: { requiresAuth: true },
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: { requiresAuth: true },
  },
  {
    path: 'notifications',
    component: NotificationComponent,
    canActivate: [AuthGuard],
    data: { requiresAuth: true, allowedRoles: ['Student'] },
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: { requiresAuth: true, allowedRoles: ['Admin'] },
    children: [
      { path: '', component: AdminDashboardComponent },
      { path: 'students', component: StudentsListComponent },
      { path: 'teachers', component: TeachersListComponent },
      { path: 'subjects', component: SubjectsListComponent },
      { path: 'classes', component: ClassesListComponent },
    ],
  },
];
