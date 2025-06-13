export class WeatherModel{
    constructor(
        public location: string,
        public weatherIcon: string,
        public temperature: number,
        public condition: string,
        public humidity: number,
        public windSpeed: number
    ) {}
}
