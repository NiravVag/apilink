import { Currency } from "./echange-rate.model";

export class CurrencyItem {

  public currency: Currency;

  public currencyValueList: Array<CurrencyValue>;

}

export class CurrencyValue {

  public currency: Currency;
  public value: Number; 
}


