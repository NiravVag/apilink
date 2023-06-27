import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PageSizeCommon, SearchType, DefaultDateType, bookingSearchtypelst, datetypelst, UserType, BookingSearchRedirectPage, BookingStatus, RoleEnum, InspectionServiceType, PageTypeSummary, ListSize, SupplierType, FileContainerList, InspectedStatusList, InspectedStatusData, FBStatus, ControlType, ControlAttribute, DynamicControlModuleType, DFDDLSourceType } from '../../common/static-data-common'
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionBookingsummarymodel, BookingItem, HolidayRequest, BookingReportItem } from '../../../_Models/booking/inspectionbookingsummarymodel';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { InternalFBReportService } from 'src/app/_Services/booking/fbreport.service';
import { ReportService } from 'src/app/_Services/Report/report.service';
import { TranslateService } from '@ngx-translate/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CustomReportMaster, ReportMaster, UploadCustomReport, UploadCustomReportRequest, UploadCustomReportResult } from 'src/app/_Models/Report/reportmodel';
import { of, Subject } from 'rxjs';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { DfCustomerConfigurationRequest } from 'src/app/_Models/dynamicfields/dfcustomerconfigsummary.model';
import { DynamicFieldService } from 'src/app/_Services/dynamicfields/dynamicfield.service';
import { DropDownControl } from 'src/app/_Models/dynamiccontrols/control-dropdown.model';
import { ControlAttributes } from 'src/app/_Models/dynamiccontrols/dynamicontrolbase.model';
import { InspectionDFTransaction } from 'src/app/_Models/booking/inspectionbooking.model';

