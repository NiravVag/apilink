import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { APIGatewayPUTRequest, GenericAPIGETRequest, GenericAPIPOSTRequest, GenericFileUploadRequest } from 'src/app/_Models/genericapi/genericapirequest.model';
import { CompleteStatusColor, ProductFileInfo, TCFDetail, TCFDetailMaster, TCFDocumentUpload, TCFProductInfoResponse, TCFScope, TCFScopeDetailResponse, TCFScopeResponse, TCFUploadDocumentResponse, ValidateTCFResponse } from 'src/app/_Models/tcf/tcfdetail.model'
import { TCFRequestData, userTokenRequest } from 'src/app/_Models/tcf/tcflanding.model';
import { DateObject, FileContainerList, UserType } from '../../common/static-data-common';
import { TrafficLightColor, TCFDownloadDocument, TCFStandardsResponse, TCFDocumentTypeResponse, TCFDocumentIssuerResponse, TCFDownloadDocumentResponse } from 'src/app/_Models/tcf/tcfcommon.model';
import { TCFService } from 'src/app/_Services/tcfdata/tcf.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { TCFStage } from 'src/app/_Models/tcf/tcfcommon.model';
import { NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery-9';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AttachmentFile, FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { Validator } from '../../common/validator';
import { JsonHelper } from '../../common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
const config = require("src/assets/appconfig/appconfig.json");
@Component({
	selector: 'app-tcf-detail',
	templateUrl: './tcf-detail.component.html',
	styleUrls: ['./tcf-detail.component.scss']
})

export class TcfDetailComponent {

	tcfDetail: TCFDetail;
	tcfScopeList: Array<TCFScope>;
	tcfDetailMaster: TCFDetailMaster;
	userTokenRequest: userTokenRequest;
	user: any;
	genericGetRequest: GenericAPIGETRequest;
	tcfDocumentUpload: TCFDocumentUpload;
	genericFileUploadRequest: GenericFileUploadRequest;
	tcfId: number;
	tcfStage = TCFStage;
	trafficLightColor = TrafficLightColor;
	completeStatusColor = CompleteStatusColor;
	paramParent: any;
	public productGalleryOptions: NgxGalleryOptions[];
	public productGalleryImages: NgxGalleryImage[];
	public modelRef: NgbModalRef;
	modalInputData: any;
	fileInfo: FileInfo;
	productFileInfo: ProductFileInfo;
	public fileAttachments: Array<AttachmentFile>;
	public productFileAttachments: Array<AttachmentFile>;
	uploadFileExtensions: string;
	uploadLimit: any;
	fileSize: any;
	public jsonHelper: JsonHelper;
	tcfDownloadDocument = TCFDownloadDocument;


	constructor(protected router: Router, private tcfService: TCFService, 
		public tcfDetailValidator: Validator, public modalService: NgbModal, 
		public pathroute: ActivatedRoute, private route: ActivatedRoute,
		 protected translate?: TranslateService, protected toastr?: ToastrService,public utility?:UtilityService) {

		//assign the user data from the local storage
		if (localStorage.getItem('currentUser'))
			this.user = JSON.parse(localStorage.getItem('currentUser'));

		//initializing the objects
		this.objectInitialization();

		//intializing the photo gallery data
		this.intializePhotoGalleryData();

		//intialize the tcf document validator
		this.intializeDocumentValidator(tcfDetailValidator);

	}

	//initializing the objects
	objectInitialization() {
		this.tcfDetail = new TCFDetail();
		this.genericGetRequest = new GenericAPIGETRequest();
		this.tcfDetailMaster = new TCFDetailMaster();
		this.tcfScopeList = new Array<TCFScope>();
		this.tcfDocumentUpload = new TCFDocumentUpload();
		this.fileInfo = new FileInfo();
		this.productFileInfo = new ProductFileInfo();
		this.fileAttachments = new Array<AttachmentFile>();
		this.fileAttachments = [];
		this.genericFileUploadRequest = new GenericFileUploadRequest();
	}

	//intializing the photo gallery data
	intializePhotoGalleryData() {
		this.productGalleryOptions = [
			{
				width: '100%',
				height: '400px',
				thumbnailsColumns: 4
			}
		];
		this.productGalleryImages = [];
		this.fileAttachments = new Array<AttachmentFile>();
	}

	//intialize the tcf document validator
	intializeDocumentValidator(tcfDetailValidator) {
		this.tcfDetailValidator.setJSON("tcf/tcf-detail.valid.json");
		this.tcfDetailValidator.setModelAsync(() => this.tcfDocumentUpload);
		this.jsonHelper = tcfDetailValidator.jsonHelper;
		this.tcfDetailValidator.isSubmitted = false;
	}

	ngOnInit() {

		this.initialize();
	}

	//initialize the objects
	initialize() {
		//get tcfid from the querystring
		var tcfId = this.pathroute.snapshot.paramMap.get("tcfId");

		//get tcf detail data
		if (tcfId) {
			this.tcfId = Number.parseInt(tcfId);
			this.getTCFDetailData();
		}

		//get the standard list
		this.getTCFStandardList();
		//get the tcf type list
		this.getTCFTypeList();
		//get the issuer list
		this.getIssuerList();

		//set param parent data since it has to navigate to summary page
		this.route.queryParams.subscribe(
			params => {
				if (params != null && params['paramParent'] != null) {
					this.paramParent = params['paramParent'];
				}
			}
		);

		//assign the fileextension since there is no validation
		this.uploadFileExtensions = '';
	}

	getTCFDetailData() {
		//make the usertoken request structure
		this.generateUserTokenRequest();
		//get the tcf detail info
		this.getUserTokenData();
	}

	//get the issuer list
	getIssuerList() {
		this.genericGetRequest.requestUrl = config.TCF.issuerList;
		this.tcfDetailMaster.issuerListLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processIssuerListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.issuerListLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
			});
	}

	//process the issuer list response
	processIssuerListResponse(response) {
		if (response && response.status && response.status == TCFDocumentIssuerResponse.Success) {
			this.tcfDetailMaster.issuerList = response.data;
			this.tcfDetailMaster.issuerListLoading = false;
		}
		else if (response && response.status && response.status == TCFDocumentIssuerResponse.NotFound) {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_ERROR');
			this.tcfDetailMaster.issuerListLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.issuerListLoading = false;
		}
	}

	//make the usertoken request structure
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

	generateUserTokenRequestData() {
		var request = new GenericAPIPOSTRequest();
		request.requestUrl = config.TCF.userAuthentication;
		request.requestData = this.userTokenRequest;
		return request;
	}

	//get user authenticated token
	getUserTokenData() {
		var request = this.generateUserTokenRequestData();
		this.tcfDetailMaster.isDetailLoader = true;
		this.tcfService.postData(request).subscribe(response => {
			this.processUserTokenResponse(response);
		},
			error => {
				this.showError('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
			});
	}

	//process the tcf detail response
	processUserTokenResponse(response) {
		if (response && response.result) {
			this.tcfDetailMaster.isDetailLoader = false;
			//parse the json result
			var resultData = JSON.parse(response.result);
			//process the success response
			if (resultData && resultData.status && resultData.status == "1"
				&& resultData.data && resultData.data.authorization) {
				this.processUserTokenSuccessResponse(resultData);
			}
			else {
				this.tcfDetailMaster.isDetailLoader = false;
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
			}
		}
		else {
			this.tcfDetailMaster.isDetailLoader = false;
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
		}
	}

	//process TCF Detail success response
	processUserTokenSuccessResponse(resultData) {
		this.tcfDetailMaster.userToken = resultData.data.authorization;
		//get the tcf product(base) information
		this.getTCFProductInformation();
		//get the tcf scope details
		this.getTCFScope();
	}

	//get the tcf product information
	getTCFProductInformation() {

		this.genericGetRequest.requestUrl = config.TCF.tcfPageInfo + this.tcfId;
		this.genericGetRequest.token = this.tcfDetailMaster.userToken;
		this.genericGetRequest.isGenericToken = false;
		this.tcfDetailMaster.productInfoLoading = true;
		this.tcfDetailMaster.isDetailLoader = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFProductInfoResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.productInfoLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
			});
	}

	//process tcf product info response
	processTCFProductInfoResponse(response) {
		if (response && response.status && response.status == TCFProductInfoResponse.Success) {
			this.processTCFProductInfoSuccessResponse(response);
			this.tcfDetailMaster.productInfoLoading = false;
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else if (response && response.status && response.status == TCFProductInfoResponse.NotFound) {
			this.showError('TCF_DETAIL.LBL_ERROR', 'TCF_DETAIL.MSG_TCF_INFO_NOT_FOUND');
			this.tcfDetailMaster.productInfoLoading = false;
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.productInfoLoading = false;
			this.tcfDetailMaster.isDetailLoader = false;
		}
	}

	//process tcf product info success response
	processTCFProductInfoSuccessResponse(response) {
		if (response && response.data[0])
			this.tcfDetail = response.data[0];
		if (this.tcfDetail.image1)
			this.tcfDetail.productUrl = 'data:image/jpeg;base64,' + this.tcfDetail.image1;
		if (this.tcfDetail.image2)
			this.tcfDetail.productUrl2 = 'data:image/jpeg;base64,' + this.tcfDetail.image2;


		setTimeout(() => {
			if (this.tcfDetail.statusId == this.tcfStage.Completed && this.tcfDetailMaster.standardList.length == 0) {
				this.showWarning('TCF_COMMON.LBL_WARNING', 'TCF_DETAIL.MSG_NOMORE_DOCUMENTS_UPLOADED');
			}
		}, 10);

	}

	//get the tcf scope details
	getTCFScope() {
		this.genericGetRequest.requestUrl = config.TCF.tcfScope + this.tcfId;
		this.genericGetRequest.token = this.tcfDetailMaster.userToken;
		this.genericGetRequest.isGenericToken = false;
		this.tcfDetailMaster.isDetailLoader = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFScopeResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.tcfScopeLoading = false;
			});
	}

	//process the tcf scope response
	processTCFScopeResponse(response) {
		if (response && response.status && response.status == TCFScopeResponse.Success && response.data) {
			this.mapTCFScope(response.data);
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else if (response && response.status && response.status == TCFScopeResponse.NotFound) {
			//this.showError('TCF_DETAIL.LBL_ERROR', 'TCF_DETAIL.MSG_TCF_SCOPE_NOT_DEFINED');
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else {
			this.showError('TCF_DETAIL.LBL_ERROR', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.isDetailLoader = false;
		}
	}

	//map the tcf scope data
	mapTCFScope(data) {
		this.tcfScopeList = new Array<TCFScope>();
		if (data) {
			if (data[0]) {
				this.tcfDetailMaster.completeStatus = data[0].completeStatus;
				this.tcfDetailMaster.completeStatusColor = data[0].completeStatusColor;
			}
			data.forEach(detail => {
				var scopeDetail = new TCFScope();
				scopeDetail.standardName = detail.standardName;
				scopeDetail.remark = detail.remark;
				scopeDetail.regulationName = detail.regulationName;
				scopeDetail.status = detail.status;
				scopeDetail.trafficColor = detail.trafficColor;
				scopeDetail.version = detail.version;
				scopeDetail.projectStandardId = detail.projectStandardId;
				scopeDetail.scopeDetailList = [];
				this.tcfScopeList.push(scopeDetail);
			});
		}
	}

	//toggling the scope detail rows
	toggleExpandRow(event, index, tcfScope) {

		let triggerTable = event.target.parentNode.parentNode;
		var firstElem = document.querySelector('[data-expand-id="scopeDetail' + index + '"]');
		firstElem.classList.toggle('open');

		triggerTable.classList.toggle('active');

		if (firstElem.classList.contains('open')) {
			event.target.innerHTML = '-';
			this.getScopeDetail(tcfScope);
		} else {
			event.target.innerHTML = '+';
		}
	}

	//get the scope detail data
	getScopeDetail(tcfScope) {
		this.genericGetRequest.requestUrl = config.TCF.tcfScopeDetail + tcfScope.projectStandardId;
		this.genericGetRequest.token = this.tcfDetailMaster.userToken;
		this.genericGetRequest.isGenericToken = false;
		tcfScope.scopeDetailLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFScopeDetailResponse(tcfScope, response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				tcfScope.scopeDetailLoading = false;
			});

	}

	//process the tcf scope detail response
	processTCFScopeDetailResponse(tcfScope, response) {
		if (response && response.status && response.status == TCFScopeDetailResponse.Success) {
			tcfScope.scopeDetailList = response.data;
			tcfScope.scopeDetailLoading = false;
		}
		else if (response && response.status && response.status == TCFScopeDetailResponse.NotFound) {
			this.showWarning('TCF_DETAIL.LBL_WARNING', 'TCF_DETAIL.MSG_NO_ATTACHMENT_FOUND');
			tcfScope.scopeDetailLoading = false;
		}
	}

	//navigate to TCF List
	public returnToTCFList(path, type = '') {
		let entity: string = this.utility.getEntityName();
		if (type == '')
			this.router.navigate([`/${entity}/${path}`], { queryParams: { param: this.paramParent } });
		else
			this.router.navigate([`/${entity}/${path}/${type}`], { queryParams: { param: this.paramParent } });
	}

	//preview the product image
	getPreviewProductImage(modalcontent) {
		this.productGalleryImages = [];
		//assign image1 
		if (this.tcfDetail.productUrl) {
			this.productGalleryImages.push(
				{
					small: this.tcfDetail.productUrl,
					medium: this.tcfDetail.productUrl,
					big: this.tcfDetail.productUrl,
				});
		}
		//assign image2
		if (this.tcfDetail.productUrl2) {
			this.productGalleryImages.push(
				{
					small: this.tcfDetail.productUrl2,
					medium: this.tcfDetail.productUrl2,
					big: this.tcfDetail.productUrl2,
				});
		}

		this.modelRef = this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true });

		this.modelRef.result.then((result) => {
		}, (reason) => { });
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
					let fileItem: AttachmentFile = {
						fileName: file.name,
						file: file,
						fileSize:file.fileSize,
						isNew: true,
						id: 0,
						status: 0,
						mimeType: file.type,
						fileUrl: "",
						uniqueld: file.name,
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

	selectProductFiles(event) {
		if (event && !event.error && event.files) {
			if (this.isValidFileExtensions(event.files) && this.isValidFileSize(event.files)) {

				this.productFileAttachments = [];
				if (event.files.length > this.fileInfo.uploadLimit) {
					this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
				}
				else {
					for (let file of event.files) {
						let fileItem: AttachmentFile = {
							fileName: file.name,
							file: file,
							fileSize:file.fileSize,
							isNew: true,
							id: 0,
							status: 0,
							mimeType: file.type,
							fileUrl: "",
							uniqueld: file.name,
							isSelected: false,
							fileDescription:null
						};
						this.productFileAttachments.push(fileItem);
					}
				}
			}

		}
		else if (event && event.error && event.errorMessage) {
			this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
		}
	}

	//gets or download the file
	getFile(file: AttachmentFile) {
		const url = window.URL.createObjectURL(file.file);
		window.open(url);
	}

	//remove the file from the attachment
	removeAttachment(index) {
		this.fileAttachments.splice(index, 1);
		this.fileAttachments = [];
	}

	removeProductAttachment(index) {
		this.productFileAttachments.splice(index, 1);
	}

	//validate the upload file form
	isUploadFormValid() {
		var isOk = this.tcfDetailValidator.isValid('standardIds')
			&& this.tcfDetailValidator.isValid('issueDate');
		if (isOk) {
			if (this.fileAttachments.length == 0) {
				isOk = false;
				this.showWarning('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_FILES_REQUIRED');
			}
		}
		return isOk;
	}

	//reset the upload data
	resetUploadData() {
		this.tcfDocumentUpload = new TCFDocumentUpload();
		this.tcfDetailValidator.isSubmitted = false;
		this.fileAttachments = [];
	}

	//upload the TCF Document
	uploadTCFDocument() {
		this.tcfDetailValidator.initTost();
		this.tcfDetailValidator.isSubmitted = true;
		if (this.isUploadFormValid()) {
			this.tcfDocumentUpload.tcfId = this.tcfId;
			this.tcfDetailMaster.uploadTCFDocumentLoading = true;
			this.tcfDetailMaster.isDetailLoader = true;
			this.tcfService.uploadTCFDocument(this.tcfDocumentUpload, this.fileAttachments, this.tcfDetailMaster.userToken).subscribe(response => {
				this.processUploadTCFDocument(response);
			},
				error => {
					this.showError('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
				});
		}

	}

	processUploadTCProductImage(response) {
		if (response && response.status) {

			if (response && response.status && response.status == TCFUploadDocumentResponse.Success) {
				this.showSuccess('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_UPLOAD_SUCCESS');
				this.resetUploadData();
				this.getTCFDetailData();
				this.tcfDetailMaster.uploadTCFProductFileLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
				this.modelRef.close();

			}
			else if (response && response.status && response.status == TCFUploadDocumentResponse.Failure) {
				this.tcfDetailMaster.isDetailLoader = false;
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_DOCUMENT_UPLOAD_FAILED');
				this.tcfDetailMaster.uploadTCFProductFileLoading = false;
				this.modelRef.close();
			}
		}
		else {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.uploadTCFProductFileLoading = false;
			this.tcfDetailMaster.isDetailLoader = false;
			this.modelRef.close();
		}
	}

	//process the upload tcf document response
	processUploadTCFDocument(response) {
		if (response && response.status) {

			if (response && response.status && response.status == TCFUploadDocumentResponse.Success) {
				this.showSuccess('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_UPLOAD_SUCCESS');
				this.resetUploadData();
				this.getTCFDetailData();
				this.tcfDetailMaster.uploadTCFDocumentLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
			}
			else if (response && response.status && response.status == TCFUploadDocumentResponse.Failure) {
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_DOCUMENT_UPLOAD_FAILED');
				this.tcfDetailMaster.uploadTCFDocumentLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
			}
		}
		else {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TCF_UNKNOWN_ERROR');
          this.tcfDetailMaster.uploadTCFDocumentLoading = false;
          this.tcfDetailMaster.isDetailLoader = false;
		}
	}

	//get the TCF Standard List
	getTCFStandardList() {
		this.genericGetRequest.requestUrl = config.TCF.standardList + this.tcfId;
		this.tcfDetailMaster.standardListLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFStandardListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.productInfoLoading = false;
				this.tcfDetailMaster.isDetailLoader = false;
			});
	}

	//process the TCF Standard List Response
	processTCFStandardListResponse(response) {
		if (response && response.status && response.status == TCFStandardsResponse.Success) {
			this.tcfDetailMaster.standardList = response.data;
			this.tcfDetailMaster.standardListLoading = false;
		}
		else if (response && response.status && response.status == TCFStandardsResponse.NotFound) {
			this.tcfDetailMaster.standardListLoading = false;
		}
		else {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.standardListLoading = false;
		}
	}

	//get the TCF Type List Master Data
	getTCFTypeList() {
		this.genericGetRequest.requestUrl = config.TCF.typeList;
		this.tcfDetailMaster.typeLoading = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(response => {
			this.processTCFTypeListResponse(response);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.typeLoading = false;
			});
	}

	//process the tcf type list response
	processTCFTypeListResponse(response) {
		if (response && response.status && response.status == TCFDocumentTypeResponse.Success) {
			this.tcfDetailMaster.typeList = response.data;
			this.tcfDetailMaster.typeLoading = false;
		}
		else if (response && response.status && response.status == TCFDocumentTypeResponse.NotFound) {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TYPE_NOT_FOUND');
			this.tcfDetailMaster.typeLoading = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.typeLoading = false;
		}
	}

	//get the tcf document by type
	getTCFDocumentByType(uniqueId, documentType) {
		this.assignDownloadUrls(uniqueId, documentType);

		this.genericGetRequest.token = this.tcfDetailMaster.userToken;
		this.tcfDetailMaster.isDetailLoader = true;
		this.tcfService.getData(this.genericGetRequest).subscribe(data => {
			this.processDownloadDocumentResponse(data);
		},
			error => {
				this.showError('TCF_LIST.LBL_ERROR', 'COMMON.MSG_UNKNONW_ERROR');
				this.tcfDetailMaster.isDetailLoader = false;
				//this.masterData.statusLoading = false;
			});
	}

	//assign the download api urls based on the document type requested
	assignDownloadUrls(uniqueId, documentType) {
		switch (documentType) {
			case this.tcfDownloadDocument.AllDocuments:
				this.genericGetRequest.requestUrl = config.TCF.downloadAllDocument + uniqueId;
				break;
			case this.tcfDownloadDocument.AllValidDocuments:
				this.genericGetRequest.requestUrl = config.TCF.downloadAllValidDocument + uniqueId;
				break;
			case this.tcfDownloadDocument.SpecificDocument:
				this.genericGetRequest.requestUrl = config.TCF.specificDocument + uniqueId;
				break;
		}
	}

	processDownloadDocumentResponse(data) {
		if (data && data.status && data.status == TCFDownloadDocumentResponse.Success) {
			this.downloadFile(data.data.attachment, data.data.mimetype, data.data.filename);
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else if (data && data.status && data.status == TCFDownloadDocumentResponse.NotFound) {
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_NO_DOCUMENTS_FOUND');
			this.tcfDetailMaster.isDetailLoader = false;
		}
		else {
			this.showError('TCF_LIST.LBL_ERROR', 'TCF_LIST.MSG_TCF_UNKNOWN_ERROR');
			this.tcfDetailMaster.isDetailLoader = false;
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

	//validate the TCF(changing the status to completed in TCF System)
	validateTCF() {
		var request = new APIGatewayPUTRequest();
		request.requestUrl = config.TCF.validateTCF + this.tcfId;
		request.token = this.tcfDetailMaster.userToken;
		request.isGenericToken = false;
		this.tcfDetailMaster.isDetailLoader = true;

		this.tcfService.putData(request).subscribe(response => {
			this.processValidateTCFResponse(response);
		},
			error => {
				this.tcfDetailMaster.isDetailLoader = false;
				this.showError('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
			});
	}

	//process the validate tcf response
	processValidateTCFResponse(response) {
		if (response && response.status) {
			if (response && response.status && response.status == ValidateTCFResponse.Success) {
				this.tcfDetailMaster.isDetailLoader = false;
				this.processSuccessValidateTCF();
			}
			else if (response && response.status && response.status == ValidateTCFResponse.Failure) {
				this.tcfDetailMaster.isDetailLoader = false;
				this.showError('TCF_COMMON.LBL_ERROR', 'TCF Error');
			}
		}
		else {
			this.tcfDetailMaster.isDetailLoader = false;
			this.showError('TCF_COMMON.LBL_ERROR', 'TCF_COMMON.MSG_TOKEN_GENERATION_FAILED');
		}
	}

	//process the sucess validate tcf response
	processSuccessValidateTCF() {
		this.showSuccess('TCF_DETAIL.LBL_TITLE', "Validate TCF Successfully");
		this.getTCFDetailData();
		this.tcfDetailMaster.isDetailLoader = false;
	}

	//show the error msg function
	public showError(title: string, msg: string, _disableTimeOut?: boolean) {
		let tradTitle: string = "";
		let tradMessage: string = "";

		this.translate.get(title).subscribe((text: string) => { tradTitle = text });
		this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

		this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
	}
	//show the warning msg function
	public showWarning(title: string, msg: string, _disableTimeOut?: boolean) {
		let tradTitle: string = "";
		let tradMessage: string = "";

		this.translate.get(title).subscribe((text: string) => { tradTitle = text });
		this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

		this.toastr.warning(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
	}

	//show the success msg function
	public showSuccess(title: string, msg: string, _disableTimeOut?: boolean) {
		let tradTitle: string = "";
		let tradMessage: string = "";

		this.translate.get(title).subscribe((text: string) => { tradTitle = text });
		this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

		this.toastr.success(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
	}

	//upload the TCF Document
	uploadTCFProductImage() {
		if (this.productFileAttachments && this.productFileAttachments.length > 0) {
			this.genericFileUploadRequest.id = this.tcfId;
			this.genericFileUploadRequest.token = this.tcfDetailMaster.userToken;
			this.genericFileUploadRequest.requestUrl = config.TCF.tcfProductImageUpload + this.tcfId;
			this.tcfDetailMaster.uploadTCFProductFileLoading = true;
			this.tcfService.uploadTCFiles(this.genericFileUploadRequest, this.productFileAttachments).subscribe(response => {
				this.processUploadTCProductImage(response);
			},
				error => {
					this.showError('TCF_DETAIL.LBL_TITLE', 'TCF_DETAIL.MSG_TCF_UNKNOWN_ERROR');
					this.tcfDetailMaster.uploadTCFProductFileLoading = false;
				});
		}
		else {
			this.showWarning('TCF_DETAIL.LBL_TITLE', 'Please select the files');
		}
	}

	openAddProduct(content) {
		this.productFileAttachments = [];
		this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
		this.modelRef.result.then((result) => {

		}, (reason) => {

		});
	}

	isValidFileExtensions(files) {
		var isExtensionExists = true;
		if (files && files.length > 0) {

			files.forEach(element => {
				var extensions = (this.productFileInfo.uploadFileExtensions.split(','))
					.map(function (x) { return x.toLocaleUpperCase().trim() });

				// Get file extension
				var ext = element.name.toUpperCase().split('.').pop() || element.name;
				// Check the extension exists
				var exists = extensions.includes(ext);

				if (!exists) {
					this.showWarning('TCF_DETAIL.LBL_TITLE', "Please upload the files with the following extensions " + extensions.join());
					isExtensionExists = false;
				}
			});
		}
		return isExtensionExists;
	}

	isValidFileSize(files) {
		var valid = true;
		for (let file of files) {
			var fileSize = Math.round((file.size / 1024));
			if (fileSize > this.productFileInfo.fileSize) {
				valid = false;
				this.showError('TCF_DETAIL.LBL_TITLE', 'Accepted File Size is 2 MB');
				break;
			}
		}
		return valid;
	}

}
