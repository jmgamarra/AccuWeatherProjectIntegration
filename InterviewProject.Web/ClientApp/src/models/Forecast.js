import { Temperature } from './Temperature';

export class Forecast {
    constructor(date, temperature) {
        this.date = date;
        this.temperature = temperature instanceof Temperature ? temperature : new Temperature(temperature.minimum, temperature.maximum);
    }
}