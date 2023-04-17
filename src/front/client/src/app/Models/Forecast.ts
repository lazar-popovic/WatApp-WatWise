export class Forecast {
  date: Date;
  day: string;
  description: string;
  icon: string;
  temperature: number;
  minTemperature: number;
  maxTemperature: number;
  humidity: number;
  wind: number;

  constructor(
    date: Date,
    description: string,
    icon: string,
    temperature: number,
    minTemperature: number,
    maxTemperature: number,
    humidity: number,
    wind: number
  ) {
    this.date = date;
    this.day = this.getDayOfWeek(date);
    this.description = description;
    this.icon = icon;
    this.temperature = temperature;
    this.minTemperature = minTemperature;
    this.maxTemperature = maxTemperature;
    this.humidity = humidity;
    this.wind = wind;
  }

  private getDayOfWeek(date: Date): string {
    const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    return days[date.getDay()];
  }
}
