import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';

export const routes: Routes = [
    {path: 'login', component: Login},
    {path: 'home/:username', component: Home},
];
