import { Component } from '@angular/core';
import { CustomerResult } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper, Validator } from '../../common';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomerCheckPointService } from 'src/app/_Services/customer/customercheckpoint.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { CustomerPriceCardService } from 'src/app/_Services/customer/customerpricecard.service';
import {
  MasterCustomerPriceCard, CustomerPriceCard, SaveCustomerPriceCardResponse, CustomerPriceCardResponseResult,
  EditCustomerPriceCardResponse,
  InvoiceRequestType,
  PriceInvoiceRequest,
  InvoiceFeesFrom, InvoiceBillingTo, PriceSubCategory, PriceSpecialRule, PriceComplexType
} from 'src/app/_Models/customer/customer-price-card.model';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { first, takeUntil } from 'rxjs/operators';
import { LocationService } from 'src/app/_Services/location/location.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { TravelMatrixService } from 'src/app/_Services/invoice/travelmatrix.service';
import { APIService, BillingMethod, EntityFeature, InterventionType, Url } from "src/app/components/common/static-data-common"
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { ServiceTypeRequest } from 'src/app/_Models/reference/servicetyperequest.model';
import { truncateWithEllipsis } from '@amcharts/amcharts4/.internal/core/utils/Utils';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { Subject } from 'rxjs/internal/Subject';
import { DataSourceResult } from 'src/app/_Models/kpi/datasource.model';

