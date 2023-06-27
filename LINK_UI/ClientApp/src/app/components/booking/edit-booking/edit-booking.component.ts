import { Component, OnInit, ViewChild, ElementRef, ViewChildren, QueryList, TemplateRef } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { UserModel } from '../../../_Models/user/user.model'
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { BookingService } from '../../../_Services/booking/booking.service'
import { first, retry, every, distinctUntilChanged, tap, switchMap, catchError, debounceTime, takeUntil, timestamp } from 'rxjs/operators';
import { NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import {
  UserType, BookingStatus, BookingSearchRedirectPage, BookingTableScrollHeight, Unit, InspectionPageType, Service,
  ControlType, ControlAttribute, DynamicControlModuleType, InspectionServiceType, AqlType, SampleType, Url, FileContainerList, SupplierType, ContainerServiceList, ListSize, RightsEnum, EntPageFieldAccessEnum, EntityAccess, InspectionLocationEnum, InspectedStatusList, FileAttachmentCategory, PaymentOptions, BookingTypes, CustomerEnum, CustomTypePercentList, CustomTypePiecesList, LabType, DFDDLSourceType, FlashProcessAudit
} from '../../common/static-data-common';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditPurchaseOrderModel, purchaseOrderDetail } from '../../../_Models/purchaseorder/edit-purchaseorder.model';
import { CustomerProduct } from '../../../_Services/customer/customerproductsummary.service'
import {
  EditInspectionBooking, InspectionFileAttachment, ProductFileAttachment, BookingCustomerContactRequest,
  InspectionDFTransaction,
  EditBookingCustomerResult,
  BusinessLine,
  InspectionPOProductDetail,
  commonDataSource,
  POProductDataSourceRequest,
  POProductList,
  CustomerProductDetailResult,
  BookingUnit,
  BookingProductFieldType,
  DraftInspectionBooking,
  DraftInspectionResult,
  SaveDraftInspectionResult,
  DeleteDraftInspectionResult,
  BookingViewType,
  EntPageRequest,
  BookingProductTableMaster,
  EntPageFieldAccessResult,
  PoProductDetailRequest,
  POProductDetailResult,
  BookingFileZipResult,
  FilterPoProductEnum,
  InspectionPickingMaster,
  InspectionPickingDetails,
  LabDataResult,
  SavePickingContact
} from '../../../_Models/booking/inspectionbooking.model';
import { BookingPreviewData, ContactDetailPreview, ConfigPreviewData, PriceCategoryRequest, PriceCategoryResult, UserAccessRequest, UserAccessResult, CSConfigListResult } from 'src/app/_Models/booking/bookingpreview.model';
import { ApplicantStaffResponseResult, BookingMasterData, CommonFieldType, PanelData, PanelNavigation, PanelNavigationStatus, PODataSourceRequest, POProductDataRequest, ProductValidationStatus, selectedDeletePoDetail, selectedSearchPoDetail } from 'src/app/_Models/booking/bookingmaster.model';
import { PurchaseOrderService } from '../../../_Services/purchaseorder/purchaseorder.service';
import { LocationService } from '../../../_Services/location/location.service';
import { CustomerServiceConfig } from 'src/app/_Services/customer/customerserviceconfig.service';
import { SupplierService } from '../../../_Services/supplier/supplier.service';
import { NgxGalleryOptions, NgxGalleryImage } from 'ngx-gallery-9';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { DynamicFieldService } from '../../../_Services/dynamicfields/dynamicfield.service'
import { ControlAttributes } from '../../../_Models/dynamiccontrols/dynamicontrolbase.model';
import { TextboxControl } from '../../../_Models/dynamiccontrols/control-textbox.model';
import { DropDownControl } from '../../../_Models/dynamiccontrols/control-dropdown.model';
import { DateTimePickerControl } from '../../../_Models/dynamiccontrols/control-datepicker.model';
import { TextAreaControl } from 'src/app/_Models/dynamiccontrols/control-textarea.model';
import { Observable, Subject, of, concat, BehaviorSubject } from 'rxjs';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { ProductValidateData, ProductValidateMaster, ValidationType } from 'src/app/_Models/booking/productValidate.model';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { CustomerProductFileResult, PoProductRequest, ProductScreenCallType } from 'src/app/_Models/customer/editcustomerproduct.model';
import { forEachChild } from 'typescript';
import { CountryDataSourceRequest, CustomerCommonDataSourceRequest, ProductDataSourceRequest, ProductDetailsDataSourceRequest, SupplierDataSourceRequest } from 'src/app/_Models/common/common.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MasterContact, MasterContactData, SaveMasterContactRequest } from 'src/app/_Models/booking/mastercontact.model';
import { PurchaseOrderListModel } from 'src/app/_Models/purchaseorder/purchaseordersummary.model';
import { ExistingBookingPoProductData } from 'src/app/_Models/purchaseorder/upload-purchaseorder.model';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { CheckPointTypeEnum } from 'src/app/_Models/customer/customer-checkpoint.model';
import { InspectionPickingService } from 'src/app/_Services/booking/inspectionpicking.service';
import { LabAddressRequest, LabAddressResult, LabContactRequest, LabDataContactsListResult, MasterLabAddressData, SaveLabAddressData, SaveLabAddressResult } from 'src/app/_Models/booking/LabDetailsModel';
import { LabAddress } from 'src/app/_Models/lab/edit-lab.model';
import { LabService } from 'src/app/_Services/lab/lab.service';



@Component({
  selector: 'app-edit-booking',
  templateUrl: './edit-booking.component.html',
  styleUrls: ['./edit-booking.component.scss'],
  animations: [
    trigger('expandCollapse', [
      state('open', style({
        'height': '*',
        'opacity': 1,
        'padding': '*'
      })),
      state('close', style({
        'height': '0px',
        'opacity': 0,
        'padding': 0
      })),
      transition('open <=> close', animate(300))
    ])
  ]
})
export class EditBookingComponent extends DetailComponent {



  //#region Variable Declaration

  isPreviewPanelVisible: boolean;
  isProductDelete: boolean = true;
  _userTypeId = UserType;
  _unit = Unit;
  aqlType = AqlType;
  sampleType = SampleType;
  downloadloading: boolean = false;
  destinationCountryList: any;
  containerList: any = [];
  modalInputData: any;
  modalUploadPOInputData: any;
  _bookingredirectpage = BookingSearchRedirectPage;
  _inspectionPageType = InspectionPageType;
  _customerService = Service;

  _bookingStatus = BookingStatus;
  contactDetailPreview: ContactDetailPreview;
  _controlType = ControlType;
  _controlAttribute = ControlAttribute;
  _dynamicControlModuleType = DynamicControlModuleType;



  public purchaseModel: EditPurchaseOrderModel;
  public poDetailValidators: Array<any>;
  public modelRef: NgbModalRef;
  public model: EditInspectionBooking;
  public customerContactModel: BookingCustomerContactRequest;
  public bookingPreviewData: BookingPreviewData;
  public bookingMasterData: BookingMasterData;
  public jsonHelper: JsonHelper;
  public poDetailJsonHelper: JsonHelper;

  public selectedAllProducts: boolean = false;


  uploadLimit: number;
  uploadFileExtensions: string;
  _HolidayDates = [];
  _leadtime: number;
  _serviceLeadDayMessage: string;
  BookingRuleDesc: string;
  leaddays: NgbDate;
  isHasFactory: boolean = false;
  isReInspection: boolean = false;
  isReBooking: boolean = false;
  inspectionPageType: number;
  isRedirectFromQuotation: boolean = false;
  public fileExtension: string = ""
  public fileSize: number;
  public poColSpan: number = 20;

  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[];
  dynamicControls: any;
  dynamicControlBaseData: any;



  public poPopupProductList: any;



  IsEditPO: boolean = false;
  currentUser: UserModel;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  isDateDisabled: boolean = false;
  spacebeforeCusContact: boolean = false;
  spacebeforeCusBookingNo: boolean = false;

  public inspectionServiceType = InspectionServiceType;
  public exportProductLoading: boolean = false;
  productValidateMaster: ProductValidateMaster;
  _productValidationStatus = ProductValidationStatus;
  componentDestroyed$: Subject<boolean> = new Subject();
  _businessLine = BusinessLine;
  _commonfieldType: number;
  _commonFieldTypeEnum = CommonFieldType;
  commonfieldApplyMsg: string;

  public isContainerService: boolean = false;

  panelNavigation: PanelNavigation;
  panelNavigationStatus = PanelNavigationStatus;
  isBookingParentProductData: boolean;
  panelData = PanelData;
  bookingUnit = BookingUnit;
  _bookingProductFieldType = BookingProductFieldType;
  isProductCheckedinPopUp: boolean = false;
  searchpoloader: boolean = false;

  poProductCommonData: boolean = false;

  draftInspectionId: number;
  deleteDraftInspectionId: number;
  draftBookingList: any;

  userTypeEnum = UserType;
  _bookingNoteVisible: boolean = false;

  isDefaultProductCategoryRequired: boolean = false;
  draftInspectionBooking: DraftInspectionBooking;
  @ViewChild('scrollableProductTable') private scrollableProductTable: ElementRef;

  viewType: number;

  customerMoreSection: boolean;

  commonFields: boolean;

  _bookingViewTypeEnum = BookingViewType;

  entPageFieldAccess: any;
  filterEntFieldAccess: any;

  bookingProductTableMaster: BookingProductTableMaster;

  entPageFieldAccessEnum = EntPageFieldAccessEnum;

  masterContactData: MasterContactData;

  masterLabAddressData: MasterLabAddressData;

  selectedEntity;

  customerProductList: any;
  listModel: PurchaseOrderListModel;

  _bookingIsInspected: boolean;
  _inspectionBookingTypeEnum = BookingTypes;

  _bookingAttachmentConfirmForEmail = "if selected then the document will available in booking email";
  _bookingAttachmentConfirmForReport = "if selected then the document will available in report filing page for QC";

  _paymentOptions = PaymentOptions;
  _fileAttachmentCategory = FileAttachmentCategory;
  _customerEnum = CustomerEnum;
  //#endregion VariableDeclaration


  @ViewChild('addNewPoTemplate') newPoTemplate: TemplateRef<any>;
  @ViewChild('confirmRemovePicking') confirmRemovePickingTemplate: TemplateRef<any>;
  dialog: NgbModalRef | null;

  inspectionPickingMaster: InspectionPickingMaster;
  pickingDescription: string;

  constructor(toastr: ToastrService,
    private authService: AuthenticationService,
    public locationService: LocationService,
    public puchaseOrderService: PurchaseOrderService,
    public refService: ReferenceService,
    public modalService: NgbModal,
    public customerProductService: CustomerProduct,
    public dynamicFieldService: DynamicFieldService,
    public fileStoreService: FileStoreService,
    public validator: Validator,
    public poDetailValidator: Validator,
    route: ActivatedRoute,
    router: Router,

    translate: TranslateService,
    public service: BookingService,
    public pickingService: InspectionPickingService,
    public supplierService: SupplierService,
    public serviceConfig: CustomerServiceConfig,
    public calendar: NgbCalendar,
    public pathroute: ActivatedRoute,
    public utility: UtilityService, public customerService: CustomerService,
    public customerCheckPointService: CustomerCheckPointService, public labService: LabService) {

    super(router, route, translate, toastr, modalService);

    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("booking/edit-booking.valid.json");
    this.validator.setModelAsync(() => this.model);



    this.currentUser = authService.getCurrentUser();

    this.bookingProductTableMaster = new BookingProductTableMaster();
    this.masterContactData = new MasterContactData();
    this.masterLabAddressData = new MasterLabAddressData();
    this.customerProductList = [];
    this.listModel = new PurchaseOrderListModel();

    this.inspectionPickingMaster = new InspectionPickingMaster();
    this.bookingMasterData = new BookingMasterData();
    this.bookingPreviewData = new BookingPreviewData();
    this.customerContactModel = new BookingCustomerContactRequest();
    this.productValidateMaster = new ProductValidateMaster();
    this.bookingMasterData.userTypeId = this.currentUser.usertype;
    this.bookingMasterData.IsInternalUser = this.bookingMasterData.userTypeId == UserType.InternalUser ? true : false;
    this.bookingMasterData.isPicking = false;



    this.selectedAllProducts = false;
    this.isPreviewPanelVisible = false;

    this.bookingMasterData.uploadLimit = 20;
    this.bookingMasterData.fileSize = 21485760;
    this.bookingMasterData.poList = [];
    this.bookingMasterData.poProductSourceList = [];


    this.bookingMasterData.poProductList = [];
    this.bookingMasterData.bookingProductFiles = [];

    this.bookingMasterData.searchPO = false;

    this.bookingMasterData.searchType = 0;
    this.bookingMasterData.bookingSubProductCategoryList = null;
    this.bookingMasterData.uploadFileExtensions = 'png,jpg,jpeg,xlsx,pdf,doc,docx,xls,csv,xlsm,xls,xlsx,zip,rar';
    this.purchaseModel = new EditPurchaseOrderModel();

    this.poDetailValidators = [];

    this.bookingMasterData.bookingPOProductValidators = [];

    this.bookingMasterData.totalBookingQty = null;
    this.bookingMasterData.totalPickingQty = null;



    this.fileExtension = 'png,jpg,jpeg';
    this.fileSize = 1242880;//5 mb

    this.productGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];

    this.productGalleryImages = [];
    this.bookingMasterData.selectedPOList = [];
    this._toastr = toastr;
    this._translate = translate;

    this.isBookingParentProductData = false;
    this.getCountryList();
    this.GetSeasonYear();
    this.getProductCategoryList();
    this.getserviceLevelPickFirst();

    this.viewType = this._bookingViewTypeEnum.NewBooking;

    this.customerMoreSection = true;
    this.commonFields = false;
    this.bookingMasterData.showDownloadAllAttachment = false;
    this._bookingIsInspected = false;
    this.pickingDescription = this.utility.textTranslate('EDIT_BOOKING.LBL_PICKING_DESC');
  }

  onInit(id?: any): void {
    this.checkPageType();
    if (id) {
      this.isHasFactory = true;
      this.isPreviewPanelVisible = true;
    }

    if (!id || this.isReBooking || this.isReInspection) {

      this.getInspectionDraftBookings();

      this.isPreviewPanelVisible = false;
      this._bookingNoteVisible = true;
      this.GetOffice();
      this.getUnitList();
      this.getInspectionLocations();
      this.getInspectionShipmentTypes();
      this.getBusinessLines();
    }
    this.getInspectionBookingTypes(id);

    this.Initialize(id);

    this.getFbTemplateList();

    //apply poProductDependentFilter if it is factory or supplier
    if (this.currentUser.usertype == this.userTypeEnum.Supplier || this.currentUser.usertype == this.userTypeEnum.Factory)
      this.bookingMasterData.poProductDependentFilter = true;

  }

  successNewBooking() {
    this.isPreviewPanelVisible = false;
    this.getInspectionDraftBookings();
    this.Initialize();
  }


  // #region Container Flow

  SetContainerLimit(searchText) {

    // do this validation if the container count is already set

    this.CalculateTotalContainers();

    if (this.bookingMasterData.totalContainers && searchText < this.bookingMasterData.totalContainers) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CONTAINER_LESS_THAN_UTITLIZED');
      this.model.containerLimit = this.bookingMasterData.totalContainers;
      this.getContainerList(this.model.containerLimit);
      this.resetContainerSelection();

    }
    else {
      this.getContainerList(searchText);
      this.resetContainerSelection();
    }
  }
  // #endregion Container Flow


  // #region Add Supplier/Factory Popup

  AddSupplierOrFactory(typeId, name, content) {

    if (this.model.customerId == undefined || this.model.customerId == null) {
      this.showError("Inspection Booking", 'Please select Customer');
      return;
    }
    if (typeId == SupplierType.Factory && (this.model.supplierId == undefined || this.model.supplierId == null)) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SUPPLIER_REQUIRED');
      return;
    }
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.modalInputData = {
      typeId: typeId,
      customerId: this.model.customerId,
      supplierId: this.model.supplierId
    };
    this.modelRef.result.then((result) => {
    }, (reason) => {
      if (typeId == 1) {
        this.getFactoryDetails(this.model.supplierId, this.model.customerId);
      }
      else if (typeId == 2) {
        this.getSuppliersbyCustomers(this.model.customerId);
      }
    });
  }

  //call after creating supplier/factory in the popup
  getFactoryDetails(supid, cusid) {
    if (supid > 0 && cusid > 0) {
      var bookingId = 0;
      this.service.GetSupplierDetailsByCusIdSupId(supid, cusid, bookingId)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 20) {
              if (!this.bookingMasterData.IsFactory) {
                if (res.factoryList != null) {
                  this.bookingMasterData.factoryList = res.factoryList;
                }
                else {
                  this.ResetFactoryDetails();
                }
              }
            }
          },
          error => {
            this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
          }
        );
    }
  }


  getSuppliersbyCustomers(customerId) {

    this.supplierService.getSuppliersbyCustomer(customerId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.bookingMasterData.supplierList = res.data;
          }
          else {
            this.bookingMasterData.supplierList = [];
          }
        },
        error => {
          this.bookingMasterData.supplierList = [];
        });
  }

  // #endregion Add Supplier/Factory Popup


  // #region Implement Abstract class from detail component

  getViewPath(): string {
    return "inspedit/view-booking";
  }
  getEditPath(): string {
    return "inspedit/edit-booking";
  }

  // #endregion Abstract class from detail component


  //#region Assign Booking information to preview page

  getArraywithCommaSeparated(SourceList, DestinationList) {
    var brandName = [];
    SourceList.forEach(item => {
      if (DestinationList.filter(x => x.id == item).length > 0) {
        brandName.push(DestinationList.filter(x => x.id == item)[0].name)
      }
    });
    return brandName.join(',');
  }

  AssignCustomerData() {
    if (this.bookingMasterData.Customerlist && this.bookingMasterData.Customerlist.length > 0 && this.model.customerId)
      this.bookingPreviewData.cusname = this.bookingMasterData.Customerlist.filter(x => x.id == this.model.customerId).length > 0 ?
        this.bookingMasterData.Customerlist.filter(x => x.id == this.model.customerId)[0].name : "";
  }

  AssignCustomerRelatedItems() {
    if (this.bookingMasterData.customerBrandList && this.bookingMasterData.customerBrandList.length > 0 && this.model.inspectionCustomerBrandList.length > 0) {
      this.bookingPreviewData.brand = this.getArraywithCommaSeparated(this.model.inspectionCustomerBrandList, this.bookingMasterData.customerBrandList);
    }

    if (this.bookingMasterData.customerBuyerList && this.bookingMasterData.customerBuyerList.length > 0 && this.model.inspectionCustomerBuyerList.length > 0) {
      this.bookingPreviewData.buyer = this.getArraywithCommaSeparated(this.model.inspectionCustomerBuyerList, this.bookingMasterData.customerBuyerList);
    }

    if (this.bookingMasterData.customerDepartmentList && this.bookingMasterData.customerDepartmentList.length > 0 && this.model.inspectionCustomerDepartmentList.length > 0) {
      this.bookingPreviewData.department = this.getArraywithCommaSeparated(this.model.inspectionCustomerDepartmentList, this.bookingMasterData.customerDepartmentList);

    }
    if (this.bookingMasterData.customerMerchandiserList && this.bookingMasterData.customerMerchandiserList.length > 0 && this.model.inspectionCustomerMerchandiserList.length > 0) {
      this.bookingPreviewData.merchandiser = this.getArraywithCommaSeparated(this.model.inspectionCustomerMerchandiserList, this.bookingMasterData.customerMerchandiserList);
    }

  }

  AssingSupplierData() {
    if (this.bookingMasterData.supplierList && this.bookingMasterData.supplierList.length > 0 && this.model.supplierId)
      this.bookingPreviewData.suppliername = this.bookingMasterData.supplierList.filter(x => x.id == this.model.supplierId).length > 0 ?
        this.bookingMasterData.supplierList.filter(x => x.id == this.model.supplierId)[0].name : "";

    if (this.bookingMasterData.supplierList && this.bookingMasterData.supplierList.length > 0 && this.model.supplierId)
      this.bookingPreviewData.supplierRegionName = this.bookingMasterData.supplierList.filter(x => x.id == this.model.supplierId).length > 0 ?
        this.bookingMasterData.supplierList.filter(x => x.id == this.model.supplierId)[0].regionalLanguageName : null;

  }

  AssignFactoryData() {
    if (this.bookingMasterData.factoryList && this.bookingMasterData.factoryList.length > 0 && this.model.factoryId)
      this.bookingPreviewData.factoryname = this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId).length > 0 ?
        this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId)[0].name : "";

    if (this.bookingMasterData.factoryList && this.bookingMasterData.factoryList.length > 0 && this.model.factoryId)
      this.bookingPreviewData.factoryRegionName = this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId).length > 0 ?
        this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId)[0].regionalLanguageName : null;

  }

  AssignOfficeData() {
    if (this.bookingMasterData.Office && this.bookingMasterData.Office.length > 0 && this.model.officeId)
      this.bookingPreviewData.Officename = this.bookingMasterData.Office.filter(x => x.id == this.model.officeId).length > 0 ?
        this.bookingMasterData.Office.filter(x => x.id == this.model.officeId)[0].name : "";
  }

  AssignCsNames() {
    if (this.bookingMasterData.csList && this.model.csList) {
      var csList = this.bookingMasterData.csList.filter(x => this.model.csList.includes(x.id));
      this.model.csNames = csList.map(x => x.name).join(',');
    }

  }

  SetPanelItem() {
    this.AssignOfficeData();

    this.AssignCustomerData()
    this.AssignCustomerRelatedItems();
    this.AssingSupplierData();
    this.AssignFactoryData();

    if (this.bookingMasterData.inspectionFileAttachments && this.bookingMasterData.inspectionFileAttachments.length > 0)
      this.bookingMasterData.totalattachedfile = this.bookingMasterData.inspectionFileAttachments.length;

    //assing aql name(critical,major,minor)

    if (this.bookingMasterData.bookingPOProductValidators && this.bookingMasterData.bookingPOProductValidators.length > 0) {

      this.bookingMasterData.bookingPOProductValidators.forEach(item => {

        item.poProductDetail.aqlName = this.bookingMasterData.bookingProductAqlList.filter(x => x.id == item.poProductDetail.aql).length > 0 ?
          this.bookingMasterData.bookingProductAqlList.filter(x => x.id == item.poProductDetail.aql)[0].value : "";

        //append critical,major minor values with aqlname
        var aqlPoints = [];
        if (item.poProductDetail.critical) {
          var critical = this.bookingMasterData.bookingProductPickList.filter(x => x.id == item.poProductDetail.critical);
          if (critical)
            aqlPoints.push(critical[0].value);
        }

        if (item.poProductDetail.major) {
          var major = this.bookingMasterData.bookingProductPickList.filter(x => x.id == item.poProductDetail.major);
          if (major)
            aqlPoints.push(major[0].value);
        }

        if (item.poProductDetail.minor) {
          var minor = this.bookingMasterData.bookingProductPickList.filter(x => x.id == item.poProductDetail.minor);
          if (minor)
            aqlPoints.push(minor[0].value);
        }

        var aqlValue = aqlPoints.length > 0 ? '(' + aqlPoints.join(',') + ')' : "";
        if (item.poProductDetail.sampleType) {
          var sampleData = this.bookingMasterData.customSampleTypeList.find(x => x.id == item.poProductDetail.sampleType);
          if (sampleData)
            item.poProductDetail.previewAqlName = item.poProductDetail.aqlName + " " + sampleData.sampleType + " " + aqlValue;
        }
        else
          item.poProductDetail.previewAqlName = item.poProductDetail.aqlName + aqlValue;

        item.poProductDetail.unitNameCount = "";

        item.poProductDetail.unitName = this.bookingMasterData.bookingProductUnitList.filter(x => x.id == item.poProductDetail.unit).length > 0 ?
          this.bookingMasterData.bookingProductUnitList.filter(x => x.id == item.poProductDetail.unit)[0].name : "";

        item.poProductDetail.unitNameCount = item.poProductDetail.unitCount && item.poProductDetail.unitCount > 0 ? item.poProductDetail.unitName

          + " (" + item.poProductDetail.unitCount + ")" : item.poProductDetail.unitName;

        item.poProductDetail.destinationCountryName = this.getDestinationCountry(item.poProductDetail.destinationCountryID);

        item.poProductDetail.containerName = this.getContainerName(item.poProductDetail.containerId);

        this.checkCountryAndFactoryReferenceExists();
      });
    }
    if (this.bookingMasterData.supplierContactList)
      this.setSupplierContactPreviewData(this.bookingMasterData.supplierContactList);
    if (this.bookingMasterData.factoryContactList)
      this.setFactoryContactPreviewData(this.bookingMasterData.factoryContactList);
    if (this.bookingMasterData.customerContactList)
      this.setCustomerContactPreviewData(this.bookingMasterData.customerContactList);


    if (this.isReInspection) {
      if (this.model.id)
        this.bookingPreviewData.previousBookingNo = this.model.id.toString();
      this.model.previousBookingNoList.push(this.model.id);
      //Remove duplicates and assign
      this.bookingPreviewData.previousBookingNoList = this.model.previousBookingNoList.filter((n, i) => this.model.previousBookingNoList.indexOf(n) === i);
    }

    if (this.model.previousBookingNo && !this.isReBooking && !this.isReInspection) {
      this.bookingPreviewData.previousBookingNo = this.model.previousBookingNo.toString();
      this.bookingPreviewData.previousBookingNoList = this.model.previousBookingNoList;
      // this.isReInspection = true;
    }
    this.bookingPreviewData.customerBookingNumber = this.model.customerBookingNo;

    this.CalculateTotalBookingQty();
    this.CalculateTotalPickingQuantity();

    this.AssignCsNames();
  }

  setpaneldate() {
    if (this.model.serviceDateFrom) {
      var fromdate: NgbDate = this.model.serviceDateFrom;
      this.bookingPreviewData.servicefromdatepanel = fromdate.day + "/" + fromdate.month + "/" + fromdate.year;
    }
    if (this.model.serviceDateTo) {
      var todate: NgbDate = this.model.serviceDateTo;
      this.bookingPreviewData.servicetodatepanel = todate.day + "/" + todate.month + "/" + todate.year;
    }

    // set first request date
    if (this.model.firstServiceDateFrom) {
      var fromdate: NgbDate = this.model.firstServiceDateFrom;
      this.bookingPreviewData.firstServicefromdatepanel = fromdate.day + "/" + fromdate.month + "/" + fromdate.year;
    }
    if (this.model.firstServiceDateTo) {
      var todate: NgbDate = this.model.firstServiceDateTo;
      this.bookingPreviewData.firstServicetodatepanel = todate.day + "/" + todate.month + "/" + todate.year;
    }
  }

  //#endregion Assign Booking information to preview page

  // #region function to move booking preview page
  bookingPreview() {
    if (this.bookingMasterData.userTypeId != UserType.InternalUser) {
      this.isPreviewPanelVisible = !this.isPreviewPanelVisible;
    }
    else {
      this.IsBookingPanelDataValid();
      //this.bookingMasterData.bookingPOProductValidators = this.bookingMasterData.bookingPOProductValidators.sort((a, b) => (a.poProductDetail.productName < b.poProductDetail.productName ? -1 : 1));
      if (!this.validator.isSubmitted) {
        this.isPreviewPanelVisible = !this.isPreviewPanelVisible;
      }
    }
    this.setpaneldate();
    this.SetPanelItem();
  }

  // #endregion function to move booking preview page

  // #region function to open panel from the preview page
  openPanelFromPreview(panelName) {
    this.isPreviewPanelVisible = false;
    this.getInitialSearchPoList();
    switch (panelName) {
      case this.panelData.Step1: {
        this.panelNavigation.stepOne = this.panelNavigationStatus.Active;
        this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepThree = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepFour = this.panelNavigationStatus.Completed;
      }
        break;
      case this.panelData.Step2: {
        this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepTwo = this.panelNavigationStatus.Active;
        this.panelNavigation.stepThree = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepFour = this.panelNavigationStatus.Completed;
      }
        break;
      case this.panelData.Step3: {
        this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepThree = this.panelNavigationStatus.Active;
        this.panelNavigation.stepFour = this.panelNavigationStatus.Completed;
      }
        break;
      case this.panelData.Step4: {
        this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepThree = this.panelNavigationStatus.Completed;
        this.panelNavigation.stepFour = this.panelNavigationStatus.Active;
      }
        break;
    }
  }
  // #endregion function to open panel from the preview page


  Initialize(id?) {

    this.model = new EditInspectionBooking();
    this.panelNavigation = new PanelNavigation();
    this.bookingMasterData.poList = null;

    this.bookingMasterData.officedetails = {};

    this.bookingMasterData.searchPoId = null;
    this.bookingMasterData.isPicking = false;

    this.bookingMasterData.poProductSourceList = [];
    this.bookingMasterData.poProductList = [];
    this.bookingMasterData.selectedPOName = "";
    this.bookingMasterData.ponumber = null;
    this.bookingMasterData.inspectionFileAttachments = [];
    this.model.inspectionFileAttachmentList = [];
    this.bookingMasterData.Initialloading = true;
    this.validator.isSubmitted = false;
    this.bookingMasterData.totalReports = null;
    this.bookingMasterData.totalSamplingSize = null;
    this.dynamicControls = [];
    this.bookingPreviewData.dfCustomerConfigPreviewBaseData = [];
    if (!id)
      this.getEntPageFieldAccessList();

    this.getEditBookingData(id);
    this.getCustomSampleSizeList();
  }

  //get the entity page field access list
  async getEntPageFieldAccessList() {
    //map the ent page field data
    var entPageRequest = this.mapEntPageField();

    var entPageFieldAccessResponse = await this.service.getEntPageAccessList(entPageRequest);

    if (entPageFieldAccessResponse.result == EntPageFieldAccessResult.Success) {
      this.entPageFieldAccess = entPageFieldAccessResponse.entPageFieldAccess;

      if (this.model.customerId)
        this.filterEntFieldAccess = this.entPageFieldAccess.filter(x => !x.customerId || x.customerId == this.model.customerId);
      else
        this.filterEntFieldAccess = this.entPageFieldAccess;
    }
    else {
      this.entPageFieldAccess = [];
    }
  }

  //map the ent page field data
  mapEntPageField() {
    var entPageRequest = new EntPageRequest();
    if (this.model.customerId)
      entPageRequest.customerId = this.model.customerId;
    entPageRequest.serviceId = Service.Inspection;
    entPageRequest.rightId = RightsEnum.BookingEdit;
    return entPageRequest;
  }

  //assign the configured page fields
  //assignConfiguredPageFields() {
  //  this.bookingProductTableMaster.isPoQtyVisible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_POQuantity, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isEcoPackVisible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_EcoPack, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isDisplayMasterVisible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_DisplayMaster, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isDisplayChildVisible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_DisplayChild, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isCustomerRefPOVisible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_CustomerRefPo, this.filterEntFieldAccess);

  //  this.bookingProductTableMaster.isPicking = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_Picking, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isCombine = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_Combine, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isBarcode = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_Product_Barcode, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isFactoryReference = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_Product_Factory_Reference, this.filterEntFieldAccess);
  //  this.bookingProductTableMaster.isFirstServiceDate = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_First_Service_Date, this.filterEntFieldAccess);

  //  this.bookingProductTableMaster.isProductSubCategory2Visible = this.utility.showPageField(this.entPageFieldAccessEnum.Booking_Edit_Product_SubCategory2, this.filterEntFieldAccess);
  //}



  getEditBookingData(bookingId) {
    this.service.EditBooking(bookingId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.processEditBookingSuccessResponse(res);
          }
          else {
            this.ErrorDetails(res);
          }
          this.GetCustomerByUserType(bookingId);
          this.isBookingProductDelete();
          this.checkAqlCustomSelected();
          this.bookingMasterData.Initialloading = false;
        },
        error => {
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
          this.bookingMasterData.Initialloading = false;
          console.log(error != null ? error : "");
        }
      );
  }

  processEditBookingSuccessResponse(responseData) {
    if (responseData && responseData.bookingDetails) {

      this.bookingMasterData.poProductDependentFilter = responseData.bookingDetails.poProductDependentFilter;
      this.mapEntityDataToClientModel(responseData);

      var customerId = responseData.bookingDetails.customerId;
      var supplierId = responseData.bookingDetails.supplierId;
      var factoryId = responseData.bookingDetails.factoryId;

      this.bookingMasterData.currentStatusId = this.model.statusId;

      // check if the service type belongs to container
      if (ContainerServiceList.includes(this.model.serviceTypeId))
        this.isContainerService = true;

      this.bookingMasterData.sampleSizeVisible = true;

      //fetch the dropdown source(only for edit booking case) includes (booking mapped data and all active data)
      if (this.model.id && !this.isReInspection && !this.isReBooking) {
        this.getEditCustomerRelatedDetails(customerId);
        this.getEditBookingOffices();
        this.getEditBookingUnits();
        this.getEditBookingBusinessLines();
        this.getPickingRelatedDetails();

        if (this.model.businessLine == this._businessLine.SoftLine) {
          this.getEditBookingCuProductCategory();
          this.getEditBookingSeason();
          this.getEditBookingInspectionLocations();
          this.getEditBookingShipmentTypes();
        }
        this.getServiceTypeList();

        this.getCsConfigList();

        this.getBookingFileAttachment();

      }
      else if (this.isReInspection || this.isReBooking) {
        this.GetCusRelatedDetailsById(customerId, true);
      }
      this.getEntPageFieldAccessList();
      this.getInspectionPaymentOptions();

      var bookingId = (!this.isReInspection && !this.isReBooking) ? responseData.bookingDetails.id : 0;

      //add booking id to filter booking mapped inactive supplier contacts(only in edit case)
      this.getSupRelatedDetailsById(supplierId, customerId, bookingId, true);

      //add booking id to filter booking mapped inactive factory contacts(only in edit case)
      this.geFactRelatedDetailsById(factoryId, customerId, bookingId, true);

      this.GetInspBookingContact(factoryId, customerId);
      this.GetInspBookingRuleDetails(customerId, factoryId ? factoryId : "");

      this.bookingMasterData.isPicking = this.model.isPickingRequired;

      this.processReInspectionDetails(customerId, responseData.userType);
      this.processReBookingDetails(responseData.userType);
      this.addEntityProductListToClientModel();

      if (!this.isReBooking && !this.isReInspection) {
        this.setDateDisabled();
        this.applyGroupColor();
      }

      this.CalculateTotalBookingQty();
      this.CalculateTotalPickingQuantity();
      this.CalculateTotalProducts();
      this.CalculateTotalSamplingSize();

      this.CalculateTotalContainers();
      // set container count while editing
      this.model.containerLimit = this.bookingMasterData.totalContainers;
      this.getContainerList(this.model.containerLimit);

      this.setPrimaryProduct();

      this.addPOandProductDataSourceToProductList();
      this.CalculateTotalPurchaseOrder();

      this.getInspectionHoldReasonTypes();

      this.processDisplayMasterProducts();

      this.setpaneldate();

    }
    else {
      this.bookingMasterData.Initialloading = false;
      this.NewBookingDefaultDataByUser(responseData);
    }
    this.SetUserType();
    if (UserType.InternalUser == this.bookingMasterData.userTypeId) {
      this.showRoleWarningMessage(responseData);
    }

  }

  async getCsConfigList() {
    var csConfigListResponse = await this.getCSNames();
    if (csConfigListResponse && csConfigListResponse.result == CSConfigListResult.Success) {
      this.bookingMasterData.csList = csConfigListResponse.csList;
    }
    else if (csConfigListResponse && csConfigListResponse.result == CSConfigListResult.RequestNotValid) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_AE_NAME');
    }
  }

  mapEntityDataToClientModel(responseData) {
    if (responseData.bookingDetails) {
      var entity = responseData.bookingDetails;
      this.model = {
        id: entity.id,
        guidId: entity.guidId,
        isEmailRequired: false,
        isFlexibleInspectionDate: false,
        factoryId: entity.factoryId,
        supplierId: entity.supplierId,
        customerId: entity.customerId,
        statusId: entity.statusId,
        statusName: entity.statusName,
        seasonId: entity.seasonId,
        seasonYearId: entity.seasonYearId,
        officeId: entity.officeId,
        entityId: entity.entityId,
        serviceTypeId: entity.serviceTypeId,
        serviceDateFrom: entity.serviceDateFrom,
        serviceDateTo: entity.serviceDateTo,
        firstServiceDateFrom: entity.firstServiceDateFrom,
        firstServiceDateTo: entity.firstServiceDateTo,
        cusBookingComments: entity.cusBookingComments,
        apiBookingComments: entity.apiBookingComments,
        internalComments: entity.internalComments,
        qcBookingComments: entity.qcBookingComments,
        applicantName: entity.applicantName,
        applicantEmail: entity.applicantEmail,
        applicantPhoneNo: entity.applicantPhoneNo,
        inspectionProductList: entity.inspectionProductList,
        inspectionCustomerContactList: entity.inspectionCustomerContactList,
        inspectionFactoryContactList: entity.inspectionFactoryContactList,
        inspectionFileAttachmentList: entity.inspectionFileAttachmentList,
        inspectionServiceTypeList: entity.inspectionServiceTypeList,
        inspectionSupplierContactList: entity.inspectionSupplierContactList,
        inspectionCustomerBrandList: entity.inspectionCustomerBrandList,
        inspectionCustomerBuyerList: entity.inspectionCustomerBuyerList,
        inspectionCustomerDepartmentList: entity.inspectionCustomerDepartmentList,
        createdBy: entity.createdBy,
        previousBookingNo: entity.previousBookingNo,
        reinspectionType: entity.reinspectionType,
        isBookingRequestRole: responseData.isBookingRequestRole,
        isBookingConfirmRole: responseData.isBookingConfirmRole,
        isBookingVerifyRole: responseData.isBookingVerifyRole,
        isPickingRequired: entity.isPickingRequired,
        isCombineRequired: entity.isCombineRequired != null ? entity.isCombineRequired : false,
        isBookingInvoiced: responseData.isBookingInvoiced,
        customerBookingNo: this.isReInspection || this.isReBooking ? "" : entity.customerBookingNo,
        inspectionPageType: null,
        splitPreviousBookingNo: null,
        isTermsChecked: false,
        isCustomerEmailSend: false,
        isSupplierOrFactoryEmailSend: true,
        inspectionDFTransactions: entity.inspectionDfTransactions,
        inspectionCustomerMerchandiserList: entity.inspectionCustomerMerchandiserList,
        CreatedUserType: entity.createdUserType,
        containerLimit: null,
        collectionId: entity.collectionId,
        priceCategoryId: entity.priceCategoryId,
        customerCollectionName: entity.collectionName,
        bookingType: entity.bookingType,
        paymentOptions: entity.paymentOptions,
        customerPriceCategoryName: entity.priceCategoryName,
        compassBookingNo: entity.compassBookingNo,
        previousBookingNoList: entity.previousBookingNoList,
        csNames: entity.csNames ? entity.csNames : "-",
        isCombineDone: entity.isCombineDone,
        isPickingDone: entity.isPickingDone,
        holdReasonTypeId: null,
        holdReasonType: entity.holdReasonType,
        holdReason: entity.holdReason,
        businessLine: entity.businessLine,
        inspectionLocation: entity.inspectionLocation,
        shipmentPort: entity.shipmentPort,
        shipmentDate: entity.shipmentDate,
        ean: entity.ean,
        cuProductCategory: entity.cuProductCategory,
        shipmentTypeIds: entity.shipmentTypeList,
        csList: entity.csList,
        draftInspectionId: null,
        isUpdateBookingDetail: false,
        isEaqf: entity.isEaqf,
        daCorrelationId: entity.gapdaCorrelation ? this.bookingMasterData.daCorrelationEnum.yes : this.bookingMasterData.daCorrelationEnum.no,
        gapDACorrelation: entity.gapdaCorrelation,
        gapDAEmail: entity.gapdaEmail,
        gapDAName: entity.gapdaName
      };

      this.bookingMasterData.inspectionFileAttachments = this.model.inspectionFileAttachmentList.filter(x => x.fileAttachmentCategoryId != FileAttachmentCategory.Gap)

      this.bookingMasterData.gapFileAttachments = this.model.inspectionFileAttachmentList.filter(x => x.fileAttachmentCategoryId == FileAttachmentCategory.Gap)

      if (!this.isReInspection && !this.isReBooking && (this.model.statusId == InspectedStatusList.Inspected || this.model.statusId == InspectedStatusList.ReportSent))
        this._bookingIsInspected = true;

      this.bookingMasterData.totalReports = entity.totalNumberofReports;
      this.bookingMasterData.bookingServiceConfig = responseData.bookingServiceConfig;

      this.bookingMasterData.totalManDays = responseData.totalMandays;

      this.setProductImageURL(entity.inspectionProductList);
    }

  }

  addEntityProductListToClientModel() {
    this.model.inspectionProductList.forEach(item => {

      item.productCategoryMapped = false;
      item.productSubCategoryMapped = false;
      item.productCategorySub2Mapped = false;
      item.productCategorySub3Mapped = false;

      if (item.productCategoryId != 0 && item.productCategoryId != null) {
        item.productCategoryMapped = true;
      }
      if (item.productSubCategoryId != 0 && item.productSubCategoryId != null) {
        item.productSubCategoryMapped = true;
      }
      if (item.productCategorySub2Id != 0 && item.productCategorySub2Id != null) {
        item.productCategorySub2Mapped = true;
      }
      if (item.productCategorySub3Id != 0 && item.productCategorySub3Id != null) {
        item.productCategorySub3Mapped = true;
      }
      //if picking is available then show the picking data
      if (this.model.isPickingRequired)
        item.togglePicking = false;
      //loop through the saveinspection picking list and add to inspectionPickingValidators
      //with validators
      if (item.saveInspectionPickingList && item.saveInspectionPickingList.length > 0) {

        item.inspectionPickingValidators = [];
        item.saveInspectionPickingList.forEach(pickingData => {
          //if lab type is customer then assign to -1
          if (pickingData.labType == LabType.Customer)
            pickingData.labOrCustomerId = -1;
          pickingData.labOrCustomerContactIds = [];
          pickingData.pickingContactList.forEach(contactData => {
            pickingData.labOrCustomerContactIds.push(contactData.labOrCustomerContactId);
          });

          //add the picking validators
          var pickingDetailValidator = { pickingData: pickingData, validator: Validator.getValidator(pickingData, "booking/edit-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };
          item.inspectionPickingValidators.push(pickingDetailValidator);
        });
      }

      this.bookingMasterData.bookingPOProductValidators.push({ poProductDetail: item, validator: Validator.getValidator(item, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

    });
  }

  addPOandProductDataSourceToProductList() {
    this.bookingMasterData.editBookingPoProductLoading = true;
    this.bookingMasterData.editBookingPoLoading = true;
    this.bookingMasterData.bookingPOProductValidators.forEach(productTransaction => {

      productTransaction.poProductDetail.isVisible = true;

      this.getPoListBySearch(productTransaction);

      this.getPoProductListBySearch(productTransaction);

      this.updateCriticalNameToPoProductItem(productTransaction.poProductDetail.critical, productTransaction);

      this.updateMajorNameToPoProductItem(productTransaction.poProductDetail.major, productTransaction);

      this.updateMinorDataToPoProductItem(productTransaction.poProductDetail.minor, productTransaction);

      if (productTransaction.poProductDetail.isPrimaryProductInGroup) {
        //show combined aql qty if it is combined
        //else aql qty for sampling size field
        productTransaction.poProductDetail.previewSamplingSize = productTransaction.poProductDetail.aqlQuantity;
        if (productTransaction.poProductDetail.combineGroupId != null) {
          productTransaction.poProductDetail.previewSamplingSize = productTransaction.poProductDetail.combineSamplingSize;
        }
      }

      //add po data to common po list datasource
      if (!this.bookingMasterData.selectedPOList.find(x => x.id == productTransaction.poProductDetail.poId))
        this.bookingMasterData.selectedPOList.push({ "id": productTransaction.poProductDetail.poId, "name": productTransaction.poProductDetail.poName });
      this.bookingMasterData.selectedPOList = this.bookingMasterData.selectedPOList.sort((a, b) => (a.name < b.name ? -1 : 1));
    });
  }


  //get the unit master list(along with booking mapped units)
  getEditBookingUnits() {
    this.service.getEditBookingUnit(this.model.id)
      .subscribe(
        response => {
          this.processBookingMappedUnit(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_UNIT_FOUND');
          this.bookingMasterData.cuscontactloading = false;
        });
  }
  //process the booking mapped units
  processBookingMappedUnit(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.bookingProductUnitList = response.dataSourceList;
      this.bookingMasterData.bookingProductUnitList = [...this.bookingMasterData.bookingProductUnitList];
    }
  }

  //get the booking mapped offices
  getEditBookingOffices() {
    this.service.getEditBookingOffice(this.model.id)
      .subscribe(
        response => {
          this.processBookingMappedOffice(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_OFFICE_FOUND');
          this.bookingMasterData.cuscontactloading = false;
        });
  }

  //process the booking mapped offices
  processBookingMappedOffice(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.Office = response.dataSourceList;
      this.bookingMasterData.Office = [...this.bookingMasterData.Office];
    }
  }


  checkContainersAreEqual(): boolean {
    // set container count while editing
    this.CalculateTotalContainers();
    return (this.bookingMasterData.totalContainers == this.model.containerLimit)

  }

  getUniqueColorList() {
    var resultColorSets = ['00ceff1a', 'dc67ce1a'];

    return resultColorSets;
  }

  getUniqueCombineOrder() {
    let groupIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.combineGroupId != null)
      .map(x => x.poProductDetail.combineGroupId);
    let distinctGroupIds = groupIds.filter((n, i) => groupIds.indexOf(n) === i);
    return distinctGroupIds;
  }

  applyGroupColor() {
    var count = 0;

    var uniqueCombineProductList = this.getUniqueCombineOrder();
    var uniqueColorList = this.getUniqueColorList();

    uniqueCombineProductList.forEach(item => {

      var productList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.combineGroupId == item);
      productList.forEach(row => {

        if (row.poProductDetail.combineGroupId == item) {
          if (count % 2 == 0)
            row.colorCode = "#" + uniqueColorList[0];
          else if (count % 2 != 0)
            row.colorCode = "#" + uniqueColorList[1];
        }

      });
      count = count + 1;

    });

    /* this.bookingMasterData.bookingProductList.sort((a, b) =>
      (a.combineGroupId != null ? a.combineGroupId : Infinity) -
      (b.combineGroupId != null ? b.combineGroupId : Infinity)
      || (a.poId != null ? a.poId : Infinity) -
      (b.poId != null ? b.poId : Infinity)); */

  }

  setProductImageURL(poProductList) {

    var productIds = poProductList.map(x => x.productId);

    var distinctProductIds = productIds.filter((n, i) => productIds.indexOf(n) === i);

    distinctProductIds.forEach(productId => {

      var productData = this.model.inspectionProductList.find(x => x.productId == productId);

      if (productData.productFileAttachments && productData.productFileAttachments.length > 0 && productData) {

        productData.productFileAttachments.forEach(item => {

          if (item.fileUrl) {
            //if (item.fileUrl) {
            this.bookingMasterData.bookingProductFiles.push({
              "productId": productData.productId,
              "productName": productData.productName,
              "productDesc": productData.productDesc,
              "fileName": item.fileName,
              "uniqueld": item.uniqueld,
              "file": item
            });
          }

        });

      }

    });

  }

  updateConfirmBooking() {
    this.modelRef.close();
  }

  confirmORVerifyBooking(content, bookingStatus) {
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {
      this.model.statusId = bookingStatus;
      this.save();

    }, (reason) => {
    });
  }
  // open hold booking popup
  openHoldBooking(content) {
    this.validator.isSubmitted = false;
    this.model.holdReason = "";
    this.model.holdReasonType = null;
    this.model.holdReasonTypeId = null;
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  openConfirmEmailPopup(content) {
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  //check hold booking reason is valid or mandatory
  isHoldBookingReasonValid() {
    this.validator.isSubmitted = true;
    return this.validator.isValid("holdReasonTypeId") && this.validator.isValid("holdReason");
  }

  // update hold status
  holdBookingConfirm() {

    if (this.isHoldBookingReasonValid()) {
      this.modelRef.close();
      this.model.statusId = BookingStatus.Hold;
      this.save();
    }
  }

  sendConfirmEmail() {
    this.modelRef.close();
    this.confirmEmail();
  }

  showRoleWarningMessage(role) {
    if (!role.isBookingRequestRole && !role.isBookingVerifyRole && (this.model.statusId == BookingStatus.New)) {
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_REQUEST_ROLE_NOT_AVAILABLE', true);
    }
    else if (!role.isBookingConfirmRole && this.model.statusId == BookingStatus.Verified) {
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CONFIRM_ROLE_NOT_AVAILABLE', true);
    }

    else if (!role.isBookingVerifyRole && (this.model.statusId == BookingStatus.Requested)) {
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_VERIFY_ROLE_NOT_AVAILABLE', true);
    }
  }

  NewBookingDefaultDataByUser(res) {
    //send bookingid as 0 to fetch only the active suppliers(since it calls for new booking)
    var bookingId = 0;
    this.bookingPreviewData = new BookingPreviewData();
    switch (res.userType) {
      case UserType.Customer:
        {
          this.model.statusId = BookingStatus.New;
          this.getUserApplicantDetails();
        }
        break;
      case UserType.Supplier:
        // if supplier login assign supplierList at page load
        this.bookingMasterData.supplierList = res.supplierList.length > 0 ? res.supplierList : null;
        this.model.supplierId = this.model.supplierId == null && res.supplierList.length > 0 ? res.supplierList[0].id : this.model.supplierId;
        this.model.statusId = BookingStatus.New;
        this.getSupRelatedDetailsById(this.model.supplierId, null, bookingId, false);
        this.getUserApplicantDetails();
        break;
      case UserType.Factory:
        {
          //factory login assign factoryList at page load
          this.bookingMasterData.factoryList = res.factoryList.length > 0 ? res.factoryList : null;
          this.model.factoryId = this.model.factoryId == null && res.factoryList.length > 0 ? res.factoryList[0].id : this.model.factoryId;
          this.model.statusId = BookingStatus.New;
          this.geFactRelatedDetailsById(this.model.factoryId, null, bookingId, false);
          this.GetInspBookingContact(this.model.factoryId, this.model.customerId);
          this.getUserApplicantDetails();
          break;
        }
      case UserType.InternalUser:
        this.model.statusId = BookingStatus.New;
        this.model.isBookingRequestRole = res.isBookingRequestRole;
        this.model.isBookingConfirmRole = res.isBookingConfirmRole;
        this.model.isBookingVerifyRole = res.isBookingVerifyRole;
        this.getBookingStaffDetails();
        break;
    }
  }
  SetUserType() {

    this.panelNavigation.stepOne = this.panelNavigationStatus.Active;
    switch (this.currentUser.usertype) {
      case UserType.Customer:
        {
          this.bookingMasterData.IsCustomer = true;
          this.bookingMasterData.IsSaveBtnShow = this.model && this.model.statusId != null
            && ((this.model.statusId == BookingStatus.New) || (this.model.statusId == BookingStatus.Requested)) ? true : false;
        }
        break;
      case UserType.Supplier:
        {
          this.bookingMasterData.Issupplier = true;
          this.bookingMasterData.IsSaveBtnShow = this.model && this.model.statusId != null &&
            (this.model.statusId == BookingStatus.New || this.model.statusId == BookingStatus.Requested) ? true : false;
          break;
        }

      case UserType.Factory:
        {
          this.bookingMasterData.IsFactory = true;
          this.bookingMasterData.IsSaveBtnShow = this.model && this.model.statusId != null &&
            (this.model.statusId == BookingStatus.New || this.model.statusId == BookingStatus.Requested) ? true : false;
          break;
        }
      case UserType.InternalUser:
        {
          // Added role base show / hide

          this.bookingMasterData.IsverifyBtnShow = this.model && this.model.statusId != null &&
            this.model.statusId != BookingStatus.Cancel &&
            (this.model.statusId == BookingStatus.Requested) //|| this.model.statusId == BookingStatus.Postpone)
            && (this.model.isBookingVerifyRole) ? true : false;

          // Add condition for hold button visible
          this.bookingMasterData.IsholdBtnShow = this.model && this.model.statusId != null &&
            (this.model.statusId == BookingStatus.Verified || this.model.statusId == BookingStatus.AllocateQC
              || this.model.statusId == BookingStatus.Confirmed || this.model.statusId == BookingStatus.Rescheduled)
            && (this.model.isBookingVerifyRole) ? true : false;

          // Add condition for confirm email button visible
          this.bookingMasterData.IsConfirmEmainBtnShow = this.model && this.model.statusId != null &&
            (this.model.statusId == BookingStatus.Confirmed || this.model.statusId == BookingStatus.AllocateQC)
            && (this.model.isBookingConfirmRole) ? true : false;

          this.bookingMasterData.IsconfirmBtnShow = this.model && this.model.statusId != null &&
            this.model.statusId != BookingStatus.Cancel &&
            (this.model.statusId == BookingStatus.Verified || this.model.statusId == BookingStatus.Hold ||
              this.model.statusId == BookingStatus.Rescheduled) &&
            (this.model.isBookingConfirmRole) ? true : false;

          this.bookingMasterData.IsSaveBtnShow = (this.model &&
            (this.model.isBookingRequestRole || this.model.isBookingVerifyRole)
            && this.model.statusId != BookingStatus.Cancel) ? true : false;
          break;
        }

    }
    if (this.model && this.model.id != null && this.model.id != 0 && (this.bookingMasterData.Issupplier || this.bookingMasterData.IsFactory) && this.bookingMasterData.IsSaveBtnShow && this.model.createdBy != this.currentUser.id
      && !this.isReInspection && !this.isReBooking) {
      this.bookingMasterData.IsSaveBtnShow = false;
    }
    if (this.model.statusId == BookingStatus.Cancel) {
      this.bookingMasterData.IsSaveBtnShow = false;
    }
    this.bookingMasterData.Initialloading = false;
    this.bookingMasterData.issuccess = false;
  }
  getCusRelatedDetails(cusitem) {
    this.filterEntFieldAccess = this.entPageFieldAccess.filter(x => !x.customerId || x.customerId == this.model.customerId);

    this.ResetCustomerChange();
    if (cusitem != null && cusitem.id != null) {
      this.GetCusRelatedDetailsById(cusitem.id, false);

      this.model.inspectionCustomerContactList = [];
      this.model.inspectionCustomerBrandList = []
      this.model.inspectionCustomerDepartmentList = [];
      this.model.inspectionCustomerBuyerList = [];
      this.model.inspectionCustomerMerchandiserList = [];
    }
  }
  GetCusRelatedDetailsById(cusid, isEdit) {
    this.bookingMasterData.customerContactList = []
    this.bookingMasterData.customerBrandList = [];
    this.bookingMasterData.customerDepartmentList = [];
    this.bookingMasterData.customerBuyerList = [];
    this.bookingMasterData.customerMerchandiserList = [];
    this.bookingMasterData.seasonList = [];
    if (this.currentUser.usertype != UserType.Supplier)
      this.bookingMasterData.supplierList = [];
    this.bookingMasterData.supplierContactList = [];
    this.bookingMasterData.customerCollection = [];
    this.bookingMasterData.customerPriceCategory = [];
    this.bookingPreviewData.suppliercode = "";
    this.bookingMasterData.factoryContactList = "";
    this.bookingPreviewData.factcode = "";
    this.bookingMasterData.customerPriceCategory = [];
    this.dynamicControls = [];
    this.bookingMasterData.isCustomerBrandAvailable = false;
    this.bookingMasterData.isCustomerBuyerAvailable = false;
    this.bookingMasterData.isCustomerDeptAvailable = false;
    this.bookingMasterData.isCustomerMerchandiserAvailable = false;
    this.bookingMasterData.isCustomerCollectionAvailable = false;
    this.bookingMasterData.isCustomerPriceCategoryAvailable = false;
    this.spacebeforeCusContact = false;
    this.spacebeforeCusBookingNo = false;
    var count = 0; //count to check if brand, buyer, dept and merchandiser are true. if any 3 are true, then add space on customer contact

    this.bookingMasterData.cusloading = true;
    this.bookingMasterData.supplierloading = !this.bookingMasterData.Issupplier ? true : false;
    this.getCustomerProductCategories();
    this.getCustomerSeasonConfig();
    this.bookingMasterData.pageLoader = true;
    this.getCustomerContactByBrandOrDept();
    this.getCustomerCheckPointList();
    this.getInspectionPaymentOptions();
    this.service.GetBookingDetailsByCusId(cusid)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 19) {
            this.bookingMasterData.pageLoader = false;
            if (res.customerBrandList) {
              this.bookingMasterData.customerBrandList = res.customerBrandList;
              this.bookingMasterData.isCustomerBrandAvailable = true;
              count++;
            }
            if (res.customerDepartmentList) {
              this.bookingMasterData.customerDepartmentList = res.customerDepartmentList;
              this.bookingMasterData.isCustomerDeptAvailable = true;
              count++;
            }
            if (res.customerBuyerList) {
              this.bookingMasterData.customerBuyerList = res.customerBuyerList;
              this.bookingMasterData.isCustomerBuyerAvailable = true;
              count++;
            }
            if (res.customerMerchandiserList) {
              this.bookingMasterData.customerMerchandiserList = res.customerMerchandiserList;
              this.bookingMasterData.isCustomerMerchandiserAvailable = true;
              count++;
            }

            //to provide margin on top only if it moves to next line.
            if (count >= 3)
              this.spacebeforeCusContact = true;
            if (count >= 2)
              this.spacebeforeCusBookingNo = true;

            if (res.collection) {
              this.bookingMasterData.customerCollection = res.collection;
              this.bookingMasterData.isCustomerCollectionAvailable = true;
            }

            if (res.priceCategory) {
              this.bookingMasterData.customerPriceCategory = res.priceCategory;
              this.bookingMasterData.isCustomerPriceCategoryAvailable = true;
            }
            //Assign Booking Default Comments to the internal comments in case of add booking scenario
            this.assignBookingDefaultComments(isEdit, res);
            this.bookingMasterData.seasonList = res.seasonList;

            this.bookingMasterData.supplierList = isEdit || !this.bookingMasterData.Issupplier ?
              res.supplierList : this.bookingMasterData.supplierList;
            if (this.bookingMasterData.Issupplier && !isEdit) {
              // if supplier login factory details should load after customer select
              this.getSupRelatedDetailsById(this.model.supplierId, cusid, 0, isEdit);
              if (this.bookingMasterData.applicantContactId > 0) {
                if (res.supplierContactList.length == 1) {
                  // set first item by default if we have only one contact
                  this.model.inspectionSupplierContactList = [res.supplierContactList[0].contactId];
                }
                else if (res.supplierContactList.length > 1) {
                  // check applicant contact is present in the contact list
                  var selectedItem = res.supplierContactList.find(x => x.contactId == this.bookingMasterData.applicantContactId);
                  if (selectedItem) {
                    this.model.inspectionSupplierContactList = [selectedItem.contactId];
                  }
                }
              }
              this.bookingMasterData.supplierContactList = res.supplierContactList;
              this.bookingPreviewData.suppliercode = res.supplierCode;
            }
            if (this.bookingMasterData.IsFactory && !isEdit) {
              this.bookingMasterData.factoryContactList = res.factoryContactList;
              this.bookingPreviewData.factcode = res.factoryCode;
              if (this.bookingMasterData.applicantContactId > 0) {
                if (res.factoryContactList.length == 1) {
                  // set first item by default if we have only one contact
                  this.model.inspectionFactoryContactList = [res.factoryContactList[0].contactId];
                }
                else if (res.factoryContactList.length > 1) {
                  // check applicant contact is present in the contact list
                  var selectedItem = res.factoryContactList.find(x => x.contactId == this.bookingMasterData.applicantContactId);
                  if (selectedItem) {
                    this.model.inspectionFactoryContactList = [selectedItem.contactId];
                  }
                }
              }
              this.model.officeId = this.model.officeId == 0 || this.model.officeId == null ? (res.officeId == 0 ? null : res.officeId) : this.model.officeId;
              this.GetInspBookingRuleDetails(cusid, this.model.factoryId);
              // this.GetAuditCSDetails(this.model.factoryid, cusid);
            }

            //Get the customer configured dynamic fields
            this.GetCustomerDynamicFields(cusid, this._dynamicControlModuleType.InspectionBooking);
            this.AssignCustomerRelatedItems();
            this.getServiceTypeList();
          }
          else {
            this.ErrorDetails(res);
            this.bookingMasterData.pageLoader = false;
          }
          this.bookingMasterData.pageLoader = false;
          this.bookingMasterData.cusloading = false;
          this.bookingMasterData.supplierloading = false;
          if (isEdit) {
            this.SetPanelItem();
          }
        },
        error => {
          this.bookingMasterData.pageLoader = false;
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
          this.bookingMasterData.cusloading = false;
          this.bookingMasterData.supplierloading = false;
          console.log(error);
        }
      );
  }
  //calls when supplier change
  getSupRelatedDetails(supitem, isEdit) {
    var bookingId = 0;
    this.bookingMasterData.officedetails.officeName = null;
    this.ResetSupplierDetails();
    this.resetBookingProducts();
    if (supitem != null && supitem.id != null) {
      var cusid = this.model.customerId == null || this.model.customerId == 0 ? null : this.model.customerId;
      this.model.supplierId = supitem.id;
      this.getSupRelatedDetailsById(supitem.id, cusid, bookingId, isEdit);

    }

  }
  getSupRelatedDetailsById(supid, cusid, bookingId, isEdit) {
    if (supid > 0 && cusid > 0) {
      this.bookingMasterData.suploading = true;
      this.bookingMasterData.factoryloading = !this.bookingMasterData.IsFactory ? true : false;
      this.AssingSupplierData();
      this.service.GetSupplierDetailsByCusIdSupId(supid, cusid, bookingId)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 20) {
              this.bookingPreviewData.supplierPhoneNumber = res.supplierPhoneNumber;

              this.bookingMasterData.supplierContactList = res.supplierContactList;
              if (!isEdit) {
                if (res.supplierContactList.length == 1)
                  this.model.inspectionSupplierContactList = [res.supplierContactList[0].contactId];
              }
              //set supplier contact details in preview page
              if (res.supplierContactList) {
                this.setSupplierContactPreviewData(res.supplierContactList);
              }
              this.bookingPreviewData.suppliercode = res.supplierCode;
              if (isEdit || !this.bookingMasterData.IsFactory) {
                if (res.factoryList != null) {
                  this.bookingMasterData.factoryList = res.factoryList;
                  //set factory name in preview page in edit scenario
                  if (this.bookingMasterData.factoryList && this.bookingMasterData.factoryList.length > 0 && this.model.factoryId)
                    this.bookingPreviewData.factoryname = this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId).length > 0 ?
                      this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId)[0].name : "";


                  if (this.bookingMasterData.factoryList && this.bookingMasterData.factoryList.length > 0 && this.model.factoryId)
                    this.bookingPreviewData.factoryRegionName = this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId).length > 0 ?
                      this.bookingMasterData.factoryList.filter(x => x.id == this.model.factoryId)[0].regionalLanguageName : "";
                }
                else {
                  this.bookingMasterData.factoryList = [];
                  this.ResetFactoryDetails();
                  res.result = 14; // factory details are empty.
                  this.ErrorDetails(res);
                }
              }
            }
            else {
              this.ErrorDetails(res);
            }
            this.bookingMasterData.suploading = false;
            this.bookingMasterData.factoryloading = false;
          },
          error => {
            this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
            this.bookingMasterData.suploading = false;
            this.bookingMasterData.factoryloading = false;
            console.log(error);
          }
        );
    }
  }

  //get factory related changes on factory change
  geFactRelatedDetails(factitem) {
    var bookingId = 0;
    this.model.inspectionFactoryContactList = [];
    if (factitem != null && factitem.id != null) {
      this.isHasFactory = true

      this.AssignFactoryData();
      var cusid = this.model.customerId == null || this.model.customerId == 0 ? null : this.model.customerId;
      !this.bookingMasterData.IsFactory ?
        this.geFactRelatedDetailsById(factitem.id, cusid, bookingId, false) : null;
      this.GetInspBookingContact(factitem.id, cusid);
      // this.GetAuditCSDetails(factitem.id, cusid);
      if (cusid != null)
        this.GetInspBookingRuleDetails(cusid, factitem.id);
    }
    else {
      this.isHasFactory = false;
    }
  }
  geFactRelatedDetailsById(factid, cusid, bookingId, IsEdit) {
    if (cusid && factid) {
      this.bookingMasterData.factloading = true;
      this.service.GetFactoryDetailsByCusIdFactId(factid, cusid, bookingId)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 23) {
              this.bookingMasterData.factoryContactList = res.factoryContactList;
              if (!IsEdit) {
                if (res.factoryContactList.length == 1)
                  this.model.inspectionFactoryContactList = [res.factoryContactList[0].contactId];
              }
              //set factory contact list
              if (res.factoryContactList) {
                this.setFactoryContactPreviewData(res.factoryContactList);
              }
              if (this.bookingMasterData.IsFactory)
                this.bookingMasterData.supplierList = IsEdit ? this.bookingMasterData.supplierList : res.supplierList;
              this.bookingPreviewData.factcode = res.factoryCode;
              this.bookingPreviewData.factaddress = res.factoryAddress;
              this.bookingPreviewData.phoneNumber = res.phoneNumber;
              this.bookingPreviewData.factRegaddress = res.factoryRegionalAddress;

              // only factory change event and new booking scenario if the factory change then assign office id.
              if(!IsEdit && res.officeId>0)
              this.model.officeId = res.officeId;

              this.SetPanelItem();
              this.setpaneldate();
            }
            else {
              this.ErrorDetails(res);
            }
            this.bookingMasterData.factloading = false;
          },
          error => {
            this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
            this.bookingMasterData.factloading = false;
            console.log(error);
          }
        );
    }
  }
  ErrorDetails(res) {
    switch (res.result) {
      case 2:
        this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_CUSTOMER');
        break;
      case 5:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_CUSTOMER_CONTACT');
        break;
      case 7:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_CUSTOMER_SERVICETYPE');
        break;
      case 6:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANONT_GET_CUSTOMER_SEASON');
        break;
      case 8:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        break;
      case 13:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_SUPPLIER');
        break;
      case 21:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_SUPPLIER_CONTACT');
        break;
      case 14:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_FACTORY');
        break;
      case 24:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_FACTORY_CONTACT');
        break;
      case 10:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_LOCATION');
        break;
      case 28:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_BOOKINGRULE');
        break;
      case 29:
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_BOOKING_INFO');

    }
  }
  ResetDetailsByCustomerId() {
    switch (this.bookingMasterData.userTypeId) {
      case UserType.Customer:
        {
          this.model.inspectionCustomerContactList = [];
          this.model.inspectionCustomerBrandList = null
          this.model.inspectionCustomerDepartmentList = null;
          this.model.inspectionCustomerBuyerList = null;
          this.model.inspectionCustomerMerchandiserList = null;
          this.model.seasonId = null;
          this.model.supplierId = null;
          this.bookingMasterData.isCustomerBrandAvailable = false;
          this.bookingMasterData.isCustomerBuyerAvailable = false;
          this.bookingMasterData.isCustomerDeptAvailable = false;
          this.bookingMasterData.isCustomerMerchandiserAvailable = false;
          this.ResetSupplierDetails();
          break;
        }
      case UserType.Supplier:
        {
          this.bookingMasterData.customerContactList = [];
          this.model.inspectionCustomerContactList = [];
          this.bookingMasterData.customerBrandList = [];
          this.model.inspectionCustomerBrandList = null
          this.model.inspectionCustomerDepartmentList = null;
          this.model.inspectionCustomerBuyerList = null;
          this.model.inspectionCustomerMerchandiserList = null;
          this.bookingMasterData.customerDepartmentList = [];
          this.bookingMasterData.customerBuyerList = [];
          this.bookingMasterData.customerServiceList = [];
          this.bookingMasterData.seasonList = [];
          this.bookingMasterData.customerBuyerList = [];
          this.model.seasonId = null;
          this.bookingMasterData.supplierContactList = [];
          this.model.inspectionSupplierContactList = [];
          this.bookingPreviewData.suppliercode = "";
          this.bookingMasterData.isCustomerBrandAvailable = false;
          this.bookingMasterData.isCustomerBuyerAvailable = false;
          this.bookingMasterData.isCustomerDeptAvailable = false;
          this.bookingMasterData.isCustomerMerchandiserAvailable = false;
          this.ResetFactoryDetails();
          break;
        }
      case UserType.Factory:
        {
          this.bookingMasterData.customerContactList = [];
          this.model.inspectionCustomerContactList = [];
          this.bookingMasterData.customerBrandList = [];
          this.bookingMasterData.customerDepartmentList = [];
          this.bookingMasterData.customerBuyerList = [];
          this.bookingMasterData.customerServiceList = [];

          this.model.inspectionCustomerBrandList = null
          this.model.inspectionCustomerDepartmentList = null;
          this.model.inspectionCustomerBuyerList = null;
          this.model.inspectionCustomerMerchandiserList = null;


          this.bookingMasterData.seasonList = [];
          this.model.seasonId = null;
          this.bookingMasterData.factoryContactList = [];
          this.model.inspectionFactoryContactList = [];
          this.bookingPreviewData.factcode = "";
          this.bookingMasterData.isCustomerBrandAvailable = false;
          this.bookingMasterData.isCustomerBuyerAvailable = false;
          this.bookingMasterData.isCustomerDeptAvailable = false;
          this.bookingMasterData.isCustomerMerchandiserAvailable = false;
          this.ResetSupplierDetails();
          break;
        }
      case UserType.InternalUser:
        this.Initialize();
        this.bookingPreviewData.suppliercode = "";
        this.bookingPreviewData.factcode = "";
        this.bookingPreviewData.factaddress = "";
        this.bookingPreviewData.factRegaddress = "";
        this.bookingMasterData.customerContactList = [];
        this.model.inspectionCustomerContactList = [];
        this.bookingMasterData.isCustomerBrandAvailable = false;
        this.bookingMasterData.isCustomerBuyerAvailable = false;
        this.bookingMasterData.isCustomerDeptAvailable = false;
        this.bookingMasterData.isCustomerMerchandiserAvailable = false;
        this.dynamicControls = [];
        break;
    }

  }
  //if we changed customer drop down value we need to clear related data.
  ResetCustomerChange() {
    //clear supplier data if supplier is not login
    if (!this.bookingMasterData.Issupplier)
      this.model.supplierId = null;

    //clear factory data  if factory is not login
    if (!this.bookingMasterData.IsFactory) {
      this.model.factoryId = null;
      this.bookingPreviewData.factaddress = "";
      this.bookingPreviewData.factRegaddress = "";
      this.bookingPreviewData.phoneNumber = "";
      this.bookingPreviewData.supplierPhoneNumber = "";
    }
    this.model.inspectionSupplierContactList = [];
    this.bookingMasterData.supplierContactList = [];
    this.bookingPreviewData.suppliercode = "";
    this.model.inspectionFactoryContactList = [];
    this.bookingMasterData.factoryContactList = [];
    this.bookingPreviewData.factcode = "";
    this.bookingMasterData.selectedPOList = [];
    this.bookingMasterData.officedetails.officeName = null;
  }
  ResetSupplierDetails() {

    switch (this.bookingMasterData.userTypeId) {
      case UserType.Customer:
        {
          this.bookingMasterData.supplierContactList = [];
          this.model.inspectionSupplierContactList = [];
          this.bookingPreviewData.suppliercode = "";
          this.bookingMasterData.factoryList = [];
          this.model.factoryId = null;
          this.ResetFactoryDetails();
          break;
        }
      case UserType.Factory:
        {
          this.model.supplierId = null;
          this.bookingMasterData.supplierContactList = [];
          this.model.inspectionSupplierContactList = [];
          this.bookingPreviewData.suppliercode = "";
          break;
        }
      case UserType.InternalUser:
        this.bookingMasterData.supplierContactList = [];
        this.model.inspectionSupplierContactList = [];
        this.bookingPreviewData.suppliercode = "";
        this.bookingMasterData.factoryList = [];
        this.model.factoryId = null;
        this.ResetFactoryDetails();
        break;
    }
  }
  ResetFactoryDetails() {
    this.model.factoryId = null;
    this.bookingMasterData.factoryContactList = [];
    this.model.inspectionFactoryContactList = [];
    this.bookingPreviewData.factcode = "";
    this.bookingPreviewData.factaddress = "";
    this.bookingPreviewData.factRegaddress = "";

  }
  onserviceDateFromSelection(date: NgbDate) {
    //if servicedateto is not visible then assing servicedatefrom to servicedateto value
    if (!this.bookingMasterData.showServiceDateTo && this.model.serviceDateFrom != null) {
      this.model.serviceDateTo = this.model.serviceDateFrom;
      if (this.isReInspection || this.isReBooking)
        this.model.firstServiceDateTo = this.model.serviceDateFrom;
    }

    if (this.model.serviceDateTo == null && this.model.serviceDateFrom != null) {
      this.model.serviceDateTo = this.model.serviceDateFrom;
      if (this.model.statusId == BookingStatus.New) {
        this.model.firstServiceDateTo = this.model.serviceDateFrom;
      }
    }
    if (this.model.serviceDateTo < this.model.serviceDateFrom) {
      this.model.serviceDateTo = null;
      if (this.model.statusId == BookingStatus.New) {
        this.model.firstServiceDateTo = null;
      }
    }
    if (this.model.statusId == BookingStatus.New) {
      this.model.firstServiceDateFrom = this.model.serviceDateFrom;
    }
  }

  onserviceDateToSelection(date: NgbDate) {
    if (this.model.serviceDateTo != null && this.model.statusId == BookingStatus.New) {
      this.model.firstServiceDateTo = this.model.serviceDateTo;
    }
  }

  onFirstDateSelection(date: NgbDate) {
    if (this.model.firstServiceDateTo == null && this.model.firstServiceDateFrom != null) {
      this.model.firstServiceDateTo = this.model.firstServiceDateFrom;
    }
    if (this.model.firstServiceDateTo < this.model.firstServiceDateFrom) {
      this.model.firstServiceDateTo = null;
    }
  }


  //#region Delete PO Popup Part

  //open the delete po list popup and populating the data
  deletePO(content) {
    this.bookingMasterData.disableAllPOChecked = false;
    this.bookingMasterData.isDeleteAllPOChecked = true;
    this.bookingMasterData.selectAllDeletePOChecked = false;

    this.productValidateMaster.totalProducts = null;
    this.productValidateMaster.deleteSuccessCount = 0;
    this.productValidateMaster.validationFailedCount = 0;


    this.bookingMasterData.selectedDeletePoDetails = [];
    this.bookingMasterData.selectedCommonPONo = null;

    this.modelRef = this.modalService.open(content, { windowClass: "add-booking-product-wrapper", centered: true, backdrop: 'static' });

  }

  importBulkPurchaseOrder(content) {
    var existingBookingPoProductList = new Array<ExistingBookingPoProductData>()
    if (this.bookingMasterData.bookingPOProductValidators && this.bookingMasterData.bookingPOProductValidators.length > 0) {
      this.bookingMasterData.bookingPOProductValidators.forEach(element => {
        if (element.poProductDetail.poId && element.poProductDetail.productId) {
          var existingBookingPoProductData = new ExistingBookingPoProductData();
          existingBookingPoProductData.poId = element.poProductDetail.poId;
          existingBookingPoProductData.productId = element.poProductDetail.productId;
          existingBookingPoProductList.push(existingBookingPoProductData);
        }
      });
    }

    this.modalInputData = {
      customerId: this.model.customerId,
      supplierId: this.model.supplierId,
      businessLineId: this.model.businessLine,
      existingBookingPoProductList: existingBookingPoProductList
    };

    this.modelRef = this.modalService.open(content, { windowClass: "add-booking-product-wrapper", centered: true, backdrop: 'static' });

    this.modelRef.result.then((result) => {
    }, (reason) => { });
  }

  addPoProductUploadDataToBookingProducts(data) {
    var poProductDetail = new InspectionPOProductDetail();

    this.bookingMasterData.bookingPOProductValidators = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.poId && x.poProductDetail.productId);

    var unitData = this.bookingMasterData.bookingProductUnitList.find(x => x.id == this.bookingUnit.Pieces);

    if (unitData)
      poProductDetail.unit = unitData.id;

    poProductDetail.bookingQuantity = data.quantity;

    poProductDetail.poQuantity = data.quantity;

    poProductDetail.etd = data.etd;

    poProductDetail.destinationCountryID = data.destinationCountryId;


    poProductDetail.colorCode = data.colorCode;
    poProductDetail.colorName = data.colorName;

    this.model.inspectionProductList.push(poProductDetail);
    this.validator.isSubmitted = false;



    var item = { poProductDetail: poProductDetail, validator: Validator.getValidator(poProductDetail, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };


    item.poProductDetail.poId = data.poData[0].id;
    item.poProductDetail.poName = data.poData[0].name;

    item.poProductDetail.productId = data.productData[0].id;
    item.poProductDetail.productName = data.productData[0].productName;

    item.poProductDetail.barcode = data.productData[0].barcode;
    item.poProductDetail.factoryReference = data.productData[0].factoryReference;
    item.poProductDetail.productDesc = data.productData[0].productDescription;

    if (!this.isContainerService) {

      this.applyAqlValueToPoProduct(this.bookingMasterData.bookingServiceConfig, item.poProductDetail);

      if (data.productData[0].productCategoryId)
        item.poProductDetail.productCategoryMapped = true;


      if (data.productData[0].productSubCategoryId) {
        var subCategoryList = new Array<commonDataSource>();
        var subCategory = new commonDataSource();
        subCategory.id = data.productData[0].productSubCategoryId;
        subCategory.name = data.productData[0].productSubCategoryName;
        subCategoryList.push(subCategory);

        item.poProductDetail.bookingCategorySubProductList = subCategoryList;
        item.poProductDetail.productSubCategoryMapped = true;
      }

      if (data.productData[0].productSubCategory2Id) {
        var subCategory2List = new Array<commonDataSource>();
        var subCategory2 = new commonDataSource();
        subCategory2.id = data.productData[0].productSubCategory2Id;
        subCategory2.name = data.productData[0].productSubCategory2Name;
        subCategory2List.push(subCategory2);
        item.poProductDetail.bookingCategorySub2ProductList = subCategory2List;
        item.poProductDetail.productCategorySub2Mapped = true;
      }

      if (data.productData[0].productSubCategory3Id) {
        var subCategory3List = new Array<commonDataSource>();
        var subCategory3 = new commonDataSource();
        subCategory3.id = data.productData[0].productSubCategory3Id;
        subCategory3.name = data.productData[0].productSubCategory3Name;
        subCategory3List.push(subCategory3);

        item.poProductDetail.bookingCategorySub3ProductList = subCategory3List;
        item.poProductDetail.productCategorySub3Mapped = true;
      }



      //item.poProductDetail.bookingCategorySubProductList

      item.poProductDetail.productCategoryId = data.productData[0].productCategoryId;
      item.poProductDetail.productSubCategoryId = data.productData[0].productSubCategoryId;
      item.poProductDetail.productCategorySub2Id = data.productData[0].productSubCategory2Id;
      item.poProductDetail.productCategorySub3Id = data.productData[0].productSubCategory3Id;
      item.poProductDetail.productCategoryName = data.productData[0].productCategoryName;
      item.poProductDetail.productSubCategoryName = data.productData[0].productSubCategoryName;
      item.poProductDetail.productCategorySub2Name = data.productData[0].productSubCategory2Name;
      item.poProductDetail.productCategorySub3Name = data.productData[0].productSubCategory3Name;

    }

    this.bookingMasterData.poDataSourceList = [];
    this.bookingMasterData.poProductDataSourceList = [];

    this.bookingMasterData.editBookingPoLoading = true;
    //item.poProductDetail.poList = data.poData;

    //this.bookingMasterData.poDataSourceList = data.productData;
    this.getPoListBySearch(item);

    this.bookingMasterData.editBookingPoProductLoading = true;

    var productDataSource = new Array<commonDataSource>();
    var product = new commonDataSource();

    product.id = data.productData[0].id;
    product.name = data.productData[0].productName;
    productDataSource.push(product);
    item.poProductDetail.productList = productDataSource;

    //this.bookingMasterData.poProductDataSourceList = data.productData;
    this.getPoProductListBySearch(item);

    this.bookingMasterData.bookingPOProductValidators.push(item);

    this.setPrimaryProduct();

    this.setPrimaryProductData(item);

    this.CalculateTotalProducts();
    this.CalculateTotalPurchaseOrder();
    this.CalculateTotalBookingQty();
    this.CalculateTotalContainers();

  }

  closeUploadPoProductFromBooking(data) {
    data.forEach(element => {
      //this.bookingMasterData.poDataSourceList = element.poData;
      this.addPoProductUploadDataToBookingProducts(element);
      //var productdata=data;
    });
    /* var poProductData = this.bookingMasterData.bookingPOProductValidators[this.bookingMasterData.selectedPoProductIndex];

    var productDetail = data[0];
    //this.bookingMasterData.poProductDataSourceList = data;
    this.bookingMasterData.poProductDataSourceList = [];
    this.bookingMasterData.poProductDataSourceList.push({ "id": productDetail.id, "name": productDetail.productID });

    this.assignProductRelatedData(poProductData, productDetail);

    this.getCustomerProductListBySearch(poProductData); */

    this.modelRef.close();

  }

  addDeletePOProductList() {
    var selectedpoDetail = new selectedDeletePoDetail();
    this.bookingMasterData.selectedDeletePoDetails = [];
    this.bookingMasterData.selectAllDeletePOChecked = true;

    var rowIndex = 0;

    var productPoList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.poId == this.bookingMasterData.selectedCommonPONo);

    if (productPoList && productPoList.length > 0) {

      productPoList.forEach(poDetail => {

        selectedpoDetail = new selectedDeletePoDetail();
        selectedpoDetail.poTransactionId = poDetail.poProductDetail.poTransactionId;
        selectedpoDetail.poId = poDetail.poProductDetail.poId;
        selectedpoDetail.poName = poDetail.poProductDetail.poName;
        selectedpoDetail.productId = poDetail.poProductDetail.productId;
        selectedpoDetail.productName = poDetail.poProductDetail.productName;
        selectedpoDetail.productDesc = poDetail.poProductDetail.productDesc;
        selectedpoDetail.poQty = poDetail.poProductDetail.bookingQuantity;
        selectedpoDetail.containerId = poDetail.poProductDetail.containerId;
        selectedpoDetail.containerName = poDetail.poProductDetail.containerName;
        selectedpoDetail.etd = poDetail.poProductDetail.etd ? this.utility.formatDate(poDetail.poProductDetail.etd) : "";
        selectedpoDetail.destinationCountry = poDetail.poProductDetail.destinationCountryName;
        selectedpoDetail.colorCode = poDetail.poProductDetail.colorCode;
        selectedpoDetail.colorName = poDetail.poProductDetail.colorName;
        selectedpoDetail.remarks = "";
        selectedpoDetail.index = poDetail.poProductDetail.poId + "-" + rowIndex;
        selectedpoDetail.isPoSelected = true;
        this.bookingMasterData.selectedDeletePoDetails.push(selectedpoDetail);
        rowIndex++;

      });

      var activePOList = this.bookingMasterData.selectedDeletePoDetails.filter(x => x.isPoSelected);
      if (activePOList)
        this.productValidateMaster.totalProducts = activePOList.length;

    }
  }

  async deletePOProducts() {
    //var canDelete = true;
    this.bookingMasterData.isDeleteAllPOChecked = false;

    if (this.bookingMasterData.selectedDeletePoDetails && this.bookingMasterData.selectedDeletePoDetails.length > 0) {
      //take the selected po list in the popup
      var poProductList = this.bookingMasterData.selectedDeletePoDetails.filter(x => x.isPoSelected == true);
      //proceed if polist is not empty
      if (poProductList && poProductList.length > 0) {

        this.productValidateMaster.totalProducts = poProductList.length;
        this.bookingMasterData.deletePopupLoading = true;

        //check only the booking is in edit mode
        if (this.model.id > 0 && !this.isReInspection && !this.isReBooking) {

          //initialize the productvalidatedata
          var productValidateData = new Array<ProductValidateData>();

          //push the data to productvalidatedata from the existing po list
          poProductList.forEach(element => {
            if (element.poTransactionId) {
              var request = new ProductValidateData();
              request.bookingId = this.model.id;
              request.poTransactionId = element.poTransactionId;
              productValidateData.push(request);
            }
          });

          if (productValidateData && productValidateData.length > 0)
            //get the validation result from the server side call
            var result = await this.service.bookingProductsValidation(productValidateData);

          //process the product validation result
          this.processProductValidationResult(result, poProductList);

        }
        //remove the newly added po list
        this.removeNewPOList(poProductList);

        if (result && result.length > 0) {
          var errorPoTransactionIds = result.filter(x => x.pickingExists || x.quotationExists || x.reportExists).map(x => x.poTransactionId);

          var errorPoList = this.bookingMasterData.selectedDeletePoDetails.filter(x => errorPoTransactionIds.includes(x.poTransactionId));

          if (errorPoList && errorPoList.length > 0) {

            errorPoList.map(x => x.status = ProductValidationStatus.Error);
            this.bookingMasterData.selectAllDeletePOChecked = false;
            if (this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Pending).length == 0)
              this.bookingMasterData.disableAllPOChecked = true;
          }

          var successPoTransIds = result.filter(x => !x.pickingExists && !x.quotationExists && !x.reportExists).map(x => x.poTransactionId);

          var successPoList = this.bookingMasterData.selectedDeletePoDetails.filter(x => successPoTransIds.includes(x.poTransactionId));

          if (successPoList && successPoList.length > 0) {
            successPoList.map(x => x.remarks = this.utility.textTranslate("EDIT_BOOKING.LBL_DELETE_SUCCESS"));
            successPoList.map(x => x.status = ProductValidationStatus.Success);
            successPoList.map(x => x.isPoSelected = false);
            successPoList.map(x => x.disablePO = true);
          }

          this.productValidateMaster.validationFailedCount = errorPoList.length;

        }


        if (this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Pending).length == 0
          && this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Error).length == 0) {
          this.bookingMasterData.selectedPOList = this.bookingMasterData.selectedPOList.filter(x => x.id != this.bookingMasterData.selectedCommonPONo);
          this.bookingMasterData.selectedCommonPONo = null;
          this.clearCommonPoNo();
          this.modelRef.close();
        }

        if (this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Pending).length == 0
          && this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Error).length > 0) {
          this.bookingMasterData.disableAllPOChecked = true;
          this.bookingMasterData.selectAllDeletePOChecked = false;
        }

        //set primary product for the products
        this.setPrimaryProduct();
        this.calculateCustomAQLQuantity();

        //calculate the total booking quantity
        this.CalculateTotalBookingQty();
        //calculate the total picking quantity
        this.CalculateTotalPickingQuantity();

        this.bookingMasterData.selectedPOList = this.bookingMasterData.selectedPOList.sort((a, b) => (a.name < b.name ? -1 : 1));

        this.CalculateTotalProducts();

        var activePOList = this.bookingMasterData.selectedDeletePoDetails.filter(x => x.isPoSelected);
        if (activePOList)
          this.productValidateMaster.totalProducts = activePOList.length;

        this.bookingMasterData.deletePopupLoading = false;

      }
      else {
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_ATLEAST_ONE_PRODUCT');
      }
    }

    if (this.bookingMasterData.bookingPOProductValidators.length == 0)
      this.addToBookingProducts();
  }

  //calculate the custom aql quantity for all the products
  calculateCustomAQLQuantity() {
    this.bookingMasterData.bookingPOProductValidators.forEach(productData => {

      if (productData.poProductDetail.isPrimaryProductInGroup && productData.poProductDetail.sampleType) {
        this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleTypeName);
        this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleSize);
      }

    });
  }

  processProductValidationResult(result: Array<ProductValidateData>, poProductList: Array<selectedDeletePoDetail>) {
    if (result && result.length > 0) {

      //process quotation remarks validation
      this.processQuotationRemarksValidation(result, poProductList);

      //process picking remarks validation
      this.processPickingRemarksValidation(result, poProductList);

      //process report remarks validation
      this.processReportRemarksValidation(result, poProductList);

      //remove the po list
      this.removePOList(result);

    }
  }

  //assign validation remarks base on the validation type
  assignPOValidationRemarks(poProductList, validationType) {
    if (validationType == ValidationType.quotationExists)
      poProductList.map(x => x.remarks = '<ul><li>' + this.utility.textTranslate('EDIT_BOOKING.MSG_QUOTATION_EXISTS') + '</li></ul>');
    else if (validationType == ValidationType.pickingExists)
      poProductList.map(x => x.remarks = ((x.remarks && x.remarks.length > 0) ?
        (x.remarks + '<ul><li>' + this.utility.textTranslate('EDIT_BOOKING.MSG_PICKING_EXISTS') + '</li></ul>') :
        '<ul><li>' + this.utility.textTranslate('EDIT_BOOKING.MSG_PICKING_EXISTS') + '</li></ul>'));
    else if (validationType == ValidationType.reportExists)
      poProductList.map(x => x.remarks = ((x.remarks && x.remarks.length > 0) ?
        (x.remarks + '<ul><li>' + this.utility.textTranslate('EDIT_BOOKING.MSG_REPORT_EXISTS') + '</li></ul>') :
        '<ul><li>' + this.utility.textTranslate('EDIT_BOOKING.MSG_REPORT_EXISTS') + '</li></ul>'));
    poProductList.map(x => x.isPoSelected = false);
    poProductList.map(x => x.disablePO = true);
  }

  processQuotationRemarksValidation(result, poProductList) {
    //add the remarks for the quotation validation
    var quotationData = result.filter(x => x.quotationExists);
    if (quotationData && quotationData.length > 0) {
      this.assignPOValidationRemarks(poProductList, ValidationType.quotationExists);
    }
  }

  processPickingRemarksValidation(result, poList) {
    //add the remarks for the picking validation
    var pickingPoTransactionIds = result.filter(x => x.pickingExists).map(x => x.poTransactionId);
    if (pickingPoTransactionIds && pickingPoTransactionIds.length > 0) {
      var poPickingList = poList.filter(x => pickingPoTransactionIds.includes(x.poTransactionId));
      this.assignPOValidationRemarks(poPickingList, ValidationType.pickingExists);
    }
  }

  processReportRemarksValidation(result, poList) {
    //add the remarks for the report valdiation
    var reportTransactionIds = result.filter(x => x.reportExists).map(x => x.poTransactionId);
    if (reportTransactionIds && reportTransactionIds.length > 0) {
      var reportList = poList.filter(x => reportTransactionIds.includes(x.poTransactionId));
      this.assignPOValidationRemarks(reportList, ValidationType.reportExists);
    }
  }

  removePOList(result) {

    //get the potransactionids which is ready to delete(which came as a result of validation server side)
    var deletePoTransactionIds = result.filter(x => !x.quotationExists && !x.pickingExists && !x.reportExists).map(x => x.poTransactionId);

    for (var i = 0; i < deletePoTransactionIds.length; i++) {
      //get the selected popup podetail using potransaction id
      var popupPODetail = this.bookingMasterData.selectedDeletePoDetails.find(x => x.poTransactionId == deletePoTransactionIds[i]);
      if (popupPODetail) {
        this.deleteSelectedPO(popupPODetail);
      }
    }

    this.setPrimaryProduct();

  }

  removeNewPOList(poProductList) {
    //take the newly add po list where potransacion id is 0
    var newlyAddedPoList = poProductList.filter(x => x.poTransactionId == 0);

    if (newlyAddedPoList && newlyAddedPoList.length > 0) {

      for (var i = 0; i < newlyAddedPoList.length; i++) {
        //delete the po
        this.deleteSelectedPO(newlyAddedPoList[i]);

      }

      this.setPrimaryProduct();

    }
  }

  deleteSelectedPO(popupPODetail) {

    var poIndex = -1;

    if (popupPODetail.containerId > 0)
      poIndex = this.bookingMasterData.bookingPOProductValidators.findIndex(x => x.poProductDetail.productId == popupPODetail.productId && x.poProductDetail.poId == popupPODetail.poId
        && x.poProductDetail.containerId == popupPODetail.containerId);
    else if (popupPODetail.colorCode)
      poIndex = this.bookingMasterData.bookingPOProductValidators.findIndex(x => x.poProductDetail.productId == popupPODetail.productId && x.poProductDetail.poId == popupPODetail.poId
        && x.poProductDetail.colorCode == popupPODetail.colorCode);
    else
      poIndex = this.bookingMasterData.bookingPOProductValidators.findIndex(x => x.poProductDetail.productId == popupPODetail.productId && x.poProductDetail.poId == popupPODetail.poId);

    if (poIndex >= 0) {

      //delete the po item by poindex
      this.bookingMasterData.bookingPOProductValidators.splice(poIndex, 1);

      var productExistsinBooking = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == popupPODetail.productId);

      if (!productExistsinBooking) {

        var productFiles = this.bookingMasterData.bookingProductFiles.filter(x => x.productId != popupPODetail.productId);

        this.bookingMasterData.bookingProductFiles = [...productFiles];

      }

      //assign the success remarks
      popupPODetail.remarks = this.utility.textTranslate("EDIT_BOOKING.LBL_DELETE_SUCCESS");
      //disable to checkbox
      popupPODetail.disablePO = true;
      //uncheck the checkbox
      popupPODetail.isPoSelected = false;
      //assign the success status
      popupPODetail.status = ProductValidationStatus.Success;
      //add the success count
      this.productValidateMaster.deleteSuccessCount = this.productValidateMaster.deleteSuccessCount + 1;
    }

  }

  //event for select all delete polist in the popup
  selectAllDeletePoCheckBox() {
    this.bookingMasterData.isDeleteAllPOChecked = false;
    this.bookingMasterData.selectAllDeletePOChecked = !this.bookingMasterData.selectAllDeletePOChecked;
    this.bookingMasterData.selectedDeletePoDetails.filter(x => x.status == ProductValidationStatus.Pending).forEach(element => {
      element.isPoSelected = this.bookingMasterData.selectAllDeletePOChecked ?
        true : false;
    });
    if (this.bookingMasterData.selectAllDeletePOChecked)
      this.bookingMasterData.isDeleteAllPOChecked = true;
    var poLength = this.bookingMasterData.selectedDeletePoDetails.filter(element => element.isPoSelected).length;
    this.productValidateMaster.totalProducts = poLength;
    this.productValidateMaster.deleteSuccessCount = 0;
    this.productValidateMaster.validationFailedCount = 0;

  }

  checkAllSelected(item) {
    this.bookingMasterData.isDeleteAllPOChecked = false;
    var poActiveLength = this.bookingMasterData.selectedDeletePoDetails.filter(element => element.isPoSelected).length;
    var poLength = this.bookingMasterData.selectedDeletePoDetails.length;
    if (poLength == poActiveLength)
      this.bookingMasterData.selectAllDeletePOChecked = true;
    else
      this.bookingMasterData.selectAllDeletePOChecked = false;
    if (poActiveLength > 0)
      this.bookingMasterData.isDeleteAllPOChecked = true;
    this.productValidateMaster.totalProducts = poActiveLength;
    this.productValidateMaster.deleteSuccessCount = 0;
    this.productValidateMaster.validationFailedCount = 0;
  }

  //#endregion

  //#region Common Fields
  //change the common po list dropdown
  changeCommonPONo(event) {
    if (event)
      this.bookingMasterData.selectedCommonPOName = event.name;
    this.addDeletePOProductList();
    //this.filterBookingProductList();
  }

  //filter the booking product list
  filterBookingProductList() {
    if (this.bookingMasterData.selectedCommonPONo) {


      this.bookingMasterData.bookingPOProductValidators.map(x => x.poProductDetail.isVisible = false);

      var poProductDetails = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.poId == this.bookingMasterData.selectedCommonPONo);

      if (poProductDetails && poProductDetails.length > 0) {

        poProductDetails.forEach(productTrans => {
          productTrans.poProductDetail.isVisible = true;
        });
      }

    }
  }

  //clear the common pono
  clearCommonPoNo() {
    this.bookingMasterData.bookingPOProductValidators.map(x => x.poProductDetail.isVisible = true);
  }

  changeParentContainer(event) {

    if (event) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.containerId)
          productTrans.poProductDetail.containerId = event.id;
      });

    }
  }

  changeParentAql(event) {

    if (event) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.aql)
          productTrans.poProductDetail.aql = event.id;
      });

    }
  }

  changeParentCritical(event) {

    if (event) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.critical)
          productTrans.poProductDetail.critical = event.id;
      });

    }
  }

  changeParentMajor(event) {

    if (event) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.major)
          productTrans.poProductDetail.major = event.id;
      });

    }
  }

  changeParentMinor(event) {

    if (event) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.minor)
          productTrans.poProductDetail.minor = event.id;
      });

    }
  }

  changeCommonField(event, content, commonFieldType) {
    var openPopup = true;
    if (event) {

      if (commonFieldType == CommonFieldType.FBTemplate) {
        this.bookingMasterData.parentFbTemplateName = event.name;
        var _fbTemplateData = this.bookingMasterData.fbTemplateList.filter(x => x.id == event.id)[0];
        this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
          " " + _fbTemplateData.name + " " +
          this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
      }
      else if (commonFieldType == CommonFieldType.ETD) {
        var _parentEtd = this.bookingMasterData.parentEtd;
        var _commonparentEtd = _parentEtd.day + "/" + _parentEtd.month + "/" + _parentEtd.year;

        this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
          " " + _commonparentEtd + " " +
          this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
      }
      else if (commonFieldType == CommonFieldType.ProductCategory) {

        var productCategoryNotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productCategoryMapped);

        if (productCategoryNotMappedList && productCategoryNotMappedList.length > 0) {
          this.bookingMasterData.parentProductCategoryName = event.name;
          this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
            " " + event.name + " " +
            this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');

        }
        else {

          var productSubCategory2NotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productCategorySub2Mapped);

          if (productSubCategory2NotMappedList && productSubCategory2NotMappedList.length > 0) {

            var productCategory = { "id": this.bookingMasterData.parentProductCategoryId, "name": this.bookingMasterData.parentProductCategoryName };
            this.getParentProductSubcategoryList(productCategory);
            openPopup = false;
          }
          else {
            openPopup = false;
            this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_PRD_MAP_WITH_PRD_CATEGORY');
          }
        }
      }
      else if (commonFieldType == CommonFieldType.ProductSubCategory) {

        var productSubCategoryNotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productSubCategoryMapped);

        if (productSubCategoryNotMappedList && productSubCategoryNotMappedList.length > 0) {
          this.bookingMasterData.parentProductSubCategoryName = event.name;
          this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
            " " + event.name + " " +
            this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
        }
        else {

          var productSubCategory2NotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productCategorySub2Mapped);

          if (productSubCategory2NotMappedList && productSubCategory2NotMappedList.length > 0) {
            var productSubCategory = { "id": this.bookingMasterData.parentProductSubCategoryId, "name": this.bookingMasterData.parentProductSubCategoryName };
            this.getParentProductSubCategoryDetails(productSubCategory);
            openPopup = false;
          }
          else {
            openPopup = false;
            this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_PRD_MAP_WITH_PRD_SUB_CATEGORY');
          }
        }
      }
      else if (commonFieldType == CommonFieldType.ProductSubCategory2) {

        var productSubCategory2NotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productCategorySub2Mapped);

        if (productSubCategory2NotMappedList && productSubCategory2NotMappedList.length > 0) {
          this.bookingMasterData.parentProductCategorySub2Name = event.name;
          this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
            " " + event.name + " " +
            this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
        }
        else {
          openPopup = false;
          this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_PRD_MAP_WITH_PRD_SUB_CATEGORY2');
        }
      }
      else if (commonFieldType == CommonFieldType.ProductSubCategory3) {

        var productSubCategory3NotMappedList = this.bookingMasterData.bookingPOProductValidators.filter(x => !x.poProductDetail.productCategorySub3Mapped);

        if (productSubCategory3NotMappedList && productSubCategory3NotMappedList.length > 0) {
          this.bookingMasterData.parentProductCategorySub3Name = event.name;
          this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
            " " + event.name + " " +
            this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
        }
        else {
          openPopup = false;
          this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_NO_PRD_MAP_WITH_PRD_SUB_CATEGORY3');
        }
      }
      else if (commonFieldType == CommonFieldType.DestinationCountry) {
        this.bookingMasterData.parentDestinationCountryName = event.countryName;
        this.commonfieldApplyMsg = this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_UPDATE') +
          " " + event.countryName + " " +
          this.utility.textTranslate('EDIT_BOOKING.MSG_COMMONFEILD_IN_TABLE');
      }

      if (openPopup) {
        this._commonfieldType = commonFieldType;
        this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
        this.modelRef.result.then((result) => {
        }, (reason) => {
        });
      }

    }
  }

  applyCommonField() {

    if (this._commonfieldType == this._commonFieldTypeEnum.FBTemplate) { //Commonfeild FB template id apply to all booking Product List

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        productTrans.poProductDetail.fbTemplateId = this.bookingMasterData.parentFbTemplateId;
        productTrans.poProductDetail.fbTemplateName = this.bookingMasterData.parentFbTemplateName;
      });


    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ETD) {//Commonfeild Etd apply to all booking Product List

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        productTrans.poProductDetail.etd = this.bookingMasterData.parentEtd;
      });


      this.model.inspectionProductList.forEach(productTrans => {
        productTrans.etd = this.bookingMasterData.parentEtd;
      });

    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductCategory) {

      this.clearParentProductCategoryRelatedChanges();
      var productCategory = { "id": this.bookingMasterData.parentProductCategoryId, "name": this.bookingMasterData.parentProductCategoryName };
      this.getParentProductSubcategoryList(productCategory);

    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory) {
      var productSubCategory = { "id": this.bookingMasterData.parentProductSubCategoryId, "name": this.bookingMasterData.parentProductSubCategoryName };
      this.getParentProductSubCategoryDetails(productSubCategory);
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory2) {

      var productSubCategory2 = { "id": this.bookingMasterData.parentProductCategorySub2Id, "name": this.bookingMasterData.parentProductCategorySub2Name };
      this.changeParentProductSubCategory2(productSubCategory2);
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory3) {
      var productSubCategory3 = { "id": this.bookingMasterData.parentProductCategorySub3Id, "name": this.bookingMasterData.parentProductCategorySub3Name };
      this.updateParentProductSubCategory3(productSubCategory3);
    }
    if (this._commonfieldType == this._commonFieldTypeEnum.DestinationCountry) { //Commonfeild FB template id apply to all booking Product List

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        productTrans.poProductDetail.destinationCountryID = this.bookingMasterData.parentDestinationCountryId;
        productTrans.poProductDetail.destinationCountryName = this.bookingMasterData.parentDestinationCountryName;
      });


    }

    this.modelRef.close()
  }

  cancelCommonFeild() {
    if (this._commonfieldType == this._commonFieldTypeEnum.FBTemplate) {
      this.bookingMasterData.parentFbTemplateId = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ETD) {
      this.bookingMasterData.parentEtd = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductCategory) {
      this.bookingMasterData.parentProductCategoryId = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory) {
      this.bookingMasterData.parentProductSubCategoryId = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory2) {
      this.bookingMasterData.parentProductCategorySub2Id = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.ProductSubCategory3) {
      this.bookingMasterData.parentProductCategorySub3Id = null;
    }
    else if (this._commonfieldType == this._commonFieldTypeEnum.DestinationCountry) {
      this.bookingMasterData.parentDestinationCountryId = null;
    }

    this.modelRef.close()
  }



  //#endregion Common Fields


  //calculate the total purchase order
  CalculateTotalPurchaseOrder() {

    var poList = [];

    this.bookingMasterData.bookingPOProductValidators.forEach(poProductData => {
      if (!poList.includes(poProductData.poProductDetail.poName))
        poList.push(poProductData.poProductDetail.poName);
    });

    poList = poList.sort();
    this.bookingMasterData.totalPos = poList.length;
    this.bookingMasterData.poList = poList;
  }



  //validate panel 1 and get CSNames by officeId,customerId,departments,brands
  bookingPanelValidate() {

    this.IsBookingPanelDataValid();
    if (this.validator.isSubmitted) return;
    this.model.csList = [];

    this.assignCSNameList();

    this.getInitialSearchPoList();
  }

  async assignCSNameList() {
    var csConfigListResponse = await this.getCSNames();

    this.processCSNameSuccessResponse(csConfigListResponse);
  }

  processCSNameSuccessResponse(csConfigListResponse) {
    if (csConfigListResponse && csConfigListResponse.result == CSConfigListResult.Success) {
      this.bookingMasterData.csList = csConfigListResponse.csList;
      if (this.bookingMasterData.csList && this.bookingMasterData.csList.length > 0)
        this.model.csList = this.bookingMasterData.csList.map(x => x.id);
    }
    else if (csConfigListResponse && csConfigListResponse.result == CSConfigListResult.RequestNotValid) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_AE_NAME');
    }
  }

  //get CSNames by officeId,customerId,departments,brands

  getOfficeRelatedDetails(officeitem) {

    if (officeitem != null && officeitem.id != null) {
      this.model.csList = [];

      this.assignCSNameList();

    }
  }

  async getCSNames() {

    this.bookingMasterData.csList = [];

    var request = new UserAccessRequest();
    var _officeIds = [];
    if (this.model.officeId != null)
      _officeIds.push(this.model.officeId);
    //frame the request
    request = {
      OfficeIds: _officeIds,
      CustomerId: this.model.customerId,
      DepartmentIds: this.model.inspectionCustomerDepartmentList,
      BrandIds: this.model.inspectionCustomerBrandList,
      serviceId: Service.Inspection
    };


    return await this.service.getCSList(request);


  }

  //Validate the general information
  validateGeneralInformation() {
    var isGenInfoValid = false;

    isGenInfoValid = this.validator.isValid('customerId') &&
      this.validator.isValid('supplierId') &&
      this.IsCustomerContactListValid() &&
      this.IsSupplierContactListValid() &&
      this.validator.isValid('applicantName') &&
      this.validator.isValid('applicantEmail') &&
      this.validator.isValid('applicantPhoneNo');

    if (isGenInfoValid && this.bookingMasterData.userTypeId != UserType.Customer ||
      (this.bookingMasterData.userTypeId == UserType.Customer
        && this.model.statusId != BookingStatus.Requested
        && this.model.statusId != BookingStatus.New))
      isGenInfoValid = this.validator.isValid('factoryId') && this.IsfactoryContactListValid();

    if (isGenInfoValid) {
      if (this.bookingMasterData.customerBuyerList != null && this.bookingMasterData.customerBuyerList.length > 0) {
        isGenInfoValid = this.IsCustomerBuyerListValid()
      }
    }

    if (isGenInfoValid) {
      if (this.bookingMasterData.customerDepartmentList != null && this.bookingMasterData.customerDepartmentList.length > 0) {
        isGenInfoValid = this.IsCustomerDepartmentListValid()
      }
    }

    if (isGenInfoValid) {
      if (this.bookingMasterData.customerBrandList != null && this.bookingMasterData.customerBrandList.length > 0) {
        isGenInfoValid = this.IsCustomerBrandListValid()
      }
    }

    if (isGenInfoValid) {
      if (this.bookingMasterData.customerMerchandiserList != null && this.bookingMasterData.customerMerchandiserList.length > 0) {
        isGenInfoValid = this.IsCustomerMerchandiserListValid()
      }
    }

    return isGenInfoValid;

  }

  //Validate the process information
  validateProcessInformation() {
    var isProcessInfoValid = false;
    for (let item of this.bookingMasterData.bookingPOProductValidators)
      item.validator.isSubmitted = true;

    isProcessInfoValid = this.validator.isValid('businessLine') &&
      this.validator.isValid('serviceTypeId') &&
      this.validator.isValid('serviceDateFrom') &&
      this.validator.isValid('serviceDateTo') &&
      (this.isContainerService ? this.validator.isValid('containerLimit')
        : true);

    if (isProcessInfoValid && this.model.customerId == this._customerEnum.Gap) {
      //validate booking type only if it is internal user
      isProcessInfoValid = this.bookingMasterData.IsInternalUser ? this.validator.isValid('bookingType') : true;
      if (isProcessInfoValid)
        isProcessInfoValid = this.validator.isValid('paymentOptions') && this.validator.isValid('daCorrelationId');

      if (isProcessInfoValid)
        isProcessInfoValid = this.model.daCorrelationId == this.bookingMasterData.daCorrelationEnum.yes ? (this.validator.isValid('gapDAName') && this.validator.isValid('gapDAEmail')) : true;

      /* if (isProcessInfoValid && this.model.paymentOptions == this._paymentOptions.A
        && (!this.bookingMasterData.gapFileAttachments
          || this.bookingMasterData.gapFileAttachments.length == 0)) {
        isProcessInfoValid = false;
        this.showWarning("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_PAYMENT_OPTION_FILE_REQ");
      } */

    }

    if (isProcessInfoValid && this.model.businessLine == BusinessLine.SoftLine) {
      isProcessInfoValid = this.validator.isValid('seasonId') &&
        this.validator.isValid('seasonYearId');
    }

    //customer collection list has, then collection required
    if (isProcessInfoValid && this.bookingMasterData.IsInternalUser &&
      this.bookingMasterData.isCustomerCollectionAvailable &&
      this.bookingMasterData.customerCollection &&
      this.bookingMasterData.customerCollection.length > 0) {
      isProcessInfoValid = this.validator.isValid('collectionId');
    }

    if (isProcessInfoValid) {
      // add below validation only for container service type
      if (this.isContainerService) {

        if (this.checkContainersAreEqual()) {
          isProcessInfoValid = true;
        }
        else {
          isProcessInfoValid = false;
          this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.LBL_CONTAINER_COUNT");
        }
      }
    }
    if (isProcessInfoValid) {

      isProcessInfoValid = this.validateBookingProductTable();

    }

    if (isProcessInfoValid && this.model.isPickingRequired)

      isProcessInfoValid = this.validatePickingTable();

    if (this.bookingMasterData.isPicking) {
      if (!this.validatePickingQuantity(isProcessInfoValid)) {
        this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_PICKINGQTYNOTAVAILABLE');
        isProcessInfoValid = false;
      }
    }


    if (this.isReInspection) {
      if (isProcessInfoValid) {
        isProcessInfoValid = this.validator.isValid('reinspectionType');
      }
    }

    if (isProcessInfoValid) {
      this.CalculateTotalContainers();
      if (this.dynamicControls && this.dynamicControls.length > 0)
        isProcessInfoValid = this.validateDynamicControls();
      if (isProcessInfoValid) {
        this.bookingPreviewData.dfCustomerConfigPreviewBaseData = [];
        this.AssignDFCustomerConfigPreviewData();
      }
    }

    if (isProcessInfoValid) {
      // price category if customer has and service type should not container service
      if (this.bookingMasterData.isCustomerPriceCategoryAvailable &&
        !this.isContainerService) {
        this.getPriceCategory();
      }
      else {
        this.bookingPreviewData.priceCategoryMessage = "";
      }
    }

    return isProcessInfoValid;
  }

  validateBookingProductTable() {
    var bookingProductDataValid = false;
    if (this.bookingMasterData.bookingPOProductValidators.length == 0) {
      bookingProductDataValid = false;
      this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_BOOKING_SELECT_PO');
    }
    else {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTran => {
        productTran.validator.isSubmitted = true;
      });

      bookingProductDataValid = this.bookingMasterData.bookingPOProductValidators.every((productTran) =>
        productTran.validator.isValid('poId') &&
        productTran.validator.isValid('productId') &&
        (!this.isContainerService ? productTran.validator.isValid('productCategoryId') : true) &&
        (!this.isContainerService ? productTran.validator.isValid('productSubCategoryId') : true) &&
        productTran.validator.isValid('unit') &&

        ((UserType.InternalUser == this.bookingMasterData.userTypeId) && !this.isContainerService ? productTran.validator.isValid('productCategorySub2Id') : true) &&
        productTran.validator.isValid('bookingQuantity') &&
        (this.isContainerService ? productTran.validator.isValid('containerId') : true)
        && ((productTran.poProductDetail.aql == this.aqlType.AQLCustom
          && productTran.poProductDetail.sampleType != this.sampleType.Other)
          ? productTran.validator.isValid('aqlQuantity') : true));

      if (bookingProductDataValid)
        bookingProductDataValid = this.bookingMasterData.bookingPOProductValidators.every((productTran) => (productTran.poProductDetail.unit == Unit.Set) ? productTran.validator.isValid('unitCount') : true);

      if (bookingProductDataValid && this.bookingMasterData.bookingPOProductValidators.length > 0) {
        var emptyInspectionPOList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.bookingQuantity == 0);
        if (emptyInspectionPOList.length > 0) {
          bookingProductDataValid = false;
          this.showWarning("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_BOOKINGQTY_GREATER_THAN_ZERO');
        }
      }

      if (bookingProductDataValid) {
        bookingProductDataValid = this.validateDuplicatePoProducts();
      }

      //validation for the display products group
      if (bookingProductDataValid)
        bookingProductDataValid = this.validateDisplayProducts();

      if (bookingProductDataValid && this.model.businessLine == BusinessLine.SoftLine) {
        bookingProductDataValid = this.bookingMasterData.bookingPOProductValidators.every((productTran) =>
          productTran.validator.isValid('colorCode') &&
          productTran.validator.isValid('colorName'));
      }

    }

    return bookingProductDataValid;

  }

  validatePickingTable() {
    var pickingDataValid = false;
    //loop through the booking po product list
    for (var index = 0; index < this.bookingMasterData.bookingPOProductValidators.length; index++) {

      var uniquePickingProducts = [];
      var duplicatePickingProducts = [];
      //take po product data
      var poProductData = this.bookingMasterData.bookingPOProductValidators[index];
      //set validator submitted to false
      if (poProductData.poProductDetail.inspectionPickingValidators) {
        poProductData.poProductDetail.inspectionPickingValidators.forEach(pickingDetail => {
          pickingDetail.validator.isSubmitted = true;
        });
        //check and assign validation data
        pickingDataValid = poProductData.poProductDetail.inspectionPickingValidators.every(
          (pickingTran) => (pickingTran.pickingData.labOrCustomerId > 0
            || pickingTran.pickingData.labOrCustomerId == -1) ?
            (pickingTran.validator.isValid('labOrCustomerAddressId') &&
              pickingTran.validator.isValid('labOrCustomerContactIds')) : true &&
                !pickingTran.pickingData.labOrCustomerId ? pickingTran.validator.isValid('remarks') : true
            && pickingTran.validator.isValid('pickingQuantity')

        );

        if (pickingDataValid) {
          //take duplicate picking products by lab and lab address id
          duplicatePickingProducts = poProductData.poProductDetail.inspectionPickingValidators.filter(o => {

            if (uniquePickingProducts.find(i => i.pickingData.labOrCustomerId === o.pickingData.labOrCustomerId
              && i.pickingData.labOrCustomerAddressId === o.pickingData.labOrCustomerAddressId)) {
              return true
            }
            uniquePickingProducts.push(o)
            return false;
          });

          if (duplicatePickingProducts && duplicatePickingProducts.length > 0) {

            this.showWarning("EDIT_BOOKING.TITLE", "product cannot assigned to same lab and address");
            pickingDataValid = false;

          }
        }
        if (!pickingDataValid)
          break;
      }
    }

    return pickingDataValid;

  }

  //validate the duplicate is available in the product list table
  //Normal Flow->Po,Product should be unique
  //Container Flow->Po,Product,Container should be unique
  //Softline Flow->Po,Product,colorcode should be unique
  validateDuplicatePoProducts() {

    var isPoProductDuplicated = false;
    var uniquePoProducts = [];
    var duplicatePoProducts = [];

    //container flow
    if (this.isContainerService) {

      duplicatePoProducts = this.bookingMasterData.bookingPOProductValidators.filter(o => {

        if (uniquePoProducts.find(i => i.poProductDetail.poId === o.poProductDetail.poId && i.poProductDetail.productId === o.poProductDetail.productId
          && i.poProductDetail.containerId === o.poProductDetail.containerId)) {
          return true
        }
        uniquePoProducts.push(o)
        return false;
      });

      if (duplicatePoProducts && duplicatePoProducts.length > 0) {
        this.showWarning("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_PO_PRD_UNIQUE_PO_PRD_CONTAINER");
        isPoProductDuplicated = true;
      }
    }
    //softline flow
    else if (this.model.businessLine == this._businessLine.SoftLine) {

      duplicatePoProducts = this.bookingMasterData.bookingPOProductValidators.filter(o => {

        if (uniquePoProducts.find(i => i.poProductDetail.poId === o.poProductDetail.poId && i.poProductDetail.productId === o.poProductDetail.productId
          && i.poProductDetail.colorCode === o.poProductDetail.colorCode && i.poProductDetail.colorName === o.poProductDetail.colorName)) {
          return true
        }
        uniquePoProducts.push(o)
        return false;
      });

      if (duplicatePoProducts && duplicatePoProducts.length > 0) {
        this.showWarning("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_PO_PRD_UNIQUE_PO_PRD_COLOR");
        isPoProductDuplicated = true;
      }

    }
    //normal flow
    else {

      duplicatePoProducts = this.bookingMasterData.bookingPOProductValidators.filter(o => {

        if (uniquePoProducts.find(i => i.poProductDetail.poId === o.poProductDetail.poId
          && i.poProductDetail.productId === o.poProductDetail.productId)) {
          return true
        }
        uniquePoProducts.push(o)
        return false;
      });

      if (duplicatePoProducts && duplicatePoProducts.length > 0) {
        this.showWarning("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_PO_PRD_UNIQUE");
        isPoProductDuplicated = true;
      }

    }

    return !isPoProductDuplicated;

  }

  //validate the display product and child concepts
  validateDisplayProducts() {

    var isDisplayProductValid = false;
    //take the parent products (which is display master)
    var parentProductIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.isDisplayMaster).
      map(x => x.poProductDetail.productId);
    //take the distinct among them
    var distinctparentProductIds = parentProductIds.filter((n, i) => parentProductIds.indexOf(n) === i);

    //if parent products is available then proceed for the validation els return true
    isDisplayProductValid = distinctparentProductIds.length == 0 ? !isDisplayProductValid : isDisplayProductValid;

    if (distinctparentProductIds && distinctparentProductIds.length > 0) {

      for (var index = 0; index < distinctparentProductIds.length; index++) {

        //take the child parents mapped to specific parent product(which is selected in the loop)
        var mappedProducts = this.bookingMasterData.bookingPOProductValidators.filter(x =>
          x.poProductDetail.parentProductId == distinctparentProductIds[index]);

        if (mappedProducts && mappedProducts.length > 0) {

          isDisplayProductValid = true;

          //check aql level is same ,if not throw the validation
          var aqlList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.parentProductId == distinctparentProductIds[index]).map(x => x.poProductDetail.aql);

          if (aqlList) {
            //take distinct aql level ids
            var distinctaqlList = aqlList.filter((n, i) => aqlList.indexOf(n) === i);
            //if aql length is zero then show the warning aql has to be same
            if (distinctaqlList && distinctaqlList.length > 1) {
              isDisplayProductValid = false;
              this.showWarning("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_CHILD_PRODUCT_AQL_SAME");
              break;
            }
          }

        }
        //if no products mapped then throw the validation 'no child products mapped'
        if (mappedProducts && mappedProducts.length == 0) {
          isDisplayProductValid = false;
          var poProduct = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == distinctparentProductIds[index]);
          var warnMessage = this.utility.textTranslate('EDIT_BOOKING.MSG_NO_CHILD_PRODUCTS_MAPPED');
          this.showWarning("EDIT_BOOKING.TITLE", warnMessage.concat(poProduct ? poProduct.poProductDetail.productName : ""));
          break;
        }
      }
    }

    return isDisplayProductValid;
  }

  //validate the booking data
  IsBookingPanelDataValid() {
    var isDataValid = false;
    this.validator.initTost();
    this.validator.isSubmitted = true;
    //validate panel1 (General Info)
    if (this.panelNavigation.stepOne == this.panelNavigationStatus.Active) {
      isDataValid = this.validateGeneralInformation();

      if (isDataValid && !this.model.id) {

        var currentEntityId = Number(this.utility.getEntityId());

        if (currentEntityId == EntityAccess.API)
          this.model.businessLine = this._businessLine.HardLine;
        else if (currentEntityId == EntityAccess.SGT)
          this.model.businessLine = this._businessLine.SoftLine;
      }



      if (isDataValid && !this.model.id && !this.isReInspection && !this.isReBooking && this.bookingMasterData.bookingPOProductValidators && this.bookingMasterData.bookingPOProductValidators.length == 0) {
        this.addToBookingProducts();
      }
    }
    //validate panel3 (Process Info)
    else if (this.panelNavigation.stepTwo == this.panelNavigationStatus.Active) {
      isDataValid = this.validateProcessInformation();
    }
    //validate panel4 (Internal Reference)
    else if (this.panelNavigation.stepFour == this.panelNavigationStatus.Active) {
      if (UserType.InternalUser == this.bookingMasterData.userTypeId) {
        isDataValid = this.validator.isValid('officeId')
      }
    }

    if (isDataValid) {
      this.validator.isSubmitted = false;
      //adding this function to have service data in the main block when click the next
      if (this.panelNavigation.stepTwo == this.panelNavigationStatus.Active)
        this.setpaneldate();
      //set preview data
      /* if (this.panelNavigation.stepFour == this.panelNavigationStatus.Active)
        this.SetPanelItem(); */
      this.moveToNextPanel();
    }

  }

  //move the control to next and update the appropriate panel status
  moveToNextPanel() {
    var panelStatusToUpdate = this.model.id ? this.panelNavigationStatus.Completed : this.panelNavigationStatus.Default;
    if (this.panelNavigation.stepOne == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepTwo = this.panelNavigationStatus.Active;
      this.panelNavigation.stepThree = panelStatusToUpdate;
      this.panelNavigation.stepFour = panelStatusToUpdate;
    }
    else if (this.panelNavigation.stepTwo == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepThree = this.panelNavigationStatus.Active;;
      this.panelNavigation.stepFour = panelStatusToUpdate;
    }
    else if (this.panelNavigation.stepThree == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepThree = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepFour = this.panelNavigationStatus.Active;
    }
  }

  //move the control to previous panel and update the appropriate panel status
  moveToPreviousPanel() {
    var panelStatusToUpdate = this.model.id ? this.panelNavigationStatus.Completed : this.panelNavigationStatus.Default;
    if (this.panelNavigation.stepTwo == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Active;
      this.panelNavigation.stepTwo = panelStatusToUpdate;
      this.panelNavigation.stepThree = panelStatusToUpdate;
      this.panelNavigation.stepFour = panelStatusToUpdate;
    }
    else if (this.panelNavigation.stepThree == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepTwo = this.panelNavigationStatus.Active;
      this.panelNavigation.stepThree = panelStatusToUpdate;
      this.panelNavigation.stepFour = panelStatusToUpdate;
    }
    else if (this.panelNavigation.stepFour == this.panelNavigationStatus.Active) {
      this.panelNavigation.stepOne = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepTwo = this.panelNavigationStatus.Completed;
      this.panelNavigation.stepThree = this.panelNavigationStatus.Active;
      this.panelNavigation.stepFour = panelStatusToUpdate;
    }
  }

  toggleBookingParentProductData() {
    this.isBookingParentProductData = !this.isBookingParentProductData;
  }



  IsFormValid() {
    switch (this.bookingMasterData.userTypeId) {

      case UserType.InternalUser:
        {
          return this.validator.isValid('customerId') &&
            this.validator.isValid('serviceDateFrom') &&
            this.validator.isValid('serviceDateTo') &&
            // this.validator.isValid('seasonId') &&
            // this.validator.isValid('seasonYearId') &&
            this.validator.isValid('supplierId') &&
            this.validator.isValid('factoryId') &&
            // this.validator.isValid('internalReferencePo') &&
            this.validator.isValid('officeId') &&
            this.IsCustomerContactListValid() &&
            this.IsSupplierContactListValid() &&
            this.IsfactoryContactListValid();
        }
      default:
        {
          var isGenInfoValid = this.validator.isValid('customerId') &&
            this.validator.isValid('serviceDateFrom') &&
            this.validator.isValid('serviceDateTo') &&
            this.validator.isValid('supplierId') &&
            this.IsCustomerContactListValid() &&
            this.IsSupplierContactListValid() &&
            this.IsTermsCheckedForExternalUser();

          if (isGenInfoValid && this.bookingMasterData.userTypeId != UserType.Customer
            || (this.bookingMasterData.userTypeId == UserType.Customer
              && this.model.statusId != BookingStatus.Requested
              && this.model.statusId != BookingStatus.New))
            isGenInfoValid = this.validator.isValid('factoryId') && this.IsfactoryContactListValid();

          return isGenInfoValid;
        }
    }
  }

  IsTermsCheckedForExternalUser() {

    if (this.model.isTermsChecked)
      return true;

    this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_TERMS_CONDITION');
    return false;

  }
  IsCustomerContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionCustomerContactList');;
  }
  IsCustomerBuyerListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionCustomerBuyerList');;
  }

  IsCustomerBrandListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionCustomerBrandList');;
  }

  IsCustomerDepartmentListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionCustomerDepartmentList');;
  }

  IsSupplierContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionSupplierContactList');
  }
  IsfactoryContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionFactoryContactList');
  }
  IsCustomerMerchandiserListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    return this.validator.isValid('inspectionCustomerMerchandiserList');;
  }

  returnToPreviouspage() {
    if (this.isRedirectFromQuotation) {
      super.return('inspsummary/quotation-pending/3');
    }
    //future we might need below code to redirect
    else if (this.bookingMasterData.isFromSplitBooking) {
      this.returnwithsearchparam('inspsummary/booking-summary', this.model.id);
    }
    else {
      super.return('inspsummary/booking-summary');
    }
  }

  setFirstRequestDate() {
    if (this.model.firstServiceDateFrom == null) {
      this.model.firstServiceDateFrom = this.model.serviceDateFrom;
    }

    if (this.model.firstServiceDateTo == null) {
      this.model.firstServiceDateTo = this.model.serviceDateTo;
    }
  }
  IsSaveValidation() {
    //customer price category list has, then price category required
    if (this.bookingMasterData.IsInternalUser &&
      this.bookingMasterData.isCustomerPriceCategoryAvailable &&
      this.bookingMasterData.customerPriceCategory &&
      this.bookingMasterData.customerPriceCategory.length > 0 && (this.model.priceCategoryId == null || this.model.priceCategoryId == 0)) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_PRICE_CATEGORY_REQ');
      return false;
    }
    else {
      return true;
    }
  }


  assignDraftProductListValidatorToProductListModel() {
    var productList = [];
    this.bookingMasterData.bookingPOProductValidators.forEach(element => {
      element.poProductDetail.poProductList = null;
      element.poProductDetail.poProductListInput = null;
      element.poProductDetail.poListInput = null;
      element.poProductDetail.productInput = null;
      productList.push(element.poProductDetail);
    });

    this.model.inspectionProductList = productList.filter(function (el) {
      return el.poId || el.productId;
    });

    this.bookingMasterData.bookingPOProductValidators = this.bookingMasterData.bookingPOProductValidators.filter(function (el) {
      return el.poProductDetail.poId || el.poProductDetail.productId;
    });

  }

  assignProductListValidatorToProductListModel() {
    var productList = [];
    var pickingList = [];
    this.bookingMasterData.bookingPOProductValidators.forEach(element => {
      element.poProductDetail.poProductList = null;
      element.poProductDetail.poProductListInput = null;
      element.poProductDetail.productInput = null;
      element.poProductDetail.poListInput = null;

      //assign picking validators to inspection picking list on save
      if (element.poProductDetail.inspectionPickingValidators) {
        pickingList = [];

        element.poProductDetail.inspectionPickingValidators.forEach(picking => {
          if (picking.pickingData.labType == LabType.Customer)
            picking.pickingData.labOrCustomerId = this.model.customerId;

          if (!this.model.id)
            picking.pickingData.pickingContactList = [];

          if (this.model.id) {
            var pickingContactIdsToDelete = picking.pickingData.pickingContactList.filter(x => !picking.pickingData.labOrCustomerContactIds.includes(x.labOrCustomerContactId)).map(x => x.labOrCustomerContactId);
            pickingContactIdsToDelete.forEach(contactId => {
              var pickingContactIndex = picking.pickingData.pickingContactList.findIndex(x => x.labOrCustomerContactId == contactId);
              if (pickingContactIndex >= 0)
                picking.pickingData.pickingContactList.splice(pickingContactIndex, 1);
            });
          }

          picking.pickingData.labOrCustomerContactIds.forEach(contactId => {

            var contactAvailable = picking.pickingData.pickingContactList.find(x => x.labOrCustomerContactId == contactId);
            if (!contactAvailable) {
              var pickingContact = new SavePickingContact();
              pickingContact.labOrCustomerContactId = contactId;
              picking.pickingData.pickingContactList.push(pickingContact);
            }
          });

          pickingList.push(picking.pickingData);
        });
        element.poProductDetail.saveInspectionPickingList = pickingList;
        element.poProductDetail.inspectionPickingValidators = [];
      }

      productList.push(element.poProductDetail);
    });

    this.model.inspectionProductList = productList;
  }

  saveInspectionBooking() {
    this.model.isUpdateBookingDetail = true;
    this.save();
  }

  applyFileAttachmentCustomizationData() {
    if (this.currentUser.usertype == this.userTypeEnum.Supplier || this.currentUser.usertype == this.userTypeEnum.Factory) {
      this.bookingMasterData.inspectionFileAttachments.forEach(element => {
        element.isBookingEmailNotification = true;
      });
    }
  }

  applyDefaultBookingTypes() {
    if (this.currentUser.usertype != this.userTypeEnum.InternalUser)
      this.model.bookingType = BookingTypes.Announced;
  }

  setSupplierOrFactoryEmailSend() {
    if (this.model.bookingType == this._inspectionBookingTypeEnum.UnAnnounced)
      this.model.isSupplierOrFactoryEmailSend = false;
  }

  save() {
    try {
      this.validator.isSubmitted = true;
      if (this.IsFormValid() && this.IsSaveValidation()) {

        this.bookingMasterData.pageLoader = true;
        //this.bookingMasterData.savedataloading = true;
        this.setFirstRequestDate();
        this.model.guidId = this.newGuid();
        if (this.model.serviceTypeId) {
          this.model.inspectionServiceTypeList = [];
          this.model.inspectionServiceTypeList.push(this.model.serviceTypeId);
        }
        this.assignProductListValidatorToProductListModel();


        this.model.gapDACorrelation = this.model.daCorrelationId == this.bookingMasterData.daCorrelationEnum.yes ? true : false;

        //add the inspectionfileattachments and gapfileattachments to model.inspectionFileAttachmentList
        this.model.inspectionFileAttachmentList = [];
        if (this.bookingMasterData.inspectionFileAttachments) {
          this.bookingMasterData.inspectionFileAttachments.forEach(element => {
            this.model.inspectionFileAttachmentList.push(element);
          });
        }

        if (this.bookingMasterData.gapFileAttachments) {
          this.bookingMasterData.gapFileAttachments.forEach(element => {
            this.model.inspectionFileAttachmentList.push(element);
          });
        }


        if (this.isReInspection) {
          this.assignReInspectionData();
        }
        if (this.isReBooking) {
          this.assignReBookingData();
        }

        if (this.model.id == 0) {
          this.model.statusId = BookingStatus.Requested;
        }

        //add customer configured value to inspection df transactions
        if (this.dynamicControls && this.dynamicControls.length > 0)
          this.addInspectionDfTransaction();

        this.model.isPickingRequired = this.bookingMasterData.isPicking;
        this.model.inspectionPageType = this.inspectionPageType;

        this.model.draftInspectionId = this.draftInspectionId;

        this.applyFileAttachmentCustomizationData();

        this.applyDefaultBookingTypes();

        this.setSupplierOrFactoryEmailSend();

        this.service.saveBooking(this.model)
          .subscribe(
            res => {
              if (res && (res.result == 1 || res.result == 8)) {

                this.model.id = res.id;
                this.bookingMasterData.issuccess = true;
                this.bookingMasterData.pageLoader = false;
                this.model.isUpdateBookingDetail = false;
                if (res.result == 8)
                  this.showWarning("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NOTIFICATIONFAIL');
              }
              else {
                switch (res.result) {
                  case 2:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_CANNOT_SAVE_AUDIT');
                    break;
                  case 3:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_FORMAT_ERROR');
                    break;
                  case 4:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NO_ITEM_FOUND');
                    break;
                  case 5:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_AUDIT_NOT_UPDATED');
                    break;
                  case 7:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_BOOKINGQTY_GREATER_THAN_ZERO');
                    break;
                  case 10:
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_INSPECTION_SERVICE_DATE_INCORRECT');
                    break;
                  case 11: {
                    this.model.statusId = BookingStatus.Requested;
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_LBL_COMBINE_REQ');
                    break;
                  }
                  case 12: {
                    this.model.statusId = BookingStatus.Requested;
                    this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_LBL_PICKING_REQ');
                    break;
                  }
                  default:
                    this.showWarning("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NOTIFICATIONFAIL');
                }
                this.bookingMasterData.pageLoader = false;
                this.model.isUpdateBookingDetail = false;
              }
            },
            errRes => {

              if (errRes && errRes.error && errRes.error.errors && errRes.error.statusCode == 400) {
                let validationErrors: [];
                validationErrors = errRes.error.errors;
                this.openValidationPopup(validationErrors);
                this.model.isUpdateBookingDetail = false;

              }
              else {
                this.model.isUpdateBookingDetail = false;
                this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
              }
              this.bookingMasterData.pageLoader = false;
            });
      }
    }
    catch (e) {
      this.model.isUpdateBookingDetail = false;
      this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_CLEINT_ERROR");
    }
  }

  saveDraftInspectionBooking() {

    //map the draft inspeciton booking
    this.mapSaveDraftInspectionBooking();

    //assign the product list validator to product list
    this.assignDraftProductListValidatorToProductListModel();

    //map the dynamic controls
    if (this.dynamicControls && this.dynamicControls.length > 0)
      this.addInspectionDfTransaction();

    this.draftInspectionBooking.bookingInfo = JSON.stringify(this.model);

    this.bookingMasterData.pageLoader = false;

    this.service.saveInspectionDraftBooking(this.draftInspectionBooking)
      .subscribe(
        res => {
          if (res && res.result == SaveDraftInspectionResult.Success) {
            this.processDraftBookingSaveSuccessResponse(res);
            this.showSuccess("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_DRAFT_SUCCESS");
          }
          else {
            this.bookingMasterData.pageLoader = false;
            this.processDraftBookingSaveErrorResponse(res);


          }
        },
        errRes => {

          if (errRes && errRes.error && errRes.error.errors && errRes.error.statusCode == 400) {
            let validationErrors: [];
            validationErrors = errRes.error.errors;
            this.openValidationPopup(validationErrors);
            this.bookingMasterData.pageLoader = false;

          }
          else {
            this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
          }

        });

  }

  processDraftBookingSaveSuccessResponse(response) {
    this.draftInspectionId = response.draftInspectionId;
    this.bookingMasterData.pageLoader = false;
    this.getInspectionDraftBookings();

  }

  processDraftBookingSaveErrorResponse(response) {
    switch (response.result) {
      case SaveDraftInspectionResult.Failure:
        this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_DRAFT_INSP_SAVE_FAILED');
        break;
      default:
        this.showWarning("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
    }
  }

  mapSaveDraftInspectionBooking() {
    this.draftInspectionBooking = new DraftInspectionBooking();
    if (this.draftInspectionId)
      this.draftInspectionBooking.id = this.draftInspectionId;
    this.draftInspectionBooking.customerId = this.model.customerId;
    this.draftInspectionBooking.supplierId = this.model.supplierId;
    this.draftInspectionBooking.factoryId = this.model.factoryId;
    this.draftInspectionBooking.serviceDateFrom = this.model.serviceDateFrom;
    this.draftInspectionBooking.serviceDateTo = this.model.serviceDateTo;
    this.draftInspectionBooking.brandId = this.model.inspectionCustomerBrandList.find(x => x);
    this.draftInspectionBooking.departmentId = this.model.inspectionCustomerDepartmentList.find(x => x);

    if (this.isReInspection) {
      this.draftInspectionBooking.isReInspectionDraft = true;
      this.draftInspectionBooking.PreviousBookingNo = this.model.id;
    }

    if (this.isReBooking) {
      this.draftInspectionBooking.IsReBookingDraft = true;
      this.draftInspectionBooking.PreviousBookingNo = this.model.id;
    }



  }

  //get the inspection draft bookings
  getInspectionDraftBookings() {

    this.bookingMasterData.pageLoader = true;
    this.draftBookingList = [];
    this.service.getInspectionDraftBookings()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == DraftInspectionResult.Success) {
            this.draftBookingList = res.inspectionData;

            if (!this.draftInspectionId) {
              this.Initialize();
              if (!this.isReBooking && !this.isReInspection)
                this.viewType = this._bookingViewTypeEnum.DraftBooking;
            }

            this.bookingMasterData.pageLoader = false;
          }
          else if (res && res.result == DraftInspectionResult.NotFound) {
            this.bookingMasterData.pageLoader = false;
            //this.model = new EditInspectionBooking();
          }

          this.bookingMasterData.pageLoader = false;

        });
  }

  cancelDraftBooking(content, item) {

    this.deleteDraftInspectionId = item.id;
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });

  }

  cancelRemoveDraftBooking() {
    this.deleteDraftInspectionId = null;
    this.modelRef.close();
  }

  deleteDraftInspectionBooking() {

    this.service.removeInspectionDraftBookings(this.deleteDraftInspectionId)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == DeleteDraftInspectionResult.DeleteSuccess)) {
            this.showSuccess("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_DRAFT_INSP_DELETE_SUCCESS");
            this.draftInspectionId = null;
            this.getInspectionDraftBookings();
            this.bookingMasterData.supplierList = [];
            this.bookingMasterData.factoryList = [];
            this.Initialize();
          }
          else {
            this.error = response.result;

            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();

  }


  //resume the booking from the draft data
  resumeBooking(item) {
    this.bookingMasterData.pageLoader = true;
    var bookingInfo = JSON.parse(item.bookingInfo);

    this.draftInspectionId = item.id;
    this.model = new EditInspectionBooking();
    this.model = bookingInfo;

    this.model.statusId = BookingStatus.New;

    this.isContainerService = false;
    if (ContainerServiceList.includes(this.model.serviceTypeId)) {
      this.isContainerService = true;
      this.getContainerList(this.model.containerLimit);
      this.CalculateTotalContainers();
    }

    this.isReInspection = false;
    this.isReBooking = false;
    if (item.isReInspectionDraft) {
      this.isReInspection = true;
      this.getReInspectionTypes();
    }
    else if (item.isReBookingDraft) {
      this.isReBooking = true;
    }
    else
      this.model.id = 0;



    this.getInitialSearchPoList();

    if (this.model.customerId)
      this.GetCusRelatedDetailsById(this.model.customerId, false);

    var bookingId = 0;

    var cusid = this.model.customerId == null || this.model.customerId == 0 ? null : this.model.customerId;

    if (this.model.supplierId) {

      this.getSupRelatedDetailsById(this.model.supplierId, cusid, bookingId, this.model.id > 0 ? true : false);

    }

    this.geFactRelatedDetailsById(this.model.factoryId, cusid, bookingId, false);
    this.GetInspBookingContact(this.model.factoryId, cusid);
    this.GetInspBookingRuleDetails(cusid, this.model.factoryId);


    this.bookingMasterData.bookingPOProductValidators = [];

    this.addEntityProductListToClientModel();



    this.viewType = this._bookingViewTypeEnum.NewBooking;

    if (this.bookingMasterData.bookingPOProductValidators.length == 0)
      this.addToBookingProducts();
    else if (this.bookingMasterData.bookingPOProductValidators.length > 0)
      this.addPOandProductDataSourceToProductList();

    this.CalculateTotalPickingQuantity();




    this.bookingMasterData.pageLoader = false;

    this.CalculateTotalProducts();
  }

  toggleTab(event, tabTarget) {

    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
  }


  confirmEmail() {
    try {
      this.validator.isSubmitted = true;
      if (this.IsFormValid() && this.IsSaveValidation()) {
        this.bookingMasterData.savedataloading = true;
        this.setFirstRequestDate();
        this.model.guidId = this.newGuid();
        if (this.model.serviceTypeId) {
          this.model.inspectionServiceTypeList = [];
          this.model.inspectionServiceTypeList.push(this.model.serviceTypeId);
        }

        this.assignProductListValidatorToProductListModel();

        if (this.isReInspection) {
          this.assignReInspectionData();
        }
        if (this.isReBooking) {
          this.assignReBookingData();
        }

        if (this.model.id == 0) {
          this.model.statusId = BookingStatus.Requested;
        }

        //add customer configured value to inspection df transactions
        if (this.dynamicControls && this.dynamicControls.length > 0)
          this.addInspectionDfTransaction();

        this.model.isPickingRequired = this.bookingMasterData.isPicking;
        this.model.inspectionPageType = this.inspectionPageType;

        this.service.confirmEmailBooking(this.model)
          .subscribe(
            res => {
              if (res && (res.result == 1 || res.result == 8)) {

                this.model.id = res.id;
                this.bookingMasterData.pageLoader = false;
                this.showSuccess("EDIT_BOOKING.TITLE", 'SCHEDULE_SUMMARY.MSG_EMAIL_WILL_SEND');
                this.returnwithsearchparam('inspsummary/booking-summary', this.model.id);

              }
              else {
                this.showWarning("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NOTIFICATIONFAIL');
                this.bookingMasterData.pageLoader = false;
              }
            },
            errRes => {
              if (errRes && errRes.error && errRes.error.errors && errRes.error.statusCode == 400) {
                let validationErrors: [];
                validationErrors = errRes.error.errors;
                this.openValidationPopup(validationErrors);

              }
              else {
                this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
              }
              this.bookingMasterData.pageLoader = false;
            });
      }
    }
    catch (e) {
      this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
    }
  }

  selectFiles(fileAttachmentCategoryId) {
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.bookingMasterData.fileSize,
      uploadFileExtensions: this.bookingMasterData.uploadFileExtensions,
      uploadLimit: this.bookingMasterData.uploadLimit,
      containerId: FileContainerList.InsepctionBooking,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;
    //modalRef.componentInstance.isDisplayAttachmentDescription = true;
    modalRef.componentInstance.callFromBooking = true;
    if (this.model.inspectionFileAttachmentList.length == 0)
      modalRef.componentInstance.bookingDocumentFirst = true;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {

          if (fileAttachmentCategoryId == this._fileAttachmentCategory.General) {
            element.fileAttachmentCategoryId = fileAttachmentCategoryId;
            element.isReportSendToFB = true;
            this.bookingMasterData.inspectionFileAttachments.push(element);
          }
          else if (fileAttachmentCategoryId == this._fileAttachmentCategory.Gap) {
            element.fileAttachmentCategoryId = fileAttachmentCategoryId;
            this.bookingMasterData.gapFileAttachments.push(element);
          }



        });
      }

    }, (reason) => {

    });

  }
  removeAttachment(index) {
    this.bookingMasterData.inspectionFileAttachments.splice(index, 1);
  }

  removeGapFileAttachment(index) {
    this.bookingMasterData.gapFileAttachments.splice(index, 1);
  }

  removeUploadImage(index, item) {
    this.bookingMasterData.bookingPOProductValidators.forEach(poProduct => {

      var productFileindex = poProduct.poProductDetail.productFileAttachments.findIndex(x => x.productId == item.productId
        && x.id == item.file.id && x.uniqueld == item.file.uniqueld);

      if (productFileindex >= 0) {
        poProduct.poProductDetail.productFileAttachments.splice(productFileindex, 1);
      }

    });

    for (var i = 0; i < this.bookingMasterData.bookingProductFiles.length; i++) {

      if (this.bookingMasterData.bookingProductFiles[i].productId == item.productId
        && this.bookingMasterData.bookingProductFiles[i].file.id == item.file.id
        && this.bookingMasterData.bookingProductFiles[i].uniqueld == item.uniqueld) {

        this.bookingMasterData.bookingProductFiles.splice(i, 1);

      }
    }

    item.poProductDetail.productImageCount = item.poProductDetail.productImageCount - 1;

  }

  getPreviewProductImage(id, data, modalcontent) {
    this.productGalleryImages = [];
    if (id == 2) {
      this.getProductFile(data);
    }
    else if (id == 1) {

      this.getFileTypeList(data);

    }


    this.modelRef = this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true });
    this.modalInputData = {
      typeId: id,
      customerId: this.model.customerId,
      supplierId: this.model.supplierId
    };
    this.modelRef.result.then((result) => {
    }, (reason) => { });
  }

  isCheckProductImage(poDataRow) {
    return poDataRow.productFileAttachments.length > 0;
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  Reset() {
    this.model.customerId = null;
    this.model.inspectionFileAttachmentList = [];
    this.model.applicantEmail = '';
    this.model.applicantName = '';
    this.model.seasonYearId = null;
    this.ResetDetailsByCustomerId();
    this.model.serviceDateFrom = null;
    this.model.serviceDateTo = null;
    this.bookingMasterData.officedetails = {};
    this.BookingRuleDesc = null;
  }

  getFile(file: InspectionFileAttachment) {
    this.downloadloading = true;
    this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.InsepctionBooking)
      .subscribe(res => {
        this.downloadFile(res, file.mimeType, file.fileName);
      },
        error => {
          this.downloadloading = false;
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        });

  }
  getBookingTerms() {
    this.service.getBookingTerms()
      .subscribe(res => {
        this.downloadFile(res, "application/pdf", "Booking_Terms");
      },
        error => {
          this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
        });
  }
  getProductFile(file: ProductFileAttachment) {

    this.productGalleryImages.push(
      {
        small: file.fileUrl,
        medium: file.fileUrl,
        big: file.fileUrl,
      });

  }

  // downloadFile(data, mimeType) {
  //   const blob = new Blob([data], { type: mimeType });
  //   if (window.navigator && window.navigator.msSaveOrOpenBlob) {
  //     window.navigator.msSaveOrOpenBlob(blob);
  //   }
  //   else {
  //     const url = window.URL.createObjectURL(blob);
  //     window.open(url);
  //   }
  //   this.downloadloading=false;
  // }


  downloadFile(data, mimeType, filename) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename ? filename : "export";//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.downloadloading = false;
    this.exportProductLoading = false;
  }
  downloadFileWithoutMimiType(file) {
    const url = window.URL.createObjectURL(file);
    window.open(url);
  }

  isDisabled = (date: NgbDate, current: { month: number }) => {
    if (!this.bookingMasterData.IsInternalUser) {
      return date.before(this.calendar.getToday()) || (this._HolidayDates != null && this._HolidayDates.filter(x => date.equals(x)).length > 0)
        || this.leaddays && (date.before(this.leaddays) || date.equals(this.leaddays))
    };
  }

  GetCustomerByUserType(id) {
    const requestModel = new CustomerCommonDataSourceRequest();
    requestModel.isVirtualScroll = false;
    this.customerService.getCustomerListByUserType(requestModel).subscribe(
      response => {
        if (response) {
          this.bookingMasterData.Customerlist = response;
          this.AssignCustomerData();
          if (this.bookingMasterData.userTypeId == UserType.Customer) {
            this.model.customerId = this.bookingMasterData.Customerlist.length > 0 ? this.bookingMasterData.Customerlist[0].id : null;
            if (id == 0 || id == null) {

              if (this.currentUser.usertype != UserType.InternalUser && !this.model.factoryId)
                this.GetInspBookingRuleDetails(this.model.customerId, "");

              this.GetCusRelatedDetailsById(this.model.customerId, false);
            }

          }
        }
      }
    );
  }
  GetSeasonYear() {
    this.service.GetSeasonYear().subscribe(
      response => {
        if (response && response.result == 1) {
          this.bookingMasterData.Seasonyearlist = response.seasonYearList;
        }
      }
    );
  }

  GetOffice() {
    this.service.GetOffice().subscribe(
      response => {
        if (response && response.result == 1) {
          this.bookingMasterData.Office = response.officeList;
          this.AssignOfficeData();
        }
      }
    );
  }

  changeContainer(container, item) {
    item.containerName = container.name;
  }

  // container change event - check container is already selected
  changeContainerList(event, polist, currentData) {
    currentData.containerName = this.getContainerName(currentData.containerId);
    if (polist.filter(x => x.poProductData.containerId == event.id && x.poProductData.poName == currentData.poName).length > 1) {

      setTimeout(() => {
        currentData.containerId = null;
        this.showError("Inspection Booking", "Container already selected");
      }, 100)

    }

  }

  addPurchaseOrderDetail() {

    var poDetail: purchaseOrderDetail = {
      id: 0,
      poId: 0,
      productId: null,
      productName: null,
      bookingStatus: 3, // not utilized by default.
      productDesc: "",
      destinationCountryId: null,
      etd: null,
      active: true,
      quantity: null,
      factoryId: 0,
      factoryReference: "",
      supplierId: 0,
      factoryList: null,
      isBooked: false,

      productList: [],
      productInput: new BehaviorSubject<string>(""),
      productLoading: false,
      productRequest: null,

      /*   supplierList: null,
        supplierInput: new BehaviorSubject<string>(""),
        supplierLoading: false,
        supplierRequest: null, */

      countryList: [],
      countryInput: new BehaviorSubject<string>(""),
      countryLoading: false,
      countryRequest: null

    };

    this.purchaseModel.purchaseOrderDetails.push(poDetail);
    var poDetailValidator = Validator.getValidator(poDetail, "purchaseorder/edit-purchaseorderdetails.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate);
    poDetailValidator.isSubmitted = false;

    var poDetailWithValidator = { poDetails: poDetail, poDetailValidator: poDetailValidator };

    this.getProductListBySearch(poDetailWithValidator);

    this.getCountryListDataBySearch(poDetailWithValidator);

    this.poDetailValidators.push(poDetailWithValidator);

  }

  getCountryList() {

    this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.destinationCountryList = data.countryList;
          }
          else {
            this.error = data.result;
          }

        },
        error => {
          this.setError(error);

        });
  }

  // create container list
  getContainerList(limit: Number) {

    if (limit) {
      this.containerList = [];

      for (let index = 1; index <= limit; index++) {

        this.containerList.push({ "id": index, "name": "container - " + index });

      }
    }


  }


  getBookingStaffDetails() {
    this.service.GetInspectionStaffDetails(this.currentUser.staffId)
      .pipe()
      .subscribe(
        data => {

          if (data) {
            this.model.applicantName = data.staffName;
            this.model.applicantPhoneNo = data.companyPhone;
            this.model.applicantEmail = data.companyEmail;
          }

        },
        error => {
          this.setError(error);

        });
  }

  getUserApplicantDetails() {
    this.service.GetUserApplicantDetails()
      .pipe()
      .subscribe(
        data => {

          if (data) {
            this.model.applicantName = data.applicantName;
            this.model.applicantPhoneNo = data.applicantPhoneNo;
            this.model.applicantEmail = data.applicantEmail;
            this.bookingMasterData.applicantContactId = data.contactId;
          }

        },
        error => {
          this.setError(error);

        });
  }

  savePurchaseOrderModel(content) {
    this.poDetailValidator.initTost();

    if (this.isPurchaseOrderFormValid()) {

      this.initializePurchaseOrderData();
      this.bookingMasterData.savePurchaseOrderLoading = true;

      this.puchaseOrderService.savePurchaseOrder(this.purchaseModel)
        .subscribe(
          res => {
            if (res && res.result == 1) {

              this.purchaseModel.id = res.id;
              this.bookingMasterData.savePurchaseOrderLoading = false;

              this.modelRef.close();

              this.openAddBookingProduct(content);

            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CANNOT_ADDPURCHASEORDER');
                  break;
                case 3:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CURRENTPO_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_PO_EXISTS');
                  break;
                case 5:
                  this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_BOOKING.MSG_PRODUCT_ALREADY_EXISTS_IN_PO');
                  break;
              }
              this.bookingMasterData.savePurchaseOrderLoading = false;
            }
          },
          error => {
            this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_UNKNONW_ERROR');
            this.bookingMasterData.savePurchaseOrderLoading = false;
          });
    }


  }

  isPurchaseOrderFormValid() {
    var isPurchaseOrderValid = false;

    if (this.purchaseModel.pono && this.purchaseModel.pono.trim() != '') {
      isPurchaseOrderValid = !isPurchaseOrderValid;
    }
    else if (!this.purchaseModel.pono || this.purchaseModel.pono.trim() == '')
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.PO_NUMBER_REQ');

    if (isPurchaseOrderValid) {
      this.poDetailValidators.forEach(element => {
        element.poDetailValidator.isSubmitted = true;
      });
      isPurchaseOrderValid = this.poDetailValidators.every((x) => x.poDetailValidator.isValid('productId')
        && x.poDetailValidator.isValid('factoryId')
        && x.poDetailValidator.isValid('quantity'));
    }

    return isPurchaseOrderValid;
  }


  initializePurchaseOrderData() {
    this.purchaseModel.customerId = this.model.customerId;

    if (this.purchaseModel.purchaseOrderDetails) {
      this.purchaseModel.purchaseOrderDetails.forEach(element => {
        element.supplierId = this.model.supplierId;
        element.factoryId = this.model.factoryId;
        element.countryInput = null;
        element.productInput = null;
        //element.supplierInput = null;
      });
    }
    this.purchaseModel.accessType = 2;
  }

  onchangeServiceType(event) {
    if (event) {
      //check if the service belongs to the container
      if (ContainerServiceList.includes(event.id))
        this.isContainerService = true;
      else
        this.isContainerService = false;

      // clear current service is container
      if (this.isContainerService) {
        //this.model.containerLimit = 100;
        this.poColSpan = 10;
        if (!this.isReBooking) {
          this.clearDataOnChangeOfServiceType();
        }
      }
      // clear previous service is container and current one other than container
      else if (this.isContainerService && this.bookingPreviewData.servicetype != event.name) {
        this.poColSpan = 20;

        if (!this.isReBooking) {
          this.clearDataOnChangeOfServiceType();
        }

        this.getAqlByServiceType();
      }
      else {
        this.poColSpan = 20;
        this.getAqlByServiceType();
      }
      //show servicedate to only it is configured for the selected service type
      //applicable for external user
      if (this.currentUser.usertype != this._userTypeId.InternalUser)
        this.bookingMasterData.showServiceDateTo = false;
      var serviceType = this.bookingMasterData.customerServiceList.find(x => x.id == event.id);
      if (this.currentUser.usertype != this._userTypeId.InternalUser && serviceType && serviceType.showServiceDateTo) {
        this.bookingMasterData.showServiceDateTo = true;
        this.model.serviceDateTo = this.model.serviceDateFrom;
      }
      for (let i = 0; i < this.dynamicControlBaseData?.length; i++) {
        this.dynamicControlBaseData[i].forEach(element => {
          if (element.controlTypeId == this._controlType.DropDown && element.dataSourceType == DFDDLSourceType.GAPAuditProductCategory) {
            this.IsProductCategoryRequired(element);
          }
        });
      }
    }
    this.bookingPreviewData.servicetype = event.name;
  }


  clearDataOnChangeOfServiceType() {
    this.bookingMasterData.productsCategoryloading = true;
    this.bookingMasterData.poProductList = [];




    this.bookingMasterData.ponumber = null;
    this.bookingMasterData.poList = null;
    this.purchaseModel.id = 0;

  }

  applyAqlValue(serviceConfigData) {

    if (serviceConfigData) {
      this.bookingMasterData.bookingPOProductValidators.forEach(element => {
        element.poProductDetail.aql = serviceConfigData.levelPick1;//Aql Value for Pick Type Single
        element.poProductDetail.critical = serviceConfigData.criticalPick1;
        element.poProductDetail.major = serviceConfigData.majorTolerancePick1;
        element.poProductDetail.minor = serviceConfigData.minorTolerancePick1;

        if (element.poProductDetail.isPrimaryProductInGroup) {

          if (element.poProductDetail.aql)
            this.updatePrimaryProductDataToChild(element, this._bookingProductFieldType.Aql);
          else
            this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Aql);

          if (element.poProductDetail.critical) {
            this.updateCriticalNameToPoProductItem(element.poProductDetail.critical, element);
            this.updatePrimaryProductDataToChild(element, this._bookingProductFieldType.Critical);
          }
          else
            this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Critical);

          if (element.poProductDetail.major) {
            this.updateMajorNameToPoProductItem(element.poProductDetail.major, element);
            this.updatePrimaryProductDataToChild(element, this._bookingProductFieldType.Major);
          }
          else
            this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Major);


          if (element.poProductDetail.minor) {
            this.updateMinorDataToPoProductItem(element.poProductDetail.minor, element);
            this.updatePrimaryProductDataToChild(element, this._bookingProductFieldType.Minor);
          }
          else
            this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Minor);

        }
      });

    }
    else {
      this.bookingMasterData.bookingPOProductValidators.forEach(element => {
        element.poProductDetail.aql = null;//Aql Value for Pick Type Single
        element.poProductDetail.critical = null;
        element.poProductDetail.major = null;
        element.poProductDetail.minor = null;
        if (element.poProductDetail.isPrimaryProductInGroup) {

          this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Aql);
          this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Critical);
          this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Major);
          this.updateEmtpyValueToChildProducts(element, this._bookingProductFieldType.Minor);

        }
      });
    }
    this.checkAqlCustomSelected();
    if (this.model.serviceTypeId > 0) {
      const serviceType = this.bookingMasterData.customerServiceList.find(x => x.id == this.model.serviceTypeId);
      if (serviceType && serviceType.is100Inspection) {
        const customAql = this.bookingMasterData.bookingProductAqlList.find(x => x.id == this.aqlType.AQLCustom);
        const sample100SampleType = this.bookingMasterData.customSampleTypeList.find(x => x.id == this.sampleType.Sample100);
        if (customAql && sample100SampleType) {
          this.bookingMasterData.isCustomAqlSelected = true;
          this.bookingMasterData.bookingPOProductValidators.forEach(element => {
            this.customAqlchange(customAql, sample100SampleType, element);
          })
        }
        else {
          this.bookingMasterData.bookingPOProductValidators.forEach(element => {
            element.poProductDetail.previewAqlName = null;
            element.poProductDetail.previewSamplingSize = null;
          })
        }
      }
    }
  }

  getAqlByServiceType() {
    this.service.GetAqlByServiceType(this.model.customerId, this.model.serviceTypeId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.bookingMasterData.bookingServiceConfig = response.customerServiceConfigData;
            this.applyAqlValue(this.bookingMasterData.bookingServiceConfig);
            //var serviceConfigData=response.customerServiceConfigData;

          }
          else {
            this.error = response.result;
          }
          this.setSampleSizeValue();
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        });
  }



  //set the primary product data to newly added product which is same in the list
  setPrimaryProductData(poProductItem) {

    //take the primary product for the given product in the group
    var productData = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == poProductItem.poProductDetail.productId && x.poProductDetail.isPrimaryProductInGroup);

    this.assignPrimaryProductDataToProductItem(productData, poProductItem);
  }

  assignPrimaryProductDataToProductItem(productData, poProductItem) {
    if (productData) {

      if (productData.poProductDetail.aql)
        this.updateAqlNameToPoProductItem(productData.poProductDetail.aql, poProductItem);

      if (productData.poProductDetail.critical)
        this.updateCriticalNameToPoProductItem(productData.poProductDetail.critical, poProductItem);

      if (productData.poProductDetail.major)
        this.updateMajorNameToPoProductItem(productData.poProductDetail.major, poProductItem);

      if (productData.poProductDetail.minor)
        this.updateMinorDataToPoProductItem(productData.poProductDetail.minor, poProductItem);

      if (productData.poProductDetail.sampleType) {
        this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleTypeName);
        this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleSize);
      }

      this.updatePrimaryProductUnitName(productData, poProductItem);

      poProductItem.poProductDetail.productId = productData.poProductDetail.productId;

      poProductItem.poProductDetail.productName = productData.poProductDetail.productName;

      poProductItem.poProductDetail.productDesc = productData.poProductDetail.productDesc;

      //poProductItem.poProductDetail.poQuantity = productData.poProductDetail.poQuantity;

      //poProductItem.poProductDetail.bookingQuantity = productData.poProductDetail.bookingQuantity;

      poProductItem.poProductDetail.unitCount = productData.poProductDetail.unitCount;

      poProductItem.poProductDetail.barcode = productData.poProductDetail.barcode;

      poProductItem.poProductDetail.customSampleTypeName = productData.poProductDetail.customSampleTypeName;

      poProductItem.poProductDetail.aqlQuantity = productData.poProductDetail.aqlQuantity;

      poProductItem.poProductDetail.fbTemplateId = productData.poProductDetail.fbTemplateId;

      poProductItem.poProductDetail.fbTemplateName = productData.poProductDetail.fbTemplateName;

      poProductItem.poProductDetail.remarks = productData.poProductDetail.remarks;

      poProductItem.poProductDetail.productCategoryId = productData.poProductDetail.productCategoryId;

      if (poProductItem.poProductDetail.productCategoryId)
        poProductItem.poProductDetail.productCategoryMapped = true;

      poProductItem.poProductDetail.productSubCategoryId = productData.poProductDetail.productSubCategoryId;

      if (poProductItem.poProductDetail.productSubCategoryId)
        poProductItem.poProductDetail.productSubCategoryMapped = true;

      poProductItem.poProductDetail.productCategorySub2Id = productData.poProductDetail.productCategorySub2Id;

      if (poProductItem.poProductDetail.productCategorySub2Id)
        poProductItem.poProductDetail.productCategorySub2Mapped = true;

      poProductItem.poProductDetail.productCategorySub3Id = productData.poProductDetail.productCategorySub3Id;

      if (poProductItem.poProductDetail.productCategorySub3Id)
        poProductItem.poProductDetail.productCategorySub3Mapped = true;

      poProductItem.poProductDetail.bookingCategorySubProductList = productData.poProductDetail.bookingCategorySubProductList;

      poProductItem.poProductDetail.bookingCategorySub2ProductList = productData.poProductDetail.bookingCategorySub2ProductList;

      poProductItem.poProductDetail.bookingCategorySub3ProductList = productData.poProductDetail.bookingCategorySub3ProductList;

      poProductItem.poProductDetail.productCategoryName = productData.poProductDetail.productCategoryName;

      poProductItem.poProductDetail.productSubCategoryName = productData.poProductDetail.productSubCategoryName;

      poProductItem.poProductDetail.productCategorySub2Name = productData.poProductDetail.productCategorySub2Name;

      poProductItem.poProductDetail.productCategorySub3Name = productData.poProductDetail.productCategorySub3Name;

      poProductItem.poProductDetail.isDisplayMaster = productData.poProductDetail.isDisplayMaster;

      poProductItem.poProductDetail.parentProductId = productData.poProductDetail.parentProductId;

      poProductItem.poProductDetail.parentProductName = productData.poProductDetail.parentProductName;

      poProductItem.poProductDetail.isEcopack = productData.poProductDetail.isEcopack;

      poProductItem.poProductDetail.factoryReference = productData.poProductDetail.factoryReference;

    }

  }

  //update the aql name to given item
  updateAqlNameToPoProductItem(aqlId, item) {
    if (aqlId) {
      var aqlData = this.bookingMasterData.bookingProductAqlList.find(x => x.id == aqlId);
      if (aqlData)
        item.poProductDetail.aqlName = aqlData.value;
    }
  }

  //update critical name value to given product
  updateCriticalNameToPoProductItem(criticalId, poProductItem) {
    if (criticalId) {
      var criticalData = this.bookingMasterData.bookingProductPickList.find(x => x.id == criticalId);
      if (criticalData)
        poProductItem.poProductDetail.criticalName = criticalData.value;
    }
  }

  //update major name value to given product
  updateMajorNameToPoProductItem(majorId, poProductItem) {
    if (majorId) {
      var majorToleranceData = this.bookingMasterData.bookingProductPickList.find(x => x.id == majorId);
      if (majorToleranceData)
        poProductItem.poProductDetail.majorName = majorToleranceData.value;
    }
  }

  //update minor name value to given product
  updateMinorDataToPoProductItem(minorId, poProductItem) {
    if (minorId) {
      var minorToleranceData = this.bookingMasterData.bookingProductPickList.find(x => x.id == minorId);
      if (minorToleranceData)
        poProductItem.poProductDetail.minorName = minorToleranceData.value;
    }
  }

  //update the primary unit data
  updatePrimaryProductUnitName(productData, poProductItem) {
    if (productData.poProductDetail.unit) {
      var unitData = this.bookingMasterData.bookingProductUnitList.find(x => x.id == productData.poProductDetail.unit);
      if (unitData) {
        poProductItem.poProductDetail.unit = unitData.id;
        poProductItem.poProductDetail.unitName = unitData.name;
      }
    }
  }

  //change the critical dropdown value
  changeCritical(event, poProductItem) {
    if (event) {
      poProductItem.poProductDetail.criticalName = event.value;
      this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.Critical);
    }
  }

  //change the major dropdown value
  changeMajor(event, poProductItem) {
    if (event) {
      poProductItem.poProductDetail.majorName = event.value;
      this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.Major);
    }
  }

  //change the minor dropdown value
  changeMinor(event, poProductItem) {
    if (event) {
      poProductItem.poProductDetail.minorName = event.value;
      this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.Minor);
    }
  }

  //update the primary product data to the child product based on the field type
  updatePrimaryProductDataToChild(poProductItem, bookingProductFieldType) {
    var productData = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == poProductItem.poProductDetail.productId && x.poProductDetail.isPrimaryProductInGroup);

    if (productData) {
      var childProductsInGroup = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == poProductItem.poProductDetail.productId && !x.poProductDetail.isPrimaryProductInGroup);
      if (childProductsInGroup && childProductsInGroup.length > 0) {


        switch (bookingProductFieldType) {
          case this._bookingProductFieldType.Aql: {
            childProductsInGroup.map(x => x.poProductDetail.aql = productData.poProductDetail.aql);
            childProductsInGroup.map(x => x.poProductDetail.aqlName = productData.poProductDetail.aqlName);
            break;
          }
          case this._bookingProductFieldType.Critical: {
            childProductsInGroup.map(x => x.poProductDetail.critical = productData.poProductDetail.critical);
            childProductsInGroup.map(x => x.poProductDetail.criticalName = productData.poProductDetail.criticalName);
            break;
          }
          case this._bookingProductFieldType.Major: {
            childProductsInGroup.map(x => x.poProductDetail.major = productData.poProductDetail.major);
            childProductsInGroup.map(x => x.poProductDetail.majorName = productData.poProductDetail.majorName);
            break;
          }
          case this._bookingProductFieldType.Minor: {
            childProductsInGroup.map(x => x.poProductDetail.minor = productData.poProductDetail.minor);
            childProductsInGroup.map(x => x.poProductDetail.minorName = productData.poProductDetail.minorName);
            break;
          }
          case this._bookingProductFieldType.Unit: {
            childProductsInGroup.map(x => x.poProductDetail.unit = productData.poProductDetail.unit);
            childProductsInGroup.map(x => x.poProductDetail.unitName = productData.poProductDetail.unitName);
            break;
          }
          case this._bookingProductFieldType.UnitCount:
            childProductsInGroup.map(x => x.poProductDetail.unitCount = productData.poProductDetail.unitCount);
            break;
          case this._bookingProductFieldType.BarCode:
            childProductsInGroup.map(x => x.poProductDetail.barcode = productData.poProductDetail.barcode);
            break;
          case this._bookingProductFieldType.FactoryReference:
            childProductsInGroup.map(x => x.poProductDetail.factoryReference = productData.poProductDetail.factoryReference);
            break;
          case this._bookingProductFieldType.ProductCategory: {
            childProductsInGroup.map(x => x.poProductDetail.productCategoryId = productData.poProductDetail.productCategoryId);
            childProductsInGroup.map(x => x.poProductDetail.productCategoryName = productData.poProductDetail.productCategoryName);
            break;
          }
          case this._bookingProductFieldType.ProductSubCategory: {
            childProductsInGroup.map(x => x.poProductDetail.productSubCategoryId = productData.poProductDetail.productSubCategoryId);
            childProductsInGroup.map(x => x.poProductDetail.productSubCategoryName = productData.poProductDetail.productSubCategoryName);
            break;
          }
          case this._bookingProductFieldType.ProductSubCategory2: {
            childProductsInGroup.map(x => x.poProductDetail.productCategorySub2Id = productData.poProductDetail.productCategorySub2Id);
            childProductsInGroup.map(x => x.poProductDetail.productCategorySub2Name = productData.poProductDetail.productCategorySub2Name);
            break;
          }
          case this._bookingProductFieldType.ProductSubCategory3: {
            childProductsInGroup.map(x => x.poProductDetail.productCategorySub3Id = productData.poProductDetail.productCategorySub3Id);
            childProductsInGroup.map(x => x.poProductDetail.productCategorySub3Name = productData.poProductDetail.productCategorySub3Name);
            break;
          }
          case this._bookingProductFieldType.CustomSampleTypeName: {
            childProductsInGroup.map(x => x.poProductDetail.sampleType = productData.poProductDetail.sampleType);
            childProductsInGroup.map(x => x.poProductDetail.customSampleTypeName = productData.poProductDetail.customSampleTypeName);
            break;
          }
          case this._bookingProductFieldType.CustomSampleSize: {
            //check selected custom sample type belongs to percent type
            var percentData = CustomTypePercentList.find(x => x.id == productData.poProductDetail.sampleType);
            //if selected custom sample type is percent data
            if (percentData && percentData.value) {
              //take the current booking quantity
              var bookingQty = productData.poProductDetail.bookingQuantity;
              //add the booking quantity with the sum of all child products
              bookingQty = bookingQty + childProductsInGroup.reduce((sum, current) =>
                sum + current.poProductDetail.bookingQuantity, 0);
              //calculate the sample quantity with the percentage value
              var sampleQuantity = Math.ceil(bookingQty * (percentData.value) / 100);
              //assign the sample quantity to current product
              productData.poProductDetail.aqlQuantity = sampleQuantity;
              //assign the sample quantity to child products
              childProductsInGroup.map(x => x.poProductDetail.aqlQuantity = sampleQuantity);
            }
            else
              childProductsInGroup.map(x => x.poProductDetail.aqlQuantity = productData.poProductDetail.aqlQuantity);
            break;
          }
          case this._bookingProductFieldType.IsEcoPack: {
            childProductsInGroup.map(x => x.poProductDetail.isEcopack = productData.poProductDetail.isEcopack);
            break;
          }
          case this._bookingProductFieldType.Remarks: {
            childProductsInGroup.map(x => x.poProductDetail.remarks = productData.poProductDetail.remarks);
            break;
          }
          case this._bookingProductFieldType.FBTemplate: {
            childProductsInGroup.map(x => x.poProductDetail.fbTemplateId = productData.poProductDetail.fbTemplateId);
            childProductsInGroup.map(x => x.poProductDetail.fbTemplateName = productData.poProductDetail.fbTemplateName);
            break;
          }

        }
      }
      //if only one product is available then update the custom aql quantity
      else {
        if (bookingProductFieldType == this._bookingProductFieldType.CustomSampleSize
          && productData.poProductDetail.sampleType != SampleType.Other)
          this.calculateAQLQuantityOnCustomSampleType(productData);
      }
    }

  }

  //update the empty values to the child list
  updateEmtpyValueToChildProducts(poProductItem, bookingProductFieldType) {
    var childProductsInGroup = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == poProductItem.poProductDetail.productId && !x.poProductDetail.isPrimaryProductInGroup);
    if (childProductsInGroup && childProductsInGroup.length > 0) {
      switch (bookingProductFieldType) {
        case this._bookingProductFieldType.Aql: {
          childProductsInGroup.map(x => x.poProductDetail.aql = null);
          childProductsInGroup.map(x => x.poProductDetail.aqlName = null);
          break;
        }
        case this._bookingProductFieldType.Critical: {
          childProductsInGroup.map(x => x.poProductDetail.critical = null);
          childProductsInGroup.map(x => x.poProductDetail.criticalName = null);
          break;
        }
        case this._bookingProductFieldType.Major: {
          childProductsInGroup.map(x => x.poProductDetail.major = null);
          childProductsInGroup.map(x => x.poProductDetail.majorName = null);
          break;
        }
        case this._bookingProductFieldType.Minor: {
          childProductsInGroup.map(x => x.poProductDetail.minor = null);
          childProductsInGroup.map(x => x.poProductDetail.minorName = null);
          break;
        }
        case this._bookingProductFieldType.FBTemplate: {
          childProductsInGroup.map(x => x.poProductDetail.fbTemplateId = null);
          childProductsInGroup.map(x => x.poProductDetail.fbTemplateName = null);
          break;
        }
        case this._bookingProductFieldType.DisplayChild: {
          childProductsInGroup.map(x => x.poProductDetail.parentProductId = null);
          childProductsInGroup.map(x => x.poProductDetail.parentProductName = null);
          break;
        }
        case this._bookingProductFieldType.ProductSubCategory: {
          childProductsInGroup.map(x => x.poProductDetail.productSubCategoryId = null);
          childProductsInGroup.map(x => x.poProductDetail.productSubCategoryName = null);
          childProductsInGroup.map(x => x.poProductDetail.bookingCategorySubProductList = []);
          break;
        }
        case this._bookingProductFieldType.ProductSubCategory2: {
          childProductsInGroup.map(x => x.poProductDetail.productCategorySub2Id = null);
          childProductsInGroup.map(x => x.poProductDetail.productCategorySub2Name = null);
          childProductsInGroup.map(x => x.poProductDetail.bookingCategorySub2ProductList = []);
          break;
        }
        case this._bookingProductFieldType.ProductSubCategory3: {
          childProductsInGroup.map(x => x.poProductDetail.productCategorySub3Id = null);
          childProductsInGroup.map(x => x.poProductDetail.productCategorySub3Name = null);
          childProductsInGroup.map(x => x.poProductDetail.bookingCategorySub3ProductList = []);
          break;
        }
      }
    }
  }

  //change unit dropdown
  onChangeUnit(event, poProductItem) {
    poProductItem.poProductDetail.unitName = event.name;
    this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.Unit);
  }

  //change unit count
  onChangeUnitCount(poProductItem) {
    this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.UnitCount);
  }

  onChangeBarCode(poProductItem) {
    this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.BarCode);
  }

  getProductCategoryList() {
    this.customerProductService.getProductCategoryList()
      .pipe()
      .subscribe(
        response => {

          if (response && response.result == 1) {
            this.bookingMasterData.bookingProductCategoryList = response.productCategoryList;
          }
          else {
            this.error = response.result;
          }

        },
        error => {
          this.setError(error);
          this.showError('PRODUCT_CATEGORY.TITLE', 'PRODUCT_CATEGORY.MSG_UNKNOWN_ERROR');
        });

  }

  resetContainerSelection() {

    this.bookingMasterData.parentContainerId = null;

    this.bookingMasterData.bookingPOProductValidators.forEach(poProductData => {
      if (this.containerList.filter(x => x.id == poProductData.poProductDetail.containerId).length == 0) {
        poProductData.poProductDetail.containerId = null;
      }
    });

  }

  resetBookingProducts() {
    this.bookingMasterData.productsCategoryloading = true;
    this.bookingMasterData.bookingPOProductValidators = [];
    this.bookingMasterData.poProductList = [];
    this.bookingMasterData.selectedPOList = [];

    this.model.serviceTypeId = null;
    this.bookingMasterData.ponumber = null;
    this.bookingMasterData.poList = null;
    this.purchaseModel.id = 0;
  }

  clearParentProductCategoryRelatedChanges() {
    this.bookingMasterData.parentProductSubCategoryId = null;
    this.bookingMasterData.parentProductCategorySub2Id = null;
    this.bookingMasterData.parentProductCategorySub3Id = null;
    this.bookingMasterData.bookingParentSubProductCategoryList = [];
    this.bookingMasterData.bookingSub2ProductCategoryList = [];
    this.bookingMasterData.bookingSub3ProductCategoryList = null;
  }

  //region ParentProductCategory Change

  //change the parent product category value
  changeParentProductCategory(event) {
    if (event) {

      this.clearParentProductCategoryRelatedChanges();

      this.getParentProductSubcategoryList(event);
    }

  }

  //get the parent product sub category list
  getParentProductSubcategoryList(productCategory) {

    this.customerProductService.getProductSubCategoryList(productCategory.id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.processParentProductSubCategoryList(response, productCategory);
          }
          else {
            this.error = response.result;
          }
        },
        error => {
          this.setError(error);
          this.showError('PRODUCT_SUB_CATEGORY.TITLE', 'PRODUCT_SUB_CATEGORY.MSG_UNKNOWN_ERROR');
        });
  }


  //process the parent product sub category list
  processParentProductSubCategoryList(response, productCategory) {
    //assign the product sub category list
    this.bookingMasterData.bookingParentSubProductCategoryList = response.productSubCategoryList;

    //assign the product category id and product subcategory list
    this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
      if (!productTrans.poProductDetail.productCategoryMapped) {
        productTrans.poProductDetail.productCategoryId = productCategory.id;
        productTrans.poProductDetail.productCategoryName = productCategory.name;
      }
      if (productTrans.poProductDetail.productCategoryId == productCategory.id)
        productTrans.poProductDetail.bookingCategorySubProductList = this.bookingMasterData.bookingParentSubProductCategoryList;
    });


  }

  //change the parent product sub category list
  getParentProductSubCategoryDetails(event) {

    this.bookingMasterData.bookingSub2ProductCategoryList = [];
    this.bookingMasterData.bookingSub3ProductCategoryList = [];

    if (event) {
      this.customerProductService.getProductCategorySub2(event.id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == DataSourceResult.Success) {
              this.processParentProductSubCategoryResponse(event, response);
            }
            else {
              this.error = response.result;
            }
          },
          error => {
            this.setError(error);
            this.showError('PRODUCT_CATEGORY.TITLE', 'PRODUCT_CATEGORY.MSG_UNKNOWN_ERROR');
          });
    }
  }

  processParentProductSubCategoryResponse(productSubCategory, response) {

    this.bookingMasterData.bookingSub2ProductCategoryList = response.productCategorySub2List;

    this.bookingMasterData.parentProductCategorySub2Id = null;

    this.bookingMasterData.parentProductCategorySub3Id = null;

    this.assignParentProductSubCategoryRelatedChanges(productSubCategory);

  }

  assignParentProductSubCategoryRelatedChanges(productSubCategory) {
    if (this.bookingMasterData.bookingPOProductValidators && this.bookingMasterData.bookingPOProductValidators.length > 0) {

      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        //assign the product subcategoryid and subcategory list
        if (!productTrans.poProductDetail.productSubCategoryMapped
          && productTrans.poProductDetail.productCategoryId == this.bookingMasterData.parentProductCategoryId) {

          productTrans.poProductDetail.productSubCategoryId = productSubCategory.id;
          productTrans.poProductDetail.productSubCategoryName = productSubCategory.name;

          productTrans.poProductDetail.productCategorySub2Id = null;
          productTrans.poProductDetail.productCategorySub2Name = null;
          productTrans.poProductDetail.bookingCategorySub2ProductList = [];

          productTrans.poProductDetail.productCategorySub3Id = null;
          productTrans.poProductDetail.productCategorySub3Name = null;
          productTrans.poProductDetail.bookingCategorySub3ProductList = [];


        }

        //assign the product subcategory2 list(not mapped already) in which the given product subcategory is matched and
        if (productTrans.poProductDetail.productSubCategoryId == productSubCategory.id
          && productTrans.poProductDetail.productCategoryId == this.bookingMasterData.parentProductCategoryId) {
          productTrans.poProductDetail.bookingCategorySub2ProductList = this.bookingMasterData.bookingSub2ProductCategoryList;
        }

      });

    }
  }

  //change the parent product subcategory2 list
  changeParentProductSubCategory2(event) {
    if (event) {
      this.bookingMasterData.bookingSub3ProductCategoryList = [];
      this.bookingMasterData.parentProductCategorySub3Id = null;
      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {
        if (!productTrans.poProductDetail.productCategorySub2Mapped
          && productTrans.poProductDetail.productSubCategoryId == this.bookingMasterData.parentProductSubCategoryId) {
          productTrans.poProductDetail.productCategorySub2Id = event.id;
          productTrans.poProductDetail.productCategorySub2Name = event.name;
          productTrans.poProductDetail.bookingCategorySub3ProductList = [];
          productTrans.poProductDetail.productCategorySub3Id = null;
        }
      });
      this.updateParentProductSubCategory2List(event);

    }
  }

  updateParentProductSubCategory2List(productSubCategory2) {
    this.customerProductService.getProductCategorySub3(productSubCategory2.id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {

            this.bookingMasterData.bookingSub3ProductCategoryList = response.productSubCategory3List;

            //assign the product category id and product subcategory list
            this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {


              //assign the product subcategory2 list(not mapped already) in which the given product subcategory is matched and
              if (productTrans.poProductDetail.productCategorySub2Id == productSubCategory2.id
                && productTrans.poProductDetail.productCategoryId == this.bookingMasterData.parentProductCategoryId
                && productTrans.poProductDetail.productSubCategoryId == this.bookingMasterData.parentProductSubCategoryId) {

                productTrans.poProductDetail.bookingCategorySub3ProductList = this.bookingMasterData.bookingSub3ProductCategoryList;
              }

            });



          }
          else {
            this.error = response.result;
          }
        },
        error => {
          this.setError(error);
          this.showError('PRODUCT_SUB_CATEGORY.TITLE', 'PRODUCT_SUB_CATEGORY.MSG_UNKNOWN_ERROR');
        });
  }

  updateParentProductSubCategory3(productSubCategory3) {

    if (productSubCategory3) {
      this.bookingMasterData.bookingPOProductValidators.forEach(productTrans => {

        if (!productTrans.poProductDetail.productCategorySub3Id
          && productTrans.poProductDetail.productCategorySub2Id == this.bookingMasterData.parentProductCategorySub2Id) {
          productTrans.poProductDetail.productCategorySub3Id = productSubCategory3.id;
          productTrans.poProductDetail.productCategorySub3Name = productSubCategory3.name;
        }

      });
    }

  }

  getserviceLevelPickFirst() {
    this.serviceConfig.getServiceLevelPickFirst()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.bookingMasterData.bookingProductAqlList = res.levelPickList;
            this.bookingMasterData.bookingProductPickList = res.pickList;
          }
          else {
            this.error = res.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getUnitList() {
    this.service.GetUnitList()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.bookingMasterData.bookingProductUnitList = res.unitList;
          }
          else {
            this.bookingMasterData.bookingProductUnitList = [];
          }
        },
        error => {
          console.log(error);
        }
      );
  }

  CalculateTotalBookingQty() {
    var totalQty = 0;

    totalQty = this.bookingMasterData.bookingPOProductValidators.reduce((sum, current) => sum + current.poProductDetail.bookingQuantity, 0);
    this.bookingMasterData.totalBookingQty = totalQty;
  }

  //calculate total picking quantity for the given item and then update the total picking quantity
  /* CalculateTotalPickingQtyByItem(item) {
    //calculate the total picking quantity for given po product item
    item.poProductDetail.pickingQuantity = item.poProductDetail.saveInspectionPickingList.reduce((sum, current) => sum + current.pickingQuantity, 0);
    var totalQty = 0;
    //calculate the total picking qty for all the products
    totalQty = this.bookingMasterData.bookingPOProductValidators.reduce((sum, current) => sum + current.poProductDetail.pickingQuantity, 0);
    this.bookingMasterData.totalPickingQty = totalQty;
  } */

  //calculate the total picking qty for all the products
  CalculateTotalPickingQuantity() {

    var totalQty = 0;
    //calculate picking for each po product item
    this.bookingMasterData.bookingPOProductValidators.forEach(item => {
      if (item.poProductDetail.saveInspectionPickingList) {
        item.poProductDetail.pickingQuantity = item.poProductDetail.saveInspectionPickingList.reduce((sum, current) => sum + current.pickingQuantity, 0);
        totalQty = totalQty + item.poProductDetail.pickingQuantity;
      }
    });

    this.bookingMasterData.totalPickingQty = totalQty;
  }

  CalculateTotalContainers() {
    var totalContainers = 0;

    var lstdistinctContainers = this.bookingMasterData.bookingPOProductValidators.filter((thing, i, arr) => {
      return arr.indexOf(arr.find(t => t.poProductDetail.containerId === thing.poProductDetail.containerId)) === i;
    }).filter(x => x.poProductDetail.containerId).map(x => x.poProductDetail.containerId);

    this.bookingMasterData.totalContainers = lstdistinctContainers.length;

  }

  ChangeBookingQty(item) {

    this.CalculateTotalBookingQty();
    this.processCustomAQLQuanity(item);
  }


  CalculateTotalSamplingSize() {
    var samplingSize = 0;
    if (this.model.inspectionProductList) {

      //take the combine product ids in the inspection product list
      var combineProductIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.combineGroupId != null).map(x => x.poProductDetail.productId);

      //take the distinct combine product ids
      var lstdistinctCombineProductIds = combineProductIds.filter((n, i) => combineProductIds.indexOf(n) === i);

      //loop through the combine product ids and sum the combine sampling size
      lstdistinctCombineProductIds.forEach(productId => {
        var combineProduct = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productId && x.poProductDetail.combineSamplingSize > 0);
        if (combineProduct)
          samplingSize = samplingSize + combineProduct.poProductDetail.combineSamplingSize;
      });

      //take the non combine product ids in the inspection product list
      var nonCombineProductIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.combineGroupId == null).map(x => x.poProductDetail.productId);

      //take the distinct non combine product ids
      var lstdistinctNonCombineProductIds = nonCombineProductIds.filter((n, i) => nonCombineProductIds.indexOf(n) === i);

      //loop through the combine product ids and sum the sampling size
      lstdistinctNonCombineProductIds.forEach(productId => {
        var aqlProduct = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productId && x.poProductDetail.aqlQuantity > 0);
        if (aqlProduct)
          samplingSize = samplingSize + aqlProduct.poProductDetail.aqlQuantity;
      });

    }

    this.bookingMasterData.totalSamplingSize = samplingSize;
  }

  ChangePickingQty(item) {

    if (item.bookingPoData.pickingQuantity != 0 && item.bookingPoData.pickingQuantity != null) {
      this.bookingMasterData.totalPickingQty = this.bookingMasterData.totalPickingQty + item.bookingPoData.pickingQuantity;
    }

  }

  selectProductFiles(item) {
    var dataList = this.bookingMasterData.bookingProductFiles;

    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.fileSize,
      uploadFileExtensions: 'png,jpg,jpeg',
      uploadLimit: this.bookingMasterData.uploadLimit,
      containerId: FileContainerList.Products,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;


    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {

          let fileItem: ProductFileAttachment = {
            fileName: element.fileName,
            fileUrl: element.fileUrl,
            isNew: true,
            id: 0,
            mimeType: element.mimeType,
            productId: item.productId,
            uniqueld: element.uniqueld,
            fileDescription: element.fileDescription
          };
          item.productFileAttachments.push(fileItem);

          item.productImageCount = item.productImageCount + 1;


          if (dataList) {
            dataList.push({
              "productId": item.productId,
              "productName": item.productName,
              "productDesc": item.productDesc,
              "fileName": element.fileName, "file": fileItem
            });
          }

        });
      }

    }, (reason) => {

    });


  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  GetInspBookingContact(factoryId, customerId) {

    if (factoryId && factoryId > 0 && customerId && customerId > 0) {
      this.service.GetInspBookingContact(factoryId, customerId).subscribe(
        response => {
          if (response && response.result == 1) {
            this.bookingMasterData.officedetails.officeAddress = response.bookingContact.officeAddress;
            this.bookingMasterData.officedetails.officeFax = response.bookingContact.officeFax;
            this.bookingMasterData.officedetails.officeName = response.bookingContact.officeName;
            this.bookingMasterData.officedetails.officeTelNo = response.bookingContact.officeTelNo;
            this.bookingMasterData.officedetails.planningEmailTo = response.bookingContact.planningEmailTo;
          }
        }, error => {
          this.setError(error);
        }
      );
    }
  }

  GetInspBookingRuleDetails(customerId, factoryId) {

    if (customerId) {
      this.service.GetInspBookingRules(customerId, factoryId)
        .subscribe(response => {

          if (response && response.result == 1) {
            this._HolidayDates = response.bookingRuleList.holidays;
            this._leadtime = response.bookingRuleList.leadDays;
            this._serviceLeadDayMessage = response.bookingRuleList.serviceLeadDaysMessage;
            this.BookingRuleDesc = response.bookingRuleList.bookingRule;
            this.GetLeadDays();
          }
          else {
            this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_CANNOT_GET_BOOKINGRULE');
          }
        }, error => {
          this.setError(error);
        }
        );
    }

  }

  GetLeadDays() {

    if (this._HolidayDates != null && this._HolidayDates.length > 0) {
      var count = 0;
      var leadtime: NgbDate = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime);
      for (var i = this.calendar.getToday(); !i.after(leadtime); i = this.calendar.getNext(i, 'd', 1)) {
        if (this._HolidayDates.filter(x => i.equals(x)).length > 0)
          count++;
      }
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime + count);
    }
    else {
      return this.leaddays = this.calendar.getNext(this.calendar.getToday(), 'd', this._leadtime);
    }
  }

  getReInspectionServiceTypes(customerId, previousBookingNo) {

    this.service.GetReInspectionServiceType(customerId)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.bookingMasterData.customerServiceList = data.customerServiceList;
            if (!previousBookingNo)
              this.model.serviceTypeId = null;
            if (this.model.serviceTypeId) {
              var service = this.bookingMasterData.customerServiceList.filter(x => x.id == this.model.serviceTypeId)[0];
              if (service) {
                this.bookingPreviewData.servicetype = service.name;
              }
            }
            this.bookingMasterData.productsCategoryloading = false;
          }
          else {
            this.error = data.result;
            this.bookingMasterData.productsCategoryloading = false;
          }

        },
        error => {
          this.bookingMasterData.productsCategoryloading = false;
          this.setError(error);

        });
  }
  getReInspectionTypes() {
    this.service.GetReInspectionTypes().subscribe(
      response => {
        if (response && response.result == 1) {
          this.bookingMasterData.reInspectionServiceTypes = response.reInspectionTypeList;
        }
        else {
          this.ErrorDetails(response);
        }
      }, error => {
        this.setError(error);
      }
    );
  }
  GetFileExtensionIcon(file) {
    var ext = file.fileName.toUpperCase().split('.').pop();
    if (ext == "XLS" || ext == "XLX" || ext == "XLSX" || ext == "XLSM")
      return "assets/images/uploaded-files.svg";
    else if (ext == "DOC" || ext == "DOCX")
      return "assets/images/uploaded-files-1.svg"
    else
      return "assets/images/uploaded-files-2.svg";
  }

  processReInspectionDetails(customerId: number, userType: number) {
    if (this.isReInspection) {
      this.model.statusId = BookingStatus.New;
      this.isPreviewPanelVisible = false;
      this.model.serviceDateFrom = null;
      this.model.serviceDateTo = null;
      this.model.isPickingRequired = false;
      this.model.compassBookingNo = null;
      this.model.isCombineRequired = false;
      this.model.isPickingRequired = false;

      this.model.inspectionProductList.forEach(poProduct => {

        poProduct.id = 0;
        poProduct.poTransactionId = 0;
        poProduct.pickingQuantity = null;
        //poProduct.customerReferencePo=poProduct.base
        poProduct.aqlQuantity = null;
        poProduct.isDisplayMaster = false;
        poProduct.parentProductId = null;
        poProduct.saveInspectionPickingList = [];

      });

      this.bookingMasterData.isPicking = false;
      this.model.inspectionFileAttachmentList = [];
      this.bookingMasterData.totalReports = null;
      this.bookingMasterData.totalSamplingSize = null;
      this.bookingMasterData.sampleSizeVisible = false;

      this.applicantDetails();
    }

    if (this.isReInspection)
      this.model.serviceTypeId = null;
    // this.model.previousBookingNo means ReInspection Booking Edit scenario
    if (!this.isReInspection && !this.model.previousBookingNo) {
      this.bookingMasterData.sampleSizeVisible = true;
    }
    else if (this.isReInspection || this.model.previousBookingNo && !this.isReBooking) {
      this.getReInspectionTypes();
    }
  }

  processReBookingDetails(userType: number) {

    if (this.isReBooking) {
      this.model.statusId = BookingStatus.New;
      this.model.previousBookingNo = null;
      this.isPreviewPanelVisible = false;
      this.bookingMasterData.isPicking = false;
      this.model.serviceDateFrom = null;
      this.model.serviceDateTo = null;
      this.model.serviceTypeId = null;
      this.model.isPickingRequired = false;
      this.model.compassBookingNo = null;
      this.model.isCombineRequired = false;
      this.model.isPickingRequired = false;

      this.model.inspectionProductList.forEach(poProduct => {
        poProduct.id = 0;
        poProduct.poTransactionId = 0;
        poProduct.bookingQuantity = poProduct.poQuantity;
        poProduct.pickingQuantity = null;
        //poProduct.customerReferencePo = poProduct.baseCustomerReferencePo;
        poProduct.aqlQuantity = null;
        poProduct.parentProductId = null;
        poProduct.isDisplayMaster = false;
        poProduct.saveInspectionPickingList = [];
      });


      this.model.inspectionFileAttachmentList = [];
      this.bookingMasterData.totalReports = null;
      this.bookingMasterData.totalSamplingSize = null;
      this.bookingMasterData.sampleSizeVisible = false;
      this.model.internalComments = null;
      this.applicantDetails();
      /* if (this.bookingMasterData.bookingProductList) {
        this.bookingMasterData.bookingProductList.forEach(row => {
          row.productFileAttachments=[];
        });
      } */
    }
  }

  applicantDetails() {

    if (this.bookingMasterData.IsInternalUser) {
      this.getBookingStaffDetails();
    }
    else {
      this.getUserApplicantDetails();
      //this.getPreviousBookingApplicantDetails();
    }
  }
  assignReInspectionData() {
    // very first time of Reinspection
    // if (this.isReInspection && (this.model.previousBookingNo==0 || this.model.previousBookingNo==null)) {
    this.model.previousBookingNo = this.model.id;
    this.model.id = 0;

    this.model.inspectionProductList.forEach(poProduct => {
      poProduct.id = 0;
      poProduct.poTransactionId = 0;
    });

    this.model.inspectionDFTransactions.forEach(element => {
      element.id = 0;
    });
    //}
  }

  assignReBookingData() {
    this.model.id = 0;

    this.model.inspectionProductList.forEach(poProduct => {
      poProduct.id = 0;
      poProduct.poTransactionId = 0;
    });

    this.model.inspectionDFTransactions.forEach(element => {
      element.id = 0;
    });
  }

  showPickingDetails() {
    var pickingExists = this.bookingMasterData.bookingPOProductValidators.some(x => x.poProductDetail.saveInspectionPickingList
      && x.poProductDetail.saveInspectionPickingList.length > 0);

    if (pickingExists) {
      this.dialog = this.modalService.open(this.confirmRemovePickingTemplate, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
    }
    else {
      this.bookingMasterData.isPicking = !this.bookingMasterData.isPicking;
      this.bookingMasterData.bookingPOProductValidators.forEach(element => {
        element.poProductDetail.pickingQuantity = null;
      });
      this.bookingMasterData.totalPickingQty = null;
      this.getPickingRelatedDetails();
    }


  }

  validatePickingQuantity(isok): boolean {

    var ispickingData = false;
    if (this.bookingMasterData.bookingPOProductValidators.length > 0) {
      this.bookingMasterData.bookingPOProductValidators.forEach(item => {

        if (item.poProductDetail.pickingQuantity && item.poProductDetail.pickingQuantity > 0) {
          ispickingData = true;
        }
      });
    }

    return ispickingData;
  }

  checkPageType() {
    var type = this.pathroute.snapshot.paramMap.get("type");
    this.inspectionPageType = this._inspectionPageType.Boooking;
    if (type == this._bookingredirectpage.ReInspectionBooking.toString()) {
      this.isReInspection = true;
      this.inspectionPageType = this._inspectionPageType.ReInspection;
    }
    else if (type == this._bookingredirectpage.ReBooking.toString()) {
      this.isReBooking = true;
      this.inspectionPageType = this._inspectionPageType.ReBooking;
    }
    else if (type == this._bookingredirectpage.ViewBookingForQuotation.toString()) {
      this.isRedirectFromQuotation = true;
      this.inspectionPageType = this._inspectionPageType.Quotation;
    }
    else if (type == this._bookingredirectpage.SplitBooking.toString()) {
      // this.inspectionPageType = this._inspectionPageType.Boooking;
      this.bookingMasterData.isFromSplitBooking = true;

    }
  }

  setPreviewPageDefault() {
    if (!this.isReInspection && !this.isReBooking && !this.bookingMasterData.isFromSplitBooking) {

      var bookingPOData = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productCategorySub2Mapped == false);

      if (bookingPOData)
        this.isPreviewPanelVisible = false;
      else
        this.isPreviewPanelVisible = true;
    }
  }

  getCustomerContactByBrandOrDept() {
    this.customerContactModel.customerId = this.model.customerId;
    this.customerContactModel.bookingId = this.model.id;
    this.customerContactModel.brandIdlst = this.model.inspectionCustomerBrandList;
    this.customerContactModel.deptIdlst = this.model.inspectionCustomerDepartmentList;
    this.customerContactModel.customerServiceId = this._customerService.Inspection;
    this.bookingMasterData.cuscontactloading = true;
    this.service.GetCustomerContacts(this.customerContactModel)
      .subscribe(
        response => {
          this.processCustomerContactSuccessReponse(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.LBL_NOCUSTOMER_CONTACTS_FOUND');
          this.bookingMasterData.cuscontactloading = false;
        });
  }

  processCustomerContactSuccessReponse(response) {
    this.bookingMasterData.customerContactList = [];
    if (response && response.result == 1) {
      response.customerContactList.forEach(element => {
        element.contactName = element.contactName;
      });
      this.bookingMasterData.customerContactList = response.customerContactList;

      if (this.bookingMasterData.IsCustomer && !(this.model.id > 0)) {
        if (this.bookingMasterData.applicantContactId > 0) {
          if (response.customerContactList.length == 1) {
            // set first item by default if we have only one contact
            this.model.inspectionCustomerContactList = [response.customerContactList[0].id];
          }
          else if (response.customerContactList.length > 1) {
            // check applicant contact is present in the contact list
            var selectedItem = response.customerContactList.find(x => x.id == this.bookingMasterData.applicantContactId);
            if (selectedItem) {
              this.model.inspectionCustomerContactList = [selectedItem.id];
            }

          }
        }
      }
      else if (!(this.model.id > 0)) {
        if (response.customerContactList.length == 1) {
          // set first item by default if we have only one contact
          this.model.inspectionCustomerContactList = [response.customerContactList[0].id];
        }
      }
      this.setCustomerContactPreviewData(response.customerContactList);
    }
    else if (response && response.result == 2) {
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.LBL_NOCUSTOMER_CONTACTS_FOUND');
    }
    this.bookingMasterData.cuscontactloading = false;
  }

  //change product category value in the booking product table
  onChangeProductCategory(event, item) {

    if (event) {

      item.poProductDetail.productCategoryName = event.name;
      item.poProductDetail.productCategorySubLoading = true;

      this.clearProductCategoryRelatedChanges(item);

      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory);
      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory2);
      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory3);

      item.poProductDetail.productCategorySubLoading = true;

      this.getProductSubcategoryList(event, item);

      item.poProductDetail.productCategorySubLoading = false;

    }
  }

  clearProductCategoryRelatedChanges(item) {
    item.poProductDetail.productSubCategoryId = null;
    item.poProductDetail.productCategorySub2Id = null;
    item.poProductDetail.productCategorySub3Id = null;

    item.poProductDetail.productSubCategoryName = null;
    item.poProductDetail.productCategorySub2Name = null;
    item.poProductDetail.productCategorySub3Name = null;

    item.poProductDetail.bookingCategorySub2ProductList = [];
    item.poProductDetail.bookingCategorySub3ProductList = [];
  }

  //get the product sub category list
  getProductSubcategoryList(productCategory, item) {

    this.customerProductService.getProductSubCategoryList(productCategory.id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.processProductSubCategoryList(response, item, productCategory);
          }
          else {
            this.error = response.result;
          }
        },
        error => {
          this.setError(error);
          this.showError('PRODUCT_SUB_CATEGORY.TITLE', 'PRODUCT_SUB_CATEGORY.MSG_UNKNOWN_ERROR');
        });
  }

  processProductSubCategoryList(response, item, productCategory) {
    if (response && response.productSubCategoryList)
      item.poProductDetail.bookingCategorySubProductList = response.productSubCategoryList;

    var productList = this.bookingMasterData.bookingPOProductValidators.
      filter(x => x.poProductDetail.productId == item.poProductDetail.productId && !x.poProductDetail.isPrimaryProductInGroup);
    if (productList && productList.length > 0) {
      productList.forEach(productTrans => {
        productTrans.poProductDetail.productCategoryId = productCategory.id;
        productTrans.poProductDetail.productCategoryName = productCategory.name;

        productTrans.poProductDetail.bookingCategorySub2ProductList = [];
        productTrans.poProductDetail.bookingCategorySub3ProductList = [];

        productTrans.poProductDetail.bookingCategorySubProductList = response.productSubCategoryList;
      });
    }
  }

  //change the product subcategory value in the booking product list
  onChangeProductSubCategory(event, item) {

    if (event) {

      item.poProductDetail.productSubCategoryName = event.name;

      this.clearProductSubCategoryRelatedChanges(item);

      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory2);
      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory3);

      this.getProductSubCategory2List(item, event.id);

    }
  }

  clearProductSubCategoryRelatedChanges(item) {
    item.poProductDetail.bookingCategorySub2ProductList = [];
    item.poProductDetail.bookingCategorySub3ProductList = [];
    item.poProductDetail.productCategorySub2Id = null;
    item.poProductDetail.productCategorySub2Name = null;
    item.poProductDetail.productCategorySub3Id = null;
    item.poProductDetail.productCategorySub3Name = null;
  }

  //get the product sub category2 list
  getProductSubCategory2List(item, productSubCategoryId) {

    item.poProductDetail.productCategorySub2Loading = true;

    this.customerProductService.getProductCategorySub2(productSubCategoryId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {

            //assign the product sub category to all the products which mapped with the selected products
            var productList = this.bookingMasterData.bookingPOProductValidators.
              filter(x => x.poProductDetail.productId == item.poProductDetail.productId);
            if (productList && productList.length > 0) {
              productList.forEach(productTrans => {

                productTrans.poProductDetail.productSubCategoryId = item.poProductDetail.productSubCategoryId;
                productTrans.poProductDetail.productSubCategoryName = item.poProductDetail.productSubCategoryName;
                productTrans.poProductDetail.bookingCategorySub2ProductList = response.productCategorySub2List;

              });
            }

            item.poProductDetail.productCategorySub2Loading = false;

          }
          else {
            this.error = response.result;
            item.poProductDetail.productCategorySub2Loading = false;
          }
        },
        error => {
          item.poProductDetail.productCategorySub2Loading = false;
          this.setError(error);
          this.showError('PRODUCT_SUB_CATEGORY.TITLE', 'PRODUCT_SUB_CATEGORY.MSG_UNKNOWN_ERROR');
        });
  }

  //change the product category2 value in the booking product list
  onChangeProductSubCategory2(event, item) {
    if (event) {
      //assign the product sub category to all the products which mapped with the selected products
      item.poProductDetail.productCategorySub2Name = event.name;

      this.clearProductSubCategory2RelatedChanges(item);

      //this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory2);

      this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.ProductSubCategory3);

      this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.ProductSubCategory2);

      this.getProductSubCategory3List(item, event.id);

    }
  }

  clearProductSubCategory2RelatedChanges(item) {
    item.poProductDetail.productCategorySub3Id = null;
    item.poProductDetail.productCategorySub3Name = null;
    item.poProductDetail.bookingCategorySub3ProductList = [];
  }

  getProductSubCategory3List(item, productSubCategory2Id) {
    item.poProductDetail.productCategorySub3Loading = true;
    this.customerProductService.getProductCategorySub3(productSubCategory2Id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            if (response.productSubCategory3List)
              this.processProductSubCategory3ListResponse(item, response.productSubCategory3List);
          }
          else {
            this.error = response.result;
          }
          item.poProductDetail.productCategorySub3Loading = false;
        },
        error => {
          item.poProductDetail.productCategorySub3Loading = false;
          this.setError(error);
          this.showError('PRODUCT_SUB_CATEGORY.TITLE', 'PRODUCT_SUB_CATEGORY.MSG_UNKNOWN_ERROR');
        });
  }

  //apply the productSubCategory3List
  processProductSubCategory3ListResponse(item, productSubCategory3List) {
    var productList = this.bookingMasterData.bookingPOProductValidators.
      filter(x => x.poProductDetail.productId == item.poProductDetail.productId);

    productList.forEach(productTran => {
      productTran.poProductDetail.bookingCategorySub3ProductList = productSubCategory3List;
    });

  }

  //change the product category3 value in the booking product list
  onChangeProductSubCategory3(event, item) {
    if (event) {
      item.poProductDetail.productCategorySub3Name = event.name;
      this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.ProductSubCategory3);
    }
  }

  isBookingProductDelete() {

    if (this.model.id != 0)
      this.isProductDelete = this.isCreatedUserDiffers();
    else if (this.model.id == 0)
      this.isProductDelete = true;

    return this.isProductDelete;
  }

  isCreatedUserDiffers() {
    if ((this.model.CreatedUserType != null && this.model.CreatedUserType == this.bookingMasterData.userTypeId) || this.bookingMasterData.IsInternalUser)
      return true;
    else
      return false;
  }

  setCustomerContactPreviewData(customerContactList) {
    this.bookingPreviewData.customerContact = [];
    var contactsList = customerContactList.filter(x => this.model.inspectionCustomerContactList.includes(x.id));
    if (contactsList) {
      contactsList.forEach(element => {
        var contactDetailPreview = new ContactDetailPreview();
        contactDetailPreview.name = element.contactName + "(" + element.email + ")";
        this.bookingPreviewData.customerContact.push(contactDetailPreview);
      });
    }
  }

  setSupplierContactPreviewData(supplierContactList) {
    this.bookingPreviewData.supplierContact = [];
    var contactsList = supplierContactList.filter(x => this.model.inspectionSupplierContactList.includes(x.contactId));
    if (contactsList) {
      contactsList.forEach(element => {
        var contactDetailPreview = new ContactDetailPreview();
        contactDetailPreview.name = element.contactName + "(" + element.contactEmail + "/" + element.phone + ")";
        this.bookingPreviewData.supplierContact.push(contactDetailPreview);
      });
    }
  }

  setFactoryContactPreviewData(factoryContactList) {
    var contactsList = factoryContactList.filter(x => this.model.inspectionFactoryContactList.includes(x.contactId));
    this.bookingPreviewData.factoryContact = [];
    if (factoryContactList) {
      contactsList.forEach(element => {
        var contactDetailPreview = new ContactDetailPreview();
        contactDetailPreview.name = element.contactName + "(" + element.contactEmail + "/" + element.phone + ")";
        this.bookingPreviewData.factoryContact.push(contactDetailPreview);
      });
    }
  }

  CalculateTotalProducts() {
    var totalQty = 0;
    var lstdistinctProducts = this.bookingMasterData.bookingPOProductValidators.filter((thing, i, arr) => {
      return arr.indexOf(arr.find(t => t.poProductDetail.productId === thing.poProductDetail.productId)) === i;
    });

    this.bookingMasterData.totalProducts = lstdistinctProducts.length;
  }


  assignBookingDefaultComments(isEdit, response) {
    if (response.bookingDefaultComments && (!isEdit || this.isReInspection || this.isReBooking))
      this.model.qcBookingComments = response.bookingDefaultComments;
  }

  assignDestinationCountryName(item) {
    item.poProductData.destinationCountryName = null;
    item.poProductData.destinationCountryName = this.getDestinationCountry(item.poProductData.destinationCountryID);


    // Assing container name if service type as container
    if (this.isContainerService) {
      item.poProductData.containerName = null;
      item.poProductData.containerName = this.getContainerName(item.poProductData.containerId);
    }

  }

  getDestinationCountry(destinationCountryID) {
    return this.destinationCountryList && this.destinationCountryList.length > 0 && destinationCountryID != null && destinationCountryID != "" ?
      this.destinationCountryList.filter(x => x.id == destinationCountryID)[0].countryName : ""
  }

  getContainerName(containerId) {
    if (this.isContainerService) {
      return this.containerList.length > 0 && containerId != null && containerId != "" ?
        this.containerList.filter(x => x.id == containerId)[0].name : ""
    }
    else
      return "";
  }

  checkCountryAndFactoryReferenceExists() {

    var destinationCountryAvailable = false;
    var factoryReferenceAvailable = false;

    this.bookingMasterData.destinationCountryAvailable = this.model.inspectionProductList.
      filter(x => x.destinationCountryID > 0).length > 0 ? !destinationCountryAvailable : destinationCountryAvailable;

    this.bookingMasterData.factoryReferenceAvailable = this.model.inspectionProductList.
      filter(x => x.factoryReference != "").length > 0 ? !factoryReferenceAvailable : factoryReferenceAvailable;

  }
  // service from date and to date enable based on below conditions
  setDateDisabled() {
    /*user has request or verify and confirm role and any one of following status(verified, rescheduled, requested) and
     below condition will execute*/
    if ((this.model.isBookingRequestRole || this.model.isBookingVerifyRole) && this.model.isBookingConfirmRole &&
      (this.model.statusId == BookingStatus.Verified || this.model.statusId == BookingStatus.Rescheduled ||
        this.model.statusId == BookingStatus.Requested || this.model.statusId == BookingStatus.Hold)) {
      // enable the service date field
      this.isDateDisabled = false;
    }
    //user has only confirm role below condition will execute
    else if (this.model.isBookingConfirmRole) {
      //if the status is verified or reschedule we are enable the service date field
      this.isDateDisabled = !(this.model.statusId == BookingStatus.Verified ||
        this.model.statusId == BookingStatus.Rescheduled || this.model.statusId == BookingStatus.Hold);
    }
    //user has request or verify role below condition will execute
    else if (this.model.isBookingRequestRole || this.model.isBookingVerifyRole) {
      //if the status is request we are enable the service date field
      this.isDateDisabled = !(this.model.statusId == BookingStatus.Requested);
    }
  }


  // first service from date and to date enable based on below conditions
  setFirstServiceDateDisabled() {
    if (this.model.statusId == BookingStatus.New || this.model.statusId == BookingStatus.Requested || this.model.statusId == BookingStatus.Verified) {
      return false;
    }
    else {
      return true;
    }
  }

  // Get Dynamic Fields configured for the customer
  GetCustomerDynamicFields(customerId, moduleId) {
    this.dynamicFieldService.getcustomerconfigurationlist(customerId, moduleId)
      .subscribe(response => {
        if (response && response.result == 1 && response.dfCustomerConfigurationList) {
          //create the dynamic control object from the configuration list
          this.createDynamicControls(response.dfCustomerConfigurationList);
        }
      }, error => {
        this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_DF_CUSTOMERCONFIG_NOT_FOUND');
      }
      );
  }

  AssignDFCustomerConfigPreviewData() {

    if (this.dynamicControls && this.dynamicControls.length > 0) {
      var columnLimit = 3;
      var rowCount = Math.ceil(this.dynamicControls.length / columnLimit);
      var columnCount = this.dynamicControls.length < columnLimit
        ? this.dynamicControls.length : columnLimit;
      var columnIndexValue = 0;
      var columnDataIndex = 0;
      let dynamicControlChildPreviewData = [];
      for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
        //columnCount=rowIndex>0?(this.dynamicControls.length-(rowIndex*4))


        if (rowIndex > 0) {
          var actualRowDataCount = this.dynamicControls.length - (rowIndex * columnLimit);
          if (actualRowDataCount < columnLimit)
            columnCount = actualRowDataCount;
          else
            columnCount = columnLimit;
        }
        while (columnIndexValue < columnCount) {


          var control = this.dynamicControls[columnDataIndex];
          var previewData = new ConfigPreviewData();
          previewData.label = control.label;
          if (control.controlTypeId == this._controlType.DropDown) {
            var controlAttribute = control.controlAttributeList.find(x => x.attributeid == this._controlAttribute.Multiple);
            if (controlAttribute && controlAttribute.value == "true") {
              var data = control.dataSource.filter(x => control.value.includes(x.key));
              if (data && data.length > 0)
                previewData.value = data.map(e => e.value).join(",");
            }
            else {
              var data = control.dataSource.find(x => x.key == control.value);
              if (data)
                previewData.value = data.value;
            }


          }
          else if (control.controlTypeId == this._controlType.DatePicker) {
            var fromdate: NgbDate = control.value;
            if (fromdate)
              previewData.value = fromdate.day + "/" + fromdate.month + "/" + fromdate.year;
          }
          else {
            previewData.value = control.value;
          }
          dynamicControlChildPreviewData.push(previewData);
          columnDataIndex = columnDataIndex + 1
          columnIndexValue = columnIndexValue + 1;
        }

        this.bookingPreviewData.dfCustomerConfigPreviewBaseData.push(dynamicControlChildPreviewData);
        columnIndexValue = 0;
        dynamicControlChildPreviewData = [];

      }


    }

  }

  //Populate control objects based on the customer configured details
  createDynamicControls(dfCustomerConfigurationList) {
    dfCustomerConfigurationList.forEach(element => {
      //if control type is textbox
      if (element.controlTypeId == this._controlType.TextBox) {

        var txtbox = new TextboxControl({
          key: element.id,
          label: element.label,
          value: this.getControlValue(element.id, element.controlTypeId),
          required: element.requiredValidation,
          order: element.displayOrder,
          controlAttributeList: element.controlAttributeList,
          controlTypeId: element.controlTypeId,
          controlConfigurationId: element.id
        });
        //assign the control attributes
        this.addControlAttributes(txtbox, element.controlAttributeList);
        //assign the required field validator from attributelist
        this.assignValidationAttribute(txtbox, element.controlAttributeList);
        this.dynamicControls.push(txtbox);
      }
      //if the control type is datepicker
      else if (element.controlTypeId == this._controlType.DatePicker) {
        var txtbox = new DateTimePickerControl({
          key: element.id,
          label: element.label,
          value: this.getControlValue(element.id, element.controlTypeId),
          required: element.requiredValidation,
          order: element.displayOrder,
          controlAttributeList: [],
          controlTypeId: element.controlTypeId,
          controlConfigurationId: element.id
        });
        //assign the control attributes
        this.addControlAttributes(txtbox, element.controlAttributeList);
        //assign the required field validator from attributelist
        this.assignValidationAttribute(txtbox, element.controlAttributeList);
        this.dynamicControls.push(txtbox);
      }
      //if the control type is dropdown
      else if (element.controlTypeId == this._controlType.DropDown) {

        //codes commented by ganesh since there is no use
        //var dropDownType = this.dropdownTypeList.find(x => x.id == element.dropDownType);
        //codes commented by ganesh since there is no use

        var controlAttribute = element.controlAttributeList.find(x => x.attributeId == this._controlAttribute.Multiple);
        var dropDown = new DropDownControl({
          key: element.id,
          label: element.label,
          options: this.populateDropDownSourceList(element.ddlSourceList),
          dataSource: this.populateDropDownSourceList(element.ddlSourceList),
          order: element.displayOrder,
          dataSourceType: element.dataSourceType,
          controlAttributeList: [],
          controlTypeId: element.controlTypeId,
          controlConfigurationId: element.id,
          value: this.getDropDownControlValue(element.id, element.controlTypeId, controlAttribute.value),
        });
        this.addControlAttributes(dropDown, element.controlAttributeList);
        //check if it is the cascading dropdown with the parentdropdown attribute
        var isCascadingAttribute = element.controlAttributeList.find(x => x.attributeId == this._controlAttribute.ParentDropDown);
        if (isCascadingAttribute) {
          if (isCascadingAttribute.value) {
            //fetch the parent dropdown based on the value(ParentDropdown id mapped as datasourcetype child)
            var parentControl = this.dynamicControls.find(x => x.dataSourceType == Number(isCascadingAttribute.value));
            if (parentControl && parentControl.value)
              //Filter drop down options from parentcontrol selected value
              this.FilterDropDownOptions(dropDown, parentControl);
          }
        }
        //assign the validation attribute
        this.assignValidationAttribute(dropDown, element.controlAttributeList);
        this.dynamicControls.push(dropDown);
      }
      else if (element.controlTypeId == this._controlType.TextArea) {
        var txtarea = new TextAreaControl({
          key: element.id,
          label: element.label,
          value: this.getControlValue(element.id, element.controlTypeId),
          required: element.requiredValidation,
          order: element.displayOrder,
          controlAttributeList: [],
          controlTypeId: element.controlTypeId,
          controlConfigurationId: element.id
        });
        //add the control contributes list
        this.addControlAttributes(txtarea, element.controlAttributeList);
        //assign the validation attributes list
        this.assignValidationAttribute(txtarea, element.controlAttributeList);
        this.dynamicControls.push(txtarea);
      }
    });

    this.splitDynamicControlsWithRows();

    this.AssignDFCustomerConfigPreviewData();

  }

  splitDynamicControlsWithRows() {
    var rowCount = Math.ceil(this.dynamicControls.length / 4);
    var columnCount = this.dynamicControls.length < 4 ? this.dynamicControls.length : 4;
    var columnIndexValue = 0;
    var columnDataIndex = 0;
    this.dynamicControlBaseData = [];
    let dynamicControlChildData = [];
    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
      //columnCount=rowIndex>0?(this.dynamicControls.length-(rowIndex*4))

      if (rowIndex > 0) {
        var actualRowDataCount = this.dynamicControls.length - (rowIndex * 4);
        if (actualRowDataCount < 4)
          columnCount = actualRowDataCount;
        else
          columnCount = 4;
      }
      while (columnIndexValue < columnCount) {
        dynamicControlChildData.push(this.dynamicControls[columnDataIndex]);
        columnDataIndex = columnDataIndex + 1
        columnIndexValue = columnIndexValue + 1;
      }

      this.dynamicControlBaseData.push(dynamicControlChildData);
      columnIndexValue = 0;
      dynamicControlChildData = [];

    }

  }

  //Add attributes given for df(dynamic controls)
  addControlAttributes(control, controlAttributeList) {
    control.controlAttributeList = [];

    controlAttributeList.forEach(element => {

      var attribute = new ControlAttributes();
      attribute.id = element.id;
      attribute.attributeid = element.attributeId,
        attribute.key = element.controlAttributeId,
        attribute.value = element.value;
      attribute.active = element.active;
      control.controlAttributeList.push(attribute)
    });

  }

  //Populate options for dynamic dropdownlist
  populateDropDownSourceList(DDLSourceList) {
    var options = [];
    DDLSourceList.forEach(element => {
      var option = { key: element.id, value: element.name, parentId: element.parentId }
      options.push(option);
    });
    return options;
  }

  //assign the filtered datasource to child dropdown based on the parent dropdown
  FilterDropDownOptions(dropDown, parentControl) {
    var filteredDataSource = dropDown.dataSource.filter(x => x.parentId == parentControl.value);
    if (filteredDataSource) {
      dropDown.options = filteredDataSource;
    }
  }

  //assign the validation attribute list
  assignValidationAttribute(control, controlAttributes) {
    var requiredValidator = controlAttributes.find(x => x.attributeId == this._controlAttribute.RequiredValidation);
    if (requiredValidator && requiredValidator.value && requiredValidator.value == "true") {
      control.required = true;
    }
    else {
      control.required = false;
    }
    if (control.controlTypeId == this._controlType.DropDown && control.dataSourceType == DFDDLSourceType.GAPAuditProductCategory) {
      this.isDefaultProductCategoryRequired = control.required;
      this.IsProductCategoryRequired(control);
    }
  }

  IsProductCategoryRequired(control: any): boolean {
    const serviceType = this.bookingMasterData.customerServiceList.find(x => x.id == this.model.serviceTypeId);
    if (serviceType) {
      if (this.model.customerId == this._customerEnum.Gap && serviceType.name.toLowerCase() == FlashProcessAudit.toLowerCase())
        control.required = true;
      else
        control.required = this.isDefaultProductCategoryRequired;

      if (control.required && !control.value)
        control.requiredNotFound = true;
      else
        control.requiredNotFound = false;
    }
    return control.required;
  }
  //populate inspectiondftransaction before saving
  addInspectionDfTransaction() {
    this.dynamicControls.forEach(element => {
      //push textbox and textarea control value
      if (element.controlTypeId != this._controlType.DropDown
        && element.controlTypeId != this._controlType.DatePicker) {

        var inspectiondftransaction = this.model.inspectionDFTransactions.
          find(x => x.controlConfigurationId == element.controlConfigurationId);
        //If It is new control add into inspectionDFTransactions
        if (!inspectiondftransaction) {
          var transaction = new InspectionDFTransaction();
          transaction.id = 0;
          transaction.controlConfigurationId = element.controlConfigurationId;
          transaction.value = element.value;
          this.model.inspectionDFTransactions.push(transaction);
        }
        //else just update the value
        else {
          inspectiondftransaction.value = element.value;
        }
      }
      //push datepicker value
      else if (element.controlTypeId == this._controlType.DatePicker) {
        var inspectiondftransaction = this.model.inspectionDFTransactions.
          find(x => x.controlConfigurationId == element.controlConfigurationId);
        //If It is new dropdown control add into inspectionDFTransactions
        if (!inspectiondftransaction) {
          var transaction = new InspectionDFTransaction();
          transaction.id = 0;
          transaction.controlConfigurationId = element.controlConfigurationId;
          transaction.value = element.value != null ? element.value.year + "-" + element.value.month + "-" + element.value.day : null;
          this.model.inspectionDFTransactions.push(transaction);
        }
        //else update the new date value
        else {
          inspectiondftransaction.value = element.value != null ? element.value.year + "-" + element.value.month + "-" + element.value.day : null;
        }
      }
      //push dropdown value
      else if (element.controlTypeId == this._controlType.DropDown) {
        //take the multiple attribute of the given dropdown
        var controlAttribute = element.controlAttributeList.find(x => x.attributeid == this._controlAttribute.Multiple);
        //If it is multi dropdown list block will work
        if (controlAttribute.value == "true") {
          var multidropDownList = this.model.inspectionDFTransactions.
            filter(x => x.controlConfigurationId == element.controlConfigurationId);

          if (multidropDownList.length == 0) {
            element.value.forEach(item => {
              var transaction = new InspectionDFTransaction();
              transaction.id = 0;
              transaction.controlConfigurationId = element.controlConfigurationId;
              transaction.value = item;
              this.model.inspectionDFTransactions.push(transaction);
            });
          }
          else {
            var removedlist = multidropDownList.filter(x => !element.value.includes(x.value));
            removedlist.forEach(data => {
              var index = this.model.inspectionDFTransactions.
                findIndex(x => x.controlConfigurationId == element.controlConfigurationId && x.id == data.id);
              if (index >= 0)
                this.model.inspectionDFTransactions.splice(index, 1);
              var transactionValues = this.model.inspectionDFTransactions.map(({ value }) => value);
              var newTransactions = element.value.filter(x => !transactionValues.includes(x));
              newTransactions.forEach(newTransaction => {
                var transaction = new InspectionDFTransaction();
                transaction.id = 0;
                transaction.controlConfigurationId = element.controlConfigurationId;
                transaction.value = newTransaction;
                this.model.inspectionDFTransactions.push(transaction);
              });

            });

          }
        }
        //If it is single dropdown list block will work
        else {
          var inspectiondftransaction = this.model.inspectionDFTransactions.
            find(x => x.controlConfigurationId == element.controlConfigurationId);
          if (!inspectiondftransaction) {
            var transaction = new InspectionDFTransaction();
            transaction.id = 0;
            transaction.controlConfigurationId = element.controlConfigurationId;
            transaction.value = element.value;
            this.model.inspectionDFTransactions.push(transaction);
          }
          else {
            inspectiondftransaction.value = element.value;
          }
        }

      }
    });

  }

  //get and assign the value for textbox,textarea and datepicker in the edit scenario
  getControlValue(id, controlTypeId): any {
    //assign the value for textbox and textarea
    if (controlTypeId !== this._controlType.DropDown
      && controlTypeId !== this._controlType.DatePicker) {
      var transaction = this.model.inspectionDFTransactions.find(x => x.controlConfigurationId == id);
      return (transaction) ? transaction.value : '';
    }
    //assign the value for datepicker
    else if (this.model.id != 0 && controlTypeId == this._controlType.DatePicker) {
      var datePickertransaction = this.model.inspectionDFTransactions.find(x => x.controlConfigurationId == id);
      if (datePickertransaction) {
        var datevalues = datePickertransaction.value.split("-");
        var datevalue = { year: parseInt(datevalues[0]), month: parseInt(datevalues[1]), day: parseInt(datevalues[2]) };
        return datevalue;
      }
    }
    return null;
  }

  //get and assign dropdown values
  getDropDownControlValue(id, controlTypeId, isMultiple): any {
    if (isMultiple == "true") {
      var ddlMultiTransaction = this.model.inspectionDFTransactions.filter(x => x.controlConfigurationId == id);
      var valuearray = [];
      ddlMultiTransaction.forEach(element => {
        valuearray.push(Number(element.value));
      });

      return (valuearray.length > 0) ? valuearray : [];
    }
    else {
      var ddltransaction = this.model.inspectionDFTransactions.find(x => x.controlConfigurationId == id);
      return (ddltransaction && ddltransaction.value) ? Number(ddltransaction.value) : null;
    }
  }

  //validate the dynamic controls
  validateDynamicControls(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    var controls = this.dynamicControls;
    var isOk = true;

    for (var index = 0; index < this.dynamicControls.length; index++) {
      if (this.dynamicControls[index].controlAttributeList) {
        var requiredAttribute = this.dynamicControls[index].controlAttributeList.
          filter(x => x.attributeid == this._controlAttribute.RequiredValidation);
        //check if the requiredattribute configured for the controls if yes check the required validator
        if (requiredAttribute && requiredAttribute.length > 0) {

          if (requiredAttribute[0].value == "true") {
            this.dynamicControls[index].requiredNotFound = false;
            isOk = (this.dynamicControls[index].value != "" && this.dynamicControls[index].value != null) ? true : false;
            if (!isOk) {
              this.dynamicControls[index].requiredNotFound = true;
              this.showWarning("EDIT_BOOKING.TITLE", this.dynamicControls[index].label + " is required");
              break;
            }
          }
        }

        var emailAttribute = this.dynamicControls[index].controlAttributeList.
          filter(x => x.attributeid == this._controlAttribute.EmailValidation);
        //check email attribute configured for the control if yes check the email is valid
        if (emailAttribute && emailAttribute.length > 0) {
          if (emailAttribute[0].value == "true") {
            this.dynamicControls[index].emailNotValid = false;
            var EMAIL_REGEXP = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            isOk = EMAIL_REGEXP.test(this.dynamicControls[index].value);

            if (!isOk) {
              this.dynamicControls[index].emailNotValid = true;
              this.showWarning("EDIT_BOOKING.TITLE", this.dynamicControls[index].label + " is not a valid email");
              //            this.showWarning('EDIT_BOOKING.TITLE', 'Invalid Email Address');
              break;
            }
          }
        }

        if (this.dynamicControls[index].controlTypeId == ControlType.DropDown && this.dynamicControls[index].required) {
          if (this.dynamicControls[index].value == null)
            isOk = false;
          else
            isOk = true;
          if (!isOk) {
            this.dynamicControls[index].requiredNotFound = true;
            this.showWarning("EDIT_BOOKING.TITLE", this.dynamicControls[index].label + " is required");
            break;
          }
        }
      }
    }
    return isOk;
  }

  getFbTemplateList() {
    this.service.GetFbTemplateList()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.bookingMasterData.fbTemplateList = res.dataSourceList;
          }
          else {
            this.error = res.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  //get price category details by customerid and product cateogry sub 2 id
  getPriceCategory() {
    var request = new PriceCategoryRequest();

    //unique productCategorySub2Id
    var uniquePCSub2List = this.bookingMasterData.bookingPOProductValidators.map(x => x.poProductDetail.productCategorySub2Id).
      filter(function (elem, index, self) {
        return index === self.indexOf(elem);
      });

    //frame the request
    request = {
      customerId: this.model.customerId,
      productSubCategory2IdList: uniquePCSub2List,
      priceCategoryId: this.model.priceCategoryId
    };

    this.service.getPriceCategory(request)
      .pipe().subscribe(
        response => {

          if (response.result == PriceCategoryResult.Success) {
            this.model.customerPriceCategoryName = this.assignPriceCategoryName();
            this.bookingPreviewData.priceCategoryMessage = null;
          }
          else if (response.result == PriceCategoryResult.MismatchPriceCategory) {

            this.model.customerPriceCategoryName = this.assignPriceCategoryName();

            this.bookingPreviewData.priceCategoryMessage = this.utility.textTranslate('EDIT_BOOKING.LBL_CONFIGURE') +
              " (" + response.priceCategoryName + ") " +
              this.utility.textTranslate('EDIT_BOOKING.LBL_NOT_SAME_AS_SELECTED') +
              " (" + this.model.customerPriceCategoryName + ") " +
              this.utility.textTranslate('EDIT_BOOKING.LBL_CATEGORY');
          }
          else if (response.result == PriceCategoryResult.MultiplePriceCategory) {

            this.model.customerPriceCategoryName = this.assignPriceCategoryName();

            this.bookingPreviewData.priceCategoryMessage = this.utility.textTranslate('EDIT_BOOKING.LBL_BOOKING_MULTIPLE_PRICE_CATEOGRY') +
              " (" + response.priceCategoryName + ") ";
            //+"LBL_PRODUCTS":"products"this.utility.textTranslate('EDIT_BOOKING.LBL_PRODUCTS');
          }
          else if (response.result == PriceCategoryResult.SelectPriceCategory) {

            this.model.priceCategoryId = response.priceCategoryId;
            this.bookingPreviewData.priceCategoryMessage = null;
            this.model.customerPriceCategoryName = this.assignPriceCategoryName();
          }
          else {
            this.model.customerPriceCategoryName = null;
            this.bookingPreviewData.priceCategoryMessage = null;
          }
        },
        error => {
          this.setError(error);
        });
  }

  assignPriceCategoryName(): string {

    if (this.model.priceCategoryId && this.model.priceCategoryId > 0) {

      return this.bookingMasterData.customerPriceCategory
        .find(x => x.id == this.model.priceCategoryId) ?
        this.bookingMasterData.customerPriceCategory
          .find(x => x.id == this.model.priceCategoryId).name : ""
    }
    else {
      return null;
    }
  }

  // get custom sample size list
  getCustomSampleSizeList() {
    this.refService.getCustomSampleSizeList()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.bookingMasterData.customSampleTypeList = res.dataSourceList;
          }
          else {
            this.error = res.result;
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  calculateAQLQuantityOnCustomSampleType(item) {
    //check sample type is available
    var sampleType = item.poProductDetail.sampleType;
    //get the sample type percentage data if it matches the sampletype
    var percentData = CustomTypePercentList.find(x => x.id == sampleType);
    //get the sample type pieces data if it matches the sampletype
    var piecesData = CustomTypePiecesList.find(x => x.id == sampleType);
    //calculate the aql quantity by percentage value of booking qty
    if (percentData && percentData.value)
      item.poProductDetail.aqlQuantity = Math.ceil(item.poProductDetail.bookingQuantity
        * (percentData.value) / 100);
    //apply the defined sample size if it is pieces data
    else if (piecesData && piecesData.sampleSize)
      item.poProductDetail.aqlQuantity = piecesData.sampleSize;
    //if others then clear the aql quantity then user will input the data
    else if (sampleType == this.sampleType.Other)
      item.poProductDetail.aqlQuantity = null;

  }

  changeSampleType(event, item) {
    if (event) {
      item.poProductDetail.customSampleTypeName = event.sampleType;
      this.processCustomAQLQuanity(item);
    }
  }

  processCustomAQLQuanity(item) {
    if (item.poProductDetail.sampleType) {
      this.calculateAQLQuantityOnCustomSampleType(item);
      this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.CustomSampleTypeName);
      this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.CustomSampleSize);
    }
  }

  // while chnage - check anywhere aql is custom
  changeAql(event, poProductItem) {
    poProductItem.poProductDetail.aqlName = event.value;
    poProductItem.poProductDetail.sampleType = null;
    poProductItem.poProductDetail.aqlQuantity = null;

    this.updatePrimaryProductDataToChild(poProductItem, this._bookingProductFieldType.Aql);

    this.checkAqlCustomSelected();
  }

  checkAqlCustomSelected() {
    this.bookingMasterData.isCustomAqlSelected = false;
    this.bookingMasterData.bookingPOProductValidators.forEach(element => {
      if (element.poProductDetail.aql == this.aqlType.AQLCustom) {
        this.bookingMasterData.isCustomAqlSelected = true;
      }
    });
  }

  //open the contact page in a new tab if hyperlink is clicked
  AddContact(type) {
    if (!this.model.customerId) {
      this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_CUS_REQ');
    }

    else {
      if (!type) {
        this.showError("EDIT_BOOKING.TITLE", "EDIT_AUDIT.MSG_Data_Error");
      }

      else {
        let entity: string = this.utility.getEntityName();
        var path;

        if (type == UserType.Customer) {
          path = entity + "/" + Url.CustomerContact + this.model.customerId;
        }

        else if (type == UserType.Supplier) {
          path = entity + "/" + Url.SupplierEdit + this.model.supplierId;
        }

        else if (type == UserType.Factory) {
          path = entity + "/" + Url.SupplierEdit + this.model.factoryId;
        }

        window.open(path);
      }
    }

  }



  //event to handle the changing the display master product(display master checkbox event)
  changeDisplayMasterProduct(item) {
    //take the product list belongs to the pproduct id
    var productList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == item.poProductDetail.productId);
    //check if it is displaymaster
    if (item.poProductDetail.isDisplayMaster) {
      if (productList && productList.length > 0) {

        var isProductAvailable = this.bookingMasterData.displayMasterProducts.find(x => x.id == item.poProductDetail.productId);

        //add to the display master product datasource
        if (!isProductAvailable) {
          this.bookingMasterData.displayMasterProducts.push({ "id": item.poProductDetail.productId, "name": item.poProductDetail.productName });
          this.bookingMasterData.displayMasterProducts = [... this.bookingMasterData.displayMasterProducts];
        }
        //clear the mentioned values for the product
        productList.forEach(product => {
          product.poProductDetail.aql = null;
          product.poProductDetail.critical = null;
          product.poProductDetail.major = null;
          product.poProductDetail.minor = null;
          product.poProductDetail.parentProductId = null;
          product.poProductDetail.parentProductName = null;
          product.poProductDetail.isDisplayMaster = true;
        });

        this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.Aql);
        this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.Critical);
        this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.Major);
        this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.Minor);
      }
    }
    else if (!item.poProductDetail.isDisplayMaster) {
      //take the index if already in display master and it is removed
      var index = this.bookingMasterData.displayMasterProducts.findIndex(x => x.id == item.poProductDetail.productId);
      if (index >= 0)
        this.bookingMasterData.displayMasterProducts.splice(index, 1);

      //remove the display master configuration for the child products
      var mappedProducts = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == item.poProductDetail.productId);
      if (mappedProducts && mappedProducts.length > 0)
        mappedProducts.map(x => x.poProductDetail.isDisplayMaster = false);

      productList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.parentProductId == item.poProductDetail.productId);
      //remove the display master map with the child
      this.bookingMasterData.displayMasterProducts = [... this.bookingMasterData.displayMasterProducts];
      productList.forEach(product => {
        product.poProductDetail.parentProductId = null;
        product.poProductDetail.parentProductName = null;
        product.poProductDetail.isDisplayMaster = false;
      });

    }
  }

  //display child product change event
  changeDisplayChildProduct(item, event) {

    if (item && event) {

      item.poProductDetail.parentProductName = event.name;

      var productList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == item.poProductDetail.productId);
      //assign the parent product name
      productList.forEach(productData => {
        productData.poProductDetail.parentProductId = item.poProductDetail.parentProductId;
        productData.poProductDetail.parentProductName = item.poProductDetail.parentProductName;
      });

    }

  }

  changeEcoPack(item) {
    this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.IsEcoPack);
  }

  // process the display master products data in edit mode(add the display master products into dropdown)
  processDisplayMasterProducts() {
    let displayMasterProductIds: any = [];

    this.bookingMasterData.displayMasterProducts = [];

    //take the display master product ids
    displayMasterProductIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.isDisplayMaster).map(x => x.poProductDetail.productId);

    var distinctDisplayMasterProductIds = displayMasterProductIds.filter((n, i) => displayMasterProductIds.indexOf(n) === i);

    if (distinctDisplayMasterProductIds && distinctDisplayMasterProductIds.length > 0) {

      distinctDisplayMasterProductIds.forEach(productId => {

        var product = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productId);

        if (product)
          this.bookingMasterData.displayMasterProducts.push({ "id": product.poProductDetail.productId, "name": product.poProductDetail.productName });

      });

    }

  }

  //get applicant details for external user
  getPreviousBookingApplicantDetails() {
    this.service.getPreviousBookingApplicantDetails()
      .pipe()
      .subscribe(
        response => {
          if (response.result == ApplicantStaffResponseResult.Success) {
            this.model.applicantName = response.applicantInfo.staffName;
            this.model.applicantPhoneNo = response.applicantInfo.companyPhone;
            this.model.applicantEmail = response.applicantInfo.companyEmail;
          }
        },
        error => {
          this.setError(error);
        });
  }

  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
  }

  exportProducts() {
    this.exportProductLoading = true;
    if (this.model.id > 0) {
      this.service.exportProductSummary(this.model.id)
        .subscribe(res => {
          if (res)
            this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Booking_Products");

          else {
            this.showError('EDIT_BOOKING.TITLE', "EDIT_AUDIT.MSG_Data_Error");
          }
        },
          error => {
            this.exportProductLoading = false;
          });
    }
    else {
      this.showError('EDIT_BOOKING.TITLE', 'AUIDT_SUMMARY.MSG_BOOKINGNO_ERROR');
    }
  }

  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  //get the customer related details for edit booking
  getEditCustomerRelatedDetails(cusid) {
    this.bookingMasterData.customerBrandList = [];
    this.bookingMasterData.customerDepartmentList = [];
    this.bookingMasterData.customerBuyerList = [];
    this.bookingMasterData.customerMerchandiserList = [];
    this.bookingMasterData.seasonList = [];
    this.bookingMasterData.supplierList = [];
    this.bookingMasterData.supplierContactList = [];
    this.bookingMasterData.customerCollection = [];
    this.bookingMasterData.customerPriceCategory = [];
    this.bookingPreviewData.suppliercode = "";
    this.bookingMasterData.factoryContactList = "";
    this.bookingPreviewData.factcode = "";
    this.bookingMasterData.isCustomerBrandAvailable = false;
    this.bookingMasterData.isCustomerBuyerAvailable = false;
    this.bookingMasterData.isCustomerDeptAvailable = false;
    this.bookingMasterData.isCustomerMerchandiserAvailable = false;
    this.bookingMasterData.isCustomerCollectionAvailable = false;
    this.bookingMasterData.isCustomerPriceCategoryAvailable = false;
    this.spacebeforeCusContact = false;
    this.spacebeforeCusBookingNo = false;
    this.bookingMasterData.cusloading = true;
    this.getEditBookingCustomerDetails(cusid);
  }

  //get editbooking customer details
  getEditBookingCustomerDetails(customerId) {
    this.service.getEditBookingCustomerRelatedDetails(customerId, this.model.id)
      .pipe()
      .subscribe(
        response => {
          this.processEditBookingCustomerResponse(response);
        },
        error => {
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
          this.bookingMasterData.cusloading = false;
          this.bookingMasterData.supplierloading = false;
        }
      );
  }

  processEditBookingCustomerResponse(response) {
    var count = 0; //count to check if brand, buyer, dept and merchandiser are true. if any 3 are true, then add space on customer contact

    if (response && response.result == EditBookingCustomerResult.success) {

      //customer brand list
      if (response.customerBrandList) {
        this.bookingMasterData.customerBrandList = response.customerBrandList;
        this.bookingMasterData.isCustomerBrandAvailable = true;
        count++;
      }
      //customer dept list
      if (response.customerDepartmentList) {
        this.bookingMasterData.customerDepartmentList = response.customerDepartmentList;
        this.bookingMasterData.isCustomerDeptAvailable = true;
        count++;
      }
      //customer buyer list
      if (response.customerBuyerList) {
        this.bookingMasterData.customerBuyerList = response.customerBuyerList;
        this.bookingMasterData.isCustomerBuyerAvailable = true;
        count++;
      }
      //customer merchandiser list
      if (response.customerMerchandiserList) {
        this.bookingMasterData.customerMerchandiserList = response.customerMerchandiserList;
        this.bookingMasterData.isCustomerMerchandiserAvailable = true;
        count++;
      }

      //to provide margin on top only if it moves to next line.
      if (count >= 3)
        this.spacebeforeCusContact = true;
      if (count >= 2)
        this.spacebeforeCusBookingNo = true;

      //customer collection list
      if (response.collection) {
        this.bookingMasterData.customerCollection = response.collection;
        this.bookingMasterData.isCustomerCollectionAvailable = true;
      }

      //customer price category list
      if (response.priceCategory) {
        this.bookingMasterData.customerPriceCategory = response.priceCategory;
        this.bookingMasterData.isCustomerPriceCategoryAvailable = true;
      }
      //supplier list
      if (response.supplierList)
        this.bookingMasterData.supplierList = response.supplierList;

      this.getCustomerContactByBrandOrDept();
      this.GetCustomerDynamicFields(this.model.customerId, this._dynamicControlModuleType.InspectionBooking);
      this.AssignCustomerRelatedItems();

    }
    else if (response.result == EditBookingCustomerResult.cannotGetDetails) {
      this.ErrorDetails(response);
    }
    this.bookingMasterData.cusloading = false;
    this.bookingMasterData.supplierloading = false;
    this.SetPanelItem();
  }

  //get inspection hold reason types
  getInspectionHoldReasonTypes() {
    this.service.getInspectionHoldReasonTypes()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.prcoessInspectionHoldReasonTypes(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_HOLD_REASON_TYPE_NOT_FOUND');
          this.bookingMasterData.cuscontactloading = false;
        });
  }
  //process the booking mapped units
  prcoessInspectionHoldReasonTypes(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionHoldReasonTypes = response.dataSourceList;
    }
  }

  //get the inspection location list
  getInspectionLocations() {
    this.bookingMasterData.inspectionLocationLoading = true;
    this.refService.getInspectionLocationList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionLocation(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.inspectionLocationLoading = false;
        }
      );
  }

  //process the inspection locations
  processInspectionLocation(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.InspectionLocationList = response.dataSourceList;
      this.model.inspectionLocation = InspectionLocationEnum.Factory;
      this.bookingMasterData.inspectionLocationLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.inspectionLocationLoading = false;
    }
  }

  //inspection shipment types
  getInspectionShipmentTypes() {
    this.bookingMasterData.shipmentTypeLoading = true;
    this.refService.getInspectionShipmentTypes()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionShipmentTypes(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.shipmentTypeLoading = false;
        }
      );
  }

  //process inspection shipment type response
  processInspectionShipmentTypes(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionShipmentTypes = response.dataSourceList;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.shipmentTypeLoading = false;
    }
  }

  //get the business line
  getBusinessLines() {
    this.bookingMasterData.businessLineLoading = true;
    this.refService.getBusinessLines()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processBusinessLine(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_BUSINESSL_LINE_NOT_FOUND');
          this.bookingMasterData.businessLineLoading = false;
        }
      );
  }

  processBusinessLine(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.businessLines = response.dataSourceList;



      this.bookingMasterData.businessLineLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_BUSINESSL_LINE_NOT_FOUND');
      this.bookingMasterData.businessLineLoading = false;
    }
  }

  //get the customer product categories
  getCustomerProductCategories() {
    this.bookingMasterData.customerProductCategoryLoading = true;
    this.customerService.getCustomerProductCategories(this.model.customerId)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processCustomerProductCategoryResponse(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.customerProductCategoryLoading = false;
        }
      );
  }

  //process the customer product category response
  processCustomerProductCategoryResponse(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.customerProductCategories = response.dataSourceList;
      this.bookingMasterData.customerProductCategoryLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.customerProductCategoryLoading = false;
    }
  }

  //get the season details based on the customer
  getCustomerSeasonConfig() {
    this.bookingMasterData.customerConfigLoading = true;
    this.bookingMasterData.customerSeasonConfigList = [];
    this.customerService.getCustomerSeasonConfig(this.model.customerId)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processCustomerSeasonResponse(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.customerConfigLoading = false;
        }
      );
  }

  //process the customer season response
  processCustomerSeasonResponse(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.customerSeasonConfigList = response.dataSourceList;
      this.bookingMasterData.customerConfigLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.customerConfigLoading = false;
    }
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    //serviceTypeRequest.customerId = this.model.customerId;
    serviceTypeRequest.serviceId = Service.Inspection;
    //commented as part of ALD-3877
    //serviceTypeRequest.businessLineId = this.model.businessLine;
    if (!this.isReInspection) {
      serviceTypeRequest.bookingId = this.model.id;
      serviceTypeRequest.isReInspectedService = false;
    }
    if (this.isReInspection || this.model.previousBookingNo)
      serviceTypeRequest.isReInspectedService = true;
    return serviceTypeRequest;
  }

  //get the customer service type list
  getServiceTypeList() {
    this.bookingMasterData.customerServiceList = [];
    this.bookingMasterData.customerConfigLoading = true;
    //generate the service type request with needed filters
    var serviceTypeRequest = this.generateServiceTypeRequest();

    this.refService.getServiceTypes(serviceTypeRequest)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processServiceTypeListResponse(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SERVICE_TYPE_NOT_FOUND');
          this.bookingMasterData.customerConfigLoading = false;
        }
      );
  }

  //process the service type response
  processServiceTypeListResponse(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.customerServiceList = response.serviceTypeList;
      this.bookingMasterData.customerConfigLoading = false;
      //take the service type and apply the name
      var serviceType = this.bookingMasterData.customerServiceList.find(x => x.id == this.model.serviceTypeId);
      if (serviceType)
        this.bookingPreviewData.servicetype = serviceType.name;
      //in edit case toggling showservicedateto
      if (this.model.id) {
        if (this.currentUser.usertype != this._userTypeId.InternalUser)
          this.bookingMasterData.showServiceDateTo = false;
        if (this.currentUser.usertype != this._userTypeId.InternalUser && serviceType && serviceType.showServiceDateTo) {
          this.bookingMasterData.showServiceDateTo = true;
          this.model.serviceDateTo = this.model.serviceDateFrom;
        }
      }

    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SERVICE_TYPE_NOT_FOUND');
    }
  }

  onChangeBusinessLines(event) {
    this.model.inspectionLocation = null;
    this.model.seasonId = null;
    this.model.seasonYearId = null;

    this.model.shipmentTypeIds = [];
    this.model.shipmentPort = null;
    this.model.shipmentDate = null;
    this.model.ean = null;
    this.model.cuProductCategory = null;
    //commented as part of ALD-3877
    //this.bookingMasterData.customerServiceList = [];

    //clear the as received date and tf received date for all the products
    this.bookingMasterData.bookingPOProductValidators.forEach(product => {
      product.poProductDetail.asReceivedDate = null;
      product.poProductDetail.tfReceivedDate = null;
    });
    //commented as part of ALD-3877
    //this.model.serviceTypeId = null;
    //this.getServiceTypeList();
    this.getCustomerSeasonConfig();
    this.getCustomerProductCategories();
    this.getInspectionLocations();
    this.getInspectionShipmentTypes();
  }

  //get the inspection location list(along with booking mapped inspection location)
  getEditBookingInspectionLocations() {
    this.bookingMasterData.inspectionLocationLoading = true;
    this.service.getEditBookingInspectionLocations(this.model.id)
      .subscribe(
        response => {
          this.processBookingInspectionLocations(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.inspectionLocationLoading = false;
        });
  }
  //process the booking inspection location
  processBookingInspectionLocations(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.InspectionLocationList = response.dataSourceList;
      this.bookingMasterData.InspectionLocationList = [...this.bookingMasterData.InspectionLocationList];
      this.bookingMasterData.inspectionLocationLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.inspectionLocationLoading = false;
    }
  }

  //get the inspection location list(along with booking mapped inspection location)
  getEditBookingShipmentTypes() {
    this.bookingMasterData.shipmentTypeLoading = true;
    this.service.getEditBookingShipmentTypes(this.model.id)
      .subscribe(
        response => {
          this.processBookingShipmentTypes(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.shipmentTypeLoading = false;
        });
  }
  //process the booking inspection location
  processBookingShipmentTypes(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionShipmentTypes = response.dataSourceList;
      this.bookingMasterData.inspectionShipmentTypes = [...this.bookingMasterData.inspectionShipmentTypes];
      this.bookingMasterData.shipmentTypeLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.shipmentTypeLoading = false;
    }
  }

  //get the inspection location list(along with booking mapped inspection location)
  getEditBookingCuProductCategory() {
    this.bookingMasterData.customerProductCategoryLoading = true;
    this.service.getEditBookingCuProductCategory(this.model.customerId, this.model.id)
      .subscribe(
        response => {
          this.processBookingCuProductCategory(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.customerProductCategoryLoading = false;
        });
  }
  //process the booking inspection location
  processBookingCuProductCategory(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.customerProductCategories = response.dataSourceList;
      this.bookingMasterData.customerProductCategories = [...this.bookingMasterData.customerProductCategories];
      this.bookingMasterData.customerProductCategoryLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.customerProductCategoryLoading = false;
    }
  }

  //get the inspection location list(along with booking mapped inspection location)
  getEditBookingSeason() {
    this.bookingMasterData.shipmentTypeLoading = true;
    this.bookingMasterData.customerSeasonConfigList = [];
    this.service.getEditBookingSeason(this.model.customerId, this.model.id)
      .subscribe(
        response => {
          this.processBookingSeason(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.shipmentTypeLoading = false;
        });
  }
  //process the booking inspection location
  processBookingSeason(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.customerSeasonConfigList = response.dataSourceList;
      this.bookingMasterData.customerSeasonConfigList = [...this.bookingMasterData.customerSeasonConfigList];
      this.bookingMasterData.shipmentTypeLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.shipmentTypeLoading = false;
    }
  }

  //get the inspection location list(along with booking mapped inspection location)
  getEditBookingBusinessLines() {
    this.bookingMasterData.businessLineLoading = true;
    this.service.getEditBookingBusinessLines(this.model.id)
      .subscribe(
        response => {
          this.processBookingBusinessLines(response);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_BUSINESSL_LINE_NOT_FOUND');
          this.bookingMasterData.shipmentTypeLoading = false;
        });
  }
  //process the booking inspection location
  processBookingBusinessLines(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.businessLines = response.dataSourceList;
      this.bookingMasterData.businessLines = [...this.bookingMasterData.businessLines];
      this.bookingMasterData.businessLineLoading = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_BUSINESSL_LINE_NOT_FOUND');
      this.bookingMasterData.businessLineLoading = false;
    }
  }


  //add to the booking product list
  addToBookingProducts() {
    var poProductDetail = new InspectionPOProductDetail();

    var unitData = this.bookingMasterData.bookingProductUnitList.find(x => x.id == this.bookingUnit.Pieces);
    if (unitData)
      poProductDetail.unit = unitData.id;

    this.model.inspectionProductList.push(poProductDetail);
    this.validator.isSubmitted = false;

    if (this.model.isPickingRequired)
      poProductDetail.togglePicking = false;

    this.applyAqlValueToPoProduct(this.bookingMasterData.bookingServiceConfig, poProductDetail);

    var item = { poProductDetail: poProductDetail, validator: Validator.getValidator(poProductDetail, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };

    this.bookingMasterData.editBookingPoLoading = false;
    this.bookingMasterData.editBookingPoProductLoading = false;
    this.bookingMasterData.poDataSourceList = [];
    this.bookingMasterData.poProductDataSourceList = [];

    this.getPoListBySearch(item);
    if (!(this.currentUser.usertype == this.userTypeEnum.Customer && this.bookingMasterData.poProductDependentFilter))
      this.getPoProductListBySearch(item);

    if (this.model.serviceTypeId > 0) {
      const serviceType = this.bookingMasterData.customerServiceList.find(x => x.id == this.model.serviceTypeId);
      if (serviceType && serviceType.is100Inspection) {
        const customAql = this.bookingMasterData.bookingProductAqlList.find(x => x.id == this.aqlType.AQLCustom);
        const sample100SampleType = this.bookingMasterData.customSampleTypeList.find(x => x.id == this.sampleType.Sample100);
        if (customAql && sample100SampleType) {
          this.bookingMasterData.isCustomAqlSelected = true;
          this.customAqlchange(customAql, sample100SampleType, item);
        }
        else {
          item.poProductDetail.previewAqlName = null;
          item.poProductDetail.previewSamplingSize = null;
        }
      }
    }
    this.bookingMasterData.bookingPOProductValidators.push(item);

  }

  applyAqlValueToPoProduct(serviceConfigData, poProductDetail) {

    if (serviceConfigData) {
      poProductDetail.aql = serviceConfigData.levelPick1;//Aql Value for Pick Type Single
      poProductDetail.critical = serviceConfigData.criticalPick1;
      poProductDetail.major = serviceConfigData.majorTolerancePick1;
      poProductDetail.minor = serviceConfigData.minorTolerancePick1;
    }
  }

  /*  ngAfterViewChecked() {
     this.scrollableProductTable.nativeElement.scrollTop = this.scrollableProductTable.nativeElement.scrollHeight;
   } */

  //copy the booking product data with new row (with product and po details)
  copyToBookingProducts(item) {
    var poProductDetail = new InspectionPOProductDetail();

    poProductDetail = { ...item };

    poProductDetail.id = 0;
    poProductDetail.poTransactionId = 0;
    poProductDetail.colorTransactionId = 0;

    //reset and copy the picking data
    if (this.model.isPickingRequired) {
      var pickingList = poProductDetail.saveInspectionPickingList;
      poProductDetail.saveInspectionPickingList = new Array<InspectionPickingDetails>();
      poProductDetail.inspectionPickingValidators = [];
      this.copyPickingData(pickingList, poProductDetail);
    }

    var poProductValidator = { poProductDetail: poProductDetail, validator: Validator.getValidator(poProductDetail, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };


    this.bookingMasterData.poDataSourceList = item.poList;

    this.bookingMasterData.poDataSourceList.push({ "id": item.poId, "name": item.poName });

    this.bookingMasterData.editBookingPoLoading = this.model.id ? true : false;

    this.bookingMasterData.editBookingPoProductLoading = this.model.id ? true : false;

    this.bookingMasterData.poProductDataSourceList = item.poProductList;

    this.getPoListBySearch(poProductValidator);


    this.updateAqlNameToPoProductItem(poProductDetail.aql, poProductValidator);
    this.updateCriticalNameToPoProductItem(poProductDetail.critical, poProductValidator);
    this.updateMajorNameToPoProductItem(poProductDetail.major, poProductValidator);
    this.updateMinorDataToPoProductItem(poProductDetail.minor, poProductValidator);

    this.bookingMasterData.bookingPOProductValidators.push(poProductValidator);

    this.bookingMasterData.poProductDataSourceList = [];

    this.CalculateTotalBookingQty();
    this.CalculateTotalPickingQuantity();

    this.setPrimaryProduct();

  }

  //copy the picking data to newly copying poproduct detail row
  copyPickingData(pickingList, poProductDetail) {

    pickingList.forEach(picking => {
      var pickingData = new InspectionPickingDetails();
      pickingData.labOrCustomerId = picking.labOrCustomerId;
      pickingData.labOrCustomerAddressId = picking.labOrCustomerAddressId;
      pickingData.labOrCustomerAddressList = picking.labOrCustomerAddressList;
      pickingData.labOrCustomerContactList = picking.labOrCustomerContactList;
      pickingData.labOrCustomerContactIds = picking.labOrCustomerContactIds;
      pickingData.pickingQuantity = picking.pickingQuantity;
      pickingData.remarks = picking.remarks;
      pickingData.labType = picking.labType;

      //add picking data with validator
      var pickingDetailValidator = { pickingData: pickingData, validator: Validator.getValidator(pickingData, "booking/edit-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };
      poProductDetail.inspectionPickingValidators.push(pickingDetailValidator);
      poProductDetail.saveInspectionPickingList.push(pickingData);
    });
  }

  //get the po list autosearch and defualt load
  getPoListBySearch(item) {
    item.poProductDetail.requestPoDataSourceModel = new PODataSourceRequest();
    //push the customerid to  customer id list
    if (this.model.supplierId && this.bookingMasterData.poProductDependentFilter) {
      item.poProductDetail.requestPoDataSourceModel.supplierId = this.model.supplierId;
    }
    if (this.model.customerId) {
      item.poProductDetail.requestPoDataSourceModel.customerId = this.model.customerId;
    }

    item.poProductDetail.poListInput = new BehaviorSubject<string>("");

    item.poProductDetail.poListInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poProductDetail.poLoading = true),
      switchMap(term => term
        ? this.puchaseOrderService.getPODataSource(term, item, this.bookingMasterData.editBookingPoLoading, this.bookingMasterData.poDataSourceList, this.bookingMasterData.bookingPOProductValidators, null)
        : this.puchaseOrderService.getPODataSource("", item, this.bookingMasterData.editBookingPoLoading, this.bookingMasterData.poDataSourceList, this.bookingMasterData.bookingPOProductValidators, null)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => item.poProductDetail.poLoading = false))
      ))
      .subscribe(data => {
        if (data)
          item.poProductDetail.poList = data;
        else if (!data) {
          item.poProductDetail.poList = [];
          var poProductIndex = this.bookingMasterData.bookingPOProductValidators.
            findIndex(x => x.poProductDetail.poId == item.poProductDetail.poId &&
              x.poProductDetail.productId == item.poProductDetail.productId);
          if (poProductIndex >= 0)
            this.bookingMasterData.selectedPoNotAvailableIndex = poProductIndex;
        }
        item.poProductDetail.poLoading = false;
      });
  }

  //virutal scroll po list option
  getPoListData(isDefaultLoad: boolean, item) {
    this.bookingMasterData.editBookingPoLoading = false;

    if (isDefaultLoad) {
      item.poProductDetail.requestPoDataSourceModel.searchText = item.poProductDetail.poListInput.getValue();
      item.poProductDetail.requestPoDataSourceModel.skip = item.poProductDetail.poList.length;
    }

    this.bookingMasterData.poLoading = true;
    this.puchaseOrderService.getPODataSource("", item, this.bookingMasterData.editBookingPoLoading, null, this.bookingMasterData.bookingPOProductValidators, item.poProductDetail.poList).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(poData => {

        if (poData && poData.length > 0) {
          item.poProductDetail.poList = item.poProductDetail.poList.concat(poData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poProductDetail.requestPoDataSourceModel.skip = 0;
          item.poProductDetail.requestPoDataSourceModel.take = ListSize;
        }
        item.poProductDetail.poLoading = false;
      }),
      error => {
        item.poProductDetail.poLoading = false;
        this.setError(error);
      };
  }


  //get the po list autosearch and defualt load
  getInitialSearchPoList() {
    this.bookingMasterData.requestPoDataSourceModel = new PODataSourceRequest();

    //push the customerid to  customer id list
    if (this.model.supplierId && this.bookingMasterData.poProductDependentFilter) {
      this.bookingMasterData.requestPoDataSourceModel.supplierId = this.model.supplierId;
    }

    if (this.model.customerId) {
      this.bookingMasterData.requestPoDataSourceModel.customerId = this.model.customerId;
    }

    this.bookingMasterData.poListInput = new BehaviorSubject<string>("");

    this.bookingMasterData.poListInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.bookingMasterData.poLoading = true),
      switchMap(term => term
        ? this.puchaseOrderService.getSearchPOList(term, this.bookingMasterData.requestPoDataSourceModel)
        : this.puchaseOrderService.getSearchPOList("", this.bookingMasterData.requestPoDataSourceModel)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.poLoading = false))
      ))
      .subscribe(data => {
        this.bookingMasterData.searchPoList = data;
        this.bookingMasterData.poLoading = false;
      });
  }

  //virutal scroll po list option
  getSearchPoListData(isDefaultLoad: boolean) {

    if (isDefaultLoad) {
      this.bookingMasterData.requestPoDataSourceModel.searchText = this.bookingMasterData.poListInput.getValue();
      this.bookingMasterData.requestPoDataSourceModel.skip = this.bookingMasterData.searchPoList.length;
    }

    if (this.model.supplierId && this.bookingMasterData.poProductDependentFilter) {
      this.bookingMasterData.requestPoDataSourceModel.supplierId = this.model.supplierId;
    }

    if (this.model.customerId) {
      this.bookingMasterData.requestPoDataSourceModel.customerId = this.model.customerId;
    }

    this.bookingMasterData.searchPoLoading = true;
    this.puchaseOrderService.getSearchPOList("", this.bookingMasterData.requestPoDataSourceModel).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(poData => {

        if (poData && poData.length > 0) {
          this.bookingMasterData.searchPoList = this.bookingMasterData.searchPoList.concat(poData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.bookingMasterData.requestPoDataSourceModel.skip = 0;
          this.bookingMasterData.requestPoDataSourceModel.take = ListSize;
        }
        this.bookingMasterData.searchPoLoading = false;
      }),
      error => {
        this.bookingMasterData.searchPoLoading = false;
        this.setError(error);
      };
  }

  changeSearchPOList(event) {

    if (event)
      this.bookingMasterData.searchPoName = event.name;

  }

  addToDeletePOList() {
    this.bookingMasterData.selectedPOList = [];
    this.bookingMasterData.bookingPOProductValidators.forEach(element => {
      if (!this.bookingMasterData.selectedPOList.find(x => x.id == element.poProductDetail.poId))
        this.bookingMasterData.selectedPOList.push({ "id": element.poProductDetail.poId, "name": element.poProductDetail.poName });
    });
  }

  addSearchPoProductDataToBooking() {
    //filter the po product rows with having either po or producid(considering only those are valid data)
    var filteredPoProductRows = this.bookingMasterData.bookingPOProductValidators.filter(function (el) {
      return el.poProductDetail.poId || el.poProductDetail.productId;
    });

    this.bookingMasterData.bookingPOProductValidators = filteredPoProductRows;


    this.bookingMasterData.poProductDataSourceList = [];

    var selectedProducts = this.bookingMasterData.selectedSearchPoDetails.filter(x => x.isPoSelected);

    if (selectedProducts && selectedProducts.length > 0) {
      selectedProducts.forEach(poProductData => {

        var poProductDetail = new InspectionPOProductDetail();
        this.applyAqlValueToPoProduct(this.bookingMasterData.bookingServiceConfig, poProductDetail);

        var unitData = this.bookingMasterData.bookingProductUnitList.find(x => x.id == this.bookingUnit.Pieces);
        if (unitData)
          poProductDetail.unit = unitData.id;

        var poProductDetailValidator = { poProductDetail: poProductDetail, validator: Validator.getValidator(poProductDetail, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };

        poProductDetailValidator.validator.isSubmitted = false;

        this.bookingMasterData.editBookingPoLoading = false;

        var poData = new commonDataSource();
        poData.id = poProductData.poId;
        poData.name = poProductData.poName;
        this.bookingMasterData.poDataSourceList = [];
        this.bookingMasterData.poDataSourceList.push(poData);

        poProductDetailValidator.poProductDetail.poId = poProductData.poId;

        poProductDetailValidator.poProductDetail.poName = poProductData.poName;

        var productData = new POProductList();
        productData.id = poProductData.productId;
        productData.name = poProductData.productName;
        this.bookingMasterData.editBookingPoProductLoading = false;
        this.bookingMasterData.poProductDataSourceList.push(productData);

        this.getPoListBySearch(poProductDetailValidator);

        this.getPoProductListBySearch(poProductDetailValidator);

        poProductDetailValidator.poProductDetail.poQuantity = poProductData.poQty;

        poProductDetailValidator.poProductDetail.bookingQuantity = poProductData.poQty;

        poProductDetailValidator.poProductDetail.etd = poProductData.etd;

        poProductDetailValidator.poProductDetail.destinationCountryID = poProductData.destinationCountryId;

        poProductDetailValidator.poProductDetail.destinationCountryName = poProductData.destinationCountryName;

        this.assignSearchPoProductRelatedData(poProductDetailValidator, poProductData);


        this.bookingMasterData.bookingPOProductValidators.push(poProductDetailValidator);

        this.setPrimaryProduct();
        this.setPrimaryProductData(poProductDetailValidator);

        this.CalculateTotalPurchaseOrder();
        this.CalculateTotalProducts();

        this.CalculateTotalBookingQty();

        this.addToDeletePOList();


      });

      this.setPrimaryProduct();
    }
    else
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SEARCH_PO_PLS_SEL_PRD');



    this.modelRef.close();

  }

  assignSearchPoProductRelatedData(poProductDetailValidator, productDetail: selectedSearchPoDetail) {


    poProductDetailValidator.poProductDetail.productId = productDetail.productId;
    poProductDetailValidator.poProductDetail.productName = productDetail.productName;
    poProductDetailValidator.poProductDetail.productDesc = productDetail.productDesc;



    var categoryData = new commonDataSource();

    //asign the product category
    if (productDetail.productCategoryId) {
      poProductDetailValidator.poProductDetail.productCategoryName = productDetail.productCategoryName;
      poProductDetailValidator.poProductDetail.productCategoryMapped = true;
    }
    //asign the product sub category
    if (productDetail.productSubCategoryId) {
      var productSubCategoryList = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategoryId;
      categoryData.name = productDetail.productSubCategoryName;
      productSubCategoryList.push(categoryData);
      poProductDetailValidator.poProductDetail.productSubCategoryName = productDetail.productSubCategoryName;
      poProductDetailValidator.poProductDetail.bookingCategorySubProductList = productSubCategoryList;
      poProductDetailValidator.poProductDetail.productSubCategoryMapped = true;
    }
    else if (!productDetail.productSubCategoryId && productDetail.productSubCategoryList)
      poProductDetailValidator.poProductDetail.bookingCategorySubProductList = productDetail.productSubCategoryList;

    //asign the product categorysub2
    if (productDetail.productSubCategory2Id) {
      var productSubCategory2List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategory2Id;
      categoryData.name = productDetail.productSubCategory2Name;
      productSubCategory2List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub2Name = productDetail.productSubCategory2Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub2ProductList = productSubCategory2List;
      poProductDetailValidator.poProductDetail.productCategorySub2Mapped = true;
    }
    else if (!productDetail.productSubCategory2Id && productDetail.productSubCategory2List)
      poProductDetailValidator.poProductDetail.bookingCategorySub2ProductList = productDetail.productSubCategory2List;

    //asign the product categorysub2
    if (productDetail.productSubCategory3Id) {
      var productSubCategory3List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategory3Id;
      categoryData.name = productDetail.productSubCategory3Name;
      productSubCategory3List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub3Name = productDetail.productSubCategory3Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub3ProductList = productSubCategory3List;
      poProductDetailValidator.poProductDetail.productCategorySub3Mapped = true;
    }
    else if (!productDetail.productSubCategory3Id && productDetail.productSubCategory3List)
      poProductDetailValidator.poProductDetail.bookingCategorySub3ProductList = productDetail.productSubCategory3List;


    poProductDetailValidator.poProductDetail.productCategoryId = productDetail.productCategoryId;
    poProductDetailValidator.poProductDetail.productSubCategoryId = productDetail.productSubCategoryId;
    poProductDetailValidator.poProductDetail.productCategorySub2Id = productDetail.productSubCategory2Id;
    poProductDetailValidator.poProductDetail.productCategorySub3Id = productDetail.productSubCategory3Id;

    poProductDetailValidator.poProductDetail.barcode = productDetail.barCode;
    poProductDetailValidator.poProductDetail.factoryReference = productDetail.factoryReference;
    poProductDetailValidator.poProductDetail.remarks = productDetail.remarks;

  }

  getProductListBySearchPO(poId, content) {
    var poProductDataRequest = new POProductDataRequest();
    poProductDataRequest.poId = poId;

    if (this.model.supplierId)
      poProductDataRequest.supplierId = this.model.supplierId;

    this.puchaseOrderService.getSearchPOProductList(poProductDataRequest)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.processSearchPoProductSuccessResponse(response, content);
          }
          else if (response && response.result == DataSourceResult.NotFound) {
            this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SEARCH_PO_ADD_PRD_TO_BOOKING');
          }
          this.searchpoloader = false;
        },
        error => {
          this.searchpoloader = false;
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        }
      );
  }

  processSearchPoProductSuccessResponse(response, content) {
    if (response.productList) {
      this.bookingMasterData.selectedSearchPoDetails = [];
      response.productList.forEach(product => {

        var poProductExists = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.poId == product.poId && x.poProductDetail.productId == product.id);

        if (!poProductExists) {
          var poProductDetail = new selectedSearchPoDetail();

          poProductDetail.productId = product.id;
          poProductDetail.productName = product.name;
          poProductDetail.productDesc = product.description;
          poProductDetail.productCategoryId = product.productCategoryId;
          poProductDetail.productCategoryName = product.productCategoryName;
          poProductDetail.productSubCategoryId = product.productSubCategoryId;
          poProductDetail.productSubCategoryName = product.productSubCategoryName;
          poProductDetail.productSubCategory2Id = product.productSubCategory2Id;
          poProductDetail.productSubCategory2Name = product.productSubCategory2Name;

          poProductDetail.productSubCategory3Id = product.productSubCategory3Id;
          poProductDetail.productSubCategory3Name = product.productSubCategory3Name;

          poProductDetail.barCode = product.barCode;
          poProductDetail.factoryReference = product.factoryReference;
          poProductDetail.remarks = product.remarks;
          poProductDetail.productSubCategory2Name = product.productSubCategory2Name;
          poProductDetail.isNewProduct = product.isNewProduct;

          poProductDetail.poId = product.poId;
          poProductDetail.poName = product.poName;
          poProductDetail.poQty = product.poQuantity;
          poProductDetail.etd = product.etd;
          poProductDetail.destinationCountryId = product.destinationCountryId;
          poProductDetail.destinationCountryName = product.destinationCountryName;

          poProductDetail.productSubCategoryList = product.productSubCategoryList;
          poProductDetail.productSubCategory2List = product.productSubCategory2List;
          poProductDetail.productSubCategory3List = product.productSubCategory3List;

          this.bookingMasterData.selectedSearchPoDetails.push(poProductDetail);
        }

      });

      if (this.bookingMasterData.selectedSearchPoDetails && this.bookingMasterData.selectedSearchPoDetails.length > 0)
        this.modelRef = this.modalService.open(content, { windowClass: "add-booking-product-wrapper", centered: true, backdrop: 'static' });
      else
        this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SEARCH_PO_ADD_PRD_TO_BOOKING');


    }
  }

  searchPOProducts(content) {
    if (this.bookingMasterData.searchPoId) {
      this.searchpoloader = true;
      this.isProductCheckedinPopUp = false;
      this.bookingMasterData.selectAllSearchPOChecked = false;
      this.bookingMasterData.selectedSearchPoDetails.forEach(poProductData => {
        poProductData.isPoSelected = false;
      });
      this.getProductListBySearchPO(this.bookingMasterData.searchPoId, content);
    }
    else
      this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SEARCH_PO_REQ');

  }

  selectAllSearchPoCheckBox() {

    //this.bookingMasterData.isDeleteAllPOChecked = false;

    this.bookingMasterData.selectAllSearchPOChecked = !this.bookingMasterData.selectAllSearchPOChecked;

    this.bookingMasterData.selectedSearchPoDetails.forEach(element => {
      element.isPoSelected = this.bookingMasterData.selectAllSearchPOChecked ?
        true : false;
    });
    this.isProductCheckedinPopUp = this.bookingMasterData.selectedSearchPoDetails.some(x => x.isPoSelected);
    /* if (this.bookingMasterData.selectAllDeletePOChecked)
      this.bookingMasterData.isDeleteAllPOChecked = true;


    var poLength = this.bookingMasterData.selectedDeletePoDetails.filter(element => element.isPoSelected).length;
    this.productValidateMaster.totalProducts = poLength;
    this.productValidateMaster.deleteSuccessCount = 0;
    this.productValidateMaster.validationFailedCount = 0; */

  }

  checkAllSearchPoSelected() {
    var poActiveLength = this.bookingMasterData.selectedSearchPoDetails.filter(element => element.isPoSelected).length;

    var poLength = this.bookingMasterData.selectedSearchPoDetails.length;
    if (poLength == poActiveLength)
      this.bookingMasterData.selectAllSearchPOChecked = true;
    else
      this.bookingMasterData.selectAllSearchPOChecked = false;

    this.isProductCheckedinPopUp = this.bookingMasterData.selectedSearchPoDetails.some(x => x.isPoSelected);
  }

  addSearchPoProducts(content) {
    this.modelRef.close();
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  async onProductModelChange(productId, item) {
    if (item && productId) {

      this.clearProductRelatedData(item);

      var productList = item.poProductDetail.productList;
      item.poProductDetail.id = 0;
      item.poProductDetail.poTransactionId = 0;
      item.poProductDetail.colorTransactionId = 0;

      var primaryProduct = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productId && x.poProductDetail.isPrimaryProductInGroup);

      var product = await this.getPoProductDetails(item.poProductDetail.poId, productId, item);

      if (product) {
        item.poProductDetail.poQuantity = product.poQuantity;
        item.poProductDetail.bookingQuantity = product.poQuantity;
        item.poProductDetail.productImageCount = product.productImageCount;
        if (product.destinationCountryId)
          item.poProductDetail.destinationCountryID = product.destinationCountryId;
        else
          item.poProductDetail.destinationCountryID = null;
        if (product.etd)
          item.poProductDetail.etd = product.etd;
        else
          item.poProductDetail.etd = null;
        this.bookingMasterData.poProductDataSourceList.push({ "id": product.id, "name": product.name });
        this.getPoProductListBySearch(item);
      }

      if (primaryProduct)
        this.assignPrimaryProductDataToProductItem(primaryProduct, item);
      else if (product) {

        this.assignPoProductData(item, product, productId);

      }

      this.CalculateTotalProducts();

      this.CalculateTotalBookingQty();

      this.setPrimaryProduct();

      this.setPrimaryProductData(item);

    }
  }

  getAsDateFromString(dateString) {
    var dateParts = dateString.split("/");
    var dateObject = new Date(+dateParts[2], dateParts[1], +dateParts[0]);
    return dateObject
  }

  assignPoProductData(item, product, productId) {
    item.poProductDetail.productDesc = product.description;
    item.poProductDetail.factoryReference = product.factoryReference;
    item.poProductDetail.productCategoryId = product.productCategoryId;
    item.poProductDetail.productSubCategoryId = product.productSubCategoryId;
    item.poProductDetail.productCategorySub2Id = product.productSubCategory2Id;
    item.poProductDetail.productCategorySub3Id = product.productSubCategory3Id;
    item.poProductDetail.productCategoryName = product.productCategoryName;
    item.poProductDetail.productSubCategoryName = product.productSubCategoryName;
    item.poProductDetail.productCategorySub2Name = product.productSubCategory2Name;
    item.poProductDetail.productCategorySub3Name = product.productSubCategory3Name;
    item.poProductDetail.barcode = product.barCode;
    item.poProductDetail.productName = product.name;
    item.poProductDetail.isNewProduct = product.isNewProduct;
    item.poProductDetail.remarks = product.remarks;
    item.poProductDetail.productId = productId;

    var subCategoryList = new Array<commonDataSource>();

    if (product.productCategoryId)
      item.poProductDetail.productCategoryMapped = true;

    if (product.productSubCategoryId) {
      var subCategory = new commonDataSource();
      subCategory.id = product.productSubCategoryId;
      subCategory.name = product.productSubCategoryName;
      subCategoryList.push(subCategory);
      item.poProductDetail.bookingCategorySubProductList = subCategoryList;
      item.poProductDetail.productSubCategoryMapped = true;
    }


    if (product.productSubCategory2Id) {
      var subCategory2List = new Array<commonDataSource>();
      var subCategory2 = new commonDataSource();
      subCategory2.id = product.productSubCategory2Id;
      subCategory2.name = product.productSubCategory2Name;
      subCategory2List.push(subCategory2);
      item.poProductDetail.bookingCategorySub2ProductList = subCategory2List;
      item.poProductDetail.productCategorySub2Mapped = true;
    }

    if (product.productSubCategory3Id) {
      var subCategory3List = new Array<commonDataSource>();
      var subCategory3 = new commonDataSource();
      subCategory3.id = product.productSubCategory3Id;
      subCategory3.name = product.productSubCategory3Name;
      subCategory3List.push(subCategory3);
      item.poProductDetail.bookingCategorySub3ProductList = subCategory3List;
      item.poProductDetail.productCategorySub3Mapped = true;
    }
  }

  changeProductData() {
    this.setPrimaryProduct();
    this.setSampleSizeValue();
  }

  clearFBTemplateList(item) {
    this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.FBTemplate);
  }

  clearDisplayChild(item) {
    this.updateEmtpyValueToChildProducts(item, this._bookingProductFieldType.DisplayChild);
  }

  //clera event for the product in booking product list
  clearProduct(item) {
    if (item) {
      this.clearProductRelatedData(item);
      item.poProductDetail.poProductList = [];
      this.bookingMasterData.poProductDataSourceList = [];
      this.bookingMasterData.editBookingPoProductLoading = false;
      this.getPoProductListBySearch(item);

    }
    this.setPrimaryProduct();
    //this.bookingMasterData.bookingPOProductValidators = this.bookingMasterData.bookingPOProductValidators.sort((a, b) => (a.poProductDetail.productName > b.poProductDetail.productName ? -1 : 1));
  }

  clearProductRelatedData(item) {
    item.poProductDetail.productDesc = null;
    item.poProductDetail.factoryReference = null;
    item.poProductDetail.productCategoryId = null;
    item.poProductDetail.productSubCategoryId = null;
    item.poProductDetail.productCategorySub2Id = null;
    item.poProductDetail.barcode = null;
    item.poProductDetail.bookingCategorySubProductList = [];

    item.poProductDetail.bookingCategorySub2ProductList = [];
    item.poProductDetail.bookingCategorySub3ProductList = [];

    item.poProductDetail.productCategoryMapped = false;
    item.poProductDetail.productSubCategoryMapped = false;
    item.poProductDetail.productCategorySub2Mapped = false;
    item.poProductDetail.productCategorySub3Mapped = false;
    item.poProductDetail.productCategorySub3Id = null;


    if (item.poProductDetail.unit != this._unit.Pcs)
      item.poProductDetail.unit = null;

    item.poProductDetail.unitCount = null;
    item.poProductDetail.fbTemplateId = null;
    item.poProductDetail.poQuantity = null;
    item.poProductDetail.bookingQuantity = null;
    item.poProductDetail.remarks = null;

    item.poProductDetail.etd = null;
    item.poProductDetail.destinationCountryID = null;
    item.poProductDetail.aqlQuantity = null;
  }

  //change event for the pono in booking product list
  changePoNumber(item, currentPoData) {
    if (item && item.poProductDetail.poId) {

      this.bookingMasterData.editBookingPoLoading = false;

      item.poProductDetail.productId = null;
      this.bookingMasterData.editBookingPoProductLoading = false;
      this.bookingMasterData.poProductList = [];
      this.bookingMasterData.poDataSourceList = [];
      this.bookingMasterData.poProductDataSourceList = [];

      item.poProductDetail.poName = currentPoData.name;
      item.poProductDetail.etd = null;
      item.poProductDetail.destinationCountryID = null;

      if (!this.bookingMasterData.selectedPOList.find(x => x.id == currentPoData.id))
        this.bookingMasterData.selectedPOList.push({ "id": currentPoData.id, "name": currentPoData.name });

      this.bookingMasterData.bookingProductLoading = true;

      this.getPoProductListBySearch(item);

      this.CalculateTotalPurchaseOrder();

      //this.clearProductRelatedData(item);

      this.setPrimaryProduct();
    }
  }

  //change event for the po no in booking product list
  clearPoNumber(item) {
    item.poProductDetail.poList = [];
    item.poProductDetail.poProductList = [];
    item.poProductDetail.productId = null;
    item.poProductDetail.etd = null;
    item.poProductDetail.destinationCountryID = null;
    this.bookingMasterData.editBookingPoLoading = false;
    this.bookingMasterData.poDataSourceList = [];
    this.clearProductRelatedData(item);
    this.setPrimaryProduct();
    this.getPoListBySearch(item);

  }

  /* applyPoProductFiler(item) {

    //get product by po and supplier if dependent filter applied
    if (this.bookingMasterData.poProductDependentFilter) {
      if (item.poProductDetail.poId)
        item.poProductDetail.requestPoProductDataSourceModel.poId = item.poProductDetail.poId;
      if (this.model.supplierId)
        item.poProductDetail.requestPoProductDataSourceModel.supplierId = this.model.supplierId;
    }
    //if not apply only by customer
    else if (!this.bookingMasterData.poProductDependentFilter) {
      if (this.model.customerId)
        item.poProductDetail.requestPoProductDataSourceModel.customerIds.push(this.model.customerId);
    }
    //push the customerid to  customer id list

    if (this.bookingMasterData.poProductDependentFilter)
      item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByPo;
    else if (!this.bookingMasterData.poProductDependentFilter)
      item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByCustomer;
  } */

  applyPoProductFiler(item) {

    //apply customer id filter by default
    if (this.model.customerId)
      item.poProductDetail.requestPoProductDataSourceModel.customerIds.push(this.model.customerId);

    //if it is internal user load the products by customer
    if (this.currentUser.usertype == this.userTypeEnum.InternalUser)
      item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByCustomer;

    //if it is customer and poProductDependentFilter applied then fetch the product by supplier and customer
    //if not fetch the product from the customer product list
    else if (this.currentUser.usertype == this.userTypeEnum.Customer) {
      //get product by po and supplier if dependent filter applied
      if (this.bookingMasterData.poProductDependentFilter) {
        //temporarily commenting this piece of code since it is not required by customer
        /* if (item.poProductDetail.poId)
          item.poProductDetail.requestPoProductDataSourceModel.poId = item.poProductDetail.poId; */
        if (this.model.supplierId)
          item.poProductDetail.requestPoProductDataSourceModel.supplierId = this.model.supplierId;
        item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByPo;
      }
      else if (!this.bookingMasterData.poProductDependentFilter)
        item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByCustomer;
    }

    //if it is supplier or factory then fetch the products from purchase order details with customer and supplier filter
    else if (this.currentUser.usertype == this.userTypeEnum.Supplier || this.currentUser.usertype == this.userTypeEnum.Factory) {
      if (this.model.supplierId)
        item.poProductDetail.requestPoProductDataSourceModel.supplierId = this.model.supplierId;
      item.poProductDetail.requestPoProductDataSourceModel.filterPoProduct = FilterPoProductEnum.ProductByPo;
    }
  }

  //get the po product list autosearch and defualt load
  getPoProductListBySearch(item) {
    item.poProductDetail.requestPoProductDataSourceModel = new POProductDataSourceRequest();
    this.applyPoProductFiler(item);
    item.poProductDetail.poProductListInput = new BehaviorSubject<string>("");
    item.poProductDetail.poProductListInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poProductDetail.poProductLoading = true),
      switchMap(term => term
        ? this.puchaseOrderService.getPOProductListByName(item, term, this.bookingMasterData.editBookingPoProductLoading, this.bookingMasterData.poProductDataSourceList)
        : this.puchaseOrderService.getPOProductListByName(item, "", this.bookingMasterData.editBookingPoProductLoading, this.bookingMasterData.poProductDataSourceList)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => item.poProductDetail.poProductLoading = false))
      ))
      .subscribe(data => {
        item.poProductDetail.poProductList = data;
        item.poProductDetail.poProductLoading = false;
        if (this.bookingMasterData.bookingProductLoading && !item.poProductDetail.poProductList) {
          this.showWarning('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_SEARCH_PO_ADD_PRD_TO_BOOKING');
        }

      });
  }

  //get the po product list with virtual scroll
  getPoProductListData(item, isDefaultLoad: boolean) {
    this.bookingMasterData.editBookingPoProductLoading = false;
    item.poProductDetail.requestPoProductDataSourceModel = new POProductDataSourceRequest();
    if (this.bookingMasterData.poProductDependentFilter) {
      item.poProductDetail.requestPoProductDataSourceModel.poId = item.poProductDetail.poId;
      if (this.model.supplierId)
        item.poProductDetail.requestPoProductDataSourceModel.supplierId = this.model.supplierId;
    }
    if (this.model.customerId)
      item.poProductDetail.requestPoProductDataSourceModel.customerIds.push(this.model.customerId);

    if (isDefaultLoad) {
      item.poProductDetail.requestPoProductDataSourceModel.searchText = item.poProductDetail.poProductListInput.getValue();
      item.poProductDetail.requestPoProductDataSourceModel.skip = item.poProductDetail.poProductList.length;
    }

    item.poProductDetail.poProductLoading = true;
    this.puchaseOrderService.getPOProductListByName(item, "", this.bookingMasterData.editBookingPoProductLoading, null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(customerData => {

        if (customerData && customerData.length > 0) {
          item.poProductDetail.poProductList = item.poProductDetail.poProductList.concat(customerData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poProductDetail.requestPoProductDataSourceModel.skip = 0;
          item.poProductDetail.requestPoProductDataSourceModel.take = ListSize;
        }
        item.poProductDetail.poProductLoading = false;
      }),
      error => {
        item.poProductDetail.poProductLoading = false;
        this.setError(error);
      };
  }

  //assign the fb template data for the selected product in the product list
  assignFBTemplate(item, fbTemplate) {

    item.poProductDetail.fbTemplateName = fbTemplate.name;

    this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.FBTemplate);

  }

  changeDestinationCountry(destinationCountry, item) {
    item.destinationCountryName = destinationCountry.countryName;
  }

  async removeBookingPoProducts(poProductIndex, content) {
    var productTransaction = this.bookingMasterData.bookingPOProductValidators[poProductIndex];
    if (productTransaction) {

      this.productValidateMaster.poValid = true;
      this.productValidateMaster.poRemarks = '';

      await this.getPOValidationRemarks(productTransaction.poTransactionId);

      if (!this.isReInspection && !this.isReBooking && !this.productValidateMaster.poValid) {
        this.showError('EDIT_BOOKING.TITLE', this.productValidateMaster.poRemarks);
      }

      else {
        if (productTransaction.poProductDetail.id) {

          this.bookingMasterData.deletePoProductIndex = null;
          this.bookingMasterData.deletePoProductTransaction = null;

          this.bookingMasterData.deletePoProductIndex = poProductIndex;
          this.bookingMasterData.deletePoProductTransaction = productTransaction;
          this.bookingMasterData.deletePoProductName = productTransaction.poProductDetail.productName;

          this.modelRef = this.modalService.open(content, { centered: true, backdrop: 'static' });
        }
        else
          this.bookingMasterData.bookingPOProductValidators.splice(poProductIndex, 1);

        if (this.bookingMasterData.bookingPOProductValidators.length == 0)
          this.addToBookingProducts();

        this.updateCustomSampleSizeForPercentageSampleType(productTransaction.poProductDetail.productId);

        var selectedPOIndex = this.bookingMasterData.selectedPOList.findIndex(x => x.id == productTransaction.poProductDetail.poId);
        this.bookingMasterData.selectedPOList.splice(selectedPOIndex, 1);

        //take the primary product for the given product in the group
        var productData = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productTransaction.poProductDetail.productId && x.poProductDetail.isPrimaryProductInGroup);

        if (productData.poProductDetail.sampleType) {
          this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleTypeName);
          this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleSize);
        }

        var selectedPOIndex = this.bookingMasterData.selectedPOList.findIndex(x => x.id == productTransaction.poProductDetail.poId);
        this.bookingMasterData.selectedPOList.splice(selectedPOIndex, 1);

        this.CalculateTotalBookingQty();
        this.CalculateTotalPickingQuantity();
        this.CalculateTotalProducts();
        this.CalculateTotalPurchaseOrder();

      }
    }
  }

  updateCustomSampleSizeForPercentageSampleType(productId) {
    this.setPrimaryProduct();

    //take the primary product for the given product in the group
    var productData = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productId && x.poProductDetail.isPrimaryProductInGroup);

    if (productData && productData.poProductDetail && productData.poProductDetail.sampleType) {
      this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleTypeName);
      this.updatePrimaryProductDataToChild(productData, this._bookingProductFieldType.CustomSampleSize);
    }
  }

  removePoProductData() {
    this.selectedAllProducts = false;

    let poProductIndex = this.bookingMasterData.deletePoProductIndex;

    var productTransaction = this.bookingMasterData.bookingPOProductValidators[poProductIndex];

    this.bookingMasterData.bookingPOProductValidators.splice(poProductIndex, 1);

    var productExistsinBooking = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.productId == productTransaction.poProductDetail.productId);

    if (!productExistsinBooking) {

      var productFiles = this.bookingMasterData.bookingProductFiles.filter(x => x.productId != productTransaction.poProductDetail.productId);

      this.bookingMasterData.bookingProductFiles = [...productFiles];

    }

    var poExistsinBooking = this.bookingMasterData.bookingPOProductValidators.find(x => x.poProductDetail.poId == productTransaction.poProductDetail.poId);

    if (!poExistsinBooking) {
      var selectedPOIndex = this.bookingMasterData.selectedPOList.findIndex(x => x.id == productTransaction.poProductDetail.poId);
      this.bookingMasterData.selectedPOList.splice(selectedPOIndex, 1);
    }

    this.CalculateTotalBookingQty();
    this.CalculateTotalPickingQuantity();
    this.CalculateTotalProducts();
    this.CalculateTotalPurchaseOrder();

    if (this.bookingMasterData.bookingPOProductValidators.length == 0) {
      this.bookingMasterData.isCustomAqlSelected = false;
      this.addToBookingProducts();
    }
    this.setPrimaryProduct();

    this.updateCustomSampleSizeForPercentageSampleType(productTransaction.poProductDetail.productId);

    this.modelRef.close();
  }

  //get the poValidation remarks and po is valid
  async getPOValidationRemarks(poTransactionId) {


    var productValidateData = new Array<ProductValidateData>();

    //push the data to productvalidatedata from the existing po list
    if (poTransactionId) {
      var request = new ProductValidateData();
      request.bookingId = this.model.id;
      request.poTransactionId = poTransactionId;
      productValidateData.push(request);
    }

    if (productValidateData && productValidateData.length > 0)
      var result = await this.service.bookingProductsValidation(productValidateData);

    if (result && result.length > 0) {
      var validationResult = result[0];

      if (validationResult && (validationResult.reportExists || validationResult.pickingExists || validationResult.quotationExists))
        this.productValidateMaster.poValid = false;

      //assign the po validation remarks
      if (validationResult && validationResult.pickingExists && validationResult.quotationExists && validationResult.reportExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_INSPECTION_PICKING_QUOT_RPT_DATA');
      else if (validationResult && validationResult.pickingExists && validationResult.quotationExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_INSPECTION_PICKING_QUOT_DATA');
      else if (validationResult && validationResult.pickingExists && validationResult.reportExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_INSPECTION_PICKING_RPT_DATA');
      else if (validationResult && validationResult.quotationExists && validationResult.reportExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_INSPECTION_QUOT_RPT_DATA');
      else if (validationResult && validationResult.quotationExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('INSPECTION_CANCEL.MSG_INTERNAL_USER_QUOTATIONCANCEL');
      else if (validationResult && validationResult.pickingExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_INSPECTION_PICKING_DATA');
      else if (validationResult && validationResult.reportExists)
        this.productValidateMaster.poRemarks = this.utility.textTranslate('EDIT_BOOKING.MSG_REPORT_VALIDATION');
    }
  }

  //open the purchase order creation/updation popup
  openPurchaseOrderPopup(addPoContent, isEditPO, poProduct) {

    poProduct.validator.isSubmitted = false;
    this.validator.isSubmitted = false;
    this.purchaseModel.id = 0;
    this.purchaseModel.pono = "";
    this.purchaseModel.purchaseOrderDetails = [];
    this.poDetailValidators = [];

    this.bookingMasterData.isPoProductPopupVisible = true;

    if (isEditPO) {
      this.purchaseModel.pono = poProduct.poProductDetail.poName;
      this.purchaseModel.id = poProduct.poProductDetail.poId;
    }



    this.addPurchaseOrderDetail();

    this.modelRef = this.modalService.open(addPoContent,
      {
        windowClass: "add-booking-product-wrapper",
        centered: true,
        backdrop: 'static'
      });

  }

  async getPoProductList(productIds) {
    this.bookingMasterData.customerProductLoading = true;
    var poProductRequest = new PoProductRequest();
    poProductRequest.productIds = productIds;

    this.poPopupProductList = [];

    var response = await this.customerProductService.getCustomerProductByProductIds(poProductRequest);

    if (response.result == 1) {
      this.poPopupProductList = response.productList;
    }
  }

  // open hold booking popup
  openAddBookingProduct(content) {
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  //add the po and product data created in the po popup list to booking product list
  async addPurchaseOrderPopupDataToBooking() {

    //assign po data souce list from po model
    var poData = new commonDataSource();
    poData.id = this.purchaseModel.id;
    poData.name = this.purchaseModel.pono;
    this.bookingMasterData.poDataSourceList.push(poData);

    //filter the po product rows with having either po or producid(considering only those are valid data)
    var filteredPoProductRows = this.bookingMasterData.bookingPOProductValidators.filter(function (el) {
      return el.poProductDetail.poId && el.poProductDetail.productId;
    });

    this.bookingMasterData.bookingPOProductValidators = filteredPoProductRows;

    var unitData = this.bookingMasterData.bookingProductUnitList.find(x => x.id == this.bookingUnit.Pieces);

    var productIds = this.poDetailValidators.map(x => x.poDetails.productId);

    await this.getPoProductList(productIds);

    this.poDetailValidators.forEach(poDetail => {

      var poProductDetail = new InspectionPOProductDetail();

      if (unitData)
        poProductDetail.unit = unitData.id;

      this.applyAqlValueToPoProduct(this.bookingMasterData.bookingServiceConfig, poProductDetail);

      var poProductDetailValidator = { poProductDetail: poProductDetail, validator: Validator.getValidator(poProductDetail, "booking/edit-booking-po.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };

      poProductDetailValidator.validator.isSubmitted = false;

      this.bookingMasterData.editBookingPoLoading = false;

      this.getPoListBySearch(poProductDetailValidator);

      this.assignPoRelatedDataFromPoPopup(poProductDetailValidator, poDetail);

      this.bookingMasterData.editBookingPoProductLoading = false;

      //var productDetails = this.purchaseModel.purchaseOrderDetails.filter(x => x.poId == this.purchaseModel.id).map(x=>x.productList);

      var productDetail = this.poPopupProductList.find(x => x.id == poDetail.poDetails.productId);

      if (productDetail) {

        var productData = this.assignProductRelatedDataFromPoPopup(poProductDetailValidator, productDetail);

        this.bookingMasterData.poProductDataSourceList.push(productData);

        this.getPoProductListBySearch(poProductDetailValidator);
      }

      this.setPrimaryProductData(poProductDetailValidator);

      this.bookingMasterData.bookingPOProductValidators.push(poProductDetailValidator);

      this.CalculateTotalBookingQty();
      this.CalculateTotalPurchaseOrder();
      this.CalculateTotalProducts();

    });

    this.setPrimaryProduct();

    this.modelRef.close();

  }

  assignPoRelatedDataFromPoPopup(poProductDetailValidator, poDetail) {
    poProductDetailValidator.poProductDetail.poId = this.purchaseModel.id;

    poProductDetailValidator.poProductDetail.poName = this.purchaseModel.pono;

    poProductDetailValidator.poProductDetail.poQuantity = poDetail.poDetails.quantity;

    poProductDetailValidator.poProductDetail.bookingQuantity = poDetail.poDetails.quantity;

    poProductDetailValidator.poProductDetail.etd = poDetail.poDetails.etd;

    poProductDetailValidator.poProductDetail.destinationCountryID = poDetail.poDetails.destinationCountryID;
  }

  assignProductRelatedDataFromPoPopup(poProductDetailValidator, productDetail) {
    var productData = new POProductList();

    productData.id = productDetail.id;
    productData.name = productDetail.productName;

    poProductDetailValidator.poProductDetail.productId = productDetail.id;
    poProductDetailValidator.poProductDetail.productName = productDetail.productName;
    poProductDetailValidator.poProductDetail.productDesc = productDetail.productDescription;
    poProductDetailValidator.poProductDetail.productImageCount = productDetail.productImageCount;

    var categoryData = new commonDataSource();

    //asign the product category
    if (productDetail.productCategoryId) {
      poProductDetailValidator.poProductDetail.productCategoryName = productDetail.productCategoryName;
      poProductDetailValidator.poProductDetail.productCategoryMapped = true;
    }
    //asign the product sub category
    if (productDetail.productSubCategoryId) {
      var productSubCategoryList = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategoryId;
      categoryData.name = productDetail.productSubCategoryName;
      productSubCategoryList.push(categoryData);
      poProductDetailValidator.poProductDetail.productSubCategoryName = productDetail.productSubCategoryName;
      poProductDetailValidator.poProductDetail.bookingCategorySubProductList = productSubCategoryList;
      poProductDetailValidator.poProductDetail.productSubCategoryMapped = true;
    }
    //asign the product categorysub2
    if (productDetail.productSubCategory2Id) {
      var productSubCategory2List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategory2Id;
      categoryData.name = productDetail.productSubCategory2Name;
      productSubCategory2List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub2Name = productDetail.productSubCategory2Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub2ProductList = productSubCategory2List;
      poProductDetailValidator.poProductDetail.productCategorySub2Mapped = true;
    }
    //asign the product categorysub3
    if (productDetail.productSubCategory3Id) {
      var productSubCategory3List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategory3Id;
      categoryData.name = productDetail.productSubCategory3Name;
      productSubCategory3List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub3Name = productDetail.productSubCategory3Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub3ProductList = productSubCategory3List;
      poProductDetailValidator.poProductDetail.productCategorySub3Mapped = true;
    }

    poProductDetailValidator.poProductDetail.productCategoryId = productDetail.productCategoryId;
    poProductDetailValidator.poProductDetail.productSubCategoryId = productDetail.productSubCategoryId;
    poProductDetailValidator.poProductDetail.productCategorySub2Id = productDetail.productSubCategory2Id;
    poProductDetailValidator.poProductDetail.productCategorySub3Id = productDetail.productSubCategory3Id;

    poProductDetailValidator.poProductDetail.barcode = productDetail.barcode;
    poProductDetailValidator.poProductDetail.factoryReference = productDetail.factoryReference;
    poProductDetailValidator.poProductDetail.remarks = productDetail.remarks;

    return productData;

  }

  changePoPopupProduct(poProduct, productData) {
    poProduct.productDesc = productData.productDescription;
  }

  clearPoPopupProduct(item) {
    item.poDetails.productDesc = "";
    this.getProductListBySearch(item);
  }

  openCustomerProductPopup(content, index) {
    this.modalInputData = {
      typeId: 1,
      customerId: this.model.customerId,
      supplierId: this.model.supplierId,
      productScreenCallType: ProductScreenCallType.Booking
    };

    this.bookingMasterData.selectedPoProductIndex = index;

    this.modelRef = this.modalService.open(content, { windowClass: "add-booking-product-wrapper", centered: true });
  }

  backToPoProductPopup() {

    this.bookingMasterData.isPoProductPopupVisible = true;

  }

  closeProductPopupFromBooking(data) {

    var poProductData = this.bookingMasterData.bookingPOProductValidators[this.bookingMasterData.selectedPoProductIndex];

    var productDetail = data[0];
    //this.bookingMasterData.poProductDataSourceList = data;
    //this.bookingMasterData.poProductDataSourceList = [];

    if (productDetail) {
      this.bookingMasterData.poProductDataSourceList.push({ "id": productDetail.id, "name": productDetail.productID });

      this.assignProductRelatedData(poProductData, productDetail);

      this.getPoProductListBySearch(poProductData);
    }

    this.modelRef.close();

  }

  async addProductDataToPoPopup() {
    this.listModel.productRequest.customerIds = [];

    if (this.purchaseModel.customerId)
      this.listModel.productRequest.customerIds.push(this.purchaseModel.customerId);

    var customerProductResponse = await this.customerProductService.getBaseProductDataSource(this.listModel.productRequest);

    if (customerProductResponse.result == 1) {
      this.customerProductList = customerProductResponse.dataSourceList;
      this.poDetailValidators.forEach(poDetailValidator => {
        this.getProductListBySearch(poDetailValidator);
      });
    }
  }

  removePurchaseOrderDetail(index) {
    this.purchaseModel.purchaseOrderDetails.splice(index, 1);
    this.poDetailValidators.splice(index, 1);
  }

  numericValidation(event) {
    this.utility.numericValidation(event, 7);
  }

  setPrimaryProduct() {

    if (this.bookingMasterData.bookingPOProductValidators && this.bookingMasterData.bookingPOProductValidators.length > 0) {

      //default map all the product to isprimaryproduct to false
      this.bookingMasterData.bookingPOProductValidators.map(x => x.poProductDetail.isPrimaryProductInGroup = false);

      //take the products
      let productIds = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId != null).map(x => x.poProductDetail.productId);

      //take the distinct products
      let distinctproductIds = productIds.filter((n, i) => productIds.indexOf(n) === i);

      //loop through the products and assign the first product to primary product in the group
      distinctproductIds.forEach(productId => {
        var productList = this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == productId);

        if (productList && productList.length > 0)
          productList[0].poProductDetail.isPrimaryProductInGroup = true;
      });

      this.bookingMasterData.bookingPOProductValidators.filter(x => x.poProductDetail.productId == null).map(x => x.poProductDetail.isPrimaryProductInGroup = true);

    }
  }

  changeProductRemarks(item) {
    if (!this.isContainerService)
      this.updatePrimaryProductDataToChild(item, this._bookingProductFieldType.Remarks)
  }

  //fetch the first 10 countries on load
  getProductListBySearch(item) {

    item.poDetails.productRequest = new ProductDataSourceRequest();

    if (this.model.customerId)
      item.poDetails.productRequest.customerIds.push(this.model.customerId);

    item.poDetails.productLoading = true;

    item.poDetails.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poDetails.productLoading = true),
      switchMap(term => term
        ? this.customerProductService.getPOProductListByName(item, term, null)
        : this.customerProductService.getPOProductListByName(item, "", null)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => item.poDetails.productLoading = false))
      ))
      .subscribe(data => {
        item.poDetails.productList = data;
        item.poDetails.productLoading = false;

      });
  }

  //get the po product list with virtual scroll
  getProductListData(item, isDefaultLoad: boolean) {



    item.poDetails.productRequest = new ProductDataSourceRequest();

    item.poDetails.productRequest.productId = item.poDetails.productId;

    if (this.model.customerId)
      item.poDetails.productRequest.customerIds.push(this.model.customerId);

    if (this.model.supplierId)
      item.poDetails.productRequest.supplierIdList.push(this.model.supplierId);

    if (isDefaultLoad) {
      item.poDetails.productRequest.searchText = item.poDetails.productInput.getValue();
      item.poDetails.productRequest.skip = item.poDetails.productList.length;
    }

    item.poDetails.productLoading = true;

    this.customerProductService.getPOProductListByName(item, "", null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(productData => {

        if (productData && productData.length > 0) {
          item.poDetails.productList = item.poDetails.productList.concat(productData);
          item.poDetails.productLoading = false;
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poDetails.productRequest.skip = 0;
          item.poDetails.productRequest.take = ListSize;
        }

        item.poDetails.productLoading = false;

      }),
      error => {
        item.poDetails.productLoading = false;
        this.setError(error);
      };
  }

  getCountryListDataBySearch(item) {

    item.poDetails.countryRequest = new SupplierDataSourceRequest();

    item.poDetails.countryLoading = true;

    item.poDetails.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poDetails.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryListByName(item, term, null)
        : this.locationService.getCountryListByName(item, "", null)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => item.poDetails.countryLoading = false))
      ))
      .subscribe(data => {

        item.poDetails.countryList = data;
        item.poDetails.countryLoading = false;

      });
  }

  //get the po product list with virtual scroll
  getCountryListData(item, isDefaultLoad: boolean) {


    item.poDetails.countryRequest = new CountryDataSourceRequest();


    if (isDefaultLoad) {
      item.poDetails.countryRequest.searchText = item.poDetails.countryInput.getValue();
      item.poDetails.countryRequest.skip = item.poDetails.countryList.length;
    }

    item.poDetails.countryLoading = true;

    this.locationService.getCountryListByName(item, "", null).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(countryData => {

        if (countryData && countryData.length > 0) {
          item.poDetails.countryList = item.poDetails.countryList.concat(countryData);
          item.poDetails.countryLoading = false;
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poDetails.countryRequest.skip = 0;
          item.poDetails.countryRequest.take = ListSize;
        }
        item.poDetails.countryLoading = false;

      }),
      error => {
        item.poDetails.countryLoading = false;
        this.setError(error);
      };
  }

  showLeadDayPopup(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "add-booking-product-wrapper", centered: true });
  }

  togglePoProductCommon() {
    this.poProductCommonData = !this.poProductCommonData;
  }

  async getFileTypeList(data) {
    this.bookingMasterData.productFileLoading = true;

    this.bookingMasterData.productFileUrls = [];

    //get the base product file urls mapped to the products
    var productFileResponse = await this.customerProductService.getProductFileUrls(data.productId);

    //if it is success assign product file urls
    if (productFileResponse.result == CustomerProductFileResult.Success && productFileResponse.productFileUrls) {

      this.bookingMasterData.productFileUrls = productFileResponse.productFileUrls;

      //add the uploaded file url from booking if it is new case
      if (!this.model.id) {

        data.productFileAttachments.forEach(file => {

          this.bookingMasterData.productFileUrls.push(file.fileUrl);

        });
      }

      //pass the file urls to the popup
      this.bookingMasterData.productFileUrls.forEach(fileUrl => {
        this.productGalleryImages.push(
          {
            small: fileUrl,
            medium: fileUrl,
            big: fileUrl,
          });
      });



    }
    else {
      if (data.productFileAttachments) {
        data.productFileAttachments.forEach(file => {
          this.productGalleryImages.push(
            {
              small: file.fileUrl,
              medium: file.fileUrl,
              big: file.fileUrl,
            });
        });
      }
    }

    this.bookingMasterData.productFileLoading = false;

  }

  changeView(view: number) {
    this.viewType = view;
  }

  toggleCustomerSection() {
    this.customerMoreSection = !this.customerMoreSection;
  }

  toggleCommonFields() {
    this.commonFields = !this.commonFields;
  }

  getImageIcon(file) {

    if (file.fileName) {
      var splitFiles = file.fileName.split('.');
      var fileExtension = splitFiles[splitFiles.length - 1].toLocaleLowerCase();

      if (fileExtension == "pdf")
        return "assets/images/new-set/pdf-icon.svg";
      else if (fileExtension == "doc" || fileExtension == "docx")
        return "assets/images/new-set/word-icon.svg";
      else if (fileExtension == "png" || fileExtension == "jpg" || fileExtension == "jpeg")
        return "assets/images/new-set/image-icon.svg";
      else if (fileExtension == "xlsx")
        return "assets/images/new-set/excel-icon.svg";

    }
  }

  //add new contact popup
  addNewContactPopup(masterContactTypeId, content) {
    this.masterContactData.contactValidators = [];
    this.masterContactData.contactList = [];
    this.masterContactData.masterContactTypeId = masterContactTypeId;
    this.addMasterContactRow();
    this.modelRef = this.modalService.open(content,
      {
        windowClass: "add-booking-product-wrapper",
        centered: true,
        backdrop: 'static'
      });
  }

  //add the master contact new row
  addMasterContactRow() {
    var contact = new MasterContact();

    //create the validator
    var masterContactValidator = Validator.getValidator(contact, "booking/add-master-contact.valid.json",
      this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate);

    masterContactValidator.isSubmitted = false;

    //push the contact validators to list
    var contactValidator = { contact: contact, masterContactValidator: masterContactValidator };
    this.masterContactData.contactValidators.push(contactValidator);
    this.masterContactData.contactList.push(contact);
  }

  //remove the master contact row
  removeMasterContactRow(index) {
    this.masterContactData.contactList.splice(index, 1);
    this.masterContactData.contactValidators.splice(index, 1);
    if (this.masterContactData.contactValidators.length == 0)
      this.addMasterContactRow();
  }

  //master contact validation for each row
  isMasterContactValid() {
    var isMasterContactValid = false;
    this.masterContactData.contactValidators.forEach(element => {
      element.masterContactValidator.isSubmitted = true;
    });

    //validation for external users
    isMasterContactValid = this.masterContactData.contactValidators.every((x) => x.masterContactValidator.isValid('name')
      && x.masterContactValidator.isValid('emailId'));

    //validation for customers
    if (isMasterContactValid && this.masterContactData.masterContactTypeId == this.masterContactData.masterContactTypeEnum.customer)
      isMasterContactValid = this.masterContactData.contactValidators.every((x) => x.masterContactValidator.isValid('phoneNo'));

    return isMasterContactValid

  }

  //save the master contact data
  saveMasterContact() {
    this.validator.initTost();
    if (this.isMasterContactValid()) {

      //map the master contact data
      this.mapMasterContact();

      this.bookingMasterData.pageLoader = true;

      this.service.saveMasterContact(this.masterContactData.saveMasterContactRequest)
        .subscribe(
          res => {
            if (res && res.result == SaveDraftInspectionResult.Success) {
              this.processSaveMasterContactResponse();
            }
            else if (res && res.result == SaveDraftInspectionResult.DuplicateEmailFound) {
              this.processSaveMasterContactDuplicateEmailResponse(res);
            }
            else {
              this.showError("MASTER_CONTACT.LBL_TITLE", "MASTER_CONTACT.MSG_UNKNOWN_ERROR");
              this.bookingMasterData.pageLoader = false;
            }
          },
          error => {
            this.setError(error);
            this.showError("MASTER_CONTACT.LBL_TITLE", "MASTER_CONTACT.MSG_UNKNOWN_ERROR");
            this.bookingMasterData.pageLoader = false;
          });
    }
  }

  //process the save master contact response
  processSaveMasterContactResponse() {

    switch (this.masterContactData.masterContactTypeId) {
      case this.masterContactData.masterContactTypeEnum.customer:
        this.getCustomerContactByBrandOrDept();
        break;
      case this.masterContactData.masterContactTypeEnum.supplier:
        this.getBaseSupplierContactDetails(this.model.supplierId, SupplierType.Supplier);
        break;
      case this.masterContactData.masterContactTypeEnum.factory:
        this.getBaseSupplierContactDetails(this.model.factoryId, SupplierType.Factory);
        break;
    }

    this.showSuccess("MASTER_CONTACT.LBL_TITLE", "MASTER_CONTACT.MSG_CONTACT_SUCCESS");
    this.modelRef.close();
    this.bookingMasterData.pageLoader = false;
  }

  //process the save master duplicate email response
  processSaveMasterContactDuplicateEmailResponse(res) {
    this.bookingMasterData.pageLoader = false;
    var emailMsg = "Email '" + res.duplicateEmailIds.join(',') + "' Already Exists";
    this.showError("MASTER_CONTACT.LBL_TITLE", emailMsg);
  }

  //map the master contact data
  mapMasterContact() {

    this.masterContactData.saveMasterContactRequest.contactList = new Array<MasterContact>();

    this.masterContactData.saveMasterContactRequest.masterContactTypeId = this.masterContactData.masterContactTypeId;
    this.masterContactData.saveMasterContactRequest.contactList = this.masterContactData.contactList;

    this.masterContactData.saveMasterContactRequest.customerId = this.model.customerId;
    this.masterContactData.saveMasterContactRequest.supplierId = this.model.supplierId;
    this.masterContactData.saveMasterContactRequest.factoryId = this.model.factoryId;

  }

  //get the base supplier contact details
  getBaseSupplierContactDetails(id, type) {
    this.bookingMasterData.pageLoader = true;
    this.supplierService.getBaseSupplierContactDetails(id)
      .subscribe(
        response => {
          this.processSupplierContactResponse(response, type);
        },
        error => {
          this.setError(error);
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.LBL_NOCUSTOMER_CONTACTS_FOUND');
          this.bookingMasterData.cuscontactloading = false;
        });
  }

  //process the supplier contact response
  processSupplierContactResponse(response, type) {

    if (response && response.result == 1) {

      if (type == SupplierType.Supplier) {
        this.bookingMasterData.supplierContactList = [];
        this.bookingMasterData.supplierContactList = response.contactList;
        this.setSupplierContactPreviewData(response.contactList);
      }

      else if (type == SupplierType.Factory) {
        this.bookingMasterData.factoryContactList = [];
        this.bookingMasterData.factoryContactList = response.contactList;
        this.setFactoryContactPreviewData(response.contactList);
      }

      this.bookingMasterData.pageLoader = false;
    }
    else if (response && response.result == 2) {
      this.showWarning('EDIT_BOOKING.TITLE', 'Contacts Not Found');
    }
    this.bookingMasterData.pageLoader = false;
  }

  clearETD() {
    this.bookingMasterData.parentEtd = null;
  }

  clearChildETD(item) {
    item.poProductDetail.etd = null;
  }

  initializePurchaseOrderDataModel() {
    this.purchaseModel.customerId = this.model.customerId;
    this.purchaseModel.pono = this.bookingMasterData.selectedPoNotAvailableName;
    //add supplier ids,factory ids to purchase order
    this.purchaseModel.supplierIds = [];
    if (this.model.supplierId)
      this.purchaseModel.supplierIds.push(this.model.supplierId);
    if (this.model.factoryId)
      this.purchaseModel.factoryIds.push(this.model.factoryId);
  }

  savePurchaseOrderDataModel() {
    this.initializePurchaseOrderDataModel();

    this.bookingMasterData.pageLoader = true;
    this.purchaseModel.id = 0;
    this.puchaseOrderService.savePurchaseOrder(this.purchaseModel)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.purchaseModel.id = res.id;
            this.bookingMasterData.pageLoader = false;

            var poProductItem = this.bookingMasterData.bookingPOProductValidators[this.bookingMasterData.selectedPoNotAvailableIndex];

            var poData = new commonDataSource();
            poData.id = res.id;
            poData.name = this.purchaseModel.pono;

            poProductItem.poProductDetail.poId = poData.id;

            poProductItem.poProductDetail.poName = this.purchaseModel.pono;

            this.bookingMasterData.poDataSourceList = [];
            this.bookingMasterData.poDataSourceList.push(poData);

            this.getPoListBySearch(poProductItem);

            this.CalculateTotalPurchaseOrder();

            this.showSuccess('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.SAVE_OK');

            this.dialog.close();

          }
          else {
            switch (res.result) {
              case 2:
                this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CANNOT_ADDPURCHASEORDER');
                break;
              case 3:
                this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CURRENTPO_NOTFOUND');
                break;
              case 4: {
                this.addExistingPoToProductList(res.id);
              }
                break;
              case 5:
                this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_BOOKING.MSG_PRODUCT_ALREADY_EXISTS_IN_PO');
                break;
            }
            this.bookingMasterData.pageLoader = false;
          }
        },
        error => {
          this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_UNKNONW_ERROR');
          this.bookingMasterData.pageLoader = false;
        });

  }

  addExistingPoToProductList(poId) {

    if (poId) {
      var poProductItem = this.bookingMasterData.bookingPOProductValidators[this.bookingMasterData.selectedPoNotAvailableIndex];

      var poData = new commonDataSource();
      poData.id = poId;
      poData.name = this.purchaseModel.pono;

      poProductItem.poProductDetail.poId = poData.id;

      poProductItem.poProductDetail.poName = this.purchaseModel.pono;

      this.bookingMasterData.poDataSourceList = [];
      this.bookingMasterData.poDataSourceList.push(poData);

      this.getPoListBySearch(poProductItem);
    }

    this.dialog.close();
  }

  addNewPoData(poName) {
    this.bookingMasterData.selectedPoNotAvailableName = poName;

    this.dialog = this.modalService.open(this.newPoTemplate, { windowClass: "smModelWidth", ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });

  }

  closeAddNewPo() {
    var poProductItem = this.bookingMasterData.bookingPOProductValidators[this.bookingMasterData.selectedPoNotAvailableIndex];
    if (poProductItem)
      this.getPoListBySearch(poProductItem);
    this.dialog.close();
  }

  //fetch the first 10 countries on load
  getCustomerProductListBySearch(item) {

    item.poProductDetail.productRequest = new ProductDetailsDataSourceRequest();

    if (this.model.customerId)
      item.poProductDetail.productRequest.customerIds.push(this.model.customerId);

    //item.poProductDetail.productLoading = true;

    this.bookingMasterData.pageLoader = true;

    item.poProductDetail.productInput = new BehaviorSubject<string>("");

    item.poProductDetail.productInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => item.poProductDetail.productLoading = true),
      switchMap(term => term
        ? this.service.getPOProductListByName(item, term, this.bookingMasterData.editBookingPoProductLoading, this.bookingMasterData.poProductDataSourceList)
        : this.service.getPOProductListByName(item, "", this.bookingMasterData.editBookingPoProductLoading, this.bookingMasterData.poProductDataSourceList)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.bookingMasterData.pageLoader = false))
      ))
      .subscribe(data => {
        item.poProductDetail.productList = data;
        item.poProductDetail.productLoading = false;
        this.bookingMasterData.pageLoader = false;

      });
  }

  //get the po product list with virtual scroll
  getCustomerProductListData(item, isDefaultLoad: boolean) {

    item.poProductDetail.productRequest = new ProductDataSourceRequest();

    item.poProductDetail.productRequest.productId = item.poProductDetail.productId;

    if (this.model.customerId)
      item.poProductDetail.productRequest.customerIds.push(this.model.customerId);

    if (this.model.supplierId)
      item.poProductDetail.productRequest.supplierIdList.push(this.model.supplierId);

    if (isDefaultLoad) {
      item.poProductDetail.productRequest.searchText = item.poProductDetail.productInput.getValue();
      item.poProductDetail.productRequest.skip = item.poProductDetail.productList.length;
    }

    item.poProductDetail.productLoading = true;

    this.service.getPOProductListByName(item, "", this.bookingMasterData.editBookingPoProductLoading, this.bookingMasterData.poProductDataSourceList).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(productData => {

        if (productData && productData.length > 0) {
          item.poProductDetail.productList = item.poProductDetail.productList.concat(productData);
          item.poProductDetail.productLoading = false;
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          item.poProductDetail.productRequest.skip = 0;
          item.poProductDetail.productRequest.take = ListSize;
        }

        item.poProductDetail.productLoading = false;

      }),
      error => {
        item.poProductDetail.productLoading = false;
        this.setError(error);
      };
  }

  assignProductRelatedData(poProductDetailValidator, productDetail) {
    poProductDetailValidator.poProductDetail.productId = productDetail.id;
    poProductDetailValidator.poProductDetail.productName = productDetail.productID;
    poProductDetailValidator.poProductDetail.productDesc = productDetail.productDescription;

    var categoryData = new commonDataSource();

    //asign the product category
    if (productDetail.productCategory) {
      poProductDetailValidator.poProductDetail.productCategoryId = productDetail.productCategory;
      poProductDetailValidator.poProductDetail.productCategoryName = productDetail.productCategoryName;
      poProductDetailValidator.poProductDetail.productCategoryMapped = true;
    }

    //asign the product sub category
    if (productDetail.productSubCategory) {

      poProductDetailValidator.poProductDetail.productSubCategoryId = productDetail.productSubCategory;

      var productSubCategoryList = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productSubCategory;
      categoryData.name = productDetail.productSubCategoryName;
      productSubCategoryList.push(categoryData);
      poProductDetailValidator.poProductDetail.productSubCategoryName = productDetail.productSubCategoryName;
      poProductDetailValidator.poProductDetail.bookingCategorySubProductList = productSubCategoryList;
      poProductDetailValidator.poProductDetail.productSubCategoryMapped = true;
    }

    //asign the product categorysub2
    if (productDetail.productCategorySub2) {

      poProductDetailValidator.poProductDetail.productCategorySub2Id = productDetail.productCategorySub2;


      var productSubCategory2List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productCategorySub2;
      categoryData.name = productDetail.productCategorySub2Name;
      productSubCategory2List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub2Name = productDetail.productCategorySub2Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub2ProductList = productSubCategory2List;
      poProductDetailValidator.poProductDetail.productCategorySub2Mapped = true;
    }
    //asign the product categorysub3
    if (productDetail.productCategorySub3) {

      poProductDetailValidator.poProductDetail.productCategorySub3Id = productDetail.productCategorySub3;

      var productSubCategory3List = new Array<commonDataSource>();
      categoryData = new commonDataSource();
      categoryData.id = productDetail.productCategorySub3;
      categoryData.name = productDetail.productCategorySub3Name;
      productSubCategory3List.push(categoryData);
      poProductDetailValidator.poProductDetail.productCategorySub3Name = productDetail.productCategorySub3Name;
      poProductDetailValidator.poProductDetail.bookingCategorySub3ProductList = productSubCategory3List;
      poProductDetailValidator.poProductDetail.productCategorySub3Mapped = true;
    }

    //poProductDetailValidator.poProductDetail.productImageCount = productDetail.productImageCount;





    poProductDetailValidator.poProductDetail.barcode = productDetail.barcode;
    poProductDetailValidator.poProductDetail.factoryReference = productDetail.factoryReference;
    poProductDetailValidator.poProductDetail.remarks = productDetail.remarks;

  }

  //get the booking file attachment
  getBookingFileAttachment() {
    this.bookingMasterData.bookingZipFileLoading = true;
    this.service.getBookingFileAttachment(this.model.id)
      .subscribe(
        response => {
          this.processBookingFileAttachment(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.bookingZipFileLoading = false;
        });
  }

  processBookingFileAttachment(response) {
    if (response.result == BookingFileZipResult.Success) {
      if (response.fileAttachment) {
        this.bookingMasterData.bookingZipFileAttachment = response.fileAttachment;
        if (this.bookingMasterData.bookingZipFileAttachment.zipFileUrl)
          this.bookingMasterData.showDownloadAllAttachment = true;
      }

      this.bookingMasterData.bookingZipFileLoading = false;
    }
  }

  getBookingZipFileAttachment() {
    if (this.bookingMasterData.bookingZipFileAttachment
      && this.bookingMasterData.bookingZipFileAttachment.zipFileUrl)
      window.open(this.bookingMasterData.bookingZipFileAttachment.zipFileUrl, "_blank");
  }

  async getPoProductDetails(poId, productId, item) {

    //map the po proudct detail request
    var poProductDetailRequest = this.mapPoProductDetailRequest(poId, productId);

    var response = await this.service.getPoProductDetail(poProductDetailRequest);

    if (response.result == POProductDetailResult.Success) {

      var poProductDetail = response.poProductDetail;;
      /*  var product=response.poProductDetail;
       item.poProductDetail.poQuantity = product.poQuantity;
       item.poProductDetail.bookingQuantity = product.poQuantity; */
    }

    return poProductDetail;

  }

  mapPoProductDetailRequest(poId, productId) {
    var poProductDetailRequest = new PoProductDetailRequest();
    poProductDetailRequest.poId = poId;
    poProductDetailRequest.productId = productId;
    poProductDetailRequest.customerId = this.model.customerId;
    poProductDetailRequest.supplierId = this.model.supplierId;
    //poProductDetailRequest.filterPoByProduct = true;
    return poProductDetailRequest;
  }

  //get the inspection type
  getInspectionBookingTypes(id) {
    this.bookingMasterData.inspectionBookingTypeVisible = true;
    this.refService.getInspectionBookingTypes()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionBookingTypes(response, id);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.inspectionBookingTypeVisible = false;
        }
      );
  }

  //process the inspection type
  processInspectionBookingTypes(response, id) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionBookingTypes = response.inspectionBookingTypeList;
      this.bookingMasterData.inspectionBookingTypeVisible = false;
      if (this.currentUser.usertype == this.userTypeEnum.InternalUser && !id && !this.isReBooking && !this.isReInspection) {
        this.model.bookingType = BookingTypes.Announced;
      }
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.inspectionBookingTypeVisible = false;
    }
  }

  //get the inspection location list
  getInspectionPaymentOptions() {
    this.bookingMasterData.inspectionPaymentOptionVisible = true;
    this.refService.getInspectionPaymentOptions(this.model.customerId)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processInspectionPaymentOptions(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.inspectionPaymentOptionVisible = false;
        }
      );
  }

  //process the inspection locations
  processInspectionPaymentOptions(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.bookingMasterData.inspectionPaymentOptions = response.inspectionPaymentOptions;
      this.bookingMasterData.inspectionPaymentOptionVisible = false;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.inspectionPaymentOptionVisible = false;
    }
  }

  //get the inspection location list
  getCustomerCheckPointList() {
    this.bookingMasterData.customerCheckPointLoading = true;
    this.customerCheckPointService.getCustomerCheckPointDataSource(this.model.customerId, Service.Inspection)
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          this.processCustomerCheckPointList(response);
        },
        error => {
          this.setError(error);
          this.bookingMasterData.customerCheckPointLoading = false;
        }
      );
  }

  //process the inspection locations
  processCustomerCheckPointList(response) {
    if (response && response.result == DataSourceResult.Success)
      this.processSuccessCustomerCheckPointList(response);
    else if (response && response.result == DataSourceResult.NotFound) {
      this.bookingMasterData.customerCheckPointLoading = false;
    }
  }

  processSuccessCustomerCheckPointList(response) {

    this.bookingMasterData.customerCheckPointLoading = false;
    this.bookingMasterData.showFactoryCreationLink = false;
    this.bookingMasterData.showSupplierCreationLink = false;
    this.bookingMasterData.poProductDependentFilter = false;

    if (response.checkPointList) {
      if (!(this.model.id > 0) && (this.bookingMasterData.IsCustomer || this.bookingMasterData.Issupplier)) {

        var factoryCreationNotAllowedByCustomer = response.checkPointList.
          find(x => x == CheckPointTypeEnum.FactoryCreationNotAllowedByCustomer);

        if (this.currentUser.usertype == this.userTypeEnum.Customer && !factoryCreationNotAllowedByCustomer)
          this.bookingMasterData.showFactoryCreationLink = true;

        var factoryCreationNotAllowedBySupplier = response.checkPointList.
          find(x => x == CheckPointTypeEnum.FactoryCreationNotAllowedBySupplier);

        if (this.currentUser.usertype == this.userTypeEnum.Supplier && !factoryCreationNotAllowedBySupplier)
          this.bookingMasterData.showFactoryCreationLink = true;

      }

      if (!(this.model.id > 0) && (this.bookingMasterData.IsCustomer)) {
        var supplierCreationNotAllowedByCustomer = response.checkPointList.
          find(x => x == CheckPointTypeEnum.SupplierCreationNotAllowedByCustomer);

        if (!supplierCreationNotAllowedByCustomer)
          this.bookingMasterData.showSupplierCreationLink = true;
      }
      if (this.currentUser.usertype == this.userTypeEnum.Customer && response.checkPointList.
        find(x => x == CheckPointTypeEnum.PoProductBySupplier))
        this.bookingMasterData.poProductDependentFilter = true;
    }
  }

  //toggle the picking quantity field
  toggleExpandPicking(event, index, rowItem) {
    //toggle the picking data for the po product detail
    rowItem.poProductDetail.togglePicking = !rowItem.poProductDetail.togglePicking;
    rowItem.poProductDetail.isPlaceHolderVisible = true;
    let triggerTable = event.target.parentNode.parentNode;
    //pick the element with the picking
    var firstElem = document.querySelector('[data-expand-id="picking' + index + '"]');
    //open the element
    if (rowItem.poProductDetail.togglePicking)
      firstElem.classList.add('open');
    else
      firstElem.classList.remove('open');

    //make active the trigger table
    if (rowItem.poProductDetail.togglePicking)
      triggerTable.classList.add('active');
    else
      triggerTable.classList.remove('active');

    if (firstElem.classList.contains('open')) {
      rowItem.isRowSelected = true;
      var content: any;
      //add the inspection picking list
      if (!rowItem.poProductDetail.saveInspectionPickingList || (rowItem.poProductDetail.saveInspectionPickingList && rowItem.poProductDetail.saveInspectionPickingList.length == 0))
        this.addInspectionPickingList(rowItem);

    }
    else {
      rowItem.poProductDetail.isPlaceHolderVisible = false;
    }
  }

  toggleExpandPickingPreview(event, index, rowItem) {
    //toggle the picking data for the po product detail
    //rowItem.poProductDetail.togglePicking = !rowItem.poProductDetail.togglePicking;
    rowItem.poProductDetail.isPlaceHolderVisible = true;
    let triggerTable = event.target.parentNode.parentNode;
    //pick the element with the picking
    var firstElem = document.querySelector('[data-expand-id="pickingPreview' + index + '"]');
    //open the element
    firstElem.classList.toggle('open');
    //make active the trigger table
    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      rowItem.isRowSelected = true;
      var content: any;
      /*  //add the inspection picking list
       if (rowItem.poProductDetail.saveInspectionPickingList && rowItem.poProductDetail.saveInspectionPickingList.length == 0)
         this.addInspectionPickingList(rowItem); */

    }
    else {
      rowItem.poProductDetail.isPlaceHolderVisible = false;
    }
  }

  OpenFile(url) {
    window.open(url);
  }

  //add the inspection picking row
  addInspectionPickingList(item) {
    var pickingData = new InspectionPickingDetails();
    //saveInspectionPickingList is not there then initialize the list
    if (!item.poProductDetail.saveInspectionPickingList) {
      item.poProductDetail.saveInspectionPickingList = [];
      item.poProductDetail.inspectionPickingValidators = [];
    }
    //add picking data with validator
    var pickingDetailValidator = { pickingData: pickingData, validator: Validator.getValidator(pickingData, "booking/edit-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) };
    if (!item.poProductDetail.inspectionPickingValidators)
      item.poProductDetail.inspectionPickingValidators = [];
    item.poProductDetail.inspectionPickingValidators.push(pickingDetailValidator);
    item.poProductDetail.saveInspectionPickingList.push(pickingData);
  }

  //remove the inspection picking list by index
  removeInspectionPickingList(item, index) {
    item.poProductDetail.saveInspectionPickingList.splice(index, 1);
    item.poProductDetail.inspectionPickingValidators.splice(index, 1);
    /*  if (item.poProductDetail.saveInspectionPickingList == 0)
       this.addInspectionPickingList(item); */
    this.CalculateTotalPickingQuantity();
  }

  //get the lab list by customer id
  getlabList(customerID) {
    this.inspectionPickingMaster.labLoading = true;
    this.pickingService.GetLabList(customerID)
      .pipe()
      .subscribe(
        response => {
          this.processLabListResponse(response);
          this.inspectionPickingMaster.labLoading = false;
        },
        error => {
          this.inspectionPickingMaster.labLoading = false;
          this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
        }
      );
  }

  //process lab list response
  processLabListResponse(response) {
    if (response.result == LabDataResult.Success && response.labList)
      this.inspectionPickingMaster.labOrCustomerList = response.labList;
    else if (response.result == LabDataResult.CannotGetTypeList)
      this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABFOUND");
  }

  //get the picking related details
  getPickingRelatedDetails() {
    //if picking required is available
    if (this.model.isPickingRequired) {
      //get lab list data
      if (this.model.customerId)
        this.getlabList(this.model.customerId);

      this.model.inspectionProductList.forEach(poProductData => {
        poProductData.togglePicking = false;
      });
      //assign picking related data only on edit booking
      if (this.model.id > 0 && !this.isReInspection && !this.isReBooking)
        this.assignPickingRelatedDetails();

    }
  }

  //assign picking related details
  assignPickingRelatedDetails() {
    var labIdList = [];
    var customerInvolvedInPicking = false;
    //get the picking list
    var pickingList = this.model.inspectionProductList.filter(x => x.saveInspectionPickingList).map(x => x.saveInspectionPickingList);

    pickingList.forEach(pickingData => {

      //get the lab id list
      var labIds = pickingData.filter(x => x.labType == LabType.Lab && x.labOrCustomerId).map(x => x.labOrCustomerId);
      labIdList = labIdList.concat(labIds);

      //check customer involved in the picking
      if (pickingData.filter(x => x.labType == LabType.Customer).length > 0)
        customerInvolvedInPicking = true;
    });

    if (labIdList && labIdList.length > 0) {
      //get the distinct lab id list
      var distinctLabIdList = labIdList.filter((n, i) => labIdList.indexOf(n) === i);

      this.assingLabAddressListOnEditBooking(distinctLabIdList);

      this.assingLabContactListOnEditBooking(distinctLabIdList);
    }

    if (customerInvolvedInPicking)
      this.getEditCustomerAddressAndContactsForPicking(this.model.inspectionProductList);
  }

  //assign the lab address list on edit bookng
  assingLabAddressListOnEditBooking(labIdList) {
    //get the lab address list and assign to picking row
    var labAddressRequest = new LabAddressRequest();
    labAddressRequest.labIdList = labIdList;
    this.getEditLabAddressList(labAddressRequest, this.model.inspectionProductList);
  }

  //assign the lab contact list on edit booking
  assingLabContactListOnEditBooking(labIdList) {
    //get the lab contact list and assign to picking row
    var labContactRequest = new LabContactRequest();
    labContactRequest.labIdList = labIdList;
    labContactRequest.customerId = this.model.customerId;
    this.getEditLabContactList(labContactRequest, this.model.inspectionProductList);
  }

  //get the lab related on changing the lab data
  getLabRelatedDetails(lab, pickingItem) {

    pickingItem.pickingData.labOrCustomerAddressList = null;
    pickingItem.pickingData.labOrCustomerAddressId = null;

    pickingItem.pickingData.labOrCustomerContactList = null;
    pickingItem.pickingData.labOrCustomerContactIds = null;
    pickingItem.pickingData.labOrCustomerName = lab.name;

    pickingItem.pickingData.labType = lab.type;
    if (lab.type == this.inspectionPickingMaster._labType.Lab) {

      var labIdList = [];
      labIdList.push(lab.id);
      this.getlabAddressList(labIdList, pickingItem);
      this.getlabContactList(labIdList, pickingItem);
    }
    else if (lab.type == this.inspectionPickingMaster._labType.Customer) {
      this.getCustomerAddressContactsForPicking(this.model.customerId, pickingItem);
    }
  }

  changeLabAddress(labAddress, pickingItem) {
    pickingItem.pickingData.labOrCustomerAddressName = labAddress.name;
  }

  //get customer address contacts
  getCustomerAddressContactsForPicking(labId, pickingItem) {
    //this.labContactLoading = true;
    pickingItem.pickingData.labOrCustomerAddressLoading = true;
    if (this.model.customerId) {
      this.service.getCustomerAddressContactDetails(this.model.customerId)
        .pipe()
        .subscribe(
          response => {
            if (response.result == 1)
              this.processAddCustomerAddressContactsForPicking(response, pickingItem);
            else if (response.result == 2) {
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOCUSTOMERCONTACTSFOUND");
            }
          },
          error => {
            pickingItem.pickingData.labOrCustomerAddressLoading = false;
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  processAddCustomerAddressContactsForPicking(response, pickingItem) {
    this.assignCustomerAddressForPicking(response, pickingItem);
    this.assignCustomerContactsForPicking(response, pickingItem);
  }

  //assign the customer address for po product item
  assignCustomerAddressForPicking(response, pickingItem) {
    //if address list available then assign the data
    if (response.addressList) {
      pickingItem.pickingData.labOrCustomerAddressList = response.addressList;
      //assign the address in the selected list if there is only one address and if it's a new row
      if (response.addressList == 1 && pickingItem.pickingData.labOrCustomerAddressId == null) {
        pickingItem.pickingData.labOrCustomerAddressId = response.addressList[0].id;
      }
      pickingItem.pickingData.labOrCustomerAddressLoading = false;
    }
  }

  //assign customer contact for po product item
  assignCustomerContactsForPicking(response, pickingItem) {
    if (response.contactList) {
      pickingItem.pickingData.labOrCustomerContactList = response.contactList;
      //assign the contact in the selected list if there is only one contact and if it's a new row
      if (response.contactList.length == 1 && pickingItem.pickingData.labOrCustomerContactIds == null) {
        pickingItem.pickingData.labOrCustomerContactIds = [];
        if (response.contactList[0] && response.contactList[0].id)
          pickingItem.pickingData.labOrCustomerContactIds.push(response.contactList[0].id);
      }
      pickingItem.pickingData.labOrCustomerAddressLoading = false;
    }
  }

  //get customer address,contacts for picking
  getEditCustomerAddressAndContactsForPicking(productList) {
    //this.labContactLoading = true;
    if (this.model.customerId) {
      this.service.getCustomerAddressContactDetails(this.model.customerId)
        .pipe()
        .subscribe(
          response => {
            if (response.result == 1)
              this.processCustomerAddressAndContactsSuccessResponse(response, productList);

            else if (response.result == 2) {
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOCUSTOMERCONTACTSFOUND");
            }
          },
          error => {
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  //process customer address contacts success response
  processCustomerAddressAndContactsSuccessResponse(response, productList) {
    if (response.addressList && response.contactList)
      this.assignCustomerAddressContactsForPicking(response, productList);
  }

  //assign customer address contacts for picking
  assignCustomerAddressContactsForPicking(response, productList) {
    //loop through the product list
    productList.forEach(productData => {
      //loop through the inspection picking list
      productData.saveInspectionPickingList.forEach(pickingData => {
        //assign customer address if lab type is customer
        if (pickingData.labType == LabType.Customer) {
          var customerAddressList = response.addressList.filter(x => x.customerId == this.model.customerId);
          if (customerAddressList)
            pickingData.labOrCustomerAddressList = customerAddressList;
          //assing customer contact if lab type is customer
          var customerContactList = response.contactList.filter(x => x.customerId == this.model.customerId);
          if (customerContactList)
            pickingData.labOrCustomerContactList = customerContactList;
        }

      });

    });
  }

  //get the lab address list on edit booking
  getEditLabAddressList(labAddressRequest, productList) {
    this.service.getLabAddressDetails(labAddressRequest)
      .pipe()
      .subscribe(
        response => {
          if (response.result == LabAddressResult.Success) {
            this.processEditLabAddressListSuccessResponse(response, productList);
          }
          else if (response.result == LabAddressResult.CannotGetTypeList) {
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABADDRESSFOUND");
          }
        },
        error => {
          this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
        }
      );
  }

  processEditLabAddressListSuccessResponse(response, productList) {
    if (response.addressList) {
      //loop through the inspection product list
      productList.forEach(productData => {
        //loop through the inspection picking list
        productData.saveInspectionPickingList.forEach(pickingData => {
          //if lab type is lab then assing the lab address list
          if (pickingData.labType == LabType.Lab) {
            //filter the address list by lab id
            var labAddressList = response.addressList.filter(x => x.labId == pickingData.labOrCustomerId);
            //assign the lab adress list
            if (labAddressList)
              pickingData.labOrCustomerAddressList = labAddressList;
          }

        });

      });
    }
  }

  //get the lab contacts on edit booking
  getEditLabContactList(labContactRequest, productList) {
    this.service.getLabContactDetails(labContactRequest)
      .pipe()
      .subscribe(
        res => {
          if (res.result == 1) {
            if (res.labContactList) {
              productList.forEach(productData => {

                productData.saveInspectionPickingList.forEach(pickingData => {

                  if (pickingData.labType == LabType.Lab) {
                    var labContactList = res.labContactList.filter(x => x.labId == pickingData.labOrCustomerId);
                    if (labContactList)
                      pickingData.labOrCustomerContactList = labContactList;
                  }

                });

              });
            }
          }
          else if (res.result == 2) {
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOCONTACTSFOUND");
          }
        },
        error => {
          this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
        }
      )
  };

  //get the lab address list and assing to picking item
  getlabAddressList(labIdList, pickingItem) {
    pickingItem.pickingData.labOrCustomerAddressLoading = true;
    if (labIdList) {

      var labAddressRequest = new LabAddressRequest();
      labAddressRequest.labIdList = labIdList;

      this.service.getLabAddressDetails(labAddressRequest)
        .pipe()
        .subscribe(
          response => {
            if (response.result == LabAddressResult.Success)
              this.processGetLabAddressListSuccessResponse(response, pickingItem);
            else if (response.result == LabAddressResult.CannotGetTypeList)
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABADDRESSFOUND");
            pickingItem.pickingData.labOrCustomerAddressLoading = false;
          },
          error => {
            pickingItem.pickingData.labOrCustomerAddressLoading = false;
            this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  processGetLabAddressListSuccessResponse(response, pickingItem) {
    if (response.addressList) {
      pickingItem.pickingData.labOrCustomerAddressList = response.addressList;

      //assign the address in the selected list if there is only one address and if it's a new row
      if (response.addressList.length == 1 && pickingItem.pickingData.labOrCustomerAddressId == null) {
        pickingItem.pickingData.labOrCustomerAddressId = response.addressList[0].id;
        pickingItem.pickingData.labOrCustomerAddressName = response.addressList[0].name;
      }
    }
  }

  //Fetch the lab contact details and assign to picking item
  getlabContactList(labIdList, pickingItem) {
    pickingItem.pickingData.labContactLoading = true;
    if (labIdList) {
      var labContactRequest = new LabContactRequest();
      labContactRequest.labIdList = labIdList;
      labContactRequest.customerId = this.model.customerId;

      this.service.getLabContactDetails(labContactRequest)
        .pipe()
        .subscribe(
          response => {
            if (response.result == LabDataContactsListResult.Success)
              this.processLabContactResponse(response, pickingItem);
            else if (response.result == LabDataContactsListResult.CannotGetTypeList)
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOCONTACTSFOUND");
            pickingItem.pickingData.labContactLoading = false;
          },
          error => {
            pickingItem.pickingData.labContactLoading = false;
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  //process the lab contact response
  processLabContactResponse(response, pickingItem) {
    if (response.labContactList) {
      pickingItem.pickingData.labOrCustomerContactList = response.labContactList;
      //assign the contact in the selected list if there is only one contact and if it's a new row
      if (response.labContactList.length == 1 && pickingItem.pickingData.labOrCustomerContactIds == null) {
        pickingItem.pickingData.labOrCustomerContactIds = [];
        pickingItem.pickingData.labOrCustomerContactIds.push(response.labContactList[0].id);
      }
    }
  }

  //add new lab address popup
  addNewLabAddressPopup(pickingItem, content) {
    if (pickingItem && pickingItem.pickingData.labOrCustomerId > 0) {
      this.masterLabAddressData = new MasterLabAddressData();
      this.masterLabAddressData.labId = pickingItem.pickingData.labOrCustomerId;
      this.masterLabAddressData.pickingItem = pickingItem;
      this.addlabAddressRow();
      this.modelRef = this.modalService.open(content,
        {
          windowClass: "add-booking-product-wrapper",
          centered: true,
          backdrop: 'static'
        });
    }
  }

  //add the master contact new row
  addlabAddressRow() {
    var saveLabAddressData = new SaveLabAddressData();

    //create the validator
    var masterLabAddressValidator = Validator.getValidator(saveLabAddressData, "lab/edit-address.valid.json",
      this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate);

    masterLabAddressValidator.isSubmitted = false;

    //push the lab address validators to list
    var labAddressValidator = { addressData: saveLabAddressData, masterLabAddressValidator: masterLabAddressValidator };
    this.masterLabAddressData.labAddressValidators.push(labAddressValidator);
    this.masterLabAddressData.labAddressList.push(saveLabAddressData);
  }

  //remove the master lab address row
  removeMasterLabAddressRow(index) {
    this.masterLabAddressData.labAddressList.splice(index, 1);
    this.masterLabAddressData.labAddressValidators.splice(index, 1);
    if (this.masterLabAddressData.labAddressValidators.length == 0)
      this.addlabAddressRow();
  }

  //master lab address validation for each row
  isMasterLabAddressValid() {
    var isMasterLabAddressValid = false;
    this.masterLabAddressData.labAddressValidators.forEach(element => {
      element.masterLabAddressValidator.isSubmitted = true;

    });

    //validation for external users
    isMasterLabAddressValid = this.masterLabAddressData.labAddressValidators.every((x) =>
      x.masterLabAddressValidator.isValid('way') &&
      x.masterLabAddressValidator.isValid('countryId') &&
      x.masterLabAddressValidator.isValid('regionId') &&
      x.masterLabAddressValidator.isValid('cityId') &&
      x.masterLabAddressValidator.isValid('zipCode')
    );

    return isMasterLabAddressValid;

  }

  //get the region list
  refreshRegions(countryId, item: LabAddress) {
    this.labService.getStates(countryId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.regionList = res.data;
          }
          else {
            item.regionList = [];
          }
        },
        error => {
          item.regionList = [];
          this.setError(error);
        });
  }

  //get the city list
  refreshCities(stateId, item: LabAddress) {
    this.labService.getCities(stateId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.cityList = res.data;
          }
          else {
            item.cityList = [];
          }
        },
        error => {
          item.cityList = [];
        });
  }

  mapLabAddressData() {
    this.masterLabAddressData.saveLabAddressRequestData.labId = this.masterLabAddressData.labId;
    this.masterLabAddressData.labAddressList.forEach(labAddress => {
      if (!labAddress.localLanguage)
        labAddress.localLanguage = labAddress.way;
    });

    this.masterLabAddressData.saveLabAddressRequestData.labAddressList = this.masterLabAddressData.labAddressList;
  }

  //save the master lab address data
  saveMasterLabAddressData() {
    this.validator.initTost();
    if (this.isMasterLabAddressValid()) {
      //map the master lab address data
      this.mapLabAddressData();
      this.bookingMasterData.pageLoader = true;
      this.service.saveLabAddressData(this.masterLabAddressData.saveLabAddressRequestData)
        .subscribe(
          res => {
            if (res && res.result == SaveLabAddressResult.Success)
              this.processSuccessMasterLabAddressData();
            else if (res && res.result == SaveLabAddressResult.LabAddressIsNotFound) {
              this.showError("Save Lab Address", "Lab not found");

              this.bookingMasterData.pageLoader = false;
            }
            else {
              this.showError("Save Lab Address", "MASTER_CONTACT.MSG_UNKNOWN_ERROR");
              this.bookingMasterData.pageLoader = false;
            }
          },
          error => {
            this.setError(error);
            this.showError("Save Lab Address", "MASTER_CONTACT.MSG_UNKNOWN_ERROR");
            this.bookingMasterData.pageLoader = false;
          });
    }
  }

  //process the success save lab address data
  processSuccessMasterLabAddressData() {
    this.masterLabAddressData.pickingItem.pickingData.labOrCustomerAddressList = null;
    this.bookingMasterData.pageLoader = false;
    var labIdList = [];
    labIdList.push(this.masterLabAddressData.saveLabAddressRequestData.labId);
    this.getlabAddressList(labIdList, this.masterLabAddressData.pickingItem);
    this.modelRef.close();
  }

  //remove the picking data temporarily
  removePickingData() {
    this.bookingMasterData.isPicking = !this.bookingMasterData.isPicking;
    this.bookingMasterData.bookingPOProductValidators.forEach(poProductData => {
      poProductData.poProductDetail.saveInspectionPickingList = [];
      poProductData.poProductDetail.inspectionPickingValidators = [];
    });
    this.dialog.close();
  }

  //close the picking data
  closeRemovePickingData() {
    this.model.isPickingRequired = true;
    this.dialog.close();
  }

  changeLabContact(pickingItem) {
    if (pickingItem.pickingData.labOrCustomerContactIds && pickingItem.pickingData.labOrCustomerContactList) {
      pickingItem.pickingData.labOrCustomerContactName = pickingItem.pickingData.labOrCustomerContactList.filter(x => pickingItem.pickingData.labOrCustomerContactIds.includes(x.id)).map(x => x.name).join(',');
    }

  }

  changeDACorrelation() {
    this.model.gapDAName = null;
    this.model.gapDAEmail = null;
  }

  showPickingDescription(content) {
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  customAqlchange(customAql, sample100SampleType, productItem) {
    productItem.poProductDetail.aql = customAql.id;
    productItem.poProductDetail.aqlName = customAql.aqlName;
    productItem.poProductDetail.sampleType = sample100SampleType.id;
    productItem.poProductDetail.aqlQuantity = productItem.poProductDetail.bookingQuantity;
    this.updatePrimaryProductDataToChild(productItem, this._bookingProductFieldType.Aql);
    productItem.poProductDetail.previewAqlName = productItem.poProductDetail.aqlName + " " + productItem.poProductDetail.sampleType;
    productItem.poProductDetail.previewSamplingSize = productItem.poProductDetail.aqlQuantity;
  }

  setSampleSizeValue() {
    this.bookingMasterData.bookingPOProductValidators.forEach(element => {
      this.processCustomAQLQuanity(element);
    })
  }
}
