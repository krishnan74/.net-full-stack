import { Routes } from '@angular/router';
import { TeacherList } from './features/teacher/components/teacher-list/teacher-list';

export const routes: Routes = [
  {
    path: 'teachers',
    component: TeacherList,
  },
];
