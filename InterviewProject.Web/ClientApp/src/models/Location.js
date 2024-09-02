import { Forecast } from './Forecast';

export class Location {
    constructor(key, localizedName, forecasts) {
        this.key = key;
        this.localizedName = localizedName;
        this.forecasts = forecasts.map(forecast => forecast instanceof Forecast ? forecast : new Forecast(forecast.date, forecast.temperature));
    }
}
export default Location;