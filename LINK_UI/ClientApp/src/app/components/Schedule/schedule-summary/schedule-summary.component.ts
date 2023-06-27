import { Component, OnInit } from '@angular/core';
import { PageSizeCommon, SearchType, DefaultDateType, bookingSearchtypelst, scheduleDatetypelst, RoleEnum, BookingSearchRedirectPage, BookingStatus, ScheduleRole, Url, EntPageFieldAccessEnum, Service, RightsEnum, ScheduleSummaryExportType, EntityAccess, scheduleSummaryExportTypeList } from '../../common/static-data-common'
import { SummaryComponent } from '../../common/summary.component';
import { ScheduleModel, BookingItemSchedule, MandayModel, QuotationMandayResult, ScheduleFilterMaster, ScheduleZoneFilterMaster, QcVisibilityBookingModel, BookingDataQcVisible, bookingDataQcVisibleRequest, ScheduleProductModel, ScheduleMasterData } from '../../../_Models/Schedule/schedulemodel';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap } from 'rxjs/operators';
import { Validator } from "../../common/validator";
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ScheduleService } from '../../../_Services/Schedule/schedule.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { UserModel } from '../../../_Models/user/user.model';
import { HolidayMasterItemModel } from '../../../_Models/hr/holidaymaster.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { QcDataSourceRequest, CommonZoneSourceRequest, ResponseResult } from '../../../_Models/common/common.model';
import { of } from 'rxjs';
import { QuotationService } from '../../../_Services/quotation/quotation.service';
import { EntPageFieldAccessResult, EntPageRequest } from 'src/app/_Models/booking/inspectionbooking.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';

