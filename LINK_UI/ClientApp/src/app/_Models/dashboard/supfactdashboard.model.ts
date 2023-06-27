import { array } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { APIService, ListSize } from "src/app/components/common/static-data-common";
import { DataSource } from "../common/common.model";
import { CustomerAPIRADashboard, InspectionRejectDashboard } from "./customerdashboard.model";

export class SupFactDashboardMasterModel {
  supplierList: Array<DataSource>;
  factoryList: Array<DataSource>;
  customerList: Array<DataSource>;
  bookingStatusList: Array<DataSource>;

  customerLoading: boolean;
  factoryLoading: boolean;
  supplierLoading: boolean;
  bookingStatusLoading: boolean;

  customerInput: BehaviorSubject<string>;
  supplierInput: BehaviorSubject<string>;
  factoryInput: BehaviorSubject<string>;

  isSupplier: boolean;
  isFactory: boolean;

  supplierModelRequest: SupFactDashboardDataSourceRequest;
  factoryModelRequest: SupFactDashboardDataSourceRequest;
  customerModelRequest: SupFactDashboardDataSourceRequest;

  searchLoading: boolean;
  mapLoading: boolean;

  centreLatitude: number;
  centreLongitude: number;
  mapHeight: number;

  inspectionRejectDashboard: Array<InspectionRejectDashboard>;
  inspectionRejectLoading: boolean;
  inspectionRejectError: boolean;
  inspectionRejectFound: boolean;

  apiresultLoading: boolean;
  apiresultFound: boolean;
  apiresultError: boolean;
  apiraDashboard: Array<CustomerAPIRADashboard>;

  customerDataLoading: boolean;
  customerDataFound: boolean;
  customerDataError: boolean;
  customerDataDashboard: Array<CustomerBookingModel>;

  bookingDataLoading: boolean;
  bookingDataFound: boolean;
  bookingDataError: boolean;
  bookingDataDashboard: Array<BookingDetails>;

  constructor() {
    this.customerInput = new BehaviorSubject<string>("");
    this.supplierInput = new BehaviorSubject<string>("");
    this.factoryInput = new BehaviorSubject<string>("");

    this.customerModelRequest = new SupFactDashboardDataSourceRequest();
    this.supplierModelRequest = new SupFactDashboardDataSourceRequest();
    this.factoryModelRequest = new SupFactDashboardDataSourceRequest();
    this.apiraDashboard = new Array<CustomerAPIRADashboard>();
    this.inspectionRejectDashboard = new Array<InspectionRejectDashboard>();
    this.customerDataDashboard = new Array<CustomerBookingModel>();

    this.centreLatitude = 30.77;
    this.centreLongitude = 11.25;
    this.mapHeight = 380;
  }
}


export class SupFactDashboardDataSourceRequest {
  searchText: string;
  skip: number;
  take: number;
  customerId: number;
  supplierId: number;
  supplierType: number;
  factoryId: number;
  id: number;
  locationId: number;
  idList: Array<number>;
  supSearchTypeId: number;
  customerglCodes: Array<string>;
  serviceId: number;
  locationIdList: Array<number>;
  constructor() {
    this.serviceId = APIService.Inspection;
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
  }
}

export class SupFactDashBoardModel {
  supplierId: number;
  factoryId: number;
  customerId: number;
  fromDate: any;
  toDate: any;
  statusIdList: Array<number>;
}

export enum DashboardResult {
  Success = 1,
  CannotGetList = 2,
  Failed = 3,
  RequestNotCorrectFormat = 4
}

export class CustomerBookingModel
{
      customerName : string;
      bookingId : number;
      bookingCount :number;
      statusColor: string;
}

export class BookingDetails
{
      bookingId :number;
      customerName :string;
      factoryName :string;
      supplierName :string;
      serviceType :string;
      serviceFromDate :string;
      serviceToDate :string;
      countryName :string;
      isEdit :boolean;
}
