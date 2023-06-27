
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PageSizeCommon, SearchType, DefaultDateType, RoleEnum, bookingSearchtypelst, datetypelst, UserType, BookingSearchRedirectPage, BookingStatus, APIService, BillPaidBy, QuotationSummaryNameLength, ServiceDateLength, QuotationSummaryNameFirstLineLength, SupplierType, ListSize, supplierTypeList, quotationSearchTypeList } from '../../common/static-data-common'
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { of, Subject } from 'rxjs';
import {
  Quotationsummarymodel, QuotationItem, QuotationSummaryResponse, QuotationSummaryResult, QuotationDataSummaryResponse,
  QuotationDataSummaryResult, QuotStatus, StatusChange, QuotationPageType, QuotationCustomerTaskType, InvoiceModel, QuotationMasterModel
} from '../../../_Models/quotation/quotationsummary.model';
import { QuotationService } from '../../../_Services/quotation/quotation.service';
import { DataSource, QuotationDataSourceResponse, QuotationDataSourceResult, QuotationAbility, Service, QuotationStatus, SetStatusRequest, SetStatusQuotationResponse, SetStatusQuotationResult, BookingDateChangeInfo, BookingDateChangeInfoResult } from '../../../_Models/quotation/quotation.model';
import { EditOfficeModel } from '../../../_Models/office/edit-officemodel';
import { TranslateService } from '@ngx-translate/core';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ServiceType } from 'src/app/_Models/Audit/auditcusreportmodel';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CommonCustomerSourceRequest } from 'src/app/_Models/common/common.model';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { commonDataSource } from 'src/app/_Models/booking/inspectionbooking.model';
@Component({
  selector: 'app-quotation-summary',
  templateUrl: './quotationsummary.component.html',
  styleUrls: ['./quotationsummary.component.css'],
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

export class QuotationSummaryComponent extends SummaryComponent<Quotationsummarymodel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  public model: Quotationsummarymodel;
  public invoiceModel: InvoiceModel;
  public customerList: Array<DataSource>;
  public supplierList: Array<DataSource>;
  public factoryList: Array<DataSource>;
  public officeList: Array<EditOfficeModel>;
  public statusList: Array<QuotStatus>;
  public serviceList: Array<Service>;
  public serviceTypeList: Array<DataSource>;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  public ability: any = QuotationAbility;
  isFilterOpen: boolean;
  isUserHasVerifiedRole: boolean;
  pagesizeitems = PageSizeCommon;
  billPaidBy = BillPaidBy;

  selectedPageSize;

  datetypelst: any = datetypelst;
  Initialloading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  public adeoDataLoading = false;
  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = "bookinkNo";
  currentUser: UserModel;
  public _IsInternalUser: boolean = false;
  public _IsCustomerUser: boolean = false;
  _bookingredirectpage = BookingSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public isApproveSummary: boolean;
  public isExternalLogin: boolean;
  public isQuotationToClientPending: boolean;
  public isQuotationRejected: boolean;
  private _router: Router;
  public _status: any = QuotationStatus;
  public ServicedatechangePopup: string;
  statusListColor: any[];
  modalStatus: StatusChange;
  MessagePopup: string;
  popuploading: boolean = false;
  toggleFormSection: boolean;
  _quotationPageType = QuotationPageType;
  _quotationCustomerTaskType = QuotationCustomerTaskType;
  public hasAccountingRole: boolean = false;
  _RoleEnum = RoleEnum;;
  user: UserModel;
  public isInvoiceLoading: boolean = false;
  public isInvoicePopUpLoading: boolean = false;
  isListResultView: boolean;
  isApprove: boolean;
  apiService = APIService;
  masterModel: QuotationMasterModel;
  brandSearchRequest: CommonCustomerSourceRequest;
  deptSearchRequest: CommonCustomerSourceRequest;
  collectionSearchRequest: CommonCustomerSourceRequest;
  buyerSearchRequest: CommonCustomerSourceRequest;
  supplierTypeList: any = supplierTypeList;

  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  @ViewChild('quotationdatechange') servicedatechangepopup: ElementRef;

  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return 'quotations/edit-quotation';
  }

  constructor(private service: QuotationService, public validator: Validator, router: Router,
    route: ActivatedRoute, authserve: AuthenticationService, public modalService: NgbModal, private userAccountService: UserAccountService,
    public calendar: NgbCalendar, public dateparser: NgbDateParserFormatter, public pathroute: ActivatedRoute,
    public utility: UtilityService, translate: TranslateService,
    toastr: ToastrService, public brandService: CustomerBrandService,
    public deptService: CustomerDepartmentService,
    public buyerService: CustomerbuyerService,
    public referenceService: ReferenceService, private supService: SupplierService) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
    this.currentRoute = router;
    this.isApproveSummary = false;
    this._router = router;
    this.isExternalLogin = false;
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.getIsMobile();
    this.isListResultView = this._IsInternalUser ? true : false;
    this.masterModel = new QuotationMasterModel();
    this.masterModel.entityId=parseInt(this.utility.getEntityId());
    this.masterModel.isCustomerLogin = this.currentUser.usertype == UserType.Customer;
  }
  onInit(): void {
    this.Initialize();
    this.validator.setJSON("quotation/quotation-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.isApproveSummary = this._router.url.indexOf("quotation-approve") >= 0;
    this.isExternalLogin = this._router.url.indexOf("quotation-confirm") >= 0;
    this.isQuotationToClientPending = this._router.url.indexOf("quotation-clientpending") >= 0;
    this.isQuotationRejected = this._router.url.indexOf("quotation-rejected") >= 0;
    this.getRoleAccess();
    this.assignSupplierDetails();
  }
  async Initialize() {

    if (localStorage.getItem('currentUser')) {
      this.user = JSON.parse(localStorage.getItem('currentUser'));
      var index = this.user.roles.findIndex(x => x.id == this._RoleEnum.Accounting);
      if (index != -1) {
        this.hasAccountingRole = true;
      }
    }

    this.model = new Quotationsummarymodel();
    this.invoiceModel = new InvoiceModel();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.collectionSearchRequest = new CommonCustomerSourceRequest();
    this.buyerSearchRequest = new CommonCustomerSourceRequest();
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    if (this._IsCustomerUser)
      this.model.customerid = this.currentUser.customerid;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = "serviceDate";
    this.model.advancedSearchtypeid = "factoryRef"
    this.model.pageSize = 10;
    this.model.index = 0;
    this.data = [];
    this.Initialloading = true;
    this.modalStatus = new StatusChange();
    this.MessagePopup = "";
    this.statusListColor = [];
    this.masterModel.searchQuotationNumber = this.pathroute.snapshot.paramMap.get("id");
    this.masterModel.quotationSearchTypeList = quotationSearchTypeList;
    let response: QuotationSummaryResponse;

    try {
      response = await this.service.getQuotationSummary();
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.Initialloading = false;

    if (response) {
      switch (response.result) {
        case QuotationSummaryResult.Success:
          this.customerList = response.customerList;
          this.officeList = response.officeList;
          this.statusList = response.statusList;
          this.serviceList = response.serviceList;
          if (!this.model.serviceId && this.serviceList) {
            this.model.serviceId = this.serviceList.filter(x => x.id == APIService.Inspection)[0]?.id;
          }

          this.GetServiceTypelist();

          if (this.model.customerid > 0) {
            this.masterModel.supsearchRequest.customerId = this.model.customerid;
            this.brandSearchRequest.customerId = this.model.customerid;
            this.deptSearchRequest.customerId = this.model.customerid;
            this.getSupplierListBySearch()
            this.getBrandListBySearch();
            this.getDeptListBySearch();
          }
          if (this.model.supplierid > 0)
            this.GetFactoryList(this.model.customerid);

          if (this.model.serviceId > 0)
            this.GetServiceTypelist();

          if (this.isApproveSummary || this.isExternalLogin || this.isQuotationToClientPending || this.isQuotationRejected) {
            this.CheckRequestStatus();
          }
          break;
        case QuotationSummaryResult.CustomerListNotFound:
          break;
      }
      /* setTimeout(() => {
        this.limitTableHeight();
      }, 5); */

    }
    //check if it is coming from the customer quotation task dashboard
    this.checkCustomerTask();
  }

  ChangeCustomer(cusitem: DataSource) {
    this.assignSupplierDetails();
    this.model.supplierid = null;
    this.model.factoryidlst = [];
    this.model.serviceTypelst = [];
    if (cusitem != null && cusitem.id != null) {

      this.masterModel.brandList = [];
      this.masterModel.deptList = [];
      this.masterModel.buyerList = [];

      //clear the selected values
      this.model.brandIdList = [];
      this.model.deptIdList = [];
      this.model.buyerIdList = [];

      this.brandSearchRequest.customerId = cusitem.id;
      this.deptSearchRequest.customerId = cusitem.id;
      this.buyerSearchRequest.customerId = cusitem.id;
      this.masterModel.supsearchRequest.customerId = cusitem.id;
      this.getSupplierListBySearch();
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getDeptListBySearch();

      if (this.model.serviceId) {
        this.GetServiceTypelist();
      }
    }
  }
  ChangeSupplier(supitem) {
    this.model.factoryidlst = [];
    if (supitem != null && supitem.id != null) {
      this.GetFactoryList(supitem.id);
    }
  }

  //logged in user has verified role access
  getRoleAccess() {
    this.userAccountService.loggedUserRoleExists(this._RoleEnum.InspectionVerified).subscribe(
      response => {
        this.isUserHasVerifiedRole = response;
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    );
  }

  async GetFactoryList(id) {
    if (id) {
      this.factloading = true;

      let response: QuotationDataSourceResponse;

      try {
        response = await this.service.getFactoryList(id);
      }
      catch (e) {
        console.error(e);
        this.setError(e);
      }

      this.factloading = false;

      if (response && response.result == QuotationDataSourceResult.Success) {
        this.factoryList = response.dataSource;
      }

    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
    }
  }

  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this.masterModel.bookingSearchTypeId
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }
  Reset() {
    this.Initialize();
  }
  SetSearchTypemodel(item) {
    this.model.searchtypeid = item.id;
  }
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }
  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }
  SearchDetails() {
    this.validator.initTost();
    this.model.invoiceNo = " ";
    this.model.invoiceDate = this.calendar.getToday();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleResultContainer() {
    if (!this._IsInternalUser)
      this.isListResultView = !this.isListResultView;
  }
  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;

    if (this.model.searchtypetext == null || this.model.searchtypetext == "") {
      if (!this.model.fromdate)
        this.validator.isValid('fromdate');

      else if (this.model.fromdate && !this.model.todate)
        this.validator.isValid('todate');
    }
    return isOk;
  }
  formValid(): boolean {
    let isOk = !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('customerid', this.model.advancedsearchtypetext)

    if (this.model.advancedsearchtypetext && !this.model.customerid) {
      this.validator.isValid('customerid');
    }


    return isOk;
  }

  async GetSearchData() {
    this.searchloading = true;
    this.model.invoiceNo = "";

    let response: QuotationDataSummaryResponse;
    try {
      response = await this.service.getQuotationDataSummary(this.model);
    }
    catch (e) {
      console.error(e);
      this.setError(e);
    }

    this.searchloading = false;

    if (response) {
      if (response.result == QuotationDataSummaryResult.Success) {
        this.mapPageProperties(response);
        this.statusListColor = response.statusList;
        var data = [];

        response.data.forEach(element => {
          //supplier name trim based on space in letters with first line and second line
          if (element.supplierName.substring(0, QuotationSummaryNameFirstLineLength).indexOf(' ') >= 0) {
            if (element.supplierName.length > QuotationSummaryNameLength) {
              element.supplierNameTrim = element.supplierName.substring(0, QuotationSummaryNameLength) + "..";
            }
            else {
              element.supplierNameTrim = element.supplierName;
            }
          }
          else {
            if (element.supplierName.length > QuotationSummaryNameFirstLineLength) {
              element.supplierNameTrim = element.supplierName.substring(0, QuotationSummaryNameFirstLineLength) + " " +
                element.supplierName.substring(QuotationSummaryNameFirstLineLength, QuotationSummaryNameLength) + "..";
            }
            else {
              element.supplierNameTrim = element.supplierName;
            }
          }

          if (element.factoryName.substring(0, QuotationSummaryNameFirstLineLength).indexOf(' ') >= 0) {
            if (element.factoryName.length > QuotationSummaryNameLength) {
              element.factoryNameTrim = element.factoryName.substring(0, QuotationSummaryNameLength) + "..";
            }
            else {
              element.factoryNameTrim = element.factoryName;
            }
          }
          else {
            if (element.factoryName.length > QuotationSummaryNameFirstLineLength) {
              element.factoryNameTrim = element.factoryName.substring(0, QuotationSummaryNameFirstLineLength) + " " +
                element.factoryName.substring(QuotationSummaryNameFirstLineLength, QuotationSummaryNameLength) + "..";
            }
            else {
              element.factoryNameTrim = element.factoryName;
            }
          }

          element.serviceDateListTrim = element.serviceDateList.length > ServiceDateLength ? element.serviceDateList.substring(0, ServiceDateLength) + ".." : element.serviceDateList;

          element.validatedMsg = element.statusId == QuotationStatus.CustomerValidated ?
            element.validatedBy == UserType.InternalUser ?
              this.utility.getEntityName() :
              element.validatedBy == UserType.Customer ?
                this.utility.textTranslate('QUOTATION_SUMMARY.LB_CUSTOMER') + " (" + element.validatedUserName + ")" :
                element.validatedBy == UserType.Supplier ?
                  this.utility.textTranslate('QUOTATION_SUMMARY.LBL_SUPPLIER') + " (" + element.validatedUserName + ")" :
                  element.validatedBy == UserType.Factory ?
                    this.utility.textTranslate('QUOTATION_SUMMARY.LBL_FACTORY') + " (" + element.validatedUserName + ")" :
                    ""
            : "";

          element.validatedMsgTbl = element.statusId == QuotationStatus.CustomerValidated ?
            element.validatedBy == UserType.InternalUser ?
              this.utility.getEntityName() + " (" + element.validatedUserName + ")" :
              element.validatedBy == UserType.Customer ?
                this.utility.textTranslate('QUOTATION_SUMMARY.LB_CUSTOMER') + " (" + element.validatedUserName + ")" :
                element.validatedBy == UserType.Supplier ?
                  this.utility.textTranslate('QUOTATION_SUMMARY.LBL_SUPPLIER') + " (" + element.validatedUserName + ")" :
                  element.validatedBy == UserType.Factory ?
                    this.utility.textTranslate('QUOTATION_SUMMARY.LBL_FACTORY') + " (" + element.validatedUserName + ")" :
                    ""
            : "";

          data.push(element);
        });
        this.model.items = response.data;
      }
      else if (response.result == QuotationDataSummaryResult.NotFound) {
        this.model.noFound = true;
      }
      else {
        this.error = response.result;
      }

      setTimeout(() => {
        this.limitTableHeight();
      }, 500);
    }

  }

  SearchByStatus(id) {
    this.model.statusidlst = [];
    this.model.statusidlst.push(id);
    this.SearchDetails();
  }
  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });

  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "quotation.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
    this.adeoDataLoading = false;
  }
  //RedirectPage(type, bookingid) {
  //  switch (type) {
  //    case this._bookingredirectpage.Edit:
  //      {
  //        this._redirectpath = "inspedit/edit-booking";
  //        super.getDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.Cancel:
  //      {
  //        this._redirectpath = "inspcancel/cancel-booking";
  //        this._redirecttype = this._bookingredirectpage.Cancel;
  //        this.getDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.Reschedule:
  //      {
  //        this._redirectpath = "inspcancel/cancel-booking";
  //        this._redirecttype = this._bookingredirectpage.Reschedule;
  //        this.getDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.Report:
  //      {
  //        this._redirectpath = "booking-report";
  //        super.getDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.Schedule:
  //      {
  //        this._redirectpath = "inspedit/edit-booking";
  //        this._redirecttype = this._bookingredirectpage.Schedule;
  //        this.getDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.CombineProduct:
  //      {
  //        this._redirectpath = "inspcombine/edit-combineorders";
  //        this.getCombineProductDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.SplitBooking:
  //      {
  //        this._redirectpath = "inspsplit/split-booking";
  //        this.getCombineProductDetails(bookingid);
  //        break;
  //      }
  //    case this._bookingredirectpage.ReInspectionBooking:
  //      {
  //        this._redirectpath = "editreinspection-booking";
  //        this._redirecttype = this._bookingredirectpage.ReInspectionBooking;
  //        this.getDetails(bookingid);
  //        break;
  //      }
  //  }
  //}
  getDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }


  getCombineProductDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  getPickingProductDetails(id, customerId, customerName) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${customerId}/${customerName}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  RedirectPickingProducts(bookingid, customerId, customerName) {
    this._redirectpath = "insppicking/edit-inspectionpicking";
    this.getPickingProductDetails(bookingid, customerId, customerName);
  }

  async setStatus(successModal) {
    this.popuploading = true;
    let request = new SetStatusRequest();
    // console.log("idStatus:" + idStatus);
    if (this.modalStatus && this.modalStatus.item && this.modalStatus.quotationStatus &&
      this.modalStatus.item.quotationId) {
      request.id = this.modalStatus.item.quotationId;
      request.statusId = this.modalStatus.quotationStatus;
      request.cuscomment = this.modalStatus.item.customerRemark;
      request.confirmDate = this.calendar.getToday();
    }
    let response: SetStatusQuotationResponse;

    try {
      response = await this.service.setStatus(request);
    }
    catch (e) {
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }

    if (response) {
      switch (response.result) {
        case SetStatusQuotationResult.Success:
          if (this._IsInternalUser) {
            this.refresh();
          }
          else {
            //success popup
            this.modalService.open(successModal, { windowClass: "smModelWidth", centered: true, backdrop: true });

            //current record status and validate message field updated
            var quotationItem = this.model.items.find(x => x.quotationId == this.modalStatus.item.quotationId);

            if (quotationItem) {
              quotationItem.statusId = response.item.statusId;

              if (response.item.statusId == QuotationStatus.CustomerValidated) {
                quotationItem.validatedMsg =
                  this.utility.textTranslate('QUOTATION_SUMMARY.LBL_VALIDATED_BY') + " " + (this._IsInternalUser ?
                    this.utility.getEntityName() :
                    this.currentUser.fullName ?
                      this.utility.textTranslate('QUOTATION_SUMMARY.LBL_BY') + " " + this.currentUser.fullName : "");
              }
            }
          }
          break;
        case SetStatusQuotationResult.CannotUpdateStatus:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTUPDATESTATUS');
          break;
        case SetStatusQuotationResult.NoAccess:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_NORIGHTSTOUPDATESTATUS');
          break;
        case SetStatusQuotationResult.StatusNotConfigued:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWSTATUS');
          break;
        case SetStatusQuotationResult.QuotationNotFound:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_QUOTATIONNOTFOUND');
          break;
        case SetStatusQuotationResult.ServiceDateChanged:
          {
            this.modelRef.close();
            this.openServiceDateChangePopup(response.serviceDateChangeInfo);
            break;
          }
      }
    }
    this.modalStatus = new StatusChange();
    if (SetStatusQuotationResult.ServiceDateChanged != response.result)
      this.modelRef.close();
    this.popuploading = false;
  }
  openServiceDateChangePopup(bookdatechangeinfo: BookingDateChangeInfo) {
    if (bookdatechangeinfo.result == BookingDateChangeInfoResult.DateChanged) {
      this.ServicedatechangePopup = (this.utility.textTranslate('QUOTATION.MSG_BOOKING') + "#" + bookdatechangeinfo.bookingId +
        this.utility.textTranslate('QUOTATION.MSG_SERVICE_DATE') + bookdatechangeinfo.previousServiceDateFrom + " - " + bookdatechangeinfo.previousServiceDateTo +
        this.utility.textTranslate('QUOTATION.MSG_SERVICEDATETO') + bookdatechangeinfo.serviceDateFrom + " - " + bookdatechangeinfo.serviceDateTo + ".");

      this.modelRef = this.modalService.open(this.servicedatechangepopup, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
    else {
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTSAVE');
    }
  }
  GetStatusColor(statusid?) {

    if (this.statusListColor != null && this.statusListColor.length > 0 && statusid != null) {
      var result = this.statusListColor.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  openModalPopup(quotationstatus, idStatus: QuotationStatus, item: QuotationItem, isApprove: boolean) {
    this.modalStatus = {
      quotationStatus: idStatus,
      item: item
    };
    this.isApprove = isApprove;
    this.ModalPopupContentMessage(idStatus);
    this.modelRef = this.modalService.open(quotationstatus, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {
    }, (reason) => {

    });
  }
  // show Message content in popup based on quotation status
  ModalPopupContentMessage(idStatus) {
    if (idStatus == QuotationStatus.ManagerApproved)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_APPROVE');
    else if (idStatus == QuotationStatus.Canceled)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.LBL_CANCEL');
    else if (idStatus == QuotationStatus.CustomerRejected)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_REJECT');
    else if (idStatus == QuotationStatus.ManagerRejected)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_REJECT');
    else if (idStatus == QuotationStatus.CustomerValidated)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_APPROVE');
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  //buyer field shown when not audit service
  isBuyerFieldEnable(serviceId) {
    //if audit service hide the buyer field in search column
    if (serviceId && serviceId > 0 && serviceId == APIService.Audit) {
      this.model.buyerIdList = [];
      this.masterModel.buyerList = [];
      this.masterModel.isAuditService = true;
    }
    else {
      this.masterModel.isAuditService = false;
      if (this.model.customerid && this.model.customerid > 0) {
        this.buyerSearchRequest.customerId = this.model.customerid;
        this.getBuyerListBySearch();
      }
    }
  }

  async GetServiceTypelist() {

    this.isBuyerFieldEnable(this.model.serviceId);

    if (this.model.serviceId) {


      this.masterModel.supplierLoading = true;

      let response: QuotationDataSourceResponse;

      try {
        this.masterModel.servicetypeLoading = true;
        let request = this.generateServiceTypeRequest();
        this.referenceService.getServiceTypes(request)
          .pipe(takeUntil(this.componentDestroyed$), first())
          .subscribe(
            response => {

              this.serviceTypeList = response.serviceTypeList;
              this.masterModel.servicetypeLoading = false;
            },
            error => {
              this.setError(error);
              this.serviceTypeList = [];
              this.masterModel.servicetypeLoading = false;
            }
          );
      }
      catch (e) {
        console.error(e);
        this.setError(e);
      }

      this.masterModel.supplierLoading = false;
    }
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerid ?? 0;
    serviceTypeRequest.serviceId = this.model.serviceId;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  ClearServiceType() {
    this.serviceTypeList = [];
    this.model.serviceTypelst = null;
  }

  ChangeService() {
    this.serviceTypeList = [];
    this.model.serviceTypelst = null;
    if (this.model.serviceId) {
      this.GetServiceTypelist();
    }
  }

  ClearCustomer() {
    this.assignSupplierDetails();
    this.serviceTypeList = [];
    this.model.serviceTypelst = null;

    this.model.brandIdList = null;
    this.model.deptIdList = null;
    this.model.buyerIdList = null;
    
    this.masterModel.brandList = [];
    this.masterModel.deptList = [];
    this.masterModel.buyerList = [];
    
    if (this.model.serviceId) {
      this.GetServiceTypelist();
    }
  }

  limitTableHeight() {

    // this.scrollableTable.nativeElement.classList.add('scroll-x'); 

  }

  CheckRequestStatus() {
    if (this.isApproveSummary) {
      this.model.statusidlst = this.statusList.filter(x => x.id == QuotationStatus.QuotationCreated).map(x => x.id);
    }
    else if (this.isExternalLogin)//show only status customer confirmed, sent , rejected status
    {
      this.statusList = this.statusList.filter(x => x.id == QuotationStatus.CustomerValidated || x.id == QuotationStatus.CustomerRejected || x.id == QuotationStatus.SentToClient);
      this.model.statusidlst = this.statusList.filter(x => x.id == QuotationStatus.SentToClient).map(x => x.id);
    }
    else if (this.isQuotationToClientPending) {
      this.model.statusidlst = this.statusList.filter(x => x.id == QuotationStatus.ManagerApproved || x.id == QuotationStatus.CustomerRejected || x.id == QuotationStatus.QuotationVerified).map(x => x.id);
    }
    else if (this.isQuotationRejected) {
      this.model.statusidlst = this.statusList.filter(x => x.id == QuotationStatus.ManagerRejected || x.id == QuotationStatus.AERejected).map(x => x.id);
    }
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'm', 3);
    this.model.todate = this.calendar.getNext(this.calendar.getToday(), 'm', 3);
    this.model.serviceId = APIService.Inspection
    this.search();
  }

  checkCustomerTask() {
    var type = this.pathroute.snapshot.paramMap.get("type");
    //check page comes from customer task dashboard
    if (parseInt(type) == this._quotationPageType.CustomerTaskDashboard) {
      var customerTaskType = this.pathroute.snapshot.paramMap.get("customertasktype");
      this.model.statusidlst = [];
      this.model.serviceId = APIService.Inspection;
      //check the task type and update the status accordingly
      if (parseInt(customerTaskType) == this._quotationCustomerTaskType.PendingQuotations) {
        this.model.statusidlst.push(QuotationStatus.SentToClient);
      }
      else if (parseInt(customerTaskType) == this._quotationCustomerTaskType.CompletedQuotations) {
        this.model.statusidlst.push(QuotationStatus.CustomerValidated);
      }
      this.loadDataByQuotationNumber();
    }
    else {
      this.loadDataByQuotationNumber();
    }
  }

  //load the search data by quotation number
  loadDataByQuotationNumber() {
    if (this.masterModel.searchQuotationNumber && this.masterModel.searchQuotationNumber != '') {
      this.model.searchtypetext = this.masterModel.searchQuotationNumber;
      this.model.searchtypeid = SearchType.QuotationNo;
      this.GetSearchData();
    }
  }

  getClientQuotation(id) {
    this.adeoDataLoading = true;
    this.service.getClientQuotation(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_NOTFOUND');
          this.adeoDataLoading = false;
        });

  }

  openInvoicePopUp(invoice, quotationId) {
    this.isInvoicePopUpLoading = true;
    this.service.getInvoice(quotationId, this.model.serviceId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        res => {
          if (res != null) {
            this.model.invoiceNo = res.invoiceNo,
              this.model.invoiceDate = res.invoiceDate;
            this.invoiceModel.invoiceRemarks = res.invoiceREmarks;
            this.isInvoicePopUpLoading = false;
          }
          else {
            this.model.invoiceNo = null;
            this.model.invoiceDate = null;
          }
        }
      )

    this.validator.isSubmitted = false;
    this.invoiceModel.quotationId = quotationId;
    this.invoiceModel.Service = this.model.serviceId;
    this.modelRef = this.modalService.open(invoice, { windowClass: "smModelWidth", centered: true, backdrop: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }

  invoiceSave() {
    this.validator.isSubmitted = true;
    this.isInvoiceLoading = true;
    if (this.isInvoicePopUpValid()) {

      this.modelRef.close();
      this.invoiceModel.invoiceNo = this.model.invoiceNo;
      this.invoiceModel.invoiceDate = this.model.invoiceDate;
      this.service.saveInvoice(this.invoiceModel)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          res => {
            if (res.result == 1)
              this.showSuccess('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'QUOTATION.MSG_INVOICE_SAVED');
            this.resetInvoiceModel();
          },
          error => {
            this.setError(error);
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'QUOTATION.MSG_CANNOTSAVE');
          });
      this.isInvoiceLoading = false;
    }

  }

  isInvoicePopUpValid() {
    return this.validator.isValid('invoiceDate') && this.validator.isValid('invoiceNo');
  }

  resetInvoiceModel() {
    this.isInvoiceLoading = false;
    this.modelRef.close();
    this.model.invoiceDate = null;
    this.model.invoiceNo = "";
    this.invoiceModel.invoiceRemarks = null;
  }
  preview(item) {
    item.isPreview = true;
    this.service.preview(item.quotationId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.filePath) {
          item.isPreview = false;
          window.open(res.filePath);
        }
      },
        error => {
          item.isPreview = false;
          console.log(error);
        });
  }
  preivewFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "Fromdate": {
        this.model.fromdate = null;
        break;
      }
      case "Todate": {
        this.model.todate = null;
        break;
      }
    }
  }
  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }


  //fetch the brand data with virtual scroll
  getBrandData() {
    this.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    this.brandSearchRequest.skip = this.masterModel.brandList.length;

    this.masterModel.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.masterModel.brandList = this.masterModel.brandList.concat(brandData);
        }
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.brandLoading = false;
      }),
      error => {
        this.masterModel.brandLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.masterModel.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.brandLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.brandList = data;
        this.masterModel.brandLoading = false;
      });
  }

  //fetch the dept data with virtual scroll
  getDeptData() {
    this.deptSearchRequest.searchText = this.masterModel.deptInput.getValue();
    this.deptSearchRequest.skip = this.masterModel.deptList.length;

    this.masterModel.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.masterModel.deptList = this.masterModel.deptList.concat(deptData);
        }
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.deptLoading = false;
      }),
      error => {
        this.masterModel.deptLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count dept on load
  getDeptListBySearch() {
    this.masterModel.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.deptLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.deptList = data;
        this.masterModel.deptLoading = false;
      });
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.buyerSearchRequest.searchText = this.masterModel.buyerInput.getValue();
    this.buyerSearchRequest.skip = this.masterModel.buyerList.length;

    this.masterModel.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.masterModel.buyerList = this.masterModel.buyerList.concat(buyerData);
        }
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.buyerLoading = false;
      }),
      error => {
        this.masterModel.buyerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.masterModel.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.buyerLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.buyerList = data;
        this.masterModel.buyerLoading = false;
      });
  }
  changeSupplierType(item) {
    this.masterModel.supplierLoading = true;
    this.masterModel.supplierList = [];
    this.model.supplierid = null;
    this.factoryList = [];
    this.model.factoryidlst = [];
    if (item.id == SearchType.SupplierCode) {
      this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;

    }
    else if (item.id == SearchType.SupplierName) {
      this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    }

    this.getSupplierData();
  }
  assignSupplierDetails() {
    this.masterModel.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
  }
  getSupplierListBySearch() {
    this.masterModel.supsearchRequest.supplierId = null;
    if (this.model.customerid)
      this.masterModel.supsearchRequest.customerId = this.model.customerid;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.masterModel.supsearchRequest.supplierId = this.model.supplierid;
    this.masterModel.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supplierLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supsearchRequest.searchText = this.masterModel.supInput.getValue();
    this.masterModel.supsearchRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supsearchRequest.customerId = this.model.customerid;
    this.masterModel.supsearchRequest.supplierType = SupplierType.Supplier;
    this.masterModel.supplierLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.supsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(customerData);
        }
        this.masterModel.supsearchRequest.skip = 0;
        this.masterModel.supsearchRequest.take = ListSize;
        this.masterModel.supplierLoading = false;
      }),
      (error: any) => {
        this.masterModel.supplierLoading = false;
      };
  }
  clearSupplier() {
    this.model.factoryidlst = [];
    this.factoryList = [];
  }
}