@Component({
  selector: 'app-schedule-summary',
  templateUrl: './schedule-summary.component.html',
  styleUrls: ['./schedule-summary.component.scss'],
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
export class ScheduleSummaryComponent extends SummaryComponent<ScheduleModel> {
  public model: ScheduleModel;
  public mandaymodel: MandayModel;
  public saveQcVisibleModel: bookingDataQcVisibleRequest;
  public qcVisibilityBookingModel: QcVisibilityBookingModel;
  public scheduleProductModel: ScheduleProductModel;
  public data: any;
  public searchloading: boolean = false;
  public error: string = "";
  public modelRef: NgbModalRef;
  public modelPopUpRef: NgbModalRef;
  ismodalOpen: boolean = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  bookingSearchtypelst: any = bookingSearchtypelst;
  datetypelst: any = scheduleDatetypelst;
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  public _statuslist: any[] = [];
  _bookingredirectpage = BookingSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _BookingStatus = BookingStatus;
  _ScheduleRole = ScheduleRole;
  _searchbookingtid: any;
  isReInspection: boolean = false;
  isReBooking: boolean = false;
  cancelLoading: boolean;
  titlePopup: string;
  public modelPopup: BookingItemSchedule;
  public QCStaffList: Array<any>;
  public CSStaffList: Array<any>;
  public QCList: Array<any>;
  public AdditionalQCList: Array<any>;
  public CSList: Array<any>;
  provinceLoading: boolean = false;
  cityLoading: boolean = false;
  public isSchedulePending: boolean;
  public isBookingForEmailSelected: boolean = false;
  public isBookingForQcVisible: boolean = false;
  public showSelectAllCheckBox: boolean = false;
  public showSelectAllQCCheckBox: boolean = false;
  public sendEmailLoading: boolean = false;
  public selectAllChecked: boolean = false;
  public selectAllQcVisibleChecked: boolean = false;
  public selectAllQcPopupChecked: boolean = false;
  public user: UserModel;
  public hasInspScheduleRole: boolean = false;
  _RoleEnum = RoleEnum;
  public showCheckBox: boolean = false;
  public isPopUpOpen: boolean = false;
  public popUpLoading: boolean = false;
  public loader: boolean = false;
  public mandaySave: boolean = false;
  public qcVisibleSave: boolean = false;
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  public redirectFromDetail: boolean = false;
  public mandayCount: number;
  public _quotationStatusList: any[] = [];
  public scheduleFilterRequest: ScheduleFilterMaster;
  public scheduleZoneFilterRequest: ScheduleZoneFilterMaster;
  public qcRequest: QcDataSourceRequest;
  public zoneRequest: CommonZoneSourceRequest;
  public showMandayButton: boolean;
  public qcVisiblePopUpData: Array<BookingDataQcVisible>;
  public isQcVisiblePopUpSave: boolean = true;
  entPageFieldAccessEnum = EntPageFieldAccessEnum;
  filterEntFieldAccess: any;
  entPageFieldAccess: any;
  scheduleSummaryExportTypeList = scheduleSummaryExportTypeList;
  serviceTypeList: Array<any>;
  scheduleMasterData: ScheduleMasterData;
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "schedule/schedule-allocation";
  }

  constructor(private service: BookingService, private serviceSCH: ScheduleService, private serviceLocation: LocationService, public validator: Validator, router: Router,
    route: ActivatedRoute, authserve: AuthenticationService, public modalService: NgbModal, public calendar: NgbCalendar, private quotService: QuotationService,
    public dateparser: NgbDateParserFormatter, public pathroute: ActivatedRoute, public utility: UtilityService, translate: TranslateService,
    toastr: ToastrService, public refService: ReferenceService) {
    super(router, validator, route, translate, toastr);
    this.currentRoute = router;
    this.modelPopup = new BookingItemSchedule();
    this.toggleFormSection = false;
    this.isFilterOpen = true;
    this.getIsMobile();
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     this.onInit();
    //   }
    // });
  }
  onInit(): void {
    var type = this.pathroute.snapshot.paramMap.get("type");
    this._searchbookingtid = this.pathroute.snapshot.paramMap.get("id");
    this.isSchedulePending = this.currentRoute.url.indexOf("schedule-pending") >= 0;
    this.redirectFromDetail = this.currentRoute.url.indexOf("param") >= 0;
    this.Initialize();
    this.validator.setJSON("schedule/schedule-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.getEntPageFieldAccessList();
    this.scheduleMasterData = new ScheduleMasterData();
    this.scheduleMasterData.entityId = parseInt(this.utility.getEntityId());

  }
  Initialize() {
    this.model = new ScheduleModel();

    if (localStorage.getItem('currentUser')) {
      this.user = JSON.parse(localStorage.getItem('currentUser'));
      var index = this.user.roles.findIndex(x => x.id == this._RoleEnum.InspectionScheduled);
      if (index != -1) {
        this.hasInspScheduleRole = true;
      }
    }

    this.validator.isSubmitted = false;
    this.isBookingForQcVisible = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.model.pageSize = 10;
    this.model.index = 0;
    this.data = [];
    this._statuslist = [];
    this._quotationStatusList = [];
    this.scheduleFilterRequest = new ScheduleFilterMaster();
    this.scheduleZoneFilterRequest = new ScheduleZoneFilterMaster();
    this.qcRequest = new QcDataSourceRequest();
    this.zoneRequest = new CommonZoneSourceRequest();
    this.getServiceTypeList();
    this.getQcListBySearch();
    this.getZoneListBySearch();
    this.getQuotationStatusColor();
    this.service.Getbookingsummary()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            this.CheckRequestStatus();
            this.GetSupplierlist(this.model.customerid);
            this.GetFactoryList(this.model.supplierid);
            this.refreshCountry();

            if (this.model.countryId && this.model.countryId > 0) {
              this.getProvince(this.model.countryId);
            }

            if (this.model.provinceId && this.model.provinceId > 0) {
              this.getCity(this.model.provinceId);
            }

            if (this._searchbookingtid) {
              this.model.searchtypetext = this._searchbookingtid;
              this.GetSearchData();
            }
          }
          else {
          }
          this.Initialloading = false;
        },
        error => {
          this.Initialloading = false;
          this.setError(error);
        });
  }
  ChangeCustomer(cusitem) {
    this.model.supplierid = null;
    this.model.factoryidlst = [];
    //if (cusitem != null && cusitem.id != null) {
    this.GetSupplierlist(cusitem.id);
    this.data.factoryList = [];
    //}
  }
  ChangeSupplier(supitem) {
    this.model.factoryidlst = [];
    this.data.factoryList = [];
    //if (supitem != null && supitem.id != null) {
    this.GetFactoryList(supitem.id);
    //}
  }
  GetSupplierlist(id) {
    if (id) {
      this.suploading = true;
      this.service.Getsupplierbycusid(id)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data.supplierList = response.data;
            }
            this.suploading = false;
          },
          error => {
            this.suploading = false;
          }
        );
    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
      this.data.supplierList = [];
      this.model.supplierid = null;
    }
  }
  GetFactoryList(supId) {
    if (supId) {
      this.factloading = true;
      this.service.GetFactoryDetailsByCusSupId(supId, this.model.customerid)
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data.factoryList = response.data;
            }
            this.factloading = false;
          },
          error => {
            this.factloading = false;
          }
        );
    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
    }
  }
  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && ((isNaN(Number(this.model.searchtypetext))) || (this.model.searchtypetext.trim().length > 9));
  }


  Reset() {
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
    this.isBookingForQcVisible = false;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }
  IsDateValidationRequired(): boolean {
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }
  formValid(): boolean {
    return !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
  }
  GetSearchData() {
    this.searchloading = true;
    this.model.noFound = false;
    this.showSelectAllCheckBox = false;
    this.showSelectAllQCCheckBox = false;
    this.selectAllQcVisibleChecked = false;
    this.isBookingForEmailSelected = false;
    this.isBookingForQcVisible = false;
    this.serviceSCH.SearchInspectionBookingSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this.mandayCount = response.mandayCount;
            this._statuslist = response.inspectionStatuslst;
            this.mapItem(response.data);
            var indexQcVisible = this.model.items.findIndex(x => x.hasQcVisible == true);
            if (indexQcVisible != -1 && this.hasInspScheduleRole) {
              this.showSelectAllQCCheckBox = true;
            }
            //check whether to show the QC email check box

            var index = this.model.items.findIndex(x => x.showCheckBox == true);


            if (index != -1 && this.hasInspScheduleRole)
              this.showSelectAllCheckBox = true;
            this.showMandayButton = this.model.items.findIndex(x => x.isMandayButtonVisible == true) != -1 ? true : false;

          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this._statuslist = [];
            this.model.items = [];
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
  SearchByStatus(id) {
    this.model.statusidlst = [];
    this.model.statusidlst.push(id);
    this.SearchDetails();
  }
  GetStatusColor(statusName) {
    if (this._statuslist != null && statusName != null) {
      var result = this._statuslist.find(x => x.statusName == statusName);
      if (result)
        return result.statusColor;
    }
  }
  export() {
    this.exportDataLoading = true;
    this.serviceSCH.exportSummary(this.model)
      .subscribe(res => {
        if (this.modelRef)
          this.modelRef.close();
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "schedule_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "schedule_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }


  getStaff(values, roleId) {
    var result;
    if (this.QCStaffList == null || values == null || this.CSStaffList == null)
      return [];
    if (roleId == this._ScheduleRole.QC)
      return this.QCStaffList.filter(x => values);
    else
      return this.CSStaffList.filter(x => values);

  }

  refreshCountry() {
    this.serviceLocation.getCountrySummary()
      .pipe()
      .subscribe(
        result => {
          this.data.countryValues = result.countryList;
        }
      )
  }
  refreshprovince(countryid) {
    this.model.provinceId = null;
    this.model.cityId = null;
    this.getProvince(countryid);
  }

  getProvince(countryid: number) {
    if (countryid) {
      this.provinceLoading = true;
      this.serviceLocation.getprovincebycountryid(countryid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.provincevalues = result.data;
            }
            else {
              this.data.provincevalues = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.data.provincevalues = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.data.provincevalues = [];
    }
  }
  refreshcity(provinceid) {
    this.model.cityId = null;
    this.getCity(provinceid);
  }

  getCity(provinceid) {
    if (provinceid) {
      this.cityLoading = true;
      this.serviceLocation.getcitybyprovinceid(provinceid)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.cityvalues = result.data;
            }
            else {
              this.data.cityvalues = [];
            }
            this.cityLoading = false;
          },
          error => {
            this.data.cityvalues = [];
            this.cityLoading = false;
          });
    }
    else {
      this.data.cityvalues = [];
    }
  }
  getCSStaffList(content) {
    this.serviceSCH.getCSStaffList()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.CSStaffList = data.data;

            this.QCList = this.QCStaffList;
            this.AdditionalQCList = this.QCStaffList;
            this.CSList = this.CSStaffList;

            this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });

            this.modelRef.result.then((result) => {
            }, (reason) => {
            });
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });
  }
  getQCStaffList(content) {
    this.serviceSCH.getQCStaffList()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.QCStaffList = data.data;
            this.getCSStaffList(content);
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });
  }


  Check(item): boolean {
    return item.serviceDateQCNames != null && item.serviceDateQCNames.length > 0
  }


  mapItem(res) {
    this.model.items = res.map((x) => {
      //this.mapSchedule(x);
      var item: BookingItemSchedule = {
        bookingId: x.bookingId,
        poNumber: x.poNumber,
        customerName: x.customerName,
        supplierName: x.supplierName,
        factoryName: x.factoryName,
        serviceDateFrom: x.serviceDateFrom,
        serviceDateTo: x.serviceDateTo,
        serviceType: x.serviceType,
        office: x.office,
        statusName: x.statusName,
        statusId: x.statusId,
        bookingCreatedBy: x.bookingCreatedBy,
        customerId: x.customerID,
        internalReferencePo: x.internalReferencePo,
        isPicking: x.isPicking,
        previousBookingNo: x.previousBookingNo,
        factoryId: x.factoryId,
        countryId: x.countryId,
        productCategory: x.productCategory,
        manDay: x.manDay,
        serviceDateQCNames: x.serviceDateQCNames,
        serviceDateCSNames: x.serviceDateCSNames,
        serviceDateAdditionalQCNames: x.serviceDateAdditionalQCNames,
        factoryLocation: x.factoryLocation,
        factoryZoneName: x.zoneName,
        actualManDay: x.actualManDay ? Number.isInteger(x.actualManDay) ? x.actualManDay : x.actualManDay.toFixed(2) : 0,
        productCount: x.productcount,
        reportCount: x.reportCount,
        serviceDate: x.serviceDate,
        isBookingSelected: false,
        isQcVisibleBookingSelected: false,
        showCheckBox: false,
        isMandayButtonVisible: x.isMandayButtonVisible,
        factoryProvinceName: x.provinceName,
        factoryCityName: x.cityName,
        sampleSize: x.sampleSize,
        firstServiceDate: x.firstServiceDate,
        quotationStatus: x.quotationStatus,
        showAddButton: x.showAddButton,
        factoryCountyName: x.countyName,
        factoryTownName: x.townName,
        plannedManday: x.plannedManday,
        hasQcVisible: x.hasQcVisible,
        qcVisibleToEmail: x.qcVisibleToEmail,
        calculatedWorkingHours: x.calculatedWorkingHours,
        productSubCategory: x.productSubCategory,
        productSubCategory2: x.productSubCategory2,
        productId: x.productId,
        isMSChartProduct: x.isMSChartProduct,
        isEAQF: x.isEAQF,
        customerProductId: x.cuProductId
      }
      //check if service date to is greater than or equal to today 
      var isDateValid = this.CheckServiceDate(item.serviceDateTo);
      //enable the checkbox only if status is allocated QC, logged in user has insp schedule role and date is >= today
      if (item.statusId == this._BookingStatus.AllocateQC && this.hasInspScheduleRole && isDateValid && item.qcVisibleToEmail)
        item.showCheckBox = true;

      if (item.statusId == this._BookingStatus.AllocateQC && this.hasInspScheduleRole && item.hasQcVisible == false && isDateValid)
        item.hasQcVisible = true;
      else if (item.hasQcVisible == true) {
        item.hasQcVisible = false;
      }

      return item;
    }
    );
  }

  CheckRequestStatus() {
    if (this.isSchedulePending) {
      this.model.statusidlst = this.data.statusList.filter(x => x.id == BookingStatus.Confirmed || x.id == BookingStatus.AllocateQC).map(x => x.id);

    }

    else {
      this.model.statusidlst = this.data.statusList.filter(x => x.id == BookingStatus.Confirmed || x.id == BookingStatus.Requested
        || x.id == BookingStatus.Verified || x.id == BookingStatus.AllocateQC || x.id == BookingStatus.Inspected
        || x.id == BookingStatus.ReportSent).map(x => x.id);
    }

    if (!this.redirectFromDetail) {
      this.model.fromdate = this.calendar.getNext(this.calendar.getToday(), 'd', 1);
      this.model.todate = this.calendar.getNext(this.calendar.getToday(), 'd', 1);
      //this.search();
    }

  }

  //check and uncheck of the email checkbox
  Getbookingids(item) {
    var selectedcount = this.model.items.filter(x => x.isBookingSelected).length;

    //uncheck select all even if one selected value is unchecked
    if (selectedcount < this.model.items.filter(x => x.showCheckBox).length) {
      this.selectAllChecked = false;
      this.isBookingForEmailSelected = false;
    }
    else
      this.selectAllChecked = true;

    if (selectedcount && selectedcount > 0) {
      var count = this.model.items.filter(x => x.isBookingSelected).length;
      if ((count == selectedcount || count == 0)) {
        this.isBookingForEmailSelected = true;
      }
      else {
        this.model.items.filter(x => x.bookingId == item.bookingId)[0].isBookingSelected = false;
        this.isBookingForEmailSelected = false;
      }
    }
    else {
      this.isBookingForEmailSelected = false;
    }
  }

  //check and uncheck of the email checkbox
  GetQcVisiblePopUpbookingids() {
    var selectedcount = this.qcVisiblePopUpData.filter(x => x.isQcVisibility).length;
    //uncheck select all even if one selected value is unchecked
    if (selectedcount != this.qcVisiblePopUpData.length) {
      this.selectAllQcPopupChecked = false;
      // this.isQcVisiblePopUpSave = true;
    }
    else {
      this.selectAllQcPopupChecked = true;
      // this.isQcVisiblePopUpSave = false;
    }


    if (selectedcount && selectedcount > 0) {
      this.isQcVisiblePopUpSave = false;
    }
    else {
      this.isQcVisiblePopUpSave = true;
    }
  }


  //check and uncheck of the email checkbox
  GetQcVisiblebookingids(item) {

    var selectedcount = this.model.items.filter(x => x.isQcVisibleBookingSelected).length;

    //uncheck select all even if one selected value is unchecked
    if (selectedcount < this.model.items.filter(x => x.hasQcVisible).length) {
      this.selectAllQcVisibleChecked = false;
      this.isBookingForQcVisible = false;
    }
    else
      this.selectAllQcVisibleChecked = true;

    if (selectedcount && selectedcount > 0) {
      var count = this.model.items.filter(x => x.isQcVisibleBookingSelected).length;
      if ((count == selectedcount || count == 0)) {
        this.isBookingForQcVisible = true;
      }
      else {
        this.model.items.filter(x => x.bookingId == item.bookingId)[0].isQcVisibleBookingSelected = false;
        this.isBookingForQcVisible = false;
      }
    }
    else {
      this.isBookingForQcVisible = false;
    }
  }

  //Send QC schedule Email
  SendScheduleEmail(model) {
    this.sendEmailLoading = true;

    var bookingIdList = new Array();
    //fetch all the booking Ids of the selected checkboxes
    model.items.forEach(element => {
      if (element.isBookingSelected)
        bookingIdList.push(element.bookingId);
    });

    this.serviceSCH.sendEmail(bookingIdList)
      .pipe()
      .subscribe(
        result => {
          if (result) {
            this.sendEmailLoading = false;
            this.showSuccess('USER_CONFIG.LBL_EMAIL', 'SCHEDULE_SUMMARY.MSG_EMAIL_SUCCESSFUL');
            //uncheck the checkbox
            model.items.forEach(element => {
              element.isBookingSelected = false;
            });
            this.selectAllChecked = false; //uncheck the select all
            this.isBookingForEmailSelected = false; //hide the QC Schedule Email button
          }
          else {
            this.showError('USER_CONFIG.LBL_EMAIL', 'EXPENSE.MSG_CANNOT_GET');
          }
        },
        error => {
          this.showError('USER_CONFIG.LBL_EMAIL', 'EXPENSE.MSG_CANNOT_GET');
        });
  }

  SelectAllQcVisibleCheckBox() {
    var selectedcount = this.model.items.filter(x => x.isQcVisibleBookingSelected).length;
    if (selectedcount != this.model.items.filter(x => x.hasQcVisible).length) {
      this.model.items.forEach(element => {
        if (element.hasQcVisible)
          element.isQcVisibleBookingSelected = true;
      });
      this.isBookingForQcVisible = true; //show the QC Schedule Email button
      this.selectAllQcVisibleChecked = true; //check the select all
    }
    else {
      this.model.items.forEach(element => {
        element.isQcVisibleBookingSelected = false;
      });
      this.isBookingForQcVisible = false; //hide the QC Schedule Email button
      this.selectAllQcVisibleChecked = false; //uncheck the select all
    }
  }

  SelectAllCheckBox() {
    var selectedcount = this.model.items.filter(x => x.isBookingSelected).length;
    if (selectedcount != this.model.items.filter(x => x.showCheckBox).length) {
      this.model.items.forEach(element => {
        if (element.showCheckBox)
          element.isBookingSelected = true;
      });
      this.isBookingForEmailSelected = true; //show the QC Schedule Email button
      this.selectAllChecked = true; //check the select all
    }

    else {
      this.model.items.forEach(element => {
        element.isBookingSelected = false;
      });
      this.isBookingForEmailSelected = false; //hide the QC Schedule Email button
      this.selectAllChecked = false; //uncheck the select all
    }
  }
  SelectAllQcPopupCheckBox() {
    var selectedcount = this.qcVisiblePopUpData.filter(x => x.isQcVisibility).length;
    if (selectedcount != this.qcVisiblePopUpData.length) {
      this.qcVisiblePopUpData.forEach(element => {
        element.isQcVisibility = true;
      });
      this.isQcVisiblePopUpSave = false;
      this.selectAllQcPopupChecked = true //show the QC Schedule Email button
    }
    else {
      this.qcVisiblePopUpData.forEach(element => {
        element.isQcVisibility = false;
      });
      this.isQcVisiblePopUpSave = true;
      this.selectAllQcPopupChecked = false
    }
  }

  //check if the service date To is >= today's date
  CheckServiceDate(servicedateTo) {
    var todaydate = this.calendar.getToday();
    var date = this.dateparser.parse(servicedateTo);
    var serviceDate = new NgbDate(date.year, date.month, date.day);
    var servicedayafter = todaydate.before(serviceDate);
    var servicedateequaltodaydate = todaydate.equals(serviceDate);
    return servicedayafter || servicedateequaltodaydate;
  }
  openQcVisibilityPopUp(content) {
    this.qcVisibilityBookingModel = new QcVisibilityBookingModel();
    this.popUpLoading = true;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.isPopUpOpen = true;
    this.isQcVisiblePopUpSave = true;
    this.selectAllQcPopupChecked = false;

    var bookingIdList = new Array();
    //fetch all the booking Ids of the selected checkboxes
    this.model.items.forEach(element => {
      if (element.isQcVisibleBookingSelected)
        bookingIdList.push(element.bookingId);
    });
    this.qcVisibilityBookingModel.bookingIdlst = bookingIdList;
    this.serviceSCH.getQcVisibilityByBooking(this.qcVisibilityBookingModel)
      .pipe()
      .subscribe(
        res => {
          if (res.result == ResponseResult.Success) {
            this.qcVisiblePopUpData = res.data;

          }
          else {
            if (this.isPopUpOpen)
              this.modelRef.close();
          }

          this.popUpLoading = false;
        },
        error => {
          this.setError(error);
          this.popUpLoading = false;
        });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  saveQcVisibility() {
    this.saveQcVisibleModel = new bookingDataQcVisibleRequest();
    var UpdatedqcVisibility = this.qcVisiblePopUpData.filter(x => x.isQcVisibility == true);
    this.saveQcVisibleModel.bookingDataQcVisible = UpdatedqcVisibility;
    this.popUpLoading = true;
    this.qcVisibleSave = true;
    this.serviceSCH.saveQcVisibility(this.saveQcVisibleModel)
      .pipe()
      .subscribe(
        res => {
          if (res == ResponseResult.Success) {
            if (this.isPopUpOpen)
              this.modelRef.close();
            this.selectAllChecked = false;
            this.selectAllQcVisibleChecked = false;
            this.isBookingForQcVisible = false;
            this.GetSearchData();
            this.showSuccess("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_SAVE_SUCCESS");
          }
          else {
            this.showError("SCHEDULE_SUMMARY.TITLE", `SCHEDULE_SUMMARY.MSG_SAVE_FAIL`);
          }
          this.popUpLoading = false;
          this.qcVisibleSave = false;
        },
        error => {
          this.setError(error);
          this.popUpLoading = false;
          this.qcVisibleSave = false;
          this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_UNKNOWN_ERROR");
        });

  }



  openPopUp(content, item) {
    this.mandaymodel = new MandayModel();
    this.popUpLoading = true;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.isPopUpOpen = true;
    this.serviceSCH.getQuotationManday(item)
      .pipe()
      .subscribe(
        res => {
          if (res.result == QuotationMandayResult.success) {
            this.mandaymodel = res.data;
          }

          else {
            if (this.isPopUpOpen)
              this.modelRef.close();
          }

          this.popUpLoading = false;
        },
        error => {
          this.setError(error);
          this.popUpLoading = false;
        });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }

  isPopupValid() {
    var totalquomanday = this.mandaymodel.mandayList.filter(x => x.bookingId).reduce((sum, x) => x.manDay + sum, 0) + this.mandaymodel.travelManday;
    if (totalquomanday > this.mandaymodel.totalManday) {
      this.showError('SCHEDULE_SUMMARY.TITLE', 'SCHEDULE_SUMMARY.MSG_BOOK_MAN_Day_NOT_MATCH');
      return false
    }
    return true;
  }

  saveManday() {
    if (this.isPopupValid()) {
      this.popUpLoading = true;
      this.mandaySave = true;
      this.serviceSCH.saveManday(this.mandaymodel)
        .pipe()
        .subscribe(
          res => {
            if (res == QuotationMandayResult.success) {
              if (this.isPopUpOpen)
                this.modelRef.close();
              this.showSuccess("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_SAVE_SUCCESS");
            }
            else {
              this.showError("SCHEDULE_SUMMARY.TITLE", `SCHEDULE_SUMMARY.MSG_SAVE_FAIL`);
            }
            this.popUpLoading = false;
            this.mandaySave = false;
          },
          error => {
            this.setError(error);
            this.popUpLoading = false;
            this.mandaySave = false;
            this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_UNKNOWN_ERROR");
          });
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  GetQuotationStatusColor(statusid?) {
    if (this._quotationStatusList != null && this._quotationStatusList.length > 0 && statusid != null) {
      var result = this._quotationStatusList.find(x => x.statusName == statusid);
      if (result)
        return result.statusColor;
    }
  }

  //fetch the Qc names data with virtual scroll
  getQcData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.qcRequest.searchText = this.scheduleFilterRequest.qcInput.getValue();
      this.qcRequest.skip = this.scheduleFilterRequest.qcList.length;
    }

    this.scheduleFilterRequest.qcLoading = true;
    this.serviceSCH.getQcDataSourceList(this.qcRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.scheduleFilterRequest.qcList = this.scheduleFilterRequest.qcList.concat(customerData);
        }
        if (isDefaultLoad)
          this.qcRequest = new QcDataSourceRequest();
        this.scheduleFilterRequest.qcLoading = false;
      }),
      error => {
        this.scheduleFilterRequest.qcLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Qc on load
  getQcListBySearch() {
    this.scheduleFilterRequest.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.scheduleFilterRequest.qcLoading = true),
      switchMap(term => term
        ? this.serviceSCH.getQcDataSourceList(this.qcRequest, term)
        : this.serviceSCH.getQcDataSourceList(this.qcRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.scheduleFilterRequest.qcLoading = false))
      ))
      .subscribe(data => {
        this.scheduleFilterRequest.qcList = data;
        this.scheduleFilterRequest.qcLoading = false;
      });
  }


  //fetch the Zones data with virtual scroll
  getZoneData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.zoneRequest.searchText = this.scheduleZoneFilterRequest.zoneInput.getValue();
      this.zoneRequest.skip = this.scheduleZoneFilterRequest.zoneList.length;
    }

    this.scheduleZoneFilterRequest.zoneLoading = true;
    this.serviceSCH.getZoneDataSourceList(this.zoneRequest).
      subscribe(zoneData => {
        if (zoneData && zoneData.length > 0) {
          this.scheduleZoneFilterRequest.zoneList = this.scheduleZoneFilterRequest.zoneList.concat(zoneData);
        }
        if (isDefaultLoad)
          this.zoneRequest = new CommonZoneSourceRequest();
        this.scheduleZoneFilterRequest.zoneLoading = false;
      }),
      error => {
        this.scheduleZoneFilterRequest.zoneLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Zones on load
  getZoneListBySearch() {
    this.scheduleZoneFilterRequest.zoneInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.scheduleZoneFilterRequest.zoneLoading = true),
      switchMap(term => term
        ? this.serviceSCH.getZoneDataSourceList(this.zoneRequest, term)
        : this.serviceSCH.getZoneDataSourceList(this.zoneRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.scheduleZoneFilterRequest.zoneLoading = false))
      ))
      .subscribe(data => {
        this.scheduleZoneFilterRequest.zoneList = data;
        this.scheduleZoneFilterRequest.zoneLoading = false;
      });
  }

  getServiceTypeList() {
    const serviceId = Service.Inspection;
    this.refService
      .getServiceTypeListByServiceIds([serviceId])
      .subscribe((res) => {
        if (res && res.result == ResponseResult.Success) {
          this.serviceTypeList = res.dataSourceList;
        }
      },
        error => {
          this.setError(error);
        });
  }

  getQuotationStatusColor() {
    this.quotService.getQuotationStatusColor()
      .pipe()
      .subscribe(response => {
        if (response.result == ResponseResult.Success) {
          this._quotationStatusList = response.quotationStatusList;
        }
      },
        error => {
          this._quotationStatusList = [];
        });
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "fromdate": {
        this.model.fromdate = null;
        break;
      }
      case "todate": {
        this.model.todate = null;
        break;
      }
    }
  }
  showPaginationTitleText() {
    let paginationTitleText: string = "";
    let TotalNO: string = "";
    let ManDAY: string = "";
    this.translate.get("TRAVEL_MATRIX.LBL_TOTAL").subscribe((text: string) => { TotalNO = text });
    this.translate.get("SCHEDULE_SUMMARY.LBL_PLANNED_MANDAY").subscribe((text: string) => { ManDAY = text });
    let paginationText = this.showPaginationText();
    paginationTitleText = paginationText + ',' + '&nbsp;' + TotalNO + '&nbsp;' + "" + this.mandayCount + '&nbsp;' + ManDAY;
    return paginationTitleText;

  }
  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    if (bookingId && bookingId > 0) {
      var editPage = entity + "/" + Url.EditBooking + bookingId;
      window.open(editPage);
    }
  }

  openProductData(content, bookingId) {
    this.scheduleProductModel = new ScheduleProductModel();
    this.loader = true;
    this.serviceSCH.getproductdetails(bookingId)
      .pipe()
      .subscribe(
        res => {
          if (res.result == ResponseResult.Success) {
            this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });
            this.scheduleProductModel = res.scheduleProductModel;
          }
          this.loader = false;
        },
        error => {
          this.setError(error);
          this.loader = false;
        });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }

  //get the entity page field access list
  async getEntPageFieldAccessList() {

    //map the ent page field data
    var entPageRequest = this.mapEntPageField();

    var entPageFieldAccessResponse = await this.service.getEntPageAccessList(entPageRequest);

    if (entPageFieldAccessResponse.result == EntPageFieldAccessResult.Success) {
      this.entPageFieldAccess = entPageFieldAccessResponse.entPageFieldAccess;

      this.filterEntFieldAccess = this.entPageFieldAccess;
    }
    else {
      this.entPageFieldAccess = [];
    }
  }

  //map the ent page field data
  mapEntPageField() {
    var entPageRequest = new EntPageRequest();
    entPageRequest.serviceId = Service.Inspection;
    entPageRequest.rightId = RightsEnum.ScheduleSummary;
    return entPageRequest;
  }

  openExportPopup(content) {
    const entityId = Number(this.utility.getEntityId());
    this.model.exportType = entityId === EntityAccess.API ? ScheduleSummaryExportType.QCLevel : ScheduleSummaryExportType.ProductRefLevel;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  RedirectToProductReference(cuProductId) {
    let entity: string = this.utility.getEntityName();
    if (cuProductId && cuProductId > 0) {
      const productReferencePage = entity + "/" + Url.CustomerProduct + cuProductId;
      window.open(productReferencePage);
    }
  }
}
