export class Weather {
  public name: string;
    public description: string;
    public icon: string;
    public temperature: number;
    public minTemperature: number;
    public maxTemperature: number;
    public humidity: number;
    public wind: number;

  constructor(
    name: string,
     description: string,
     icon: string,
     temperature: number,
     minTemperature: number,
     maxTemperature: number,
     humidity: number,
     wind: number
  ) {
    this.name = name;
    this.description = description;
    this.icon = icon;
    this.temperature = temperature;
    this.minTemperature = minTemperature;
    this.maxTemperature = maxTemperature;
    this.humidity = humidity;
    this.wind = wind;
  }
}
