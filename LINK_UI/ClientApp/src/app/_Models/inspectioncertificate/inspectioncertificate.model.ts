import { summaryModel } from "../summary.model";
import { ICFromPendingIC } from "./pendinginspectioncertificate.model";

export interface DataList {
  id: number;
  name: string;
}

export class ICMasterData {
  customerLoading: boolean;
  supplierLoading: boolean;
  icTitleLoading: boolean;
  previewLoading: boolean;
  saveLoading: boolean;
  cancelLoading: boolean;
  draftLoading: boolean;
  bookingSearchLoading: boolean;
  customerList: Array<DataList>;
  supplierList: Array<DataList>;
  icTitleList: Array<DataList>;
  beneficiaryName: string;
  bookingSearchList: Array<ICBookingSearch>;
  noDataBooking: boolean;
  selectedAllBooking: boolean;
  selectedAllCheckBoxEnable: boolean;
  icFromPendingICList: InspectionCertificateBookingSearchRequest;
  isFromPendingIC: boolean;
  poLoading: boolean;
  poList: Array<DataList>;
  poId: number[];
  isICRoleAccess: boolean;
  icBookingAllList: Array<InspectionCertificateBookingRequest>;
}

export class EditInspectionCertificate {
  editIC: InspectionCertificateRequest;
  result: InspectionCertificateResult;
}

export class InspectionCertificateRequest {
  customerId: number;
  approvalDate: any;
  icTitle: string;
  supplierId: number;
  icTitleId: number;
  beneficiaryName: string;
  supplierAddress: string;
  icNo: string;
  id: number;
  icStatus: number;
  icStatusName: string;
  comment: string;
  icBookingList: Array<InspectionCertificateBookingRequest>;
  buyerName: string;
  businessLine: number;
}
export class InspectionCertificateBookingSearchRequest extends summaryModel {
  bookingId: number;
  serviceFromDate: any;
  serviceToDate: any;
  customerId: number;
  supplierId: number;
  selectedCustomerId: number;
  selectedSupplierId: number;
  bookingIds: number[];
  inspPoTransactionIds: number[];
}

export class InspectionCertificateBookingRequest {
  id: number;
  icId: number;
  bookingProductId: number;
  shipmentQty: number;
  bookingNumber: number;
  poNo: string;
  poId: number;
  productCode: string;
  productDescription: string;
  destinationCountry: string;
  unit: string;
  remainingQty: number;
  presentedQty: number;
  totalICQty: number;
  color: string;
  colorCode: string;
  poColorId: number;
  businessLine: number;
}

export class ICBookingSearch {
  bookingNumber: number;
  customerName: string;
  supplierName: string;
  factoryName: string;
  serviceFromDate: string;
  serviceToDate: string;
  productList: Array<ICBookingSearchProduct>;
  checked: boolean;
  customerId: number;
  supplierId: number;
  enableCheckbox: boolean;
  bookingStatus: string;
  serviceType: string;
  isExpand: boolean;
  businessLine: number;
  //  poHeadingChecked : boolean;
}

export class ICBookingSearchProduct {
  poNo: string;
  poId: number;
  productCode: string;
  productDescription: string;
  shipmentQty: number;
  remainingQty: number;
  destinationCountry: string
  unit: string
  // fBReportStatus:string;
  fBReportId: number
  bookingId: number;
  checked: boolean;
  inspPOTransactionId: number;
  enableCheckbox: boolean;
  presentedQty: number;
  totalICQty: number;
  color: string;
  colorCode: string;
  poColorId: number;
}
export class ICBookingProductRequest {
  BookingIdList: number[];
  ProductIdList: number[];
}
export enum CustomerResult {
  Success = 1,
  CannotGetCustomerList = 2
}
export enum SupplierResult {
  Success = 1,
  NodataFound = 2
}
export enum DropdownResult {
  Success = 1,
  NodataFound = 2
}
export enum InspectionCertificateResult {
  Success = 1,
  Failure = 2,
  RequestNotCorrectFormat = 3,
  ICNoNotInserted = 4
}

export enum ICBookingSearchResult {
  Success = 1,
  Failure = 2,
  RequestNotCorrectFormat = 3,
  NodataFound = 4
}
