export class Forecast {
  constructor(
    public date: Date,
    public description: string,
    public icon: string,
    public temperature: number,
    public minTemperature: number,
    public maxTemperature: number,
    public humidity: number,
    public wind: number
  ) {}
}
