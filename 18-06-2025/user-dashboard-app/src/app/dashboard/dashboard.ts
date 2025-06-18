import { Component, OnInit } from '@angular/core';
import { UserService } from '../features/users/users.service';
import { ChartConfiguration, ChartType } from 'chart.js';
import { Chart, registerables } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { UserModel } from '../features/users/user.model';
import { feature } from 'topojson-client';
import * as ChartGeo from 'chartjs-chart-geo';

@Component({
  selector: 'app-dashboard',
  imports: [BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
  providers: [UserService],
})
export class Dashboard implements OnInit {
  us: any;

  pieChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [],
  };

  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [],
  };

  stateChartData: ChartConfiguration<'choropleth'>['data'] = {
    labels: [],
    datasets: [],
  };

  stateChartOptions: ChartConfiguration<'choropleth'>['options'] = {
    scales: {
      projection: {
        axis: 'x',
        projection: 'albersUsa',
      },
      color: {
        axis: 'x',
        quantize: 5,
        legend: {
          position: 'bottom-right',
          align: 'right',
        },
      },
    },
  };

  constructor(private userService: UserService) {
    Chart.register(
      ...registerables,
      ChartGeo.ChoroplethController,
      ChartGeo.GeoFeature,
      ChartGeo.ColorScale,
      ChartGeo.ProjectionScale
    );
  }

  async ngOnInit(): Promise<void> {
    this.us = await fetch(
      'https://cdn.jsdelivr.net/npm/us-atlas/states-10m.json'
    ).then((r) => r.json());

    this.userService.users$.subscribe((users) => {
      this.prepareGenderChart(users);
      this.prepareRoleChart(users);
      this.prepareStateChart(users);
    });

    this.userService.getUsers();
  }

  prepareGenderChart(users: UserModel[]): void {
    const genderCount: { [key: string]: number } = {};
    users.forEach((user) => {
      genderCount[user.gender] = (genderCount[user.gender] || 0) + 1;
    });
    this.pieChartData = {
      labels: Object.keys(genderCount),
      datasets: [{ data: Object.values(genderCount) }],
    };
  }

  prepareRoleChart(users: UserModel[]): void {
    const roleCount: { [key: string]: number } = {};
    users.forEach((user) => {
      roleCount[user.role] = (roleCount[user.role] || 0) + 1;
    });
    this.barChartData = {
      labels: Object.keys(roleCount),
      datasets: [{ data: Object.values(roleCount), label: 'Users by Role' }],
    };
  }

  async prepareStateChart(users: UserModel[]): Promise<void> {
    const nation = (feature(this.us, this.us.objects.nation) as any)
      .features[0];
    const states = (feature(this.us, this.us.objects.states) as any).features;

    const stateCount: { [key: string]: number } = {};
    users.forEach((user) => {
      stateCount[user.state] = (stateCount[user.state] || 0) + 1;
    });

    this.stateChartData = {
      labels: states.map((d: any) => d.properties.name),
      datasets: [
        {
          label: 'Users by State',
          outline: nation,
          data: states.map((d: any) => ({
            feature: d,
            value: stateCount[d.properties.name] || 0,
          })),
        },
      ],
    };
  }
}
