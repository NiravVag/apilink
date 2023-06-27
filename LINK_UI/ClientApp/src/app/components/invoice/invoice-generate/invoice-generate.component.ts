import { Component, ViewChild, TemplateRef } from '@angular/core';
import { JsonHelper, Validator } from '../../common';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { InvoiceGenerateMasterData, InvoiceGenerateModel, InvoiceSupplierInfo } from 'src/app/_Models/invoice/invoicegenerate.model';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap } from 'rxjs/operators';
import { BehaviorSubject, fromEventPattern, of } from 'rxjs';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { DetailComponent } from '../../common/detail.component';
import { CommonDataSourceRequest, DataSourceResponse, InvoicePreviewFrom, InvoicePreviewRequest } from 'src/app/_Models/common/common.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import { SupplierType, BillPaidBy, Url, APIService, InvoiceType, InvoicePreviewType } from '../../../components/common/static-data-common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { InvoiceBillingTo } from 'src/app/_Models/customer/customer-price-card.model';
import { EditInvoiceService } from 'src/app/_Services/invoice/editinvoice.service';
import { InvoiceBilledAddressResult, InvoiceBilledAddressResponse, InvoiceContactsResponse, InvoiceBilledContactsResult } from '../../../_Models/invoice/editinvoice.model';
import { InvoiceGenerateResponse, InvoiceGenerationGroupBy, InvoiceGenerateResult } from 'src/app/_Models/invoice/invoicegenerate.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InvoiceSummaryService } from 'src/app/_Services/invoice/invoicesummary.service';
import { DataSource } from '@amcharts/amcharts4/core';
import { ServiceType } from 'src/app/_Models/Audit/auditcusreportmodel';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
    selector: 'app-invoice-generate',
    templateUrl: './invoice-generate.component.html'
})
export class InvoiceGenerateComponent extends DetailComponent {

    masterData: InvoiceGenerateMasterData;
    invoiceGenerateModel: InvoiceGenerateModel;
    isInvoiceDataLoading: boolean;
    _billPaidBy = BillPaidBy;
    toggleFormSection: boolean;
    _invoiceGenerationGroupBy = InvoiceGenerationGroupBy;
    invoiceGenerateResponse: InvoiceGenerateResponse;
    public invoicetypeList: Array<any>;
    private _translate: TranslateService;
    private _toastr: ToastrService;
    dialog: NgbModalRef | null;
    invoicePreviewRequest = new InvoicePreviewRequest();
    Url = Url;
    @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;
    invoicePreviewTypes = [InvoicePreviewType.Booking, InvoicePreviewType.Product, InvoicePreviewType.SimpleInvoice, InvoicePreviewType.PO, InvoicePreviewType.Audit];

