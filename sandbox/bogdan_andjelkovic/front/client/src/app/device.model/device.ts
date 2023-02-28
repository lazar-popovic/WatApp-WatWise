export class Device {
  Id: number | undefined;
  Name: string | undefined;
  Price: number | undefined;

  constructor( Name?: string, Price?: number) {
    this.Name = Name;
    this.Price = Price;
  }
}
