import { Routes } from '@angular/router';
import { TemplateForm } from './template-form/template-form';
import { ReactiveForm } from './reactive-form/reactive-form';

export const routes: Routes = [
  { path: 'templateForm', component: TemplateForm },
  { path: 'reactiveForm', component: ReactiveForm },
];
