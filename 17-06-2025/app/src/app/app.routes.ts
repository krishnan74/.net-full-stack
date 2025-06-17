import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Products } from './products/products';
import { Home } from './home/home';
import { Profile } from './profile/profile';
import { AuthGuard } from './auth-guard';
import { About } from './about/about';
import { Contact } from './contact/contact';
import { ProductDetail } from './product-detail/product-detail';


export const routes: Routes = [
    {path:'login',component:Login},
    {path:'products',component:Products, canActivate:[AuthGuard]},
    {path:'products/:id',component:ProductDetail, canActivate:[AuthGuard]},
    {path:'home/:un',component:Home,children:
        [
            {path:'about',component:About},
            {path:'contact',component:Contact}

        ]
    },
    {path:'profile',component:Profile,canActivate:[AuthGuard]}

];