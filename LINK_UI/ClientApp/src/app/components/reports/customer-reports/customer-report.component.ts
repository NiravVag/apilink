import { CustomerCommonDataSourceRequest } from './../../../_Models/common/common.model';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PageSizeCommon, SearchType, DefaultDateType, customerReportSearchtypelst, datetypelst, UserType, BookingSearchRedirectPage, BookingStatus, InspectionServiceType, PageTypeSummary, RoleEnum, CustomerReportSearchType, ListSize, SupplierType, reportSearchTypeLst, bookingAdvanceSearchtypelst, Service, BookingSummaryNameTrim, MobileViewFilterCount, supplierTypeList, Url, BookingTypes, FbReportResultType } from '../../common/static-data-common'
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionBookingsummarymodel, HolidayRequest, BookingReportItem } from 'src/app/_Models/booking/inspectionbookingsummarymodel';
import { ReportMaster, CustomerReportModel, CustomerReportSummaryResponseResult, ReportItem, StatusRequest } from 'src/app/_Models/Report/reportmodel';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { ReportService } from 'src/app/_Services/Report/report.service';
import { TranslateService } from "@ngx-translate/core";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { of, Subject } from 'rxjs';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { CityListSearchRequest, CommonCustomerSourceRequest, CountryDataSourceRequest, ProvinceListSearchRequest, ProvinceResult, ResponseResult } from 'src/app/_Models/common/common.model';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery-9';

