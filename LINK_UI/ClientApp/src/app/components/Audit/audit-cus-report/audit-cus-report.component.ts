import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuditService } from '../../../_Services/audit/audit.service'
import { OfficeService } from '../../../_Services/office/office.service'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { UserModel } from '../../../_Models/user/user.model'
import { PageSizeCommon, SearchType, DefaultDateType, searchtypelst, datetypelst, UserType, AuditStatus, FileContainerList, Service, RoleEnum } from '../../common/static-data-common'
import {
  auditcusreportrequest, Loadingstatus, AuditCusReportItem, AuditCusReportBookingDetailsResult, AuditStatusColor,
  DataList, AuditServiceTypeResponseResult, AuditStatusResponseResult, _AuditStatus, ServiceType
} from '../../../_Models/Audit/auditcusreportmodel'
import { SummaryComponent } from '../../common/summary.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Validator } from '../../common';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { CustomerSummaryResult, CustomerItem } from "../../../_Models/customer/edit-customer.model";
import { SupplierListResult, SupplierItem } from "../../../_Models/supplier/edit-supplier.model";
import { OfficeResponseResult, Office } from "../../../_Models/office/edit-officemodel";
import { NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ServiceTypeRequest, ServiceTypeResult } from 'src/app/_Models/reference/servicetyperequest.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { Subject, of } from 'rxjs';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CommonCustomerSourceRequest, CountryDataSourceRequest, CustomerCommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
@Component({
  selector: 'app-audit-cus-report',
  templateUrl: './audit-cus-report.component.html',
  styleUrls: ['./audit-cus-report.component.scss'],
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
export class AuditCusReportComponent extends SummaryComponent<auditcusreportrequest> {

  //--------inherited abstract class start----------
  onInit(): void {
    this.validator.setJSON("audit/audit-cus-report.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.Initialize();
  }
  getData(): void {
    this.SearchReport();
  }
  getPathDetails(): string {
    throw new Error("Method not implemented.");
  }

  //--------inherited abstract class End----------

  //---------declare variable start --------------
  public currentUser: UserModel;
  public _IsInternalUser: boolean = false;
  public model: auditcusreportrequest;
  public selectedPageSize: number;
  public loadingmodel: Loadingstatus;
  public masterdate: DataList;
  public _searchtypelst: any = searchtypelst;
  public _datetypelst: any = datetypelst;
  public pagesizeitems = PageSizeCommon;
  public _statuscolorlst: Array<AuditStatusColor> = [];
  public _customvalidationforbookid: boolean = false;
  public toggleFormSection: boolean = false;
  isFilterOpen: boolean;
  roleEnum = RoleEnum;
  componentDestroyed$: Subject<boolean> = new Subject();
  _isQCUser = false;
  _isReportCheckUser = false;
  _isOtherUser = false;
  _auditStatus = AuditStatus;
  //---------declare variable end --------------


  constructor(private auditservice: AuditService, authserve: AuthenticationService, public validator: Validator, router: Router, translate: TranslateService,
    public accService: UserAccountService,
    route: ActivatedRoute, public customerservice: CustomerService, public officeservice: OfficeService, public supplierservice: SupplierService,
    public calendar: NgbCalendar, public dateparser: NgbDateParserFormatter, public fileStoreService: FileStoreService, public utility: UtilityService,
    public referenceService: ReferenceService, public locationService: LocationService) {

    //--- dervied class contain super class start---
    super(router, validator, route, translate);
    //  --- dervied class contain super class end---

    //---set the current user start---
    this.currentUser = authserve.getCurrentUser();
    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this.isFilterOpen = true;
    //---set the current user end---

  }

  Initialize() {
    this.model = new auditcusreportrequest();
    this.loadingmodel = new Loadingstatus();
    this.masterdate = new DataList();
    this.selectedPageSize = PageSizeCommon[0];
    this.model.searchtypeid = SearchType.BookingNo;
    this.model.datetypeid = DefaultDateType.ServiceDate;

    //--load initial data start
    this.GetCustomerlist();
    this.GetOfficelist();
    this.GetAuditStatuslist();
    this.getServiceTypelist();
    this.getCountryListBySearch();

    if (this.currentUser.roles.filter(x => x.id == this.roleEnum.Inspector).length > 0) {
      this._isQCUser = true;
    }
    else if (this.currentUser.roles.filter(x => x.id == this.roleEnum.ReportChecker).length > 0) //11 - report checker profile
    {
      this._isQCUser = false;
      this._isReportCheckUser = true;
    }
    else {
      this._isQCUser = false;
      this._isReportCheckUser = false;
      this._isOtherUser = true;
      this.model.fromdate = null;
      this.model.todate = null;
    }
    //-- load initial data end
  }

  //--get customer list start
  GetCustomerlist() {

    this.loadingmodel.cusloading = true;
    const customerDataSourceRequest = new CustomerCommonDataSourceRequest();
    customerDataSourceRequest.isVirtualScroll = false;
    this.customerservice.getCustomerListByUserType(customerDataSourceRequest)
      .subscribe(
        response => {
          if (response) {
            this.masterdate.customerlist = response;
          }
          this.loadingmodel.cusloading = false;
        },
        error => {
          this.loadingmodel.cusloading = false;
        }
      );
  }
  //--get customer list End

  //--get office list start
  GetOfficelist() {

    this.loadingmodel.officeloading = true;
    this.officeservice.getOfficeforInternalUser()
      .subscribe(
        response => {
          if (response && response.result == OfficeResponseResult.success) {
            this.masterdate.officelist = response.offices;
          }
          this.loadingmodel.officeloading = false;
        },
        error => {
          this.loadingmodel.officeloading = false;
        }
      );
  }
  //--get office list End

  //--get status list start
  GetAuditStatuslist() {

    this.loadingmodel.statusloading = true;
    this.auditservice.GetAuditStatus()
      .subscribe(
        response => {
          if (response && response.result == AuditStatusResponseResult.success) {
            this.masterdate.statuslist = response.auditstatuslist;
          }
          this.loadingmodel.statusloading = false;
        },
        error => {
          this.loadingmodel.statusloading = false;
        }
      );
  }
  //--get status list End

  //--get supplier list by cus id start
  GetSupplierlistByCustomerId(cusid) {

    //clear the customer related data
    this.masterdate.supplierlist = new Array<SupplierItem>();
    this.model.supplierid = null;

    this.loadingmodel.supplierloading = true;

    this.auditservice.Getsupplierbycusid(cusid, true)
      .subscribe(
        response => {
          if (response && response.result == SupplierListResult.Success) {
            this.masterdate.supplierlist = response.data;
          }
          this.loadingmodel.supplierloading = false;
        },
        error => {
          this.loadingmodel.supplierloading = false;
        }
      );
  }
  //--get supplier list by cus id  End

  //--get factory list by cus id, sup id start
  GetfactorylistByCustomerIdSupplierid(cusid, supplierid) {

    //clear the supplier related data
    this.masterdate.factorylist = [];
    this.model.factoryidlst = [];

    this.loadingmodel.factloading = true;

    this.supplierservice.GetfactoryBycustomeridsupId(cusid, supplierid)
      .subscribe(
        response => {
          if (response && response.result == SupplierListResult.Success) {
            this.masterdate.factorylist = response.data;
          }
          this.loadingmodel.factloading = false;
        },
        error => {
          this.loadingmodel.factloading = false;
        }
      );
  }
  //--get factory list by cus id, sup id  End


  //--get service type start
  getServiceTypelist() {

    //clear the customer related data
    this.masterdate.auditservicetypelist = [];
    this.model.serviceTypelst = [];

    let request = this.generateServiceTypeRequest();
    this.loadingmodel.auditservicetypeloading = true;

    this.referenceService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ServiceTypeResult.Success) {
            this.masterdate.auditservicetypelist = response.serviceTypeList;
          }
          else {
            this.masterdate.auditservicetypelist = [];
          }
          this.loadingmodel.auditservicetypeloading = false;
        },
        error => {
          this.setError(error);
          this.masterdate.auditservicetypelist = [];
          this.loadingmodel.auditservicetypeloading = false;
        }
      );
  }
  //--get service type  End

  generateServiceTypeRequest() {
    const serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerid ?? 0;
    serviceTypeRequest.serviceId = Service.Audit;
    return serviceTypeRequest;
  }

  clearCustomer() {
    this.model.serviceTypelst = [];
    this.getServiceTypelist();
  }

  //---validation for date range
  IsDateValidationRequired(): boolean {
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }

  // --- set search type in model
  SetSearchTypemodel(searchtype) {
    this.model.searchtypeid = searchtype;
  }
  // ---set date type in model
  SetSearchDatetype(searchdatetype) {
    this.model.datetypeid = searchdatetype;
  }

  // customer on change event
  SetCusonchange(event) {
    this.clearcustomerrelateddata();

    if (event != null) {
      this.GetSupplierlistByCustomerId(event.id);
      this.getServiceTypelist();
    }
  }

  //supplier on change event
  setSupplieronchange(event) {

    this.masterdate.factorylist = [];
    this.model.factoryidlst = [];

    if (event != null) {
      this.GetfactorylistByCustomerIdSupplierid(this.model.customerid, event.id);
    }
  }

  //clear customer related date
  clearcustomerrelateddata() {
    this.masterdate.supplierlist = [];
    this.model.supplierid = null;
    this.masterdate.factorylist = [];
    this.model.factoryidlst = [];
    this.masterdate.auditservicetypelist = [];
    this.model.serviceTypelst = [];
  }

  //search data
  SearchReport() {
    this.loadingmodel.searchloading = true;
    this.auditservice.SearchAuditcusReport(this.model)
      .subscribe(
        response => {
          if (response && response.result == AuditCusReportBookingDetailsResult.Success) {
            this.mapPageProperties(response);
            this._statuscolorlst = response.auditStatuslst;
            this.model.items = response.data.map((x) => {
              var item: AuditCusReportItem = {
                auditId: x.auditId,
                customer: x.customer,
                supplier: x.supplier,
                factory: x.factory,
                serviceDate: x.serviceDate,
                serviceType: x.serviceType,
                reportNo: x.reportNo,
                officeName: x.officeName,
                statusId: x.statusId,
                reportid: x.reportid,
                mimeType: x.mimeType,
                pathextension: x.pathextension,
                customerBookingNo: x.customerBookingNo,
                reportUrl: x.reportUrl,
                fbReportUrl: x.fbReportUrl,
                reportFileUniqueId: x.reportFileUniqueId,
                reportFileName: x.reportFileName,
                factoryCountry: x.factoryCountry,
                fbreportid: x.fbreportid
              }
              return item;
            });
          }
          else if (response && response.result == AuditCusReportBookingDetailsResult.NotFound) {
            this.model.noFound = true;
            this._statuscolorlst = [];
          }
          else {
            this.error = response.result;
            this._statuscolorlst = [];
          }
          this.loadingmodel.searchloading = false;
        },
        error => {
          this.setError(error);
          this.loadingmodel.searchloading = false;
        });
  }

  //search by status
  SearchByStatus(id) {
    this.model.statusidlst = [];
    this.model.statusidlst.push(id);
    this.SearchReport();
  }
  //on click event of status tile
  GetStatusColor(statusid?) {
    if (this._statuscolorlst != null && this._statuscolorlst.length > 0 && statusid != null) {
      var result = this._statuscolorlst.find(x => x.id == statusid);
      if (result)
        return result.statusColor;
    }
  }

  //search the details, set page size.
  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.pageSize = this.selectedPageSize;
      this.search();
    }
  }
  //reset the data
  Reset() {
    this.Initialize();
  }
  //download the report
  downloadreport(audititem: AuditCusReportItem) {
    if (audititem == null)
      this.return;
    if (audititem.reportUrl) { //manual report + with cloud
      this.fileStoreService.downloadBlobFile(audititem.reportFileUniqueId, FileContainerList.Audit)
        .subscribe(res => {
          this.downloadFile(res, audititem.mimeType, audititem.pathextension, audititem.reportFileName);
          this.loadingmodel.downloadreport = false;
        },
          error => {
            this.loadingmodel.downloadreport = false;
            this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          });
      this.loadingmodel.downloadreport = false;
    }
    else if (audititem.fbReportUrl) { //fb report
      window.open(audititem.fbReportUrl);
    }
    else { //manual report + with db
      this.loadingmodel.downloadreport = true;
      this.auditservice.getAuditReportFiles(audititem.reportid)
        .subscribe(res => {
          this.downloadFile(res, audititem.mimeType, audititem.pathextension, audititem.reportFileName);
          this.loadingmodel.downloadreport = false;
        },
          error => {
            this.loadingmodel.downloadreport = false;
            this.showError('AUDIT_REPORT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          });
    }
  }
  downloadFile(data, mimetype, pathextension, filename) {
    const blob = new Blob([data], { type: mimetype });
    filename = filename ? filename : "Audit_Report" + pathextension;
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, filename);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }


  //validate & set booking no
  BookingNoValidation() {
    return this._customvalidationforbookid = this.model.searchtypeid == SearchType.BookingNo
      && this.model.searchtypetext != null && this.model.searchtypetext.trim() != "" && isNaN(Number(this.model.searchtypetext));
  }

  //validate the form
  formValid(): boolean {
    return !this.BookingNoValidation() && this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired())
  }
  //toogle for advance search
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
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

  //on change country event
  onChangeCountry(event) {
    if (!event) {
      this.masterdate.countryRequest.countryIds = [];
      this.masterdate.countryList = [];

      this.model.factoryCountryIdList = [];

      this.getCountryListBySearch();
    }
  }

  //fetch the country data with virtual scroll
  getCountryData() {

    this.masterdate.countryRequest.searchText = this.masterdate.countryInput.getValue();
    this.masterdate.countryRequest.skip = this.masterdate.countryList.length;

    this.loadingmodel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterdate.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterdate.countryList = this.masterdate.countryList.concat(customerData);
        }

        this.masterdate.countryRequest = new CountryDataSourceRequest();
        this.loadingmodel.countryLoading = false;
      }),
      error => {
        this.loadingmodel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    if (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0)
      this.masterdate.countryRequest.countryIds = this.model.factoryCountryIdList;

    this.masterdate.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.loadingmodel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterdate.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.masterdate.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.loadingmodel.countryLoading = false))
      ))
      .subscribe(data => {
        this.masterdate.countryList = data;
        this.loadingmodel.countryLoading = false;
      });
  }

  RedirectToFbReport(reportId) {
    this.accService.getUserTokenToFB()
      .subscribe(
        response => {
          if (response != null) {
            window.open(response.reportUrl + reportId + "?token=" + response.token + "", "_blank");
          }
        }
      );
  }

}
