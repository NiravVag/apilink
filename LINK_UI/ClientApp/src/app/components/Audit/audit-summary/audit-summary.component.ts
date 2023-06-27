import { Component, OnInit } from '@angular/core';
import { AuditService } from '../../../_Services/audit/audit.service'
import { TranslateService } from '@ngx-translate/core';
import { Auditsummarymodel, AuditItem, AuditMasterData } from '../../../_Models/Audit/auditsummarymodel'
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, takeUntil, tap } from 'rxjs/operators';
import { PageSizeCommon, SearchType, DefaultDateType, searchtypelst, datetypelst, UserType, AuditSearchRedirectPage, AuditStatus, Service } from '../../common/static-data-common'
import { Validator } from "../../common/validator"
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../../../_Models/user/user.model'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { LocationService } from 'src/app/_Services/location/location.service';
import { of, Subject } from 'rxjs';
import { CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ServiceTypeRequest, ServiceTypeResult } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
@Component({
  selector: 'app-audit-summary',
  templateUrl: './audit-summary.component.html',
  styleUrls: ['./audit-summary.component.css'],
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

export class AuditSummaryComponent extends SummaryComponent<Auditsummarymodel> {
  public model: Auditsummarymodel;
  public data: any;
  public searchloading: boolean = false;
  public error: string = "";
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  searchtypelst: any = searchtypelst;
  datetypelst: any = datetypelst;
  Initialloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  public exportDataLoading = false;
  public _customvalidationforbookid: boolean = false;
  _booksearttypeid = SearchType.BookingNo;
  public _statuslist: any[] = [];
  currentUser: UserModel;
  public _IsInternalUser: boolean = false;
  _auditredirectpage = AuditSearchRedirectPage;
  _redirectpath: string;
  private currentRoute: Router;
  public _redirecttype: any;
  _AuditStautus = AuditStatus;
  _searchauditid: any
  isFilterOpen: boolean;
  toggleFormSection: boolean;
  _Auditorlist = [];
  componentDestroyed$: Subject<boolean> = new Subject();
  countryRequest: CountryDataSourceRequest;
  auditMasterData: AuditMasterData;
  _ServiceTypeLoading: boolean = false;
  _ServiceTypeList = [];

  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return this._redirectpath;
  }

  constructor(private service: AuditService, private bookingservice: BookingService, public validator: Validator, router: Router,
    route: ActivatedRoute, authserve: AuthenticationService, public utility: UtilityService, public calendar: NgbCalendar, public dateparser: NgbDateParserFormatter, translate: TranslateService, public pathroute: ActivatedRoute,
    public locationService: LocationService, public referenceService: ReferenceService) {
    super(router, validator, route, translate);
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.currentRoute = router;
    this.isFilterOpen = true;
  }
  onInit(): void {
    this._searchauditid = this.pathroute.snapshot.paramMap.get("id");
    this.Initialize();
    this.validator.setJSON("audit/audit-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
  }
  Initialize() {
    this.model = new Auditsummarymodel();
    this.auditMasterData = new AuditMasterData();
    this.countryRequest = new CountryDataSourceRequest();
    this.validator.isSubmitted = false;
    this.Initialloading = true;
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;
    this.data = [];
    this._statuslist = [];
    this.GetAuditor();
    this.getServiceTypelist();
    this.service.Getauditsummary()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (this.data && this.data.customerList && this.data.customerList.length > 0 && this.currentUser.usertype == UserType.Customer )
              this.model.customerid = this.data.customerList[0].id;
            if (this._searchauditid) {
              this.model.searchtypetext = this._searchauditid;
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
  ngAfterViewInit() {
    this.getCountryListBySearch();
  }
  changeCustomer(cusitem) {
    if (cusitem != null && cusitem.id != null) {
      this.getSupplierlist();
    }
    else {
      this.data.factoryList = [];
      this.model.factoryidlst = [];
      this.data.supplierList = [];
      this.model.supplierid = null;
    }
    this.getServiceTypelist();
  }
  GetSupplierlistSuccessResponse(response) {
    if (response && response.result == 1) {
      this.data.supplierList = response.data;
    }
    else {
      this.data.supplierList = [];
    }
    this.model.supplierid = null;
    this.data.factoryList = [];
    this.model.factoryidlst = [];
    this.suploading = false;
  }
  GetFactoryList(supitem, customerid) {
    if (supitem != null && supitem.id != null) {
      this.factloading = true;
      this.bookingservice.GetFactoryDetailsByCusSupId(supitem.id, customerid)
        .subscribe(
          response => {
            this.GetFactoryListSuccessResponse(response);
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
  GetFactoryListSuccessResponse(response) {
    if (response && response.result == 1) {
      this.data.factoryList = response.data;
    }
    else {
      this.data.factoryList = [];
    }
    this.model.factoryidlst = [];
    this.factloading = false;
  }
  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == this._booksearttypeid
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != ""
      && isNaN(Number(this.model.searchtypetext));
  }
  Reset() {
    this.Initialize();
    this.getCountryListBySearch();
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
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }
  formValid(): boolean {
    return !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
  }
  GetSearchData() {
    this.searchloading = true;
    this.service.SearchAuditSummary(this.model)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.mapPageProperties(response);
            this._statuslist = response.auditStatuslst;

            this.model.items = response.data.map((x) => {
              var item: AuditItem = {
                auditId: x.auditId,
                customerName: x.customerName,
                supplierName: x.supplierName,
                factoryName: x.factoryName,
                serviceDateFrom: x.serviceDateFrom,
                serviceDateTo: x.serviceDateTo,
                serviceType: x.serviceType,
                poNumber: x.poNumber,
                reportNo: x.reportNo,
                office: x.office,
                statusId: x.statusId,
                bookingCreatedBy: x.bookingCreatedBy,
                quotationStatus: x.quotationStatus,
                customerBookingNo: x.customerBookingNo
              }
              return item;
            }
            );
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
    this.service.exportSummary(this.model)
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
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
    this.exportDataLoading = false;
  }
  RedirectPage(type, auditid) {
    switch (type) {
      case this._auditredirectpage.Edit:
        {
          this._redirectpath = "auditedit/edit-audit";
          super.getDetails(auditid);
          break;
        }
      case this._auditredirectpage.Cancel:
        {
          this._redirectpath = "auditcancel/cancel-audit";
          this._redirecttype = this._auditredirectpage.Cancel;
          this.getDetails(auditid);
          break;
        }
      case this._auditredirectpage.Reschedule:
        {
          this._redirectpath = "auditcancel/cancel-audit";
          this._redirecttype = this._auditredirectpage.Reschedule;
          this.getDetails(auditid);
          break;
        }
      case this._auditredirectpage.Report:
        {
          this._redirectpath = "auditreport/audit-report";
          super.getDetails(auditid);
          break;
        }
      case this._auditredirectpage.Schedule_Auditor:
        {
          this._redirectpath = "auditedit/edit-audit";
          this._redirecttype = this._auditredirectpage.Schedule_Auditor;
          this.getDetails(auditid);
          break;
        }
    }
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
  IsRescheduleVisible(statusid, serviceDateTo, bookingcreatedby) {
    var date = this.dateparser.parse(serviceDateTo);
    var ngbdatestruct = new NgbDate(date.year, date.month, date.day);
    if (statusid == this._AuditStautus.Cancel)
      return false;
    else if (this._IsInternalUser && statusid == this._AuditStautus.Audited)
      return false;
    else if (this._IsInternalUser && ngbdatestruct.after(this.calendar.getToday()))
      return true;
    else if (statusid == this._AuditStautus.Cancel || ngbdatestruct.before(this.calendar.getToday())
      || bookingcreatedby != this.currentUser.usertype)
      return false;
    else
      return true;

    //return statusid == this._AuditStautus.Cancel ? true : false;
  }
  IsCancelVisible(statusid, serviceDateTo, bookingcreatedby) {
    var date = this.dateparser.parse(serviceDateTo);
    var ngbdatestruct = new NgbDate(date.year, date.month, date.day);

    if (this._IsInternalUser)
      return true;
    else if (statusid == this._AuditStautus.Cancel || ngbdatestruct.before(this.calendar.getToday())
      || bookingcreatedby != this.currentUser.usertype)
      return false;
    else
      return true;
  }
  IsReportVisible(isheader, statusid) {
    if (this._IsInternalUser && isheader)
      return true;
    else if (this._IsInternalUser) {
      if (statusid == this._AuditStautus.Scheduled || statusid == this._AuditStautus.Audited)
        return true;
      else
        return false;
    }
    else
      return false;
  }
  IsScheduleAuditorVisible(statusid) {
    if (this._IsInternalUser) {
      if (statusid != this._AuditStautus.Cancel && statusid != this._AuditStautus.Audited)
        return true;
      else
        return false;
    }
    else
      return false;
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


  //fetch the country data with virtual scroll
  getCountryData() {

    this.countryRequest.searchText = this.auditMasterData.countryInput.getValue();
    this.countryRequest.skip = this.auditMasterData.countryList.length;

    this.auditMasterData.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.auditMasterData.countryList = this.auditMasterData.countryList.concat(customerData);
        }

        this.countryRequest = new CountryDataSourceRequest();
        this.auditMasterData.countryLoading = false;
      }),
      error => {
        this.auditMasterData.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    if (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0)
      this.countryRequest.countryIds = this.model.factoryCountryIdList;

    this.auditMasterData.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.auditMasterData.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.auditMasterData.countryLoading = false))
      ))
      .subscribe(data => {
        this.auditMasterData.countryList = data;
        this.auditMasterData.countryLoading = false;
      });
  }

  //on change country event
  onChangeCountry(event) {
    if (!event) {
      this.countryRequest.countryIds = [];
      this.auditMasterData.countryList = [];

      this.model.factoryCountryIdList = [];

      this.getCountryListBySearch();
    }
  }

  //get auditor list
  GetAuditor() {
    this.service.GetAuditor()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this._Auditorlist = response.auditors;
          }
          else {
            this._Auditorlist = [];
          }
        },
        error => {
          this.setError(error);
        });
  }

  getSupplierlist() {
    const customerId = this.model.customerid ?? 0;
    this.suploading = true;
    this.service.Getsupplierbycusid(customerId, true)
      .subscribe(
        response => {
          this.GetSupplierlistSuccessResponse(response);
        },
        error => {
          this.setError(error);
          this.suploading = false;
        }
      );
  }

  getServiceTypelist() {
    let request = this.generateServiceTypeRequest();
    this.model.serviceTypeIdList = [];
    this._ServiceTypeLoading = true;

    this.referenceService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ServiceTypeResult.Success) {
            this._ServiceTypeList = response.serviceTypeList;
          }
          else {
            this._ServiceTypeList = [];
          }
          this._ServiceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this._ServiceTypeList = [];
          this._ServiceTypeLoading = false;
        }
      );
  }

  generateServiceTypeRequest() {
    const serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerid ?? 0;
    serviceTypeRequest.serviceId = Service.Audit;
    return serviceTypeRequest;
  }
}