    constructor(private jsonHelper: JsonHelper,
        public validator: Validator,
        translate: TranslateService,
        toastr: ToastrService,
        public referenceService: ReferenceService,
        router: Router, route: ActivatedRoute,
        private cusService: CustomerService,
        private supService: SupplierService,
        public editInvoiceService: EditInvoiceService,
        private bookingService: BookingService,
        public locationService: LocationService,
        public customerService: CustomerService,
        public utility: UtilityService,
        public customerCheckPointService: CustomerCheckPointService,
        public modalService: NgbModal,
        private service: InvoiceSummaryService,
        public customerPriceCardService: CustomerPriceCardService) {
        super(router, route, translate, toastr);
        this.jsonHelper = validator.jsonHelper;
        this._toastr = toastr;
        this._translate = translate;
    }
    onInit() {
        this.masterData = new InvoiceGenerateMasterData();
        this.invoiceGenerateModel = new InvoiceGenerateModel();
        this.invoiceGenerateModel.invoiceType = InvoiceType.Monthly;
        this.masterData.requestCustomerModel = new CommonDataSourceRequest();
        this.masterData.requestSupModel = new CommonDataSourceRequest();
        this.masterData.requestSupFactoryModel = new CommonDataSourceRequest();
        this.invoiceGenerateModel.isInspection = true;
        this.invoiceGenerateModel.isTravelExpense = true;
        this.invoiceGenerateModel.service = APIService.Inspection;

        this.checkUserHasInvoiceAccess();
        this.getCustomerListBySearch();
        this.getInvoiceRequestList();
        this.getBillingToList();
        this.getServiceList();
        this.getCountryList();
        this.getSplitInvoiceList();
        this.getCurrencyList();
        this.getInvoiceType();
        this.getBillingEntityList();
        this.getServiceTypeList();

        this.validator.isSubmitted = false;
        this.validator.setJSON("invoice/invoice-generate.valid.json");
        this.validator.setModelAsync(() => this.invoiceGenerateModel);
        this.jsonHelper = this.validator.jsonHelper;

        var supplierInfo = new InvoiceSupplierInfo();
        var supplierInfoValidator = { supplierInfo: supplierInfo, validator: Validator.getValidator(supplierInfo, "invoice/invoice-supplier-info.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };
        this.masterData.supplierInfoValidator = supplierInfoValidator;

    }

    getEditPath(): string {
        return "";
    }

    getViewPath(): string {
        return "";
    }

    //fetch the first 10 suppliers for the customer on load
    getCustomerListBySearch() {
        this.masterData.requestCustomerModel.customerId = 0;
        this.masterData.customerInput.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            tap(() => this.masterData.customerLoading = true),
            switchMap(term => term
                ? this.cusService.getCustomerDataSourceList(this.masterData.requestCustomerModel, term)
                : this.cusService.getCustomerDataSourceList(this.masterData.requestCustomerModel)
                    .pipe(
                        catchError(() => of([])), // empty list on error
                        tap(() => this.masterData.customerLoading = false))
            ))
            .subscribe(data => {
                this.masterData.customerList = data;
                this.masterData.customerLoading = false;
            });
    }

    //fetch the customer data with virtual scroll
    getCustomerData(isDefaultLoad: boolean) {
        if (isDefaultLoad) {
            this.masterData.requestCustomerModel.searchText = this.masterData.customerInput.getValue();
            this.masterData.requestCustomerModel.skip = this.masterData.customerList.length;
        }
        this.masterData.customerLoading = true;
        this.cusService.getCustomerDataSourceList(this.masterData.requestCustomerModel).
            subscribe(data => {
                if (data && data.length > 0) {
                    this.masterData.customerList = this.masterData.customerList.concat(data);
                }
                if (isDefaultLoad)
                    this.masterData.requestCustomerModel = new CommonDataSourceRequest();
                this.masterData.customerLoading = false;
            }),
            error => {
                this.masterData.customerLoading = false;
                this.setError(error);
            };
    }

    clearCustomerSelection() {
        this.invoiceGenerateModel.serviceTypes = null;
        this.invoiceGenerateModel.brandIdList = null;
        this.invoiceGenerateModel.departmentIdList = null;
        this.invoiceGenerateModel.buyerIdList = null;
        this.invoiceGenerateModel.customerContacts = null;
        this.masterData.serviceTypeList = null;
        this.masterData.customerBrandList = null;
        this.masterData.customerDepartmentList = null;
        this.masterData.customerBuyerList = null;
        this.masterData.customerContactList = null;

        this.masterData.supplierList = null;
        this.invoiceGenerateModel.supplierList = null;
        this.getCustomerData(true);
        this.resetSupplierInfo();
    }

    //fetch the first 10 suppliers for the customer on load
    getSupListBySearch() {
        this.masterData.requestSupModel.customerId = this.invoiceGenerateModel.customerId;
        this.masterData.supplierList = null;
        this.masterData.requestSupModel.serviceId = this.invoiceGenerateModel.service ?? 0;
        this.masterData.requestSupModel.supplierType = SupplierType.Supplier;
        this.masterData.supplierInput.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            tap(() => this.masterData.supplierLoading = true),
            switchMap(term => term
                ? this.supService.getFactoryDataSourceList(this.masterData.requestSupModel, term)
                : this.supService.getFactoryDataSourceList(this.masterData.requestSupModel)
                    .pipe(
                        catchError(() => of([])), // empty list on error
                        tap(() => this.masterData.supplierLoading = false))
            ))
            .subscribe(data => {
                this.masterData.supplierList = data;
                this.masterData.supplierLoading = false;
            });
    }

