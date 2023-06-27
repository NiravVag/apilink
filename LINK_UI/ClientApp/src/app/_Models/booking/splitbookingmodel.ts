import { EditInspectionBooking } from "./inspectionbooking.model";

export class ProductInfo {
  public id: number;
  public productId: number;
  public productName: string;
  public productDescription: string;
  public bookingQuantity: number;
  public selected: boolean = false;
  public inspectionId:number;
  public poList: any;
  public poTransactionId:number;
  public poId:number;
  public poName:string;
  public etd:string;
  public destinationCountryName:string;
  public colorTransactionId:number;
  public colorCode:string;
  public colorName:string;
  public combineGroupId:number;
  public pickingQuantity:number;
  unitName: string;
}

export class SplitBooking {
  public bookingId: number;
  public isEmailRequired: boolean;
  public splitBookingComments: string;
  public bookingData: EditInspectionBooking;
  public splitBookingProductList: Array<ProductInfo>;
}
