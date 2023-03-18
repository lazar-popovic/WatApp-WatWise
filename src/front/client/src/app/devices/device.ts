export class Device 
{
    id: number = 0;
    name: string;
    type: string;
    category: string;

    constructor(name: string, type: string, category: string) 
    {
        this.name = name;
        this.type = type;
        this.category = category;
    }
}
  