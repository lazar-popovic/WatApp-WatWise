export class User {
    id ?: number;
    firstname = "Ime";
    lastname = "Prezime";
    mail = "mejl";
    password? = "as";
    roleId? = 3;
    role? = "Prosumer";
    address = "Radoja Domanovica";
    num = 1;
    city = "Kragujevac";
    neighborhood ?: string;
    profileImage? = "";
    currentConsumption ?: number;
    currentProduction ?: number;
    activeConsumers ?:number;
    activeProducers ?:number;
    latitude ?:number;
    longitude ?:number;
}

