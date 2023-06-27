export class CustomerBuyermodel
{
    public customerValue:number;
    public buyerList:Array<buyermodel>=[];
}

export class buyermodel{
    public id:number;
    public name:string;
    public code:string;
    public apiServiceIds:any;
}

export class customerBuyerToRemove {
    constructor() {
    }
    id: number;
    name: string;
  }