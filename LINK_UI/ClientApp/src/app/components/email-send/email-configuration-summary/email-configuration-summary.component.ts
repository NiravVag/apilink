import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { EmailConfigSummaryItem, EmailConfigSummaryMasterModel, EmailConfigSummaryModel } from 'src/app/_Models/email-send/email-config-summary.model';
import { EmailConfigResponseResult } from 'src/app/_Models/email-send/email-config.model';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { EmailConfigurationService } from 'src/app/_Services/email-send/email-config.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Validator } from '../../common';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-email-configuration-summary',
  templateUrl: './email-configuration-summary.component.html',
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
export class EmailConfigurationSummaryComponent extends SummaryComponent<EmailConfigSummaryModel> {
  componentDestroyed$: Subject<boolean> = new Subject();
  pagesizeitems = PageSizeCommon;
  isFilterOpen: boolean;
  selectedPageSize;
  masterModel: EmailConfigSummaryMasterModel;
  private modelRef: NgbModalRef;
  toggleFormSection: boolean;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, private cusService: CustomerService, private modalService: NgbModal,
    public emailConfigService: EmailConfigurationService, public customerPriceCardService: CustomerPriceCardService,
    public brandService: CustomerBrandService, public deptService: CustomerDepartmentService,
    public referenceService: ReferenceService, public customerProductService: CustomerProduct,
    public officeService: OfficeService, public locationService: LocationService,public utility: UtilityService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
    this.validator.isSubmitted = false;
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(): void {
    this.model = new EmailConfigSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.masterModel = new EmailConfigSummaryMasterModel();

    this.getServiceTypeList();
    this.getServiceList();
    this.getProductCategoryList();
    this.getFullBridgeResultData();
    this.getEmailSendTypeList();
  }

	ngAfterViewInit() {
    this.getCustomerListBySearch();
    this.getOfficeList();
    this.getCountryListBySearch();
    this.getBrandListBySearch();
    this.getDeptListBySearch();
  }

  changeCustomer(event) {
    this.customerLoadList(event);

    this.model.brandIdList = [];
    this.model.departmentIdList = [];

    this.getServiceTypeList();
  }