    changeService(event) {
        this.invoiceGenerateModel.serviceTypes = [];
        this.getSplitInvoiceList();
        var buyerId = this._invoiceGenerationGroupBy.Buyer;

        if (event) {
            if (event.id == APIService.Audit) {
                this.masterData.splitInvoiceList = this.masterData.splitInvoiceList.filter(x => x.id != buyerId);
            }
            this.resetSupplierInfo();
            this.getServiceTypeList();
        }
        else {
            this.masterData.serviceTypeList = [];
        }
    }

    //fetch the supplier data with virtual scroll
    getSupplierData(isDefaultLoad: boolean) {

        if (isDefaultLoad) {
            this.masterData.requestSupModel.searchText = this.masterData.supplierInput.getValue();
            this.masterData.requestSupModel.skip = this.masterData.supplierList.length;
        }
        this.masterData.requestSupModel.customerId = this.invoiceGenerateModel.customerId;
        this.masterData.requestSupModel.serviceId = this.invoiceGenerateModel.service ?? 0;
        this.masterData.requestSupModel.supplierType = SupplierType.Supplier;
        this.masterData.supplierLoading = true;
        this.supService.getFactoryDataSourceList(this.masterData.requestSupModel).
            subscribe(data => {
                if (data && data.length > 0) {
                    this.masterData.supplierList = this.masterData.supplierList.concat(data);
                }
                if (isDefaultLoad)
                    this.masterData.requestSupModel = new CommonDataSourceRequest();
                this.masterData.supplierLoading = false;
            }),
            error => {
                this.masterData.supplierLoading = false;
                this.setError(error);
            };
    }