@Component({
  selector: 'app-customer-report',
  templateUrl: './customer-report.component.html',
  styleUrls: ['./customer-report.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding-top': '0px',
        'padding-bottom': '16px'
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
export class CustomerReportComponent extends SummaryComponent<CustomerReportModel> {
  public model: CustomerReportModel;
  public data: any;
  public searchloading: boolean = false;
  public error: string = "";
  public bookingReportNumbers: string = "";
  public modelRef: NgbModalRef;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  customerReportSearchtypelst: any = customerReportSearchtypelst;
  datetypelst: any = datetypelst.filter(x => x.name == "Service Date");
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  public containerMasterList: any[] = [];
  fbReportResult: any = FbReportResultType;
  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  public _statuslist: any[] = [];
  currentUser: UserModel;
  _IsInternalUser: boolean = false;
  _IsCustomerUser: boolean = false;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _BookingStatus = BookingStatus;
  _searchbookingtid: any;
  cancelLoading: boolean;
  titlePopup: string;
  toggleFormSection: boolean;
  inspectionServiceType = InspectionServiceType;
  bookingId: number;
  reportDate: any;
  _roleEnum = RoleEnum;
  isDateUpdateRole: boolean;
  isFilterOpen: boolean;
  searchTextErrorMsg: string;
  reportMaster: ReportMaster;
  componentDestroyed$: Subject<boolean> = new Subject();
  filterDataShown: boolean;
  filterCount: number;
  reportSearchTypeLst: any = reportSearchTypeLst;
  bookingAdvanceSearchtypelst: any = bookingAdvanceSearchtypelst;
  countryRequest: CountryDataSourceRequest;
  brandSearchRequest: CommonCustomerSourceRequest;
  deptSearchRequest: CommonCustomerSourceRequest;
  buyerSearchRequest: CommonCustomerSourceRequest;
  disableStatusList: boolean = false;
  productGalleryImages: NgxGalleryImage[] = [];
  productGalleryOptions: NgxGalleryOptions[];
  supplierTypeList: any = supplierTypeList;
  requestCustomerModel: CustomerCommonDataSourceRequest;
  bookingTypes = BookingTypes;

  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {

    return this._redirectpath;
  }

  constructor(private service: BookingService, public validator: Validator, router: Router, private reportService: ReportService, public customerService: CustomerService,

    public officeService: OfficeService, private supplierService: SupplierService, route: ActivatedRoute, authserve: AuthenticationService, public accService: UserAccountService, public modalService: NgbModal, public calendar: NgbCalendar,
    public dateparser: NgbDateParserFormatter, public pathroute: ActivatedRoute, public utility: UtilityService
    , translate: TranslateService, toastr: ToastrService, public referenceService: ReferenceService,
    public brandService: CustomerBrandService,
    public deptService: CustomerDepartmentService,
    public collectionService: CustomerCollectionService,
    public buyerService: CustomerbuyerService,
    public locationService: LocationService,
    public refService: ReferenceService) {
    super(router, validator, route, translate, toastr);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
    //logged user has report checker role or insp verified(AE) role
    this.isDateUpdateRole = this.currentUser.roles.
      filter(x => x.id == this._roleEnum.ReportChecker).length > 0 ||
      this.currentUser.roles.
        filter(x => x.id == this._roleEnum.InspectionVerified).length > 0;
    //InspectionVerified
    this.currentRoute = router;
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.getIsMobile();
    this.reportMaster = new ReportMaster();
    this.countryRequest = new CountryDataSourceRequest();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.buyerSearchRequest = new CommonCustomerSourceRequest();
    this.requestCustomerModel = new CustomerCommonDataSourceRequest();
    this.reportMaster.DateName = datetypelst[0].name;
  }

  onInit(id?: any): void {

    var type = this.pathroute.snapshot.paramMap.get("type");

    this.reportMaster.entityId = parseInt(this.utility.getEntityId());

    this._searchbookingtid = this.pathroute.snapshot.paramMap.get("id");
    this.productGalleryOptions =
      [
        {
          "previewRotate": true, "thumbnails": false, "imageArrows": false, width: '800px',
          height: '500px'
        }

      ];
    this.Initialize(null);
    this.validator.setJSON("booking/booking-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];

    this.getInspectionBookingTypes(id);
  }
  Initialize(fromLocal) {
    this.filterDataShown = false;

    if (fromLocal == null)
      this.model = new InspectionBookingsummarymodel();

    this.containerMasterList = this.utility.getContainerList(100);


    this.reportMaster.selectedNumber = this.reportSearchTypeLst.find(x => x.id == 1) ?
      this.reportSearchTypeLst.find(x => x.id == 1).shortName : "";

    this.reportMaster.selectedNumberPlaceHolder = this.reportSearchTypeLst.find(x => x.id == 1) ?
      "Enter " + this.reportSearchTypeLst.find(x => x.id == 1).name : "";


    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.pageSize = 10;
    this.model.index = 1;

    this.data = [];
    this._statuslist = [];
    this.assignSupplierDetails();
    this.getCustomerListBySearch();
    this.getBookingStatusList();
    this.getOfficeLocationList();

    this.GetServiceTypelist();
    this.getCountryListBySearch();
    if (this._IsCustomerUser) {
      this.customerDefaultData();
      this.GetSearchData();
    }
  }
  customerDefaultData() {
    this.model.fromdate = this.calendar.getPrev(this.calendar.getToday(), 'd', 7);
    this.model.todate = this.calendar.getToday();
    this.model.customerid = this.currentUser.customerid;
    this.getCustomerBasedDetails(this.model.customerid);
  }
  ChangeCustomer(cusitem) {
    if (cusitem != null && cusitem.id != null) {
      this.assignSupplierDetails();
      this.reportMaster.supplierList = [];
      this.reportMaster.factoryList = [];
      this.reportMaster.brandList = [];
      this.reportMaster.deptList = [];
      this.reportMaster.buyerList = [];
      this.model.supplierid = null;
      this.model.factoryidlst = [];
      this.model.selectedBrandIdList = [];
      this.model.selectedDeptIdList = [];
      this.model.selectedBuyerIdList = [];
      this.model.serviceTypelst = [];
      this.getCustomerBasedDetails(this.model.customerid);
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

  RedirectToFbReport(reportPath, finalManualReportPath) {
    if (finalManualReportPath)
      window.open(finalManualReportPath);
    else if (reportPath)
      window.open(reportPath);
  }

  RedirectToReportSummaryLink(reportlink) {
    window.open(reportlink);
  }


  //get factory list based on the supplier change
  ChangeSupplier(supitem) {
    if (supitem != null && supitem.id != null) {
      this.reportMaster.factoryList = [];
      this.model.factoryidlst = [];
      this.getFactoryListBySearch();

      var supplierDetails = this.reportMaster.supplierList.find(x => x.id == supitem.id);
      if (supplierDetails)
        this.reportMaster.supplierName =
          supplierDetails.name.length > BookingSummaryNameTrim ?
            supplierDetails.name.substring(0, BookingSummaryNameTrim) + "..." : supplierDetails.name;
    }
    else {
      this.reportMaster.supplierName = "";
    }
  }

  //search textbox numeric validation for booking no and report no
  BookingNoValidation(bookingText) {
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && bookingText && bookingText.trim() != "" && ((isNaN(Number(bookingText))) || (bookingText.trim().length > 9));

    this.reportMaster.isShowSearchLens = bookingText && bookingText.trim() != "";
  }

  Reset() {
    this.reportMaster.supplierList = [];
    this.reportMaster.factoryList = [];
    this.Initialize(null);
  }

  SetSearchTypemodel(item) {
    this.model.searchtypeid = item.id;
    this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
    this.reportMaster.selectedNumber = item.shortName;

    if (item.id == SearchType.BookingNo) {
      this.reportMaster.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.PoNo) {
      this.reportMaster.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.CustomerBookingNo) {
      this.reportMaster.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.ProductId) {
      this.reportMaster.selectedNumberPlaceHolder = "Enter " + item.name;
    }
    else if (item.id == SearchType.ReportNo) {
      this.reportMaster.selectedNumberPlaceHolder = "Enter " + item.name;
    }
  }

  SetSearchDatetype(item) {
    this.model.datetypeid = item.id;
    this.reportMaster.DateName = item.name;
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
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }
  formValid(): boolean {

    let isOk = !this._customvalidationforbookid && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())

    return isOk;
  }

  GetSearchData() {
    this.searchloading = true;
    this.model.callingFrom = PageTypeSummary.CustomerReport;

    if (this.isFilterOpen)
      this.isFilterOpen = !this.isFilterOpen;

    this.filterDataShown = this.filterTextShown();

    this.reportService.SearchReportSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);

            this._statuslist = response.inspectionStatuslst;
            this.model.items = response.data.map((x) => {
              var item: ReportItem = {
                bookingId: x.bookingId,
                customerBookingNo: x.customerBookingNo,
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
                factoryId: x.factoryId,
                productCategory: x.productCategory,
                productList: [],
                isExpand: false,
                reportSummaryLink: x.reportSummaryLink,
                reportDate: x.reportDate,
                productCount: x.productCount,
                bookingType: x.bookingType,
                isEAQF: x.isEAQF
              }
              return item;
            }
            );

            this.applyGroupColor();
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


  deleteConfirMission() {
    this.modelRef.close();
  }

  // get booking product list by booking id
  getProductListByBooking(rowItem) {
    this.service.GetProductDetailsByBooking(rowItem.bookingId)
      .subscribe(res => {
        this.reportMaster.pageLoader = false;
        if (res.result == 1) {
          rowItem.productList = res.bookingProductsList;
          rowItem.productList.forEach(element => {
            element.poNumberCountDisplay = element.poNumberCount - 1 > 0 ? ", +" + (element.poNumberCount - 1) : '';
            element.poNumberShow = element.poNumberList && element.poNumberList.length > 0 ? element.poNumberList[0] : '';
            element.productDescTrim = element.productDescription.length > 46 ?
              element.productDescription.substring(0, 46) + "..." : element.productDescription;
            element.isProductDescTooltipShow = element.productDescription.length > 46;
          });
          rowItem.statusList = res.bookingStatusList;
          // this.bookingMasterData.statusList = res.bookingStatusList;
          // this.bookingMasterData.productList = rowItem.productList;
          this.model.items.filter(x => x.bookingId == rowItem.bookingId)[0].productList = rowItem.productList;
          this.searchloading = false;
        }
      },
        error => {
          this.reportMaster.pageLoader = false;
          rowItem.isPlaceHolderVisible = false;
          this.exportDataLoading = false;
        });
  }

  getContainerDetailsByBooking(rowItem) {
    this.service.GetContainerDetailsByBooking(rowItem.bookingId)
      .subscribe(res => {
        this.reportMaster.pageLoader = false;
        if (res.result == 1) {
          rowItem.containerList = res.bookingContainerList;
          rowItem.statusList = res.bookingStatusList;
          rowItem.isPlaceHolderVisible = true;
          // this.bookingMasterData.statusList = res.bookingStatusList;
        }
      },
        error => {
          this.reportMaster.pageLoader = false;
          rowItem.isPlaceHolderVisible = false;
          this.exportDataLoading = false;
        });
  }
  // get booking product list by booking id
  getProductOrContainerListByBooking(rowItem) {
    if (rowItem.serviceTypeId != this.inspectionServiceType.Container) {
      this.getProductListByBooking(rowItem);
    }
    else {
      this.getContainerDetailsByBooking(rowItem);
    }
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
      this.getProductOrContainerListByBooking(rowItem);
    }
    else {
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get po list by booking and product
  getPoListByBookingAndProduct(modalOpen, rowItem, containerId, serviceTypeId, content) {
    if (serviceTypeId != this.inspectionServiceType.Container) {
      this.reportMaster.pageLoader = true;
      this.service.GetPODetailsByBookingAndProduct(rowItem.bookingId, rowItem.id)
        .subscribe(res => {
          this.reportMaster.pageLoader = false;
          if (res.result == 1) {
            rowItem.isPlaceHolderVisible = false;
            this.reportMaster.poList = res.bookingProductPoList;
            if (modalOpen)
              this.modalService.open(content, { windowClass: 'product-detail-wrapper stick-to-bottom' });
          }

        },
          error => {
            this.reportMaster.pageLoader = false;
            rowItem.isPlaceHolderVisible = false;
          });
    }
    else {
      this.service.GetPODetailsByBookingAndConatinerAndProduct(rowItem.bookingId, containerId, rowItem.id)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            this.reportMaster.productName = rowItem.productName;
            this.reportMaster.containerPOList = res.bookingProductPoList;
            this.reportMaster.bookingNumber = this.reportMaster.containerPOList && this.reportMaster.containerPOList[0] ?
              this.reportMaster.containerPOList[0].bookingId : '';
            this.reportMaster.isproductOrPODetails = false;
          }
        },
          error => {
            rowItem.isPlaceHolderVisible = false;
          });
    }
  }

  toggleExpandRowProduct(modalOpen, event, rowItem, content) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];
    var containerId = this.reportMaster.containerItem.id;
    var serviceTypeId = this.reportMaster.bookingItem.serviceTypeId;
    this.getPoListByBookingAndProduct(modalOpen, rowItem, containerId, serviceTypeId, content);
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
  RedirectToEdit(bookingId) {
    let entity = this.utility.getEntityName();
    let editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
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
    if (data) {
      const blob = new Blob([data], { type: mimeType });
      if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob, "customer_report.xlsx");
      }
      else {
        const a = document.createElement('a');
        const url = window.URL.createObjectURL(blob);
        a.download = "customer_report.xlsx";
        a.href = url;
        a.click();
        //window.open(url);
      }
    }
    else
      this.showError('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.MSG_FILE_NOT_FOUND');

    this.exportDataLoading = false;
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
    this.assignSupplierDetails();
    this.model.customerid = null;
    this.model.supplierid = null;
    this.reportMaster.supplierList = [];
    this.reportMaster.factoryList = [];
    this.model.factoryidlst = [];
    this.getCustomerListBySearch();
  }
  clearSupplier() {
    this.model.supplierid = null;
    this.reportMaster.factoryList = null;
    this.model.factoryidlst = [];
    this.getSupplierListBySearch();
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  getContainerList() {

    for (let index = 1; index <= 100; index++) {

      this.containerMasterList.push({ "id": index, "name": "container - " + index });

    }
  }

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
  }

  // get product list by container and booking
  getProductListByContainerAndBooking(modalOpen, rowItem, content, bookingItem) {

    this.service.GetProductListByBookingAndContainer(rowItem.bookingId, rowItem.id)
      .subscribe(res => {
        this.reportMaster.pageLoader = false;
        if (res.result == 1) {
          rowItem.productList = res.bookingProductList;
          this.reportMaster.containerProductList = res.bookingProductList;

          this.reportMaster.containerProductList.forEach(element => {
            element.productDescTrim = element.productDescription.length > 78 ?
              element.productDescription.substring(0, 78) + "..." : element.productDescription;
            element.isProductDescTooltipShow = element.productDescription.length > 78;
          });

          this.reportMaster.isproductOrPODetails = true;

          this.reportMaster.bookingItem = bookingItem;
          this.reportMaster.containerItem = rowItem;
          rowItem.isPlaceHolderVisible = false;
          if (modalOpen)
            this.modalService.open(content, { windowClass: 'product-detail-wrapper stick-to-bottom' });
        }
      },
        error => {
          this.reportMaster.pageLoader = false;
          rowItem.isPlaceHolderVisible = false;
        });
  }

  //open the popup
  openSetStatusPopup(reportDatepopup, bookingId: number) {
    this.model.reportDate = this.calendar.getToday();
    this.bookingId = bookingId;

    this.modelRef = this.modalService.open(reportDatepopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });

    this.modelRef.result.then((result) => {

    }, (reason) => {

    });
  }

  //update booking status to report sent status
  setStatus() {
    this.reportMaster.statusLoading = true;

    var request: StatusRequest = {
      bookingId: this.bookingId,
      reportDate: this.model.reportDate
    };

    this.reportService.Updatestatus(request)
      .pipe()
      .subscribe(
        res => {
          this.modelRef.close();
          this.GetSearchData();
          this.model.reportDate = null;
          if (res) {
            if (res == CustomerReportSummaryResponseResult.Success) {
              this.showSuccess("BOOKING_SUMMARY.LBL_REPORT", "BOOKING_SUMMARY.MSG_UPDATED_SUCCESSFULLY");
            }
            else {
              this.showError("BOOKING_SUMMARY.LBL_REPORT", "COMMON.MSG_UNKNONW_ERROR");
            }
          }
          this.reportMaster.statusLoading = false;
        }
      ),
      (error: any) => {
        console.log(error);
      }
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
      this.reportMaster.requestCustomerCommonModel.idList.push(this.model.customerid);
    }
    else {
      this.reportMaster.requestCustomerCommonModel.idList = null;
    }

    this.reportMaster.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerListByUserType(this.reportMaster.requestCustomerCommonModel, null, term)
        : this.customerService.getCustomerListByUserType(this.reportMaster.requestCustomerCommonModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.customerLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.customerList = data;
        if (this._IsCustomerUser) {
          this.customerDefaultData();
        }
        this.reportMaster.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.reportMaster.requestCustomerCommonModel.searchText = this.reportMaster.customerInput.getValue();
      this.reportMaster.requestCustomerCommonModel.skip = this.reportMaster.customerList.length;
    }

    this.reportMaster.customerLoading = true;
    this.customerService.getCustomerListByUserType(this.reportMaster.requestCustomerCommonModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.reportMaster.customerList = this.reportMaster.customerList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.reportMaster.requestCustomerCommonModel.skip = 0;
          this.reportMaster.requestCustomerCommonModel.take = ListSize;
        }
        this.reportMaster.customerLoading = false;
      }),
      error => {
        this.reportMaster.customerLoading = false;
        this.setError(error);
      };
  }

  //get supplier details based on customer id
  getCustomerBasedDetails(customerId) {
    if (customerId) {
      this.getSupplierListBySearch();
      if (this.model.supplierid)
        this.getFactoryListBySearch();
      this.GetServiceTypelist();
      this.brandSearchRequest.customerId = customerId;
      this.deptSearchRequest.customerId = customerId;
      this.buyerSearchRequest.customerId = customerId;
      this.getBrandListBySearch();
      this.getBuyerListBySearch();
      this.getDeptListBySearch();

      var customerDetails = this.reportMaster.customerList.find(x => x.id == customerId);
      if (customerDetails)
        this.reportMaster.customerName =
          customerDetails.name.length > BookingSummaryNameTrim ?
            customerDetails.name.substring(0, BookingSummaryNameTrim) + "..." : customerDetails.name;
    }
    else {
      this.reportMaster.customerName = "";
    }
  }

  //get supplier list search
  getSupplierListBySearch() {
    this.reportMaster.supsearchRequest.supplierId = null;
    this.reportMaster.supsearchRequest.customerId = null;
    if (this.model.customerid)
      this.reportMaster.supsearchRequest.customerId = this.model.customerid;
    this.reportMaster.supsearchRequest.supplierType = SupplierType.Supplier;
    if (this.model.supplierid)
      this.reportMaster.supsearchRequest.supplierId = this.model.supplierid;
    this.reportMaster.supInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.supLoading = true),
      switchMap(term => term
        ? this.supplierService.getFactoryDataSourceList(this.reportMaster.supsearchRequest, term)
        : this.supplierService.getFactoryDataSourceList(this.reportMaster.supsearchRequest)
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
    this.reportMaster.supsearchRequest.searchText = this.reportMaster.supInput.getValue();
    this.reportMaster.supsearchRequest.skip = this.reportMaster.supplierList.length;

    this.reportMaster.supsearchRequest.customerId = this.model.customerid;
    this.reportMaster.supsearchRequest.supplierType = SupplierType.Supplier;
    this.reportMaster.supLoading = true;
    this.supplierService.getFactoryDataSourceList(this.reportMaster.supsearchRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.reportMaster.supplierList = this.reportMaster.supplierList.concat(customerData);
        }
        this.reportMaster.supsearchRequest.skip = 0;
        this.reportMaster.supsearchRequest.take = ListSize;
        this.reportMaster.supLoading = false;
      }),
      (error: any) => {
        this.reportMaster.supLoading = false;
      };
  }

  //apply filters to fetch factory
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

  //get factory details
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

  //get the booking status list
  getBookingStatusList() {
    this.reportMaster.bookingStatusLoading = true;
    this.service.getBookingStatusList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.processBookingStatusList(response);
        },
        error => {
          this.setError(error);
          this.reportMaster.bookingStatusLoading = false;
        });
  }

  //process the booking status list response
  processBookingStatusList(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.reportMaster.bookingStatusList = response.dataSourceList;
    }
    this.reportMaster.bookingStatusLoading = false;
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

  SetSearchTypeText(searchtypetext) {
    this.model.advancedSearchtypeid = searchtypetext;
  }

  GetServiceTypelist() {
    this.reportMaster.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();

    this.referenceService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {

          this.reportMaster.serviceTypeList = response.serviceTypeList;
          this.reportMaster.serviceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.reportMaster.serviceTypeList = [];
          this.reportMaster.serviceTypeLoading = false;
        }
      );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerid ?? 0;
    serviceTypeRequest.serviceId = Service.Inspection;
    return serviceTypeRequest;
  }

  //fetch the country data with virtual scroll
  getCountryData() {

    this.countryRequest.searchText = this.reportMaster.countryInput.getValue();
    this.countryRequest.skip = this.reportMaster.countryList.length;

    this.reportMaster.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.reportMaster.countryList = this.reportMaster.countryList.concat(customerData);
        }

        this.countryRequest = new CountryDataSourceRequest();
        this.reportMaster.countryLoading = false;
      }),
      error => {
        this.reportMaster.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.reportMaster.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.countryLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.countryList = data;
        this.reportMaster.countryLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.brandSearchRequest.searchText = this.reportMaster.brandInput.getValue();
    this.brandSearchRequest.skip = this.reportMaster.brandList.length;

    this.reportMaster.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.brandSearchRequest).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.reportMaster.brandList = this.reportMaster.brandList.concat(brandData);
        }
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.brandSearchRequest.customerId = this.model.customerid;

        this.reportMaster.brandLoading = false;
      }),
      error => {
        this.reportMaster.brandLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {
    this.reportMaster.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.brandLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.brandList = data;
        this.reportMaster.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.deptSearchRequest.searchText = this.reportMaster.deptInput.getValue();
    this.deptSearchRequest.skip = this.reportMaster.deptList.length;

    this.reportMaster.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.reportMaster.deptList = this.reportMaster.deptList.concat(deptData);
        }
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.deptSearchRequest.customerId = this.model.customerid;

        this.reportMaster.deptLoading = false;
      }),
      error => {
        this.reportMaster.deptLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {
    if (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) {
      this.deptSearchRequest.idList = this.model.selectedDeptIdList;
    }
    else {
      this.deptSearchRequest.idList = null;
    }

    this.reportMaster.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.deptLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.deptList = data;
        this.reportMaster.deptLoading = false;
      });
  }

  //get Booking Type
  getInspectionBookingTypes(id) {
    this.refService.getInspectionBookingTypes()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionBookingTypes(response);
        },
        error => {
          this.setError(error);
          this.reportMaster.inspectionBookingTypeVisible = false;
        }
      );
  }

  //process the inspection type
  processInspectionBookingTypes(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.reportMaster.inspectionBookingTypes = response.inspectionBookingTypeList;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.reportMaster.inspectionBookingTypeVisible = false;
    }
  }

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.buyerSearchRequest.searchText = this.reportMaster.buyerInput.getValue();
    this.buyerSearchRequest.skip = this.reportMaster.buyerList.length;

    this.reportMaster.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest).
      subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.reportMaster.buyerList = this.reportMaster.buyerList.concat(buyerData);
        }
        this.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.buyerSearchRequest.customerId = this.model.customerid;

        this.reportMaster.buyerLoading = false;
      }),
      error => {
        this.reportMaster.buyerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {
    this.reportMaster.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.reportMaster.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.reportMaster.buyerLoading = false))
      ))
      .subscribe(data => {
        this.reportMaster.buyerList = data;
        this.reportMaster.buyerLoading = false;
      });
  }

  getSearchDetails() {
    var searchLoading = true;
    if (this.model.searchtypetext && this.model.searchtypetext != '') {
      if (!this.isFilterOpen)
        this.isFilterOpen = this.model.searchtypeid != this._booksearttypeid && !(this.model.customerid > 0);
      this.SearchDetails();
    }
  }

  isfilterData() {
    if ((this.model.fromdate && this.model.fromdate != '') ||
      (this.model.todate && this.model.todate != '') || this.model.customerid > 0 || this.model.supplierid > 0
      || (this.model.factoryidlst && this.model.factoryidlst.length > 0) ||
      (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) ||
      (this.model.selectedProvinceIdList && this.model.selectedProvinceIdList.length > 0) ||
      (this.model.selectedCityIdList && this.model.selectedCityIdList.length > 0) ||
      (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) ||
      (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) ||
      (this.model.officeidlst && this.model.officeidlst.length > 0) ||
      (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) ||
      (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0) ||
      (this.model.barcode && this.model.barcode != '') ||
      (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') ||
      this.model.isPicking || this.model.isEAQF) {
      this.filterDataShown = true;
    }
    else {
      this.filterDataShown = false;
    }
  }

  changeCountryData() {
    this.isfilterData();
    this.getProvinceByCountryList();
  }

  changeProvinceData() {
    this.isfilterData();
    this.getCityByProvinceList();
  }

  changeCityData() {
    this.isfilterData();
    this.getProvinceByCountryList();
  }

  //get the province list
  getProvinceByCountryList() {
    this.reportMaster.provinceLoading = true;
    var request = new ProvinceListSearchRequest();
    if (this.model.selectedCountryIdList)
      request.countryIds = this.model.selectedCountryIdList;

    this.locationService.getProvinceByCountryIds(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        if (response)
          this.processProvinceListResponse(response);
        this.reportMaster.provinceLoading = false;
      }),
      error => {
        this.reportMaster.provinceLoading = false;
        this.setError(error);
      };
  }

  //process the province list
  processProvinceListResponse(response) {
    if (response.result == ProvinceResult.Success) {
      this.reportMaster.provinceList = response.dataSourceList;
    }
    else if (response.result == ProvinceResult.DataNotFound) {
      this.showWarning('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.MSG_PROVINCE_NOT_FOUND');
    }
  }

  //get the city list
  getCityByProvinceList() {
    this.reportMaster.cityLoading = true;

    var request = new CityListSearchRequest();
    if (this.model.selectedProvinceIdList)
      request.provinceIds = this.model.selectedProvinceIdList;

    this.locationService.getCityByProvinceIds(request)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(response => {
        this.processCityListResponse(response);
        this.reportMaster.cityLoading = false;
      }),
      error => {
        this.reportMaster.cityLoading = false;
        this.setError(error);
      };
  }

  //process the city list response
  processCityListResponse(response) {
    if (response.result == ResponseResult.Success) {
      this.reportMaster.cityList = response.dataSourceList;
    }
    else if (response.result == ProvinceResult.DataNotFound) {
      this.showWarning('BOOKING_SUMMARY.TITLE', 'BOOKING_SUMMARY.MSG_CITY_NOT_FOUND');
    }
  }

  statusChange(item) {
    if (item) {

      if (this.model.statusidlst && this.model.statusidlst.length > 0) {

        var statusLength = this.model.statusidlst.length;

        var statusDetails = [];
        var maxLength = statusLength > 1 ? 1 : statusLength;

        for (var i = 0; i < maxLength; i++) {
          statusDetails.push(this.reportMaster.bookingStatusList.find(x => x.id == this.model.statusidlst[i]).name);
        }

        if (statusLength > 1) {
          statusDetails.push(" " + (statusLength - 1) + "+");
        }
        this.reportMaster.statusNameList = statusDetails;
      }
      else {
        this.reportMaster.statusNameList = [];
      }
    }
  }

  officeChange(item) {
    if (item) {

      if (this.model.officeidlst && this.model.officeidlst.length > 0) {

        var officeLength = this.model.officeidlst.length;

        var officeDetails = [];
        var maxLength = officeLength > 1 ? 1 : officeLength;
        for (var i = 0; i < maxLength; i++) {

          officeDetails.push(this.reportMaster.officeList.find(x => x.id == this.model.officeidlst[i]).name);
        }

        if (officeLength > 1) {
          officeDetails.push(" " + (officeLength - 1) + "+");
        }
        this.reportMaster.officeNameList = officeDetails;
      }
      else {
        this.reportMaster.officeNameList = [];
      }
    }
  }

  clickStatus(id) {
    if (id && id > 0) {

      //if it contains the value
      var isValueExists = this.model.statusidlst.includes(id);

      //open the filter
      if (!this.isFilterOpen)
        this.isFilterOpen = true;

      this.toggleFormSection = false;

      if (isValueExists) {
        this.model.statusidlst = this.model.statusidlst.filter(x => x != id);
        this.model.statusidlst = [...this.model.statusidlst];

      }
      else {
        this.model.statusidlst.push(id);
        this.model.statusidlst = [...this.model.statusidlst];
      }
    }
  }

  changeStatus(id) {
    return this.model.statusidlst.includes(id);
  }


  filterTextShown() {
    var isFilterDataSelected = false;

    if ((this.model.fromdate && this.model.fromdate != '') ||
      (this.model.todate && this.model.todate != '') || this.model.customerid > 0 || this.model.supplierid > 0
      || (this.model.factoryidlst && this.model.factoryidlst.length > 0) ||
      (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) ||
      (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) ||
      (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) ||
      (this.model.officeidlst && this.model.officeidlst.length > 0) ||
      (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) ||
      (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) ||
      (this.model.statusidlst && this.model.statusidlst.length > 0) ||
      (this.model.barcode && this.model.barcode != '') ||
      (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') ||
      this.model.isPicking) {

      //desktop version
      if (!this.isMobile) {
        isFilterDataSelected = true;
      }
      //mobile version
      else if (this.isMobile) {
        var count = 0;
        //date add
        count = MobileViewFilterCount + count;

        if (this.model.customerid > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.supplierid > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.factoryidlst && this.model.factoryidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedCountryIdList && this.model.selectedCountryIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedDeptIdList && this.model.selectedDeptIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedBrandIdList && this.model.selectedBrandIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.officeidlst && this.model.officeidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.serviceTypelst && this.model.serviceTypelst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.selectedBuyerIdList && this.model.selectedBuyerIdList.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.statusidlst && this.model.statusidlst.length > 0) {
          count = MobileViewFilterCount + count;
        }
        if (this.model.barcode && this.model.barcode != '') {
          count = MobileViewFilterCount + count;
        }
        if (this.model.advancedsearchtypetext && this.model.advancedsearchtypetext != '') {
          count = MobileViewFilterCount + count;
        }
        if (this.model.isPicking) {
          count = MobileViewFilterCount + count;
        }

        this.filterCount = count;

        isFilterDataSelected = true;
      }
      else {
        this.filterCount = 0;
        this.reportMaster.countryNameList = [];
        this.reportMaster.supplierName = "";
        this.reportMaster.customerName = "";
        this.reportMaster.factoryNameList = [];
        this.reportMaster.officeNameList = [];
        this.reportMaster.serviceTypeNameList = [];
        this.reportMaster.brandNameList = [];
        this.reportMaster.buyerNameList = [];
        this.reportMaster.deptNameList = [];
        this.reportMaster.statusNameList = [];
      }
    }

    return isFilterDataSelected;
  }

  toggleExpandRowContainer(modalOpen, rowItem, content, bookingItem) {
    if (modalOpen)
      this.reportMaster.pageLoader = true;
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];
    this.getProductListByContainerAndBooking(modalOpen, rowItem, content, bookingItem);
  }
  openProductDetail(content, index, rowItem, containerId, serviceTypeId, bookingId) {
    this.reportMaster.pageLoader = true;
    this.reportMaster.poList = [];
    this.reportMaster.bookingNumber = bookingId;
    this.reportMaster.productId = rowItem.productId;

    this.getPoListByBookingAndProduct(true, rowItem, containerId, serviceTypeId, content);
  }

  changeSupplierType(item) {
    this.reportMaster.supLoading = true;
    this.reportMaster.supplierList = [];
    this.model.supplierid = null;
    this.reportMaster.factoryList = [];
    this.model.factoryidlst = [];
    if (item.id == SearchType.SupplierCode) {
      this.reportMaster.supsearchRequest.supSearchTypeId = SearchType.SupplierCode;
    }
    else if (item.id == SearchType.SupplierName) {
      this.reportMaster.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    }
    this.getSupplierData();
  }

  assignSupplierDetails() {
    this.reportMaster.supsearchRequest.supSearchTypeId = SearchType.SupplierName;
    this.model.supplierTypeId = SearchType.SupplierName;
  }
  getPreviewProductImage(imageUrl, modalcontent) {
    this.reportMaster.pageLoader = true;
    if (imageUrl && imageUrl != null && imageUrl != "") {

      this.productGalleryImages = [];

      this.productGalleryImages.push(
        {
          small: imageUrl,
          medium: imageUrl,
          big: imageUrl,
        });

      this.reportMaster.pageLoader = false;
      this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
  }
}


