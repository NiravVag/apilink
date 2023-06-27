export class BookingCancelModel {
  public bookingId: number;
  public reasonTypeId: number;
  public timeTypeId: number = 0;
  public travelExpense: number = 0;
  public currencyId: number;
  public comment: string;
  public internalComment: string;
  public isEmailToAccounting: boolean;
  public serviceFromDate?: string;
  public serviceToDate?: string;
  isKeepAllocatedQC: boolean;
  isCancelKeepAllocatedQC: boolean;
  isDisableServiceFromDate: boolean;
  public firstServiceDateFrom: any;
  public firstServiceDateTo: any;
  constructor() {
    this.isCancelKeepAllocatedQC = false;
  }
}

export class BookingDetail {

  public bookingId: number;
  public customerName: string;
  public supplierName: string;
  public factoryName: string;
  public serviceType: string;
  public serviceDateFrom: string;
  public serviceDateTo: string;
  public productCategory: string;
  public statusId: number;
}
