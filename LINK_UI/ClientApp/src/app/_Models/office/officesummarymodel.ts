import { summaryModel } from "../summary.model";

export class OfficeSummaryModel extends summaryModel {
  OfficeValues: any[];
}
//API Return
export class OfficeSummaryItemModel {
  id: number;
  officename: string;
  locationtypeid?: number;
  locationtypename: string;
  fax?: string;
  tel?: string;
  zipcode?: number;
  address1: string;
  address2: string;
  email?: string;
  parentId?: number;
  comment?: string;
  city: string;
  country: string;
  operationcountriesname: string;
}
