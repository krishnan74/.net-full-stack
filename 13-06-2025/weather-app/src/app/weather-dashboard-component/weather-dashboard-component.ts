import { Component, OnInit } from '@angular/core';
import { WeatherModel } from '../models/weather';
import { WeatherService } from '../services/weather.service';
import { WeatherCardComponent } from '../weather-card-component/weather-card-component';

@Component({
  selector: 'app-weather-dashboard-component',
  imports: [WeatherCardComponent],
  templateUrl: './weather-dashboard-component.html',
  styleUrl: './weather-dashboard-component.css'
})
export class WeatherDashboardComponent implements OnInit {
  weather$: any;
  weather: WeatherModel | null = null;

  constructor(private weatherService: WeatherService) {
    this.weatherService.weather$.subscribe({
      next: (value) => {
        this.weather = value;
      },
      error: (err) => {
        console.error('Error fetching weather data:', err);
      }
    });
  }

  ngOnInit(): void {
    this.weather$ = undefined;
  }
}
