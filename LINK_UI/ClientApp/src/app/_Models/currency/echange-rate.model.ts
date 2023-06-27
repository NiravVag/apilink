
export class CurrencyRateItem {
  public beginDate: any;
  public endDate: any;
  public rateList: Array<RateItem>;
  public isNew:boolean = false; 
}


export class RateItem {
  public currencyId: number;
  public value: number;
  public conversionId: number; 
}


export class Currency {
  public id: number;
  public currencyCode: string;
  public currencyName: string; 
}