@Component({
  selector: 'app-report-summary',
  templateUrl: './report-summary.component.html',
  styleUrls: ['./report-summary.component.scss'],
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

export class ReportSummaryComponent extends SummaryComponent<InspectionBookingsummarymodel> {
  public model: InspectionBookingsummarymodel;
  public data: any;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  public modelConfirmCreateReportRef: NgbModalRef;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  bookingSearchtypelst: any = bookingSearchtypelst;
  datetypelst: any = datetypelst;
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  public containerMasterList: any[] = [];

  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  public _statuslist: any[] = [];
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  _IsInspectionConfirmed: boolean = false;
  _isQCUser: boolean = false;
  _isReportCheckUser: boolean = false;
  _isCustomer: boolean = false;
  _isOtherUser: boolean = false;
  _bookingredirectpage = BookingSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _BookingStatus = BookingStatus;
  _searchbookingtid: any;
  isReInspection: boolean = false;
  isReBooking: boolean = false;
  cancelLoading: boolean;
  titlePopup: string;
  isPreviewLoading: boolean = false;
  isPDFLoading: boolean = false;
  roleEnum = RoleEnum;
  inspectionServiceType = InspectionServiceType;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  reportMaster: ReportMaster;
  componentDestroyed$: Subject<boolean> = new Subject();
  public uploadfileimage: string = "assets/images/uploaded-files.svg";
  public smallSignatureTitle = "Upload Report File";
  public smallUploadButtonText = "Add File";
  public uploadLimit: number;
  public fileSize: number;
  public uploadFileExtensions: string;
  public pageLoader: boolean = false;
  uploadCustomReport: UploadCustomReport;
  uploadCustomReportLoading: boolean = false;
  uploadCustomReportRequest: UploadCustomReportRequest;

  _fbStatus = FBStatus;

  @ViewChild('cancelresheduleBooking') cancelresheduleBooking: ElementRef;
  _controlType = ControlType;
  _controlAttribute = ControlAttribute;
  _dynamicControlModuleType = DynamicControlModuleType;
  dynamicControls: any;
  dynamicControlBaseData: any;

  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {

    return this._redirectpath;
  }

  constructor(private service: BookingService, public validator: Validator, router: Router,
    private fbService: InternalFBReportService,
    route: ActivatedRoute, authserve: AuthenticationService,
    public accService: UserAccountService,
    public modalService: NgbModal, public calendar: NgbCalendar,
    public dateparser: NgbDateParserFormatter, private reportService: ReportService,
    public pathroute: ActivatedRoute, public utility: UtilityService, public customerService: CustomerService,
    public officeService: OfficeService, private supplierService: SupplierService,
    translate: TranslateService,
    toastr: ToastrService, public dynamicFieldService: DynamicFieldService,
    private userAccountService: UserAccountService, public fileStoreService: FileStoreService) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.currentRoute = router;
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.reportMaster = new ReportMaster();
    this.reportMaster.entityId= parseInt(this.utility.getEntityId());
    this.uploadCustomReport = new UploadCustomReport();
    this.getIsMobile();
    this.fileSize = 5000000;
    this.uploadLimit = 1;
    this.uploadFileExtensions = 'jpg,jpeg,xlsx,xls,pdf,doc,docx';
  }
  onInit(): void {

    this.Initialize();
    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
  }
  //logged user role exists
  async getRoleAccess(): Promise<boolean> {

    let response: boolean;

    try {
      response = await this.userAccountService.loggedUserRoleExist(this.roleEnum.ReportChecker);
    }
    catch (e) {
      console.error(e);
      // this.showError('BOOKING_SUMMARY.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }
    return response;
  }

  async getUserProfile() {
    // check qc is availble
    var today = new Date();
    var fromdate: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', -1);

    var todates: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', +1);

    this.model.fromdate = fromdate;

    this.model.todate = todates;

    if (this.currentUser.profiles.filter(x => x == 4).length > 0) {
      this._isQCUser = true;

    }
    else if (this.currentUser.profiles.filter(x => x == 11).length > 0) //11 - report checker profile
    {
      this._isQCUser = false;
      this._isReportCheckUser = true;
    }
    else if (this.currentUser.roles.filter(x => x.id == this.roleEnum.ReportChecker).length > 0) {
      this._isReportCheckUser = true;
    }
    else {
      this._isQCUser = false;
      this._isReportCheckUser = false;
      this._isOtherUser = true;
      this.model.fromdate = null;
      this.model.todate = null;
    }

  }

  Initialize() {

    var type = this.pathroute.snapshot.paramMap.get("type");
    if (type) {
      this.isReInspection = (type == "1") ? true : false;
      this.isReBooking = (type == "2") ? true : false;
    }

    this.containerMasterList = this.utility.getContainerList(100);

    this._searchbookingtid = this.pathroute.snapshot.paramMap.get("id");
    this.model = new InspectionBookingsummarymodel();
    this.getUserProfile();
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.pageSize = 10;
    this.model.index = 1;
    this.data = [];
    this._statuslist = [];
    this.dynamicControls = [];
    this.getCustomerListBySearch();
    this.getBookingStatusList();
    this.getOfficeLocationList();

    //if qc user or report checker fetch only the bookings belongs to them
    if (this._isQCUser || this._isReportCheckUser) {
      this.model.statusidlst = [this._BookingStatus.AllocateQC];
      this.GetSearchData();
    }
  }

  //get the supplier list based on customer change
  ChangeCustomer(cusitem) {
    this.dynamicControls = [];
    this.model.inspectionDFTransactions = [];
    if (cusitem != null && cusitem.id != null) {
      this.reportMaster.supplierList = [];
      this.reportMaster.factoryList = [];
      this.model.supplierid = null;
      this.model.factoryidlst = [];
      this.getSupplierListBySearch();
      this.GetCustomerDynamicFields();
    }
  }

  getUniqueColorList() {
    var resultColorSets = ['FFC1C1', '87CEEB', 'D3D3D3', 'FAF0E6', 'FFE4C4', 'FFDEAD', 'FFFACD', 'F0FFF0',
      'E6E6FA', 'FFE4E1', 'ADD8E6', 'AFEEEE', '48D1CC', 'E0FFFF', '66CDAA', '7FFFD4',
      '8FBC8F', '98FB98', 'BDB76B', 'F0E68C', 'FAFAD2', 'EEDD82', 'BC8F8F', 'D2B48C', 'FFB6C1',
      'DDA0DD', '8EE5EE', '9AFF9A', 'EEEE00', 'FFC1C1', '9381FF', 'FFD8BE', 'FFEEDD', '9986DF', 'CCAFCF',
      'F2CEC7', 'F686BD', 'F4BBD3', 'D6D2D2', 'FE5D9F', 'F5A1C8', 'F3D0E3', 'EDF5FC', 'B8C5D6', 'A39BA8',
      '5ADA8F', 'D3DDE9', '00B295', 'C9DAEA', 'C05A74'];

    return resultColorSets;
  }

  getUniqueCombineOrder(productList) {
    const resultCombineOrders = [];
    const map = new Map();
    if (productList) {
      for (const item of productList) {
        if (item.combineProductId != null && item.combineProductId != 0 && !map.has(item.combineProductId)) {
          map.set(item.combineProductId, true);    // set any value to Map
          resultCombineOrders.push({
            id: item.combineProductId
          });
        }

      }
    }

    return resultCombineOrders;
  }

  applyGroupColor() {

    var count = 0;

    var uniqueColorList = this.getUniqueColorList();

    this.model.items.forEach(row => {

      count = 0;

      var uniqueCombineProductList = this.getUniqueCombineOrder(row.productList);

      uniqueCombineProductList.forEach(item => {

        row.productList.forEach(element => {

          if (element.combineProductId == item.id) {
            element.colorCode = "#" + uniqueColorList[count];
          }

        });
        count = count + 1;

      });
    });
  }

  RedirectToFbReport(reportId) {
    this.accService.getUserTokenToFB()
      .subscribe(
        response => {
          if (response != null) {
            window.open(response.reportUrl + reportId + "?token=" + response.token + "", "_blank");
          }
        },
        error => {
          this.suploading = false;
        }
      );
  }

  //open the custom report
  openCustomReport(content, customProductContent, productData) {
    this.uploadCustomReportRequest = new UploadCustomReportRequest();

    //if productdata is available
    if (productData) {
      //apply the apiReportId
      this.uploadCustomReportRequest.apiReportId = productData.apiReportId;
      //if fb report data is available and final manual report data is not there then ask for
      //confirmation before upload the fb report
      if (productData.finalReportLink && !productData.finalManualReportLink) {
        //ask for the confirmation popup(you want to overwrit the existing fb report)
        this.modelConfirmCreateReportRef = this.modalService.open(content, { windowClass: "confirmModal" });
      }
      else {
        //call the upload custom report popup
        this.modelRef = this.modalService.open(customProductContent, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
      }
    }
  }

  //show the upload custom report popup
  showUploadReportPopup(content) {
    this.modelConfirmCreateReportRef.close();
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  //get factory list based on supplier change
  ChangeSupplier(supitem) {
    if (supitem != null && supitem.id != null) {
      this.reportMaster.factoryList = [];
      this.model.factoryidlst = [];
      this.getFactoryListBySearch();
    }
  }

  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }


  Reset() {
    this.reportMaster.supplierList = [];
    this.reportMaster.factoryList = [];
    this.Initialize();
  }
  SetSearchTypemodel(searchtype) {
    this.model.searchtypeid = searchtype;
  }
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }
  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
  
    //add customer configured value to inspection df transactions
    if (this.dynamicControls && this.dynamicControls.length > 0)
      this.addInspectionDfTransaction();
  
      this.search();
    }
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

  toggleExpandRow(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];
    rowItem.statusList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="booking' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getProductOrContainerListByBooking(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get booking product list by booking id
  getProductOrContainerListByBooking(rowItem) {
    if (rowItem.serviceTypeId != this.inspectionServiceType.Container) {
      this.getProductListByBooking(rowItem);
    }
    else {
      this.service.GetContainerReportDetailsByBooking(rowItem.bookingId)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.containerList = res.bookingContainerList && res.bookingContainerList.length > 0 ?
              res.bookingContainerList.sort((a, b) => a.reportId - b.reportId) : res.bookingContainerList;

            rowItem.statusList = res.bookingStatusList;
            rowItem.isPlaceHolderVisible = false;
          }

        },
          error => {
            rowItem.isPlaceHolderVisible = false;
            this.exportDataLoading = false;
          });
    }
  }


  /* toggleExpandRow(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];
    rowItem.statusList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="booking' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getProductListByBooking(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  } */

  // get booking product list by booking id
  getProductListByBooking(rowItem) {
    this.reportService.SearchBookingProducts(rowItem.bookingId)
      .pipe()
      .subscribe(
        res => {

          var response = res && res.length > 0 ? res.sort((a, b) => a.fbReportId - b.fbReportId) : res;

          this.model.items.filter(x => x.bookingId == rowItem.bookingId)[0].productList = response;

          this.applyGroupColor();
          this.searchloading = false;
        }
      );
  }


  GetSearchData() {
    this.searchloading = true;
    this.model.callingFrom = PageTypeSummary.FillingReview;
    this.reportService.SearchReportSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);

            this._statuslist = response.inspectionStatuslst;
            this.model.items = response.data.map((x) => {
              var _cancelbtnshow = this.isCancelVisible(x);
              var _rescheduleBtnShow = this.isRescheduleVisible(x);
              var _editBtnText = this.isEditShow(x);
              var item: BookingReportItem = {
                bookingId: x.bookingId,
                poNumber: x.poNumber,
                customerName: x.customerName,
                supplierName: x.supplierName,
                factoryName: x.factoryName,
                serviceDateFrom: x.serviceDateFrom,
                serviceDateTo: x.serviceDateTo,
                serviceType: x.serviceType,
                serviceTypeId: x.serviceTypeId,
                office: x.office,
                statusId: x.statusId,
                bookingCreatedBy: x.bookingCreatedBy,
                customerId: x.customerID,
                internalReferencePo: x.internalReferencePo,
                isPicking: x.isPicking,
                previousBookingNo: x.previousBookingNo,
                factoryId: x.factoryId,
                cancelBtnShow: _cancelbtnshow,
                rescheduleBtnShow: _rescheduleBtnShow,
                editBtnText: _editBtnText,
                countryId: x.countryId,
                isCsReport: x.isCsReport,
                productList: x.reportProducts,
                isExpand: false,
                fbMissionId: x.fbMissionId,
                overAllStatus: x.missionStatus,
                csList: x.inspectionCsList,
                customerBookingNo: x.customerBookingNo,
                isEAQF: x.isEAQF
              }
              return item;
            }
            );

            // this.applyGroupColor();
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this._statuslist = [];
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloading = false;
        });
  }

  expand(id) {
    let item = this.model.items.filter(x => x.bookingId == id)[0];

    if (item != null)
      item.isExpand = true;
  }

  collapse(id) {
    let item = this.model.items.filter(x => x.bookingId == id)[0];

    if (item != null)
      item.isExpand = false;
  }

  SearchByStatus(id) {
    this.model.statusidlst = [];
    this.model.statusidlst.push(id);
    this.SearchDetails();
  }
  GetStatusColor(statusid?) {
    if (this._statuslist != null && this._statuslist.length > 0 && statusid != null) {
      var result = this._statuslist.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }
  export() {
    this.exportDataLoading = true;
    this.reportService.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.exportDataLoading = false;
    this.pageLoader = false;
  }

  RedirectPage(type, item) {
    switch (type) {
      case this._bookingredirectpage.Edit:
        {
          this._redirectpath = "inspedit/edit-booking";
          super.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.Cancel:
        {
          //have to control 1 working day before for external user show popup
          this.holidayExistsShowPopup(item, this._bookingredirectpage.Cancel);
          break;
        }
      case this._bookingredirectpage.Reschedule:
        {
          this.holidayExistsShowPopup(item, this._bookingredirectpage.Reschedule);
          break;
        }
      case this._bookingredirectpage.Report:
        {
          this._redirectpath = "booking-report";
          super.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.Schedule:
        {
          this._redirectpath = "inspedit/edit-booking";
          this._redirecttype = this._bookingredirectpage.Schedule;
          this.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.CombineProduct:
        {
          this._redirectpath = "inspcombine/edit-combineorders";
          this.getCombineProductDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.SplitBooking:
        {
          this._redirectpath = "inspsplit/split-booking";
          this.getCombineProductDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.ReInspectionBooking:
        {
          this._redirectpath = "inspedit/editreinspection-booking";
          this._redirecttype = this._bookingredirectpage.ReInspectionBooking;
          this.getDetails(item.bookingId);
          break;
        }
      case this._bookingredirectpage.ReBooking:
        {
          this._redirectpath = "inspedit/editre-booking";
          this._redirecttype = this._bookingredirectpage.ReBooking;
          this.getDetails(item.bookingId);
          break;
        }
    }
  }

  //HolidayRequest map the model value
  mapModel(item, serviceFromDate): HolidayRequest {
    var model: HolidayRequest = {
      factoryCountryId: item.countryId > 0 ? item.countryId : 0,
      factoryId: item.factoryId,
      serviceDateFrom: serviceFromDate
    };
    return model;
  }
  //cancel/reshedule page redirect
  redirectPage(bookingId, type) {
    this._redirectpath = "inspcancel/cancel-booking";
    this._redirecttype = type == this._bookingredirectpage.Cancel ?
      this._bookingredirectpage.Cancel :
      this._bookingredirectpage.Reschedule;
    this.getDetails(bookingId);
  }
  //before one working day logic
  holidayExistsShowPopup(item, type) {
    var date = this.dateparser.parse(item.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    if (this._IsInternalUser && (serviceFromDate.before(this.calendar.getToday())
      || serviceFromDate.equals(this.calendar.getToday()))) {
      this.redirectPage(item.bookingId, type);
    }
    else if (serviceFromDate.before(this.calendar.getToday())
      || serviceFromDate.equals(this.calendar.getToday())) {
      this.infoPopup(this.cancelresheduleBooking, type);
    }
    else {
      this.cancelLoading = true;
      this.isHolidayExists(this.mapModel(item, serviceFromDate), type, item.bookingId);
    }
  }
  //ajax call for get holiday condition based on holidays and service date
  isHolidayExists(model: HolidayRequest, type, bookingId) {
    this.service.IsHolidayExists(model)
      .pipe()
      .subscribe(
        res => {
          this.isHolidayExistsSuccessHandler(res, type, bookingId);
        },
        error => {
          this.cancelLoading = false;
          this.setError(error);
        });
  }
  //isHolidayExists - success handler
  isHolidayExistsSuccessHandler(res, type, bookingId) {
    if (!res) {
      this.infoPopup(this.cancelresheduleBooking, type);
    }
    else {
      this.redirectPage(bookingId, type);
    }
    this.cancelLoading = false;
  }
  infoPopup(content, type) {
    this.titlePopup = (this._bookingredirectpage.Cancel == type) ?
      this.utility.textTranslate('BOOKING_SUMMARY.LBL_CANCEL')
      : this.utility.textTranslate('BOOKING_SUMMARY.LBL_POSTPONE');
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  getDetails(id) {
    var data = Object.keys(this.model);
    var currentItem: any = {};

    for (let item of data) {
      if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
        currentItem[item] = this.model[item];
    }
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${this._redirecttype}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }
  isRescheduleVisible(bookingItem) {
    var date = this.dateparser.parse(bookingItem.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    var isRescheduleVisble = false;
    if (bookingItem.statusId != BookingStatus.Cancel) {
      if (this._IsInternalUser && this.model.isBookingConfirmRole &&
        !this.utility.checkBookingStatus(bookingItem.statusId)
        && (serviceFromDate.after(this.calendar.getToday())
          || serviceFromDate.equals(this.calendar.getToday()))) { //  service from date less or equal to today date
        isRescheduleVisble = true;
      }
      else if (!this.utility.checkBookingStatus(bookingItem.statusId) &&
        bookingItem.bookingCreatedBy == this.currentUser.usertype
        && (serviceFromDate.after(this.calendar.getToday()))) {
        isRescheduleVisble = true;
      }
    }
    return isRescheduleVisble;
  }
  isCancelVisible(bookingItem) {
    var date = this.dateparser.parse(bookingItem.serviceDateFrom);
    var serviceFromDate = new NgbDate(date.year, date.month, date.day);
    var isCancelVisible = false;
    if (this._IsInternalUser &&
      ((bookingItem.statusId == BookingStatus.Requested &&
        (this.model.isBookingRequestRole || this.model.isBookingVerifyRole)) ||
        ((bookingItem.statusId != BookingStatus.Requested) && this.model.isBookingVerifyRole)) &&
      !this.utility.checkBookingStatus(bookingItem.statusId)) {
      isCancelVisible = true;
    }
    else if (!this.utility.checkBookingStatus(bookingItem.statusId) &&
      bookingItem.statusId != BookingStatus.Cancel) {
      if (bookingItem.bookingCreatedBy == this.currentUser.usertype
        && (serviceFromDate.after(this.calendar.getToday()))) {
        isCancelVisible = true;
      }
    }
    return isCancelVisible;
  }
  isEditShow(bookingItem) {
    var editTextBtn = "Edit";
    if (this._IsInternalUser) {
      if (this.model.isBookingConfirmRole && bookingItem.statusId == BookingStatus.Verified)
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_CONFIRM');
      else if (this.model.isBookingVerifyRole && bookingItem.statusId == BookingStatus.Requested)
        editTextBtn = this.utility.textTranslate('BOOKING_SUMMARY.LBL_VERIFY');
    }
    return editTextBtn;
  }
  IsReportVisible(isheader, statusid) {
    if (this._IsInternalUser && isheader)
      return true;
    else if (this._IsInternalUser) {
      if (statusid == this._BookingStatus.Scheduled || statusid == this._BookingStatus.Confirmed)
        return true;
      else
        return false;
    }
    else
      return false;
  }
  IsScheduleBookingVisible(statusid) {
    if (this._IsInternalUser) {
      if (statusid != this._BookingStatus.Cancel && statusid != this._BookingStatus.Confirmed)
        return true;
      else
        return false;
    }
    else
      return false;
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
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${customerId}`], { queryParams: { paramParent: encodeURI(JSON.stringify(currentItem)) } });
  }

  redirectPickingProducts(bookingid, customerId, customerName) {
    this._redirectpath = "insppicking/edit-inspectionpicking";
    this.getPickingProductDetails(bookingid, customerId, customerName);
  }
  clearCustomer() {
    this.model.customerid = null;
    this.model.supplierid = null;
    this.reportMaster.supplierList = [];
    this.reportMaster.factoryList = [];
    this.model.factoryidlst = [];
  }
  clearSupplier() {
    this.model.supplierid = null;
    this.reportMaster.factoryList = null;
    this.model.factoryidlst = [];
  }
  preview(inspectionId) {
    this.isPreviewLoading = true;
    this.service.preview(inspectionId)
      .subscribe(res => {
        this.downloadReportFile(res, "application/pdf");
        this.isPreviewLoading = false;
      },
        error => {
          this.isPreviewLoading = false;
        });
  }
  downloadReportFile(data, mimeType) {
    let windowNavigator: any = window.navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }


  getPickingData(bookingId) {
    this.isPDFLoading = true;
    this.service.GetPickingPdf(bookingId)
      .pipe()
      .subscribe(res => {
        this.downloadPDFFile(res, "application/pdf");
        this.isPDFLoading = false;
      },
        error => {
          this.showError('BOOKING_SUMMARY.LBL_PICKING', 'BOOKING_SUMMARY.ADDRESS_NOTFOUND');
          this.isPDFLoading = false;
        });
  }

  downloadPDFFile(data, mimeType) {
    let windowNavigator: any = window.navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
  }

  toggleExpandRowContainer(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="container' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getProductListByContainerAndBooking(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get product list by container and booking
  getProductListByContainerAndBooking(rowItem) {
    this.service.GetProductListByBookingAndContainer(rowItem.bookingId, rowItem.id)
      .subscribe(res => {
        if (res.result == 1) {
          rowItem.productList = res.bookingProductList;
          rowItem.isPlaceHolderVisible = false;
        }
      },
        error => {
          rowItem.isPlaceHolderVisible = false;

        });
  }

  toggleExpandRowProduct(event, index, rowItem, containerId, serviceType) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var productIndex = index;
    if (containerId)
      productIndex = index + 'container' + containerId;
    var firstElem = document.querySelector('[data-expand-id="product' + productIndex + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getPoListByBookingAndProduct(rowItem, containerId, serviceType);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get po list by booking and product
  getPoListByBookingAndProduct(rowItem, containerId, serviceTypeId) {
    if (serviceTypeId != this.inspectionServiceType.Container) {
      this.service.GetPODetailsByBookingAndProduct(rowItem.bookingId, rowItem.id)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            rowItem.isPlaceHolderVisible = false;
          }

        },
          error => {
            rowItem.isPlaceHolderVisible = false;

          });
    }
    else {
      this.service.GetPODetailsByBookingAndConatinerAndProduct(rowItem.bookingId, containerId, rowItem.id)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            rowItem.isPlaceHolderVisible = false;
          }

        },
          error => {
            rowItem.isPlaceHolderVisible = false;

          });
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
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

  getCustomerListBySearch() {

    //push the customerid to  customer id list 
    if (this.model.customerid) {
      this.reportMaster.requestCustomerModel.idList.push(this.model.customerid);
    }
    else {
      this.reportMaster.requestCustomerModel.idList = null;
    }

    this.reportMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.reportMaster.requestCustomerModel, term)
        : this.customerService.getCustomerDataSourceList(this.reportMaster.requestCustomerModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.customerLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.customerList = data;
        this.reportMaster.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.reportMaster.requestCustomerModel.searchText = this.reportMaster.customerInput.getValue();
      this.reportMaster.requestCustomerModel.skip = this.reportMaster.customerList.length;
    }

    this.reportMaster.customerLoading = true;
    this.customerService.getCustomerDataSourceList(this.reportMaster.requestCustomerModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          this.reportMaster.customerList = this.reportMaster.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.reportMaster.requestCustomerModel.skip = 0;
          this.reportMaster.requestCustomerModel.take = ListSize;
        }
        this.reportMaster.customerLoading = false;
      }),
      error => {
        this.reportMaster.customerLoading = false;
        this.setError(error);
      };
  }

  //get the supplier details based on the customer change
  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
    }
  }

  //fetch the intial supplier list
  getSupplierListBySearch() {
    this.reportMaster.supsearchDataRequest.supplierIds = [];
    this.reportMaster.supsearchDataRequest.customerIds = [];
    if (this.model.customerid)
      this.reportMaster.supsearchDataRequest.customerIds.push(this.model.customerid);
    this.reportMaster.supsearchDataRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.reportMaster.supsearchDataRequest.supplierIds.push(this.model.supplierid);
    this.reportMaster.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.supLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.reportMaster.supsearchDataRequest, null, term)
        : this.supplierService.getSupplierDataSource(this.reportMaster.supsearchDataRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.supLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.supplierList = data;
        this.reportMaster.supLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.reportMaster.supsearchDataRequest.searchText = this.reportMaster.supInput.getValue();
    this.reportMaster.supsearchDataRequest.skip = this.reportMaster.supplierList.length;

    this.reportMaster.supsearchDataRequest.customerIds.push(this.model.customerid);
    this.reportMaster.supsearchDataRequest.supplierType = SupplierType.Supplier;
    this.reportMaster.supLoading = true;
    this.supplierService.getSupplierDataSource(this.reportMaster.supsearchDataRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.reportMaster.supplierList = this.reportMaster.supplierList.concat(customerData);
        }
        this.reportMaster.supsearchDataRequest.skip = 0;
        this.reportMaster.supsearchDataRequest.take = ListSize;
        this.reportMaster.supLoading = false;
      }),
      (error: any) => {
        this.reportMaster.supLoading = false;
      };
  }

  //apply filters to fetch for the factory list
  applyFactoryRelatedFilters() {
    this.reportMaster.facsearchRequest.customerIds = [];
    this.reportMaster.facsearchRequest.supplierIds = [];
    if (this.model.customerid)
      this.reportMaster.facsearchRequest.customerIds.push(this.model.customerid);
    this.reportMaster.facsearchRequest.supplierType = SupplierType.Factory;
    if (this.model.supplierid)
      this.reportMaster.facsearchRequest.supplierIds.push(this.model.supplierid);
    else
      this.reportMaster.facsearchRequest.supplierIds = null;
  }

  getFactoryListBySearch() {

    this.applyFactoryRelatedFilters();
    this.reportMaster.facInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.facLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.reportMaster.facsearchRequest, null, term)
        : this.supplierService.getSupplierDataSource(this.reportMaster.facsearchRequest, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.facLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.factoryList = data;
        this.reportMaster.facLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getFactoryData() {

    this.applyFactoryRelatedFilters();
    this.reportMaster.facsearchRequest.searchText = this.reportMaster.facInput.getValue();
    this.reportMaster.facsearchRequest.skip = this.reportMaster.factoryList.length;
    this.reportMaster.facLoading = true;
    this.supplierService.getSupplierDataSource(this.reportMaster.facsearchRequest, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.reportMaster.factoryList = this.reportMaster.factoryList.concat(customerData);
        }
        this.reportMaster.facsearchRequest.skip = 0;
        this.reportMaster.facsearchRequest.take = ListSize;
        this.reportMaster.facLoading = false;
      }),
      (error: any) => {
        this.reportMaster.facLoading = false;
      };
  }

  getBookingStatusList() {
    this.reportMaster.bookingStatusLoading = true;
    this.service.getBookingStatusList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == DataSourceResult.Success) {
            this.reportMaster.bookingStatusList = data.dataSourceList;
          }
          this.reportMaster.bookingStatusLoading = false;
        },
        error => {
          this.setError(error);
          this.reportMaster.bookingStatusLoading = false;
        });
  }

  //get the office list
  getOfficeLocationList() {
    this.reportMaster.officeLoading = true;
    this.officeService.getOfficeDetails()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processOfficeLocationResponse(response);
        },
        error => {
          this.setError(error);
          this.reportMaster.officeLoading = false;
        });
  }

  //process the office location response
  processOfficeLocationResponse(response) {
    if (response && response.result == ResponseResult.Success)
      this.reportMaster.officeList = response.dataSourceList;
    else if (response && response.result == ResponseResult.NoDataFound)
      this.showError('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.TITLE.MSG_OFFICE_NOT_FOUND');
    this.reportMaster.officeLoading = false;
  }

  //upload the file to the cloud 
  selectReportFile(event) {
    var uploadFiles = [];
    if (event && !event.error && event.files) {

      if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
        this.pageLoader = true;
        for (let file of event.files) {
          var guid = this.newUniqueId();
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            fileSize: file.fileSize,
            isNew: true,
            id: 0,
            status: 0,
            mimeType: file.type,
            fileUrl: "",
            uniqueld: guid,
            isSelected: false,
            fileDescription: null
          };
          this.uploadCustomReportRequest.fileName = file.name;
          this.uploadCustomReportRequest.fileType = file.type;
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {
            this.fileStoreService.uploadFiles(FileContainerList.Report, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                this.uploadCustomReportRequest.fileUrl = response.fileUploadDataList[0].fileCloudUri;
                this.uploadCustomReportRequest.uniqueId = response.fileUploadDataList[0].fileName;
              }
              this.pageLoader = false;
            },
              error => {
                this.pageLoader = false;
                this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
              });
          }
        }
      }
    }
    else {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
    }
  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  // get the file data
  getFile(file: string, fileName: string) {
    this.pageLoader = true;
    this.fileStoreService.downloadBlobFile(file, FileContainerList.Report)
      .subscribe(res => {

        this.downloadFile(res, res.type);
      },
        error => {
          this.pageLoader = false;
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });

  }

  //save the report details to the server
  saveReport() {

    //if the report file url is available then save the information
    if (this.uploadCustomReportRequest.fileUrl) {

      this.uploadCustomReportLoading = true;

      this.fbService.updateCustomReport(this.uploadCustomReportRequest)
        .subscribe(
          res => {
            //if it is success
            if (res && res.result == UploadCustomReportResult.Success) {
              this.uploadCustomReportLoading = false;
              //refresh the product data
              this.GetSearchData();
              this.showSuccess("UPLOAD_CUSTOM_REPORT.LBL_TITLE", 'UPLOAD_CUSTOM_REPORT.MSG_REPORT_SAVE_SUCCESS');
              this.modelRef.close();
            }
            else if (res && res.result == UploadCustomReportResult.NotSaved) {
              this.uploadCustomReportLoading = false;
              this.showSuccess("UPLOAD_CUSTOM_REPORT.LBL_TITLE", 'UPLOAD_CUSTOM_REPORT.MSG_REPORT_NOT_SAVE');
              this.modelRef.close();
            }
          },
          errRes => {
            this.uploadCustomReportLoading = false;
            this.showError("UPLOAD_CUSTOM_REPORT.LBL_TITLE", "UPLOAD_CUSTOM_REPORT.MSG_UNKNOWN_ERROR");
            this.modelRef.close();
          });
    }
    else {
      this.showWarning("UPLOAD_CUSTOM_REPORT.LBL_TITLE", "UPLOAD_CUSTOM_REPORT.MSG_PLS_CHOOSE_FILE");
    }

  }

  //download the custom report
  downloadCustomReport(item) {
    if (item.finalManualReportLink)
      window.open(item.finalManualReportLink);
    else if (item.finalReportLink)
      window.open(item.finalReportLink);
  }
  //show upload button only if the logged in role is inspection verified or report checker and booking status is in inspected status
  showUploadButton(bookingStatusId) {
    var showUploadBtn: boolean = false;
    var allowedRoles = this.currentUser.roles.filter(x => x.id == this.roleEnum.InspectionVerified || x.id == this.roleEnum.ReportChecker);
    var inspectedAvailable = InspectedStatusData.filter(x => x.id == bookingStatusId);
    if (inspectedAvailable && inspectedAvailable.length > 0 && allowedRoles && allowedRoles.length > 0)
      showUploadBtn = true;
    return showUploadBtn;
  }
  removeReportFile() {
    this.uploadCustomReportRequest.fileName = "";
    this.uploadCustomReportRequest.uniqueId = "";
    this.uploadCustomReportRequest.fileUrl = "";
    this.uploadCustomReportRequest.fileType = "";
  }

  // Get Dynamic Fields configured for the customer
  GetCustomerDynamicFields() {
    const request = new DfCustomerConfigurationRequest();
    request.customerId = this.model.customerid;
    request.moduleId = this._dynamicControlModuleType.InspectionBooking;
    request.dataSourceTypeIds.push(DFDDLSourceType.GAPInspectionPlatform);
    this.dynamicFieldService.getgapcustomerconfigurationlist(request)
      .subscribe(response => {
        if (response && response.result == 1 && response.dfCustomerConfigurationList) {
          //create the dynamic control object from the configuration list
          this.createDynamicControls(response.dfCustomerConfigurationList);
        }
      }, error => {
        this.setError(error);
      }
      );
  }

    //Populate control objects based on the customer configured details
    createDynamicControls(dfCustomerConfigurationList) {
      dfCustomerConfigurationList.forEach(element => {
        //if the control type is dropdown
        if (element.controlTypeId == this._controlType.DropDown) {
          const controlAttribute = element.controlAttributeList.find(x => x.attributeId == this._controlAttribute.Multiple);
          const dropDown = new DropDownControl({
            key: element.id,
            label: element.label,
            options: this.populateDropDownSourceList(element.ddlSourceList),
            dataSource: this.populateDropDownSourceList(element.ddlSourceList),
            order: element.displayOrder,
            dataSourceType: element.dataSourceType,
            controlAttributeList: [],
            controlTypeId: element.controlTypeId,
            controlConfigurationId: element.id,
            value: this.getDropDownControlValue(element.id, element.controlTypeId, controlAttribute.value)
          });
          this.addControlAttributes(dropDown, element.controlAttributeList);
          //check if it is the cascading dropdown with the parentdropdown attribute
          const isCascadingAttribute = element.controlAttributeList.find(x => x.attributeId == this._controlAttribute.ParentDropDown);
          if (isCascadingAttribute) {
            if (isCascadingAttribute.value) {
              //fetch the parent dropdown based on the value(ParentDropdown id mapped as datasourcetype child)
              const parentControl = this.dynamicControls.find(x => x.dataSourceType == Number(isCascadingAttribute.value));
              if (parentControl && parentControl.value)
                //Filter drop down options from parentcontrol selected value
                this.FilterDropDownOptions(dropDown, parentControl);
            }
          }
          //assign the validation attribute
          this.assignValidationAttribute(dropDown, element.controlAttributeList);
          this.dynamicControls.push(dropDown);
        }
      });
  
      this.splitDynamicControlsWithRows();
    }

  splitDynamicControlsWithRows() {
    const rowCount = Math.ceil(this.dynamicControls.length / 4);
    let columnCount = this.dynamicControls.length < 4 ? this.dynamicControls.length : 4;
    let columnIndexValue = 0;
    let columnDataIndex = 0;
    this.dynamicControlBaseData = [];
    let dynamicControlChildData = [];
    for (let rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      if (rowIndex > 0) {
        const actualRowDataCount = this.dynamicControls.length - (rowIndex * 4);
        if (actualRowDataCount < 4)
          columnCount = actualRowDataCount;
        else
          columnCount = 4;
      }
      while (columnIndexValue < columnCount) {
        dynamicControlChildData.push(this.dynamicControls[columnDataIndex]);
        columnDataIndex = columnDataIndex + 1
        columnIndexValue = columnIndexValue + 1;
      }

      this.dynamicControlBaseData.push(dynamicControlChildData);
      columnIndexValue = 0;
      dynamicControlChildData = [];
    }
  }
  //Add attributes given for df(dynamic controls)
  addControlAttributes(control, controlAttributeList) {
    control.controlAttributeList = [];
    controlAttributeList.forEach(element => {
      const attribute = new ControlAttributes();
      attribute.id = element.id;
      attribute.attributeid = element.attributeId,
        attribute.key = element.controlAttributeId,
        attribute.value = element.value;
      attribute.active = element.active;
      control.controlAttributeList.push(attribute)
    });

  }

  //Populate options for dynamic dropdownlist
  populateDropDownSourceList(DDLSourceList) {
    const options = [];
    DDLSourceList.forEach(element => {
      const option = { key: element.id, value: element.name, parentId: element.parentId }
      options.push(option);
    });
    return options;
  }

  //assign the filtered datasource to child dropdown based on the parent dropdown
  FilterDropDownOptions(dropDown, parentControl) {
    const filteredDataSource = dropDown.dataSource.filter(x => x.parentId == parentControl.value);
    if (filteredDataSource) {
      dropDown.options = filteredDataSource;
    }
  }

  //assign the validation attribute list
  assignValidationAttribute(control, controlAttributes) {
    const requiredValidator = controlAttributes.find(x => x.attributeId == this._controlAttribute.RequiredValidation);
    if (requiredValidator && requiredValidator.value && requiredValidator.value == "true") {
      control.required = true;
    }
    else {
      control.required = false;
    }
  }

  //get and assign dropdown values
  getDropDownControlValue(id, controlTypeId, isMultiple): any {
    if (isMultiple == "true") {
      const ddlMultiTransaction = this.model.inspectionDFTransactions.filter(x => x.controlConfigurationId == id);
      const valuearray = [];
      ddlMultiTransaction.forEach(element => {
        valuearray.push(Number(element.value));
      });
      return (valuearray.length > 0) ? valuearray : [];
    }
    else {
      const ddltransaction = this.model.inspectionDFTransactions.find(x => x.controlConfigurationId == id);
      return (ddltransaction && ddltransaction.value) ? Number(ddltransaction.value) : null;
    }
  }

  //populate inspectiondftransaction before saving
  addInspectionDfTransaction() {
    this.dynamicControls.forEach(element => {
      //push textbox and textarea control value
      if (element.controlTypeId != this._controlType.DropDown
        && element.controlTypeId != this._controlType.DatePicker) {

        const inspectiondftransaction = this.model.inspectionDFTransactions.
          find(x => x.controlConfigurationId == element.controlConfigurationId);
        //If It is new control add into inspectionDFTransactions
        if (!inspectiondftransaction) {
          const transaction = new InspectionDFTransaction();
          transaction.id = 0;
          transaction.controlConfigurationId = element.controlConfigurationId;
          transaction.value = element.value;
          this.model.inspectionDFTransactions.push(transaction);
        }
        //else just update the value
        else {
          inspectiondftransaction.value = element.value;
        }
      }
      //push datepicker value
      else if (element.controlTypeId == this._controlType.DatePicker) {
        const inspectiondftransaction = this.model.inspectionDFTransactions.
          find(x => x.controlConfigurationId == element.controlConfigurationId);
        //If It is new dropdown control add into inspectionDFTransactions
        if (!inspectiondftransaction) {
          const transaction = new InspectionDFTransaction();
          transaction.id = 0;
          transaction.controlConfigurationId = element.controlConfigurationId;
          transaction.value = element.value != null ? element.value.year + "-" + element.value.month + "-" + element.value.day : null;
          this.model.inspectionDFTransactions.push(transaction);
        }
        //else update the new date value
        else {
          inspectiondftransaction.value = element.value != null ? element.value.year + "-" + element.value.month + "-" + element.value.day : null;
        }
      }
      //push dropdown value
      else if (element.controlTypeId == this._controlType.DropDown) {
        //take the multiple attribute of the given dropdown
        const controlAttribute = element.controlAttributeList.find(x => x.attributeid == this._controlAttribute.Multiple);
        //If it is multi dropdown list block will work
        if (controlAttribute.value == "true") {
          const multidropDownList = this.model.inspectionDFTransactions.
            filter(x => x.controlConfigurationId == element.controlConfigurationId);

          if (multidropDownList.length == 0) {
            element.value.forEach(item => {
              const transaction = new InspectionDFTransaction();
              transaction.id = 0;
              transaction.controlConfigurationId = element.controlConfigurationId;
              transaction.value = item;
              this.model.inspectionDFTransactions.push(transaction);
            });
          }
          else {
            const removedlist = multidropDownList.filter(x => !element.value.includes(x.value));
            removedlist.forEach(data => {
              const index = this.model.inspectionDFTransactions.
                findIndex(x => x.controlConfigurationId == element.controlConfigurationId && x.id == data.id);
              if (index >= 0)
                this.model.inspectionDFTransactions.splice(index, 1);
              const transactionValues = this.model.inspectionDFTransactions.map(({ value }) => value);
              const newTransactions = element.value.filter(x => !transactionValues.includes(x));
              newTransactions.forEach(newTransaction => {
                const transaction = new InspectionDFTransaction();
                transaction.id = 0;
                transaction.controlConfigurationId = element.controlConfigurationId;
                transaction.value = newTransaction;
                this.model.inspectionDFTransactions.push(transaction);
              });

            });

          }
        }
        //If it is single dropdown list block will work
        else {
          const inspectiondftransaction = this.model.inspectionDFTransactions.
            find(x => x.controlConfigurationId == element.controlConfigurationId);
          if (!inspectiondftransaction) {
            const transaction = new InspectionDFTransaction();
            transaction.id = 0;
            transaction.controlConfigurationId = element.controlConfigurationId;
            transaction.value = element.value;
            this.model.inspectionDFTransactions.push(transaction);
          }
          else {
            inspectiondftransaction.value = element.value;
          }
        }
      }
    });
  }
}

