import { Component } from '@angular/core';
import { SummaryComponent } from '../../common/summary.component';
import {
  CustomerPriceCardSummary, MasterCustomerPriceCardSummary, CustomerPriceCardSummaryResponse, CustomerPriceCardResponseResult,
  CustomerPriceCardSummaryItem
} from 'src/app/_Models/customer/customer-price-card.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Validator } from '../../common';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { CustomerResult } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { first, takeUntil } from 'rxjs/operators';
import { PageSizeCommon } from '../../common/static-data-common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Subject } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customer-price-card-summary',
  templateUrl: './customer-price-card-summary.component.html',
  styleUrls: ['./customer-price-card-summary.component.scss'],
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
export class CustomerPriceCardSummaryComponent extends SummaryComponent<CustomerPriceCardSummary> {
  componentDestroyed$: Subject<boolean> = new Subject();
  masterData: MasterCustomerPriceCardSummary;
  model: CustomerPriceCardSummary;
  pagesizeitems = PageSizeCommon;
  selectedPageSize: number;
  public modelRef: NgbModalRef;
  toggleFormSection: boolean;
  isFilterOpen: boolean;
  constructor(router: Router, validator: Validator, route: ActivatedRoute,
    public customerPriceCardService: CustomerPriceCardService, public bookingService: BookingService,
    public locationService: LocationService,
    public customerCheckPointService: CustomerCheckPointService,
    public customerService: CustomerService,
    public productManagementService: ProductManagementService,
    public utility: UtilityService,
    public referenceService: ReferenceService,
    public modalService: NgbModal, private subrouter: Router, translate: TranslateService, toastr: ToastrService
  ) {
    super(router, validator, route, translate,toastr);
    this.validator.setJSON('customer/customer-pricecardsummary.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;
  }

  onInit() {
    this.masterData = new MasterCustomerPriceCardSummary();
    this.model = new CustomerPriceCardSummary();
    this.model.pageSize=PageSizeCommon[0];
    this.selectedPageSize = PageSizeCommon[0];

    
    this.getBillingMethodList();
    this.getBillingToList();
    this.getCustomerListByUserType();
    this.getProductCategoryList();
    this.getCountryList();     
    this.getServiceList();
    this.isShownColumn();
  }

  ngAfterViewInit(){
    
    if(this.model.serviceId){ 
      this.getServiceTypesList();
    }
  }

  isShownColumn() {
    this.masterData.isShowColumn = this.masterData.isShowColumn == true ?
      false : true;
    if (!this.masterData.isShowColumn) {
      this.masterData.isShowColumnImagePath = "assets/images/new-set/table-expand.svg";
      this.masterData.showColumnTooltip = "Expand";
    }
    else {
      this.masterData.isShowColumnImagePath = "assets/images/new-set/table-collapsea.svg";
      this.masterData.showColumnTooltip = "Collapse";
    }
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //getServiceTypesList
  getServiceTypesList() {
        this.masterData.serviceTypeLoading = true;
      let request = this.generateServiceTypeRequest();

      this.referenceService.getServiceTypes(request)
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {

            this.masterData.serviceTypeList = response.serviceTypeList;
            this.masterData.serviceTypeLoading = false;
          },
          error => {
            this.setError(error);
            this.masterData.serviceTypeList = [];
            this.masterData.serviceTypeLoading = false;
          }
        );
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerId ?? 0;
    serviceTypeRequest.serviceId = this.model.serviceId ?? 0;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  //get service list
  getServiceList() {
    this.masterData.serviceLoading = true;
    this.customerCheckPointService.getService()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.serviceList = response.serviceList;
          this.masterData.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.serviceLoading = false;
        });
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  onChangeCustomer(event)
  {
    this.model.serviceTypeIdList = [];
    if(event)
    {
      this.getCustomerDepartments();
      this.getCustomerPriceCategoryList();

      if(this.model.serviceId){
        this.getServiceTypesList();
      }
    }
  }

  getCustomerDepartments() {
    this.masterData.departmentLoading = true;
    this.masterData.departmentList = [];
    this.customerService.getCustomerDepartments(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.departmentList = response.dataSourceList;
          this.masterData.departmentLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.departmentLoading = false;
        });
  }

  
  getCustomerPriceCategoryList() {
    this.masterData.priceCategoryLoading = true;
    this.masterData.priceCategoryList = [];
    this.customerService.getCustomerPriceCategories(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.priceCategoryList = response.dataSourceList;
          this.masterData.priceCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.priceCategoryLoading = false;
        });
  }


  //getBillingMethodList
  getBillingMethodList() {
    this.masterData.billingMethodLoading = true;

    this.customerPriceCardService.getBillingMethodList()
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.masterData.billingMethodList = response.dataSourceList;
            }
            this.masterData.billingMethodLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.serviceLoading = false;
        });
  }

  //getBillingToList
  getBillingToList() {
    this.masterData.billingToLoading = true;
    this.customerPriceCardService.getBillingToList()
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.masterData.billingToList = response.dataSourceList;
            }
            this.masterData.billingToLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.serviceLoading = false;
        });
  }

  //get customer list
  getCustomerListByUserType() {
    this.masterData.customerLoading = true;
    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        // this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.customerLoading = false;
      }
    );
  }

  //customer list success response
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.masterData.customerList = response.customerList;
      }
    }
    this.masterData.customerLoading = false;
  }

  //get product category list
  getProductCategoryList() {
    this.masterData.productCategoryLoading = true;
    this.productManagementService.getProductCategorySummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.productCategoryList = response.productCategoryList;
          this.masterData.productCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.productCategoryLoading = false;
        });
  }

  //get country list
  getCountryList() {
    this.masterData.countryLoading = true;
    this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.countryList = response.countryList;
          this.masterData.countryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.countryLoading = false;
        });
  }

  isFormValid() {
    return this.validator.isValid('customerId');

  }
  buttonDisable(): boolean {
    return (this.masterData.customerLoading ||
      this.masterData.productCategoryLoading ||
      this.masterData.billingMethodLoading ||
      this.masterData.billingToLoading ||
      this.masterData.serviceTypeLoading ||
      this.masterData.serviceLoading ||
      this.masterData.countryLoading ||
      this.masterData.searchLoading ||
      this.masterData.exportDataLoading);
  }

  getData() {
    this.getSummaryData();
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.masterData.searchLoading = true;
      this.search();
    }
  }

  async getSummaryData() {

    var response: CustomerPriceCardSummaryResponse;
    try {
      response = await this.customerPriceCardService.summaryData(this.model);
    }
    catch (e) {
      console.error(e);
      this.masterData.searchLoading = false;
    }

    if (response) {
      if (response.result) {
        if (response.result == CustomerPriceCardResponseResult.Success) {

          this.mapPageProperties(response);

          this.model.items = response.getData.map((x) => {
            var item: CustomerPriceCardSummaryItem = {
              currencyName: x.currencyName,
              billMethodName: x.billMethodName,
              billToName: x.billToName,
              customerName: x.customerName,
              departmentName:x.departmentName,
              priceCategory:x.priceCategory,
              periodFrom: x.periodFrom,
              periodTo: x.periodTo,
              unitPrice: x.unitPrice,
              serviceName: x.serviceName,
              id: x.id,
              factoryCountryList: x.factoryCountryList,
              serviceTypeList: x.serviceTypeList,
              travelIncluded: x.travelIncluded,
              createdByName:x.createdByName,
              createdOn:x.createdOn,
              updatedByName:x.updatedByName,
              updatedOn:x.updatedOn
            }
            return item;
          });
        }
        else if (response.result == CustomerPriceCardResponseResult.NotFound) {
          this.model.noFound = true;
        }
        else {
          this.model.noFound = true;
        }
      }
      this.masterData.searchLoading = false;
    }
  }

  redirectRegisterPage(id: number, newId: number) {
    if (newId > 0)
      this.getDetail(id, newId);
    else
      this.getDetail(id, newId);
  }
  getDetail(id, newid) {

    this.getDetails(id, newid);
  }
  getPathDetails() {
    return "pricecardedit/price-card";
  }

  async delete() {

    var response: CustomerPriceCardResponseResult;
    try {
      response = await this.customerPriceCardService.delete(this.model.id);
    }
    catch (e) {
      console.error(e);
    }

    if (response) {
      if (response == CustomerPriceCardResponseResult.Success) {
        this.showSuccess('CUSTOMER_PRICE_CARD_SUMMARY.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
        this.refresh();
      }
      else if (response == CustomerPriceCardResponseResult.NotFound) {
        this.showError('CUSTOMER_PRICE_CARD_SUMMARY.LBL_TITLE', 'COMMON.MSG_CANNOT_DELETE');
      }
      this.masterData.searchLoading = false;
      this.modelRef.close();
    }
  }
  reset() {
    this.onInit();
  }
  openPopup(id, content) {
    this.model.id = id;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
  }

  //export the summary details
  export() {
    this.masterData.exportDataLoading = true;
    this.customerPriceCardService.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.masterData.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "pricecard_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "pricecard_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.masterData.exportDataLoading = false;
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "PeriodFrom": { 
        this.model.periodFrom=null;
        break; 
     } 
     case "PeriodTo": { 
      this.model.periodTo=null;
        break; 
     } 
    }
  }

  changeService(){
    this.masterData.serviceTypeList = [];
    this.model.serviceTypeIdList = [];
    if (this.model.serviceId) {
      this.getServiceTypesList();
    }
  }
}
