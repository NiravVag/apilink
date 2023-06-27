
export class CustomerSummaryModel {
 
  groupValues: any[];
  customerValues: any[];
  items: Array<CustomerSummaryItemModel> = [];
  index: number;
  pageSize: number;
  totalCount: number;
  pageCount: number;
  noFound: boolean;
  isLeft: boolean;

  constructor() {
    this.groupValues = [];
    this.customerValues = [];
    this.noFound = false;
    this.isLeft = false; 
  }
}

export class CustomerSummaryItemModel {
  constructor() {
  }
  id: number;
  name: string;
  group: string;  
  list: Array<CustomerSummaryItemModel>;
  isExpand: boolean = false; 
}


export class customerToRemove {
  constructor() {
  }
  id: number;
  name: string;
}
