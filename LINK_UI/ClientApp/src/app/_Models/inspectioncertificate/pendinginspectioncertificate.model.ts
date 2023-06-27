import { DataList, ICBookingSearch } from "../inspectioncertificate/inspectioncertificate.model";

export class PendingICMasterData {
  customerList: Array<DataList>;
  customerLoading: boolean;
  searchloading: boolean;
  bookingSearchList: Array<ICBookingSearch>;
  selectedPageSize: number;
  pageSizeItems: any;
  selectedAllBooking: boolean;
  isICSelected: boolean;
  redirectPath: string;
  isICRoleAccess: boolean;
}
export class ICFromPendingIC {
  public customerId: number;
  public supplierId: number;
  public bookingIds: number[];
  public inspPoTransactionIds: number[];
}
