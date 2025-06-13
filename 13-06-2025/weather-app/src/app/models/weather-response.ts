export interface WeatherApiResponse {
    location: {
        name: string;
    };
    current: {
        condition: {
            icon: string;
            text: string;
        };
        temp_c: number;
        humidity: number;
        wind_kph: number;
    };
}