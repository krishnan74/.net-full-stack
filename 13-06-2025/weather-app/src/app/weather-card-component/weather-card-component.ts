import { Component, Input } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { WeatherModel } from '../models/weather';

@Component({
  selector: 'app-weather-card-component',
  templateUrl: './weather-card-component.html',
  styleUrl: './weather-card-component.css',
  imports: []
})
export class WeatherCardComponent {
  @Input() weather : WeatherModel | null = null;

}
