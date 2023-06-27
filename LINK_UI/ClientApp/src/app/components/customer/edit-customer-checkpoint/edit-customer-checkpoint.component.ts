import { Component } from '@angular/core';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from "ngx-toastr";
import { Validator } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { EditCustomerCheckPointModel, CustomerCheckPointSummaryModel, CustomerCheckPointModel, CheckPointTypeEnum } from '../../../_Models/customer/customer-checkpoint.model'
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerServiceConfig } from 'src/app/_Services/customer/customerserviceconfig.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { of, Subject } from 'rxjs';
import { CountryDataSourceRequest } from 'src/app/_Models/common/common.model';
@Component({
  selector: 'app-edit-customer-checkpoint',
  templateUrl: './edit-customer-checkpoint.component.html',
  styleUrls: ['./edit-customer-checkpoint.component.scss']
})
export class EditCustomerCheckpointComponent extends DetailComponent {
  private modelRef: NgbModalRef;
  modelAdd: EditCustomerCheckPointModel;
  modelsummary: CustomerCheckPointSummaryModel;
  componentDestroyed$: Subject<boolean> = new Subject();
  countryRequest: CountryDataSourceRequest;
  checkPointTypeEnum = CheckPointTypeEnum;
  constructor(private service: CustomerCheckPointService, private cusService: CustomerService, private cusConfigService: CustomerServiceConfig, public modalService: NgbModal,
    router: Router, public validator: Validator, translate: TranslateService,public utility: UtilityService,public locationService: LocationService,
    toastr: ToastrService, route: ActivatedRoute) {
    super(router, route, translate, toastr);
    this.modelsummary = new CustomerCheckPointSummaryModel();
    this.modelAdd = new EditCustomerCheckPointModel();
    this.countryRequest = new CountryDataSourceRequest();
    this.validator.setJSON("customer/customer-checkpoint.valid.json");
    this.validator.setModelAsync(() => this.modelAdd);
    this.validator.isSubmitted = false;
  }
  onInit(id?: number) {
    this.Intitialize(id);
  }
  getCustomerData(id?: number) {
    this.service.getCustomer()
      .pipe()
      .subscribe(
        response => {
          this.getCustomerResponse(response, id);
        },
        error => {
          this.setError(error);
          this.modelsummary.customerLoading = false;
        });
  }
  getCheckPointData() {
    this.service.getCheckPoint()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1)
            this.modelsummary.checkPointList = response.checkPointList;
          else
            this.error = response.result;
          this.modelsummary.customerCPLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.customerCPLoading = false;
        });
  }
  getServiceData() {
    this.service.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1)
            this.modelsummary.serviceList = response.serviceList;
          else
            this.error = response.result;
          this.modelsummary.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.serviceLoading = false;
        });
  }
  getData(cusId?: number, serviceId?: number) {
    this.modelsummary.items = [];
    //get data only the customer id exists 
    if (cusId > 0) {
      this.loading = true;
      serviceId = serviceId > 0 ? serviceId : this.modelAdd.serviceId > 0 ? this.modelAdd.serviceId : 0;
      let service = cusId > 0 || serviceId > 0 ?
        this.service.getCustomerCheckPointSummary(cusId, serviceId) : this.service.getCustomerCheckPointSummary(0, 0);
      service.pipe()
        .subscribe(
          response => {
            this.summaryResponse(response);
          },
          error => {
            this.setError(error);
            this.loading = false;
          });
    }
  }
  Formvalid(): boolean {
    return this.validator.isValid('customerId')
      && this.validator.isValid('serviceId')
      && this.validator.isValid('checkPointId');
  }
  refresh() {
    this.modelAdd = new EditCustomerCheckPointModel();
    this.modelAdd.customerId = this.modelsummary.customerIdParam > 0 ? Number(this.modelsummary.customerIdParam) : null;
    this.modelsummary.isNewItem = true;
    this.modelsummary.customerCPLoading = true;
    this.modelsummary.serviceLoading = true;
    this.getServiceData();
    this.getCheckPointData();
    this.getData(this.modelAdd.customerId);
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.modelsummary.saveLoading = true;
      this.service.saveCustomerCheckPoint(this.modelAdd)
        .subscribe(
          response => {
            this.saveResponse(response);
          }, error => {
            this.showError("CUSTOMER_CHECK_POINTS.TITLE", 'CUSTOMER_CHECK_POINTS.MSG_UNKNONW_ERROR');
            this.modelsummary.saveLoading = false;
          });
    }
  }
  openConfirm(item, content) {
    this.modelsummary.deleteId = item.id;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
  }
  delete(id) {
    this.loading = true;
    this.service.deleteCustomerCheckPoint(id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1))
            // refresh
            this.refresh();
          else if (response.result == 2)
            this.showError("CUSTOMER_CHECK_POINTS.TITLE", "CUSTOMER_CHECK_POINTS.MSG_CANNOT_DELETE");
          else
            this.showError("CUSTOMER_CHECK_POINTS.TITLE", "CUSTOMER_CHECK_POINTS.MSG_UNKNONW_ERROR");
          this.loading = false;
        },
        error => {
          this.error = error;
          this.loading = false;
        });
    this.modelRef.close();
  }
  openEdit(item) {
    this.modelAdd = {
      id: item.id,
      customerId: item.customerId,
      serviceId: item.serviceId,
      checkPointId: item.checkPointId,
      remarks: item.remarks,
      brandId: item.brandIdList,
      deptId: item.deptIdList,
      serviceTypeId: item.serviceTypeIdList,
      countryIdList: item.countryIdList
    }
    
    if(item.checkPointId == CheckPointTypeEnum.QuotationRequired || CheckPointTypeEnum.AutoCustomerDecisionForPassReportResult){
      this.countryRequest.countryIds=item.countryIdList;
      this.getCountryListBySearch();       
    }
    this.modelsummary.isNewItem = false;
  }
  getViewPath(): string {
    return "edit-customercheckpoint";
  }
  getEditPath(): string {
    return "edit-customercheckpoint";
  }
  onChangeCustomer(id) {
    this.getData(id);
  }
  onChangeService(id) {
    this.getData(this.modelAdd.customerId, id);
  }
  private saveResponse(response) {
    if (response && response.result == 1) {
      this.showSuccess("CUSTOMER_CHECK_POINTS.TITLE", "CUSTOMER_CHECK_POINTS.MSG_SAVE_SUCCESS");
      this.refresh();
    }
    else if (response && response.result == 5)
      this.showError("CUSTOMER_CHECK_POINTS.TITLE", "CUSTOMER_CHECK_POINTS.MSG_EXISTS_RECORD");
    else
      this.showError("CUSTOMER_CHECK_POINTS.TITLE", "CUSTOMER_CHECK_POINTS.MSG_CANNOT_SAVE");
    this.modelsummary.saveLoading = false;
    this.validator.isSubmitted = false;
    this.modelsummary.customerDisabled = this.modelsummary.customerIdParam > 0 ? true : false;
  }
  private summaryResponse(response) {
    if (response && response.result == 1) {
      this.modelsummary.noFound = false;
      //GetCustomerCheckPointModel
      this.modelsummary.items = response.customerCheckPointList.map((x) => {
        var item: CustomerCheckPointModel = {
          id: x.id,
          customerName: x.customerName,
          checkPointName: x.checkPointName,
          serviceName: x.serviceName,
          remarks: x.remarks,
          customerId: x.customerId,
          serviceId: x.serviceId,
          checkPointId: x.checkPointId,
          brandIdList: x.brandList,
          deptIdList: x.deptList,
          serviceTypeIdList: x.serviceTypeList,
          brandNames: x.brandNames,
          deptNames: x.deptNames,
          serviceTypeNames: x.serviceTypeNames,
          countryIdList: x.countryIdList
        }
        return item;
      });
    }
    else {
      this.error = response.result;
      this.modelsummary.noFound = true;
    }
    this.loading = false;
  }
  private getCustomerResponse(response, id) {
    if (response && response.result == 1)
      this.modelsummary.customerList = response.customerList;
    else
      this.error = response.result;
    this.modelsummary.customerLoading = false;
    // customer id check for load data and disable customer drop down control
    if (id > 0) {
      this.modelAdd.customerId = Number(id);
      this.modelsummary.customerDisabled = true;
      this.getData(id);
    }
    else {
      this.modelAdd.customerId = null;
      this.modelsummary.customerDisabled = false;
    }
  }
  private Intitialize(id?: number) {
    this.modelsummary.isNewItem = true;
    this.modelsummary.customerLoading = true;
    this.modelsummary.customerIdParam = id;
    this.loading = false;
    this.getCustomerData(id);
    this.getServiceData();
    this.getCheckPointData();
    this.getCustomerServiiceTypes(id);
    this.getCustomerDepts(id);
    this.getCustomerBrands(id);
  }

  changeCheckPoint(item) {
    if (item) {
      this.modelsummary.customerCPLoading = true;
      if (item.id == CheckPointTypeEnum.QuotationRequired || CheckPointTypeEnum.AutoCustomerDecisionForPassReportResult) {
        this.modelsummary.customerCPLoading = false;
        this.getCountryListBySearch();
      }
      else {
        this.modelsummary.customerCPLoading = false;
      }
    }
    else {
      this.modelsummary.countryList = [];
      this.countryRequest.countryIds = [];
      this.modelsummary.customerCPLoading = false;
    }
  }

  changeCountryData(event) {
    if(!(event && event.length>0)){
      this.countryRequest.countryIds = [];
      this.getCountryListBySearch();
    }
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.modelsummary.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.modelsummary.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.countryRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.modelsummary.countryLoading = false))
      ))
      .subscribe(data => {
        this.modelsummary.countryList = data;
        this.modelsummary.countryLoading = false;
        this.countryRequest.countryIds = [];
      });
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.countryRequest = new CountryDataSourceRequest();
    this.countryRequest.searchText = this.modelsummary.countryInput.getValue();
    this.countryRequest.skip = this.modelsummary.countryList.length;

    this.modelsummary.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.modelsummary.countryList = this.modelsummary.countryList.concat(customerData);
        }

        this.countryRequest = new CountryDataSourceRequest();
        this.modelsummary.countryLoading = false;
      }),
      error => {
        this.modelsummary.countryLoading = false;
        this.setError(error);
      };
  }

  getCustomerBrands(id) {
    this.modelsummary.customerBrandLoading = true;
    this.cusService.getCustomerBrands(id)
    .pipe()
    .subscribe(response => {
      if(response && response.result == ResponseResult.Success)
      {
          this.modelsummary.customerBrandList = response.dataSourceList;
          this.modelsummary.customerBrandLoading = false;
      }
      else {
        this.modelsummary.customerBrandList = [];
        this.modelsummary.customerBrandLoading = false;
      }
    },
    error => {
      this.modelsummary.customerBrandLoading = false;
    });
  }


  getCustomerDepts(id) {
    this.modelsummary.customerDeptLoading = true;
    this.cusService.getCustomerDepartments(id)
    .pipe()
    .subscribe(response => {
      if(response && response.result == ResponseResult.Success)
      {
          this.modelsummary.customerDeptList = response.dataSourceList;
          this.modelsummary.customerDeptLoading = false;
      }
      else {
        this.modelsummary.customerDeptList = [];
        this.modelsummary.customerDeptLoading = false;
      }
    },
    error => {
      this.modelsummary.customerDeptLoading = false;
    });
  }

  getCustomerServiiceTypes(id) {
    this.modelsummary.serviceTypeLoading = true;
    this.cusConfigService.getCustomerServiceTypes(id, this.modelAdd.serviceId)
    .pipe()
    .subscribe(response => {
      if(response && response.result == ResponseResult.Success)
      {
          this.modelsummary.serviceTypeList = response.dataSourceList;
          this.modelsummary.serviceTypeLoading = false;
      }
      else {
        this.modelsummary.serviceTypeList = [];
        this.modelsummary.serviceTypeLoading = false;
      }
    },
    error => {
      this.modelsummary.serviceTypeLoading = false;
    });
  }
}
