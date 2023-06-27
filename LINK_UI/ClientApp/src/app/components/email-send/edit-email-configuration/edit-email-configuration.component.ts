import { flatten } from '@angular/compiler';
import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { TouchSequence } from 'selenium-webdriver';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { SupFactDashboardDataSourceRequest } from 'src/app/_Models/dashboard/supfactdashboard.model';
import { EmailSendingType } from 'src/app/_Models/email-send/edit-email-send.model';
import { EmailConfig, EmailConfigMaster, EmailConfigResponseResult, EmailSubRequest, ReportInEmail, EmailSendType, AdditionalEmailRecipient } from 'src/app/_Models/email-send/email-config.model';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { InspectionCustomerDecisionService } from 'src/app/_Services/booking/inspectioncustomerdecision.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerBrandService } from 'src/app/_Services/customer/customerbrand.service';
import { CustomerbuyerService } from 'src/app/_Services/customer/customerbuyer.service';
import { CustomerCollectionService } from 'src/app/_Services/customer/customercollection.service';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { EmailConfigurationService } from 'src/app/_Services/email-send/email-config.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { EmailSendToOrCCAPITeam, EmailSendToOrCCCustomerContact, EmailTypeEnum, EmailTypeReportSend, InvoiceType, ListSize, ReportSendInEmailAttachment, SupplierType } from '../../common/static-data-common';

