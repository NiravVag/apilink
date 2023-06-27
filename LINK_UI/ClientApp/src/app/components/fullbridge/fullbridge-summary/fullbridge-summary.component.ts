import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PageSizeCommon, SearchType, DefaultDateType, bookingSearchtypelst, datetypelst, UserType, BookingSearchRedirectPage, BookingStatus, InspectionServiceType, FetchFBReportOption, FbFetchStatusList, ListSize, SupplierType } from '../../common/static-data-common'
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
import { TranslateService } from "@ngx-translate/core";
import { ReportService } from 'src/app/_Services/Report/report.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { of, Subject } from 'rxjs';
import { ReportMaster } from 'src/app/_Models/Report/reportmodel';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { ResponseResult } from 'src/app/_Models/common/common.model';


@Component({
  selector: 'app-fullbridge-summary',
  templateUrl: './fullbridge-summary.component.html',
  styleUrls: ['./fullbridge-summary.component.scss'],
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

export class FullbridgeSummaryComponent extends SummaryComponent<InspectionBookingsummarymodel> {
  public model: InspectionBookingsummarymodel;
  public data: any;
  public searchloading: boolean = false;
  public searchloader: boolean = false;
  public error: string = "";
  public bookingMissionNumber: string = "";
  public bookingReportNumbers: string = "";
  public modelRef: NgbModalRef;
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
  _bookingredirectpage = BookingSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _BookingStatus = BookingStatus;
  FbReportFetchOption = FetchFBReportOption;
  _searchbookingtid: any;
  isReInspection: boolean = false;
  isReBooking: boolean = false;
  cancelLoading: boolean;
  titlePopup: string;
  pageLoader: boolean = false;
  pageControlEnable: boolean = false;
  inspectionServiceType = InspectionServiceType;
  isFilterOpen: boolean;
  reportMaster: ReportMaster;
  componentDestroyed$: Subject<boolean> = new Subject();
  @ViewChild('cancelresheduleBooking') cancelresheduleBooking: ElementRef;
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {

    return this._redirectpath;
  }

  constructor(private service: BookingService, public validator: Validator, router: Router,
    route: ActivatedRoute, authserve: AuthenticationService, public accService: UserAccountService, public modalService: NgbModal, public calendar: NgbCalendar,
    public customerService: CustomerService, public officeService: OfficeService, private supplierService: SupplierService,
    public dateparser: NgbDateParserFormatter, private reportService: ReportService, public pathroute: ActivatedRoute, public utility: UtilityService, translate: TranslateService,
    toastr: ToastrService) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.currentRoute = router;
    this.isFilterOpen = true;
    this.reportMaster = new ReportMaster();

  }
  onInit(): void {
    var type = this.pathroute.snapshot.paramMap.get("type");
    if (type) {
      this.isReInspection = (type == "1") ? true : false;
      this.isReBooking = (type == "2") ? true : false;
    }

    this._searchbookingtid = this.pathroute.snapshot.paramMap.get("id");
    this.Initialize(null);
    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
  }
  Initialize(fromLocal) {
    if (fromLocal == null)
      this.model = new InspectionBookingsummarymodel();

    this.reportMaster.entityId = parseInt(this.utility.getEntityId());
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.pageSize = 10;
    this.model.index = 0;
    this.data = [];
    this._statuslist = [];
    this.containerMasterList = this.utility.getContainerList(100);

    // set default status search
    const chunkSize = 10; //array length

    //get the values from enum
    this.model.statusidlst.push(Object.keys(FbFetchStatusList).filter(k => typeof FbFetchStatusList[k as any] === "number").map(k => FbFetchStatusList[k as any]));

    //convert the array to Array<int> - eg: [1,2,3] -> 0: 1, 1: 2
    const groups = this.model.statusidlst.map((e, i) => {
      return i % chunkSize === 0 ? this.model.statusidlst.slice(i, i + chunkSize) : null;
    }).filter(e => { return e; });

    this.model.statusidlst = groups[0];
    this.model.statusidlst = this.model.statusidlst[0];

    this.getCustomerListBySearch();
    this.getOfficeLocationList();
  }

  //get the supplier list based on the customer change
  ChangeCustomer(cusitem) {
    if (cusitem != null && cusitem.id != null) {
      this.getSupplierListBySearch();
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
            //  window.open("https://testing.qualitycontrol.tools/api/backend/report/"+reportId+"?token="+response.token+"", "_blank");
          }
        },
        error => {
          this.suploading = false;
        }
      );
  }


  //get factory list based on the supplier change
  ChangeSupplier(supitem) {
    if (supitem != null && supitem.id != null) {
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
    this.Initialize(null);
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
  GetSearchData() {
    this.searchloading = true;
    this.searchloader = true;
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

            //  this.applyGroupColor();
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this._statuslist = [];
          }
          else {
            this.error = response.result;
          }
          this.searchloader = false;
          this.searchloading = false;
        },
        error => {
          this.setError(error);
          this.searchloader = false;
          this.searchloading = false;
        });
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
            rowItem.containerList = res.bookingContainerList;
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

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
  }

  // get booking product list by booking id
  getProductListByBooking(rowItem) {
    this.reportService.SearchBookingProducts(rowItem.bookingId)
      .pipe()
      .subscribe(
        res => {
          this.model.items.filter(x => x.bookingId == rowItem.bookingId)[0].productList = res;
          this.applyGroupColor();
          this.searchloading = false;
        }
      );
  }



  updateFBBookingDetails(bookingItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.UpdateFBBookingDetails(bookingItem.bookingId)
      .subscribe(
        response => {
          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;
          this.showSuccess('FullBridge', 'Booking details updated successfully');
          this.GetSearchData();

        });
  }

  /* updateFBProductDetails(bookingItem){
     this.service.UpdateFBProductDetails(bookingItem.bookingId)
     .subscribe(
       response => {
 
         this.showSuccess('FullBridge', 'Booking details updated successfully');
         
       });
   }*/
  createFBMission(bookingItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.CreateFBMission(bookingItem.bookingId)
      .subscribe(
        response => {
          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;
          this.showSuccess('FullBridge', 'Mission created successfully');
          this.GetSearchData();
        });
  }

  confirmMissionDelete(content, bookingItem) {


    //"serviceType":"Container Service",

    if (bookingItem.serviceTypeId != this.inspectionServiceType.Container) {



      this.reportService.SearchBookingProducts(bookingItem.bookingId)
        .pipe()
        .subscribe(
          res => {

            var productReportList = res;
            if (productReportList) {

              this.bookingMissionNumber = bookingItem.bookingId;
              this.bookingReportNumbers = productReportList.map(function (item) { if (item.fbReportId != 0) { return item.reportTitle; } }).join();

              // remove duplicate report names
              this.bookingReportNumbers = Array.from(new Set(this.bookingReportNumbers.split(','))).toString();

              this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
              this.modelRef.result.then((result) => {

                this.deleteFBMission(bookingItem);

              }, (reason) => {
              });
            }
          }
        );
    }

    else {



      this.reportService.SearchBookingContainers(bookingItem.bookingId)
        .pipe()
        .subscribe(
          res => {

            var productReportList = res;
            if (productReportList) {

              this.bookingMissionNumber = bookingItem.bookingId;
              this.bookingReportNumbers = productReportList.map(function (item) { if (item.fbReportId != 0) { return item.reportTitle; } }).join();

              // remove duplicate report names
              this.bookingReportNumbers = Array.from(new Set(this.bookingReportNumbers.split(','))).toString();

              this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
              this.modelRef.result.then((result) => {

                this.deleteFBMission(bookingItem);

              }, (reason) => {
              });
            }
          }
        );

    }


  }


  deleteConfirMission() {
    this.modelRef.close();
  }

  // Delete FB Mission if the report is not processed.
  deleteFBMission(bookingItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.DeleteFBMission(bookingItem.bookingId)
      .subscribe(
        response => {
          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;
          if (response.result == 1) {
            this.showSuccess('FullBridge', 'Mission deleted successfully');
            this.GetSearchData();
          }
          else if (response.result == 2) {
            this.showError('FullBridge', 'Mission delete - failure - Please contact IT-Team');
          }
          else if (response.result == 3) {
            this.showError('FullBridge', ' Report was filled by QC. Mission can’t delete. Please contact IT team.');
          }
          else if (response.result == 4) {
            this.showError('FullBridge', ' Report was filled by QC. Mission can’t delete. Please contact IT team.');
          }

        });
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

  createFBReport(bookingItem, productItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.CreateFBReport(bookingItem.bookingId, productItem.productId)
      .subscribe(
        response => {
          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;
          this.showSuccess('FullBridge', 'Report created successfully');
          this.GetSearchData();
        });
  }


  createFBReportContainer(bookingItem, productItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.CreateFBReport(bookingItem.bookingId, productItem.containerId)
      .subscribe(
        response => {
          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;
          this.showSuccess('FullBridge', 'Report created successfully');
          this.GetSearchData();
        });
  }

  deleteFBReport(bookingItem, productItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.DeleteFBReport(bookingItem.bookingId, productItem.fbReportId, productItem.apiReportId)
      .subscribe(
        response => {
          if (response.result == 1) {
            this.showSuccess('FullBridge', 'Report deleted successfully');
            this.GetSearchData();
          }
          else if (response.result == 2) {
            this.showError('FullBridge', 'FB Connectivity Error - please contact IT-Team');
          }
          else if (response.result == 3) {
            this.showError('FullBridge', 'Report was filled by QC. Report can’t delete. please contact IT-Team');
          }
          else if (response.result == 4) {
            this.showError('FullBridge', 'Mission was not exist in FB. please contact IT-Team');
          }

          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;

        });
  }


  fetchFBReport(reportId, apiReportId) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.fetchFBReport(reportId, apiReportId)
      .subscribe(
        response => {

          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;

          if (response.result == 1) {
            this.showSuccess('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_UPDATED');
          }
          else if (response.result == 2) {
            this.showWarning('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_VALIDATED');
          }

          else if (response.result == 3) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_NOTEXIST');
          }

          else if (response.result == 4) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_ID_NOTVALID');
          }

          else if (response.result == 5) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_CONNECT');
          }
        });
  }

  fetchFBReportByBooking(bookingId, fetchOption) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.fetchFBReportByBooking(bookingId, fetchOption)
      .subscribe(
        response => {

          this.pageLoader = false;
          this.pageControlEnable = false;
          this.searchloading = false;

          if (response.result == 1) {
            this.showSuccess('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_UPDATED');
          }
          else if (response.result == 2) {
            this.showWarning('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_VALIDATED');
          }

          else if (response.result == 3) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_NOTEXIST');
          }

          else if (response.result == 4) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_ID_NOTVALID');
          }

          else if (response.result == 5) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_CONNECT');
          }

          else if (response.result == 6) {
            this.showError('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_BOOKING_NOT_VALID');
          }
          else if (response.result == 8) {
            this.showSuccess('BOOKING_SUMMARY.MSG_FB', 'BOOKING_SUMMARY.MSG_REPORT_SYNC');
          }
        });
  }



  deleteFBReportContainer(bookingItem, productItem) {
    this.pageLoader = true;
    this.pageControlEnable = true;
    this.searchloading = true;
    this.service.DeleteFBReport(bookingItem.bookingId, productItem.reportId, productItem.apiReportId)
      .subscribe(
        response => {
          if (response.result == 1) {
            this.showSuccess('FullBridge', 'Report deleted successfully');
            this.GetSearchData();
          }
          else if (response.result == 2) {
            this.showError('FullBridge', 'FB Connectivity Error - please contact IT-Team');
          }
          else if (response.result == 3) {
            this.showError('FullBridge', 'Report was filled by QC. Report can’t delete. please contact IT-Team');
          }
          else if (response.result == 4) {
            this.showError('FullBridge', 'Mission was not exist in FB. please contact IT-Team');
          }

          this.pageLoader = false;
          this.pageControlEnable = false;
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

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
    this.exportDataLoading = false;
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
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
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

  //get the supplier list based on the customer change
  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
    }
  }

  //get the intial supplier list
  getSupplierListBySearch() {
    this.reportMaster.supsearchDataRequest.supplierIds = [];
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


  //apply the filters for the factory list fetch
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

  //get the initial factory list
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

}



