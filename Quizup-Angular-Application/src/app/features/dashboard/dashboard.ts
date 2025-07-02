import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit {
  summary: any = {};
  userType: 'student' | 'teacher' = 'student'; // This should be set based on auth

  constructor(private dashboardService: DashboardService) {}

  ngOnInit() {
    this.dashboardService.getSummary(this.userType).subscribe((data: any) => {
      this.summary = data;
    });
  }
}
