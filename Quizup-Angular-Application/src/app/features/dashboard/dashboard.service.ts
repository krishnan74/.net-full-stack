import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private http: HttpClient) {}

  getSummary(userType: 'student' | 'teacher'): Observable<any> {
    return this.http.get(`/api/dashboard/summary?type=${userType}`);
  }
}
