import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { WeatherModel } from "../models/weather";
import { WeatherApiResponse } from "../models/weather-response";
import { environment } from "../../environments/environment.development";

@Injectable()
export class WeatherService {

    private http = inject(HttpClient);
    private weatherSubject = new BehaviorSubject<WeatherModel | null>(null);
    weather$: Observable<WeatherModel | null> = this.weatherSubject.asObservable();

    async fetchWeather(cityName: string): Promise<void> {
        const weather = await this.getWeather(cityName);
        this.weatherSubject.next(weather);
    }

    clearWeather(): void {
        this.weatherSubject.next(null);
    }



    async getCityNames(searchQuery: string): Promise<{
        id: number;
        name: string;
    }[]> {
        try{
            if(!searchQuery || searchQuery.trim() === "") {
                return [];
            }
            const response = await this.http.get<any[]>(`http://api.weatherapi.com/v1/search.json?key=${environment.WEATHER_API_KEY}&q=${searchQuery}`).toPromise();
            const cityNames = response?.map(item => ({ id: item.id, name: item.name }));
            return cityNames || [];
        }

        catch(error: any) {
            console.error("Error fetching city names:", error);
            return [];
        }
    }

    async getWeather(cityName: string): Promise<WeatherModel | null> {
        try {

            if (!/^[a-zA-Z\s-]+$/.test(cityName)) {
                throw new Error(
                    "Please enter a valid city name (letters, spaces, and hyphens only)"
                );
            }

            const response = await this.http
                .get<WeatherApiResponse>(
                    `http://api.weatherapi.com/v1/current.json?key=${environment.WEATHER_API_KEY}&q=${encodeURIComponent(
                        cityName
                    )}&aqi=no`
                )
                .toPromise();

            if(!response){
                throw new Error("No weather data found for the specified city.");
            }

            const weatherData = new WeatherModel(
                response.location.name,
                response.current.condition.icon,
                response.current.temp_c,
                response.current.condition.text,
                response.current.humidity,
                response.current.wind_kph
            )
            console.log(weatherData);

            return weatherData || null;
        } catch (error: any) {
            console.log(error.message);
            return null;
        }
    }
}
