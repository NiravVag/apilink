import { Component, ChangeDetectorRef } from "@angular/core";
import { TCFDocumentDetail, TCFDocumentRequest, Vindication, TCFDocumentUpload, TCFDocumentMaster, TCFDocumentItem, TCFDocumentUploadValidator, TCFDocumentDetailResponse } from "src/app/_Models/tcf/tcfdocument.model";
import { SummaryComponent } from "../../common/summary.component";
import { JsonHelper, Validator } from '../../common';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { TCFMasterModel } from "src/app/_Models/tcf/tcfmastermodel";
import { tcfSearchtypelst, tcfDatetypelst, APIService, PageSizeCommon, Country, UserType } from '../../common/static-data-common';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from "rxjs/operators";
import { CustomerService } from "src/app/_Services/customer/customer.service";
import { SupplierService } from "src/app/_Services/supplier/supplier.service";
import { CustomerProduct } from "src/app/_Services/customer/customerproductsummary.service";
import { LocationService } from "src/app/_Services/location/location.service";
import { of } from "rxjs";
import { BuyerDataSourceRequest, CommonDataSourceRequest, CountryDataSourceRequest, CustomerContactSourceRequest, CustomerDataSourceRequest, ProductCategorySourceRequest, ProductSubCategorySourceRequest, SupplierDataSourceRequest } from "src/app/_Models/common/common.model";
import { TCFService } from "src/app/_Services/tcfdata/tcf.service";
import { TCFDocumentResponse } from 'src/app/_Models/tcf/tcfdocument.model';
import { GenericAPIGETRequest, GenericAPIPOSTRequest } from 'src/app/_Models/genericapi/genericapirequest.model';
import { NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { TCFListRequest, TCFListResponse, userTokenRequest } from "src/app/_Models/tcf/tcflanding.model";
import { TCFDepartmentResponse, TCFDocumentIssuerResponse, TCFDocumentTypeResponse, TCFDownloadDocumentResponse, TCFStageResponse, TCFStandardsResponse, TrafficLightColor } from "src/app/_Models/tcf/tcfcommon.model";
import { AttachmentFile, FileInfo } from "src/app/_Models/fileupload/fileupload";
import { TCFUploadDocumentResponse } from "src/app/_Models/tcf/tcfdetail.model";
import { UtilityService } from "src/app/_Services/common/utility.service";
const config = require("src/assets/appconfig/appconfig.json");

@Component({
	selector: 'app-tcf-document',
	templateUrl: './tcf-document.component.html',
	styleUrls: ['./tcf-document.component.scss']
})

export class TcfDocumentComponent extends SummaryComponent<TCFDocumentRequest>  {

	masterData: TCFMasterModel;
	model: TCFDocumentRequest;
	tcfSearchtypelst: any = tcfSearchtypelst;
	tcfDatetypelst: any = tcfDatetypelst;
	toggleFormSection: boolean;
	tcfDocumentResponse: TCFDocumentResponse;
	pagesizeitems = PageSizeCommon;
	selectedPageSize;
	tcfDocumentDetail: TCFDocumentDetail;
	vindication = Vindication;
	private modelRef: NgbModalRef;
	tcfDocumentUpload: TCFDocumentUpload;
	tcfDocumentMaster: TCFDocumentMaster;
	searchloading: boolean;
	userTokenRequest: userTokenRequest;
	user: any;
	userToken: string;
	requestModel: TCFListRequest;
	tcfList: any;
	trafficLightColor = TrafficLightColor;
	tcfId: number;
	fileInfo: FileInfo;
	public fileAttachments: Array<AttachmentFile>;
	uploadFileExtensions: string;
	uploadLimit: any;
	fileSize: any;
	public jsonHelper: JsonHelper;
	tcfDocumentUploadValidator: TCFDocumentUploadValidator;
	private _translate: TranslateService;
	private _toastr: ToastrService;
	genericGetRequest:GenericAPIGETRequest;
	genericPostRequest:GenericAPIPOSTRequest;

	constructor(private cdr: ChangeDetectorRef, public validator: Validator, router: Router,
		route: ActivatedRoute, translate: TranslateService, private tcfService: TCFService, private modalService: NgbModal,
		toastr: ToastrService, private customerService: CustomerService, private supplierService: SupplierService,
		private customerProductService: CustomerProduct, private locationService: LocationService,  public utility: UtilityService) {

		super(router, validator, route, translate, toastr);

		this.objectInitialization();

		this.fileAttachments = [];

		//get the current user from the local storage
		if (localStorage.getItem('currentUser'))
			this.user = JSON.parse(localStorage.getItem('currentUser'));
		this.requestModel = new TCFListRequest();

		this.jsonHelper = validator.jsonHelper;
		this.uploadFileExtensions = '';
		this._toastr = toastr;
		this._translate = translate;

	}

	objectInitialization() {
		this.masterData = new TCFMasterModel();
		this.model = new TCFDocumentRequest();
		this.tcfDocumentResponse = new TCFDocumentResponse();
		this.tcfDocumentMaster = new TCFDocumentMaster();

		this.tcfDocumentUpload = new TCFDocumentUpload();
		this.fileInfo = new FileInfo();
		this.fileAttachments = new Array<AttachmentFile>();
	}

	//validate the from date and to date
	IsDateValidationRequired(): boolean {
		let isOk = this.validator.isSubmitted && this.model.searchTypeText != null &&
			this.model.searchTypeText.trim() == "" ? true : false;

		if (this.model.searchTypeText == null || this.model.searchTypeText == "") {
			if (!this.model.fromdate)
				this.validator.isValid('fromdate');

			else if (this.model.fromdate && !this.model.todate)
				this.validator.isValid('todate');
		}
		return isOk;
	}

	onInit() {
		this.initialize();
		this.validator.setJSON("tcf/tcf-summary.valid.json");
		this.validator.setModelAsync(() => this.model);
	}

	initialize() {
		this.getCustomerListBySearch();
		this.getSupplierListBySearch();
		this.getBuyerListBySearch();
		this.getProductCategoryListBySearch();
		this.getCountryOriginListBySearch();
		this.getCountryDestinationListBySearch();
		this.genericGetRequest = new GenericAPIGETRequest();
		//this.getProductSubCategoryListBySearch();

		this.getTCFStatusList();
		this.getBuyerDepartmentList();

		this.getIssuerList();
		this.selectedPageSize = PageSizeCommon[0];
		this.model.pageSize = 10;
		this.model.index = 1;
		this.model.searchTypeId = 2;
		this.model.dateTypeId = 10;

	}

	getData() {
		this.GetSearchData();
	}

	async GetSearchData() {
		this.searchTCFListData();
	}

	SearchDetails() {
		this.validator.initTost();
		this.validator.isSubmitted = true;
		if (this.formValid()) {
			this.model.pageSize = this.selectedPageSize;
			this.search();
		}
	}

	//validate the form
	formValid(): boolean {

		let isOk = this.validator.isValidIf('todate', this.IsDateValidationRequired()) &&
			this.validator.isValidIf('fromdate', this.IsDateValidationRequired())

		return isOk;
	}

	getPathDetails(): string {
		return '';
	}

	toggleSection() {
		this.toggleFormSection = !this.toggleFormSection;
	}

	toggleExpandRow(event, index, rowItem) {
		let triggerTable = event.target.parentNode.parentNode;
		var firstElem = document.querySelector('[data-expand-id="tcfDocumentDetail' + index + '"]');
		firstElem.classList.toggle('open');

		triggerTable.classList.toggle('active');

		if (firstElem.classList.contains('open')) {
			event.target.innerHTML = '-';
			this.getTCFDocumentDetail(rowItem);
		} else {
			event.target.innerHTML = '+';
		}
	}

	//get the tcf document detail
	getTCFDocumentDetail(rowItem) {
		rowItem.tcfDocumentDetails = [];
		this.genericGetRequest.requestUrl = config.TCF.documentDetailList + rowItem.tcfId;
		this.genericGetRequest.token = this.userToken;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFDetailResponse(response, rowItem);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.masterData.statusLoading = false;
			});
	}

	//process the tcf detail response
	processTCFDetailResponse(response, rowItem) {
		if (response && response.status && response.status == TCFDocumentDetailResponse.Success) {
			this.mapTCFDocumentDetails(response.data, rowItem);
		}
		else if (response && response.status && response.status == TCFDocumentDetailResponse.NotFound) {
			this.showError('TCF_DOCUMENT.LBL_TITLE', 'TCF_DOCUMENT.MSG_TCF_DOCUMENT_DETAIL_NOT_FOUND');
		}
		else {
			this.showError('TCF_COMMON.LBL_TITLE', 'TCF_COMMON.MSG_TCF_UNKNOWN_ERROR');
		}
	}

	mapTCFDocumentDetails(tcfDocumentDetails, rowItem) {
		if (tcfDocumentDetails && tcfDocumentDetails.length > 0) {
			rowItem.tcfDocumentDetails = [];
			tcfDocumentDetails.forEach(element => {
				var tcfDocumentDetail = new TCFDocumentDetail();
				tcfDocumentDetail.docName = element.docName;
				tcfDocumentDetail.attachmentName = element.attachmentName;
				tcfDocumentDetail.statusOfStandard = element.standardStatus;
				tcfDocumentDetail.docType = element.docType;
				tcfDocumentDetail.docStatus = element.status;
				tcfDocumentDetail.comment = element.comment;
				tcfDocumentDetail.trafficColor = element.trafficColor;
				if (element.receiveDate)
					tcfDocumentDetail.dateReceived = element.receiveDate;
				rowItem.tcfDocumentDetails.push(element);
			});
		}
	}


	// Master Data Load Starts--------------

	//#region Customer Data Loading

	//fetch the first 10 customers on load
	getCustomerListBySearch() {

		this.masterData.requestCustomerModel.serviceId = APIService.Tcf;
		this.masterData.requestCustomerModel.take = 7;
		this.masterData.customerInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.customerLoading = true),
			switchMap(term => term
				? this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel, term)
				: this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel)
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
		this.masterData.requestCustomerModel.serviceId = APIService.Tcf;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestCustomerModel.searchText = this.masterData.customerInput.getValue();
			this.masterData.requestCustomerModel.skip = this.masterData.customerList.length;
		}
		this.masterData.customerLoading = true;
		this.customerService.getCustomerDataSource(this.masterData.requestCustomerModel).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.customerList = this.masterData.customerList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestCustomerModel = new CustomerDataSourceRequest();
				this.masterData.customerLoading = false;
			}),
			error => {
				this.masterData.customerLoading = false;
			};
	}

	changeCustomerData() {

		this.model.customerGLCodes = [];
		if (this.model.customerIds && this.model.customerIds.length > 0) {
			this.masterData.supplierList = [];
			this.masterData.buyerList = [];
			//push the customer glcodes
			this.model.customerIds.forEach(customerId => {
				var customer = this.masterData.customerList.find(x => x.id == customerId);
				if (customer)
					this.model.customerGLCodes.push(customer.glCode);
			});
			this.getSupplierData(true);
			this.getBuyerData(true);
			this.getCustomerContactListBySearch();
		}
	}

	clearCustomerSelection() {
		this.model.customerGLCodes = [];
		if (this.model.customerIds && this.model.customerIds.length > 0) {
			this.model.customerIds.forEach(customerId => {
				var customer = this.masterData.customerList.find(x => x.id == customerId);
				if (customer)
					this.model.customerGLCodes.push(customer.glCode);
			});
		}
		this.masterData.requestCustomerModel = new CustomerDataSourceRequest();
		this.masterData.customerList = [];
		this.model.supplierIds = [];
		this.model.buyerIds = [];
		this.model.customerContactIds = [];
		this.masterData.customerContactList = [];
		this.masterData.supplierList = [];
		this.masterData.buyerList = [];
		this.getCustomerData(false);
		this.getSupplierData(false);
		this.getBuyerData(false);
	}

	//#endregion


	//#region Supplier Data Loading

	//fetch the first 10 customers on load
	getSupplierListBySearch() {
		this.masterData.requestSupplierModel.serviceId = APIService.Tcf;
		if (this.model.customerIds)
			this.masterData.requestSupplierModel.customerIds = this.model.customerIds;
		this.masterData.supplierInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.supplierLoading = true),
			switchMap(term => term
				? this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf, term)
				: this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.supplierLoading = false))
			))
			.subscribe(data => {
				this.masterData.supplierList = data;
				this.masterData.supplierLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getSupplierData(isDefaultLoad: boolean) {
		this.masterData.requestSupplierModel.serviceId = APIService.Tcf;
		if (this.model.customerIds)
			this.masterData.requestSupplierModel.customerIds = this.model.customerIds;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestSupplierModel.searchText = this.masterData.supplierInput.getValue();
			this.masterData.requestSupplierModel.skip = this.masterData.supplierList.length;
		}
		this.masterData.supplierLoading = true;
		this.supplierService.getSupplierDataSource(this.masterData.requestSupplierModel, APIService.Tcf).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.supplierList = this.masterData.supplierList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestSupplierModel = new SupplierDataSourceRequest();
				this.masterData.supplierLoading = false;
			}),
			error => {
				this.masterData.supplierLoading = false;
			};
	}

	clearSupplierSelection() {

		this.masterData.requestSupplierModel = new SupplierDataSourceRequest();
		this.masterData.supplierList = [];
		this.getSupplierData(false);
	}

	//#endregion


	//#region Buyer Data Loading

	//fetch the first 10 buyers on load
	getBuyerListBySearch() {

		if (this.model.customerIds)
			this.masterData.requestBuyerModel.customerIds = this.model.customerIds;
		this.masterData.buyerInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.buyerLoading = true),
			switchMap(term => term
				? this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf, term)
				: this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.buyerLoading = false))
			))
			.subscribe(data => {
				this.masterData.buyerList = data;
				this.masterData.buyerLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getBuyerData(isDefaultLoad: boolean) {
		if (this.model.customerIds)
			this.masterData.requestBuyerModel.customerIds = this.model.customerIds;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestBuyerModel.searchText = this.masterData.buyerInput.getValue();
			this.masterData.requestBuyerModel.skip = this.masterData.buyerList.length;
		}
		this.masterData.buyerLoading = true;
		this.customerService.getCustomerBuyerDataSource(this.masterData.requestBuyerModel, APIService.Tcf).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.buyerList = this.masterData.buyerList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestBuyerModel = new BuyerDataSourceRequest();
				this.masterData.buyerLoading = false;
			}),
			error => {
				this.masterData.buyerLoading = false;
			};
	}

	clearBuyerSelection() {
		this.masterData.requestBuyerModel = new BuyerDataSourceRequest();
		this.masterData.buyerList = [];
		this.getBuyerData(false);
	}

	//#endregion


	//#region Customer Contact Loading

	//fetch the first 10 buyers on load
	getCustomerContactListBySearch() {

		if (this.model.customerIds)
			this.masterData.requestCustomerContactModel.customerIds = this.model.customerIds;
		this.masterData.customerContactInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.customerContactLoading = true),
			switchMap(term => term
				? this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf, term)
				: this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.customerContactLoading = false))
			))
			.subscribe(data => {
				this.masterData.customerContactList = data;
				this.masterData.customerContactLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getCustomerContactData(isDefaultLoad: boolean) {
		if (this.model.customerIds)
			this.masterData.requestCustomerContactModel.customerIds = this.model.customerIds;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestCustomerContactModel.searchText = this.masterData.customerContactInput.getValue();
			this.masterData.requestCustomerContactModel.skip = this.masterData.customerContactList.length;
		}
		this.masterData.customerContactLoading = true;
		this.customerService.getCustomerContactDataSource(this.masterData.requestCustomerContactModel, APIService.Tcf).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.customerContactList = this.masterData.customerContactList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestCustomerContactModel = new CustomerContactSourceRequest();
				this.masterData.customerContactLoading = false;
			}),
			error => {
				this.masterData.customerContactLoading = false;
			};
	}

	clearCustomerContactSelection() {
		this.masterData.requestCustomerContactModel = new CustomerContactSourceRequest();
		this.masterData.customerContactList = [];
		this.getCustomerContactData(false);
	}

	//#endregion


	//#region ProductCategory Loading

	//fetch the first 10 buyers on load
	getProductCategoryListBySearch() {

		this.masterData.requestProductCategoryModel.serviceId = APIService.Tcf;
		this.masterData.productCategoryInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.productCategoryLoading = true),
			switchMap(term => term
				? this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel, term)
				: this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.productCategoryLoading = false))
			))
			.subscribe(data => {
				this.masterData.productCategoryList = data;
				this.masterData.productCategoryLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getProductCategoryData(isDefaultLoad: boolean) {
		this.masterData.requestProductCategoryModel.serviceId = APIService.Tcf;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestProductCategoryModel.searchText = this.masterData.productCategoryInput.getValue();
			this.masterData.requestProductCategoryModel.skip = this.masterData.productCategoryList.length;
		}
		this.masterData.productCategoryLoading = true;
		this.customerProductService.getProductCategoryDataSource(this.masterData.requestProductCategoryModel).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.productCategoryList = this.masterData.productCategoryList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestProductCategoryModel = new ProductCategorySourceRequest();
				this.masterData.productCategoryLoading = false;
			}),
			error => {
				this.masterData.productCategoryLoading = false;
			};
	}

	changeProductCategoryData() {

		if (this.model.productCategoryIds) {
			this.masterData.productSubCategoryList = [];

			this.getProductSubCategoryData(true);
		}
	}

	clearProductCategorySelection() {
		this.masterData.requestProductCategoryModel = new ProductCategorySourceRequest();
		this.masterData.productCategoryList = [];
		this.getProductCategoryData(false);
	}

	//#endregion


	//#region ProductSubCategory Loading
	//fetch the first 10 buyers on load
	getProductSubCategoryListBySearch() {

		this.masterData.requestProductSubCategoryModel.serviceId = APIService.Tcf;
		if (this.model.productCategoryIds)
			this.masterData.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
		this.masterData.productSubCategoryInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.productSubCategoryLoading = true),
			switchMap(term => term
				? this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel, term)
				: this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.productSubCategoryLoading = false))
			))
			.subscribe(data => {
				this.masterData.productSubCategoryList = data;
				this.masterData.productSubCategoryLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getProductSubCategoryData(isDefaultLoad: boolean) {
		this.masterData.requestProductSubCategoryModel.serviceId = APIService.Tcf;
		if (this.model.productCategoryIds)
			this.masterData.requestProductSubCategoryModel.productCategoryIds = this.model.productCategoryIds;
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestProductSubCategoryModel.searchText = this.masterData.productSubCategoryInput.getValue();
			this.masterData.requestProductSubCategoryModel.skip = this.masterData.productSubCategoryList.length;
		}
		this.masterData.productSubCategoryLoading = true;
		this.customerProductService.getProductSubCategoryDataSource(this.masterData.requestProductSubCategoryModel).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.productSubCategoryList = this.masterData.productSubCategoryList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
				this.masterData.productSubCategoryLoading = false;
			}),
			error => {
				this.masterData.productSubCategoryLoading = false;
			};
	}

	clearProductSubCategorySelection() {
		this.masterData.requestProductSubCategoryModel = new ProductSubCategorySourceRequest();
		this.masterData.productSubCategoryList = [];
		this.getProductSubCategoryData(false);
	}

	//#endregion

	//#region Country Origin

	//fetch the first 10 buyers on load
	getCountryOriginListBySearch() {

		this.masterData.countryOriginInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.countryOriginLoading = true),
			switchMap(term => term
				? this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel, term)
				: this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.countryOriginLoading = false))
			))
			.subscribe(data => {
				this.masterData.countryOriginList = data;
				this.masterData.countryOriginLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getCountryOriginData(isDefaultLoad: boolean) {
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestCountryModel.searchText = this.masterData.countryOriginInput.getValue();
			this.masterData.requestCountryModel.skip = this.masterData.countryOriginList.length;
		}
		this.masterData.countryOriginLoading = true;
		this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.countryOriginList = this.masterData.countryOriginList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestCountryModel = new CountryDataSourceRequest();
				this.masterData.countryOriginLoading = false;
			}),
			error => {
				this.masterData.countryOriginLoading = false;
			};
	}

	clearCountryOriginSelection() {
		this.masterData.requestCountryModel = new CountryDataSourceRequest();
		this.masterData.countryOriginList = [];
		this.getCountryOriginData(false);
	}

	//#endregion

	//#region Country Destination
	//fetch the first 10 buyers on load
	getCountryDestinationListBySearch() {

		this.masterData.countryDestinationInput.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			tap(() => this.masterData.countryDestinationLoading = true),
			switchMap(term => term
				? this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel, term)
				: this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel, term)
					.pipe(
						catchError(() => of([])), // empty list on error
						tap(() => this.masterData.countryDestinationLoading = false))
			))
			.subscribe(data => {
				this.masterData.countryDestinationList = data;
				this.masterData.countryDestinationLoading = false;
			});
	}

	//fetch the customer data with virtual scroll
	getCountryDestinationData(isDefaultLoad: boolean) {
		//this.masterData.requestCustomerModel.take = 2;
		if (isDefaultLoad) {
			this.masterData.requestCountryModel.searchText = this.masterData.countryDestinationInput.getValue();
			this.masterData.requestCountryModel.skip = this.masterData.countryDestinationList.length;
		}
		this.masterData.countryDestinationLoading = true;
		this.locationService.getCountryDataSourceList(this.masterData.requestCountryModel).
			subscribe(data => {
				if (data && data.length > 0) {
					this.masterData.countryDestinationList = this.masterData.countryDestinationList.concat(data);
				}
				if (isDefaultLoad)
					this.masterData.requestCountryModel = new CountryDataSourceRequest();
				this.masterData.countryDestinationLoading = false;
			}),
			error => {
				this.masterData.countryOriginLoading = false;
			};
	}

	clearCountryDestinationSelection() {
		this.masterData.requestCountryModel = new CountryDataSourceRequest();
		this.masterData.countryDestinationList = [];
		this.getCountryDestinationData(false);
	}

	//#endregion


	//get the tcf status list
	getTCFStatusList() {
		this.genericGetRequest.requestUrl = config.TCF.tcfStatusList;
		this.masterData.statusLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(data => {
			this.processTCFStatusResponse(data);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.masterData.statusLoading = false;
			});
	}

	//process the tcf status response
	processTCFStatusResponse(data) {
		if (data && data.status && data.status == TCFStageResponse.Success) {
			this.masterData.statusList = data.data;
			this.masterData.statusLoading = false;
		}
		else if (data && data.status && data.status == TCFStageResponse.NotFound) {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_STATUS_NOT_FOUND');
			this.masterData.statusLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.masterData.statusLoading = false;
		}
	}

	//get the tcf buyer department list
	getBuyerDepartmentList() {

		this.masterData.buyerdepartmentLoading = true;
		this.genericGetRequest.requestUrl = config.TCF.tcfDepartmentList;

		this.tcfService.getData(this.genericGetRequest).subscribe(data => {
			this.processBuyerDepartmentResponse(data);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.masterData.buyerdepartmentLoading = false;
			});
	}

	//process the buyer department response
	processBuyerDepartmentResponse(data) {
		if (data && data.status && data.status == TCFDepartmentResponse.Success) {
			this.masterData.buyerDepartmentList = data.data;
			this.masterData.buyerdepartmentLoading = false;
		}
		else if (data && data.status && data.status == TCFDepartmentResponse.NotFound) {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.LBL_TCF_BUYER_DEPT_NOT_FOUND');
			this.masterData.buyerdepartmentLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.masterData.buyerdepartmentLoading = false;
		}
	}

	//open tcf document modal popup
	openModalPopup(documentpopup, item) {
		if (item) {
			this.tcfId = item.tcfId;
			var tcfDocumentUpload = new TCFDocumentUpload();
			tcfDocumentUpload.tcfName = item.documentName;
			this.tcfDocumentUploadValidator = new TCFDocumentUploadValidator();
			this.tcfDocumentUploadValidator.tcfDocumentUpload = tcfDocumentUpload;
			this.tcfDocumentUploadValidator.documentValidator = Validator.getValidator(tcfDocumentUpload, "tcf/tcf-detail.valid.json", this.jsonHelper, false, this._toastr, this._translate);
			this.getTCFStandardList(item.tcfId);
			this.getTCFTypeList();
			this.getIssuerList();
		}
		this.modelRef = this.modalService.open(documentpopup, { windowClass: "mdModelWidth", centered: true });
		this.modelRef.result.then((result) => {
		}, (reason) => {
		});
	}

	//get the TCF Standard List
	getTCFStandardList(tcfId) {
		this.genericGetRequest.requestUrl = config.TCF.standardList + tcfId;
		this.genericGetRequest.token = config.TCF.masterToken;
		this.tcfDocumentMaster.standardListLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFStandardListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDocumentMaster.standardListLoading = false;
				this.tcfDocumentMaster.isDetailLoader = false;
			});
	}

	//process the TCF Standard List Response
	processTCFStandardListResponse(response) {
		if (response && response.status && response.status == TCFStandardsResponse.Success) {
			this.tcfDocumentMaster.standardList = response.data;
			this.tcfDocumentMaster.standardListLoading = false;
		}
		else if (response && response.status && response.status == TCFStandardsResponse.NotFound) {
			this.showWarning('TCF_LIST.LBL_ERROR', 'No Standard Defined');
			this.tcfDocumentMaster.standardListLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDocumentMaster.standardListLoading = false;
		}
	}

	//get the TCF Type List Master Data
	getTCFTypeList() {
		this.genericGetRequest.requestUrl = config.TCF.typeList;
		this.genericGetRequest.token = config.TCF.masterToken;
		this.tcfDocumentMaster.typeLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFTypeListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDocumentMaster.typeLoading = false;
			});
	}

	//process the tcf type list response
	processTCFTypeListResponse(response) {
		if (response && response.status && response.status == TCFDocumentTypeResponse.Success) {
			this.tcfDocumentMaster.typeList = response.data;
			this.tcfDocumentMaster.typeLoading = false;
		}
		else if (response && response.status && response.status == TCFDocumentTypeResponse.NotFound) {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_ERROR');
			this.tcfDocumentMaster.typeLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDocumentMaster.typeLoading = false;
		}
	}

	//get the issuer list
	getIssuerList() {
		this.genericGetRequest.requestUrl = config.TCF.issuerList;
		this.genericGetRequest.token = config.TCF.masterToken;
		this.tcfDocumentMaster.issuerListLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processIssuerListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDocumentMaster.issuerListLoading = false;
				this.tcfDocumentMaster.isDetailLoader = false;
			});
	}

	//process the issuer list response
	processIssuerListResponse(response) {
		if (response && response.status && response.status == TCFDocumentIssuerResponse.Success) {
			this.tcfDocumentMaster.issuerList = response.data;
			this.tcfDocumentMaster.issuerListLoading = false;
		}
		else if (response && response.status && response.status == TCFDocumentIssuerResponse.NotFound) {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_ERROR');
			this.tcfDocumentMaster.issuerListLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDocumentMaster.issuerListLoading = false;
		}
	}

	//remove the file from the attachment
	removeAttachment(index) {
		this.fileAttachments.splice(index, 1);
		this.fileAttachments = [];
	}

	//validate the form
	isUploadFormValid() {

		var isOk = this.tcfDocumentUploadValidator.documentValidator.isValid('standardIds')
			&& this.tcfDocumentUploadValidator.documentValidator.isValid('issueDate');

		if (isOk) {
			if (this.fileAttachments.length == 0) {
				isOk = false;
				this.showWarning('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_FILES_REQUIRED');
			}
		}
		return isOk;
	}

	//upload the TCF Document
	uploadTCFDocument() {
		this.tcfDocumentUploadValidator.documentValidator.initTost();
		this.tcfDocumentUploadValidator.documentValidator.isSubmitted = true;

		if (this.isUploadFormValid()) {

			this.tcfDocumentUploadValidator.tcfDocumentUpload.tcfId = this.tcfId;
			this.tcfDocumentMaster.uploadTCFDocumentLoading = true;
			this.tcfService.uploadTCFDocument(this.tcfDocumentUploadValidator.tcfDocumentUpload, this.fileAttachments, this.userToken).subscribe(response => {
				this.processUploadTCFDocument(response);
			},
				error => {
					this.showError('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
				});

		}

	}

	reset() {
		this.model = new TCFDocumentRequest();
		this.tcfDocumentResponse.tcfDocumentList = [];
		this.initialize();
	}

	//attach files in the file attachment data list
	selectFiles(event) {

		if (event && !event.error && event.files) {
			this.fileAttachments = [];
			if (event.files.length > this.fileInfo.uploadLimit) {
				this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
			}
			else {

				for (let file of event.files) {
					var guid = this.newUniqueId();
					let fileItem: AttachmentFile = {
						fileName: file.name,
						file: file,
						fileSize:file.fileSize,
						isNew: true,
						id: 0,
						status: 0,
						mimeType: file.type,
						fileUrl: "",
						uniqueld: guid,
						isSelected: false,
						fileDescription:null
					};
					this.fileAttachments.push(fileItem);
				}
			}
		}
		else if (event && event.error && event.errorMessage) {
			this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
		}
	}

	newUniqueId() {
		return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
	}

	//process the upload tcf document response
	processUploadTCFDocument(response) {
		if (response && response.status) {
			if (response && response.status && response.status == TCFUploadDocumentResponse.Success) {
				this.showSuccess('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_UPLOAD_SUCCESS');
				this.resetUploadData();
				this.tcfDocumentMaster.uploadTCFDocumentLoading = false;
				this.modelRef.close();
			}
			else if (response && response.status && response.status == TCFUploadDocumentResponse.Failure) {
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
				this.tcfDocumentMaster.uploadTCFDocumentLoading = false;
				this.modelRef.close();
			}
		}
		else {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
			this.tcfDocumentMaster.uploadTCFDocumentLoading = false;
			this.modelRef.close();
		}
		this.validator.isSubmitted = false;
	}

	//reset the upload data
	resetUploadData() {
		this.tcfDocumentUpload = new TCFDocumentUpload();
		this.validator.isSubmitted = false;
		this.fileAttachments = [];
	}

	searchTCFListData() {
		//make the user token request
		this.generateUserTokenRequest();
		//make the user token post request
		var request = this.generatePostRequest(config.TCF.userAuthentication, this.userTokenRequest, config.TCF.masterToken);
		this.getTCFListDataWithUserToken(request);
	}

	//make the user token request
	generateUserTokenRequest() {
		this.userTokenRequest = new userTokenRequest();
		this.userTokenRequest.userId = this.user.id;
		switch (this.user.usertype) {
			case UserType.InternalUser:
				this.userTokenRequest.userType = UserType[UserType.InternalUser];
				break;
			case UserType.Customer:
				this.userTokenRequest.userType = UserType[UserType.Customer];
				break;
			case UserType.Supplier:
				this.userTokenRequest.userType = UserType[UserType.Supplier];
				break;
		}
	}

	//make the post request structure with required inputs
	generatePostRequest(requestUrl, requestInput, token) {
		var request = new GenericAPIPOSTRequest();
		request.requestUrl = requestUrl;
		request.requestData = requestInput;
		request.token = token;
		return request;
	}

	getTCFListDataWithUserToken(request) {
		this.searchloading = true;
		this.tcfService.postData(request).subscribe(data => {
			this.processUserTokenResponse(data);
		},
			error => {
				this.showError('TCF_LIST.TITLE', 'TCF_LIST.MSG_UNKNOWN_ERROR');
				this.searchloading = false;
			});
	}

	processUserTokenResponse(data) {
		if (data && data.result) {
			//parse ther response result to json
			var resultData = JSON.parse(data.result);
			//if response result is success and then assign the user token(this.userToken will be used for all subsequent requests)
			if (resultData && resultData.status && resultData.status == "Success"
				&& resultData.data && resultData.data.authorization) {

				this.userToken = resultData.data.authorization;
				this.getTCFSearchData();

			}
			else {
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
			}
			this.searchloading = false;
		}
		else {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
			this.searchloading = false;
		}
	}

	SetSearchTypemodel(searchtype) {
		this.model.searchTypeId = searchtype;
	}

	//assign the tcf list request data
	mapTCFListRequest() {
		if (this.model.searchTypeId)
			this.requestModel.searchTypeId = this.model.searchTypeId;
		if (this.model.searchTypeText)
			this.requestModel.searchTypeText = this.model.searchTypeText;
		if (this.model.statusIds)
			this.requestModel.statusIds = this.model.statusIds;
		if (this.model.customerGLCodes)
			this.requestModel.customerIds = this.model.customerGLCodes;
		if (this.model.supplierIds)
			this.requestModel.supplierIds = this.model.supplierIds;
		if (this.model.buyerIds)
			this.requestModel.buyerIds = this.model.buyerIds;
		if (this.model.customerContactIds)
			this.requestModel.customerContactIds = this.model.customerContactIds;
		if (this.model.buyerDepartmentIds)
			this.requestModel.buyerDepartmentIds = this.model.buyerDepartmentIds;
		if (this.model.productCategoryIds)
			this.requestModel.productCategoryIds = this.model.productCategoryIds;
		if (this.model.productSubCategoryIds)
			this.requestModel.productSubCategoryIds = this.model.productSubCategoryIds;
		if (this.model.countryOriginIds)
			this.requestModel.countryOriginIds = this.model.countryOriginIds;
		if (this.model.countryDestinationIds)
			this.requestModel.countryDestinationIds = this.model.countryDestinationIds;
		if (this.model.dateTypeId) {
			if (this.model.dateTypeId == 7)
				this.requestModel.dateTypeId = 1;
			else if (this.model.dateTypeId == 8)
				this.requestModel.dateTypeId = 2;
			else if (this.model.dateTypeId == 9)
				this.requestModel.dateTypeId = 3;
			else if (this.model.dateTypeId == 10)
				this.requestModel.dateTypeId = 4;
		}
		if (this.model.fromdate)
			this.requestModel.fromDate = this.formatTCFDate(this.model.fromdate);
		if (this.model.todate)
			this.requestModel.toDate = this.formatTCFDate(this.model.todate);
		if (this.model.pictureUploaded)
			this.requestModel.pictureUploaded = this.model.pictureUploaded;
		if (this.model.pageSize)
			this.requestModel.pageSize = this.model.pageSize;
		if (this.model.index)
			this.requestModel.index = this.model.index;
	}

	formatTCFDate(dateValue) {
		return dateValue.year + "-" + dateValue.month + "-" + dateValue.day;
	}

	//get the tcf search data
	getTCFSearchData() {
		this.mapTCFListRequest();
		var request = this.generatePostRequest(config.TCF.documentTCFList, this.requestModel, this.userToken);
		this.searchloading = true;
		this.tcfList = [];
		this.tcfService.postData(request).subscribe(data => {
			this.processTCFSearchData(data);
		},
			error => {
				this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
				this.searchloading = false;
			});
	}

	//process the tcf search data
	processTCFSearchData(data) {
		if (data && data.result) {
			var resultData = JSON.parse(data.result);
			if (resultData && resultData.status && resultData.status == TCFListResponse.Success) {
				this.mapTCFListResponse(resultData);
			}
			else if (resultData && resultData.status && resultData.status == TCFListResponse.NotFound) {
				this.model.noFound = true;
			}
			else {
				this.error = resultData;
			}
			this.searchloading = false;
		}
		else {
			this.showError('BOOKING_SUMMARY.LBL_ERROR', 'TCF Error.Please refer the log data');
			this.searchloading = false;
		}
	}

	mapTCFListResponse(resultData) {
		this.tcfList = [];
		if (resultData && resultData.data) {

			this.model.totalCount = resultData.total_records;
			this.tcfDocumentResponse.tcfDocumentList = [];
			resultData.data.forEach(element => {
				var document = new TCFDocumentItem();
				document.documentName = element.tcfNo;
				document.tcfId = element.tcfId;
				document.tcfDocumentDetails = [];
				document.isSelected = true;

				this.tcfDocumentResponse.tcfDocumentList.push(document);
			});

			if (resultData.data.length == 0)
				this.model.noFound = true;
		}

		//console.log(this.tcfResponse);
	}

	//gets or download the file
	getFile(file: AttachmentFile) {
		const url = window.URL.createObjectURL(file.file);
		window.open(url);
	}

	//clear the date input
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

	//download the file with the detail(blob data,file extension)
	downloadFile(data, mimetype, fileName) {
		//mimetype="application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		var blobData = this.base64ToBlob(data, mimetype);
		if (window.navigator && window.navigator.msSaveOrOpenBlob) {
			window.navigator.msSaveOrOpenBlob(blobData, fileName);
		}
		else {
			const url = window.URL.createObjectURL(blobData);
			//window.open(url);
			var a = document.createElement('a');
			a.href = url;
			a.download = fileName;//url.substr(url.lastIndexOf('/') + 1);
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
		}
	}

	//convert base64 to blob
	base64ToBlob(base64, mimeType) {
		const binaryString = window.atob(base64);
		const len = binaryString.length;
		const bytes = new Uint8Array(len);
		for (let i = 0; i < len; ++i) {
			bytes[i] = binaryString.charCodeAt(i);
		}
		return new Blob([bytes], { type: mimeType });
	};

	getTCFDocument(uniqueId) {
		this.genericGetRequest.requestUrl = config.TCF.specificDocument + uniqueId;
		this.genericGetRequest.token = this.userToken;
		this.masterData.isProcessLoader = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(data => {
			this.processDownloadDocumentResponse(data);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
			});
	}

	processDownloadDocumentResponse(data) {
		if (data && data.status && data.status == TCFDownloadDocumentResponse.Success) {
			this.downloadFile(data.data.attachment, data.data.mimetype, data.data.filename);
			this.masterData.isProcessLoader=false;
		}
		else if (data && data.status && data.status == TCFDownloadDocumentResponse.NotFound) {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_NO_DOCUMENTS_FOUND');
			this.masterData.isProcessLoader=false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.masterData.isProcessLoader=false;
		}
	}


}
