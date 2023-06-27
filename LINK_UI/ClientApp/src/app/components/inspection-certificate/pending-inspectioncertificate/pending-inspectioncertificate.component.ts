import { Component } from '@angular/core';
import { DataList, InspectionCertificateBookingSearchRequest, ICBookingSearchResult, CustomerResult, ICBookingSearch } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { Validator } from "../../common/validator";
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryComponent } from '../../common/summary.component';
import { PendingICMasterData, ICFromPendingIC } from 'src/app/_Models/inspectioncertificate/pendinginspectioncertificate.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { InspectionCertificateService } from 'src/app/_Services/inspectioncertificate/inspectioncertificate.service';
import { PageSizeCommon, RoleEnum, UserType } from '../../common/static-data-common';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { ICUserAccessData } from 'src/app/_Models/inspectioncertificate/inspectioncertificatesummary.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
@Component({
  selector: 'app-pending-inspectioncertificate',
  templateUrl: './pending-inspectioncertificate.component.html',
  styleUrls: ['./pending-inspectioncertificate.component.scss'],
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
export class PendingInspectioncertificateComponent extends SummaryComponent<InspectionCertificateBookingSearchRequest> {
  private currentRoute: Router;

  model: InspectionCertificateBookingSearchRequest;
  ICMasterModel: PendingICMasterData;
  currentUser: UserModel;
  roleEnum = RoleEnum;
  icuserAccessData: ICUserAccessData;  
  isFilterOpen: boolean;
  constructor(public bookingService: BookingService, public icService: InspectionCertificateService,
    public calendar: NgbCalendar, authserve: AuthenticationService, public utility: UtilityService,
    public validator: Validator, router: Router, route: ActivatedRoute,translate: TranslateService, 
     private userAccountService: UserAccountService,private authService: AuthenticationService) {
    super(router, validator, route,translate);
    this.currentUser = authserve.getCurrentUser();
    this.currentRoute = router;
    this.isFilterOpen = true;
  }

  onInit(): void {                        
    this.initialize();
  }

  initialize(): void {
    this.ICMasterModel = new PendingICMasterData();
    this.model = new InspectionCertificateBookingSearchRequest();
    this.icuserAccessData = new ICUserAccessData();
    
    this.validator.isSubmitted = false;
    this.validator.setJSON("inspectioncertificate/pending-ic-booking-search.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.ICMasterModel.selectedPageSize = PageSizeCommon[0];
    this.ICMasterModel.pageSizeItems = PageSizeCommon;
    this.ICMasterModel.customerLoading = true;
    this.ICMasterModel.bookingSearchList = [];

    this.model.pageSize = PageSizeCommon[0];
    this.model.index = 0;
    this.model.serviceFromDate = this.calendar.getNext(this.calendar.getToday(), 'd', -30);
    this.model.serviceToDate = this.calendar.getToday();

    // this.icuserAccessData.isCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;

    this.getCustomerListByUserType();
    this.getRoleAccess();
  }
  getCustomerListByUserType() {
    this.icService.GetCustomerByCheckPointUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICMasterModel.customerLoading = false;
      }
    );
  }
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.ICMasterModel.customerList = response.customerList.map((x) => {
          var item: DataList = {
            id: x.id,
            name: x.name
          }
          return item;
        });
        this.SearchDetails();
        //if customer login default select the customer
        /*if (this.icuserAccessData.isCustomerUser) {
          this.model.customerId = this.ICMasterModel.customerList.length > 0 ? this.ICMasterModel.customerList[0].id : 0;
        }*/
      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETCUSTLIST');
      }
      this.ICMasterModel.customerLoading = false;
    }
  }
  getData(): void {
    this.GetSearchData();
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.ICMasterModel.selectedPageSize;
      this.search();
    }
  }
  formValid(): boolean {
    return this.validator.isValidIf('serviceToDate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('serviceFromDate', this.IsDateValidationRequired())
  }
  getPathDetails(): string {
    return this.ICMasterModel.redirectPath;
  }
  GetSearchData() {
    this.ICMasterModel.bookingSearchList = null;
    this.ICMasterModel.searchloading = true;
    this.icService.BookingSearch(this.model).subscribe(
      response => {
        this.searchResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICMasterModel.searchloading = false;
      }
    );
  }
  searchResponse(response) {
    if (response) {
      if (response.result == ICBookingSearchResult.Success) {
        this.mapPageProperties(response);
        this.ICMasterModel.bookingSearchList = response.bookingList;
        this.model.noFound = false;
      }
      else if (response && response.result == ICBookingSearchResult.NodataFound) {
        this.model.noFound = true;
        this.ICMasterModel.bookingSearchList = null;
      }
      else if (response && response.result == ICBookingSearchResult.Failure) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_BOOKING_SEARCH_FAILURE');
      }
      else if (response && response.result == ICBookingSearchResult.RequestNotCorrectFormat) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_REQUEST_NOT_CORRECT_FORMAT');
      }
      this.ICMasterModel.searchloading = false;

    }
  }
  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && (this.model.bookingId && this.model.bookingId > 0) ? true : false;

    if (!(this.model.bookingId) || !(this.model.bookingId > 0)) {
      if (!this.model.serviceFromDate)
        this.validator.isValid('serviceFromDate');

      else if (this.model.serviceFromDate && !this.model.serviceToDate)
        this.validator.isValid('serviceToDate');
      else
        isOk = true;
    }
    return isOk;
  }
  Reset() {
    this.initialize();
  }
  PODataSelect(bookingItem: ICBookingSearch) {
    if (bookingItem && bookingItem.productList && bookingItem.productList.length > 0) {

      var productList = bookingItem.productList.filter(x => x.remainingQty > 0 && x.enableCheckbox);
      
      bookingItem.checked = productList.some(function (item: any) {
        return item.checked == true;
      });
    }
    this.SameCusSuppSelectedCount(bookingItem);
  }
  bookingDataSelect(bookingItem: ICBookingSearch) {
    if (bookingItem && bookingItem.productList && bookingItem.productList.length > 0) {

      var validateProduct = bookingItem.productList.filter(x => x.remainingQty > 0 && x.enableCheckbox);

      validateProduct.forEach(function (item: any) {
        item.checked = bookingItem.checked;
      });
    }
    this.SameCusSuppSelectedCount(bookingItem);
  }

  //selection of booking to new ic page should have same customer and supplier
  SameCusSuppSelectedCount(bookingItem: ICBookingSearch) {
    var selectedcount = this.ICMasterModel.bookingSearchList.filter(x => x.checked).length;

    if (selectedcount && selectedcount > 0) {
     
      var count = this.ICMasterModel.bookingSearchList.filter(x => x.checked
        && x.customerId == bookingItem.customerId && x.supplierId == bookingItem.supplierId).length;
     
        if ((count == selectedcount || count == 0)) {
        this.ICMasterModel.isICSelected = this.ICMasterModel.bookingSearchList.filter(x =>
          x.productList.filter(x => x.checked).length).length > 0;
      }
      else {
        this.ICMasterModel.isICSelected = false;
        this.showError('COMMON.LBL_ERROR', 'PENDING_INSPECTION_CERTIFICATE.MSG_SELECTED_ERROR');
      }
    }
    else {
      this.ICMasterModel.isICSelected = false;
    }
  }
  // framing model(ICFromPendingIC) to new ic page
  MapICmodel() {
    if (this.ICMasterModel.bookingSearchList.filter(x =>
      x.productList.filter(x => x.checked).length).length > 0) {
      var bookingIds: number[] = [];
      var poTransIds: number[] = [];
     
      //take first value to get customerid and supplier id
      var icList = this.ICMasterModel.bookingSearchList.filter(x => x.productList.filter(y => y.checked)
        .length)[0];

      //selected product booking id(insptransaction id)
      this.ICMasterModel.bookingSearchList.filter(x => x.productList.filter(y => y.checked).length)
        .forEach(x => bookingIds.push(x.bookingNumber));

      //selected product id(insppotransation id)
      this.ICMasterModel.bookingSearchList.forEach(x => x.productList.filter(y => y.checked).
        forEach(y => poTransIds.push(y.inspPOTransactionId)));

      var item: ICFromPendingIC = {
        customerId: icList.customerId,
        supplierId: icList.supplierId,
        bookingIds: bookingIds,
        inspPoTransactionIds: poTransIds
      };
      
      this.model.selectedCustomerId = icList.customerId;
      this.model.selectedSupplierId = icList.supplierId;
      this.model.bookingIds = bookingIds;
      this.model.inspPoTransactionIds = poTransIds;
      // this.ICMasterModel.icFromPendingIC = item;
    }
    // else
    //   this.ICMasterModel.icFromPendingIC = null;
  }

  //redirection to new ic page and map the model
  redirectToIC() {
    this.ICMasterModel.redirectPath = "inspectioncertificateedit/new-ic";
    this.MapICmodel();
    this.RedirectTo(this.model);
  }

  //redirect url frame to new ic page
  RedirectTo(icFromPendingIC: ICFromPendingIC) {
    var data = Object.keys(icFromPendingIC);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = icFromPendingIC[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}`], {queryParams:{ paramParent: encodeURI(JSON.stringify(currentItem)) }});
  }
  expand(bookingId: number): void {
    this.setExpandCollapse(bookingId, true);
  }

  collapse(bookingId: number): void {
    this.setExpandCollapse(bookingId, false);
  }
  
  //set the variable for expand collapse 
  setExpandCollapse(bookingId: number, isExpand: boolean) {
    if (this.ICMasterModel.bookingSearchList &&
      this.ICMasterModel.bookingSearchList.filter(x => x.bookingNumber == bookingId).length > 0) {
      this.ICMasterModel.bookingSearchList.filter(x => x.bookingNumber == bookingId)[0].isExpand = isExpand;
    }
  }
   //logged user ic role exists
   getRoleAccess() {
    this.userAccountService.loggedUserRoleExists(this.roleEnum.InspectionCertificate).subscribe(
      response => {
        this.ICMasterModel.isICRoleAccess = response;
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    );
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "Fromdate": { 
        this.model.serviceFromDate=null;
        break; 
     } 
     case "Todate": { 
      this.model.serviceToDate=null;
        break; 
     } 
    }
  }
}
