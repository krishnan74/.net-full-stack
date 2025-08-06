import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, map, take } from 'rxjs';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { User } from '../../store/auth/auth.model';

export interface RouteData {
  requiresAuth?: boolean;
  allowedRoles?: string[];
}

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private store: Store, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.store.select(selectUser).pipe(
      take(1),
      map((user: User | null) => {
        const routeData = route.data as RouteData;

        // Check if route requires authentication
        if (routeData.requiresAuth !== false) {
          // Route requires authentication
          if (!user) {
            // User is not authenticated, redirect to login with return URL
            this.router.navigate(['/auth/login'], {
              queryParams: { returnUrl: state.url },
            });
            return false;
          }

          // Check if route has role restrictions
          if (routeData.allowedRoles && routeData.allowedRoles.length > 0) {
            if (!routeData.allowedRoles.includes(user.role)) {
              // User doesn't have required role, redirect to dashboard
              this.router.navigate(['/dashboard']);
              return false;
            }
          }
        }

        return true;
      })
    );
  }
}
