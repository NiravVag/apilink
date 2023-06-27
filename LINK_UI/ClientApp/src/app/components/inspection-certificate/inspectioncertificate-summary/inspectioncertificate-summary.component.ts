import { Component, OnInit } from '@angular/core';
import { icsearchtypelst, datetypelst, ICStatus } from '../../common/static-data-common';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { PageSizeCommon, SearchType, DefaultDateType, UserType, BookingSearchRedirectPage, BookingStatus } from '../../common/static-data-common'
import { ICSummaryMasterData, CustomerResult, DataList, ICSummaryModel, SupplierResult, ICStatusResult, ICItem,ICUserAccessData } from 'src/app/_Models/inspectioncertificate/inspectioncertificatesummary.model';
import { InspectionCertificateService } from 'src/app/_Services/inspectioncertificate/inspectioncertificate.service';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from "../../common/validator";
import { Router, ActivatedRoute } from '@angular/router';
import { first, retry } from 'rxjs/operators';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-inspectioncertificate-summary',
  templateUrl: './inspectioncertificate-summary.component.html',
  styleUrls: ['./inspectioncertificate-summary.component.scss'],
  animations: [
    trigger('expandCollapse', [
        state('open', style({
            'height': '*',
            'opacity': 1,
            'padding-top': '24px',
            'padding-bottom': '24px'
        })),
        state('close', style({
            'height': '0px',
            'opacity': 0,
            'padding-top': 0,
            'padding-bottom': 0
        })),
        transition('open <=> close', animate(300))
    ])
]
})
export class InspectioncertificateSummaryComponent extends SummaryComponent<ICSummaryModel> {
  icSearchTypeList: any = icsearchtypelst;
  dateTypeList: any = datetypelst;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;
  public _statuslist: any[] = [];
  icStatus = ICStatus;
  _redirectpath: string;
  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  Initialloading: boolean = false;

  icSummaryMasterModel: ICSummaryMasterData;
  icuserAccessData:ICUserAccessData;
  isFilterOpen: boolean;
  constructor(public bookingService: BookingService, public utility: UtilityService, public icService: InspectionCertificateService,
    private authService: AuthenticationService,
    public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService) {
    super(router, validator, route,translate,toastr);
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.Initialize(null);
    
    this.selectedPageSize = PageSizeCommon[0];
    
  }

  Initialize(fromLocal) {
    if (fromLocal == null)
      this.model = new ICSummaryModel();
    this.icSummaryMasterModel = new ICSummaryMasterData();
    this.icuserAccessData=new ICUserAccessData();
    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchTypeId = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.pageSize = 10;
    this.model.index = 1;
    this._statuslist = [];
    this.icuserAccessData.currentUser = this.authService.getCurrentUser();
    this.icuserAccessData.isInternalUser = this.icuserAccessData.currentUser.usertype == UserType.InternalUser ? true : false;
    this.getCustomerListByUserType();
    this.getICStatusList();
  }