  customerLoadList(event) {
    if (event && event.id > 0) {
      this.getBrandListBySearch();
      this.getDeptListBySearch();

      this.masterModel.brandSearchRequest.customerId = this.model.customerId;
      this.masterModel.deptSearchRequest.customerId = this.model.customerId;
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  reset() {
    this.model = new EmailConfigSummaryModel();
    this.validator.isSubmitted = false;
    this.masterModel.customerModelRequest.searchText = "";
    this.getCustomerListBySearch();
  }

  getData(): void {
    this.getSearchData();
  }

  getPathDetails(): string {
    return "emailconfig/config";
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {
    this.masterModel.customerModelRequest.searchText = this.masterModel.customerInput.getValue();
    this.masterModel.customerModelRequest.skip = this.masterModel.customerList.length;

    // if (this.masterModel.customerModelRequest.searchText ==null && this.model.customerId && this.model.customerId > 0) {
    //   this.masterModel.customerModelRequest.id = this.model.customerId;
    // }
    // else {
    // }
      this.masterModel.customerModelRequest.id = 0;

    this.masterModel.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.customerList = this.masterModel.customerList.concat(data);
        }
        this.masterModel.customerModelRequest = new CommonDataSourceRequest();
        this.masterModel.customerLoading = false;
      }),
      error => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 customer on load
  getCustomerListBySearch() {
    if( this.model.customerId &&  this.model.customerId >0) {
      this.masterModel.customerModelRequest.id = this.model.customerId;
    }
    else{
      this.masterModel.customerModelRequest.id = null;
    }

    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest, term)
        : this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() =>
            this.masterModel.customerLoading = false
            ))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //search api call
  getSearchData() {
    this.masterModel.searchLoading = true;
    this.model.noFound = false;

      this.emailConfigService.search(this.model)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == EmailConfigResponseResult.Success) {
            this.mapPageProperties(response);

            this.model.items = response.data.map(x => {
              var item: EmailConfigSummaryItem = {
                emailConfigId: x.emailConfigId,
                customerName: x.customerName,
                brandName: x.brandName,
                departmentName: x.departmentName,
                factoryCountryName: x.factoryCountryName,
                officeName: x.officeName,
                productcategoryName: x.productcategoryName,
                resultName: x.resultName,
                serviceTypeName: x.serviceTypeName,
                reportSendType:x.reportSendType,
                reportinemail:x.reportInEmail,
                service:x.service,
                emailTypeName: x.emailTypeName,
                createdOn: x.createdOn,
                createdBy: x.createdBy,
                updatedOn: x.updatedOn,
                updatedBy: x.updatedBy
              }
              return item;
            });
            this.masterModel.searchLoading = false;
          }
          else if (response && response.result == EmailConfigResponseResult.NotFound) {
            this.model.items = [];
            this.model.noFound = true;
            this.masterModel.searchLoading = false;
          }
          else {
            this.model.items = [];
            this.model.noFound = true;
            this.masterModel.searchLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;
        });

  }

  //search details
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getData();

  }

  //open pop up
  openConfirm(id, content) {

    this.masterModel.deleteId = id;

    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
  }

  //remove the id assign
  getId() {
    this.masterModel.deleteId = null;
  }

  //delete the email config subject
  delete() {
    this.masterModel.deleteLoading = true;
    if (this.masterModel.deleteId > 0) {
      this.emailConfigService.delete(this.masterModel.deleteId)
      .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailConfigResponseResult.Success) {
                this.showSuccess('EMAIL_CONFIGURATION_SUMMARY.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
              }
              else {
                this.showError('EMAIL_CONFIGURATION_SUMMARY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
              this.masterModel.deleteLoading = false;
              this.modelRef.close();
              this.getSearchData();
            }
          },
          error => {
            this.setError(error);
            this.masterModel.deleteLoading = false;
          });
    }
  }

  getServiceList() {
    this.masterModel.serviceLoading = true;
    this.referenceService.getServiceList()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.serviceList = data.dataSourceList;
          }
          this.masterModel.serviceLoading = false;
        },
        error => {
          this.masterModel.serviceLoading = false;
        });
  }

  getServiceTypeList() {
    this.model.serviceTypeIdList = [];
    if(this.model.serviceId) {
    this.masterModel.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();

    this.referenceService.getServiceTypes(request)
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.serviceTypeList = data.serviceTypeList;
          }
          else{
          this.masterModel.serviceTypeList = [];
          }
          this.masterModel.serviceTypeLoading = false;
        },
        error => {
          this.masterModel.serviceTypeList = [];
          this.masterModel.serviceTypeLoading = false;
        });
      }
      else {
        this.masterModel.serviceTypeList = [];
      }
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerId ?? 0;
    serviceTypeRequest.serviceId = this.model.serviceId ?? 0;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  getProductCategoryList() {
    this.masterModel.productCategoryLoading = true;
    this.customerProductService.getProductCategoryList()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.productCategoryList = data.productCategoryList;
          }
          this.masterModel.productCategoryLoading = false;
        },
        error => {
          this.masterModel.productCategoryLoading = false;
        });
  }
  getFullBridgeResultData() {
    this.masterModel.apiResultLoading = true;
    this.referenceService.getFullBridgeResultData()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.apiResultList = data.dataSourceList;
          }
          this.masterModel.apiResultLoading = false;
        },
        error => {
          this.masterModel.apiResultLoading = false;
        });
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {

    if (this.model.factoryCountryIdList && this.model.factoryCountryIdList.length > 0) {
      this.masterModel.factoryCountryRequest.countryIds = this.model.factoryCountryIdList;
    }
    else {
      this.masterModel.factoryCountryRequest.countryIds = null;
    }

    this.masterModel.factoryCountryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factoryCountryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest, term)
        : this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factoryCountryLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.factoryCountryList = data;
        this.masterModel.factoryCountryLoading = false;
      });
  }
  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {

    if (this.model.brandIdList && this.model.brandIdList.length > 0) {
      this.masterModel.brandSearchRequest.idList = this.model.brandIdList;
    }
    else {
      this.masterModel.brandSearchRequest.idList = null;
    }

    this.masterModel.brandSearchRequest.customerId = this.model.customerId;

    this.masterModel.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.brandLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.brandList = data;
        this.masterModel.brandLoading = false;
      });
  }

  //fetch the brand data with virtual scroll
  getDeptData() {
    this.masterModel.deptSearchRequest.searchText = this.masterModel.departmentInput.getValue();
    this.masterModel.deptSearchRequest.skip = this.masterModel.departmentList.length;

    this.masterModel.departmentLoading = true;
    this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.masterModel.departmentList = this.masterModel.departmentList.concat(deptData);
        }
        this.masterModel.deptSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.departmentLoading = false;
      }),
      error => {
        this.masterModel.departmentLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getDeptListBySearch() {

    if (this.model.departmentIdList && this.model.departmentIdList.length > 0) {
      this.masterModel.deptSearchRequest.idList = this.model.departmentIdList;
    }
    else {
      this.masterModel.deptSearchRequest.idList = null;
    }

    this.masterModel.deptSearchRequest.customerId = this.model.customerId;

    this.masterModel.departmentInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.departmentLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.masterModel.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.departmentLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.departmentList = data;
        this.masterModel.departmentLoading = false;
      });
  }

  //get office list
  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.referenceService.getOfficeList()
    .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.officeList = response.dataSourceList;
          this.masterModel.officeLoading = false;

        },
        error => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        });
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.masterModel.factoryCountryRequest.searchText = this.masterModel.factoryCountryInput.getValue();
    this.masterModel.factoryCountryRequest.skip = this.masterModel.factoryCountryList.length;

    this.masterModel.factoryCountryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.factoryCountryList = this.masterModel.factoryCountryList.concat(customerData);
        }
        this.masterModel.factoryCountryRequest = new CountryDataSourceRequest();
        this.masterModel.factoryCountryLoading = false;
      }),
      error => {
        this.masterModel.factoryCountryLoading = false;
      };
  }

  //fetch the brand data with virtual scroll
  getBrandData() {
    this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    this.masterModel.brandSearchRequest.skip = this.masterModel.brandList.length;

    this.masterModel.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.masterModel.brandSearchRequest)
    .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.masterModel.brandList = this.masterModel.brandList.concat(brandData);
        }
        this.masterModel.brandSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.brandLoading = false;
      }),
      error => {
        this.masterModel.brandLoading = false;
      };
  }

  //redirect to form page
  redirectRegisterPage(id: number, newId: number) {
    if (newId > 0)
      this.getDetails(id, newId);
    else
      this.getDetails(id, newId);
  }
  clearCustomer() {
    this.masterModel.customerModelRequest.searchText = null;
    this.getCustomerListBySearch();
    this.getServiceTypeList();
  }
    toggleSection() {
      this.toggleFormSection = !this.toggleFormSection;
    }

    clearDepartment() {
      this.getDeptListBySearch();
    }

    clearBrand() {
      this.getBrandListBySearch();
    }

    clearCountry() {
      this.getCountryListBySearch();
    }
    //get email send type list
	getEmailSendTypeList() {
		this.masterModel.esTypeLoading = true;
		this.emailConfigService.getEmailSendTypeList()
		.pipe(takeUntil(this.componentDestroyed$))
			.subscribe(
				data => {
					if (data && data.result == ResponseResult.Success) {
						this.masterModel.esTypeList = data.dataSourceList;
					}
					this.masterModel.esTypeLoading = false;
				},
				error => {
					this.masterModel.esTypeLoading = false;
				});
	}

    changeService(){
      this.getServiceTypeList();
    }
}
