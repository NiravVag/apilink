


import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, Inject } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { CommonKpiTemplate, Country, Service, CustomerMandayGroupByFilter, CustomerMandayKPITemplate, ARFollowUpReportKPITemplate, KpiInvoicePaymentStatuslst, XeroKPIInvoiceTemplate, UserType } from '../../../components/common/static-data-common'
import { TranslateService } from '@ngx-translate/core';
import { CustomKpiService } from '../../../_Services/customKpi/customKpi.service';
import { CustomKpiLists, CustomkpiModel } from '../../../_Models/kpi/customkpimodel';
import { LocationService } from 'src/app/_Services/location/location.service';
import { SummaryComponent } from '../../common/summary.component';
import { NgbCalendar, NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ResponseResult, CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { of, Subject } from 'rxjs';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { KpiTeamplateRequest } from 'src/app/_Models/kpi/kpi-teamplate-model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UserModel } from 'src/app/_Models/user/user.model';

@Component({
  selector: 'app-custom-kpi',
  templateUrl: './custom-kpi.component.html',
  styleUrls: ['./custom-kpi.component.scss']
})
export class CustomKpiComponent extends SummaryComponent<CustomkpiModel>{
  exportDataLoading: boolean;
  invoiceTypeLoading: boolean;
  invoiceTypeList: any;
  isShowInvoiceTypeddl: boolean;
  isShowPaymentStatusddl: boolean;
  currentUser: UserModel;
  _IsCustomerUser: boolean;
  getData(): void {

  }
  getPathDetails(): string {
    return 'customkpi/custom-kpi';
  }

  public componentDestroyed$: Subject<boolean> = new Subject();
  public model: CustomkpiModel;
  private currentRoute: Router;
  public customerList: any;
  public templateList: any;
  public loading: boolean = false;
  public isTemplateAsCommon = false;
  public completeTemplateList: any;
  public officeList: any;
  public summaryModel: CustomKpiLists;
  toggleFormSection: boolean;
  customerId: number;
  customerMandayGroupByFilters = CustomerMandayGroupByFilter;
  customerMandayKPITemplate = CustomerMandayKPITemplate;
  kpiInvoicePaymentStatuslst: any = KpiInvoicePaymentStatuslst;
  constructor(
    private service: CustomKpiService, public validator: Validator, router: Router,
    route: ActivatedRoute, public calendar: NgbCalendar, public dateparser: NgbDateParserFormatter,
    private locationService: LocationService,
    public pathroute: ActivatedRoute, translate: TranslateService, private refService: ReferenceService,
    public utility: UtilityService, private customerService: CustomerService, authserve: AuthenticationService,
    toastr: ToastrService, private customerDepartmentService: CustomerDepartmentService, private customerBrandService: CustomerBrandService) {
    super(router, validator, route, translate, toastr);
    this.currentRoute = router;
    this.validator.setJSON("customkpi/customkpi.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.currentUser = authserve.getCurrentUser();
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
  }

  onInit(): void {
    this.Initialize();
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  Initialize() {

    this.model = new CustomkpiModel();
    this.summaryModel = new CustomKpiLists();
    if (this._IsCustomerUser && !this.model.customerId) {
      this.model.customerId = this.currentUser.customerid;
      this.getCustomerBrandListList();
      this.getCustomerDepartmentList();
    }
    this.getServiceTypeList();
    this.getInvoiceTypeList();
    this.getTemplateList();
    this.loading = true;
    this.getCountryListBySearch();
    this.service.getKpiSummary()
      //this.bservice.Getbookingsummary()
      .pipe()
      .subscribe(
        res => {
          if (res) {
            this.loading = false;
            this.customerList = res.customerList;
            this.officeList = res.officeList;
          }
          else {
          }
        },
        error => {

        });
  }

  getServiceTypeList() {
    this.model.serviceTypeIdLst = [];
    this.summaryModel.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();

    this.refService.getServiceTypes(request)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {

          this.summaryModel.serviceTypeList = response.serviceTypeList;
          this.summaryModel.serviceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.summaryModel.serviceTypeList = [];
          this.summaryModel.serviceTypeLoading = false;
        }
      );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerId ?? 0;
    // serviceTypeRequest.serviceId = Service.Inspection;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  export() {
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.exportDataLoading = true;
      this.service.exportSummary(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        },
          error => {
            if(error.status==400)
            {
              this.exportDataLoading = false;
              this.showError("KPI.LBL_TITLE", "No Invoice Access to Export the Data");
            }
            else{
              this.exportDataLoading = false;
              this.showError("KPI.LBL_TITLE", "KPI.MSG_DATA_NOTFOUND");
            }


          });
    }
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    var template = this.templateList.filter(x => x.id == this.model.templateId);
    var templateName = template[0].name;
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, templateName + ".xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = templateName + ".xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  IsDateValidationRequired(): boolean {
    let isOk = this.validator.isSubmitted ? true : false;

    if (!this.model.fromdate) {
      this.validator.isValid('fromdate');
    }

    else if (this.model.fromdate && !this.model.todate) {
      this.validator.isValid('todate');
    }

    return isOk;
  }
  //toggle advance search section
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  formValid() {

    var isValid = this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
      this.validator.isValidIf('fromdate', this.IsDateValidationRequired()) &&
      this.validator.isValid('templateId') && this.ValidateDate();

    if (isValid && !this.isTemplateAsCommon) {
      isValid = this.validator.isValid('customerId');
    }

    if (this.model.templateId == this.customerMandayKPITemplate) {
      if (!this.model.customerMandayGroupByFields || this.model.customerMandayGroupByFields.length == 0) {
        this.showError("KPI.LBL_TITLE", "KPI.MSG_GROUP_BY_REQ");
        isValid = false;
      }
    }
    return isValid;
  }
  ValidateDate() {
    if (this.model.fromdate != null && this.model.todate != null) {
      var _fromdate = this.model.fromdate;
      var fromDate = new NgbDate(_fromdate.year, _fromdate.month, _fromdate.day);

      var _todate = this.model.todate;
      var toDate = new NgbDate(_todate.year, _todate.month, _todate.day);

      var leadtime: NgbDate = this.calendar.getNext(fromDate, 'y', 1);

      if (toDate.after(leadtime)) {
        this.showError("KPI.LBL_TITLE", "KPI.MSG_DATE_VALIDATION");
        return false;
      }
      return true;
    }
    else {
      return false;
    }

  }
  FilterTemplate(item) {
    this.model.templateId = null;
    this.customerId = item;
    this.getTemplateList();
    this.getCustomerBrandListList();
    this.getCustomerDepartmentList();
    this.getServiceTypeList();
  }

  ClearTemplate() {
    this.model.templateId = null;
    this.templateList = this.completeTemplateList;
    this.templateList = this.templateList.filter(x => x.customerId == null);
    this.getServiceTypeList();
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

  //get department list
  getCustomerDepartmentList() {
    this.summaryModel.departmentLoading = true;
    if (this.model.customerId) {
      this.customerDepartmentService.getCustomerDepartment(this.model.customerId)
        .pipe(first())
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              this.summaryModel.departmentList = response.customerDepartmentList;
            this.summaryModel.departmentLoading = false;

          },
          error => {
            this.setError(error);
            this.summaryModel.departmentLoading = false;
          });
    }
    this.summaryModel.departmentList = [];
    this.summaryModel.departmentLoading = false;
  }

  //get brand list
  getCustomerBrandListList() {
    this.summaryModel.brandLoading = true;
    if (this.model.customerId) {
      this.customerBrandService.getEditCustomerBrand(this.model.customerId)
        .pipe(first())
        .subscribe(
          response => {
            if (response)
              this.summaryModel.brandList = response.customerBrands;
            this.summaryModel.brandLoading = false;

          },
          error => {
            this.setError(error);
            this.summaryModel.brandLoading = false;
          });
    }
    this.summaryModel.brandList = [];
    this.summaryModel.brandLoading = false;
  }

  getTemplateList(): any {
    const request = new KpiTeamplateRequest();
    request.customerId = this.customerId;
    this.loading = true;

    this.service.getKpiTeamplateSummary(request)
      .pipe()
      .subscribe(
        res => {
          this.loading = false;
          this.templateList = res;



        },
        error => {
          this.loading = false;
          console.log(error);
        });
  }

  changeKpiTemplate(templateId) {
    this.isTemplateAsCommon = false;
    if (templateId) {
      this.isTemplateAsCommon = CommonKpiTemplate.filter(x => x == templateId).length > 0;
    }

    if (templateId == ARFollowUpReportKPITemplate) {
      this.isShowInvoiceTypeddl = true;
      this.isShowPaymentStatusddl = true;
    }
    else if (templateId == XeroKPIInvoiceTemplate) {
      this.isShowInvoiceTypeddl = true;
      this.isShowPaymentStatusddl = false;
    }
    else {
      this.isShowInvoiceTypeddl = false;
      this.isShowPaymentStatusddl = false;
    }
    this.model.invoiceTypeIdList = [];
    this.model.paymentStatusIdList = [];
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.summaryModel.countryRequest.searchText = this.summaryModel.countryInput.getValue();
    this.summaryModel.countryRequest.skip = this.summaryModel.countryList.length;

    this.summaryModel.countryLoading = true;

    this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.countryList = this.summaryModel.countryList.concat(customerData);
        }
        this.summaryModel.countryRequest = new CountryDataSourceRequest();
        this.summaryModel.countryLoading = false;
      }),
      (error: any) => {
        this.summaryModel.countryLoading = false;
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.summaryModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.countryList = data;
        this.summaryModel.countryLoading = false;
      });
  }

  getInvoiceTypeList() {
    this.invoiceTypeLoading = true;
    this.customerService.getInvoiceType()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.invoiceTypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }
          this.invoiceTypeLoading = false;
        },
        error => {
          this.invoiceTypeLoading = false;
          this.setError(error);
        });
  }
}

