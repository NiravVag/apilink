
export class HolidayMasterModel {

  countryId:number ;
  branchId: number;
  year: number;
  items: Array<HolidayMasterItemModel> = [];
  index: number;
  pageSize: number;
  totalCount: number;
  pageCount: number;
  noFound: boolean;

  constructor() {

    this.countryId = 0;
    this.branchId = 0;
    this.year = 0;
    this.noFound = false;
  }
}

export class HolidayMasterItemModel {

  constructor() {

  }

  id: number;
  name: string;
  recurrenceType: number;
  startDate?: any;
  endDate?: any;
  startDay?: number;
  endDay?: number;
  startDayType?: number;
  endDayType?: number;
   }

export class HolidayRequest {

  constructor() {

  }

  id: number;
  startDate?: any;
  endDate?: any;
  startDay?: number;
  endDay ?: number;
  countryId: number;
  recurrenceType: number;
  officeId: number;
  holidayName: string;
  forAllIterations: boolean;
  endDateWeek?: any;
  startDayType?: number;
  endDayType?: number;

}



