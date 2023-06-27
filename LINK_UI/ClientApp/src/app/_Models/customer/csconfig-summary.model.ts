import { summaryModel } from "../summary.model";

export class csConfigSearchModel extends summaryModel {
  userId: number;
  customerId: number;
  serviceId: any[];
  productCategoryId: any[];
  officeLocationId: any[];
  constructor() {
    super();
  }
}
export class csConfigItem {
  public csConfigId: number;
  public customerName: string;
  public office: string;
  public customerService: string;
  public productCatgory: string;
  public service: string;
  public selected: boolean;
}
export class csDeleteItem {
  id: any[] =[];
}

