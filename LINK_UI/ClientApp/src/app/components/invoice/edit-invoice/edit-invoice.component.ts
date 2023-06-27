import { Component, TemplateRef, ViewChild } from '@angular/core';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper, Validator } from '../../common';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import {
    EditInvoiceMaster, InvoiceTransactionDetails, InvoiceBookingMoreInfoResult, InvoiceTransactionDetailsResult,
    InvoiceBookingMoreInfo, InvoiceBookingProductResult, DeleteInvoiceDetailResult, InvoiceMoExistsResult, InvoiceBaseDetailResponse, InvoiceTransactionDetailsResponse, InvoiceBilledAddressResponse, InvoiceContactsResponse, NewInvoiceBookingSearchRequest, InvoiceNewBookingResponse, InvoiceNewBookingResult, InvoiceNewBookingDetail
} from '../../../_Models/invoice/editinvoice.model';
import { CustomerPriceCardService } from '../../../_Services/customer/customerpricecard.service';
import { InvoicePreviewFrom, InvoicePreviewRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import {
    InvoiceBaseDetail, InvoiceBaseDetailResult, InvoiceBilledAddressResult, UpdateInvoiceDetail, FeeType,
    InvoiceBilledContactsResult, UpdateInvoiceBaseDetail, UpdateInvoiceDetailRequest, UpdateInvoiceDetailsResponse,
    DeleteInvoiceDetailResponse
} from '../../../_Models/invoice/editinvoice.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import {
    InvoiceRequestType,
    InvoiceFeesFrom, InvoiceBillingTo
} from 'src/app/_Models/customer/customer-price-card.model';