@Component({
  selector: 'app-edit-email-configuration',
  templateUrl: './edit-email-configuration.component.html'
})
export class EditEmailConfigurationComponent extends DetailComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: EmailConfigMaster;
  model: EmailConfig;
  copyId: number;
  _EmailTypeReportSend = EmailTypeReportSend;
  pageLoading: boolean = true;
  iscustomercontactvalidate: boolean = false;
  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public modalService: NgbModal, private cusService: CustomerService,
    public validator: Validator, private supService: SupplierService, public locationService: LocationService,
    public brandService: CustomerBrandService, public deptService: CustomerDepartmentService, public collectionService: CustomerCollectionService,
    public buyerService: CustomerbuyerService, public customerPriceCardService: CustomerPriceCardService,
    public customerProductService: CustomerProduct, public referenceService: ReferenceService,
    public emailConfigService: EmailConfigurationService, private activatedRoute: ActivatedRoute,
    public officeService: OfficeService, private cusDecisionService: InspectionCustomerDecisionService,
    public customerService: CustomerService, public utility: UtilityService) {

    super(router, route, translate, toastr, modalService);

    this.validator.setJSON("emailsend/emailconfig.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;

    this.copyId = parseInt(this.activatedRoute.snapshot.paramMap.get("newid"));
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.masterModel = new EmailConfigMaster();
    this.model = new EmailConfig();

    if (id && id > 0) {
      this.edit(id);
    }
    else {
      this.getCustomerListBySearch();
      this.getCountryListBySearch();
      this.getOfficeList();

      this.getSpecialRuleList();
      this.getReportInEmailList();
      this.getEmailSizeList();
      //this.getReportSendTypeList();
      this.getEmailSubjectList();
      this.getServiceTypeList();
      this.getFullBridgeResultData();
      this.getServiceList();
      this.getProductCategoryList();
      this.getStaffNameList();
      this.getEmailSendTypeList();
      this.getFileNameList();
      this.getRecipientTypeList();

      this.getInvoiceTypeList();
    }
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {
    if (this.masterModel.customerInput) {
      this.masterModel.customerModelRequest.searchText = this.masterModel.customerInput.getValue();
    }
    this.masterModel.customerModelRequest.skip = this.masterModel.customerList?.length;


    if (this.model.customerId && this.model.customerId > 0) {
      this.masterModel.customerModelRequest.id = this.model.customerId;
    }
    else {
      this.masterModel.customerModelRequest.id = null;
    }

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

    if (this.model.customerId && this.model.customerId > 0) {
      this.masterModel.customerModelRequest.id = this.model.customerId;
    }
    else {
      this.masterModel.customerModelRequest.id = null;
    }

    this.masterModel.customerModelRequest.customerId = 0;
    this.masterModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest, term)
        : this.cusService.getCustomerDataSourceList(this.masterModel.customerModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
        this.pageLoading = false;
      });
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterModel.supplierModelRequest.searchText = this.masterModel.supplierInput.getValue();
    this.masterModel.supplierModelRequest.skip = this.masterModel.supplierList.length;

    this.masterModel.supplierModelRequest.customerId = this.model.customerId;
    this.masterModel.supplierModelRequest.supplierType = SupplierType.Supplier;
    this.masterModel.supplierLoading = true;
    this.supService.getFactoryDataSourceList(this.masterModel.supplierModelRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(customerData);
        }
        this.masterModel.supplierModelRequest.skip = 0;
        this.masterModel.supplierModelRequest.take = ListSize;
        this.masterModel.supplierLoading = false;
      }),
      error => {
        this.masterModel.supplierLoading = false;
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {

    if (this.model.supplierIdList && this.model.supplierIdList.length > 0) {
      this.masterModel.supplierModelRequest.idList = this.model.supplierIdList;
    }
    else {
      this.masterModel.supplierModelRequest.idList = null;
    }

    this.masterModel.supplierModelRequest.customerId = this.model.customerId;
    this.masterModel.supplierModelRequest.supplierType = SupplierType.Supplier;
    this.masterModel.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.supplierModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.supplierModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.supplierLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.supplierList = data;
        this.masterModel.supplierLoading = false;
      });
  }

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.masterModel.factoryModelRequest.searchText = this.masterModel.factoryInput.getValue();
    this.masterModel.factoryModelRequest.skip = this.masterModel.factoryList.length;

    this.masterModel.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryLoading = true;
    this.masterModel.factoryModelRequest.customerId = this.model.customerId;
    this.supService.getFactoryDataSourceList(this.masterModel.factoryModelRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(supplierList => {
        if (supplierList && supplierList.length > 0) {
          this.masterModel.supplierList = this.masterModel.supplierList.concat(supplierList);
        }
        this.masterModel.factoryModelRequest.skip = 0;
        this.masterModel.factoryModelRequest.take = ListSize;
        this.masterModel.factoryLoading = false;
      }),
      error => {
        this.masterModel.factoryLoading = false;
      };
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {

    if (this.model.factoryIdList && this.model.factoryIdList.length > 0) {
      this.masterModel.factoryModelRequest.idList = this.model.factoryIdList;
    }
    else {
      this.masterModel.factoryModelRequest.idList = null;
    }

    this.masterModel.factoryModelRequest.customerId = this.model.customerId;
    this.masterModel.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterModel.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterModel.factoryModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterModel.factoryModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.factoryLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.factoryList = data;
        this.masterModel.factoryLoading = false;
      });
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
        this.masterModel.brandSearchRequest = new CommonDataSourceRequest();
        this.masterModel.brandLoading = false;
      }),
      error => {
        this.masterModel.brandLoading = false;
      };
  }

  //fetch the first take (variable) count brand on load
  getBrandListBySearch() {

    if (this.model.brandIdList && this.model.brandIdList.length > 0) {
      this.masterModel.brandSearchRequest.idList = this.model.brandIdList;
    }
    else {
      this.masterModel.brandSearchRequest.idList = null;
    }

    this.masterModel.brandSearchRequest.searchText = "";
    //apply search text when there is keyinput data
    if (this.masterModel.brandInput.getValue()) {
      this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    }

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
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.masterModel.departmentList = this.masterModel.departmentList.concat(deptData);
        }
        this.masterModel.deptSearchRequest = new CommonDataSourceRequest();
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

    this.masterModel.deptSearchRequest.searchText = "";
    //apply search text when there is keyinput data
    if (this.masterModel.departmentInput.getValue()) {
      this.masterModel.deptSearchRequest.searchText = this.masterModel.departmentInput.getValue();
    }

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

  //fetch the buyer data with virtual scroll
  getBuyerData() {
    this.masterModel.buyerSearchRequest.searchText = this.masterModel.buyerInput.getValue();
    this.masterModel.buyerSearchRequest.skip = this.masterModel.buyerList.length;

    this.masterModel.buyerLoading = true;
    this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(buyerData => {
        if (buyerData && buyerData.length > 0) {
          this.masterModel.buyerList = this.masterModel.buyerList.concat(buyerData);
        }
        this.masterModel.buyerSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.buyerLoading = false;
      }),
      error => {
        this.masterModel.buyerLoading = false;
      };
  }

  //fetch the first take (variable) count buyer on load
  getBuyerListBySearch() {

    if (this.model.buyerIdList && this.model.buyerIdList.length > 0) {
      this.masterModel.buyerSearchRequest.idList = this.model.buyerIdList;
    }
    else {
      this.masterModel.buyerSearchRequest.idList = null;
    }

    this.masterModel.buyerSearchRequest.searchText = "";
    //apply search text when there is keyinput data
    if (this.masterModel.buyerInput.getValue()) {
      this.masterModel.buyerSearchRequest.searchText = this.masterModel.buyerInput.getValue();
    }

    this.masterModel.buyerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.buyerLoading = true),
      switchMap(term => term
        ? this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest, term)
        : this.buyerService.getBuyerListByCustomerId(this.masterModel.buyerSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.buyerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.buyerList = data;
        this.masterModel.buyerLoading = false;
      });
  }

  //fetch the collection data with virtual scroll
  getCollectionData() {
    this.masterModel.collectionSearchRequest.searchText = this.masterModel.collectionInput.getValue();
    this.masterModel.collectionSearchRequest.skip = this.masterModel.collectionList.length;

    this.masterModel.collectionLoading = true;
    this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(collectionData => {
        if (collectionData && collectionData.length > 0) {
          this.masterModel.collectionList = this.masterModel.collectionList.concat(collectionData);
        }
        this.masterModel.collectionSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.collectionLoading = false;
      }),
      error => {
        this.masterModel.collectionLoading = false;
      };
  }

  //fetch the first take (variable) count collection on load
  getCollectionListBySearch() {

    if (this.model.collectionIdList && this.model.collectionIdList.length > 0) {
      this.masterModel.collectionSearchRequest.idList = this.model.collectionIdList;
    }
    else {
      this.masterModel.collectionSearchRequest.idList = null;
    }

    this.masterModel.collectionSearchRequest.searchText = "";
    //apply search text when there is keyinput data
    if (this.masterModel.collectionInput.getValue()) {
      this.masterModel.collectionSearchRequest.searchText = this.masterModel.collectionInput.getValue();
    }

    this.masterModel.collectionInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.collectionLoading = true),
      switchMap(term => term
        ? this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest, term)
        : this.collectionService.getCollectionListByCustomerId(this.masterModel.collectionSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.collectionLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.collectionList = data;
        this.masterModel.collectionLoading = false;
      });
  }

  //fetch the country data with virtual scroll
  getCountryData() {
    this.masterModel.factoryCountryRequest.searchText = this.masterModel.factoryCountryInput.getValue();
    this.masterModel.factoryCountryRequest.skip = this.masterModel.factoryCountryList.length;

    this.masterModel.factoryCountryLoading = true;
    this.locationService.getCountryDataSourceList(this.masterModel.factoryCountryRequest)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(customerData => {
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

  customerLoadList(event) {
    if (event && event.id > 0) {

      this.masterModel.brandSearchRequest.customerId = this.model.customerId;
      this.masterModel.deptSearchRequest.customerId = this.model.customerId;
      this.masterModel.buyerSearchRequest.customerId = this.model.customerId;
      this.masterModel.collectionSearchRequest.customerId = this.model.customerId;
      this.masterModel.supplierModelRequest.customerId = this.model.customerId;
      this.masterModel.factoryModelRequest.customerId = this.model.customerId;

      this.getBrandListBySearch();
      this.getDeptListBySearch();
      this.getBuyerListBySearch();
      this.getCollectionListBySearch();
      this.getSupListBySearch();
      this.getFactListBySearch();
      this.getCustomerContactList();
    }
  }

  changeCustomer(event) {
    this.clearData();
    this.customerLoadList(event);
    this.getCustomerDecisionList();
    this.getEmailSubjectList();
    this.getServiceTypeList();
    this.getFileNameList();
  }

  changeCustomerData(event) {
    this.getFileNameList();
    this.customerLoadList(event);
    this.getCustomerDecisionList();
    this.getEmailSubjectList();
    this.getServiceTypeList();
  }

  changeEmailType(event) {

    if (event && event.id && event.id > 0) {
      this.setBaseOnEmailType(event.id);
      this.getReportSendTypeList(event.id);
      this.getToAndCCRecipientList();
      this.model.toIdList = [];
      this.model.ccIdList = [];
      this.model.emailSubjectId = null;
      this.model.reportSendTypeId = null;
      if (event.id == EmailSendType.ReportSend) {
        this.model.numberOfReports = null;
        this.model.reportInEmailId = null;
      }
      if (event.id != EmailSendType.InvoiceSend) {
        this.model.invoiceTypeId = null;
      }
    }

    this.getEmailSubjectList();
  }

  setBaseOnEmailType(id) {
    this.masterModel.EmailTypeBaseOnLoad = id && id == EmailSendType.CustomerDecision;
    if (id == EmailSendType.CustomerDecision) {
      this.masterModel.disableNoOfReport = true;
      this.masterModel.disableReportInEmail = true;
      this.model.numberOfReports = null;
      this.model.reportInEmailId = ReportInEmail.Link;
    }
    else if (id == EmailSendType.ReportSend) {
      this.masterModel.disableNoOfReport = false;
      this.masterModel.disableReportInEmail = false;
    }
    else {
      this.masterModel.disableNoOfReport = false;
      //this.model.reportInEmailId = null; // Commented this to resolve bug ALD-3348
      this.masterModel.disableReportInEmail = false;
    }
  }

  clearData() {
    this.model.brandIdList = [];
    this.model.departmentIdList = [];
    this.model.buyerIdList = [];
    this.model.collectionIdList = [];
    this.model.factoryIdList = [];
    this.model.supplierIdList = [];
    this.model.customerContactIdList = [];
    this.model.emailSubjectId = null;
  }

  clearListData() {
    this.masterModel.brandList = [];
    this.masterModel.departmentList = [];
    this.masterModel.buyerList = [];
    this.masterModel.collectionList = [];
    this.masterModel.factoryList = [];
    this.masterModel.supplierList = [];
    this.masterModel.customerContactList = [];
  }

  getServiceTypeList() {

    if (!this.model.id || !(this.model.id > 0))
      this.model.serviceTypeIdList = [];

    if (this.model.serviceId) {
      this.masterModel.serviceTypeLoading = true;

      let request = this.generateServiceTypeRequest();
      this.referenceService.getServiceTypes(request)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          data => {
            if (data && data.result == ResponseResult.Success) {
              this.masterModel.serviceTypeList = data.serviceTypeList;
            }
            else {
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

  getSpecialRuleList() {
    this.masterModel.specialRuleLoading = true;
    this.emailConfigService.getSpecialRuleList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.specialRuleList = data.dataSourceList;
          }
          this.masterModel.specialRuleLoading = false;
        },
        error => {
          this.masterModel.specialRuleLoading = false;
        });
  }
  getReportInEmailList() {
    this.masterModel.reportInEmailLoading = true;
    this.emailConfigService.getReportInEmailList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.reportInEmailList = data.dataSourceList;
          }
          this.masterModel.reportInEmailLoading = false;
        },
        error => {
          this.masterModel.reportInEmailLoading = false;
        });
  }
  getEmailSizeList() {
    this.masterModel.emailSizeLoading = true;
    this.emailConfigService.getEmailSizeList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.emailSizeList = data.dataSourceList;
          }
          this.masterModel.emailSizeLoading = false;
        },
        error => {
          this.masterModel.emailSizeLoading = false;
        });
  }

  //frame the object
  frameObject(): EmailSubRequest {
    var request = new EmailSubRequest();
    request.customerId = this.model.customerId ? this.model.customerId : 0;
    request.emailTypeId = this.model.typeId ? this.model.typeId : 0;
    return request;
  }

  //clear the email type
  clearEmailType() {
    this.masterModel.EmailTypeBaseOnLoad = false;
    this.getEmailSubjectList();

  }

  //clear the customer selection
  clearCustomer() {
    this.getEmailSubjectList();
    this.clearData();
    this.getCustomerDecisionList();
    this.clearListData();
    this.getCustomerListBySearch();
    this.getServiceTypeList();
    this.getFileNameList();
  }

  //get email subject list
  getEmailSubjectList() {
    this.masterModel.emailSubjectList = [];
    this.masterModel.emailSubjectLoading = true;
    this.emailConfigService.getEmailSubjectList(this.frameObject())
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.emailSubjectList = data.dataSourceList;
          }
          this.masterModel.emailSubjectLoading = false;
        },
        error => {
          this.masterModel.emailSubjectLoading = false;
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

  getReportSendTypeList(emailTypeId) {

    this.masterModel.reportSendTypeLoading = true;
    this.emailConfigService.getReportSendTypeList(emailTypeId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.reportSendTypeList = data.dataSourceList;
          }
          this.masterModel.reportSendTypeLoading = false;
        },
        error => {
          this.masterModel.reportSendTypeLoading = false;
        });
  }

  getStaffNameList() {
    this.masterModel.apiContactLoading = true;
    this.emailConfigService.getStaffNameList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.apiContactList = data.dataSourceList;
          }
          this.masterModel.apiContactLoading = false;
        },
        error => {
          this.masterModel.apiContactLoading = false;
        });
  }

  getCustomerContactList() {
    this.masterModel.customerContactLoading = true;
    this.customerService.getCustomerContactList(this.model.customerId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.customerContactList = response.dataSourceList;
          this.masterModel.customerContactLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.customerContactLoading = false;
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

  getCustomerDecisionList() {
    this.masterModel.cusDecisionLoading = true;
    var id = this.model.customerId > 0 ? this.model.customerId : 0;
    this.cusDecisionService.GetInspectionCustomerDecision(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.cusDecistionList = data.customerDecisionList;
          }
          this.masterModel.cusDecisionLoading = false;
        },
        error => {
          this.masterModel.cusDecisionLoading = false;
        });
  }
  //is form valid
  isformValid(): boolean {
    var isOk = false;
    if (this.model.invoiceTypeId != InvoiceType.PreInvoice) {
      isOk = this.validator.isValid('customerId');
      if (!isOk)
        return isOk;
    }
    var isOk = this.validator.isValid('typeId') && this.validator.isValid('serviceId') && this.validator.isValid('apiContactIdList') &&
      this.validator.isValid('toIdList') &&
      this.validator.isValid('emailSubjectId') &&
      this.validator.isValid('reportInEmailId') && this.validator.isValid('reportSendTypeId')
      && this.validator.isValid('fileNameId');

    if (isOk && this.model.typeId == EmailSendingType.InvoiceStatus) {
      isOk = this.validator.isValid("invoiceTypeId");
    }
    if (isOk && (this.model.numberOfReports && !(this.model.numberOfReports > 0)) || this.model.numberOfReports == 0) {
      isOk = !isOk;
      this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.LBL_NO_OF_REPORTS_GREATER_THAN_ZERO');
    }

    // if (isOk && this.model.typeId == EmailSendType.ReportSend && !this.model.isCustomerContact) {
    // 	if (!(this.model.customerContactIdList && this.model.customerContactIdList.length > 0)) {
    // 		isOk = !isOk;
    // 	}
    // }
    var Iscc: boolean = false;
    if (isOk && (this.model.toIdList && this.model.toIdList.includes(EmailSendToOrCCCustomerContact)) ||
      (this.model.ccIdList && this.model.ccIdList.includes(EmailSendToOrCCCustomerContact))) {
      Iscc = true;
    }
    this.iscustomercontactvalidate = false;
    if (isOk && Iscc && (this.model.customerContactIdList && this.model.customerContactIdList.length == 0)) {
      isOk = !isOk;
      this.iscustomercontactvalidate = true;
      this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.LBL_SELECT_CUSTOMER_CONTACT');
    }
    var IsApiteam: boolean = false;
    if (isOk && (this.model.toIdList && this.model.toIdList.includes(EmailSendToOrCCAPITeam)) ||
      (this.model.ccIdList && this.model.ccIdList.includes(EmailSendToOrCCAPITeam))) {
      IsApiteam = true;
    }
    if (isOk && !IsApiteam) {
      isOk = !isOk;
      this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_API_TEAM_SELECT_TO_OR_CC_LIST');
    }
    if (isOk && this.model.reportInEmailId && this.model.reportInEmailId == ReportSendInEmailAttachment && !this.model.fileNameId) {
      isOk = !isOk;
      this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_FILE_FORMAT');
    }

    //iscustomercontact or customerContactIdList select. but, not select to or cc
    if (isOk && (this.model.isCustomerContact || this.model.customerContactIdList.length > 0) &&
      ((this.model.toIdList.indexOf(EmailSendToOrCCCustomerContact) == -1) &&
        (this.model.ccIdList.indexOf(EmailSendToOrCCCustomerContact) == -1))

    ) {
      isOk = !isOk;
      this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.LBL_TO_OR_CC_LIST_CHOOSE_CUSTOMER');
    }
    return isOk;
  }

  //save the details
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isformValid()) {
      if (this.model.id == null || this.copyId == 0) {
        this.model.id = 0;
      }
      this.masterModel.saveLoading = true;
      this.emailConfigService.save(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(
          response => {
            if (response) {
              if (response.result == EmailConfigResponseResult.Success) {
                //if (this.fromSummary) {
                this.return('econfig/summary');
                //}
                // else {
                // 	this.edit(response.id);
                // }
                this.showSuccess('EMAIL_CONFIGURATION.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');
              }
              else if (response.result == EmailConfigResponseResult.DataExists) {
                this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_SAME_RULE_DETAILS_EXISTS');
              }
              else {
                this.showWarning('EMAIL_CONFIGURATION.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
              }
            }
            this.masterModel.saveLoading = false;
          },
          error => {
            if (error && error.error && error.error.errors && error.error.statusCode == 400) {
              let validationErrors: [];
              validationErrors = error.error.errors;
              this.openValidationPopup(validationErrors);
            }
            else {
              this.showError("EMAIL_CONFIGURATION.LBL_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterModel.saveLoading = false;
          });
    }
  }

  //reset the selected vlaues
  reset() {
    this.model = new EmailConfig();
    this.validator.isSubmitted = false;
  }

  //edit the details
  edit(id: number) {
    this.emailConfigService.edit(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success) {
            this.model = response.emailSendDetails;
            this.masterModel.customerModelRequest.id = this.model.customerId;
            // this.masterModel.supplierModelRequest.id = this.model.supplierIdList;
            // this.masterModel.factoryModelRequest.id = this.model.factoryIdList;

            var data = { id: this.model.customerId, name: "" };
            this.changeCustomerData(data);
            //this.customerLoadList(data);
            this.setBaseOnReportInEmail(this.model.reportInEmailId);
            this.setBaseOnEmailType(this.model.typeId);

            this.getCustomerListBySearch();
            this.getCountryListBySearch();
            this.getOfficeList();

            this.getToAndCCRecipientList();
            this.getSpecialRuleList();
            this.getReportInEmailList();
            this.getEmailSizeList();
            this.getReportSendTypeList(this.model.typeId);
            this.getEmailSubjectList();
            this.getServiceTypeList();
            this.getFullBridgeResultData();
            this.getServiceList();
            this.getProductCategoryList();
            this.getStaffNameList();
            this.getEmailSendTypeList();
            this.getFileNameList();
            this.getRecipientTypeList();
            // this.changeCCList(this.model.ccIdList);
            // this.changeToList(this.model.toIdList);
            this.getInvoiceTypeList();
          }
        },
        error => {
          this.setError(error);
        });
  }

  getViewPath(): string {
    return '';
  }

  getEditPath(): string {
    return '';
  }

  //get file name from email subject config
  getFileNameList() {
    this.masterModel.subjectFileNameList = [];
    this.masterModel.subjectFileNameLoading = true;
    this.emailConfigService.getFileNameList(this.frameObject())
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {
            this.masterModel.subjectFileNameList = data.dataSourceList;
          }
          this.masterModel.subjectFileNameLoading = false;
        },
        error => {
          this.masterModel.subjectFileNameLoading = false;
        });
  }

  //report in email drop down change
  changeReportInEmail(event) {

    if (event && event.id > 0) {
      this.setBaseOnReportInEmail(event.id);
    }
    this.model.emailSizeId = null;
    this.model.fileNameId = null;
    // this.masterModel.reportInEmaiBaseOnlLoad = event && event.id == ReportSendInEmailAttachment;
  }

  //set value to show dependent drop down
  setBaseOnReportInEmail(id: number) {
    this.masterModel.reportInEmaiBaseOnlLoad = id && id == ReportSendInEmailAttachment;
  }

  //report in email drop down clear
  clearReportInEmail() {
    this.masterModel.reportInEmaiBaseOnlLoad = false;
    this.model.emailSizeId = null;
    this.model.fileNameId = null;
  }

  //get to and cc recipient list
  getToAndCCRecipientList() {
    this.masterModel.toLoading = true;
    this.masterModel.ccLoading = true;
    this.emailConfigService.getToAndCCRecipientList(this.model.typeId)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        data => {
          if (data && data.result == ResponseResult.Success) {

            this.masterModel.recipientList = data.dataSourceList.slice();
            this.masterModel.toList = data.dataSourceList.slice();
            this.masterModel.ccList = data.dataSourceList.slice();
            this.masterModel.fulllRecipientIdList = this.masterModel.recipientList.map(x => x.id);

            //while edit map the cc and to list
            if (this.model.id && this.model.id > 0) {
              this.mapToList(this.model.ccIdList, this.masterModel.toList, this.model.toIdList)
              this.mapCCList(this.model.toIdList, this.masterModel.ccList, this.model.ccIdList);
            }
          }
          this.masterModel.toLoading = false;
          this.masterModel.ccLoading = false;
        },
        error => {
          this.masterModel.toLoading = false;
          this.masterModel.ccLoading = false;
        });
  }

  //remove the selected value from another list(to and cc list)
  removelist(currentids, recipientList, idsList): Array<DataSource> {

    var selectedList: any;

    if (currentids && currentids.length > 0) {
      currentids.forEach(id => {
        let index = recipientList.findIndex(x => x.id == id);

        if (index >= 0)
          recipientList.splice(index, 1);
      });
    }

    //both list has select value append to list
    if (currentids && currentids.length > 0 && idsList && idsList.length > 0) {
      selectedList = currentids.concat(idsList);
    }
    else if ((!currentids || currentids.length <= 0) && idsList && idsList.length > 0) {
      selectedList = idsList;
    }
    else if (currentids && currentids.length > 0 && (!idsList || idsList.length <= 0)) {
      selectedList = currentids;
    }
    else {
      return this.masterModel.recipientList;
    }

    var unselectIds = new Array<number>();

    //if we unselect any item below condidtion will execute
    this.masterModel.fulllRecipientIdList.forEach(element => {
      if (selectedList.findIndex(x => x == element) == -1) {
        unselectIds.push(element);
      }
    });
    unselectIds.forEach(id => {
      let index = recipientList.findIndex(x => x.id == id);

      if (index == -1)
        recipientList.push(this.masterModel.recipientList.find(x => x.id == id));
    });

    return recipientList;
  }

  changeCCList(elementList) {
    var currentids;
    var idsList = this.model.toIdList;

    if (elementList && elementList.length > 0) {
      currentids = elementList.map(x => x.id);
    }
    this.mapToList(currentids, this.masterModel.toList, idsList);
  }

  mapToList(currentids, recipientList, idsList) {
    this.masterModel.toList = this.removelist(currentids, recipientList, idsList);
    this.masterModel.toList = [...this.masterModel.toList];
  }

  changeToList(elementList) {
    var idsList = this.model.ccIdList;
    var currentids;

    if (elementList && elementList.length > 0) {

      currentids = elementList.map(x => x.id);
    }
    this.mapCCList(currentids, this.masterModel.ccList, idsList);
  }

  mapCCList(currentids, recipientList, idsList) {
    this.masterModel.ccList = this.removelist(currentids, recipientList, idsList);
    this.masterModel.ccList = [...this.masterModel.ccList];
  }

  //clear all the value if any to or cc list
  clearRecipientList(recipientList, currentList, idsList, isTo: boolean): DataSource[] {

    idsList.forEach(id => {
      let index = currentList.findIndex(x => x.id == id);

      if (index >= 0)
        currentList.splice(index, 1);

    });

    this.masterModel.fulllRecipientIdList.forEach(id => {
      let index = recipientList.findIndex(x => x.id == id);

      if (index == -1)
        recipientList.push(this.masterModel.recipientList.find(x => x.id == id));

    });

    if (isTo) {
      this.masterModel.toList = currentList;
    }
    else {
      this.masterModel.ccList = currentList;
    }

    this.masterModel.ccList = [...this.masterModel.ccList];

    this.masterModel.toList = [...this.masterModel.toList];

    return recipientList;
  }

  //clear to list values
  clearToList() {
    this.masterModel.ccList = this.masterModel.recipientList;

    var ccIds = this.model.ccIdList.map(x => x);

    var ccList = this.masterModel.ccList;
    //ccIds  - idsList
    this.masterModel.ccList = this.clearRecipientList(ccList, this.masterModel.toList, ccIds, true);
  }

  //clear cc list values
  clearCCList() {
    //assign full list to tolist
    this.masterModel.toList = this.masterModel.recipientList;

    //get selected to id list
    var toIds = this.model.toIdList.map(x => x);

    var toList = this.masterModel.toList;

    this.masterModel.toList = this.clearRecipientList(toList, this.masterModel.ccList, toIds, false);
  }

  clearBrandData() {
    this.getBrandListBySearch();
  }

  clearDeptData() {
    this.getDeptListBySearch();
  }

  clearBuyerData() {
    this.getBuyerListBySearch();
  }

  clearCollectionData() {
    this.getCollectionListBySearch();
  }

  clearCountryData() {
    this.getCountryListBySearch();
  }

  clearFactoryData() {
    this.getFactListBySearch();
  }

  clearSupplierData() {
    this.getSupListBySearch();
  }

  changeService() {
    this.getServiceTypeList();
  }


  getRecipientTypeList() {
    this.masterModel.recipientTypeLoading = true;
    this.emailConfigService.getRecipientTypeList().subscribe(res => {
      if (res && res.result == ResponseResult.Success) {

        this.masterModel.recipientTypeList = res.dataSourceList;
      }
      this.masterModel.recipientTypeLoading = false;
    },
      err => {
        this.masterModel.recipientTypeLoading = false;
      });
  }

  addAdditionalEmailRecipient() {

    if (this.isAdditionalEmailFormValid()) {
      let additionalEmailRecipient = new AdditionalEmailRecipient();
      additionalEmailRecipient.email = this.masterModel.additionalEmailRecipient.email;
      additionalEmailRecipient.recipientId = this.masterModel.additionalEmailRecipient.recipientId;
      additionalEmailRecipient.recipientType = this.masterModel?.recipientTypeList?.find(x => x.id == additionalEmailRecipient.recipientId)?.name;
      this.model.additionalEmailRecipients.push(additionalEmailRecipient);
      this.masterModel.additionalEmailRecipient = new AdditionalEmailRecipient();
    }

  }

  removeAdditionalEmailRecipient(index) {
    this.model.additionalEmailRecipients.splice(index, 1);
  }

  isAdditionalEmailFormValid() {
    if (this.masterModel.additionalEmailRecipient.email === null || this.masterModel.additionalEmailRecipient.email === undefined || this.masterModel.additionalEmailRecipient.email === '') {
      this.showError('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_ADDITIONAL_EMAIL_REQ');
      return false;
    }
    if (this.validator.email(this.masterModel.additionalEmailRecipient.email)) {
      this.showError('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_ADDITIONAL_EMAIL');
      return false;
    }

    const isEmailAlredyExist = this.model.additionalEmailRecipients.some(x => x.email == this.masterModel.additionalEmailRecipient.email);
    if (isEmailAlredyExist) {
      this.showError('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_ADDITIONAL_EMAIL_ALREADYEXIST');
      return false;
    }

    if (this.masterModel.additionalEmailRecipient.recipientId == null || !this.validator.number(this.masterModel.additionalEmailRecipient.recipientId) || this.masterModel.additionalEmailRecipient.recipientId <= 0) {
      this.showError('EMAIL_CONFIGURATION.LBL_TITLE', 'EMAIL_CONFIGURATION.MSG_RECIPIENT_REQ');
      return false;
    }

    return true;
  }

  getInvoiceTypeList() {
    this.masterModel.invoiceTypeLoading = true;
    this.cusService.getInvoiceType()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.masterModel.invoiceTypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.masterModel.invoiceTypeLoading = false;

        },
        error => {
          this.masterModel.invoiceTypeLoading = false;
          this.setError(error);
        });
  }
}
