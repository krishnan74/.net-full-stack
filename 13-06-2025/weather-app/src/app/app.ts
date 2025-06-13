import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CitySearchComponent } from './city-search-component/city-search-component';
import { WeatherDashboardComponent } from './weather-dashboard-component/weather-dashboard-component';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CitySearchComponent,WeatherDashboardComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-app';
}