import { DataSourceResponse } from '../../../_Models/common/common.model';
import { first } from 'rxjs/operators';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { APIService, BillingMethod, InvoicePreviewType } from "src/app/components/common/static-data-common"
import { EditInvoiceService } from '../../../_Services/invoice/editinvoice.service';
import { NgbModalRef, NgbModal, NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { UpdateInvoiceDetailResult } from 'src/app/_Models/customer/customercontactsummary.model';
import { InvoiceGenerateModel, InvoiceGenerateResponse, InvoiceGenerateResult, InvoiceSupplierInfo } from 'src/app/_Models/invoice/invoicegenerate.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InvoicePreviewComponent } from '../../shared/invoice-preview/invoice-preview.component';



@Component({
    selector: 'app-edit-invoice',
    templateUrl: './edit-invoice.component.html',
    styleUrls: ['./edit-invoice.component.scss']
})
export class EditInvoiceComponent extends DetailComponent {
    masterData: EditInvoiceMaster;
    invoiceBaseDetail: InvoiceBaseDetail;
    invoiceSupplierOrFactoryInfo: InvoiceSupplierInfo;
    newInvoiceBookingRequest: NewInvoiceBookingSearchRequest;
    invoicetransactionDetails: Array<InvoiceTransactionDetails>;
    updateInvoiceBaseDetail: UpdateInvoiceBaseDetail;
    updateInvoiceDetailRequest: UpdateInvoiceDetailRequest;
    bookingMoreInfoDetail: InvoiceBookingMoreInfo;
    invoiceGenerateModel: InvoiceGenerateModel;
    invoiceGenerateResponse: InvoiceGenerateResponse;
    selectedPageSize;
    invRequestType = InvoiceRequestType;
    invFeesFrom = InvoiceFeesFrom;
    billingMethod = BillingMethod;
    billingTo = InvoiceBillingTo;
    private modelRef: NgbModalRef;
    deleteInvoiceId: number;
    public serviceId: number;
    public columnSpan: number = 15;
    isInvoiceDataLoading: boolean;
    invoiceDataLoadingMsg: string;
    public orderDetails: Array<InvoiceNewBookingDetail>;
    public bookingSearchLoading: boolean = false;
    public noBookingForInvoice: boolean = false;
    public generateInvoiceLoading: boolean = false;
    public _customvalidationforbookid: boolean = false;
    _feeType = FeeType;
    deleteBookingNo: number;
    private _translate: TranslateService;
    private _toastr: ToastrService;
    bookingvalidator: any;

    @ViewChild('invoicePreviewTemplate') invoicePreviewTemplate: TemplateRef<any>;
    invoicePreviewRequest = new InvoicePreviewRequest();
    constructor(private jsonHelper: JsonHelper,
        private bookingJsonHelper: JsonHelper,
        public validator: Validator,
        translate: TranslateService,
        toastr: ToastrService,
        router: Router,
        route: ActivatedRoute,
        private activatedRoute: ActivatedRoute,
        public editInvoiceService: EditInvoiceService,
        public referenceService: ReferenceService,
        public routeCurrent: ActivatedRoute,
        private modalService: NgbModal,
        private bookingService: BookingService,
        public utility: UtilityService,
        public customerPriceCardService: CustomerPriceCardService) {
        super(router, route, translate, toastr);
        this.masterData = new EditInvoiceMaster();
        this.invoiceBaseDetail = new InvoiceBaseDetail();
        this.newInvoiceBookingRequest = new NewInvoiceBookingSearchRequest();
        this.invoiceSupplierOrFactoryInfo = new InvoiceSupplierInfo();
        this._toastr = toastr;
        this._translate = translate;

        this.validator.isSubmitted = false;
        this.validator.setJSON("invoice/edit-invoice-basedetail.valid.json");
        this.validator.setModelAsync(() => this.invoiceBaseDetail);
        this.jsonHelper = this.validator.jsonHelper;
        // new invoice booking request model validation setup


    }

    onInit(invoiceNo: string) {

        let inputparam = this.routeCurrent.snapshot.paramMap;
        if (inputparam && inputparam.get("service")) {
            this.serviceId = Number(inputparam.get("service"));
        }
        this.routeCurrent.queryParams.subscribe(
            params => {
                if (params != null && params['paramParent'] != null) {
                    this.paramParent = params['paramParent'];
                    var paramParent = decodeURI(this.paramParent);
                    var parentObj = JSON.parse(paramParent);
                    if (parentObj) {
                        this.serviceId = parentObj.serviceId;
                    }
                }
            }
        );
        // set column span
        if (this.serviceId == APIService.Inspection) {
            this.columnSpan = 15;
        }
        else {
            this.columnSpan = 11;
        }

        this.initialize(invoiceNo);
    }

    initialize(invoiceNo) {
        /* this.isInvoiceDataLoading = true;
        this.invoiceDataLoadingMsg = this.utility.textTranslate('INVOICE_MODIFY.LBL_INVOICE_DATA_LOADING'); */
        this.getBillingToList();
        this.getInvoiceTypeList();
        this.getBillingMethodList();
        this.getInvoicePaymentStatus();
        this.getInvoicePaymentTermList();
        this.getInvoiceOffice();
        this.getInvoiceBaseDetails(invoiceNo);
        this.getInvoiceTransactionDetails(invoiceNo);
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

    addNewBookingToInvoice(content) {
        this.orderDetails = [];
        this.newInvoiceBookingRequest = new NewInvoiceBookingSearchRequest();
        this.bookingvalidator = Validator.getValidator(this.newInvoiceBookingRequest, "invoice/new-invoice-booking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate)
        this.bookingvalidator.isSubmitted = false;
        if (content != null) {
            this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", backdrop: 'static' });
            this.modelRef.result.then((result) => {

            }, (reason) => {

            });
        }
    }

    //getBillingMethodList
    getBillingMethodList() {
        this.masterData.billingMethodLoading = true;

        this.customerPriceCardService.getBillingMethodList()
            .pipe()
            .subscribe(
                response => {
                    this.processBillingMethodListResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');

                    this.masterData.billingMethodLoading = false;
                });

    }

    //process the billing method list response
    processBillingMethodListResponse(response: DataSourceResponse) {
        if (response) {
            if (response.result == ResponseResult.Success) {
                this.masterData.billingMethodList = response.dataSourceList;
            }
            else if (response.result == ResponseResult.NoDataFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_BILL_METHOD_NOT_FOUND');
            }
            this.masterData.billingMethodLoading = false;
        }
    }

    getInvoiceTypeList() {
        this.masterData.invoiceTypeLoading = true;
        this.bookingService.getInvoiceType()
            .pipe()
            .subscribe(
                data => {

                    if (data && data.result == 1) {
                        this.masterData.invoiceTypeList = data.customerSource;
                    }
                    else {
                        this.error = data.result;
                    }

                    this.masterData.invoiceTypeLoading = false;

                },
                error => {
                    this.masterData.invoiceTypeLoading = false;
                    this.setError(error);
                });
    }

    //get the invoice payment status list
    getInvoicePaymentStatus() {
        this.masterData.invoicePaymentStatusLoading = true;
        this.editInvoiceService.getInvoicePaymentStatus()
            .pipe()
            .subscribe(
                response => {
                    this.processInvoicePaymentStatusResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.invoicePaymentStatusLoading = false;
                });
    }

    //process the invoice payment status reponse
    processInvoicePaymentStatusResponse(response: DataSourceResponse) {
        if (response) {
            if (response.result == ResponseResult.Success) {
                this.masterData.invoicePaymentStatusList = response.dataSourceList;
            }
            else if (response.result == ResponseResult.NoDataFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_STATUS_NOT_FOUND');
            }
            this.masterData.invoicePaymentStatusLoading = false;
        }
    }


    //get the invoice base details
    getInvoiceBaseDetails(invoiceNo) {
        this.isInvoiceDataLoading = true;
        this.invoiceDataLoadingMsg = this.utility.textTranslate('INVOICE_MODIFY.LBL_INVOICE_BASE_DETAILS');
        var response = this.editInvoiceService.getInvoiceBaseDetails(invoiceNo, this.serviceId)
            .pipe()
            .subscribe(
                response => {
                    this.processInvoiceBaseDetailResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                });
    }

    //process the invoice base detail response
    processInvoiceBaseDetailResponse(response: InvoiceBaseDetailResponse) {
        if (response) {
            if (response.result == InvoiceBaseDetailResult.Success) {

                this.isInvoiceDataLoading = false;
                this.invoiceBaseDetail = response.invoiceBaseDetail;
                if (this.invoiceBaseDetail.bankDetails) {
                    this.masterData.bankDetails = this.invoiceBaseDetail.bankDetails.accountName +
                        "(" + this.invoiceBaseDetail.bankDetails.accountCurrency + ")" +
                        "," + this.invoiceBaseDetail.bankDetails.accountNumber +
                        "," + this.invoiceBaseDetail.bankDetails.bankName +
                        "," + this.invoiceBaseDetail.bankDetails.bankAddress
                }
                this.getInvoiceBilledAddress();
                this.getInvoiceContacts();
                this.getInvoiceSupplierInfo();
            }
            else if (response.result == InvoiceBaseDetailResult.NotFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_BASE_DETAILS_NOT_FOUND');
            }
            this.isInvoiceDataLoading = false;
        }
    }

    //get the invoice transaction details
    getInvoiceTransactionDetails(invoiceNo) {
        this.isInvoiceDataLoading = true;
        this.invoiceDataLoadingMsg = this.utility.textTranslate('INVOICE_MODIFY.LBL_INVOICE_DATA_LOADING');
        var response = this.editInvoiceService.getInvoiceTransactionDetails(invoiceNo, this.serviceId)
            .pipe()
            .subscribe(
                response => {
                    this.processInvoiceTransactionDetailResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                });

    }

    getColumnSpanbyService(): number {
        if (this.serviceId == APIService.Inspection) {
            return 15;
        }
        else {
            return 11;
        }
    }

    //process the invoice transaction details response
    processInvoiceTransactionDetailResponse(response: InvoiceTransactionDetailsResponse) {
        if (response) {
            if (response.result == InvoiceTransactionDetailsResult.Success) {
                this.invoicetransactionDetails = response.transactionDetails;
                this.calculateFeesTotal();
                this.isInvoiceDataLoading = false;
            }
            else if (response.result == InvoiceTransactionDetailsResult.DataNotFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_TRANS_DETAIL_NOT_FOUND');
            }
        }
    }

    //get the invoice billed address
    getInvoiceBilledAddress() {
        switch (this.invoiceBaseDetail.billTo) {
            case InvoiceBillingTo.Supplier:
                this.getAddressData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.supplierId);
                break;
            case InvoiceBillingTo.Customer:
                this.getAddressData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.customerId);
                break;
            case InvoiceBillingTo.Factory:
                this.getAddressData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.factoryId);
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

        switch (this.invoiceBaseDetail.billTo) {
            case InvoiceBillingTo.Supplier:
                this.getContactsData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.supplierId);
                break;
            case InvoiceBillingTo.Customer:
                this.getContactsData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.customerId);
                break;
            case InvoiceBillingTo.Factory:
                this.getContactsData(this.invoiceBaseDetail.billTo, this.invoiceBaseDetail.factoryId);
                break;
        }
    }

    //get the invoice contacts
    getInvoiceSupplierInfo() {
        this.invoiceSupplierOrFactoryInfo = new InvoiceSupplierInfo();

        switch (this.invoiceBaseDetail.billTo) {
            case InvoiceBillingTo.Supplier:
                this.invoiceSupplierOrFactoryInfo.billedName = this.invoiceBaseDetail.billedName;
                this.invoiceSupplierOrFactoryInfo.billingAddress = this.invoiceBaseDetail.billedAddress;
                this.invoiceSupplierOrFactoryInfo.contactPersonIdList = this.invoiceBaseDetail.contactIds;
                this.invoiceSupplierOrFactoryInfo.supplierId = this.invoiceBaseDetail.supplierId;
                break;
            case InvoiceBillingTo.Customer:

                break;
            case InvoiceBillingTo.Factory:
                this.invoiceSupplierOrFactoryInfo.billedName = this.invoiceBaseDetail.billedName;
                this.invoiceSupplierOrFactoryInfo.billingAddress = this.invoiceBaseDetail.billedAddress;
                this.invoiceSupplierOrFactoryInfo.contactPersonIdList = this.invoiceBaseDetail.contactIds;
                this.invoiceSupplierOrFactoryInfo.supplierId = this.invoiceBaseDetail.factoryId;
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
                this.masterData.contacts = response.contacts;
            }
            else if (response.result == InvoiceBilledContactsResult.ContactsNotFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_CONTACTS_NOT_FOUND');
            }
            this.masterData.invoiceContactsLoading = false;
        }
    }

    //change the billTo.load the billed adddress and invoice contacts based on the Bill To Selection
    changeBillTo() {
        this.invoiceBaseDetail.billedAddress = null;
        this.invoiceBaseDetail.contactIds = null;
        this.masterData.contacts = null;
        this.masterData.billingAddressList = null;
        this.getInvoiceBilledAddress();
        this.getInvoiceContacts();
    }

    //assign the billing address value
    changeBillingAddress(event) {
        if (event)
            this.invoiceBaseDetail.billedAddress = event.name;
    }

    //assign the payment terms value
    changePaymentTerms(event) {
        if (event) {
            this.invoiceBaseDetail.paymentTerms = event.name;
            this.invoiceBaseDetail.paymentDuration = event.duration;
        }
    }

    //get the invoice office list
    getInvoiceOffice() {
        this.masterData.invoiceOfficeLoading = true;
        this.editInvoiceService.getInvoiceOffice()
            .pipe()
            .subscribe(
                response => {
                    this.processInvoiceOfficeResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.invoiceOfficeLoading = false;
                });
    }

    //process the invoice office response
    processInvoiceOfficeResponse(response: DataSourceResponse) {
        if (response) {
            if (response.result == ResponseResult.Success) {
                this.masterData.invoiceOfficeList = response.dataSourceList;
            }
            else if (response.result == ResponseResult.NoDataFound) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_OFFICE_NOT_FOUND');
            }
            this.masterData.invoiceOfficeLoading = false;
        }
    }

    //get the invoice payment terms list
    getInvoicePaymentTermList() {
        this.masterData.invoicePaymentTermsLoading = true;
        this.referenceService.getInvoicePaymentTypeList()
            .pipe()
            .subscribe(
                response => {
                    this.processInvoicePaymentTermsResponse(response);
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                    this.masterData.invoicePaymentTermsLoading = false;
                });
    }

    //process the invoice payment terms response
    processInvoicePaymentTermsResponse(response: DataSourceResponse) {
        if (response) {
            if (response.result == ResponseResult.Success)
                this.masterData.invoicePaymentTermList = response.dataSourceList;
            else if (response.result = ResponseResult.NoDataFound)
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_PAYMENT_TERMS_NOT_FOUND');
        }
        this.masterData.invoicePaymentTermsLoading = false;
    }

    getEditPath(): string {
        return "";
    }

    getViewPath(): string {
        return "";
    }

    //validate the invoice details before submits to the server
    async isInvoiceDetailValid() {
        var isok = this.validator.isValid('invoiceNo') &&
            this.validator.isValid('invoiceDate') &&
            this.validator.isValid('postDate') &&
            this.validator.isValid('billTo') &&
            this.validator.isValid('billMethod') &&
            this.validator.isValid('billedAddress') &&
            this.validator.isValid('contactIds') &&
            this.validator.isValid('paymentTerms') &&
            this.validator.isValid('paymentDuration') &&
            this.validator.isValid('office') &&
            this.validator.isValid('paymentStatus')
            && this.validator.isValid('invoiceType');
        if (isok && this.invoiceBaseDetail.invoiceNo && this.invoiceBaseDetail.invoiceNo
            != this.invoiceBaseDetail.oldInvoiceNo) {
            var isInvoiceExists = await this.getInvoiceNumberExistsStatus();
            if (isInvoiceExists) {
                isok = false;
                this.showWarning('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_NO_ALREADY_EXISTS');
            }
        }
        return isok;

    }
    clearDateInput(controlName: any) {
        switch (controlName) {
            case "paymentDate": {
                this.invoiceBaseDetail.paymentDate = null;
                break;
            }
        }
    }
    //update the invoice details
    async updateInvoiceDetails() {
        this.validator.initTost();
        this.validator.isSubmitted = true;
        let response: UpdateInvoiceDetailsResponse;
        this.masterData.saveDataLoading = true;
        if (await this.isInvoiceDetailValid()) {

            this.mapUpdateRequest();

            try {
                response = await this.editInvoiceService.save(this.updateInvoiceDetailRequest);
            }
            catch (e) {
                console.error(e);
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
            }

            if (response) {
                this.processUpdateInvoiceDetailsResponse(response);
            }
        }
        this.masterData.saveDataLoading = false;
    }

    //process the update details response
    processUpdateInvoiceDetailsResponse(response: UpdateInvoiceDetailsResponse) {
        switch (response.result) {
            case UpdateInvoiceDetailResult.Success:
                this.showSuccess('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_DETAILS_UPDATED');
                this.initialize(this.invoiceBaseDetail.invoiceNo);
                //if (this.fromSummary)
                //this.returnToSummary();
                // else
                //   this.editData(response.id);
                break;
            case UpdateInvoiceDetailResult.Failure:
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                break;
        }
        this.masterData.saveDataLoading = false;
    }

    //map the update invoice request
    mapUpdateRequest() {
        this.updateInvoiceDetailRequest = new UpdateInvoiceDetailRequest();
        this.updateInvoiceDetailRequest.invoiceBaseDetail = this.mapUpdateInvoiceBaseDetails();
        this.updateInvoiceDetailRequest.invoiceDetails = this.mapUpdateInvoiceTransactionDetails();
    }

    //map the update invoice base detail request
    mapUpdateInvoiceBaseDetails() {
        var invoiceBaseDetail = new UpdateInvoiceBaseDetail();
        invoiceBaseDetail.invoiceNo = this.invoiceBaseDetail.invoiceNo;
        invoiceBaseDetail.invoiceDate = this.invoiceBaseDetail.invoiceDate;
        invoiceBaseDetail.postDate = this.invoiceBaseDetail.postDate;
        invoiceBaseDetail.subject = this.invoiceBaseDetail.subject;
        invoiceBaseDetail.billTo = this.invoiceBaseDetail.billTo;
        invoiceBaseDetail.billedName = this.invoiceBaseDetail.billedName;
        invoiceBaseDetail.billedAddress = this.invoiceBaseDetail.billedAddress;
        invoiceBaseDetail.contactIds = this.invoiceBaseDetail.contactIds;
        invoiceBaseDetail.paymentTerms = this.invoiceBaseDetail.paymentTerms;
        invoiceBaseDetail.paymentDuration = this.invoiceBaseDetail.paymentDuration;
        invoiceBaseDetail.office = this.invoiceBaseDetail.office;
        invoiceBaseDetail.billMethod = this.invoiceBaseDetail.billMethod;
        invoiceBaseDetail.invoicePaymentStatus = this.invoiceBaseDetail.paymentStatus;
        invoiceBaseDetail.invoicePaymentDate = this.invoiceBaseDetail.paymentDate;
        invoiceBaseDetail.totalInvoiceFees = this.masterData.totalInvoiceFees ? (parseFloat(this.masterData.totalInvoiceFees) - this.masterData.totalExtarFeeswithTax) : 0;
        invoiceBaseDetail.totalTaxAmount = this.masterData.totalTaxAmount ? this.masterData.totalTaxAmount : 0;
        invoiceBaseDetail.totalTravelFees = this.masterData.totalTravelFees ? this.masterData.totalTravelFees : 0;
        invoiceBaseDetail.serviceId = this.serviceId;
        return invoiceBaseDetail;
    }

    //map the invoice transaction request
    mapUpdateInvoiceTransactionDetails() {

        var invoiceDetails = new Array<UpdateInvoiceDetail>();
        this.invoicetransactionDetails.forEach(invoice => {
            var invoiceDetail = new UpdateInvoiceDetail();
            invoiceDetail.id = invoice.id;
            invoiceDetail.bookingNo = invoice.bookingNo;
            invoiceDetail.manDays = invoice.manDay;
            invoiceDetail.unitPrice = invoice.unitPrice ? parseFloat(invoice.unitPrice) : 0;
            invoiceDetail.inspectionFees = invoice.inspectionFees ? parseFloat(invoice.inspectionFees) : 0;
            invoiceDetail.travelAirFees = invoice.airCost ? parseFloat(invoice.airCost) : 0;
            invoiceDetail.travelLandFees = invoice.landCost ? parseFloat(invoice.landCost) : 0;
            invoiceDetail.hotelFees = invoice.hotelCost ? parseFloat(invoice.hotelCost) : 0;
            invoiceDetail.travelOtherFees = invoice.travelOtherFees ? parseFloat(invoice.travelOtherFees) : 0;
            invoiceDetail.otherFees = invoice.otherCost ? parseFloat(invoice.otherCost) : 0;
            invoiceDetail.discount = invoice.discount ? parseFloat(invoice.discount) : 0;
            invoiceDetail.remarks = invoice.remarks;
            invoiceDetail.totalInspectionFees = invoice.totalInspectionFees ? invoice.totalInspectionFees : 0;
            invoiceDetail.totalTravelFees = invoice.totalTravelFees ? invoice.totalTravelFees : 0;
            invoiceDetail.totalTaxAmount = invoice.totalTaxAmount ? invoice.totalTaxAmount : 0;
            invoiceDetails.push(invoiceDetail);
        }
        );
        return invoiceDetails;
    }

    returnToSummary() {
        this.return('invoicesummary/invoice-summary');
    }

    //navigate to booking page
    navigateToBooking(bookingNo) {

        var bookingPage = '';
        if (this.serviceId == APIService.Inspection) {
            bookingPage = 'api/inspedit/edit-booking/' + bookingNo;

        }
        else if (this.serviceId == APIService.Audit) {
            bookingPage = 'api/auditedit/edit-audit/' + bookingNo;
        }

        window.open(bookingPage);

    }

    //navigate to quotation page
    navigateToQuotation(quotationNo) {
        var quotationPage = 'api/quotations/edit-quotation/' + quotationNo;
        window.open(quotationPage);
    }

    //get the booking more info popup
    getBookingMoreInfo(bookingMoreInfo, bookingId: number) {

        var response = this.editInvoiceService.getBookingMoreInfo(bookingId)
            .pipe()
            .subscribe(
                response => {
                    if (response) {
                        if (response.result == InvoiceBookingMoreInfoResult.success) {

                            this.bookingMoreInfoDetail = response.invoiceBookingMoreInfo;
                            this.modelRef = this.modalService.open(bookingMoreInfo, { windowClass: "mdModelWidth", backdrop: 'static' });
                        }
                    }
                },
                error => {
                    this.setError(error);
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                });

    }

    //handling the booking expand row
    toggleExpandRow(event, index, rowItem) {
        rowItem.isPlaceHolderVisible = true;
        rowItem.productList = [];
        rowItem.statusList = [];

        let triggerTable = event.target.parentNode.parentNode;
        var firstElem = document.querySelector('[data-expand-id="booking' + index + '"]');
        firstElem.classList.toggle('open');

        triggerTable.classList.toggle('active');

        if (firstElem.classList.contains('open')) {
            //  event.target.innerHTML = '-';
            this.getProductListByBooking(rowItem);
        }
        else {
            // event.target.innerHTML = '+';
            rowItem.isPlaceHolderVisible = false;
        }
    }

    //get the product list by booking
    getProductListByBooking(rowItem) {
        rowItem.isPlaceHolderVisible = true;
        this.editInvoiceService.getInvoiceBookingProducts(rowItem.bookingNo)
            .pipe()
            .subscribe(
                res => {
                    if (res.result == InvoiceBookingProductResult.success) {
                        rowItem.isPlaceHolderVisible = false;
                        this.invoicetransactionDetails.filter(x => x.bookingNo == rowItem.bookingNo)[0].productList = res.invoiceBookingProducts;
                    }
                },
                error => {
                    this.setError(error);
                    rowItem.isPlaceHolderVisible = false;
                    this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
                }
            );
    }

    numberOnly(event): boolean {
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;

    }

    //calculate the total fees
    calculateFeesTotal() {
        this.calculateSubTotalFees();
        this.calculateTaxFees();
        // this.calculateTotalInvoiceFees();
    }

    //calculate the total invoice fees
    calculateTotalInvoiceFees() {
        var totalInvoiceFees: string = "0";

        //calculate the total invoice fees
        var totalInvoiceFees = ((parseFloat(this.masterData.inspectionFeesTotal) + parseFloat(this.masterData.airCostTotal) + parseFloat(this.masterData.landCostTotal) +
            parseFloat(this.masterData.otherTravelCostTotal) + parseFloat(this.masterData.hotelCostTotal) + parseFloat(this.masterData.otherCostTotal) +
            parseFloat(this.masterData.inspectionFeesTaxTotal) + parseFloat(this.masterData.airCostTaxTotal) + parseFloat(this.masterData.landCostTaxTotal) +
            parseFloat(this.masterData.otherTravelCostTaxTotal) + parseFloat(this.masterData.hotelCostTaxTotal) +
            parseFloat(this.masterData.otherCostTaxTotal)) - parseFloat(this.masterData.discountTotal)).toFixed(2);

        this.masterData.totalInvoiceFees = totalInvoiceFees;
        //calcualte the total travel fees
        this.masterData.totalTravelFees = parseFloat(this.masterData.airCostTotal) + parseFloat(this.masterData.landCostTotal) +
            parseFloat(this.masterData.otherTravelCostTotal);
    }

    changeManday(row){
        if(row.manDay==undefined || row.manDay=='' || row.manDay==null)
        {
           row.manDay="0";
        }

        if(row.unitPrice==undefined || row.unitPrice=='' || row.unitPrice==null)
        {
           row.unitPrice="0";
        }

        if(this.invoiceBaseDetail.billMethod==this.billingMethod.ManDay)
        {
            if(row)
            {            
             var inspectioFees=(parseFloat(row.manDay) * parseFloat(row.unitPrice));
             row.inspectionFees=inspectioFees.toString();
             this.calculateFeesTotal();
            } 
        }       
    }
    //calculate the sub total fees
    calculateSubTotalFees() {
        let inspectionFeesTotal: string = "0";
        let airCostTotal: string = "0";
        let landCostTotal: string = "0";
        let otherTravelCostTotal: string = "0";
        let hotelCostTotal: string = "0";
        let otherCostTotal: string = "0";
        let discountTotal: string = "0";
        let extraFeeTotal: string = "0";
        let extraFeeSubTotal: string = "0";
        let extraFeeTax: string = "0";
        this.masterData.totalTravelFees = 0;
        this.masterData.totalInvoiceFees = "0";
        let invoiceamount: number = 0;
        let totalExtraFessAmount: number = 0;
        const formatter = new Intl.NumberFormat('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
        //calculate the invoice fees subtotal for each category
        this.invoicetransactionDetails.forEach(invoice => {

           
            inspectionFeesTotal = (parseFloat(inspectionFeesTotal) + (invoice.inspectionFees ? parseFloat(invoice.inspectionFees) : 0)).toFixed(2);
            airCostTotal = (parseFloat(airCostTotal) + (invoice.airCost ? parseFloat(invoice.airCost) : 0)).toFixed(2);
            landCostTotal = (parseFloat(landCostTotal) + (invoice.landCost ? parseFloat(invoice.landCost) : 0)).toFixed(2);;
            otherTravelCostTotal = (parseFloat(otherTravelCostTotal) + (invoice.travelOtherFees ? parseFloat(invoice.travelOtherFees) : 0)).toFixed(2);;
            hotelCostTotal = (parseFloat(hotelCostTotal) + (invoice.hotelCost ? parseFloat(invoice.hotelCost) : 0)).toFixed(2);;
            otherCostTotal = (parseFloat(otherCostTotal) + (invoice.otherCost ? parseFloat(invoice.otherCost) : 0)).toFixed(2);;
            discountTotal = (parseFloat(discountTotal) + (invoice.discount ? parseFloat(invoice.discount) : 0)).toFixed(2);
            extraFeeTotal = (parseFloat(extraFeeTotal) + (invoice.extraFees ? parseFloat(invoice.extraFees) : 0)).toFixed(2);
            extraFeeSubTotal = (parseFloat(extraFeeSubTotal) + (invoice.extraFeeSubTotal ? parseFloat(invoice.extraFeeSubTotal) : 0)).toFixed(2);
            extraFeeTax = (parseFloat(extraFeeTax) + (invoice.extraFeeTax ? parseFloat(invoice.extraFeeTax) : 0)).toFixed(2);

            // calculate the the total travel fees and total inspection fees for each invoice booking
            invoice.totalTravelFees = parseFloat(invoice.landCost) + parseFloat(invoice.airCost) +
                parseFloat(invoice.travelOtherFees);
            invoice.totalInspectionFees = (parseFloat(invoice.inspectionFees) + parseFloat(invoice.hotelCost) +
                parseFloat(invoice.otherCost) + invoice.totalTravelFees);

            if (this.invoiceBaseDetail.taxValue) {
                invoice.totalTaxAmount = parseFloat((invoice.totalInspectionFees * this.invoiceBaseDetail.taxValue).toString());
                // invoice.totalTaxAmount=invoice.totalInspectionFees*this.invoiceBaseDetail.taxValue;
                invoice.totalInspectionFees = parseFloat((invoice.totalInspectionFees + invoice.totalTaxAmount).toString());

            }

            invoice.totalInspectionFees = invoice.totalInspectionFees - parseFloat(invoice.discount);

            // add extra fees

            this.masterData.totalTravelFees = this.masterData.totalTravelFees + invoice.totalTravelFees;

            let extrafeesTotal = parseFloat(invoice.extraFeeSubTotal) + parseFloat(invoice.extraFeeTax);

            invoiceamount = (invoiceamount + invoice.totalInspectionFees + extrafeesTotal);

            totalExtraFessAmount = totalExtraFessAmount + (parseFloat(invoice.extraFeeSubTotal) + parseFloat(invoice.extraFeeTax));
        });

        this.masterData.totalInvoiceFees = invoiceamount.toFixed(2);

        this.masterData.totalExtarFeeswithTax = totalExtraFessAmount;
        this.masterData.inspectionFeesTotal = inspectionFeesTotal;
        this.masterData.airCostTotal = airCostTotal
        this.masterData.landCostTotal = landCostTotal;
        this.masterData.otherTravelCostTotal = otherTravelCostTotal;
        this.masterData.hotelCostTotal = hotelCostTotal;
        this.masterData.otherCostTotal = otherCostTotal;
        this.masterData.discountTotal = discountTotal;
        this.masterData.extraFeeTotal = extraFeeTotal;
        this.masterData.extraFeeSubTotal = extraFeeSubTotal;
        this.masterData.extraFeeTax = extraFeeTax;
    }

    //calculate the tax value for each fees
    calculateTaxFees() {
        var taxTotalAmount = 0;
        //calculate the subtotal tax value
        var taxValue = this.invoiceBaseDetail.taxValue;
        if (taxValue != undefined) {

            //calculate the tax total for each fees
            var inspectionFeesTaxTotal = (parseFloat(this.masterData.inspectionFeesTotal) * taxValue);
            var airCostTaxTotal = (parseFloat(this.masterData.airCostTotal) * taxValue);
            var landCostTaxTotal = (parseFloat(this.masterData.landCostTotal) * taxValue);
            var otherTravelCostTaxTotal = (parseFloat(this.masterData.otherTravelCostTotal) * taxValue);
            var hotelCostTaxTotal = (parseFloat(this.masterData.hotelCostTotal) * taxValue);
            var otherCostTaxTotal = (parseFloat(this.masterData.otherCostTotal) * taxValue);

            //assign the tax sub total
            this.masterData.inspectionFeesTaxTotal = inspectionFeesTaxTotal.toFixed(2);
            this.masterData.airCostTaxTotal = airCostTaxTotal.toFixed(2);
            this.masterData.landCostTaxTotal = landCostTaxTotal.toFixed(2);
            this.masterData.otherTravelCostTaxTotal = otherTravelCostTaxTotal.toFixed(2);
            this.masterData.hotelCostTaxTotal = hotelCostTaxTotal.toFixed(2);
            this.masterData.otherCostTaxTotal = otherCostTaxTotal.toFixed(2);

            //calculate the total tax amount and assign to it
            taxTotalAmount = taxTotalAmount + inspectionFeesTaxTotal + airCostTaxTotal + landCostTaxTotal +
                otherTravelCostTaxTotal + hotelCostTaxTotal + otherCostTaxTotal;

            this.masterData.totalTaxAmount = taxTotalAmount;
        }
    }

    //remove the invoice detail
    removeInvoiceBookingDetail(invoiceId) {

        this.isInvoiceDataLoading = true;
        this.invoiceDataLoadingMsg = "Deleting the invoice booking detail";
        var invoiceDataLength = this.invoicetransactionDetails.length;
        this.editInvoiceService.removeInvoiceBooking(invoiceId)
            .pipe()
            .subscribe(
                response => {
                    this.processRemoveInvoiceDetailResponse(response, invoiceDataLength);
                },
                error => {
                    this.setError(error);
                    this.isInvoiceDataLoading = false;
                });
        this.modelRef.close();
    }

    processRemoveInvoiceDetailResponse(response: DeleteInvoiceDetailResponse, invoiceDataLength) {
        if (response) {
            if (response.result == DeleteInvoiceDetailResult.DeleteSuccess) {
                this.showSuccess('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_DETAILS_DELETED');
                if (invoiceDataLength == 1) {
                    this.returnToSummary();
                }
                else {
                    this.getInvoiceTransactionDetails(this.invoiceBaseDetail.invoiceNo);
                    this.isInvoiceDataLoading = false;
                }
            }
            else if (response.result == DeleteInvoiceDetailResult.DeleteFailed) {
                this.showError('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_DETAILS_DELETE_FAILED');
                this.isInvoiceDataLoading = false;
            }
        }
    }

    //open the delete confirm popup
    openDeleteInvoiceConfirm(id, bookingNo, content) {

        this.deleteInvoiceId = id;
        this.deleteBookingNo = bookingNo;

        this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

        this.modelRef.result.then((result) => {
        }, (reason) => {
        });

    }

    //get the invoice number exists status
    async getInvoiceNumberExistsStatus(): Promise<boolean> {

        this.isInvoiceDataLoading = true;
        this.invoiceDataLoadingMsg = "Checking Invoice Exists Or Not";
        let response: InvoiceMoExistsResult;
        try {
            response = await this.editInvoiceService.checkInvoiceNumberExist(this.invoiceBaseDetail.invoiceNo);
        }
        catch (e) {
            console.error(e);
            this.isInvoiceDataLoading = false;
            this.showError('INVOICE_MODIFY.LBL_TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        }
        this.isInvoiceDataLoading = false;
        return response.isInvoiceNoExists;

    }

    //check the invoice number exists
    async checkInvoiceExists() {
        if (this.invoiceBaseDetail.invoiceNo) {
            if (this.invoiceBaseDetail.invoiceNo != this.invoiceBaseDetail.oldInvoiceNo) {
                if (await this.getInvoiceNumberExistsStatus()) {
                    this.showWarning('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_NO_ALREADY_EXISTS');
                }
                else {
                    this.showSuccess('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.MSG_INVOICE_NUMBER_NOT_EXISTS');
                }
            }
        }
        else {
            this.showWarning('INVOICE_MODIFY.LBL_TITLE', 'INVOICE_MODIFY.LBL_INVOICE_NO_CANNOT_BE_EMPTY');
        }
    }

    getDecimalValuewithTwoDigits(numArr) {
        if (numArr.length > 1) {
            var afterDot = numArr[1];
            if (afterDot.length > 2) {
                return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
            }
        }
    }

    validateNegativeValue(value) {
        if (parseInt(value) < 0)
            return true;
        return false;
    }

    validateDecimal(item, feeType): void {
        let numArr: Array<string>;
        switch (feeType) {
            case this._feeType.InspectionFees:
                {
                    if (item.inspectionFees) {
                        if (!this.validateNegativeValue(item.inspectionFees)) {
                            numArr = item.inspectionFees.toString().split('.');
                            var inspectionFees = this.getDecimalValuewithTwoDigits(numArr);
                            setTimeout(() => {
                                if (inspectionFees)
                                    item.inspectionFees = inspectionFees;
                            }, 10);
                        }
                        else {
                            setTimeout(() => {
                                item.inspectionFees = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.TravelAirFees:
                {
                    if (item.airCost) {
                        if (!this.validateNegativeValue(item.airCost)) {
                            numArr = item.airCost.toString().split('.');
                            if (numArr.length > 1) {
                                var travelAirFees = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (travelAirFees)
                                        item.airCost = travelAirFees;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.airCost = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.TravelLandFees:
                {
                    if (item.landCost) {
                        if (!this.validateNegativeValue(item.landCost)) {
                            numArr = item.landCost.toString().split('.');
                            if (numArr.length > 1) {
                                var travelAirFees = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (travelAirFees)
                                        item.landCost = travelAirFees;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.landCost = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.OtherTravelFees:
                {
                    if (item.travelOtherFees) {
                        if (!this.validateNegativeValue(item.travelOtherFees)) {
                            numArr = item.travelOtherFees.toString().split('.');
                            if (numArr.length > 1) {
                                var travelOtherFees = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (travelOtherFees)
                                        item.travelOtherFees = travelOtherFees;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.travelOtherFees = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.HotelFees:
                {
                    if (item.hotelCost) {
                        if (!this.validateNegativeValue(item.hotelCost)) {
                            numArr = item.hotelCost.toString().split('.');
                            if (numArr.length > 1) {
                                var travelHotelFees = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (travelHotelFees)
                                        item.hotelCost = travelHotelFees;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.hotelCost = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.OtherFees:
                {
                    if (item.otherCost) {
                        if (!this.validateNegativeValue(item.otherCost)) {
                            numArr = item.otherCost.toString().split('.');
                            if (numArr.length > 1) {
                                var travelOtherCost = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (travelOtherCost)
                                        item.otherCost = travelOtherCost;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.otherCost = null;
                            }, 10);
                        }
                    }
                    break;
                }
            case this._feeType.UnitPrice:
                {
                    if (item.unitPrice) {
                        if (!this.validateNegativeValue(item.unitPrice)) {
                            numArr = item.unitPrice.toString().split('.');
                            if (numArr.length > 1) {
                                var unitPrice = this.getDecimalValuewithTwoDigits(numArr);
                                setTimeout(() => {
                                    if (unitPrice)
                                        item.unitPrice = unitPrice;
                                }, 10);

                            }
                        }
                        else {
                            setTimeout(() => {
                                item.unitPrice = null;
                            }, 10);
                        }
                    }
                    break;
                }

                case this._feeType.Manday:
                  {
                      if (item.manDay) {
                          if (!this.validateNegativeValue(item.manDay)) {
                              numArr = item.manDay.toString().split('.');
                              if (numArr.length > 1) {
                                  var manDay = this.getDecimalValuewithTwoDigits(numArr);
                                  setTimeout(() => {
                                      if (manDay)
                                          item.manDay = manDay;
                                  }, 10);

                              }
                          }
                          else {
                              setTimeout(() => {
                                  item.manDay = null;
                              }, 10);
                          }
                      }
                      break;
                  }

            case this._feeType.Discount:
                {
                    if (!this.validateNegativeValue(item.discount)) {
                        numArr = item.discount.toString().split('.');
                        if (numArr.length > 1) {
                            var discount = this.getDecimalValuewithTwoDigits(numArr);
                            setTimeout(() => {
                                if (discount)
                                    item.discount = discount;
                            }, 10);

                        }
                    }
                    else {
                        setTimeout(() => {
                            item.discount = null;
                        }, 10);
                    }
                    break;
                }
        }
    }

    // validate the new invoice booking search request
    isNewBookingSearchValid() {
        let isOk = this.bookingvalidator.isValid('bookingEndDate') && this.bookingvalidator.isValid('bookingStartDate');

        /* let isOk = !this.bookingNoValidation() &&

            this.bookingvalidator.isValidIf('bookingEndDate',
                this.IsDateValidationRequired())

            &&

            this.bookingvalidator.isValidIf('bookingStartDate', this.IsDateValidationRequired()) */
        return isOk;
    }

    bookingNoValidation() {
        return this._customvalidationforbookid = this.newInvoiceBookingRequest.bookingNumber != null &&
            this.newInvoiceBookingRequest.bookingNumber != undefined &&
            isNaN(Number(this.newInvoiceBookingRequest.bookingNumber));
    }

    IsDateValidationRequired(): boolean {

        let isOk = this.bookingvalidator.isSubmitted &&
            (this.newInvoiceBookingRequest.bookingNumber != null ||
                this.newInvoiceBookingRequest.bookingNumber == undefined) ? true : false;

        if (!(this.newInvoiceBookingRequest.bookingNumber)) {
            if (!this.newInvoiceBookingRequest.bookingStartDate)
                this.bookingvalidator.isValid('bookingStartDate');

            else if (this.newInvoiceBookingRequest.bookingStartDate &&
                !this.newInvoiceBookingRequest.bookingEndDate)
                this.bookingvalidator.isValid('bookingEndDate');
        }
        return isOk;
    }


    async searchNewBookingToInvoice() {

        this.bookingvalidator.isSubmitted = true;
        this.noBookingForInvoice = false;
        if (this.isNewBookingSearchValid()) {
            this.bookingSearchLoading = true;
            this.orderDetails = [];
            let response: InvoiceNewBookingResponse;
            this.newInvoiceBookingRequest.customerId = this.invoiceBaseDetail.customerId;
            if (this.invoiceBaseDetail.billTo == InvoiceBillingTo.Supplier) {
                this.newInvoiceBookingRequest.supplierId = this.invoiceBaseDetail.supplierId;
            }
            if (this.invoiceBaseDetail.billTo == InvoiceBillingTo.Factory) {
                this.newInvoiceBookingRequest.factoryId = this.invoiceBaseDetail.factoryId;
            }

            this.newInvoiceBookingRequest.billedTo = this.invoiceBaseDetail.billTo;
            this.newInvoiceBookingRequest.invoiceType = this.invoiceBaseDetail.invoiceType;
            this.newInvoiceBookingRequest.serviceId = this.serviceId;
            try {
                response = await this.editInvoiceService.getBookingListForNewInvoice(this.newInvoiceBookingRequest);
            }
            catch (e) {
                console.error(e);
                this.showError('Invoice Booking', 'QUOTATION.MSG_UNKNOWN');
                this.bookingSearchLoading = false;
            }

            if (response) {
                switch (response.result) {
                    case InvoiceNewBookingResult.success:
                        this.orderDetails = response.bookingList;
                        break;
                    case InvoiceNewBookingResult.nodata:
                        this.orderDetails = [];
                        this.noBookingForInvoice = true;
                        break;
                }
            }
            this.bookingSearchLoading = false;
        }
    }

    async generateNewBookingInvoice() {

        var slectedItems = this.orderDetails.filter(x => x.isChecked);

        if (slectedItems.length > 0) {
            this.generateInvoiceLoading = true;
            this.buildInvoiceGenerateRequest(slectedItems);

            let response: any;

            try {
                response = await this.editInvoiceService.generateInvoice(this.invoiceGenerateModel);
            }
            catch (e) {
                console.error(e);

                this.showError('INVOICE_GENERATE.LBL_TITLE', 'QUOTATION.MSG_UNKNOWN');
                this.generateInvoiceLoading = false;
            }

            if (response) {
                this.processGenerteInvoiceResponse(response);
            }

            this.generateInvoiceLoading = false;
            this.modelRef.close();
        }
        else {
            this.showError('INVOICE_GENERATE.LBL_TITLE', 'Please select any one booking');
        }
    }

    buildInvoiceGenerateRequest(slectedItems) {
        this.invoiceGenerateModel = new InvoiceGenerateModel();

        let bookingIdList = slectedItems.map(a => a.bookingId);
        let serviceDateList = slectedItems.map(a => this.getAsDateFromString(a.serviceDate));

        var uniqueServiceDate = serviceDateList.filter((v, i, a) => a.indexOf(v) === i);
        var uniquebooking = bookingIdList.filter((v, i, a) => a.indexOf(v) === i);

        var endDate = new Date(Math.max.apply(null, uniqueServiceDate));
        var startDate = new Date(Math.min.apply(null, uniqueServiceDate));

        this.invoiceGenerateModel.realInspectionFromDate =
            new NgbDate(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());

        this.invoiceGenerateModel.realInspectionToDate =
            new NgbDate(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());

        // set request data from the base details
        this.invoiceGenerateModel.bookingNoList = uniquebooking;
        this.invoiceGenerateModel.customerId = this.invoiceBaseDetail.customerId;
        this.invoiceGenerateModel.invoiceTo = this.invoiceBaseDetail.billTo;
        this.invoiceGenerateModel.invoiceType = this.invoiceBaseDetail.invoiceType;
        this.invoiceGenerateModel.currencyId=this.invoiceBaseDetail.invoiceCurrency;
        this.invoiceGenerateModel.exchangeRate=this.invoiceBaseDetail.exchangeRate;
        // update bank Account details
        if (this.invoiceBaseDetail.bankDetails) {
            this.invoiceGenerateModel.bankAccount = this.invoiceBaseDetail.bankDetails.bankId;
        }

        // set supplier or factory info
        this.invoiceGenerateModel.supplierInfo = this.invoiceSupplierOrFactoryInfo;

        this.invoiceGenerateModel.invoicingRequest = this.invoiceBaseDetail.invoicingRequest > 0 ?
            this.invoiceBaseDetail.invoicingRequest : InvoiceRequestType.NotApplicable;

        this.invoiceGenerateModel.invoiceNumber = this.invoiceBaseDetail.oldInvoiceNo;
        this.invoiceGenerateModel.isInspection = this.invoiceBaseDetail.isInspectionFees;
        this.invoiceGenerateModel.isTravelExpense = (this.invoiceBaseDetail.isTravelExpense == null) ? false : this.invoiceBaseDetail.isTravelExpense;
        this.invoiceGenerateModel.isNewBookingInvoice = true;
        this.invoiceGenerateModel.service = this.serviceId;
    }

    getAsDateFromString(dateString) {
        var dateParts = dateString.split("/");
        var dateObject = new Date(+dateParts[2], dateParts[1], +dateParts[0]);
        return dateObject
    }

    processGenerteInvoiceResponse(response: InvoiceGenerateResponse) {
        this.invoiceGenerateResponse = response;
        switch (response.result) {
            case InvoiceGenerateResult.Success:
                this.showSuccess('INVOICE_GENERATE.LBL_TITLE', 'successfully generated');
                this.initialize(this.invoiceBaseDetail.invoiceNo);
                break;
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
        }
    }

    //open the popup
    openTemplatePopUp() {

        var invoiceNo = this.invoiceBaseDetail.invoiceNo;
        if (invoiceNo && invoiceNo != "") {
            this.invoicePreviewRequest = new InvoicePreviewRequest();
            this.invoicePreviewRequest.customerId = this.invoiceBaseDetail.customerId.toString();
            this.invoicePreviewRequest.invoiceNo = invoiceNo;
            this.invoicePreviewRequest.invoicePreviewTypes = [InvoicePreviewType.Booking, InvoicePreviewType.Product, InvoicePreviewType.SimpleInvoice, InvoicePreviewType.PO, InvoicePreviewType.Audit];
            this.invoicePreviewRequest.invoicePreviewFrom = InvoicePreviewFrom.InvoiceSummary;
            this.invoicePreviewRequest.previewTitle = "Invoice Preview";
            this.invoicePreviewRequest.service = APIService.Inspection;


            this.modelRef = this.modalService.open(this.invoicePreviewTemplate,
                {
                    windowClass: "mdModelWidth", ariaLabelledBy: 'modal-basic-title',
                    centered: true, backdrop: 'static'
                });
        }
    }

    closeInvoicePreview() {
        this.modelRef.close();
    }

    numericValidation(event) {
        this.utility.numericValidation(event, 5);
    }
}
