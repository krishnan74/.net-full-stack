import { Component } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-city-search-component',
  templateUrl: './city-search-component.html',
  imports: [FormsModule, CommonModule],
  styleUrl: './city-search-component.css'
})
export class CitySearchComponent {

  showSuggestions: boolean = false;
  cityName: string = '';
  suggestedCities: {id: number, name: string}[] = [];

  constructor(private weatherService: WeatherService) {}

  onSubmit(): void {
    this.weatherService.fetchWeather(this.cityName);
    this.showSuggestions = false;

  }

  onCityNameChange(): void {
    this.showSuggestions = this.cityName.length > 0;
    this.weatherService.getCityNames(this.cityName).then(cities => {
      this.suggestedCities = cities;
    });
  }

  onCitySelect(city: string): void {
    this.cityName = city;
    this.suggestedCities = [];
    this.onSubmit();
  }
  
}
