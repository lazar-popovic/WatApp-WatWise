export class Device 
{
    id: number = 0;
    name: string;
    type: string;
    consumption : number;
    category: string;

    constructor(name: string, consumption : number ,type: string, category: string) 
    {
        this.name = name;
        this.type = type;
        this.consumption = consumption;
        this.category = category;
    }
}
  