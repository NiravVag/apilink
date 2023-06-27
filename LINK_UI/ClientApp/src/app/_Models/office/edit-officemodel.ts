export class EditOfficeModel {
    public id?: number;
    public name: string;
    public officecode?: string;
    public fax: string;
    public tel: string;
    public zipcode: number;
    public address: string;
    public address2: string;
    public email: string;
    public locationType?: LocationType;
    public headoffice?: number;
    public country?: CountryModel;
    public city?: City;
    public operationcountries?: Array<number> = [];
    public comment: string;
    locationTypeId?: number;
    countryId?: number;
    cityId?: number;
}

export class LocationType {
    public id: number;
    public sgT_Location_Type?: string;
}

export class CountryModel {
    public id: number;
    public countryName?: string;
}

export class City {
    public id: number;
    public name?: string;
}
export class Office {
    public id: number;
    public name: string;
}
export enum OfficeResponseResult { 
    success = 1,
    Error = 2
}