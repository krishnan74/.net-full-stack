import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';
import { About } from './about/about';
import { Contact } from './contact/contact';

export const routes: Routes = [
    {path: 'login', component: Login},
    {path:'home/:username',component:Home,
        children:
        [
            {path:'about',component:About},
            {path:'contact',component:Contact}

        ]
    }
];