    //fetch the first 10 suppliers for the customer on load
    getSupplierFactoryListBySearch() {
        this.masterData.requestSupFactoryModel.customerId = this.invoiceGenerateModel.customerId;
        this.masterData.requestSupFactoryModel.searchText = "";
        this.masterData.requestSupFactoryModel.skip = 0;
        this.masterData.requestSupFactoryModel.serviceId = this.invoiceGenerateModel.service ?? 0;
        
        if (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Supplier) {
            this.masterData.requestSupFactoryModel.supplierType = SupplierType.Supplier;
            this.masterData.supplierHeader = "Supplier";
        }

        else if (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Factory) {
            this.masterData.requestSupFactoryModel.supplierType = SupplierType.Factory;
            this.masterData.supplierHeader = "Factory";
        }





        this.masterData.supplierfactoryInput.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            tap(() => this.masterData.supplierfactoryLoading = true),
            switchMap(term => term
                ? this.supService.getFactoryDataSourceList(this.masterData.requestSupFactoryModel, term)
                : this.supService.getFactoryDataSourceList(this.masterData.requestSupFactoryModel)
                    .pipe(
                        catchError(() => of([])), // empty list on error
                        tap(() => this.masterData.supplierfactoryLoading = false))
            ))
            .subscribe(data => {
                this.masterData.supplierfactoryList = data;
                this.masterData.supplierfactoryLoading = false;
            });
    }

    //fetch the supplier data with virtual scroll
    getSupplierFactoryData(isDefaultLoad: boolean) {
        if (isDefaultLoad) {
            this.masterData.requestSupFactoryModel.searchText = this.masterData.supplierfactoryInput.getValue();
            this.masterData.requestSupFactoryModel.skip = this.masterData.supplierfactoryList.length;
        }
        this.masterData.requestSupFactoryModel.customerId = this.invoiceGenerateModel.customerId;
        if (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Supplier)
            this.masterData.requestSupModel.supplierType = SupplierType.Supplier;
        else if (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Factory)
            this.masterData.requestSupModel.supplierType = SupplierType.Factory;
        this.masterData.supplierfactoryLoading = true;
        this.masterData.requestSupFactoryModel.serviceId = this.invoiceGenerateModel.service ?? 0;
        this.supService.getFactoryDataSourceList(this.masterData.requestSupFactoryModel).
            subscribe(data => {
                if (data && data.length > 0) {
                    this.masterData.supplierfactoryList = this.masterData.supplierfactoryList.concat(data);
                }
                if (isDefaultLoad)
                    this.masterData.requestSupFactoryModel = new CommonDataSourceRequest();
                this.masterData.supplierfactoryLoading = false;
            }),
            error => {
                this.masterData.supplierfactoryLoading = false;
                this.setError(error);
            };
    }

    clearFactorySelection() {
        this.invoiceGenerateModel.supplierInfo.supplierId = null;
    }

    clearSupplierSelection() {
        this.masterData.supplierInfoValidator.supplierInfo.billingAddress = null;
        this.masterData.supplierInfoValidator.supplierInfo.supplierId = null;
        this.masterData.supplierInfoValidator.supplierInfo.factoryId = null;
        this.masterData.supplierInfoValidator.supplierInfo.billedName = null;
        this.masterData.supplierInfoValidator.supplierInfo.contactPersonIdList = null;
        this.masterData.billingAddressList = null;
        this.masterData.billingAddressData = null;
        this.getSupplierFactoryData(true);
    }

    clearBaseSupplierDetails() {
        this.getSupplierData(true);
    }


    //get invoice request type list
    getInvoiceRequestList() {
        this.masterData.invoiceRequestLoading = true;
        this.referenceService.getInvoiceRequestTypeList()
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.invoiceRequestList = response.dataSourceList;
                    this.masterData.invoiceRequestLoading = false;

                },
                error => {
                    this.setError(error);
                    this.masterData.invoiceRequestLoading = false;
                });
    }

    checkUserHasInvoiceAccess() {
        this.masterData.hasInvoiceAccessLoading = true;
        this.referenceService.checkUserHasInvoiceAccess()
            .pipe()
            .subscribe(
                response => {
                    this.masterData.hasInvoiceAccess = response;
                    this.masterData.hasInvoiceAccessLoading = false;

                },
                error => {
                    this.setError(error);
                    this.masterData.hasInvoiceAccessLoading = false;
                });
    }

    //getBillingToList
    getBillingToList() {
        this.masterData.billingToLoading = true;
        this.customerPriceCardService.getBillingToList()
            .pipe()
            .subscribe(
                response => {
                    this.processBillToResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.billingToLoading = false;
                });
    }


    onChangeBillingEntity(event) {
        if (event) {
            this.onClearBillingEntity();
            this.getInvoiceBankList(event.id);
        }
    }

    onClearBillingEntity() {
        this.invoiceGenerateModel.bankAccount = null;
        this.masterData.invoiceBankList = [];

    }

    //process the bill to list response
    processBillToResponse(response: DataSourceResponse) {
        if (response) {
            if (response.result == ResponseResult.Success) {
                this.masterData.billingToList = response.dataSourceList;
            }
            if (response.result == ResponseResult.NoDataFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_BILL_TO_NOT_FOUND');
            }
            this.masterData.billingToLoading = false;
        }
    }

    //get the invoice billed address
    getInvoiceBilledAddress() {
        switch (this.invoiceGenerateModel.invoiceTo) {
            case InvoiceBillingTo.Supplier:
                this.getAddressData(this.invoiceGenerateModel.invoiceTo, this.masterData.supplierInfoValidator.supplierInfo.supplierId);
                break;
            case InvoiceBillingTo.Customer:
                this.getAddressData(this.invoiceGenerateModel.invoiceTo, this.invoiceGenerateModel.customerId);
                break;
            case InvoiceBillingTo.Factory:
                this.getAddressData(this.invoiceGenerateModel.invoiceTo, this.masterData.supplierInfoValidator.supplierInfo.supplierId);
                break;
        }
    }

    //get the address by invoice type
    getAddressData(billToId, searchId) {
        this.masterData.invoiceBilledAddressLoading = true;
        this.editInvoiceService.getInvoiceBillingAddress(billToId, searchId)
            .pipe()
            .subscribe(
                response => {
                    this.processAddressData(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.invoiceBilledAddressLoading = false;
                });
    }

    //process the billed address data
    processAddressData(response: InvoiceBilledAddressResponse) {
        if (response) {
            if (response.result == InvoiceBilledAddressResult.Success) {
                this.masterData.billingAddressList = response.billedAddress;
            }
            else if (response.result == InvoiceBilledAddressResult.AddressNotFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_BILL_TO_ADDRESS_NOT_FOUND');
            }
            this.masterData.invoiceBilledAddressLoading = false;
        }
    }

    //get the invoice contacts
    getInvoiceContacts() {

        switch (this.invoiceGenerateModel.invoiceTo) {
            case InvoiceBillingTo.Supplier:
                this.getContactsData(this.invoiceGenerateModel.invoiceTo, this.masterData.supplierInfoValidator.supplierInfo.supplierId);
                break;
            case InvoiceBillingTo.Customer:
                this.getContactsData(this.invoiceGenerateModel.invoiceTo, this.invoiceGenerateModel.customerId);
                break;
            case InvoiceBillingTo.Factory:
                this.getContactsData(this.invoiceGenerateModel.invoiceTo, this.masterData.supplierInfoValidator.supplierInfo.supplierId);
                break;
        }
    }

    //get the contacts data by invoice type
    getContactsData(billToId, searchId) {
        this.masterData.invoiceContactsLoading = true;
        this.editInvoiceService.getInvoiceContacts(billToId, searchId)
            .pipe()
            .subscribe(
                response => {
                    this.processInvoiceContactsResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.invoiceContactsLoading = false;
                });
    }

    //process the invoice contacts response
    processInvoiceContactsResponse(response: InvoiceContactsResponse) {
        if (response) {
            if (response.result == InvoiceBilledContactsResult.Success) {
                this.masterData.contactsList = response.contacts;
            }
            else if (response.result == InvoiceBilledContactsResult.ContactsNotFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_CONTACTS_NOT_FOUND');
            }
            this.masterData.invoiceContactsLoading = false;
        }
    }

    changeSupplier(event) {
        this.masterData.supplierInfoValidator.supplierInfo.billingAddress = null;
        this.masterData.supplierInfoValidator.supplierInfo.billedName = null;
        this.masterData.supplierInfoValidator.supplierInfo.contactPersonIdList = null;
        this.masterData.billingAddressList = null;
        this.masterData.billingAddressData = null;
        this.masterData.contactsList = null;
        if (event) {
            this.masterData.supplierInfoValidator.supplierInfo.billedName = event.name;
            this.getInvoiceBilledAddress();
            this.getInvoiceContacts();
        }
    }

    //assign the billing address value
    changeBillingAddress(event) {
        if (event)
            this.masterData.supplierInfoValidator.supplierInfo.billingAddress = event.name;
    }

    resetSupplierInfo() {
        // this.masterData.supplierInfoValidator.supplierInfo = new InvoiceSupplierInfo();
        this.masterData.supplierInfoValidator.supplierInfo.billingAddress = null;
        this.masterData.supplierInfoValidator.supplierInfo.supplierId = null;
        this.masterData.supplierInfoValidator.supplierInfo.factoryId = null;
        this.masterData.supplierInfoValidator.supplierInfo.billedName = null;
        this.masterData.supplierInfoValidator.supplierInfo.contactPersonIdList = null;
        this.masterData.billingAddressList = null;
        this.masterData.billingAddressData = null;
        this.masterData.contactsList = null;
        this.masterData.supplierfactoryList = null;
        this.masterData.supplierfactoryInput = new BehaviorSubject('');
        if (this.invoiceGenerateModel.customerId) {

            this.getSupplierFactoryListBySearch()
        }
    }

    mapSupplierInfo() {
        if (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Supplier || this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Factory) {
            this.invoiceGenerateModel.supplierInfo = this.masterData.supplierInfoValidator.supplierInfo;
        }
    }



    toggleSection() {
        this.toggleFormSection = !this.toggleFormSection;
    }

    changeCustomerData() {
        this.invoiceGenerateModel.serviceTypes = null;
        this.invoiceGenerateModel.brandIdList = null;
        this.invoiceGenerateModel.departmentIdList = null;
        this.invoiceGenerateModel.buyerIdList = null;
        this.invoiceGenerateModel.customerContacts = null;
        this.masterData.serviceTypeList = null;
        this.masterData.customerBrandList = null;
        this.masterData.customerDepartmentList = null;
        this.masterData.customerBuyerList = null;
        this.masterData.customerContactList = null;
        this.masterData.supplierfactoryList = null;


        if (this.invoiceGenerateModel.customerId) {
            this.getSupListBySearch();
            this.getCustomerBrands();
            this.getCustomerDepartments();
            this.getCustomerBuyers();
            this.getCustomerContactList();
            this.resetSupplierInfo();
            this.getCustomerProductCategoryList();
        }
        this.getServiceTypeList();
    }

    getSplitInvoiceList() {
        this.masterData.splitInvoiceLoading = true;
        this.masterData.splitInvoiceList = [];
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Supplier,
            "name": "Supplier"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Service,
            "name": "Service"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.ServiceType,
            "name": "Service Type"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Country,
            "name": "Country"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Brand,
            "name": "Brand"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Department,
            "name": "Department"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.Buyer,
            "name": "Buyer"
        });
        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.CustomerContact,
            "name": "Customer Contact"
        });

        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.BookingNo,
            "name": "Booking No"
        });


        this.masterData.splitInvoiceList.push({
            "id": this._invoiceGenerationGroupBy.ProductCategory,
            "name": "Product Category"
        });

        this.masterData.splitInvoiceLoading = false;
    }

    getServiceTypeList() {
        if (this.invoiceGenerateModel.service) {
            this.invoiceGenerateModel.serviceTypes = [];
            this.masterData.serviceTypeLoading = true;
            let request = this.generateServiceTypeRequest();
            this.referenceService.getServiceTypes(request)
                .pipe()
                .subscribe(
                    data => {
                        if (data && data.result == 1) {
                            this.masterData.serviceTypeList = data.serviceTypeList;
                            this.masterData.serviceTypeLoading = false;
                        }
                        else {
                            this.error = data.result;
                            this.masterData.serviceTypeLoading = false;
                        }

                    },
                    error => {
                        this.masterData.serviceTypeLoading = false;
                        this.setError(error);

                    });
        }
        else {
            this.masterData.serviceTypeList = [];
        }
    }

    generateServiceTypeRequest() {
        var serviceTypeRequest = new ServiceTypeRequest();
        serviceTypeRequest.customerId = this.invoiceGenerateModel.customerId ?? 0;
        serviceTypeRequest.serviceId = this.invoiceGenerateModel.service ?? 0;
        //serviceTypeRequest.businessLineId=this.model.businessLine;
        return serviceTypeRequest;
    }

    //get the service list
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

    getCustomerBrands() {
        this.masterData.customerBrandLoading = true;
        this.customerService.getCustomerBrands(this.invoiceGenerateModel.customerId)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.customerBrandList = response.dataSourceList;
                    this.masterData.customerBrandLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.customerBrandLoading = false;
                });
    }

    getCustomerProductCategoryList() {
        this.masterData.productCategoryLoading = true;
        this.customerService.getCustomerProductCategoryList(this.invoiceGenerateModel.customerId)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.productCategoryList = response.dataSourceList;
                    this.masterData.productCategoryLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.productCategoryLoading = false;
                });
    }

    getCustomerProductSubCategoryList() {
        this.masterData.productSubCategoryLoading = true;
        this.masterData.productSubCategoryList = [];
        var productCategory = [];
        if (this.invoiceGenerateModel.productCategoryIdList && this.invoiceGenerateModel.productCategoryIdList.length > 0) {
            productCategory = this.invoiceGenerateModel.productCategoryIdList
        }

        var requestCustomerSubCategory =
        {
            customerId: this.invoiceGenerateModel.customerId,
            productCategory: productCategory
        }

        this.customerService.getCustomerProductSubCategoryList(requestCustomerSubCategory)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.productSubCategoryList = response.dataSourceList;
                    this.masterData.productSubCategoryLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.productSubCategoryLoading = false;
                });
    }

    changeProductCategory(event) {

        this.invoiceGenerateModel.productSubCategoryIdList = [];
        if (event) {
            this.getCustomerProductSubCategoryList();
        }
    }


    getCustomerDepartments() {
        this.masterData.customerDepartmentLoading = true;
        this.customerService.getCustomerDepartments(this.invoiceGenerateModel.customerId)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.customerDepartmentList = response.dataSourceList;
                    this.masterData.customerDepartmentLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.customerDepartmentLoading = false;
                });
    }

    getCustomerBuyers() {
        this.masterData.customerBuyerLoading = true;
        this.customerService.getCustomerBuyers(this.invoiceGenerateModel.customerId)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.customerBuyerList = response.dataSourceList;
                    this.masterData.customerBuyerLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.customerBuyerLoading = false;
                });
    }

    getCustomerContactList() {
        this.masterData.customerContactsLoading = true;
        this.customerService.getCustomerContactList(this.invoiceGenerateModel.customerId)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.customerContactList = response.dataSourceList;
                    this.masterData.customerContactsLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.customerContactsLoading = false;
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
        var isOk = this.validator.isValid('customerId') &&
            this.validator.isValid('invoicingRequest') &&
            this.validator.isValid('realInspectionFromDate') &&
            this.validator.isValid('realInspectionToDate') &&
            this.validator.isValid('invoiceTo') &&
            this.validator.isValid('service') && this.validator.isValid('invoiceType');
        if (isOk && (this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Supplier
            || this.invoiceGenerateModel.invoiceTo == this._billPaidBy.Factory)) {
            isOk = this.masterData.supplierInfoValidator.validator.isValid('supplierId') &&
                this.masterData.supplierInfoValidator.validator.isValid('billedName') &&
                this.masterData.supplierInfoValidator.validator.isValid('billingAddress') &&
                this.masterData.supplierInfoValidator.validator.isValid('contactPersonIdList')
        }
        if (isOk) {
            if (!this.invoiceGenerateModel.isTravelExpense && !this.invoiceGenerateModel.isInspection) {
                this.showWarning("INVOICE_GENERATE.LBL_TITLE", "INVOICE_GENERATE.MSG_TRAVEL_INSPECTION_FEES");
                isOk = false;
            }
        }

        if (isOk) {
            if (this.invoiceGenerateModel.billingEntity > 0) {

                isOk = this.validator.isValid('bankAccount')
            }
        }
        return isOk;
    }

    async generateInvoice() {
        this.validator.initTost();
        this.validator.isSubmitted = true;
        this.masterData.supplierInfoValidator.validator.initTost();
        this.masterData.supplierInfoValidator.validator.isSubmitted = true;

        let response: InvoiceGenerateResponse;
        this.invoiceGenerateModel.bookingNoList = [];
        if (this.invoiceGenerateModel.bookingNo)
            this.invoiceGenerateModel.bookingNoList.push(this.invoiceGenerateModel.bookingNo);

        if (this.isFormValid()) {
            this.mapSupplierInfo();
            try {
                this.masterData.saveDataLoading = true;
                response = await this.editInvoiceService.generateInvoice(this.invoiceGenerateModel);
                this.masterData.saveDataLoading = false;
            }
            catch (e) {
                console.error(e);
                this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                this.masterData.saveDataLoading = false;
            }

            if (response) {
                this.processGenerteInvoiceResponse(response, this.invoiceGenerateModel.customerId);
            }
        }
    }

    processGenerteInvoiceResponse(response: InvoiceGenerateResponse, customerId) {
        this.invoiceGenerateResponse = response;
        switch (response.result) {
            case InvoiceGenerateResult.Success: {
                this.processGenerateInvoiceSuccessResponse(response, customerId);
                break;
            }
            case InvoiceGenerateResult.RequestIsNotValid:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_REQ_NOT_VALID');
                break;
            case InvoiceGenerateResult.NoPricecardRuleFound:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_PRICE_CARD_FOUND');
                break;
            case InvoiceGenerateResult.NoInspectionFound:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_INSPECTION_FOUND');
                break;
            case InvoiceGenerateResult.NoRuleMapped:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_RULE_MAPPED');
                break;
            case InvoiceGenerateResult.FutureDateNotAllowed:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_FUTURE_DATE_NOT_ALLOWED');
                break;
            case InvoiceGenerateResult.FromDateAfterToDate:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_TO_DATE_GREATER_FROM_DATE');
                break;
            case InvoiceGenerateResult.NoSupplierSelected:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_SUPPLIER_SELECTED');
                break;
            case InvoiceGenerateResult.SupplierIsRequired:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_SUPPLIER_REQ_INFO');
                break;
            case InvoiceGenerateResult.TravelOrInspectionRequired:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_TRAVEL_INSPECTION_FEES');
                break;

            case InvoiceGenerateResult.NoInvoiceConfigured:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_INVOICE_CONFIGURED');
                break;

            case InvoiceGenerateResult.BankIsRequired:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_Bank_REQ');
                break;

            case InvoiceGenerateResult.NoInvoiceDataAccess:
                this.showWarning('INVOICE_GENERATE.LBL_TITLE', 'INVOICE_GENERATE.MSG_NO_INVOICE_DATA_ACCESS');
                break;
        }
        this.masterData.saveDataLoading = false;
    }

    processGenerateInvoiceSuccessResponse(response: InvoiceGenerateResponse, customerId) {

        if (this.invoiceGenerateResponse.invoiceData.length == 1) {
            this.masterData.templateVisible = true;
            this.invoicePreviewRequest = new InvoicePreviewRequest();
            this.invoicePreviewRequest.customerId = customerId;
            this.invoicePreviewRequest.invoiceNo = this.invoiceGenerateResponse.invoiceData[0];
            this.invoicePreviewRequest.invoiceData = [this.invoiceGenerateResponse.invoiceData[0]];
            this.invoicePreviewRequest.invoicePreviewTypes = this.invoicePreviewTypes;
            this.invoicePreviewRequest.invoicePreviewFrom = InvoicePreviewFrom.InvoiceGenerate;
            this.invoicePreviewRequest.previewTitle = "Invoice Generated Successfully";
            this.invoicePreviewRequest.service = this.invoiceGenerateModel.service;
        }

        this.dialog = this.modalService.open(this.invoicePreviewTemplate,
            {
                windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
                centered: true, backdrop: 'static'
            });
    }

    navigateInvoiceSummary(invoiceNo) {
        if (this.dialog) {
            this.dialog.dismiss();
            this.dialog = null;
        }
        this.returnwithsearchparam('invoicesummary/invoice-summary', invoiceNo);
    }

    closeInvoicePreview() {
        this.dialog.close()
    }

    //get currency list
    getCurrencyList() {
        this.masterData.currencyLoading = true;
        this.bookingService.GetCurrency()
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.currencyList = response.currencyList;
                    this.masterData.currencyLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.currencyLoading = false;
                });
    }

    getInvoiceType() {
        this.bookingService.getInvoiceType()
            .pipe()
            .subscribe(
                data => {

                    if (data && data.result == 1) {
                        this.invoicetypeList = data.customerSource;
                    }
                    else {
                        this.error = data.result;
                    }

                    this.loading = false;

                },
                error => {
                    this.setError(error);
                    this.loading = false;
                });
    }

    getInvoiceBankList(billingEntity) {
        this.masterData.invoiceBankLoading = true;
        this.referenceService.getInvoiceBankList(billingEntity)
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.invoiceBankList = response.dataSourceList;
                    this.masterData.invoiceBankLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.invoiceBankLoading = false;
                });
    }

    getBillingEntityList() {
        this.masterData.billingEntityLoading = true;
        this.referenceService.getBillingEntityList()
            .pipe()
            .subscribe(
                response => {
                    if (response && response.result == ResponseResult.Success)
                        this.masterData.billingEntityList = response.dataSourceList;
                    this.masterData.billingEntityLoading = false;
                },
                error => {
                    this.setError(error);
                    this.masterData.billingEntityLoading = false;
                });
    }

    redirectToEditInvoice(invoiceNo) {
        let entity: string = this.utility.getEntityName();
        var editPage = entity + "/" + Url.EditInvoice + invoiceNo + "/" + this.invoiceGenerateModel.service;
        window.open(editPage);
    }

    closeInvoiceResponsePopup() {
        this.masterData.templateVisible = false;
        if (this.dialog) {
            this.dialog.dismiss();
            this.dialog = null;
        }
    }

}
