import { DataSource } from "@amcharts/amcharts4/core";
import { BehaviorSubject } from "rxjs";
import { ListSize } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest } from "../common/common.model";
import { DropdownResult, DataList } from "../inspectioncertificate/inspectioncertificate.model";

export class EditStaffModel {
  public id: number;
  public employeeNo: string;
  public dateBirth: any;
  public countryId: number = 0;
  public countryName: string;
  public staffName: string;
  public qualificationId: number = 0;
  public localLanguage: string;
  public graduate: string;
  public emergencyContact: string;
  public gender: string = "0";
  public graduateDate: any;
  public emergencyContactPhone: string;
  public martial: string = "0";
  public passportNo: string;
  public skypeId: string;
  public email: string;
  public phone: string;
  public joinDate: any;
  public positionId: number = 0;
  public companyEmail: string;
  public bankName: string;
  public reportHeadId: number = 0;
  public managerId: number = 0;
  public companyMobile: string;
  public bankAccount: string;
  public employeeTypeId: number = 0;
  public annualLeave: string;
  public assCardNo: string;
  public officeId: number = 0;
  public probExpDate: any;
  public housingFundCard: string;
  public qcStartPlaceId: number;
  public probatonPeriod: string;
  public placePurchSiHf: string;
  public departmentId: number = 0;
  public subdepartmentId: number = 0;
  public opCountryValues: any[];
  public profileValues: any[];
  public opCountryItems: any[];
  public profileItems: any[];
  public marketSegmentValues: any[];
  public productCategoryValues: any[];
  public expertiseValues: any[];
  public payrollCurrencyId: number = 0;
  public homeCountryId: number = 0;
  public homeStateId: number = 0;
  public homeCityId: number = 0;
  public homeAddress: string;
  public currentCountryId: number = 0;
  public currentStateId: number = 0;
  public currentCityId: number = 0;
  public currentCountyId: number;
  public currentAddress: string;
  public startWkDate: any;
  public workingYears: number;
  public totWkYearsGarment: number;
  public picture: File;
  public renewList: Array<renewModel> = [];
  public homeIsCurrent: boolean = false;
  public jobList: Array<jobModel> = [];
  public trainingList: Array<trainingModel> = [];
  public attachedList: Array<attachedFileModel> = [{ id: 0, fileName: "", fileTypeId: 0, userId: 0, userName: "", uploadedDate: "", file: null, isNew: true, mimeType: "", uniqueId: "", fileUrl: "" }]
  public isForecastApplicable: boolean;
  public hrPhoto: hrPhotoModel = { guidId: "", fileName: "", userId: 0, uniqueId: "", fileUrl: "" };
  statusName: string;
  statusColor: string;
  majorSubject: string;
  emergencyContactRelationship: string;
  globalGrading: string;
  noticePeriod: number;
  statusId: number;
  bandId: number;
  socialInsuranceTypeId: number;
  hukoLocationId: number;
  public apiEntityIds: any;
  public apiServiceIds: any;
  public entityServiceIds: any;
  public primaryEntity: number;
  public startPortId: number;
  public hrOutSourceCompanyId: number;
  public companyId: number;
  public payrollCompany: number = 0;
  constructor() {
    this.statusId = 0;
  }
}

export class renewModel {
  public id: number
  public startDate: any;
  public endDate: any;
}

export class jobModel {
  public id: number;
  public company: string;
  public position: string;
  public salary: string;
  public currencyId: number;
  public startDate: any;
  public endDate: any;
}

export class trainingModel {
  public id: number;
  public trainingTopic: string;
  public startDate: any;
  public endDate: any;
  public trainer: string;
  public comments: string;
}
export class hrPhotoModel {
  public guidId: string;
  public fileName: string;
  public uniqueId: string;
  public fileUrl: string;
  public userId: number;
}

export class attachedFileModel {
  public id: number;
  public fileName: string;
  public fileTypeId: number;
  public userId: number;
  public userName: string;
  public uploadedDate: string;
  public file: File;
  public isNew: boolean;
  public mimeType: string;
  public uniqueId: string;
  public fileUrl: string;
}

export class HRProfileResponse {
  profileList: Array<DataList>;
  result: DropdownResult;
}
export class StaffCountyFilterMaster {
  public cityId: number;
  public countyList: any;
  countyListName: string;
  filterCount: number;
  filterDataShown: boolean;

  countyInput: BehaviorSubject<string>;

  countyLoading: boolean;

  constructor() {
    this.countyInput = new BehaviorSubject<string>("");
    this.countyLoading = false;
    this.countyList = [];
  }
}

export class EditStaffMasterModel {
  bandList: Array<DataSource>;
  bandLoading: boolean;

  socialInsuranceTypeList: Array<DataSource>;
  socialInsuranceTypeLoading: boolean;

  hukoLocationLoading: boolean;
  hukoLocationList: Array<DataSource>;
  hukoLocationInput: BehaviorSubject<string>;
  hukoLocationModelRequest: CommonDataSourceRequest;

  startingPortNameListLoading: boolean;
  startingPortNameList: any;
  
  payrollCompanyLoading: boolean;
  payrollCompanyList: any[];
  
  constructor() {
    this.hukoLocationInput = new BehaviorSubject<string>("");
    this.hukoLocationModelRequest = new CommonDataSourceRequest();
    this.hukoLocationList = new Array<DataSource>();
  }
}

export class HROutSourceCompanyRequest {
  public searchText: string;
  public skip: number;
  public take: number;
  public id: number;
  constructor() {
    this.searchText = "";
    this.skip = 0;
    this.take = ListSize;
    this.id = null;
  }
}

export class SaveHROutSourceRequest {
  public id: number;
  public name: string
}

export class SaveHROutSourceCompany {
  id: number;
  name: string;
}

export class HROutSourceCompanyMaster {

  saveHROutSourceCompanyLoading: boolean;

  constructor() {
    this.saveHROutSourceCompanyLoading = false;
  }

}

export enum SaveHROutSourceResult {
  Success = 1,
  NameNotAvailable = 2,
  NameAlreadyExists = 3,
  Failure = 4
}