@Component({
  selector: 'app-edit-customer-price-card',
  templateUrl: './edit-customer-price-card.component.html',
  styleUrls: ['./edit-customer-price-card.component.scss']
})
export class EditCustomerPriceCardComponent extends DetailComponent {
  masterData: MasterCustomerPriceCard;
  model: CustomerPriceCard;
  selectedPageSize;
  copyId: number;
  ruleValidators: Array<any> = [];
  subCategoryValidators: Array<any> = [];
  invRequestType= InvoiceRequestType;
  invFeesFrom=InvoiceFeesFrom;
  billingMethod = BillingMethod;
  interventionType = InterventionType;
  billingTo = InvoiceBillingTo;
  invoiceFeesTypes:any;
  componentDestroyed$: Subject<boolean> = new Subject();
  private _translate: TranslateService;
  private _toastr: ToastrService;
  constructor(private jsonHelper: JsonHelper,
    public validator: Validator,
    translate: TranslateService,
    toastr: ToastrService,
    router: Router,
    route: ActivatedRoute,
    private activatedRoute: ActivatedRoute,
    public customerCheckPointService: CustomerCheckPointService,
    public productManagementService: ProductManagementService,
    public bookingService: BookingService,
    public referenceService:ReferenceService,
    public customerService: CustomerService,
    public locationService: LocationService,
    public customerPriceCardService: CustomerPriceCardService,
    public supplierService: SupplierService,
    public utility: UtilityService,
    public subRoute: Router,
    public travelMatrixService: TravelMatrixService) {
    super(router, route, translate, toastr);
    this.masterData = new MasterCustomerPriceCard();
    this.model = new CustomerPriceCard();
    this.model.serviceId = APIService.Inspection;
    this.validator.setJSON('customer/edit-customerpricecard.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.copyId = parseInt(this.activatedRoute.snapshot.paramMap.get("newid"));
    this._toastr=toastr;
    this._translate=translate;

  }

  onInit(id?: number) {
   
    this.getBillingToList();
    this.getEntityFeatureList();
    this.getCustomerListByUserType();
    this.getServiceList();
    this.getProductCategoryList();
    this.getCurrencyList();  
    this.getInvoiceOfficeList();
    this.getCountryList();
    this.getTarifftypes();
    this.getInvoicePaymentTypeList();
    this.getBillingEntityList();
    this.getInvoiceFeesTypeList();
    this.getBillingFrequencyList();
    this.getInspectionLocations();
    this.getBillingQuantityTypeList();
    this.getInterventionTypeList();
    
    this.masterData.maxStyleTypeList=[{id:1,name:'Day'},{id:1,name:'Month'},{id:1,name:'Week'}];
  

    this.model.sampleSizeBySet = false;
    if (id && id > 0) {
     
      this.model.id = id;
      this.editData(this.model.id);
    }
    else {
      if(this.model.serviceId){
      this.getServiceTypesList();
      }
    }
  }


  //get tariff types list
  getTarifftypes() {
    this.masterData.tariffTypeLoading = true;

    this.travelMatrixService.getTarifftypes()
      .pipe()
      .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.masterData.tariffTypeList = response.dataSourceList;
            }
            this.masterData.tariffTypeLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.tariffTypeLoading = false;
        });
  }

  onChangePerDay(){

    if(this.model.ruleList.length>0)
    {
      this.model.ruleList[0].max_Style_Per_Week=0;
      this.model.ruleList[0].max_Style_per_Month=0;
    }
  }

  onChangePerMonth(){

    if(this.model.ruleList.length>0)
    {
      this.model.ruleList[0].max_Style_Per_Day=0;
      this.model.ruleList[0].max_Style_Per_Week=0;
    }
  }

  onChangePerWeek(){

    if(this.model.ruleList.length>0)
    {
      this.model.ruleList[0].max_Style_Per_Day=0;
      this.model.ruleList[0].max_Style_per_Month=0;
    }
  }


  //get customer list
  getCustomerListByUserType() {
    this.masterData.customerLoading = true;
    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.customerLoading = false;
      }
    );
  }

  onchangeServiceType(event){
    this.model.serviceTypeIdList = [];
if(event){
  this.setInvoiceFeesTypeList(event.id);
  this.getServiceTypesList();
}
else{
  this.masterData.serviceTypeList= [];
  this.getServiceList();
}
   
  }

  setInvoiceFeesTypeList(servicTypeId){

    if (servicTypeId== APIService.Audit)
    {
      this.masterData.invoiceFeesTypeList= [...this.invoiceFeesTypes.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable && x.id!=InvoiceFeesFrom.Carrefour)]; 

      this.masterData.travelExpenseFeesList= [...this.invoiceFeesTypes.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable  && x.id!=InvoiceFeesFrom.Carrefour)]; 

    }
    else
    {
      this.masterData.invoiceFeesTypeList= [...this.invoiceFeesTypes.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable)]; 

      this.masterData.travelExpenseFeesList= [...this.invoiceFeesTypes.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable)]; 

    }
  }



  //customer list success response
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.masterData.customerList = response.customerList;
      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'EDIT_CUSTOMER_PRICE_CARD.MSG_CUSTOMER_LIST_NOT_FOUND');
      }
    }
    this.masterData.customerLoading = false;
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


  getInvoiceRequestTypeList() 
  {
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

  getInvoiceBankList(billingEntity) 
  {
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

  getBillingEntityList() 
  {
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

  isCheckInvoiceConfigure()
  {
    this.masterData.hotelfeeVisible = false;
    if(this.model.isInvoiceConfigured)
    {
      // by default selection
      if(this.model.id==undefined || this.model.id==0)
      {
        // if manday then set default as quotation
        if(this.model.billingMethodId == this.billingMethod.ManDay)
        {
          this.model.invoiceInspFeeFrom=this.invFeesFrom.Quotation;
        }       

        this.model.invoiceHotelFeeFrom=this.invFeesFrom.Quotation;
        this.model.invoiceDiscountFeeFrom=this.invFeesFrom.NotApplicable;
        this.model.invoiceTravelExpense=this.invFeesFrom.Quotation;
        this.model.invoiceOtherFeeFrom=this.invFeesFrom.Quotation;
      }
      //if existing price card without invoice details
      else {
        this.model.invoiceRequestType = this.invRequestType.NotApplicable;

        // if manday then set default as quotation
        if(this.model.billingMethodId == this.billingMethod.ManDay)
        {
          this.model.invoiceInspFeeFrom=this.invFeesFrom.Quotation;
        }    
      }
    }
    else
    {
      if(this.model.id==undefined || this.model.id==0)
      {        
      this.model.invoiceHotelFeeFrom=null;
      this.model.invoiceDiscountFeeFrom=null;
      this.model.invoiceTravelExpense=null;
      this.model.invoiceOtherFeeFrom=null;
      this.model.invoiceInspFeeFrom=null;
      }
    }                
  }

  getInvoiceFeesTypeList() 
  {
    this.masterData.invoiceFeesLoading = true;
    this.referenceService.getInvoiceFeesTypeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.invoiceFeesLoading = false;         

       this.invoiceFeesTypes=[...response.dataSourceList];
            
        if(this.model.serviceId == APIService.Audit)
        {          
               // copy the data based on the condition
               this.masterData.invoiceFeesTypeList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable && x.id!=InvoiceFeesFrom.Carrefour)]; 

               this.masterData.travelExpenseFeesList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable  && x.id!=InvoiceFeesFrom.Carrefour)]; 

               this.masterData.hotelFeesList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.Carrefour)]; 

               this.masterData.discountFeesList=[...this.masterData.hotelFeesList.filter(x=>x.id!=InvoiceFeesFrom.Invoice)]; 
       
               this.masterData.otherFeesList=[...this.masterData.hotelFeesList.filter(x=>x.id!=InvoiceFeesFrom.Invoice)];

        }
        else
        {
                  // copy the data based on the condition
                  this.masterData.invoiceFeesTypeList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable)]; 

                  this.masterData.travelExpenseFeesList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.NotApplicable)]; 

        this.masterData.hotelFeesList= [...response.dataSourceList.filter(x=>x.id!=InvoiceFeesFrom.Carrefour)]; 

        this.masterData.discountFeesList=[...this.masterData.hotelFeesList.filter(x=>x.id!=InvoiceFeesFrom.Invoice)]; 

        this.masterData.otherFeesList=[...this.masterData.hotelFeesList.filter(x=>x.id!=InvoiceFeesFrom.Invoice)];

        }

        },
        error => {
          this.setError(error);
          this.masterData.invoiceFeesLoading = false;
        });
  } 

  getInvoiceOfficeList() 
  {
    this.masterData.invoiceOfficeLoading = true;
    this.referenceService.getInvoiceOfficeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.invoiceOfficeList = response.dataSourceList;
          this.masterData.invoiceOfficeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.invoiceOfficeLoading = false;
        });
  } 

  getInvoicePaymentTypeList() 
  {
    this.masterData.invoicePaymentLaoding = true;
    this.referenceService.getInvoicePaymentTypeList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.invoicePaymentTypeList = response.dataSourceList;
          this.masterData.invoicePaymentLaoding = false;
        },
        error => {
          this.setError(error);
          this.masterData.invoicePaymentLaoding = false;
        });
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
              // by default
              if(this.model.id==undefined || this.model.id==0)
              {
                this.model.invoiceRequestType=this.invRequestType.NotApplicable;
              }
          },
          error => {
            this.setError(error);
            this.masterData.invoiceRequestLoading = false;
          });
    }
 

    toggleCommonInvoiceSection() {
      this.masterData.toggleInvoiceSection = !this.masterData.toggleInvoiceSection;
    }

    toggleCommonCategorySection() {
      this.masterData.toggleCategorySection = !this.masterData.toggleCategorySection;
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

  getProvinceList() {
    var id = this.model.factoryCountryIdList.length == 1 ?
      this.model.factoryCountryIdList[0] : 0;
    if (id > 0) {
      this.masterData.isProvinceShow = true;
      this.masterData.provinceLoading = true;
      this.locationService.getprovincebycountryid(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success) {
              this.masterData.provinceList = response.data;
            }
            this.masterData.provinceLoading = false;
          },
          error => {
            this.setError(error);
            this.masterData.provinceLoading = false;
          });
    }
    else {
      this.masterData.isProvinceShow = false;
    }
  }

  //get province list on country change
  setProvinceCountryChange() {

    this.masterData.provinceList = null;
    this.model.factoryProvinceIdList = null;
    this.getProvinceList();

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
  //getServiceTypesList
  getServiceTypesList() {
    
    this.masterData.serviceTypeLoading = true;
    let request = this.generateServiceTypeRequest();
    this.referenceService.getServiceTypes(request)
    .pipe()
    .subscribe(
        response => {
          if (response) {
            if (response.result == ResponseResult.Success) {
              this.masterData.serviceTypeList = response.serviceTypeList;
            }
            else{
              this.masterData.serviceTypeList = [];
            }
            this.masterData.serviceTypeLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.serviceTypeLoading = false;
        });
  }

  generateServiceTypeRequest() {
    var serviceTypeRequest = new ServiceTypeRequest();
    serviceTypeRequest.customerId = this.model.customerId ?? 0;
    serviceTypeRequest.serviceId = this.model.serviceId ?? 0;
    //serviceTypeRequest.businessLineId=this.model.businessLine;
    return serviceTypeRequest;
  }

  //get supplier list on customer change
  setSupplierCustomerChange() {

    this.masterData.supplierLoading = true;

    this.masterData.supplierList = null;
    this.model.supplierIdList = new Array<number>();
    this.model.serviceTypeIdList = [];
    this.model.billingMethodId=null;
    this.model.ruleList=[];
    this.model.subCategoryList=[];
    this.ruleValidators=[];
    this.subCategoryValidators=[];
    this.model.productCategoryIdList=[];  
    this.model.productSubCategoryIdList=[];

    this.getBillingMethodList();
    this.getSupplierList();
    this.getCustomerDepartments();
    this.getCustomerBrands();
    this.getCustomerBuyers();
    this.getCustomerProductCategoryList();
   // this.getCustomerProductSubCategoryList();
    this.getCustomerPriceCategoryList();
    this.getCustomerPriceHolidayList();
    this.getCustomerAddressList();
    this.getCustomerPriceData();
    this.getCustomerContactList();
    this.getInvoiceRequestList();

    if(this.model.serviceId){
      this.getServiceTypesList();
    }
  }

  
  onChangeCommonAddress(event)
  {
    if(event)
    {
       this.model.invoiceRequestAddress=event.name;
       this.applyCommonItems();
    }    
  }

  setAddress(event,row){
   if(event && row)
   {
    row.billedAddress=event.name;
   }
  }

  onChangeCommonAddressBox()
  {
    this.applyCommonItems();      
  }

  onChangeCommonContacts()
  {
    this.applyCommonItems();    
  }

  onChangeBilledName()
  {
    this.applyCommonItems();   
  }

  onChangeCommonInvoiceSelectAll(){
    this.applyCommonItems();
  }

  onChangeCommonSubCategorySelectAll(){
    if(!this.model.subCategorySelectAll)
    {
      this.getCustomerProductSub2CategoryList();
    }
    else{
      this.model.subCategoryList=[];  
      this.subCategoryValidators=[];
    }
  }

  onProductivityChange(){
    this.applySubCategoryCommonItems();
  }

onReportChange(){
  this.applySubCategoryCommonItems();
}

onBufferChange(){
  this.applySubCategoryCommonItems();
}

onUnitPriceChange(){
  this.applySubCategoryCommonItems();
}

  onchangeBillingTo(event){

    if(event)
    {
   
       if(event.id!=this.billingTo.Customer ||event.id!=this.billingTo.SGTCustomer)
       {
         this.model.invoiceRequestType=this.invRequestType.NotApplicable;
         this.model.invoiceRequestList=[];
       }      
    }
  }


  applyCommonItems()
  {
    if(this.model.invoiceRequestSelectAll)
    {  

       for (let index = 0; index < this.model.invoiceRequestList.length; index++) {
        this.model.invoiceRequestList[index].billedAddress=this.model.invoiceRequestAddress;
        this.model.invoiceRequestList[index].billedAddressId=this.model.customerAddressId;
        this.model.invoiceRequestList[index].billedName=this.model.invoiceRequestBilledName;
        this.model.invoiceRequestList[index].invoiceRequestContactList=this.model.invoiceRequestContact;  
        this.model.invoiceRequestList[index].isCommon=true;      
       }
    }
    else
    {
      for (let index = 0; index < this.model.invoiceRequestList.length; index++) {         
         this.model.invoiceRequestList[index].isCommon=false;      
       }
    }
  }


  applySubCategoryCommonItems()
  { 
       for (let index = 0; index < this.model.subCategoryList.length; index++) {
         // apply only man day common items
        this.model.subCategoryList[index].mandayBuffer=this.model.mandayBuffer;
        this.model.subCategoryList[index].unitPrice=this.model.unitPrice;
        this.model.subCategoryList[index].mandayProductivity=this.model.mandayProductivity;
        this.model.subCategoryList[index].mandayReports=this.model.mandayReports;  

        this.model.subCategoryList[index].aqL_QTY_8=this.model.quantity8;  
        this.model.subCategoryList[index].aqL_QTY_13=this.model.quantity13; 
        this.model.subCategoryList[index].aqL_QTY_20=this.model.quantity20;  
        this.model.subCategoryList[index].aqL_QTY_32=this.model.quantity32; 
        this.model.subCategoryList[index].aqL_QTY_50=this.model.quantity50; 
        this.model.subCategoryList[index].aqL_QTY_80=this.model.quantity80;
        this.model.subCategoryList[index].aqL_QTY_125=this.model.quantity125; 
        this.model.subCategoryList[index].aqL_QTY_200=this.model.quantity200; 
        this.model.subCategoryList[index].aqL_QTY_315=this.model.quantity315; 
        this.model.subCategoryList[index].aqL_QTY_500=this.model.quantity500; 
        this.model.subCategoryList[index].aqL_QTY_800=this.model.quantity800; 
        this.model.subCategoryList[index].aqL_QTY_1250=this.model.quantity1250;        
    }    
  }
 

  checkIfAllItemSelected() {
    this.model.invoiceRequestSelectAll = this.model.invoiceRequestList.every(function (item: any) {
      return item.isCommon == true;
    });
  }

  selectAllSubCategory() {
    for (var i = 0; i < this.model.subCategoryList.length; i++) {
      this.model.subCategoryList[i].isCommon = this.model.isSelectAllSubCategory;
    }
  }

  checkIfAllSubcategoryItemSelected() {
    this.model.isSelectAllSubCategory = this.model.subCategoryList.every(function (item: any) {
      return item.isCommon;
    })
  }

  onChangePaymentType(event)
  {
     if(event)
     {
       this.model.paymentTerms=event.name;
       this.model.paymentDuration=event.duration;
     }
  }

  addSpecialRule() {
    let specialRow: PriceSpecialRule = {      
       id :0,
       cuPriceCardId :0,    
       mandayProductivity :0,
       mandayReports: 0,
       unitPrice: 0,    
       pieceRate_Billing_Q_Start: 0,
       piecerate_Billing_Q_End: 0,
       additionalFee: 0,
       piecerate_MinBilling: 0,
       perInterventionRange1: 0,
       perInterventionRange2: 0,
       max_Style_Per_Day: 0,
       max_Style_Per_Week: 0,
       max_Style_per_Month: 0,
       interventionfee: 0  
    
    };   
    this.model.ruleList.push(specialRow);
    this.ruleValidators.push({ rule: specialRow, validator: Validator.getValidator(specialRow, 
      "customer/edit-customerpricecard-rule.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
 
  }

  deleteSpecialRule(index) 
  {    
    this.model.ruleList.splice(index); 
    this.ruleValidators.splice(index, 1);
  }

  onChangeInvRequestType(event){
    if(event)
    {   
      switch (event.id) {
        case this.invRequestType.Brand:
          this.addInvoiceRequest(this.masterData.customerBrandList,event.id);
          break;

          case this.invRequestType.Department:
            this.addInvoiceRequest(this.masterData.customerDepartmentList,event.id);
          break;

          case this.invRequestType.Buyer:
            this.addInvoiceRequest(this.masterData.customerBuyerList,event.id);
          break;

          case this.invRequestType.NotApplicable:
            this.model.invoiceRequestList=[];
            break;   

            case this.invRequestType.ProductCategory:
              this.addInvoiceRequest(this.masterData.customerCategoryList,event.id);
            break;
          }              
    }
  }

  onChangeBillingEntity(event)
  {    
    if(event)
    {
      this.onClearBillingEntity();
      this.getInvoiceBankList(event.id);
    }
  }

  onClearBillingEntity()
  {   
      this.model.bankAccount=null;
      this.masterData.invoiceBankList=[];    
    
  }

  setInvoiceRequestName(){
      switch (this.model.invoiceRequestType) {

        case this.invRequestType.Brand:

          var brandList=this.masterData.customerBrandList;
    
          for (let index = 0; index < this.model.invoiceRequestList.length; index++) {
         
            var brand=brandList.filter(x=>x.id==this.model.invoiceRequestList[index].brandId);

            if(brand)
            {
              this.model.invoiceRequestList[index].invRequestName=brand[0].name;
            }          
             
           }

           brandList.forEach(element => {
             
            var data= this.model.invoiceRequestList.filter(x=>x.brandId==element.id)
               if(data.length==0)
               {
                 let requestItem=new PriceInvoiceRequest(); 
                 requestItem.departmentId=null;    
                 requestItem.brandId=element.id;  
                 requestItem.buyerId=null;
                 requestItem.invRequestName=element.name;
                 requestItem.isCommon=false;
                 this.model.invoiceRequestList.push(requestItem);
               }
 
            });

          break;

          case this.invRequestType.Department:

            var departmentList=this.masterData.customerDepartmentList;

            for (let index = 0; index < this.model.invoiceRequestList.length; index++) {
         
              var department=departmentList.filter(x=>x.id==this.model.invoiceRequestList[index].departmentId);
  
              if(department)
              {
                this.model.invoiceRequestList[index].invRequestName=department[0].name;
              }          
               
             }
             departmentList.forEach(element => {
             
              var data= this.model.invoiceRequestList.filter(x=>x.departmentId==element.id)
                 if(data.length==0)
                 {
                   let requestItem=new PriceInvoiceRequest(); 
                   requestItem.departmentId=element.id;    
                   requestItem.brandId=null;  
                   requestItem.buyerId=null;
                   requestItem.invRequestName=element.name;
                   requestItem.isCommon=false;
                   this.model.invoiceRequestList.push(requestItem);
                 }   
              });

          break;

          case this.invRequestType.Buyer:

           var buyerList=this.masterData.customerBuyerList;

           for (let index = 0; index < this.model.invoiceRequestList.length; index++) {
         
            var buyer=buyerList.filter(x=>x.id==this.model.invoiceRequestList[index].buyerId);

            if(buyer)
            {
              this.model.invoiceRequestList[index].invRequestName=buyer[0].name;
            }              
           } 
           
           buyerList.forEach(element => {
             
           var data= this.model.invoiceRequestList.filter(x=>x.buyerId==element.id)
              if(data.length==0)
              {
                let requestItem=new PriceInvoiceRequest(); 
                requestItem.departmentId=null;    
                requestItem.brandId=null;  
                requestItem.buyerId=element.id;
                requestItem.invRequestName=element.name;
                requestItem.isCommon=false;
                this.model.invoiceRequestList.push(requestItem);
              }
           });

          break;    

          case this.invRequestType.ProductCategory:

            var categoryList=this.masterData.productCategoryList;
 
            for (let index = 0; index < this.model.invoiceRequestList.length; index++) {
          
             var category=categoryList.filter(x=>x.id==this.model.invoiceRequestList[index].productCategoryId);
 
             if(category)
             {
               this.model.invoiceRequestList[index].invRequestName=category[0].name;
             }              
            } 
            
            categoryList.forEach(element => {
              
              var data= this.model.invoiceRequestList.filter(x=>x.productCategoryId==element.id)
               if(data.length==0)
               {
                 let requestItem=new PriceInvoiceRequest(); 
                 requestItem.departmentId=null;    
                 requestItem.brandId=null;  
                 requestItem.buyerId=null;
                 requestItem.productCategoryId=null;
                 requestItem.invRequestName=element.name;
                 requestItem.isCommon=false;
                 this.model.invoiceRequestList.push(requestItem);
               }
            });
 
           break;   
           
    }
  }

  addSubCategoryList(dataSourceList){
   
    this.model.subCategoryList=[]; 

    for (let index = 0; index < dataSourceList.length; index++) {

        let requestItem=new PriceSubCategory(); 
        requestItem.subCategory2Id=dataSourceList[index].id;      
        requestItem.subCategory2Name=dataSourceList[index].name; 

     
          requestItem.mandayReports=this.model.mandayReports;   
          requestItem.mandayBuffer=this.model.mandayBuffer;  
          requestItem.mandayProductivity=this.model.mandayProductivity;    
          requestItem.unitPrice=this.model.unitPrice;  
          requestItem.aqL_QTY_8=this.model.quantity8;  
          requestItem.aqL_QTY_13=this.model.quantity13; 
          requestItem.aqL_QTY_20=this.model.quantity20;  
          requestItem.aqL_QTY_32=this.model.quantity32; 
          requestItem.aqL_QTY_50=this.model.quantity50; 
          requestItem.aqL_QTY_80=this.model.quantity80;
          requestItem.aqL_QTY_125=this.model.quantity125; 
          requestItem.aqL_QTY_200=this.model.quantity200; 
          requestItem.aqL_QTY_315=this.model.quantity315; 
          requestItem.aqL_QTY_500=this.model.quantity500; 
          requestItem.aqL_QTY_800=this.model.quantity800; 
          requestItem.aqL_QTY_1250=this.model.quantity1250; 

      this.model.subCategoryList.push(requestItem);   

      this.subCategoryValidators.push({ subCategory: requestItem, validator: Validator.getValidator(requestItem, 
        "customer/edit-customerpricecard-subcategory.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
   
    }    
  }


  addInvoiceRequest(dataSourceList,requestType){
   
    this.model.invoiceRequestList=[]; 

    for (let index = 0; index < dataSourceList.length; index++) {

      let requestItem=new PriceInvoiceRequest(); 
      requestItem.departmentId=null;    
      requestItem.buyerId=null;
      requestItem.brandId=null;   
      requestItem.productCategoryId=null;
   

      if(requestType==this.invRequestType.Brand)
      {
        requestItem.brandId=dataSourceList[index].id;
      }
     else if(requestType==this.invRequestType.Department)
      {
        requestItem.departmentId=dataSourceList[index].id;
      }
     else if(requestType==this.invRequestType.Buyer)
      {
        requestItem.buyerId=dataSourceList[index].id;
      }
      else if(requestType==this.invRequestType.ProductCategory)
      {
        requestItem.productCategoryId=dataSourceList[index].id;
      }

      requestItem.invRequestName=dataSourceList[index].name;

      if(this.model.invoiceRequestSelectAll)
      {
        requestItem.isCommon=true;
        requestItem.billedName=this.model.invoiceRequestBilledName;
        requestItem.billedAddress=this.model.invoiceRequestAddress;
        requestItem.billedAddressId=this.model.customerAddressId;
        requestItem.invoiceRequestContactList=this.model.invoiceRequestContact;
      }
      this.model.invoiceRequestList.push(requestItem);   
    }    
  }

  //get supplierlist
  getSupplierList() {
    this.supplierService.getSuppliersbyCustomer(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.supplierList = response.data;
          this.masterData.supplierLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.supplierLoading = false;
        });
  }

  getEditPath(): string {
    return "";
  }
  getViewPath(): string {
    return "";
  }

  hasComplexAccess(){
    return this.masterData.hasMandayComplexAccess || this.masterData.hasSamplingComplexAccess 
    || this.masterData.hasPieceRateComplexAccess || this.masterData.hasInterventionComplexAccess;
  }

  isFormValid(): boolean 
  {
    let isOk = this.validator.isValid('customerId') && this.validator.isValid('billingMethodId') &&
      this.validator.isValid('billingToId') &&
      this.validator.isValid('serviceId') &&
      this.validator.isValid('currencyId') &&

      this.validator.isValid('periodFrom') &&
      this.validator.isValid('periodTo') && this.validator.isValid('serviceTypeIdList');
      

    if (isOk && this.hasComplexAccess()) 
    {
        isOk = this.validator.isValid('productCategoryIdList');
    }


     if (isOk && this.model.billingMethodId == this.billingMethod.ManDay && !this.masterData.hasMandayComplexAccess) 
     {
         isOk = this.validator.isValid('unitPrice')
     }

    // add validation for complex manday access with special logic
    if (isOk && this.model.billingMethodId == this.billingMethod.ManDay && this.masterData.hasMandayComplexAccess) 
    {
        if(this.model.isSpecial)
        {
          isOk =  this.ruleValidators.every((x) =>
          x.validator.isValid('mandayProductivity')
          && x.validator.isValid('mandayReports')
          && x.validator.isValid('unitPrice'));
        }
      if(isOk && !this.model.subCategorySelectAll)
      {
        let message: string;
        isOk =  this.subCategoryValidators.every((x) =>x.validator.isValidIf('mandayProductivity',x.subCategory.isCommon));
        if (!isOk && (message == undefined || message == null))
          message = 'EDIT_CUSTOMER_PRICE_CARD.MSG_PRICE_MandayProductivity'
        
        isOk =  this.subCategoryValidators.every((x) =>x.validator.isValidIf('mandayReports',x.subCategory.isCommon));
        if (!isOk && (message == undefined || message == null))
          message = 'EDIT_CUSTOMER_PRICE_CARD.MSG_PRICE_MandayReports'
        
        isOk =  this.subCategoryValidators.every((x) =>x.validator.isValidIf('mandayBuffer',x.subCategory.isCommon));
        if (!isOk && (message == undefined || message == null))
          message = 'EDIT_CUSTOMER_PRICE_CARD.MSG_PRICE_MandayBuffer'
        
        isOk =  this.subCategoryValidators.every((x) =>x.validator.isValidIf('unitPrice',x.subCategory.isCommon));
        if (!isOk && (message == undefined || message == null))
          message = 'EDIT_CUSTOMER_PRICE_CARD.MSG_PRICE_UnitPrice'

        if (!isOk && message != undefined && message != null)
          this.showWarning('EDIT_CUSTOMER_PRICE_CARD.TITLE', message);
      }

        return isOk;
      
    }

    if (isOk && this.model.freeTravelKM != null)
      isOk = this.changeFreeTravelKM();
    if (isOk && this.model.holidayTypeIdList != null && this.model.holidayTypeIdList.length > 0)
      isOk = this.validator.isValid('holidayPrice');
    if (isOk && this.model.priceToEachProduct)
      isOk = this.validator.isValid('productPrice');

    if (isOk && (this.model.billingMethodId == this.billingMethod.Sampling)) {
      isOk = this.validator.isValid('maxSampleSize');

    }

    // add validation for complex sampling access
    if (isOk && this.model.billingMethodId == this.billingMethod.Sampling && this.masterData.hasSamplingComplexAccess) 
    {
        
      if(isOk && !this.model.subCategorySelectAll)
      {
        var subCategoryVal= this.subCategoryValidators.filter(x=>x.subCategory.isCommon);
        isOk =  subCategoryVal.every((x) =>
          x.validator.isValid('aqL_QTY_8')
          && x.validator.isValid('aqL_QTY_13')
          && x.validator.isValid('aqL_QTY_20')
          && x.validator.isValid('aqL_QTY_32')
          && x.validator.isValid('aqL_QTY_50')
          && x.validator.isValid('aqL_QTY_80')
          && x.validator.isValid('aqL_QTY_125')
          && x.validator.isValid('aqL_QTY_200')
          && x.validator.isValid('aqL_QTY_315')
          && x.validator.isValid('aqL_QTY_500')
          && x.validator.isValid('aqL_QTY_800')
          && x.validator.isValid('aqL_QTY_1250'));
      }
   

        return isOk;
    }


        // add validation for complex PieceRate access
        if (isOk && this.model.billingMethodId == this.billingMethod.PieceRate && this.masterData.hasPieceRateComplexAccess) 
        {

          if(this.model.isSpecial)
          {
            isOk =  this.ruleValidators.every((x) =>
            x.validator.isValid('pieceRate_Billing_Q_Start')
            && x.validator.isValid('piecerate_Billing_Q_End')
            && x.validator.isValid('piecerate_MinBilling')
            && x.validator.isValid('additionalFee')
            && x.validator.isValid('unitPrice'));
          }
          if(isOk && !this.model.subCategorySelectAll)
          {
            var subCategoryVal= this.subCategoryValidators.filter(x=>x.subCategory.isCommon);
            isOk =  subCategoryVal.every((x) =>
              x.validator.isValid('unitPrice'));
          }
      

           return isOk;
        }


          // add validation for complex PerIntervention access
          if (isOk && this.model.billingMethodId == this.billingMethod.PerIntervention && this.masterData.hasInterventionComplexAccess) 
          {
              
            if(this.model.interventionType==InterventionType.Range && this.model.isSpecial)
            {
              isOk =  this.ruleValidators.every((x) =>
              x.validator.isValid('perInterventionRange1')
              && x.validator.isValid('perInterventionRange2')
              && x.validator.isValid('unitPrice'));
            }

            if(this.model.interventionType==InterventionType.PerStyle && this.model.isSpecial)
            {
               isOk =  this.ruleValidators.every((x) =>             
                x.validator.isValid('unitPrice'));
            }   
            return isOk;       
          }

      if (isOk && this.model.additionalSampleSize != null) {
        isOk = this.validator.isValid('additionalSamplePrice');
      }


   
    if(isOk && this.model.isInvoiceConfigured)
    {
      if(this.model.billingToId==this.billingTo.Customer && this.validateBillDetails())
      {
      isOk = this.validator.isValid('invoiceRequestBilledName') &&
      this.validator.isValid('invoiceRequestAddress') &&
      this.validator.isValid('invoiceRequestContactIds')     
     
      }
      if(isOk)
      {
        isOk=this.validator.isValid('invoiceRequestType') &&  this.validator.isValid('invoiceNoDigit')&&   this.validator.isValid('invoiceNoPrefix') 
        && this.validator.isValid('invoiceTravelExpense') 
        && this.validator.isValid('invoiceDiscountFeeFrom') && this.validator.isValid('invoiceOtherFeeFrom')
        && this.validator.isValid('billingEntity') && this.validator.isValid('bankAccount')
      }  
      if(isOk && this.model.isInvoiceConfigured && this.model.billingToId==this.billingTo.Customer)
      {
         if(this.model.invoiceRequestType != this.invRequestType.NotApplicable && !this.model.invoiceRequestSelectAll)
         {
             if(this.model.invoiceRequestList.length>0)
             {

              var selectedRow=this.model.invoiceRequestList.filter(x=>x.isCommon);

              if(selectedRow.length==0)
              {
                this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE','EDIT_CUSTOMER_PRICE_CARD.MSG_INVOICE_REQ_VALID_BILL_DETAILS');
                isOk=false;
                return isOk;
              }
              else
              {
                this.model.invoiceRequestList.every(element => {
                                       if(element.isCommon)
                                       {
                                          if(!element.billedName ||!element.billedAddress || element.invoiceRequestContactList.length==0)
                                          {
                                            this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE','EDIT_CUSTOMER_PRICE_CARD.MSG_INVOICE_REQ_FILL_BILL_DETAILS');
                                            isOk=false;
                                            return isOk;
                                          }
                                       }                  
                                  });

              }
               
             }
         }
      }      
    }

    if (isOk && this.model.invoiceInspFeeFrom != null && this.model.invoiceInspFeeFrom ==this.invFeesFrom.Invoice) 
    {
      isOk = this.validator.isValid('billQuantityType');
    }

    return isOk;
  }

  isSamplingDataValid() {
    if (!(this.model.freeTravelKM > 0)) {
      this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'EDIT_CUSTOMER_PRICE_CARD.FREE_TRAVEL_KM_MSG_NUMBER_REQ');
      return false;
    }
    else {
      return true;
    }
  }

  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.ruleValidators)
    item.validator.isSubmitted = true;

    if(!this.model.subCategorySelectAll)
    {
      for (let item of this.subCategoryValidators)
       item.validator.isSubmitted = true;
    }
    else
    {
      this.model.subCategoryList=[];
    }


    if (this.isFormValid()) {
      this.masterData.saveLoading = true;

      if (this.model.unitPrice == undefined || this.model.unitPrice == null)
        this.model.unitPrice = 0;
        
      if (this.model.billingMethodId == this.billingMethod.Sampling)
        this.model.unitPrice = 0;

      let response: SaveCustomerPriceCardResponse;

      if (this.model.id == null || this.copyId == 0) {
        this.model.id = 0;
      }    
      try 
      {
        // Update Price complex type only at the time of create case.
        if(!(this.model.id>0))
        {
          this.model.priceComplexType=this.hasComplexAccess()?PriceComplexType.Complex:PriceComplexType.Simple;
        }
        response = await this.customerPriceCardService.save(this.model);
      }
      catch (e) 
      {
        console.error(e);
        this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.masterData.saveLoading = false;
      }
      if (response) 
      {
        switch (response.result) {
          case CustomerPriceCardResponseResult.Success:
            this.showSuccess('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'EDIT_CUSTOMER_PRICE_CARD.MSG_SAVE_SUCCESS');
            
            // if (this.fromSummary)
            this.returnToSummary();
            // else
            //   this.editData(response.id);
            break;
          case CustomerPriceCardResponseResult.Faliure:
            this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
            break;
          case CustomerPriceCardResponseResult.Exists:
            this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'EDIT_CUSTOMER_PRICE_CARD.MSG_DATA_EXISTS');
            break;
        }
        this.masterData.saveLoading = false;
      }
    }
  }

  //get data from DB
  async editData(id: number) {
    let response: EditCustomerPriceCardResponse;
    try {
      response = await this.customerPriceCardService.edit(id);
      this.editDataResponse(response);

    }
    catch (e) {
      console.error(e);
      this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
    }
  }

  validateBillDetails():boolean
  {
    let isValidate=false;

    if(this.model.billingToId == this.billingTo.Customer && this.model.isInvoiceConfigured && this.model.invoiceRequestType)
    {
      if(this.model.invoiceRequestType == this.invRequestType.NotApplicable)
      {
        isValidate=true;
      }
      else if(this.model.invoiceRequestType != this.invRequestType.NotApplicable && this.model.invoiceRequestSelectAll)
      {
        isValidate=true;
      }
    }

     return isValidate;
  }


  //edit  response
  editDataResponse(response: EditCustomerPriceCardResponse) {
    if (response) {
      switch (response.result) {
        case CustomerPriceCardResponseResult.Success:
          this.model = response.getData;
          this.masterData.isInvoiceConfigured = this.model.isInvoiceConfigured;

          if (this.model.customerId) {
            this.getBillingMethodList();
            this.getCustomerDepartments();
            this.getCustomerBrands();
            this.getCustomerBuyers();
            this.getCustomerProductCategoryList();
            this.getCustomerProductSubCategoryList();
            this.getCustomerPriceCategoryList();
            this.getCustomerPriceHolidayList();
            this.getCustomerAddressList();
            this.getCustomerPriceData();
            this.getCustomerContactList();
            this.getInvoiceRequestList(); 

            
           
            
            if(this.model.ruleList.length>0)
            {
              this.model.ruleList.map((rule) => {
                this.ruleValidators.push({ rule: rule, validator: Validator.getValidator(rule, 
                  "customer/edit-customerpricecard-rule.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
    
              });
            }

            if(this.model.subCategoryList.length>0)
            {

            // copy activity - reset values
             if(this.copyId==0)
             {
              this.model.subCategoryList.forEach(element => {
                element.id=0;
               });
             }
                      
              this.model.subCategoryList.map((requestItem) => {
                this.subCategoryValidators.push({ subCategory: requestItem, validator: Validator.getValidator(requestItem, 
                  "customer/edit-customerpricecard-subcategory.valid.json", this.jsonHelper, false, this._toastr, this._translate) });
    
              });

              this.checkIfAllSubcategoryItemSelected();
            }

            
            
            if(this.model.serviceId){
              this.setInvoiceFeesTypeList(this.model.serviceId);
              this.getServiceTypesList();
            }
            if(this.model.isInvoiceConfigured)
            {
              if(this.model.billingEntity>0)
              {
                this.getInvoiceBankList(this.model.billingEntity);
              }              
              setTimeout(() => {
                this.setInvoiceRequestName();
              }, 2000);
            }          
          }

          if (this.model.freeTravelKM == 0) {
            this.model.freeTravelKM = null;
          }
          this.getSupplierList();
          this.getProvinceList();
          this.getCityList();
          break;
        case CustomerPriceCardResponseResult.Faliure:
          this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
          break;
      }
    }
  }

  //if any loading true, disable the save button
  buttonDisable() {
    return (this.masterData.provinceLoading ||
      this.masterData.countryLoading ||
      this.masterData.customerLoading ||
      this.masterData.supplierLoading ||
      this.masterData.billingMethodLoading ||
      this.masterData.billingToLoading ||
      this.masterData.serviceLoading ||
      this.masterData.serviceTypeLoading ||
      this.masterData.productCategoryLoading ||
      this.masterData.currencyLoading || this.masterData.saveLoading);
  }
  changeFreeTravelKM() {
    if (!(this.model.freeTravelKM > 0)) {
      this.showError('EDIT_CUSTOMER_PRICE_CARD.TITLE', 'EDIT_CUSTOMER_PRICE_CARD.FREE_TRAVEL_KM_MSG_NUMBER_REQ');
      return false;
    }
    else {
      return true;
    }
  }
  returnToSummary() {
    this.return('pricecardsummary/price-card-summary');
  }
  cancel() {

  }

  getCustomerBrands() {
    this.masterData.customerBrandLoading = true;
    this.customerService.getCustomerBrands(this.model.customerId)
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

  getCustomerDepartments() {
    this.masterData.customerDepartmentLoading = true;
    this.customerService.getCustomerDepartments(this.model.customerId)
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

  getCustomerAddressList() {
    this.masterData.customerAddressLoading = true;
    this.customerService.getCustomerAddressList(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.customerAddressList = response.dataSourceList;
          this.masterData.customerAddressLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.customerAddressLoading = false;
        });
  }

  getCustomerPriceData() {
    this.customerService.getcustomerpricedata(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response)
            this.model.customerCountry = response.countryName;
            this.model.customerSegment = response.customerSegment;
        },
        error => {
          this.setError(error);
        });
  }

  getCustomerContactList() {
    this.masterData.customerContactsLoading = true;
    this.customerService.getCustomerContactList(this.model.customerId)
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

  getCustomerBuyers() {
    this.masterData.customerBuyerLoading = true;
    this.customerService.getCustomerBuyers(this.model.customerId)
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

  getCustomerProductCategoryList() {
    this.masterData.productCategoryLoading = true;
    this.customerService.getCustomerProductCategoryList(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.customerCategoryList = response.dataSourceList;
            this.masterData.productCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.productCategoryLoading = false;
        });
  }

  changeProductCategory(event){  
    this.model.ruleList=[];
    this.model.subCategoryList=[];
    this.ruleValidators=[];
    this.subCategoryValidators=[];   
    this.model.productSubCategoryIdList=[];

   if(event)
   {
    this.getCustomerProductSubCategoryList();
    if(!this.model.subCategorySelectAll)
    {
      this.getCustomerProductSub2CategoryList();
    }    
   }
  }

  changeProductSubCategory(){
    if(!this.model.subCategorySelectAll)
    {
    this.getCustomerProductSub2CategoryList();
    }
  }

  getCustomerProductSubCategoryList() {
    this.masterData.productSubCategoryLoading = true;
    this.masterData.customerProductSubCategoryList=[];
    var productCategory=[];
    if(this.model.productCategoryIdList && this.model.productCategoryIdList.length>0)
    {
      productCategory=this.model.productCategoryIdList
    }

    var requestCustomerSubCategory=
    {
      customerId:this.model.customerId,
      productCategory:productCategory
    }

    this.customerService.getCustomerProductSubCategoryList(requestCustomerSubCategory)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.customerProductSubCategoryList = response.dataSourceList;
            this.masterData.productSubCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.productSubCategoryLoading = false;
        });
  }


  getCustomerProductSub2CategoryList() {
   
    this.masterData.customerProductSub2CategoryList=[];
    var productCategory=[];
    var productSubCategory=[];

    if(this.model.productCategoryIdList && this.model.productCategoryIdList.length>0)
    {
      this.masterData.productSubCategory2Loading = true;
      productCategory=this.model.productCategoryIdList

      if(this.model.productSubCategoryIdList && this.model.productSubCategoryIdList.length>0)
      {
        productSubCategory=this.model.productSubCategoryIdList
      }
  
      var requestCustomerSubCategory=
      {
        productCategory:productCategory,
        productSubCategory:productSubCategory
      }
  
      this.customerService.getCustomerProductSub2CategoryList(requestCustomerSubCategory)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success)
              this.masterData.customerProductSub2CategoryList = response.dataSourceList;
              this.resetRuleAndSubCategory();
              this.masterData.productSubCategory2Loading = false;
          },
          error => {
            this.setError(error);
            this.masterData.productSubCategory2Loading = false;
          });
    }


  }

  getCustomerPriceCategoryList() {
    this.masterData.customerPriceCategoryLoading = true;
    this.customerService.getCustomerPriceCategories(this.model.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.customerPriceCategoryList = response.dataSourceList;
          this.masterData.customerPriceCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.customerPriceCategoryLoading = false;
        });
  }

  getCustomerPriceHolidayList() {
    this.masterData.customerPriceHolidayLoading = true;
    this.customerPriceCardService.getCustomerPriceHolidayList()
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterData.customerPriceHolidayList = response.dataSourceList;
          this.masterData.customerPriceHolidayLoading = false;
        },
        error => {
          this.setError(error);
          this.masterData.customerPriceHolidayLoading = false;
        });
  }

  changeHolidayTypeList() {
    if (this.model.holidayTypeIdList && this.model.holidayTypeIdList.length == 0)
      this.model.holidayPrice = null;

  }

  changePriceToEachProduct() {
    if (!this.model.priceToEachProduct)
      this.model.productPrice = null;
  }

  changeBillingMethod() {
    this.model.sampleSizeBySet = false;
    this.model.minBillingDay = null;
    this.model.maxSampleSize = null;
    this.model.maxProductCount = null;
    this.model.additionalSamplePrice = null;
    this.model.additionalSampleSize = null;
    this.model.quantity8 = null;
    this.model.quantity13 = null;
    this.model.quantity20 = null;
    this.model.quantity32 = null;
    this.model.quantity50 = null;
    this.model.quantity80 = null;
    this.model.quantity125 = null;
    this.model.quantity200 = null;
    this.model.quantity315 = null;
    this.model.quantity500 = null;
    this.model.quantity800 = null;
    this.model.quantity1250 = null;
    this.model.unitPrice = 0;

    this.model.mandayReports=null;
    this.model.mandayBuffer=null;
    this.model.mandayProductivity=null;
    this.model.subCategorySelectAll=true;
    this.model.isSpecial=false;
    this.model.interventionType=null;
    this.model.billQuantityType=null;
    this.resetRuleAndSubCategory();  
    
  }

  resetRuleAndSubCategory(){
    this.model.ruleList=[];
    this.ruleValidators=[];
    this.model.subCategoryList=[];  
    this.subCategoryValidators=[];

     // if manday then set default as quotation
     if(this.model.billingMethodId == this.billingMethod.ManDay)
     {
      if(this.model.id==undefined || this.model.id==0)
      {
        this.model.invoiceInspFeeFrom=this.invFeesFrom.Quotation;
      }
      
       if(this.masterData.hasMandayComplexAccess)
       {
        this.addSubCategoryList(this.masterData.customerProductSub2CategoryList);
        this.addSpecialRule();
       }
     } 
     else  
     {
      if(this.model.id==undefined || this.model.id==0)
      {
        this.model.invoiceInspFeeFrom=null;
      }   
       if(this.masterData.hasSamplingComplexAccess || this.masterData.hasPieceRateComplexAccess || this.masterData.hasInterventionComplexAccess)
       {
        this.addSubCategoryList(this.masterData.customerProductSub2CategoryList);
        this.addSpecialRule();
       }
     } 
  }

  changeInterventionType()
  {
     this.model.isSpecial=false;

     if(this.model.interventionType == this.interventionType.Range)
     {
       this.model.isSpecial=true;
     } 
     else if(this.model.interventionType == this.interventionType.PerStyle)
     {
     }
  }


  setProductSubCategoryManday()
  {

  }

  //get the inspection location list
  getInspectionLocations() {
    this.masterData.inspectionLocationLoading = true;
    this.referenceService.getInspectionLocationList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.masterData.inspectionLocationList = response.dataSourceList;
            this.masterData.inspectionLocationLoading = false;
          }
          else if (response && response.result == DataSourceResult.NotFound) {
            this.masterData.inspectionLocationLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.inspectionLocationLoading = false;
        }
      );
  }

  //get the billing frequency list
  getBillingFrequencyList() {
    this.masterData.billFrequancyLoading = true;
    this.referenceService.getBillFrequencyList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        response => {
          if (response && response.result == DataSourceResult.Success) {
            this.masterData.billFrequencyList = response.dataSourceList;
            this.masterData.billFrequancyLoading = false;
          }
          else if (response && response.result == DataSourceResult.NotFound) {
            this.masterData.billFrequancyLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterData.billFrequancyLoading = false;
        }
      );
  }

    //get the billing frequency list
    getBillingQuantityTypeList() {
      this.masterData.billQuantityTypeLoading = true;
      this.referenceService.getBillQuantityTypeList()
        .pipe(takeUntil(this.componentDestroyed$), first())
        .subscribe(
          response => {
            if (response && response.result == DataSourceResult.Success) {
              this.masterData.billQuantityTypeList = response.dataSourceList;
              this.masterData.billQuantityTypeLoading = false;
            }
            else if (response && response.result == DataSourceResult.NotFound) {
              this.masterData.billQuantityTypeLoading = false;
            }
          },
          error => {
            this.setError(error);
            this.masterData.billQuantityTypeLoading = false;
          }
        );
    }

        //get the intervention type list
        getInterventionTypeList() {
          this.masterData.interventionTypeLoading = true;
          this.referenceService.getInterventionTypeList()
            .pipe(takeUntil(this.componentDestroyed$), first())
            .subscribe(
              response => {
                if (response && response.result == DataSourceResult.Success) {
                  this.masterData.interventionTypeList = response.dataSourceList;
                  this.masterData.interventionTypeLoading = false;
                }
                else if (response && response.result == DataSourceResult.NotFound) {
                  this.masterData.interventionTypeLoading = false;
                }
              },
              error => {
                this.setError(error);
                this.masterData.interventionTypeLoading = false;
              }
            );
        }

                //get the intervention type list
                getEntityFeatureList() {
                  this.masterData.hasMandayComplexAccess = false;
                  this.masterData.hasSamplingComplexAccess = false;
                  this.masterData.hasPieceRateComplexAccess = false;
                  this.masterData.hasInterventionComplexAccess = false;
                  this.referenceService.getEntityFeatureList()
                    .pipe(takeUntil(this.componentDestroyed$), first())
                    .subscribe(
                      response => {
                        if (response) 
                        {
                            if(response.find(x=>x==EntityFeature.MandayComplex))
                            {
                              this.masterData.hasMandayComplexAccess = true;
                            }
                            if(response.find(x=>x==EntityFeature.SamplingComplex))
                            {
                              this.masterData.hasSamplingComplexAccess = true;
                            }
                            if(response.find(x=>x==EntityFeature.PieceRateComplex))
                            {
                              this.masterData.hasPieceRateComplexAccess = true;
                            }
                            if(response.find(x=>x==EntityFeature.PerInterventionComplex))
                            {
                              this.masterData.hasInterventionComplexAccess = true;
                            }
                        }                       
                      },
                      error => {
                        this.setError(error);
                        this.masterData.interventionTypeLoading = false;
                      }
                    );
                }


  ngOnDestroy(): void {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

    //open the contact page in a new tab if hyperlink is clicked
    AddCustomerContact() {
        if (!this.model.customerId) 
        {
          this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_CUS_REQ');
        }   
       else 
       {
          let entity: string = this.utility.getEntityName();
          var path=entity + "/" + Url.CustomerContact + this.model.customerId;         
          window.open(path);
        }
      }
      
  //get city list on province change
  ChangeProvinceData() {
    this.masterData.cityList = null;
    this.model.factoryCityIdList = null;
    this.getCityList();
  }

  getCityList() {
    var id = this.model.factoryProvinceIdList.length == 1 ?
      this.model.factoryProvinceIdList[0] : 0;
    if (id > 0) {
      this.masterData.showCityField = true;
      this.masterData.cityLoading = true;

      this.locationService.getcitybyprovinceid(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == ResponseResult.Success) {
              this.masterData.cityList = response.data;
            }
            this.masterData.cityLoading = false;
          },
          error => {
            this.setError(error);
            this.masterData.cityLoading = false;
          });
    }
    else {
      this.masterData.showCityField = false;
    }
  }
}
