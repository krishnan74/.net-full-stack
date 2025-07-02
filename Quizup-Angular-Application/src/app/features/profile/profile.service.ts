import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  constructor(private http: HttpClient) {}

  updateUserDetails(details: any): Observable<any> {
    return this.http.put('/api/user', details);
  }

  changePassword(data: any): Observable<any> {
    return this.http.post('/api/user/change-password', data);
  }
}
