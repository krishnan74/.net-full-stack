import { EnvironmentPlugin } from 'webpack';

require('dotenv').config();

module.exports = {
    plugins: [
    new EnvironmentPlugin([
        'WEATHER_API_KEY',
    ])
    ]
};