import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ViewChildren, QueryList, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import {
  QuotationLoader, SupplierGradeRequest,
  QuotationModel, QuotationResult, QuotationResponse, QuotationEntityType, DataSource, QuotationDataSourceResult,
  FilterOrderRequest, OrderListResult, Order, QuotProduct, SaveQuotationResult, QuotationDataSourceResponse, OrderListResponse,
  SaveQuotationResponse, QuotationAbility, QuotationStatus, SetStatusRequest, SetStatusQuotationResponse, SetStatusQuotationResult, QuotationBillMethod,
  QuotationBillPaidBy, QuoBookingInfoPopUp, QuoationMandayrequest, BillingEntity, PaymentTerm, ManDayResult, PreviewBookInfo, PreviewMainDetails,
  BookingDateChangeInfo, BookingDateChangeInfoResult, QuotCheckpointRequest, CalculatedWorkingHoursResult, PriceCardTravelResponse, PriceCardTravelResult, CalculateManDaySaveResult, SupplierGradeResult, FactoryBookingInfoRequest, FactoryBookingInfoResult, FactoryBookingInfo, FactoryBookingDetail
} from '../../../_Models/quotation/quotation.model';
import { QuotationService } from '../../../_Services/quotation/quotation.service';
import { BookingService } from '../../../_Services/booking/booking.service';
import { NgbModalRef, NgbModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { QuotationContactComponent } from '../quotation-contact/quotation-contact.component';
import { CountryModel } from '../../../_Models/office/edit-officemodel';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryThumbnailsComponent } from 'ngx-gallery-9';
import { AttachmentFile } from '../../../_Models/Audit/edit-auditmodel';
import { CustomerProduct } from '../../../_Services/customer/customerproductsummary.service';
import { UtilityService } from '../../../_Services/common/utility.service';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { UserModel } from '../../../_Models/user/user.model';
import { UserType, Currency, APIService, QuotationTableScrollHeight, Country, BillingMethod, Url, EntityAccess, QuantityType, SupplierType, Service, BillPaidBy, InvoiceType } from '../../common/static-data-common';
import { InspectionBookingsummarymodel } from 'src/app/_Models/booking/inspectionbookingsummarymodel'
import { QuotStatus } from 'src/app/_Models/Audit/auditsummarymodel';
import { QuotationCustomerPriceCard, CustomerPriceCardRequest, CustomerPriceCardUnitPriceRequest, CustomerPriceCardUnitPriceResponse, UnitPriceDetailModel, UnitPriceResponseResult, CustomerPriceCardResponseResult, QuotationPriceCard, SamplingUnitPriceRequest, SamplingUnitPriceResponse, SamplingPriceResponseResult, MasterCustomerPriceCard, CustomerPriceCard } from 'src/app/_Models/customer/customer-price-card.model';
import { CustomerPriceCardService } from '../../../_Services/customer/customerpricecard.service';
import { TravelMatrixResponseResult, QuotationTravelMatrixResponse, TravelMatrixRequest, QuotationTravelMatrix } from '../../../_Models/invoice/travelmatrix';
import { TravelMatrixService } from 'src/app/_Services/invoice/travelmatrix.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';

@Component({
  selector: 'app-quotation',
  templateUrl: './edit-quotation.component.html',
  styleUrls: ['./edit-quotation.component.css']
})

export class EditQuotationComponent extends DetailComponent {
  public model: QuotationModel;
  public bokinfopopupmodel: QuoBookingInfoPopUp;
  public bookingsummarymodel: InspectionBookingsummarymodel;
  public previewMainDetails: PreviewMainDetails
  public data: QuotationResponse;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public quotationEntityType: any = QuotationEntityType;
  public ability: any = QuotationAbility;
  public status: any = QuotationStatus;
  public previewbookInfo: PreviewBookInfo
  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[];
  private modelRef: NgbModalRef;
  public orderDetails: Array<Order>;
  public request: FilterOrderRequest
  public isNew: boolean;
  public IsInternalUser: boolean = false;
  public isBookingQuotationLoader: boolean = false;
  public bookproductdesc: string = "";
  currentUser: UserModel;
  private _router: Router
  loader: QuotationLoader;
  _userTypeId
  noBookingData: boolean;
  popuploading: boolean = false;
  previewloading: boolean = false;
  MessagePopup: string;
  public ServicedatechangePopup: string;
  HideAllbutton: boolean = false;
  public validator: Validator;
  public _isfrombookingsummary: boolean = false;
  public _isSearchManualOrder: boolean = false;
  public _IscusconfigMissing: boolean = false;
  public requestManday: QuoationMandayrequest;
  public multipleRulePriceCardList: any;
  ruleId: number;
  editTab1: boolean;
  editTab2: boolean;
  editTab3: boolean;
  editTab4: boolean;
  BillingMethod = QuotationBillMethod;
  public samplingUnitPriceList: Array<SamplingUnitPriceRequest>;
  public showConfirmDate: boolean = false;
  bookingQuotationLoaderMsg: string;
  public quotationCheckpointRequest: QuotCheckpointRequest;
  public quotationSkipCheckpointExists: boolean = false;
  public isForwardToManager = "isForwardToManager";

  @ViewChildren(QuotationContactComponent) contactContainers: QueryList<QuotationContactComponent>;
  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  @ViewChild('scrollableTablepopup') scrollableTablepopup: ElementRef;
  @ViewChild('quotationdatechange') servicedatechangepopup: ElementRef;
  @ViewChild('multiplecustomerpricecard') multiplecustomerpricemodel: TemplateRef<any>;
  customerPriceCardDialog: NgbModalRef | null;

  modalStatus: number;
  public ispreview: boolean = false;
  statusList: Array<number>;
  apiService = APIService;
  public quotationCustomerPriceCard: QuotationCustomerPriceCard;
  public quotationTravelMatrix: QuotationTravelMatrix;
  public unitPriceDetailModel: Array<UnitPriceDetailModel>;
  public containerMasterList: any[] = [];
  travelMatrixCountyId: number;
  travelMatrixCityId: number;
  travelMatrixProvinceId: number;
  public calculatedWorkingHourMessage: string;
  public refreshLoadng: boolean = false;
  travelTextMessage: string;
  isTravelWarningShow: boolean;
  isHideMessage: boolean = true;
  bookingId: number;