  getCustomerListByUserType() {
    this.icSummaryMasterModel.customerLoading = true;

    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.icSummaryMasterModel.customerLoading = false;
      }
    );
  }

  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.processSuccessCustomerResponse(response);
      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETCUSTLIST');
      }
      this.icSummaryMasterModel.customerLoading = false;
    }
  }

  processSuccessCustomerResponse(response) {
    this.icSummaryMasterModel.customerList = response.customerList.map((x) => {
      var item: DataList = {
        id: x.id,
        name: x.name
      }
      return item;
    });
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }

  formValid(): boolean {
    return !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
  }

  getData(): void {
    this.GetSearchData();
  }

  expand(id) {
    this.icService.SearchICProducts(id)
      .pipe()
      .subscribe(
        res => {
          this.model.items.filter(x => x.icId == id)[0].productList = res;
        }
      );
    let item = this.model.items.filter(x => x.icId == id)[0];

    if (item != null)
      item.isExpand = true;
  }

  collapse(id) {
    let item = this.model.items.filter(x => x.icId == id)[0];

    if (item != null)
      item.isExpand = false;
  }



  SetSearchTypemodel(searchtype) {
    this.model.searchTypeId = searchtype;
  }

  GetStatusColor(statusid?) {
    
    if (this._statuslist != null && this._statuslist.length > 0 && statusid != null) {
      var result = this._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchTypeId == this._booksearttypeid
      && this.model.searchTypeText != null && this.model.searchTypeText.trim() != "" && isNaN(Number(this.model.searchTypeText));
  }

  IsDateValidationRequired(): boolean {
    return this.validator.isSubmitted && this.model.searchTypeText != null && this.model.searchTypeText.trim() == "" ? true : false;
  }

  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }

  SearchByStatus(id) {
    this.model.statusIdlst = [];
    this.model.statusIdlst.push(id);
    this.SearchDetails();
  }

  GetSearchData() {
    this.icSummaryMasterModel.searchloading = true;
    this.icService.SearchICSummary(this.model)
      .subscribe(
        response => {
          this.processICSummarySearchData(response);
          //this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.icSummaryMasterModel.searchloading = false;
        });
  }

  processICSummarySearchData(response) {
    if (response && response.result == 1) {
      this.mapPageProperties(response);
      this.model.items = response.data.map((x) => {
        var item: ICItem = {
          bookingNumber: x.bookingNo,
          icId: x.icId,
          icNumber: x.icNo,
          customerName: x.customer,
          supplierName: x.supplier,
          serviceDate: x.serviceDate,
          statusId:x.statusId,
          productList: [],
          isExpand: false,
          buyerName: x.buyerName
        }
        return item;
      }
      );
      this._statuslist = response.icStatusList;
      this.icSummaryMasterModel.searchloading = false;
    }
    else if (response && response.result == 2) {
      this.model.noFound = true;
      this.icSummaryMasterModel.searchloading = false;
      //this._statuslist = [];
    }
    else {
      this.error = response.result;
    }
  }

  getPathDetails(): string {
    return this._redirectpath;
  }

  setSupplierCustomerChange(selectedValue) {
    this.icSummaryMasterModel.supplierList = [];
    this.model.supplierId = null;
    if (selectedValue && selectedValue.id && selectedValue.id > 0)
      this.getSupplierList(selectedValue.id);
  }

  getSupplierList(id) {
    this.icSummaryMasterModel.supplierLoading = true;
    this.bookingService.Getsupplierbycusid(id).subscribe(
      response => {
        this.getSupplierListResponse(response);
      },
      error => {
        //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.icSummaryMasterModel.supplierLoading = false;
      }
    );
  }

  getSupplierListResponse(response) {
    if (response) {
      if (response.result == SupplierResult.Success) {
        this.icSummaryMasterModel.supplierList = response.data.map((x) => {
          var item: DataList = {
            id: x.id,
            name: x.name
          }
          return item;
        });
      }
      else if (response.result == SupplierResult.NodataFound) {
        //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETSUPPLIST');
      }
      this.icSummaryMasterModel.supplierLoading = false;
    }
  }

  getICStatusList() {
    this.icSummaryMasterModel.statusLoading = true;
    this.icService.GetICStatusList().subscribe(
      response => {
        this.processSuccessICStatusResponse(response);
      },
      error => {
        //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.icSummaryMasterModel.statusLoading = false;
      }
    );
    this.icSummaryMasterModel.statusLoading = true;
  }

  processSuccessICStatusResponse(response) {
    if (response && response.result == ICStatusResult.Success) {
      this.icSummaryMasterModel.icStatusList = response.icStatusList.map((x) => {
        var item: DataList = {
          id: x.id,
          name: x.name
        }
        return item;
      });
    }
    else if (response.result == ICStatusResult.NodataFound) {
      //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETSUPPLIST');
    }
    this.icSummaryMasterModel.statusLoading = false;
  }

  Reset() {
    this.Initialize(null);
  }

  preview(id) {
    this.icSummaryMasterModel.isPreview = true;
    this.icService.preview(id,false)
      .subscribe(res => {
        this.downloadFile(res, "application/pdf");
        this.icSummaryMasterModel.isPreview = false;
      },
        error => {
          this.icSummaryMasterModel.isPreview = false;
          //this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }

  redirectPage(icId) {
    
    this._redirectpath = "inspectioncertificateedit/edit-ic";
    super.getDetails(icId);
  }
  clearCustomer() {
    this.model.customerId = null;
    this.model.supplierId = null;
    this.icSummaryMasterModel.supplierList = [];
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "Fromdate": { 
        this.model.fromdate=null;
        break; 
     } 
     case "Todate": { 
      this.model.todate=null;
        break; 
     } 
    }
  }

}