  public selectedEntity: number;
  componentDestroyed$: Subject<boolean> = new Subject();
  isBillMethodPieceRate: boolean;
  factoryGrade: string;
  supplierGrade: string;
  factoryBookingDetail: FactoryBookingDetail;
  isSearch: boolean = false;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    private jsonHelper: JsonHelper,
    route: ActivatedRoute,
    router: Router,
    private supplierService: SupplierService,
    private authService: AuthenticationService,
    private service: QuotationService,
    public bookingService: BookingService,
    private productService: CustomerProduct,
    private modalService: NgbModal,
    public customerPriceCardService: CustomerPriceCardService,
    public travelMatrixService: TravelMatrixService,
    public referenceService: ReferenceService,
    private calendar: NgbCalendar, public utility: UtilityService, public pathroute: ActivatedRoute) {
    super(router, route, translate, toastr);
    this._translate = translate;
    this._toastr = toastr;
    this._router = router;
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
    this.currentUser = authService.getCurrentUser();
    this._userTypeId = this.currentUser.usertype;
    this.IsInternalUser = this._userTypeId == UserType.InternalUser ? true : false;
    this.validator = new Validator(jsonHelper, toastr, translate);
    this.statusList = [QuotationStatus.CustomerValidated, QuotationStatus.CustomerRejected, QuotationStatus.Canceled];
    this.quotationCustomerPriceCard = new QuotationCustomerPriceCard();
    this.unitPriceDetailModel = new Array<UnitPriceDetailModel>();
    this.selectedEntity = Number(this.utility.getEntityId());
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }

  onInit(id?: any) {
    //get booking details from booking summary page
    this.pathroute.queryParams.subscribe(
      params => {
        if (params != null && params['paramParent'] != null) {
          this.bookingsummarymodel = JSON.parse(decodeURI(params['paramParent']));
          if (this.bookingsummarymodel != null && id == null) {
            this._isfrombookingsummary = true;
          }
        }
      });
    this.init(id);
  }

  getContainerList() {

    for (let index = 1; index <= 100; index++) {

      this.containerMasterList.push({ "id": index, "name": "container - " + index });

    }
  }

  getViewPath(): string {
    return "";
  }

  getEditPath(): string {
    if (!this.IsInternalUser)
      return "quotations/quotation-confirm";
    else
      return "quotations/edit-quotation";
  }

  public getAddPath(): string {
    return "quotations/new-quotation";
  }
  ExpandAllTab() {
    this.editTab1 = true;
    this.editTab2 = true;
    this.editTab3 = true;
    this.editTab4 = true;
  }
  ExpandTabForCustomer() {
    this.editTab1 = false;
    this.editTab2 = false;
    this.editTab3 = true;
    this.editTab4 = false;
  }
  async init(id?) {
    this.loading = true;
    this.loader = new QuotationLoader();
    this.model = new QuotationModel();
    this.previewbookInfo = new PreviewBookInfo();
    this.previewMainDetails = new PreviewMainDetails();
    this.requestManday = new QuoationMandayrequest();
    this.factoryBookingDetail = new FactoryBookingDetail();
    this.validator.isSubmitted = false;
    this.validator.setJSON("quotation/quotation.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = this.validator.jsonHelper;

    this.model.orderList = [];
    this.noBookingData = false;
    this.isNew = this._router.url.indexOf(this.getAddPath()) >= 0;
    if (this.IsInternalUser) {
      this.ExpandAllTab();
    }
    else {
      this.ExpandTabForCustomer();
    }
    this.getContainerList();
    this.data = {
      countryList: null,
      billingMethodList: null,
      billPaidByList: null,
      billingEntities: null,
      officeList: null,
      serviceList: null,
      result: QuotationResult.Success,
      currencyList: null,
      model: null,
      customerList: [],
      supplierList: null,
      factoryList: null,
      abilities: null,
      paymentTermList: null,
      billQuantityTypeList: null,
      factoryBookingDetail: null,
      paymentTermsValueList: null,
      isPreInvoiceContactMandatoryInQuotation: false
    };
    this.isBookingQuotationLoader = true;
    this.bookingQuotationLoaderMsg = this.utility.textTranslate("QUOTATION.LBL_BOOKING_DETAILS_LOADER");
    this.validator.isSubmitted = false;

    if (this.contactContainers) {
      this.contactContainers.forEach(x => {
        if (x.currentValidator != null)
          x.currentValidator.isSubmitted = false;
      });
    }
    let response: QuotationResponse;
    try {
      response = await this.service.getQuotation(id);
    }
    catch (e) {
      this.isBookingQuotationLoader = false;
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }

    this.loading = false;
    this.getBillingQuantityTypeList();
    if (response) {
      if (!this._isfrombookingsummary) {
        this.isBookingQuotationLoader = false;
      }
      switch (response.result) {
        case QuotationResult.Success:
          {
            this.data = response;
            if (id) {
              this.model = response.model;
              if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.PieceRate) {
                this.isBillMethodPieceRate = true;
              }
              // set after fetch
              this.ruleId = this.model.ruleId;
              this.travelIncludeUnitPrice(this.ruleId);
              this.travelMatrixCountyId = this.model && this.model.factory && this.model.factory.countyId ?
                this.model.factory.countyId : null;

              this.travelMatrixCityId = this.model && this.model.factory && this.model.factory.cityId ?
                this.model.factory.cityId : null;

              this.travelMatrixProvinceId = this.model && this.model.factory && this.model.factory.provinceId ?
                this.model.factory.provinceId : null;

              for (let item of this.model.orderList) {

                if (item.productList && item.productList.length > 0) {
                  if (item.productValidatorList == null)
                    item.productValidatorList = [];

                  for (let product of item.productList)
                    item.productValidatorList.push({
                      product: product,
                      validator: Validator.getValidator(product, "quotation/quotation-order.valid.json", this.jsonHelper, false, this._toastr, this._translate)
                    })
                }
                item.ordervalidator = { ordercost: item.orderCost, validator: Validator.getValidator(item.orderCost, "quotation/quotation-ordercost.valid.json", this.jsonHelper, false, this._toastr, this._translate) };

                if (item.orderMandayvalidatorList == null)
                  item.orderMandayvalidatorList = [];

                if (item.quotationMandayList) {
                  for (let manday of item.quotationMandayList) {
                    let val: Validator = Validator.getValidator(manday, "quotation/quotation-ordermanday.valid.json", this.jsonHelper, false, this._toastr, this._translate);
                    item.orderMandayvalidatorList.push({ ordermanday: manday, validator: val });
                  }
                }
                this.GetTotalInspFeesByBooking(item);
              }
              this.GetTotalReport_AQL_Products(true);
              this.GetTotalPicking(true);
              this.GetTotalReport_ContainerService(true);
              this.contactContainers.forEach(x => {
                x.refreshData();
              });
              if (this.model.service.id == Service.Inspection)
                this.factoryBookingInfo();
              else
                this.model.factoryBookingInfoList = [];
              this.getSupplierAndFactoryGrade();
              setTimeout(() => {
                this.limitTableHeight();
              }, 500);
              this.PreviewMainDetails();
            }
            else if (this._isfrombookingsummary) {
              var factcountryid = this.bookingsummarymodel.Quotdetails.factorycountryid;
              var serviceid = this.bookingsummarymodel.Quotdetails.service;
              var officeid = this.bookingsummarymodel.Quotdetails.officeid;
              if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.PieceRate)
                this.isBillMethodPieceRate = true;
              if (factcountryid)
                this.model.country = this.data.countryList.filter(x => x.id == factcountryid)[0];
              if (serviceid)
                this.model.service = this.data.serviceList.filter(x => x.id == serviceid)[0];

              this.model.skipQuotationSentToClient = await this.getSkipQuotationSendToClientCheckpoint(this.bookingsummarymodel.Quotdetails.bookingitems, this.bookingsummarymodel.Quotdetails.customerid);
              this.quotationSkipCheckpointExists = this.model.skipQuotationSentToClient;
              this.setCustomerList(this.model.country, this.model.service);


              if (!this._IscusconfigMissing) {
                this.addOrder(null);
                this.search();
              }
            }
            break;
          }
        case QuotationResult.CannotFindBillingMethodList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_BILLSNOTFOUND');
          break;
        case QuotationResult.CannotFindBillPaidByList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_PAIDBYLISTNOTFOUND');
          break;
        case QuotationResult.CannotFindCountryList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_COUNTRYLISTNOTFOUND');
          break;
        case QuotationResult.CannotFindOfficeList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_OFFICELISTNOTFOUND');
          break;
        case QuotationResult.CannotFindServiceList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_SERVICELISTNOTFOUND');
          break;
        case QuotationResult.CannotFindCurrencies:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CURRENCYLISTNOTFOUND');
          break;
        case QuotationResult.CurrentQuotationNotFound:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CURRENCTQUOTNOTFOUND');
          break;
        case QuotationResult.CannotGetCustList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTGETCUSTLIST');
          break;
        case QuotationResult.CannotGetSuppList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTGETSUPPLIST');
          break;
        case QuotationResult.CannotGetFactoryList:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTGETFACTLIST');
          break;
        case QuotationResult.NoAccess:
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_NOACCESS');
          break;
      }
    }
  }

  public async setCustomerList(country: CountryModel, service: any) {
    this.data.customerList = [];
    this.clearData();
    this.loader.customerLoading = true;

    if (!this._isfrombookingsummary)
      this.model.orderList = [];

    if (country != null && service != null) {


      let response: QuotationDataSourceResponse;

      try {
        response = await this.service.getCustomerList(country.id, service.id);
      }
      catch (e) {
        console.error(e);
        this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
        this.loader.customerLoading = false;
      }

      if (response) {
        switch (response.result) {
          case QuotationDataSourceResult.Success:
            this.data.customerList = [...response.dataSource];

            if (this._isfrombookingsummary) {
              var customerid = this.bookingsummarymodel.Quotdetails.customerid;
              if (customerid) {
                this.model.customer = this.data.customerList.filter(x => x.id == customerid)[0];
                if (!this.model.customer) {
                  this.isBookingQuotationLoader = false;
                  this._IscusconfigMissing = true;
                  this.loader.customerLoading = false;
                  this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CON_NOT_CONFIG');
                  return false;
                }
                this.model.customerLegalName = this.data.customerList.filter(x => x.id == customerid)[0].name;
                this.setSupplierList(this.model.customer);
              }

              //assign the office id from booking summary page
              if (this.bookingsummarymodel.Quotdetails.officeid) {
                var item = this.data.officeList.find(x => x.id == this.bookingsummarymodel.Quotdetails.officeid);
                if (item != null) {
                  this.model.office = item;
                }
              }

              //call child control and assign below data and call function to refresh the internal contacts
              this.contactContainers.forEach(x => {
                if (x.type == QuotationEntityType.Internal) {
                  x.id = this.model.office;
                  x.parentId = this.model.customer.id;
                  x.refreshData();
                }
              });
            }
            break;
          case QuotationDataSourceResult.CountryEmpty:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_COUNTRYISEMPTY');
            break;
          case QuotationDataSourceResult.ServiceEmpty:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_SERVICEISEMPTY');
            break;
          case QuotationDataSourceResult.NotFound:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_DATANOTFOUND');
            break;
        }
      }
    }
    this.loader.customerLoading = false;
  }
  public async setSupplierList(customer: DataSource) {
    this.loader.supplierLoading = true;
    if (customer != null)
      this.model.isToForward = this.model.skipQuotationSentToClient ? false : customer.isForwardToManager;
    else
      this.model.isToForward = false;

    this.model.supplierContactList = null;
    this.data.supplierList = [];
    this.model.supplier = null;

    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Supplier) {
        x.updateData(null);
      }
    });
    if (!this._isfrombookingsummary)
      this.model.orderList = [];

    if (customer != null) {
      this.model.customer = customer;

      let response: QuotationDataSourceResponse;

      try {
        response = await this.service.getSupplierList(customer.id);
      }
      catch (e) {
        console.error(e);
        this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
        this.loader.supplierLoading = false;
      }

      switch (response.result) {
        case QuotationDataSourceResult.Success:
          this.data.supplierList = [...response.dataSource];
          if (this._isfrombookingsummary) {
            var supid = this.bookingsummarymodel.Quotdetails.supplierid;
            if (supid) {
              this.model.supplier = this.data.supplierList.filter(x => x.id == supid)[0];
              this.model.supplierLegalName = this.data.supplierList.filter(x => x.id == supid)[0].name;
              this.setFactoryList(this.model.supplier);
            }
          }

          //call child control and assign below data and call function to refresh the internal contacts
          this.contactContainers.forEach(x => {
            if (x.type == QuotationEntityType.Internal) {
              x.id = this.model.office;
              x.parentId = this.model.customer.id;
              x.refreshData();
            }
          });
          break;
        case QuotationDataSourceResult.NotFound:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_DATANOTFOUND');
          break;
      }
    }
    this.setPaymentTerms(customer);
    this.loader.supplierLoading = false;
  }

  public async setFactoryList(supplier: DataSource) {
    this.loader.factoryLoading = true;
    this.model.factoryContactList = null;
    this.data.factoryList = [];
    this.model.factory = null;

    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Factory) {
        x.updateData(null);
      }
    });
    if (!this._isfrombookingsummary)
      this.model.orderList = [];
    if (supplier != null) {
      this.model.supplier = supplier;


      let response: QuotationDataSourceResponse;

      try {
        response = await this.service.getFactoryList(supplier.id);
      }
      catch (e) {
        console.error(e);
        this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
        this.loader.factoryLoading = false;
      }

      switch (response.result) {
        case QuotationDataSourceResult.Success:
          this.data.factoryList = [...response.dataSource];

          if (this._isfrombookingsummary) {
            var factid = this.bookingsummarymodel.Quotdetails.factoryid;
            if (factid) {
              this.model.factory = this.data.factoryList.filter(x => x.id == factid)[0];
              this.model.legalFactoryName = this.data.factoryList.filter(x => x.id == factid)[0].name;
              this.travelMatrixCountyId = response.dataSource.find(x => x.id == factid).countyId;
              this.travelMatrixCityId = response.dataSource.find(x => x.id == factid).cityId;
              this.travelMatrixProvinceId = response.dataSource.find(x => x.id == factid).provinceId;

              if (this.model.service.id == Service.Inspection)
                this.factoryBookingInfo();
              else
                this.model.factoryBookingInfoList = [];
            }
          }

          break;
        case QuotationDataSourceResult.NotFound:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_DATANOTFOUND');
          break;
      }
    }
    this.loader.factoryLoading = false;
  }

  public setFactoryId(factory: DataSource) {
    this.model.factory = null;

    if (factory != null) {
      this.model.factory = factory;
      this.travelMatrixCountyId = factory.countyId;
      this.travelMatrixCityId = factory.cityId;
      this.travelMatrixProvinceId = factory.provinceId;
    }
    if (!this._isfrombookingsummary)
      this.model.orderList = [];
  }

  public setOfficeId(office: DataSource) {
    if (office != null) {
      this.model.office = office;
    }
  }
  public SetBookInfoDetails(booking: Order, content) {
    if (booking != null) {
      this.bokinfopopupmodel = new QuoBookingInfoPopUp();
      this.bokinfopopupmodel = {
        inspectionFrom: booking.inspectionFrom,
        inspectionTo: booking.inspectionTo,
        serviceTypeStr: booking.serviceTypeStr,
        office: booking.office,
        qcBookingRemarks: booking.qcBookingRemarks,
        internalBookingRemarks: booking.internalBookingRemarks,
        priceCategoryName: booking.priceCategoryName,
        prevBookingNo: booking.previousBookingNo,
        bookingId: booking.id,
        bookingAttachmentUrl: booking.bookingZipFileUrl,
        paymentOption: booking.paymentOption,
        dynamicFieldDatas: booking.dynamicFieldData
      };
      this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
  }
  public GetTotalInspFeesByBooking(item: Order) {

    //if billing method is manday
    if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.Manday) {

      //calculate each booking total manday * unitprice assign to service fees
      item.orderCost.inspFees = parseFloat((item.orderCost.noOfManday * item.orderCost.unitPrice).toFixed(2));
    }
    //if billing method is sampling
    else if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.Sampling) {

      //no calculation - unitprice as srevice fees(inspFees)
      item.orderCost.inspFees = item.orderCost.unitPrice;
    }

    //if billing method is piece rate
    else if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.PieceRate) {

      //no calculation - unitprice as srevice fees(inspFees)
      item.orderCost.inspFees = (item.orderCost.quantity) * (item.orderCost.unitPrice);
    }

    //if billing method is per intervention
    else if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.PerIntervention) {

      //no calculation - unitprice as srevice fees(inspFees)
      item.orderCost.inspFees = item.orderCost.unitPrice;
    }
    else {
      this.showWarning('QUOTATION.TITLE', "QUOTATION.MSG_BILLING_METHOD_SELECT");
    }
    this.setFactoryBookingInfo(item);
    this.totalCalculateAmount();
  }

  //total calculation of all booking
  totalCalculateAmount() {
    this.model.inspectionFees = this.model.orderList.reduce((sum, qty) => sum + qty.orderCost.inspFees, 0);
    this.model.travelCostsAir = this.model.orderList.reduce((sum, qty) => sum + qty.orderCost.travelAir, 0);
    this.model.travelCostsLand = this.model.orderList.reduce((sum, qty) => sum + qty.orderCost.travelLand, 0);
    this.model.travelCostsHotel = this.model.orderList.reduce((sum, qty) => sum + qty.orderCost.travelHotel, 0);

    //sum the all booking total manday column
    this.model.estimatedManday = this.model.orderList.reduce((sum, qty) => sum + qty.orderCost.noOfManday, 0);
  }

  public GetTotalCompositionQty(product: QuotProduct, booking: Order) {
    if (product && booking) {

      return booking.productList.filter(x => x.combineProductId == product.combineProductId).reduce((sum, qty) => sum + qty.bookingQty, 0);
    }
  }
  public SetBookProductDescDetails(desc, content) {
    if (desc) {
      this.bookproductdesc = desc;
      this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
  }
  public getTotal(): number {
    return (this.model.inspectionFees || 0)
      + (this.model.travelCostsAir || 0)
      + (this.model.travelCostsLand || 0)
      + (this.model.travelCostsHotel || 0)
      + (this.model.otherCosts || 0)
      - (this.model.discount || 0);
  }
  GetTotalnumberWithCommas() {
    var x = this.getTotal();
    if (x) {
      var parts = x.toString().split(".");
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      return parts.join(".");
    } else {
      return x;
    }
  }
  public getSubTotal(): number {
    return (this.model.inspectionFees || 0)
      + (this.model.travelCostsAir || 0)
      + (this.model.travelCostsLand || 0)
      + (this.model.travelCostsHotel || 0);
  }
  public getCurrency(): string {
    return this.model.currency && this.model.currency.currencyCode ? this.model.currency.currencyCode : "";
  }
  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  isFormValid() {

    let isvalid: boolean = true;
    try {

      isvalid = this.validator.isFormValid();

      if (!isvalid)
        return false;

      this.contactContainers.forEach(x => {
        if (isvalid) {
          isvalid = x.currentValidator.isFormValid();
        }
      });

      if (!isvalid)
        return false;

      isvalid = this.isValidEmail();

      if (!isvalid)
        return false;

      for (let item of this.model.orderList) {

        if (isvalid) {
          isvalid = item.ordervalidator.validator.isValid('unitPrice')
            && item.ordervalidator.validator.isValid('noOfManday');
        }

        if (isvalid && this.isBillMethodPieceRate) {
          isvalid = item.ordervalidator.validator.isValid('billedQtyType')
            && item.ordervalidator.validator.isValid('quantity');
        }

        if (!isvalid)
          return false;

        if (item.productValidatorList) {
          for (let vproduct of item.productValidatorList) {
            if (isvalid) {
              isvalid = vproduct.validator.isFormValid();
            }
          }
        }
        if (!isvalid)
          return false;
        if (item.ordervalidator.ordercost.noOfManday > 0) {
          if (item.orderMandayvalidatorList) {
            for (let manday of item.orderMandayvalidatorList) {
              if (isvalid)
                isvalid = manday.validator.isFormValid();
            }
          }
        }
      }
      if (!isvalid)
        return false;

      if (this.model.orderList) {
        this.model.orderList.forEach(element => {
          if (element.orderCost.noOfManday != null) {
            if (isvalid) {
              var totalquomanday = element.quotationMandayList.filter(x => x.bookingId == element.id).reduce((sum, x) => x.manDay + sum, 0) + element.orderCost.travelManday;
              if (totalquomanday != element.orderCost.noOfManday) {
                this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_BOOK_MAN_Day_NOT_MATCH');
                isvalid = false;
              }
            }
          }
        });
      }
      if (!isvalid)
        return false;

      if (isvalid && this.model.totalCost < 0) {
        this.showWarning('QUOTATION.TITLE', "QUOTATION.MSG_QUOTATION_TOTALCOST");
        isvalid = false;
      }

      if (isvalid && this.model.paymentTerm == InvoiceType.PreInvoice && this.data.isPreInvoiceContactMandatoryInQuotation) {
        if (this.model?.billingPaidBy?.id == BillPaidBy.Customer) {
          if (this.model.customerContactList == undefined || this.model.customerContactList == null || this.model.customerContactList.length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CUSTOMER_CONTACT_REQ');
            return false;
          }

          if (this.model.customerContactList.filter(x => x.invoiceEmail).length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_ATLEAST_ONE_INVOICEMAIL_CUSTOMERCONTACT_REQ');
            return false;
          }
        }

        if (this.model?.billingPaidBy?.id == BillPaidBy.Supplier) {

          if (this.model.supplierContactList == undefined || this.model.supplierContactList == null || this.model.supplierContactList.length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_SUPPLIER_CONTACT_REQ');
            return false;
          }

          if (this.model.supplierContactList.filter(x => x.invoiceEmail).length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_ATLEAST_ONE_INVOICEMAIL_SUPPLIERCONTACT_REQ');
            return false;
          }

        }

        if (this.model?.billingPaidBy?.id == BillPaidBy.Factory) {

          if (this.model.factoryContactList == undefined || this.model.factoryContactList == null || this.model.factoryContactList.length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_FACTORY_CONTACT_REQ');
            return false;
          }

          if (this.model.factoryContactList.filter(x => x.invoiceEmail).length == 0) {
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_ATLEAST_ONE_INVOICEMAIL_FACTORYCONTACT_REQ');
            return false;
          }

        }
      }

      //restrict to create quotations if all bookings manday is zero and total cost ==0
      // if (isvalid) {
      //   var mandaywithzero = this.model.orderList.filter(x => x.ordervalidator.ordercost.noOfManday == 0);
      //   if (mandaywithzero && mandaywithzero.length > 0) {
      //     var noofManDayExist = this.model.orderList.filter(x => x.ordervalidator.ordercost.noOfManday > 0);
      //     //at least one man day not equal to zero
      //     if (noofManDayExist && noofManDayExist.length == 0 && this.model.totalCost == 0) {
      //       isvalid = false;
      //       this.showWarning('QUOTATION.TITLE', "QUOTATION.MSG_MANDAY_BOOKING");
      //     }
      //   }
      // }
    }
    catch (e) {
      return isvalid = false;
    }
    return isvalid;
  }

  private isValidEmail(): boolean {

    let type: QuotationEntityType;
    let emailmessage: string;
    let quotationmsg: string;
    switch (this.model.billingPaidBy.id) {
      case 1:
        type = QuotationEntityType.Customer;
        emailmessage = "QUOTATION.MSG_ATLEASTCUSTEMAIL_REQ";
        quotationmsg = "QUOTATION.MSG_ATLEASTCUSTQUO_REQ";
        break;
      case 2:
        type = QuotationEntityType.Supplier;
        emailmessage = "QUOTATION.MSG_ATLEASTSUPPEMAIL_REQ";
        quotationmsg = "QUOTATION.MSG_ATLEASTSUPPQUO_REQ";
        break;
      case 3:
        type = QuotationEntityType.Factory;
        emailmessage = "QUOTATION.MSG_ATLEASTFACTEMAIL_REQ";
        quotationmsg = "QUOTATION.MSG_ATLEASTFACTQUO_REQ";
        break;
    }

    let isvalid: boolean = true;
    // quotation team don't know the contacts while create the quotation.
    // this.contactContainers.forEach(x => {
    //   if (isvalid) {
    //     if (x.type == type) {
    //       if (x.model == null || x.model.length <= 0) {
    //         this.showWarning('QUOTATION.TITLE', emailmessage);
    //         isvalid = false;
    //       }
    //       if (isvalid && x.model && !x.model.some(x => x.email)) {
    //         this.showWarning('QUOTATION.TITLE', emailmessage);
    //         isvalid = false;
    //       }
    //       if (isvalid && x.model && !x.model.some(x => x.quotation)) {
    //         this.showWarning('QUOTATION.TITLE', quotationmsg);
    //         isvalid = false;
    //       }
    //     }
    //   }
    // });

    if (this.model.orderList == null || this.model.orderList.length == 0 && isvalid) {
      this.showWarning('QUOTATION.TITLE', "QUOTATION.MSG_ATLEASTORDER_REQ");
      isvalid = false;
    }

    return isvalid;
  }


  notSelected(): boolean {
    return this.model.customer == null || this.model.supplier == null || this.model.factory == null;
  }

  addOrder(content) {
    this.orderDetails = [];
    if (this._isfrombookingsummary && content == null) {
      this.request = {
        bookingNo: null,
        customerId: 0,
        supplierId: 0,
        factoryId: 0,
        serviceId: this.model.service && this.model.service.id ? APIService.Inspection : this.model.service.id,
        endDate: null,
        startDate: null,
        bookingIds: this.bookingsummarymodel.Quotdetails.bookingitems,
        billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
        billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
        currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : null
      }
      this._isSearchManualOrder = false;
    }
    else {
      // Search orders from database
      this.request = {
        bookingNo: null,
        customerId: this.model.customer.id,
        supplierId: this.model.supplier.id,
        factoryId: this.model.factory.id,
        serviceId: this.model.service.id,
        endDate: this.calendar.getToday(),
        startDate: this.calendar.getPrev(this.calendar.getToday(), 'd', 7),
        bookingIds: [],
        billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
        billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
        currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : null
        //should choose bill method and padi by before search booking
      }
      this._isSearchManualOrder = true;
    }

    if (content != null) {

      this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", backdrop: 'static' });
      this.modelRef.result.then((result) => {
        // this.closeResult = `Closed with: ${result}`;
      }, (reason) => {
        //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
    }
  }


  async search() {
    this.loader.bookingSearchLoading = true;
    this.noBookingData = false;
    this.orderDetails = [];
    let response: OrderListResponse;
    try {
      response = await this.service.getOrderList(this.request);
    }
    catch (e) {
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }

    if (response) {
      switch (response.result) {
        case OrderListResult.Success:

          if (this.model.orderList == null)
            this.model.orderList = [];

          this.orderDetails = response.data.filter(x => !this.model.orderList.some(y => y.id == x.id));

          if (this.orderDetails.length > 0) {
            this.isSearch = true;
            if (!this._isSearchManualOrder) {
              this.orderDetails.forEach(x => x.isChecked = true);
            }
            this.selectOrders();

            setTimeout(() => {
              this.limitTableHeightpopup();
            }, 5);

            this.setBillingEntity();
          }
          else
            this.noBookingData = true;
          break;
        case OrderListResult.NotFound:
          this.noBookingData = true;
          break;
      }
    }
    this.loader.bookingSearchLoading = false;
    if (this.model.service.id == APIService.Inspection)
      this.getCalculatedWorkingHours();
  }

  async searchOrders(fieldName, value) {
    this.loader.bookingSearchLoading = true;
    this.noBookingData = false;
    this.orderDetails = [];
    this.model.orderList = [];

    this.request[fieldName] = value;

    let response: OrderListResponse;

    try {
      response = await this.service.getOrderList(this.request);
    }
    catch (e) {
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }

    if (response) {
      switch (response.result) {
        case OrderListResult.Success:
          this.orderDetails = response.data;
          break;
        case OrderListResult.NotFound:
          this.noBookingData = true;
          break;
      }
    }
    this.loader.bookingSearchLoading = false;
  }
  public async selectOrders() {
    this.loader.selectOrderLoading = true;
    var newSelectedBookingIds = [];
    if (this.model.orderList == null)
      this.model.orderList = [];

    for (let item of this.orderDetails) {
      if (item.isChecked && !this.model.orderList.some(x => x.id == item.id)) {

        newSelectedBookingIds.push(item.id);
        item.ordervalidator = { ordercost: item.orderCost, validator: Validator.getValidator(item.orderCost, "quotation/quotation-ordercost.valid.json", this.jsonHelper, false, this._toastr, this._translate) }
        this.model.orderList.push(item);

        if (item.productList && item.productList.length > 0) {
          if (item.productValidatorList == null)
            item.productValidatorList = [];

          for (let product of item.productList) {

            let val: Validator = Validator.getValidator(product, "quotation/quotation-order.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate);
            item.productValidatorList.push({ product: product, validator: val });
          }
        }
        if (item.orderMandayvalidatorList == null)
          item.orderMandayvalidatorList = [];
        if (item.quotationMandayList) {
          for (let manday of item.quotationMandayList) {
            let val: Validator = Validator.getValidator(manday, "quotation/quotation-ordermanday.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate);
            item.orderMandayvalidatorList.push({ ordermanday: manday, validator: val });
          }
        }
      }
    }

    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {

      var priceCardRequest = new CustomerPriceCardUnitPriceRequest();

      priceCardRequest = {
        billMethodId: 0,
        billPaidById: 0,
        bookingIds: newSelectedBookingIds,
        currencyId: 0,
        ruleId: 0,
        quotationId: this.model.id,
        billQuantityType: 0,
        invoiceType: this.model.paymentTerm,
        paymentTermsValue: this.model.paymentTermsValue,
        paymentTermsCount: this.model.paymentTermsCount
      };
      //no need to show unit price change popup when loading the bookings for the first time
      this.mapUnitPrice("", priceCardRequest);

      this.travelIncludeUnitPrice(this.ruleId);
      //calculate the total amount
      this.calculateAmountByBooking();

      if (this.model.service.id == APIService.Inspection)
        this.getCalculatedWorkingHours();

    }

    this.PreviewMainDetails();
    this.GetTotalReport_AQL_Products();
    this.GetTotalReport_ContainerService();
    this.GetTotalPicking();
    if (this.model.service.id == Service.Inspection)
      this.factoryBookingInfo();
    else
      this.model.factoryBookingInfoList = [];
    this.getSupplierAndFactoryGrade();

    if (this._isSearchManualOrder) {
      this.model.skipQuotationSentToClient = await this.getSkipQuotationSendToClientCheckpoint(newSelectedBookingIds, this.model.customer.id);
    }
    this.quotationSkipCheckpointExists = this.model.skipQuotationSentToClient;

    var selectedData = this.orderDetails && this.orderDetails.length > 0 ? this.orderDetails.filter(x => x.isChecked == true) : [];
    if (this.orderDetails.length > 0 && (selectedData && selectedData.length == 0)) {
      if (!this.isSearch)
        this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_SELECTBOOKING');
      this.isSearch = false;
    }
    else {
      if (this._isSearchManualOrder) {
        this.modelRef.close();
      }
      else {
        this.isBookingQuotationLoader = false;
      }
    }
    setTimeout(() => {
      this.limitTableHeight();
    }, 15);
    this.loader.selectOrderLoading = false;
  }
  public Edittab1() {
    this.contactContainers.forEach(x => {

      if (x.type == QuotationEntityType.Customer) {
        this.model.customerContactList = x.model;
      }
      else if (x.type == QuotationEntityType.Supplier) {
        this.model.supplierContactList = x.model;
      }
      else if (x.type == QuotationEntityType.Factory) {
        this.model.factoryContactList = x.model;
        this.model.factoryAddress = x.address;
      }
    });
    this.PreviewMainDetails();
    this.editTab1 = !this.editTab1
  }
  public PreviewMainDetails() {

    this.previewMainDetails.customer = this.model.customer ? this.model.customer.name : "";
    this.previewMainDetails.supplier = this.model.supplier ? this.model.supplier.name : "";
    this.previewMainDetails.factory = this.model.factory ? this.model.factory.name : "";
    this.previewMainDetails.country = this.model.country ? this.model.country.countryName : "";
    this.previewMainDetails.service = this.model.service ? this.model.service.name : "";
    this.previewMainDetails.billPaidBy = this.model.billingPaidBy ? this.model.billingPaidBy.name : "";
    this.previewMainDetails.billingMethod = this.model.billingMethod ? this.model.billingMethod.label : "";
    this.previewMainDetails.office = this.model.office ? this.model.office.name : "";
    this.previewMainDetails.factoryAddress = this.model.factoryAddress ? this.model.factoryAddress : "";
    this.previewMainDetails.customerContactList = this.model.customerContactList ? this.model.customerContactList : [];
    this.previewMainDetails.supplierContactList = this.model.supplierContactList ? this.model.supplierContactList : [];
  }

  public GetTotalReport_ContainerService(isedit: boolean = false) {
    if (this.model.service.id == APIService.Inspection) {
      let reportcount: number = 0;
      if (this.model.orderList && this.model.orderList.length > 0 && this.model.orderList.filter(x => x.isChecked == true)) {
        var orderlist = !isedit ? this.model.orderList.filter(x => x.isChecked == true && x.isContainerServiceType) :
          this.model.orderList.filter(x => x.isContainerServiceType);
        if (orderlist && orderlist.length > 0) {
          var totalReports = 0;
          var totalProducts = 0;
          var totalBookings = 0;
          var containerList = [];
          orderlist.forEach(element => {
            totalReports = totalReports + element.containerList.length;
            //if products are available with another service type(other than container service)
            if (this.previewbookInfo.totalProductsList && this.previewbookInfo.totalProductsList.length > 0) {
              element.productList.forEach(product => {
                var productFound = this.previewbookInfo.totalProductsList.find(x => x == product.id);
                if (!productFound)
                  this.previewbookInfo.totalProductsList.push(product.id);
              });
              totalProducts = totalProducts + this.previewbookInfo.totalProductsList.length;
            }
            else {
              totalProducts = totalProducts + element.productList.length;
            }
            //push container data to calculate the total containers
            element.containerList.forEach(container => {
              if (!containerList.includes(container))
                containerList.push(container);
            });

          });
          totalBookings = orderlist.length;

          this.previewbookInfo.totalContainers = containerList.length;
          this.previewbookInfo.totalReports = (isNaN(this.previewbookInfo.totalReports) ? 0 : this.previewbookInfo.totalReports) + totalReports;
          this.previewbookInfo.totalProducts = totalProducts;
          this.previewbookInfo.totalCombine = (isNaN(this.previewbookInfo.totalCombine) ? 0 : this.previewbookInfo.totalCombine) + 0;
          this.previewbookInfo.totalSampling = (isNaN(this.previewbookInfo.totalSampling) ? 0 : this.previewbookInfo.totalSampling) + 0;
          this.previewbookInfo.totalBooking = this.previewbookInfo.totalBooking + totalBookings;
        }
      }
    }
  }

  //Get total Picking Count
  public GetTotalPicking(isedit: boolean = false) {
    if (this.model.service.id == APIService.Inspection) {
      var _totalPicking = 0;
      if (this.model.orderList && this.model.orderList.length > 0 && this.model.orderList.filter(x => x.isChecked == true)) {
        var orderlist = !isedit ? this.model.orderList.filter(x => x.isChecked == true && !x.isContainerServiceType) :
          this.model.orderList.filter(x => !x.isContainerServiceType);
        if (orderlist && orderlist.length > 0) {


          orderlist.forEach(element => {

            if (element.productList && element.productList.length > 0) {
              //Sum of each product picking qty by booking
              _totalPicking = _totalPicking + element.productList
                .reduce((sum, current) => sum + current.pickingQty, 0);
            }

          });

        }
      }
      this.previewbookInfo.totalPicking = _totalPicking;
    }
  }


  public GetTotalReport_AQL_Products(isedit: boolean = false) {
    if (this.model.service.id == APIService.Inspection) {
      let reportcount: number = 0;
      if (this.model.orderList && this.model.orderList.length > 0 && this.model.orderList.filter(x => x.isChecked == true)) {
        var orderlist = !isedit ? this.model.orderList.filter(x => x.isChecked == true && !x.isContainerServiceType) :
          this.model.orderList.filter(x => !x.isContainerServiceType);

        var productitems: QuotProduct[] = [];
        var productIds: any = [];

        orderlist.forEach(element => {
          element.productList.forEach(x => productitems.push(x));
        });
        if (productitems != null && productitems.length > 0) {
          var totalcombine: number[] = [];
          productitems.forEach(function (value) {
            if (value.combineProductId) {
              if (!totalcombine.includes(value.combineProductId))
                totalcombine.push(value.combineProductId);
            }
            else {
              reportcount++;
            }
            productIds.push(value.id);
          });
          this.previewbookInfo.totalSampling = productitems.reduce((sum, current) => sum + current.sampleQty, 0);
          this.previewbookInfo.totalCombine = totalcombine ? totalcombine.length : 0;
          this.previewbookInfo.totalReports = reportcount + this.previewbookInfo.totalCombine;
          this.previewbookInfo.totalProductsList = productIds;
          var lstdistinctproduct = productitems.filter((thing, i, arr) => {
            return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
          });
          this.previewbookInfo.totalProducts = (lstdistinctproduct ? lstdistinctproduct.length : 0);
        }
        this.previewbookInfo.totalBooking = orderlist.length;
        this.previewbookInfo.totalContainers = 0;
      }
    }
  }

  async save() {
    this.PreviewMainDetails();
    this.loader.saveLoading = true;
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.model.totalCost = this.getTotal();

    this.contactContainers.forEach(x => {
      x.currentValidator.isSubmitted = true;

      if (x.type == QuotationEntityType.Customer) {
        this.model.customerLegalName = x.legalName;
        this.model.customerContactList = x.model;
      }
      else if (x.type == QuotationEntityType.Supplier) {
        this.model.supplierLegalName = x.legalName;
        this.model.supplierContactList = x.model;
      }
      else if (x.type == QuotationEntityType.Factory) {
        this.model.legalFactoryName = x.legalName;
        this.model.factoryContactList = x.model;
        this.model.factoryAddress = x.address;
      }
      else if (x.type == QuotationEntityType.Internal) {
        this.model.internalContactList = x.model;
      }
    });

    for (let item of this.model.orderList) {
      if (item.ordervalidator && item.ordervalidator.validator) {
        item.ordervalidator.validator.isSubmitted = true;
      }

      if (item.productValidatorList) {
        for (let productV of item.productValidatorList) {
          productV.validator.isSubmitted = true;
        }
      }
      if (item.orderMandayvalidatorList) {
        for (let manday of item.orderMandayvalidatorList) {
          manday.validator.isSubmitted = true;
        }
      }
    }

    if (this.isFormValid()) {

      for (let item of this.model.orderList) {
        item.ordervalidator = null;
        item.productValidatorList = null;
        item.orderMandayvalidatorList = null;
      }
      // this.model.quotationMandays = this.getManday;

      let response: SaveQuotationResponse;

      try {

        if (this.ruleId > 0) {
          this.model.ruleId = this.ruleId;
        }

        response = await this.service.saveQuotation(this.model);
      }
      catch (e) {
        console.error(e);
        this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
        this.loader.saveLoading = false;
      }

      if (response) {
        this._isfrombookingsummary = false;
        switch (response.result) {
          case SaveQuotationResult.Success:
            this.showSuccess('QUOTATION.TITLE', 'QUOTATION.MSG_SAVED');
            this.init(response.item.id);
            break;
          case SaveQuotationResult.SuccessWithBrodcastError:
            this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_SAVED_BRDERROR');
            this.init(this.model.id);
            break;
          case SaveQuotationResult.NotFound:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_NOTFOUND');
            this.init(this.model.id);
            break;
          case SaveQuotationResult.CannotSave:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTSAVE');
            this.init(this.model.id);
            break;
          case SaveQuotationResult.NoAccess:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_NORIGHTSTOUPDATESTATUS');
            this.init(this.model.id);
            break;
          case SaveQuotationResult.ServiceDateChanged:
            {
              this.openServiceDateChangePopup(response.serviceDateChangeInfo);
              this.HideAllbutton = true;
              break;
            }
          case SaveQuotationResult.QuotationExists:
            this.showError('QUOTATION.TITLE', this.utility.textTranslate('QUOTATION.MSG_QUOTATION_EXISTS')
              + response.bookingOrAuditNos + '. ' + this.utility.textTranslate('QUOTATION.MSG_CHECK'));
            this.init(this.model.id);
            break;
          case SaveQuotationResult.BookingIsCancelled:
            this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_BOOKING_CANCELLED');
            this.init(this.model.id);
            break;
          case SaveQuotationResult.BookingIsHold:
            this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_BOOKING_HOLD');
            this.init(this.model.id);
            break;
        }
      }
    }
    else {
      if (this.IsInternalUser) {
        this.ExpandAllTab();
      }
    }
    this.loader.saveLoading = false;
    this.isBookingQuotationLoader = false;
  }
  openServiceDateChangePopup(bookdatechangeinfo: BookingDateChangeInfo) {
    if (bookdatechangeinfo.result == BookingDateChangeInfoResult.DateChanged) {
      this.ServicedatechangePopup = (this.utility.textTranslate('QUOTATION.MSG_BOOKING') + "#" + bookdatechangeinfo.bookingId +
        this.utility.textTranslate('QUOTATION.MSG_SERVICE_DATE') + bookdatechangeinfo.previousServiceDateFrom + " - " + bookdatechangeinfo.previousServiceDateTo +
        this.utility.textTranslate('QUOTATION.MSG_SERVICEDATETO') + bookdatechangeinfo.serviceDateFrom + " - " + bookdatechangeinfo.serviceDateTo + ".");

      this.modelRef = this.modalService.open(this.servicedatechangepopup, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    }
    else {
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTSAVE');
    }
  }
  async setStatus() {
    this.popuploading = true;
    let request: SetStatusRequest = {
      id: this.model.id,
      statusId: this.modalStatus,
      cuscomment: this.model.customerRemark,
      apiRemark: this.model.apiRemark,
      apiInternalRemark: this.model.apiInternalRemark,
      confirmDate: this.model.confirmDate
    }

    let response: SetStatusQuotationResponse;

    try {
      response = await this.service.setStatus(request);
    }
    catch (e) {
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }

    if (response) {
      switch (response.result) {
        case SetStatusQuotationResult.Success:
          this.showSuccess('QUOTATION.TITLE', 'QUOTATION.MSG_STATUSUPDATED');
          this.init(this.model.id);
          break;
        case SetStatusQuotationResult.SuccessButErrorBrodcast:
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_SAVED_BRDERROR');
          this.init(this.model.id);
          break;
        case SetStatusQuotationResult.CannotUpdateStatus:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CANNOTUPDATESTATUS');
          break;
        case SetStatusQuotationResult.NoAccess:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_NORIGHTSTOUPDATESTATUS');
          break;
        case SetStatusQuotationResult.StatusNotConfigued:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWNSTATUS');
          break;
        case SetStatusQuotationResult.QuotationNotFound:
          this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_QUOTATIONNOTFOUND');
          break;
        case SetStatusQuotationResult.BookingNotConfirmed:
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_CONFIRM_BOOKING');
          break;
        case SetStatusQuotationResult.ServiceDateChanged:
          {
            this.modelRef.close();
            this.openServiceDateChangePopup(response.serviceDateChangeInfo);
            this.HideAllbutton = true;
            break;
          }
        case SetStatusQuotationResult.BookingIsCancelled:
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_BOOKING_CANCELLED');
          break;
        case SetStatusQuotationResult.BookingIsHold:
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_BOOKING_HOLD');
          break;
      }
    }
    if (SetStatusQuotationResult.ServiceDateChanged != response.result)
      this.modelRef.close();
    this.popuploading = false;
    this.modalStatus = null;
  }

  deleteOrder(item: Order) {
    var index = this.model.orderList.indexOf(item);

    if (index >= 0)
      this.model.orderList.splice(index, 1);

    if (this.model.service.id == Service.Inspection)
      this.factoryBookingInfo();
    else
      this.model.factoryBookingInfoList = [];
    this.GetTotalPicking();
  }

  /**
   * function to toggle tabs on click
   * @param {event} event     [current event]
   * @param {string} tabTarget [targeted tab id]
   */
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

  getImagesProduct(ids: Array<number>, modalContent) {
    this.productGalleryImages = [];

    for (let item of ids) {
      this.getProductFiles(item);
    }

    this.modelRef = this.modalService.open(modalContent, { windowClass: "mdModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => { });
  }


  getProductFiles(id: number) {
    this.productGalleryImages = [];

    this.productService.getProductFile(id)
      .subscribe(res => {
        if (res != null) {
          var url = window.URL.createObjectURL(res);
          this.productGalleryImages.push(
            {
              small: url,
              medium: url,
              big: url,
            });
        }
      },
        error => {
          //this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
        });

  }

  preview() {
    this.ispreview = true;
    this.service.preview(this.model.id)
      .pipe()
      .subscribe(res => {
        if (res.filePath) {
          this.ispreview = false;
          window.open(res.filePath);
        }
      },
        error => {
          this.ispreview = false;
          console.log(error);
        });
  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }
  limitTableHeight() {
    let height = this.scrollableTable ?
      this.scrollableTable.nativeElement.offsetHeight : 0;
    if (height > QuotationTableScrollHeight)
      this.scrollableTable.nativeElement.classList.add('scroll-ytableQuotation');

  }
  limitTableHeightpopup() {
    let height = this.scrollableTablepopup ?
      this.scrollableTablepopup.nativeElement.offsetHeight : 0;
    if (height > 10) {
      this.scrollableTablepopup.nativeElement.classList.add('scrollable');
    }
  }
  openModalPopup(quotationstatus, idStatus: QuotationStatus) {
    this.modalStatus = idStatus;
    this.showConfirmDate = false;
    if (this.IsInternalUser && idStatus == QuotationStatus.CustomerValidated) {
      this.showConfirmDate = true;
      this.model.confirmDate = this.calendar.getToday();
    }
    this.ModalPopupContentMessage(idStatus);
    this.modelRef = this.modalService.open(quotationstatus, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }

  // show Message content in popup based on quotation status
  ModalPopupContentMessage(idStatus) {
    if (idStatus == QuotationStatus.ManagerApproved)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_APPROVE');
    else if (idStatus == QuotationStatus.Canceled)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_CANCEL');
    else if (idStatus == QuotationStatus.AERejected)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_REJECT');
    else if (idStatus == QuotationStatus.ManagerRejected)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_REJECT');
    else if (idStatus == QuotationStatus.SentToClient)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_SEND');
    else if (idStatus == QuotationStatus.CustomerValidated)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_CONFIRM');
    else if (idStatus == QuotationStatus.CustomerRejected)
      this.MessagePopup = this.utility.textTranslate('QUOTATION.MSG_CONFRMSTATUS') +
        this.utility.textTranslate('QUOTATION.MSG_REJECT');
  }

  downloadVersion(fileUrl: string) {
    if (fileUrl) {
      window.open(fileUrl);
    }
  }
  returntosummary() {
    if (this.IsInternalUser) {
      if (this._isfrombookingsummary) {
        this._router.navigate([`/${this.utility.getEntityName()}/inspsummary/quotation-pending/3`], { queryParams: { param: encodeURI(JSON.stringify(this.bookingsummarymodel)) } });
        //super.return('inspsummary/quotation-pending/3');
      }
      else {
        super.return('quotations/quotation-summary');
      }
    }
    else
      super.return('quotations/quotation-confirm');
  }

  //service country dropdown changes clear customer,supplier,factory
  clearData() {
    this.model.customerContactList = [];
    this.model.internalContactList = [];
    this.model.office = null;
    this.model.customer = null;
    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Customer) {
        x.updateData(null);
      }
    });
    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Internal) {
        x.updateData(null);
      }
    });
    this.model.supplierContactList = null;
    this.data.supplierList = [];
    this.model.supplier = null;
    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Supplier) {
        x.updateData(null);
      }
    });
    this.model.factoryContactList = null;
    this.data.factoryList = [];
    this.model.factory = null;
    this.contactContainers.forEach(x => {
      if (x.type == QuotationEntityType.Factory) {
        x.updateData(null);
      }
    });
  }
  openQuotationHistory(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }
  // booking product sample(aql, combine) qty and quotation sample qty count is equal call save or else show popup
  async checkProductAqlOrCombineQuantityChange(productquantity) {

    //service id is inspection execute if condition else call save method
    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {

      //if quotation status any one of the following status customer validated, cus reject, cancel call save method
      if (this.statusList.indexOf(this.model.statusId) > -1) {
        this.save();
      }
      else {
        var productList = Array<QuotProduct>();

        //push the product list
        this.model.orderList.forEach(orderElement => {
          productList.push.apply(productList, orderElement.productList);
        });

        //sample quantity not equal show popup
        if (!(await this.checkQuotationSampleQtyAndBookingSampleQtyAreEqual(productList))) {
          this.modalService.open(productquantity, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
        }
        else {
          this.save();
        }
      }
    }
    else {
      this.save();
    }
  }

  //api call to check booking and quotation sample qty equal or not
  async checkQuotationSampleQtyAndBookingSampleQtyAreEqual(productList: Array<QuotProduct>) {
    try {
      return await this.service.checkQuotationSampleQtyAndBookingSampleQtyAreEqual(productList);
    }
    catch (e) {
      console.error(e);
      this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
    }
  }

  //if billingmethod changed unit price amount differ so get the price by customer price rule config and calculate the amount
  changeBillingMethod(unitpricechange, event) {

    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {

      if (this.model.billingMethod && this.model.billingPaidBy && this.model.currency) {

        //make the price card request based on the selected data
        var priceCardRequest = new CustomerPriceCardUnitPriceRequest();
        // reset rule
        this.ruleId = null;

        priceCardRequest = {
          billMethodId: event.id,
          billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
          bookingIds: this.model.orderList.map(x => x.id),
          currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : 0,
          ruleId: this.ruleId,
          quotationId: this.model.id,
          billQuantityType: 0,
          invoiceType: this.model.paymentTerm,
          paymentTermsValue: this.model.paymentTermsValue,
          paymentTermsCount: this.model.paymentTermsCount
        };
        this.model.ruleId = this.ruleId;
        //map the unit price based on the billingmethod
        this.mapUnitPrice(unitpricechange, priceCardRequest);
        //calculate the total amount
        this.calculateAmountByBooking();

      }

    }
    else if (this.model.service && this.model.service.id && this.model.service.id == APIService.Audit) {

      this.model.orderList.forEach(item => {
        item.orderCost.inspFees = 0;
        item.orderCost.unitPrice = 0;
      });
      this.model.inspectionFees = 0;
    }

    if (event.id == QuotationBillMethod.PieceRate) {
      this.isBillMethodPieceRate = true;
    }
    else {
      this.isBillMethodPieceRate = false;
    }
  }

  getBillingQuantityTypeList() {
    this.referenceService.getBillQuantityTypeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.data.billQuantityTypeList = response.dataSourceList;
          }
        },
        error => {
          this.setError(error);
        }
      );
  }

  //if currency changed unit price amount differ so get the price by customer price rule config and calculate the amount
  changeCurrency(unitpricechange, event) {

    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {
      if (this.model.billingMethod && this.model.billingPaidBy && this.model.currency) {
        var priceCardRequest = new CustomerPriceCardUnitPriceRequest();
        // reset rule
        this.ruleId = null;

        priceCardRequest = {
          billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
          billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
          bookingIds: this.model.orderList.map(x => x.id),
          currencyId: event.id,
          ruleId: this.ruleId,
          quotationId: this.model.id,
          billQuantityType: 0,
          invoiceType: this.model.paymentTerm,
          paymentTermsValue: this.model.paymentTermsValue,
          paymentTermsCount: this.model.paymentTermsCount
        };

        this.model.ruleId = this.ruleId;

        this.mapUnitPrice(unitpricechange, priceCardRequest);

        this.travelIncludeUnitPrice(this.ruleId);

        //calculate the total amount
        this.calculateAmountByBooking();
      }
    }
  }

  //Set the Billing Entity Dropdown if service and bill paid by are selected
  setBillingEntity() {

    this.setBillEntity();
  }

  setBillEntity() {
    if (this.selectedEntity == EntityAccess.API) {
      this.model.billingEntity = null;
      if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {
        if (this.model.billingPaidBy && this.model.billingPaidBy.id == QuotationBillPaidBy.customer)
          this.model.billingEntity = BillingEntity.AsiaPacificInspectionLtdHONGKONG;

        else {
          if (this.model.country && this.model.country.id == Country.China)
            this.model.billingEntity = BillingEntity.GuangzhouOuyataiCHINA;

          else if (this.model.country && this.model.country.id == Country.Vietnam)
            this.model.billingEntity = BillingEntity.AsiaPacificInspectionVietnamCompanyLtdVIETNAM;

          else
            this.model.billingEntity = BillingEntity.AsiaPacificInspectionLtdHONGKONG;
        }
      }

      else if (this.model.service && this.model.service.id && this.model.service.id == APIService.Audit)
        this.model.billingEntity = BillingEntity.APIAuditLimitedHONGKONGAudit;
    }
  }

  //Set the Billing Entity Dropdown if service and bill paid by are selected
  changePaidBySetBillingEntity(unitpricechange, event) {

    this.setBillEntity();

    //based on the billing paid by we are showing the invoice email checkbox
    //when change the billing paid by then uncheck the existing invoice email for all
    if (this.contactContainers) {
      this.contactContainers.forEach(x => {
        if (x.type != QuotationEntityType.Internal && x.model)
          x.model.forEach(x => x.invoiceEmail = false);
      });
    }

    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {
      if (this.model.billingMethod && this.model.billingPaidBy && this.model.currency) {
        var priceCardRequest = new CustomerPriceCardUnitPriceRequest();

        // reset rule
        this.ruleId = null;

        priceCardRequest = {
          billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
          billPaidById: event.id,
          bookingIds: this.model.orderList.map(x => x.id),
          currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : 0,
          ruleId: this.ruleId,
          quotationId: this.model.id,
          billQuantityType: 0,
          invoiceType: this.model.paymentTerm,
          paymentTermsCount: this.model.paymentTermsCount,
          paymentTermsValue: this.model.paymentTermsValue
        };

        this.model.ruleId = this.ruleId;

        //if currency changed unit price amount differ so get the price by customer price rule config and calculate the amount
        this.mapUnitPrice(unitpricechange, priceCardRequest);
      }
    }
    this.setPaymentTerms(this.model.customer);
  }

  async getTravelMatrixInfo(travelmatrixinfo, bookingId) {
    this.isBookingQuotationLoader = true;
    let response: QuotationTravelMatrixResponse;


    var travelMatrixRequest = new TravelMatrixRequest();

    travelMatrixRequest = {
      billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
      billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
      bookingId: bookingId,
      currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : 0,
      countyId: this.travelMatrixCountyId,
      cityId: this.travelMatrixCityId,
      provinceId: this.travelMatrixProvinceId,
      customerId: this.model.customer.id,
      ruleId: this.ruleId,
      quotationId: this.model.id
    };

    try {
      response = await this.travelMatrixService.getTravelMatrixData(travelMatrixRequest);
    }
    catch (e) {
      console.error(e);
    }

    if (response) {
      if (response.result == TravelMatrixResponseResult.Success) {
        //fetch the cusotmer price card details
        this.quotationTravelMatrix = response.travelMatrixDetails;

        this.calculateTravelMatrix();
        //open popup
        this.modelRef = this.modalService.open(travelmatrixinfo, { windowClass: "mdModelWidth", backdrop: 'static' });
        this.isBookingQuotationLoader = false;
      }
      else if (response.result == TravelMatrixResponseResult.MoreMatrixExists) {
        this.isBookingQuotationLoader = false;
        this.showWarning('QUOTATION.TITLE', 'QUOTATION.LBL_MORE_THAN_ONE_TRAVEL_MATRIX_FOUND')
      }
      else if (response.result == TravelMatrixResponseResult.NotFound) {
        this.isBookingQuotationLoader = false;
        this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_NO_TRAVEL_MATRIX');
      }
      else if (response.result == TravelMatrixResponseResult.RequestNotCorrectFormat) {
        this.isBookingQuotationLoader = false;
        this.showWarning('QUOTATION.TITLE', 'TRAVEL_MATRIX.MSG_REQUEST_FORMAT');
      }
      else if (response.result == TravelMatrixResponseResult.PriceCardNotCorrect) {
        this.isBookingQuotationLoader = false;
        this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_PRICE_CARD_WRONG');
      }
    }
  }

  //calculate the travel matrix mark up with fixed exchange rate
  calculateTravelMatrix() {

    //markup - cost* markupcost
    this.quotationTravelMatrix.busMarkUp = (this.quotationTravelMatrix.busCost *
      this.quotationTravelMatrix.markUpCost).toFixed(2);

    this.quotationTravelMatrix.trainMarkUp = (this.quotationTravelMatrix.trainCost *
      this.quotationTravelMatrix.markUpCost).toFixed(2);

    this.quotationTravelMatrix.taxiMarkUp = (this.quotationTravelMatrix.taxiCost *
      this.quotationTravelMatrix.markUpCost).toFixed(2);

    //source total - markup/exchange rate
    this.quotationTravelMatrix.busTotal = ((this.quotationTravelMatrix.busCost + parseFloat(this.quotationTravelMatrix.busMarkUp)) / this.quotationTravelMatrix.fixExchangeRate).toFixed(2);
    this.quotationTravelMatrix.trainTotal = ((this.quotationTravelMatrix.trainCost + parseFloat(this.quotationTravelMatrix.trainMarkUp)) / this.quotationTravelMatrix.fixExchangeRate).toFixed(2);
    this.quotationTravelMatrix.taxiTotal = ((this.quotationTravelMatrix.taxiCost + parseFloat(this.quotationTravelMatrix.taxiMarkUp)) / this.quotationTravelMatrix.fixExchangeRate).toFixed(2);

    this.quotationTravelMatrix.airMarkUp = (this.quotationTravelMatrix.airCost *
      this.quotationTravelMatrix.markUpCostAir).toFixed(2);

    this.quotationTravelMatrix.airTotal = ((this.quotationTravelMatrix.airCost + parseFloat(this.quotationTravelMatrix.airMarkUp)) / this.quotationTravelMatrix.fixExchangeRate).toFixed(2);

    // this.quotationTravelMatrix.hotelMarkUp = (this.quotationTravelMatrix.hotelCost *
    //   this.quotationTravelMatrix.markUpCost).toFixed(2);

    this.quotationTravelMatrix.hotelTotal = ((this.quotationTravelMatrix.hotelCost) /
      this.quotationTravelMatrix.fixExchangeRate).toFixed(2);

    this.quotationTravelMatrix.otherMarkUp = (this.quotationTravelMatrix.otherCost *
      this.quotationTravelMatrix.markUpCost).toFixed(2);

    this.quotationTravelMatrix.otherTotal = ((this.quotationTravelMatrix.otherCost + parseFloat(this.quotationTravelMatrix.otherMarkUp)) /
      this.quotationTravelMatrix.fixExchangeRate).toFixed(2);

    //bus+train+taxi - travel
    this.quotationTravelMatrix.landTravelCurrencyTotal = (this.quotationTravelMatrix.busCost +
      this.quotationTravelMatrix.trainCost + this.quotationTravelMatrix.taxiCost).toFixed(2);

    //bus+train+taxi  - source currency
    this.quotationTravelMatrix.landSourceCurrencyTotal = (parseFloat(this.quotationTravelMatrix.busTotal) +
      parseFloat(this.quotationTravelMatrix.trainTotal) + parseFloat(this.quotationTravelMatrix.taxiTotal)).toFixed(2);

    //sum of all total(air, hotel,other,travel)
    this.quotationTravelMatrix.grandTotal = Math.round((parseFloat(this.quotationTravelMatrix.landSourceCurrencyTotal) +
      parseFloat(this.quotationTravelMatrix.airTotal) + parseFloat(this.quotationTravelMatrix.hotelTotal) +
      parseFloat(this.quotationTravelMatrix.otherTotal))).toString();
  }

  //get customer price card info and show in popup
  async getCustomerPriceCardInfo(customerPriceCardInfo, bookingId) {
    await this.getCustomerPriceCardData(bookingId, customerPriceCardInfo);

  }

  async changeBillQuantityType(unitpricechange, bookingId, event) {
    if (this.model.service && this.model.service.id && this.model.service.id == APIService.Inspection) {
      let priceCardRequest = new CustomerPriceCardUnitPriceRequest();
      // reset rule
      this.ruleId = null;

      priceCardRequest = {
        billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
        billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
        bookingIds: this.model.orderList.filter(x => x.id == bookingId).map(x => x.id),
        currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : 0,
        ruleId: this.ruleId,
        quotationId: this.model.id,
        billQuantityType: event.id,
        invoiceType: this.model.paymentTerm,
        paymentTermsCount: this.model.paymentTermsCount,
        paymentTermsValue: this.model.paymentTermsValue
      };
      this.model.ruleId = this.ruleId;

      //if currency changed unit price amount differ so get the price by customer price rule config and calculate the amount
      this.mapUnitPrice(unitpricechange, priceCardRequest);
    }

  }

  //get customer price card details by id
  async getCustomerPriceCardData(bookingId: number, customerPriceCardInfo) {

    let response: QuotationPriceCard;

    //frame the request
    if (this.model.billingPaidBy && this.model.billingMethod &&
      this.model.billingPaidBy.id && this.model.billingMethod.id && bookingId > 0) {

      var priceCardRequest = new CustomerPriceCardRequest();

      priceCardRequest = {
        billMethodId: this.model.billingMethod && this.model.billingMethod.id ? this.model.billingMethod.id : 0,
        billPaidById: this.model.billingPaidBy && this.model.billingPaidBy.id ? this.model.billingPaidBy.id : 0,
        bookingId: bookingId,
        ruleId: this.ruleId,
        currencyId: this.model.currency && this.model.currency.id ? this.model.currency.id : 0,
        quotationId: this.model.id
      };

      try {

        response = await this.customerPriceCardService.getPriceCardData(priceCardRequest);

      }
      catch (e) {
        console.error(e);
      }

      if (response) {
        if (response.result == CustomerPriceCardResponseResult.Success) {
          //fetch the cusotmer price card details
          this.quotationCustomerPriceCard = response.priceCardDetails;
          //open popup
          this.modelRef = this.modalService.open(customerPriceCardInfo, { windowClass: "mdModelWidth", backdrop: 'static' });
        }
        else if (response.result == CustomerPriceCardResponseResult.MoreRuleExists) {
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.LBL_MORE_THAN_ONE_UNIT_PRICE_FOUND')
        }
        else if (response.result == CustomerPriceCardResponseResult.NotFound) {
          this.showWarning('QUOTATION.TITLE', 'QUOTATION.MSG_NO_CUS_UNITPRICE')
        }
      }
    }
  }


  //map the unit price for the booking based on the mapped price card(if it is avaialble)
  mapUnitPrice(unitpricechange, pricerequest) {
    //get the new unit data
    this.getNewUnitData(unitpricechange, pricerequest);
  }

  //get customer price card config(unit price) details by bookingIds, billingmethod and paid by.
  async getNewUnitData(unitpricechange, priceCardRequest) {

    this.unitPriceDetailModel = new Array<UnitPriceDetailModel>();

    //frame the request
    if (this.model.orderList && this.model.orderList.length > 0) {

      // && this.model.currency && this.model.currency.id


      var response: CustomerPriceCardUnitPriceResponse;

      this.isBookingQuotationLoader = true;
      this.bookingQuotationLoaderMsg = this.utility.textTranslate("QUOTATION.LBL_PROCESSING_UNIT_PRICE");

      //fetch the unitprice with booking id
      response = await this.customerPriceCardService.getUnitPriceByCustomerPriceCardRule(priceCardRequest);

      this.getUnitPriceResponse(response, unitpricechange);

    }
  }

  getUnitPriceResponse(response, unitpricechange) {

    this.multipleRulePriceCardList = [];
    //f result is success
    if (response.result == CustomerPriceCardResponseResult.Success) {

      if (response.unitPriceCardList && response.unitPriceCardList.length > 0
        && this.model.orderList && this.model.orderList.length > 0) {
        this.processUnitPriceSuccessResponse(response.unitPriceCardList, unitpricechange);

      }
    }
    else if (response.result == CustomerPriceCardResponseResult.NoQuotationCommonDataMatch) {
      this.showWarning('QUOTATION.TITLE', 'QUOTATION.LBL_NO_QUOTATION_COMMON_MATCH');
    }
    else if (response.result == CustomerPriceCardResponseResult.NotFound) {
      var bookingIds = response.unitPriceCardList.map(x => x.bookingId);
      this.removeUnitPriceValue(bookingIds);
    }
    this.isBookingQuotationLoader = false;
  }

  processUnitPriceSuccessResponse(unitPriceCardList, unitpricechange) {

    var bookingWithSingleRuleExists = false;

    //if unitpricecardlist(price card list available for the specific bookings)
    if (unitPriceCardList && this.model.orderList && unitPriceCardList.length > 0 && this.model.orderList.length > 0) {

      //take the unit price configured bookingids
      var bookingIds = unitPriceCardList.map(x => x.bookingId);

      //exclude the bookings from the order list
      var bookingIdsToRemove = this.model.orderList.filter(x => !bookingIds.includes(x.id));

      //remove the unit price value empty
      this.removeUnitPriceValue(bookingIdsToRemove);

      unitPriceCardList.forEach(unitPriceData => {

        //if booking available with single price card rule then simply assign the price card data
        if (unitPriceData.result == UnitPriceResponseResult.SingleRuleExists) {
          var order = this.model.orderList.find(x => x.id == unitPriceData.bookingId);
          //apply unit price data
          if (unitPriceData.priceCardIdList[0]) {
            this.ruleId = unitPriceData.priceCardIdList[0];
            this.model.ruleId = this.ruleId;

            this.travelIncludeUnitPrice(this.ruleId);
          }
          this.applyUnitPrice(order, unitPriceData);
          bookingWithSingleRuleExists = true;
        }
        //if more than one rule exists then add it into unit pricecard list
        else if (unitPriceData.result == UnitPriceResponseResult.MoreRuleExists) {
          this.multipleRulePriceCardList.push(unitPriceData);
        }

      });

      this.showMultiplePriceCardPopUp();

      this.applyCommonDataWithSingleRule(unitPriceCardList);

      //if unit price changed for existing booking and there is no multiple rule for the booking the show the unit price popup
      if (this.unitPriceDetailModel && this.unitPriceDetailModel.length > 0 && this.multipleRulePriceCardList.length == 0) {
        this.modelRef = this.modalService.open(unitpricechange, { windowClass: "mdModelWidth", backdrop: 'static' });
      }

      this.calculateAmountByBooking();

    }

  }

  removeUnitPriceValue(bookingIds) {
    var orderListToRemove = [];
    if (bookingIds.length > 0)
      orderListToRemove = this.model.orderList.filter(x => bookingIds.includes(x.id));
    else
      orderListToRemove = this.model.orderList;
    orderListToRemove.forEach(order => {
      if (order.orderCost) {
        order.orderCost.unitPrice = null;
        order.orderCost.inspFees = null;
        order.orderCost.quantity = null;
        order.orderCost.billedQtyType = null;
      }
    });
  }

  showMultiplePriceCardPopUp() {
    //show the multiple price card popup if more than one rule specified for the selected bookings
    if (this.multipleRulePriceCardList && this.multipleRulePriceCardList.length > 0) {
      //to make first element to checked true
      this.isBookingQuotationLoader = true;
      this.bookingQuotationLoaderMsg = this.utility.textTranslate("QUOTATION.LBL_PROCESSING_UNIT_PRICE");
      this.multipleRulePriceCardList.forEach(element => {
        if (element.customerPriceCardDetails && element.customerPriceCardDetails.length > 0)
          element.customerPriceCardDetails[0].isChecked = true;
      });

      this.customerPriceCardDialog = this.modalService.open(this.multiplecustomerpricemodel, { windowClass: "lgModelWidth", backdrop: 'static' });
      this.isBookingQuotationLoader = false;
    }
  }

  applyCommonDataWithSingleRule(unitPriceCardList) {
    //get the single rule bookings details
    var singleRuleBookings = unitPriceCardList.filter(x => x.result == UnitPriceResponseResult.SingleRuleExists);

    //if bookings with singlerule greater than 1 then apply common quotation data
    if (singleRuleBookings && singleRuleBookings.length > 0) {
      //apply billingmethod,billpaidby,currency from the first row of unit price
      if (singleRuleBookings[0] && singleRuleBookings[0].customerPriceCardDetails[0])
        this.applyCommonQuotationData(singleRuleBookings[0].customerPriceCardDetails[0]);
    }
  }

  applyUnitPrice(order, unitPriceData) {
    if (order && unitPriceData) {
      this.getBillingQuantityTypeList();
      var unitPrice = new UnitPriceDetailModel();
      //if new price not equal to old price add it into unitpriceDetailModel
      if (order.orderCost.unitPrice && unitPriceData.unitPrice && order.orderCost.unitPrice != unitPriceData.unitPrice) {
        unitPrice = {
          bookingId: order.id,
          oldUnitPrice: order.orderCost.unitPrice,
          newUnitPrice: unitPriceData.unitPrice,
          isSelect: false
        };
        //Model which is used to show in the unitpricechanged popup
        this.unitPriceDetailModel.push(unitPrice);
        if (unitPriceData.remarks) {
          this.model.apiInternalRemark = unitPriceData.remarks;
        }


      }
      else {
        order.orderCost.unitPrice = unitPriceData.unitPrice;
        order.orderCost.quantity = unitPriceData.totalBillQuantity;
        order.orderCost.billedQtyType = unitPriceData.billQuantityType;
        if (unitPriceData.remarks) {
          this.model.apiInternalRemark = unitPriceData.remarks;
        }
      }
    }
  }

  //apply biillingmethod,billingpaidby,currency
  applyCommonQuotationData(unitPriceCard) {
    //var unitPriceCard = unitPriceCardData.customerPriceCardDetails[0];
    if (!this.model.billingMethod && this.data.billingMethodList
      && this.data.billingMethodList.length > 0) {
      var billingMethod = this.data.billingMethodList.find(x => x.id == unitPriceCard.billingMethodId);
      if (billingMethod)
        this.model.billingMethod = billingMethod;
    }

    if (!this.model.billingPaidBy && this.data.billPaidByList
      && this.data.billPaidByList.length > 0) {
      var billingPaidBy = this.data.billPaidByList.find(x => x.id == unitPriceCard.billingPaidById);
      if (billingPaidBy)
        this.model.billingPaidBy = billingPaidBy;
    }

    this.setPaymentTerms(this.model.customer);

    if (!this.model.currency && this.data.currencyList
      && this.data.currencyList.length > 0) {
      var currency = this.data.currencyList.find(x => x.id == unitPriceCard.currencyId);
      if (currency)
        this.model.currency = currency;
    }

    if (this.data.billingEntities
      && this.data.billingEntities.length > 0) {
      var billingEntityId = this.data.billingEntities.find(x => x.id == unitPriceCard.billingEntityId);
      if (billingEntityId)
        this.model.billingEntity = billingEntityId.id;
    }

    if (this.data.paymentTermsValueList
      && this.data.paymentTermsValueList.length > 0) {
      const paymentTermValue = this.data.paymentTermsValueList.find(x => x.name == unitPriceCard.paymentTermsValue);
      if (paymentTermValue)
        this.model.paymentTermsValue = paymentTermValue.name;
    }

    if (this.data.paymentTermsValueList
      && this.data.paymentTermsValueList.length > 0) {
      const paymentTermCount = this.data.paymentTermsValueList.find(x => x.duration == unitPriceCard.paymentTermsCount);
      if (paymentTermCount)
        this.model.paymentTermsCount = paymentTermCount.duration;
    }

  }



  //submit the button assign the  new unit price vlaue
  clickNewUnitPriceChanged() {

    //loop the new unit price list
    this.unitPriceDetailModel.forEach(element => {

      //check box selected
      if (element.isSelect) {

        //assign new unit price to model
        this.model.orderList.find(x => x.id == element.bookingId).orderCost.unitPrice = element.newUnitPrice;
        this.modelRef.close();
      }
    });

    //all booking amount calculation
    this.calculateAmountByBooking();
  }

  isDisableSubmit(): boolean {

    // atleast one checkbox should select to enable the submit button
    return this.unitPriceDetailModel.filter(x => x.isSelect).length > 0;
  }

  public calculateAmountByBooking() {

    //if billing method is manday
    if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.Manday) {

      //calculate each booking total manday * unitprice assign to service fees
      this.model.orderList.forEach(item => {
        this.model.orderList.find(x => x.id == item.id).orderCost.inspFees = (item.orderCost.noOfManday) * (item.orderCost.unitPrice);
      });
    }
    //if billing method is sampling
    else if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.Sampling) {
      this.model.orderList.forEach(item => {
        //no calculation - unitprice as srevice fees(inspFees)
        this.model.orderList.find(x => x.id == item.id).orderCost.inspFees = item.orderCost.unitPrice;
      });
    }

    //if billing method is piece rate
    else if (this.model && this.model.billingMethod && this.model.billingMethod.id && this.model.billingMethod.id == this.BillingMethod.PieceRate) {
      this.model.orderList.forEach(item => {
        //no calculation - unitprice as srevice fees(inspFees)
        this.model.orderList.find(x => x.id == item.id).orderCost.inspFees = (item.orderCost.unitPrice) * (item.orderCost.quantity);
      });
    }

    this.totalCalculateAmount();
  }

  toggleExpandRowContainer(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.productList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="container' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getProductListByContainerAndBooking(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  getProductListByContainerAndBooking(rowItem) {
    this.bookingService.GetProductListByBookingAndContainer(rowItem.bookingId, rowItem.id)
      .subscribe(res => {
        if (res.result == 1) {
          rowItem.productList = res.bookingProductList;
          rowItem.isPlaceHolderVisible = false;
        }
      },
        error => {
          rowItem.isPlaceHolderVisible = false;

        });
  }


  toggleExpandRowProduct(event, index, rowItem, containerId, isContainerServiceType, bookingId) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];

    let triggerTable = event.target.parentNode.parentNode;
    var productIndex = index;
    if (containerId)
      productIndex = index + 'container' + containerId;
    var firstElem = document.querySelector('[data-expand-id="product' + productIndex + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getPoListByBookingAndProduct(rowItem, containerId, isContainerServiceType, bookingId);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get po list by booking and product
  getPoListByBookingAndProduct(rowItem, containerId, isContainerServiceType, bookingId) {
    if (!isContainerServiceType) {
      this.bookingService.GetPODetailsByBookingAndProduct(bookingId, rowItem.productRefId)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            rowItem.isPlaceHolderVisible = false;
          }

        },
          error => {
            rowItem.isPlaceHolderVisible = false;

          });
    }
    else {
      this.bookingService.GetPODetailsByBookingAndConatinerAndProduct(rowItem.bookingId, containerId, rowItem.id)
        .subscribe(res => {
          if (res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
            rowItem.isPlaceHolderVisible = false;
          }

        },
          error => {
            rowItem.isPlaceHolderVisible = false;

          });
    }
  }

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
  }

  setPaymentTerms(customer) {
    this.model.paymentTerm = null;
    if (this.model.billingPaidBy && this.model.billingPaidBy.id != QuotationBillPaidBy.customer)
      this.model.paymentTerm = PaymentTerm.PreInvoice;

    else if (this.model.customer) {
      var res = this.data.customerList.find(x => x.id == customer.id).invoiceType;
      if (res == undefined)
        this.model.paymentTerm = null;

      else
        this.model.paymentTerm = res;
    }
  }

  //set multiple price card ischecked property
  setPriceCardItem(item, customerPriceCardDetails) {
    customerPriceCardDetails.forEach(element => {
      element.isChecked = false;
    });
    item.isChecked = true;
  }

  //check if all the booking involved in the quotation has the common quotation data(billingmethod,billingpaidby,currency)
  CheckQuotationCommonData() {
    var quotationCommonData = true;
    var billingMethod = 0;
    var billingPaidBy = 0;
    var currencyId = 0;

    if (this.multipleRulePriceCardList && this.multipleRulePriceCardList.length > 0) {

      //if already bookingMethod,billingPaidBy,Currency set for the existing booking
      if (this.model.billingMethod && this.model.billingPaidBy && this.model.currency) {

        billingMethod = this.model.billingMethod.id;
        billingPaidBy = this.model.billingPaidBy.id;
        currencyId = this.model.currency.id;

        this.multipleRulePriceCardList.forEach(priceCard => {
          if (priceCard.customerPriceCardDetails && priceCard.customerPriceCardDetails.length > 0) {
            var samePriceCard = priceCard.customerPriceCardDetails.find(x => x.billingMethodId == billingMethod && x.billingPaidById == billingPaidBy && x.currencyId == currencyId && x.isChecked == true);
            if (!samePriceCard)
              quotationCommonData = false;
          }
        });

      }
      //common data not available
      else {
        //take the first price data
        var unitPriceCard = this.multipleRulePriceCardList[0];
        if (unitPriceCard) {
          var unitpriceCardData = unitPriceCard.customerPriceCardDetails.find(x => x.isChecked);
          if (unitpriceCardData) {
            billingMethod = unitpriceCardData.billingMethodId;
            billingPaidBy = unitpriceCardData.billingPaidById;
            currencyId = unitpriceCardData.currencyId;
            //check for remaining price card list
            var remainingPriceCardList = this.multipleRulePriceCardList.filter(x => x.bookingId != unitPriceCard.bookingId);
            if (remainingPriceCardList && remainingPriceCardList.length > 0) {
              remainingPriceCardList.forEach(priceCard => {
                if (priceCard.customerPriceCardDetails && priceCard.customerPriceCardDetails.length > 0) {
                  var samePriceCard = priceCard.customerPriceCardDetails.find(x => x.billingMethodId == billingMethod && x.billingPaidById == billingPaidBy && x.currencyId == currencyId && x.isChecked == true);
                  if (!samePriceCard)
                    quotationCommonData = false;
                }
              });
            }
          }
        }
      }
    }
    return quotationCommonData;
  }

  applyCustomerPriceCardUnitPrice(unitpricechange) {
    this.samplingUnitPriceList = [];
    if (this.CheckQuotationCommonData()) {
      //if multiple price card popup is visible then
      this.processMultiplePriceCardList(unitpricechange);

      if (this.customerPriceCardDialog) {
        this.customerPriceCardDialog.dismiss();
        this.customerPriceCardDialog = null;
      }
    }
    else {
      this.showWarning('QUOTATION.TITLE', 'QUOTATION.LBL_NO_QUOTATION_COMMON_MATCH');
    }

  }

  processMultiplePriceCardList(unitpricechange) {
    //if multiple price card popup is visible then
    if (this.multipleRulePriceCardList && this.multipleRulePriceCardList.length > 0) {
      //loop through the products
      this.multipleRulePriceCardList.forEach(priceCard => {
        //read customer price card details
        if (priceCard.customerPriceCardDetails) {
          //take the user selected price card from the popup
          var unitPriceData = priceCard.customerPriceCardDetails.find(x => x.isChecked);
          //take the booking data
          var order = this.model.orderList.find(x => x.id == priceCard.bookingId);

          if (unitPriceData && order) {
            let priceCardRequest = new CustomerPriceCardUnitPriceRequest();
            priceCardRequest = {
              billMethodId: unitPriceData.billingMethodId,
              billPaidById: unitPriceData.billingPaidById,
              bookingIds: this.model.orderList.map(x => x.id),
              currencyId: unitPriceData.currencyId,
              ruleId: unitPriceData.id,
              quotationId: this.model.id,
              billQuantityType: 0,
              invoiceType: this.model.paymentTerm,
              paymentTermsCount: this.model.paymentTermsCount,
              paymentTermsValue: this.model.paymentTermsValue
            };
            this.model.ruleId = this.ruleId;

            //if currency changed unit price amount differ so get the price by customer price rule config and calculate the amount
            this.mapUnitPrice(order, priceCardRequest);
          }

          this.processMultiplePriceCardOrders(unitPriceData, order, priceCard.bookingId, unitpricechange);
        }
      });
      //if billingmethod,billingpaidby,currency not set then set it from the value
      if (!(this.model.billingMethod && this.model.billingPaidBy && this.model.currency)
        && this.multipleRulePriceCardList[0].customerPriceCardDetails[0])
        var customerPriceCardDetail = this.multipleRulePriceCardList[0].customerPriceCardDetails.find(x => x.isChecked == true);
      if (customerPriceCardDetail)
        this.applyCommonQuotationData(customerPriceCardDetail);
    }
  }

  processMultiplePriceCardOrders(unitPriceData, order, bookingId, unitpricechange) {
    if (unitPriceData && order) {
      if (order.orderCost.unitPrice == null && !(this.model.id > 0)) {

        //
        if (unitPriceData.billingMethodId == BillingMethod.ManDay) {
          //if new price not equal to old price add it into unitpriceDetailModel
          this.ruleId = unitPriceData.id;
          this.applyUnitPrice(order, unitPriceData);
        }
        else if (unitPriceData.billingMethodId == BillingMethod.Sampling) {
          //if billing method is sampling then make the request to get sampling unit price
          var samplingUnitPricerequest = new SamplingUnitPriceRequest();

          samplingUnitPricerequest.bookingId = bookingId;
          samplingUnitPricerequest.priceCardId = unitPriceData.id;
          this.ruleId = unitPriceData.id;
          this.samplingUnitPriceList.push(samplingUnitPricerequest);

        }
        else if (unitPriceData.billingMethodId == BillingMethod.PerIntervention) {
          this.ruleId = unitPriceData.id;
          this.applyUnitPrice(order, unitPriceData);
        }
        else if (unitPriceData.billingMethodId == BillingMethod.PieceRate) {
          this.ruleId = unitPriceData.id;
          this.applyUnitPrice(order, unitPriceData);
        }
        //get the sampling unit price data per booking
        if (this.samplingUnitPriceList && this.samplingUnitPriceList.length > 0) {
          this.getSamplingUnitData(unitpricechange);
        }

        //if unit price changed for existing booking and there is no multiple rule for the booking the show the unit price popup
        if (this.unitPriceDetailModel && this.unitPriceDetailModel.length > 0 && this.samplingUnitPriceList.length == 0) {
          this.modelRef = this.modalService.open(unitpricechange, { windowClass: "mdModelWidth", backdrop: 'static' });
        }
        //call travel message
        this.travelIncludeUnitPrice(this.ruleId);
        this.calculateAmountByBooking();

      }
    }
  }

  //get sampling unit price for the selected bookings
  async getSamplingUnitData(unitpricechange) {

    this.unitPriceDetailModel = new Array<UnitPriceDetailModel>();
    var response: SamplingUnitPriceResponse;

    if (this.samplingUnitPriceList && this.samplingUnitPriceList.length > 0) {
      response = await this.customerPriceCardService.getSamplingUnitPriceData(this.samplingUnitPriceList);
      if (response) {
        if (response.result == SamplingPriceResponseResult.Success)
          this.processSampleUnitPriceData(response, unitpricechange);
      }
    }



  }

  processSampleUnitPriceData(response, unitpricechange) {

    var unitPrice = new UnitPriceDetailModel();
    this.model.orderList.forEach(order => {

      //unitPriceList - get unit price compare with booking id from response and model.orderlist
      var unitPriceData = response.samplingUnitPriceList.find(x => x.bookingId == order.id);

      //new quotation if unit price null we bind the value from db
      if (unitPriceData && order.orderCost.unitPrice == null && !(this.model.id > 0)) {

        this.applyUnitPrice(order, unitPriceData);

        //if unit price changed for existing booking and there is no multiple rule for the booking the show the unit price popup
        if (this.unitPriceDetailModel && this.unitPriceDetailModel.length > 0) {
          this.modelRef = this.modalService.open(unitpricechange, { windowClass: "mdModelWidth", backdrop: 'static' });
        }

      }

    });

    this.calculateAmountByBooking();

    if (this.customerPriceCardDialog) {
      this.customerPriceCardDialog.dismiss();
      this.customerPriceCardDialog = null;
    }

  }

  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
  }

  //get the skip quotation sent to client checkpoint
  async getSkipQuotationSendToClientCheckpoint(bookingIdList, customerId) {

    var res = false;
    this.quotationCheckpointRequest = { bookingIdList: bookingIdList, customerId: customerId };

    res = await this.service.getSkipQuotationSentToClient(this.quotationCheckpointRequest);
    return res;
  }

  //update isToForward and skipQuotationSendToClientCheckpoint based on conditions
  UpdateCheckboxValue(type) {
    if (type == "isForwardToManager") {
      this.model.skipQuotationSentToClient = false;
    }

    else {
      this.model.isToForward = false;
    }
  }

  async getCalculatedWorkingHours() {
    this.refreshLoadng = true;
    for (var i = 0; i < this.model.orderList.length; i++) {
      var bookingId = this.model.orderList[i].id;
      let res = await this.service.getCalculatedWorkingHours(bookingId);

      if (res.result == CalculatedWorkingHoursResult.success) {
        this.calculatedWorkingHourMessage = null;
        this.model.orderList.filter(x => x.id == bookingId).forEach(x => {
          x.orderCost.calculatedWorkingHours = res.calculatedWorkingHours;
          x.orderCost.calculatedManday = res.calculatedManday;
        });
      }
      else if (res.result == CalculatedWorkingHoursResult.prodCatSub3NotMapped) {
        this.calculatedWorkingHourMessage = "Product sub category 3 not mapped with all products";
        this.model.orderList.filter(x => x.id == bookingId).forEach(x => {
          x.orderCost.calculatedWorkingHours = 0;
          x.orderCost.calculatedManday = 0;
        });
      }
      else if (res.result == CalculatedWorkingHoursResult.unitNotPcs) {
        this.calculatedWorkingHourMessage = "All product’s unit are not Pieces";
        this.model.orderList.filter(x => x.id == bookingId).forEach(x => {
          x.orderCost.calculatedWorkingHours = 0;
          x.orderCost.calculatedManday = 0;
        });
      }
    }

    this.refreshLoadng = false;
  }

  calculateModelOpen(calculateworkinghour, bookingId) {
    this.bookingId = bookingId;
    this.modelRef = this.modalService.open(calculateworkinghour, { windowClass: "smModelWidth", backdrop: 'static' });
  }

  async refreshCalculatedWorkingHours() {
    this.refreshLoadng = true;
    let res = await this.service.saveWorkingManday(this.bookingId);

    if (res.saveResult == CalculateManDaySaveResult.Success) {
      this.showSuccess('QUOTATION.TITLE', 'QUOTATION.LBL_WORKING_MD_SAVE_SUCCESS');
    }

    if (res.result == CalculatedWorkingHoursResult.success) {
      this.calculatedWorkingHourMessage = null;
      this.model.orderList.filter(x => x.id == this.bookingId).forEach(x => {
        x.orderCost.calculatedWorkingHours = res.calculatedWorkingHours;
        x.orderCost.calculatedManday = res.calculatedManday;
      });
    }
    else if (res.result == CalculatedWorkingHoursResult.prodCatSub3NotMapped) {
      this.calculatedWorkingHourMessage = "Product sub category 3 not mapped with all products";
      this.model.orderList.filter(x => x.id == this.bookingId).forEach(x => {
        x.orderCost.calculatedWorkingHours = 0;
        x.orderCost.calculatedManday = 0;
      });
    }
    else if (res.result == CalculatedWorkingHoursResult.unitNotPcs) {
      this.calculatedWorkingHourMessage = "All product’s unit are not Pieces";
      this.model.orderList.filter(x => x.id == this.bookingId).forEach(x => {
        x.orderCost.calculatedWorkingHours = 0;
        x.orderCost.calculatedManday = 0;
      });
    }

    this.refreshLoadng = false;
  }

  //is travel amount include value get and display
  async travelIncludeUnitPrice(ruleId) {

    if (ruleId > 0) {
      let res = await this.service.getPriceCardTravel(ruleId);

      if (res.result == PriceCardTravelResult.Success) {

        if (res.isTravelInclude)
          this.travelTextMessage = this.utility.textTranslate("QUOTATION.MSG_INCLUDED");
        else
          this.travelTextMessage = this.utility.textTranslate("QUOTATION.MSG_NOT_INCLUDED");
        this.isTravelWarningShow = true;

      }
      else {
        this.isTravelWarningShow = false;
      }
    }
    else {
      this.isTravelWarningShow = false;
    }
  }

  closeMessage() {
    this.isHideMessage = !this.isHideMessage;
  }

  downloadBookingFileAttachment(fileUrl) {
    if (fileUrl)
      window.open(fileUrl);
  }

  factoryBookingInfo() {
    let request = new FactoryBookingInfoRequest();
    request.bookingIds = this.model.orderList.map(x => x.id);
    request.factoryId = this.model.factory.id;
    this.isBookingQuotationLoader = true;
    this.model.factoryBookingInfoList = [];
    this.service.factoryBookingInfo(request)
      .subscribe(res => {
        if (res.result == FactoryBookingInfoResult.Success) {
          for (let item of res.factoryBookingInfolst) {
            let factoryBookingInfo: FactoryBookingInfo = {
              bookingId: item.bookingId,
              customerName: item.customerName,
              serviceDate: item.serviceDate,
              productCount: item.productCount,
              reportCount: item.reportCount,
              sampleSize: item.sampleSize,
              manday: item.manday,
              inspectionFee: item.inspectionFee,
              travelCost: item.travelCost,
              otherCost: item.otherCost,
              total: item.inspectionFee + item.travelCost + item.otherCost,
              averageManday: item.manday > 0 ? (item.inspectionFee + item.travelCost + item.otherCost) / item.manday : 0,
              suggestedManday: item.suggestedManday,
              isQuotation: this.model.orderList.find(x => x.id == item.bookingId) ? true : item.isQuotation,
              serviceTypeName: item.serviceTypeName
            }
            this.model.factoryBookingInfoList.push(factoryBookingInfo);
          }
          this.totalFactoryBookingDetail();
        }
        else
          this.model.factoryBookingInfoList = [];
        this.isBookingQuotationLoader = false;
      });
  }

  totalFactoryBookingDetail() {
    this.factoryBookingDetail.totalBooking = this.model.factoryBookingInfoList.length;
    this.factoryBookingDetail.totalItem = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.productCount, 0);
    this.factoryBookingDetail.totalReport = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.reportCount, 0);
    this.factoryBookingDetail.totalSampleSize = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.sampleSize, 0);
    this.factoryBookingDetail.totalManday = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.manday, 0);
    this.factoryBookingDetail.totalInspectionFee = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.inspectionFee, 0);
    this.factoryBookingDetail.totalTravelCost = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.travelCost, 0);
    this.factoryBookingDetail.totalOtherCost = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.otherCost, 0);
    this.factoryBookingDetail.totalSuggestedManday = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.suggestedManday, 0);
    this.factoryBookingDetail.totalFee = this.factoryBookingDetail.totalInspectionFee + this.factoryBookingDetail.totalTravelCost + this.factoryBookingDetail.totalOtherCost;
    this.factoryBookingDetail.totalAverageManday = this.factoryBookingDetail.totalManday > 0 ? this.factoryBookingDetail.totalFee / this.factoryBookingDetail.totalManday : 0;
    this.data.factoryBookingDetail = this.factoryBookingDetail;
  }

  GetTotalSuggestedMD() {
    this.factoryBookingDetail.totalSuggestedManday = this.model.factoryBookingInfoList.reduce((sum, qty) => sum + qty.suggestedManday, 0);
    this.data.factoryBookingDetail = this.factoryBookingDetail;
  }
  async getSupplierAndFactoryGrade() {
    setTimeout(() => {
      let request = new SupplierGradeRequest();
      request.customerId = this.model.customer.id;
      request.bookingIds = this.model.orderList.map(x => x.id);
      request.supplierId = this.model.supplier.id;
      this.getSupplierGrade(request, SupplierType.Supplier);

      request.supplierId = this.model.factory.id;
      this.getSupplierGrade(request, SupplierType.Factory);
    }, 1000);

  }

  async getSupplierGrade(request: SupplierGradeRequest, type: SupplierType) {

    if (request.customerId && request.supplierId && request.bookingIds && request.bookingIds.length > 0) {
      let res: any;
      try {
        res = await this.supplierService.getSupplierGrade(request)
      }
      catch (e) {
        console.error(e);
        this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_UNKNOWN');
      }

      if (res.result == SupplierGradeResult.Success || res.result == SupplierGradeResult.NotFound) {
        if (type == SupplierType.Factory) {
          this.factoryGrade = res.grade;
        }
        else if (type = SupplierType.Supplier) {
          this.supplierGrade = res.grade;
        }
      }
      else {
        switch (res.result) {
          case SupplierGradeResult.CustomerRequired:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CUSTOMER_REQ');
          case SupplierGradeResult.SupplierRequired:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_SUPP_REQ');
          case SupplierGradeResult.BookingIdsRequired:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_BOOKING_REQUIRED');
        }
      }

    }
  }

  validateDecimal(item, itemFeeType): void {
    let numArr: Array<string>;

    if (item[itemFeeType]) {
      if (!this.validateNegativeValue(item[itemFeeType])) {
        numArr = item[itemFeeType].toString().split(".");

        var value = this.getDecimalValuewithTwoDigits(numArr);

        setTimeout(() => {
          if (value) item[itemFeeType] = value;
        }, 10);
      } else {
        setTimeout(() => {
          item[itemFeeType] = null;
        }, 10);
      }
    }
    else {
      setTimeout(() => {
        item[itemFeeType] = 0;
      }, 10);
    }
  }
  validateNegativeValue(value) {
    if (parseInt(value) < 0) return true;

    return false;
  }

  getDecimalValuewithTwoDigits(numArr) {
    if (numArr.length > 1) {
      var afterDot = numArr[1];

      if (afterDot.length > 2) {
        return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
      }
    }
  }

  numericValidation(event) {
    this.utility.numericValidation(event, 10);
  }
  onChangePaymentType(event) {
    if (event) {
      this.model.paymentTermsValue = event.name;
      this.model.paymentTermsCount = event.duration;
    }
  }

  setFactoryBookingInfo(order: Order) {
    const factoryBooking = this.model.factoryBookingInfoList?.find(x => x.bookingId == order.id);
    if (factoryBooking) {
      factoryBooking.manday = order.orderCost.noOfManday;
      factoryBooking.inspectionFee = order.orderCost.inspFees;
      factoryBooking.travelCost = order.orderCost.travelAir + order.orderCost.travelLand + order.orderCost.travelHotel;
      factoryBooking.total = factoryBooking.inspectionFee + factoryBooking.travelCost + factoryBooking.otherCost;
      factoryBooking.averageManday = factoryBooking.manday > 0 ? (factoryBooking.total) / factoryBooking.manday : 0;
      this.totalFactoryBookingDetail();
    }
  }
}
