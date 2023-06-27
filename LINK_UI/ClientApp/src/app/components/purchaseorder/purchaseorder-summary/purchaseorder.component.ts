import { Component, ElementRef, NgModule, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PurchaseOrderService } from '../../../_Services/purchaseorder/purchaseorder.service';
import { LocationService } from '../../../_Services/location/location.service';
import { PurchaseOrderSummaryModel, PurchaseOrderSummaryItemModel, purchaseOrderToRemove, PurchaseOrderFilterModel,PurchaseOrderListModel, PoBookingDetailResponse, PoBookingResult, PoBookingDetail } from '../../../_Models/purchaseorder/purchaseordersummary.model'
import { first, switchMap, distinctUntilChanged, tap, catchError, debounceTime, takeUntil } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { CustomerService } from '../../../_Services/customer/customer.service';
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { CommonDataSourceRequest, CountryDataSourceRequest ,SupplierDataSourceRequest} from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { PageSizeCommon, UserType } from '../../common/static-data-common';

@Component({
  selector: 'app-purchaseorder',
  templateUrl: './purchaseorder.component.html',
  styleUrls: ['./purchaseorder.component.css'],
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
export class PurchaseorderComponent  extends SummaryComponent<PurchaseOrderSummaryModel> {

  public data: any;
  loading = true;
  initialLoading: boolean = false;
  searchloading: boolean = false;
  supLoading:boolean=false;
  factLoading:boolean=false;
  error = '';
  public model: PurchaseOrderSummaryModel;
  public modelRemove: purchaseOrderToRemove;
  public modelRef: NgbModalRef;
  public searchIsCustomer = false;
  public isPurchaseOrderDetails: boolean = false;
  public isCustomerContactDetails: boolean = false;
  public editPage: string;
  public viewPage: string;
  idCurrentPurchaseOrder: number;
  customerList: Array<any> = [];
  countryList: Array<any> = [];
  supplierList: Array<any> = [];
  factoryList: Array<any> = [];
  isFilterOpen: boolean;
  public currentUser: UserModel;
  public exportDataLoading = false;
  summaryListModel: PurchaseOrderListModel;
  componentDestroyed$: Subject<boolean> = new Subject();
  @ViewChild('bookingDetailTemplate') bookingDetailsTemplate: ElementRef;

  constructor(public service: PurchaseOrderService, public modalService: NgbModal,
    authService: AuthenticationService,public customerService:CustomerService,
      public locationService: LocationService,
      public supplierService:SupplierService,
      public utility: UtilityService,
     router: Router, route: ActivatedRoute, public validator: Validator,translate: TranslateService,
    public routerCurrent: Router) {
    super(router, validator, route,translate);
    this.validator.setJSON("purchaseorder/purchaseorder.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new PurchaseOrderSummaryModel();  
    this.currentUser = authService.getCurrentUser();
    this.isFilterOpen = true;
    this.summaryListModel = new PurchaseOrderListModel();
  }

  onInit() {
    this.initialLoading=true;
    this.summaryListModel.selectedPageSize = PageSizeCommon[0];
  }  
  ngAfterViewInit() {
    this.getCustomerListBySearch();
    this.getCountryListBySearch();
  }

  SearchDetails() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.model.pageSize = this.summaryListModel.selectedPageSize;
      this.search();
    }
  }

  getSupplierData() {

    this.summaryListModel.supplierRequest.searchText = this.summaryListModel.supplierInput.getValue();
    this.summaryListModel.supplierRequest.skip = this.summaryListModel.supplierList.length;
    this.summaryListModel.supplierRequest.customerIds.push(this.model.customerValues);

    this.summaryListModel.supplierLoading = true;
    this.supplierService.getSupplierDataSource(this.summaryListModel.supplierRequest,0).
      subscribe(supplierData => {
        if (supplierData && supplierData.length > 0) {
          this.summaryListModel.supplierList = this.summaryListModel.supplierList.concat(supplierData);
        }

        this.summaryListModel.supplierRequest = new SupplierDataSourceRequest();
        this.summaryListModel.supplierRequest.customerIds.push(this.model.customerValues);
        this.summaryListModel.supplierLoading = false;
      }),
      error => {
        this.summaryListModel.supplierLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getSupplierListBySearch() {
    this.summaryListModel.supplierRequest.customerIds.push(this.model.customerValues);
    this.summaryListModel.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryListModel.supplierLoading = true),
      switchMap(term => term
        ? this.supplierService.getSupplierDataSource(this.summaryListModel.supplierRequest,0, term)
        : this.supplierService.getSupplierDataSource(this.summaryListModel.supplierRequest,0)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryListModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.summaryListModel.supplierList = data;
        this.summaryListModel.supplierLoading = false;
      
      });
  }


  getSupplierListSaveResponse(data) {
    if (data && data.result == 1) {
      this.supplierList = data.data;
    }
    else {
      this.error = data.result;
      this.supplierList = [];
    }
    this.model.purchaseFilterData.supplierId = null;
    this.model.purchaseFilterData.factoryId = null;
    this.factoryList = [];
    this.supLoading = false;
  }
   
  getFactoryList(cusId,supplierId){
    this.factLoading=true;
    this.service.GetFactoryDetailsByCusSupId(supplierId,cusId)
    .pipe()
    .subscribe(
      data => {
        this.getFactoryListSaveResponse(data);
        
      },
      error => {
        this.setError(error);
        this.factLoading = false;
      });     
  }
  getFactoryListSaveResponse(data) {
    if (data && data.result == 1) {
      this.factoryList = data.data;
    }
    else {
      this.error = data.result;
      this.data.factoryList = [];
    }
    this.model.purchaseFilterData.factoryId = null;
    this.factLoading = false;
  }
  
  //fetch customer dropdown list
  getCustomerListBySearch() {   
    if (this.model.customerValues && this.model.customerValues > 0) {
      this.summaryListModel.customerRequest.id = this.model.customerValues;
    }
    else {
      this.summaryListModel.customerRequest.id = null;
    }
    this.summaryListModel.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryListModel.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.summaryListModel.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.summaryListModel.customerRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryListModel.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.summaryListModel.customerList = data;
        this.summaryListModel.customerLoading = false;
        this.initialLoading=false;
        if (this.currentUser.usertype == UserType.Customer) {
          this.model.customerValues = this.currentUser.customerid;
          this.model.purchaseFilterData.customerId = this.currentUser.customerid;
          this.summaryListModel.supplierRequest.customerIds=[];
          this.getSupplierListBySearch();
        }
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.summaryListModel.customerRequest.searchText = this.summaryListModel.customerInput.getValue();
    this.summaryListModel.customerRequest.skip = this.summaryListModel.customerList.length;


    this.summaryListModel.customerLoading = true;

    this.summaryListModel.customerRequest.id = 0;
    this.customerService.getCustomerDataSourceList(this.summaryListModel.customerRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryListModel.customerList = this.summaryListModel.customerList.concat(customerData);
        }
        this.summaryListModel.customerRequest = new CommonDataSourceRequest();
        this.summaryListModel.customerLoading = false;
      }),
      error => {
        this.summaryListModel.customerLoading = false;
        this.setError(error);
      };
  }
 
  getCountryData() {

    this.summaryListModel.countryRequest.searchText = this.summaryListModel.countryInput.getValue();
    this.summaryListModel.countryRequest.skip = this.summaryListModel.countryList.length;

    this.summaryListModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.summaryListModel.countryRequest).
      subscribe(countryData => {
        if (countryData && countryData.length > 0) {
          this.summaryListModel.countryList = this.summaryListModel.countryList.concat(countryData);
        }

        this.summaryListModel.countryRequest = new CountryDataSourceRequest();
        this.summaryListModel.countryLoading = false;
      }),
      error => {
        this.summaryListModel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.summaryListModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryListModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.summaryListModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.summaryListModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryListModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.summaryListModel.countryList = data;
        this.summaryListModel.countryLoading = false;
        this.initialLoading=false;
      });
  }


  getPathDetails(): string {
    return this.editPage;
  }

  
  onChangeCustomer(){  
      if(this.model.customerValues!=null){ 
        this.model.purchaseFilterData.supplierId=null;  
        this.model.purchaseFilterData.factoryId=null; 
        this.factoryList=[]; 
        this.summaryListModel.supplierRequest.customerIds=[];
        this.summaryListModel.supplierRequest.searchText="";
        this.model.purchaseFilterData.customerId=this.model.customerValues;     
        this.getSupplierListBySearch();
      }
      else{
        this.clearCustomer() 
      }  
       
  }

    
  onChangeSupplier(){  
   if(this.model.purchaseFilterData.supplierId!=null && this.model.purchaseFilterData.customerId != null)
    this.getFactoryList(this.model.purchaseFilterData.customerId,this.model.purchaseFilterData.supplierId);
  }

  getData() {   
    
    if(this.model.customerValues)
    {
      this.model.purchaseFilterData.customerId=this.model.customerValues;
      if (this.model.purchaseFilterData.customerId) {
        this.searchloading = true;
        this.service.getDataSummary({ index: this.model.index, pageSize: this.model.pageSize, purchaseOrderData: this.model.purchaseFilterData })
        .pipe()
        .subscribe(
          response => {
            if (response) {  
                       
              this.model.index = response.index;
              this.model.pageSize = response.pageSize;
              this.model.totalCount = response.totalCount;
              this.model.pageCount = response.pageCount;
              if(response && response.result == 1 && response.data && response.data.length > 0)
              {
                this.model.items = response.data.map((x) => {
  
                  var tabItem: PurchaseOrderSummaryItemModel = {
                    id:x.id,
                    pono: x.pono,
                    customerName: x.customerName,
                    destinationCountry: x.destinationCountry,
                    etd: x.etd,
                    isBooked:x.isBooked,
                    isDelete: x.isDelete,
                    bookingNumber: x.bookingNumber,
                    bookingCount: x.bookingCount,
                    showBookingCount: x.showBookingCount                               
                  }
  
                  if (x.typeId != 1)
                    this.searchIsCustomer = true;
  
                  return tabItem;
                });
            }
            else if(response && response.result==2){
              this.model.noFound=true;
            }
  
  
            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
            }
            else {
              this.error = response.result;
              this.searchloading = false;
              
              // TODO check error from result
            }
            this.searchloading = false;
          },
          error => {
            this.error = error;           
            this.searchloading = false;
          });
      }     
    } 
   
  }



  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

 clearCustomer() {   
      this.model.purchaseFilterData.customerId=null;    
      this.model.purchaseFilterData.supplierId=null;  
      this.model.purchaseFilterData.factoryId=null; 
      this.supplierList=[];  
      this.factoryList=[]; 
      this.summaryListModel.customerRequest.searchText ="";
      this.summaryListModel.supplierList=null;
      this.getCustomerListBySearch();  
  }
  clearSupplier() { 

    this.model.purchaseFilterData.supplierId=null;  
    this.model.purchaseFilterData.factoryId=null;   
    this.factoryList=[];
    this.summaryListModel.supplierList=null;  
    this.summaryListModel.supplierRequest.searchText =""; 
    this.getSupplierListBySearch();
 }
//below functions used further
  /*  selectCustomer() {    
  if(this.model.customerValues!=null){ 
      this.model.purchaseFilterData.customerId=this.model.customerValues["id"];     
    }    
  }
  selectCustomerGroup() {   
    if(this.model.destinationCountry!=null){ 
      this.model.purchaseFilterData.destinationCountry=this.model.destinationCountry["id"];     
    }    
  }*/

  openConfirm(id, name, content) {

        this.modelRemove = {
          id: id,
          name: name
        };
    
        this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });
    
        this.modelRef.result.then((result) => {
        }, (reason) => {
        });

  }


  getDetail(id) {
    this.isPurchaseOrderDetails = true;
    this.idCurrentPurchaseOrder = id;
  }

  addSupp() {
    this.isPurchaseOrderDetails = true;
    this.idCurrentPurchaseOrder = null;
  }  

  
  deletePurchaseOrder(id: number) { 
    this.summaryListModel.pageLoader = true;
    this.service.deletePurchaseOrder(id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            // refresh
            this.refresh();
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
          this.summaryListModel.pageLoader = false;
        },
        error => {
          this.error = error;
          this.loading = false;
          this.summaryListModel.pageLoader = false;
        });

    this.modelRef.close();

  }

  //Export purchase order product details
  export() {
    this.exportDataLoading = true;
    this.service.exportSummary({PurchaseOrderExportData: this.model.purchaseFilterData })
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });

  }
    //Export purchase order product details download xlsx file
   downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Purchase Order Products.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  getEditDetails(id) {  
  
      this.editPage = "poedit/edit-purchaseorder";
      this.viewPage = "poedit/view-purchaseorder";
      this.getDetails(id);    
  
}
  isFormValid() {    
    return this.validator.isValid('customerId') 
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "FromEDate": { 
        this.model.purchaseFilterData.fromEtd=null;
        break; 
     } 
     case "ToEDate": { 
      this.model.purchaseFilterData.toEtd=null;
        break; 
     } 
    }
  }

  //get the booking data by poid
  async getPOBookingDetailsByPoId(poId: number) {
    let response: PoBookingDetailResponse;
    try {
      this.summaryListModel.pageLoader = true;
      response = await this.service.getPOBookingDetails(poId);
      this.processPOBookingResponse(response);
    } catch (e) {
      console.error(e);
      this.summaryListModel.pageLoader = false;
      this.showError("PURCHASEORDER_SUMMARY.TITLE", "COMMON.MSG_UNKNONW_ERROR");
    }
    this.summaryListModel.pageLoader = false;
    return response;
  }

  //process the reponse data
  processPOBookingResponse(response: PoBookingDetailResponse) {
    if(response.result == PoBookingResult.success) {
      this.mapBookingDetails(response.poBookingDetails);
      this.modelRef = this.modalService.open(this.bookingDetailsTemplate, { windowClass: "mdModelWidth", centered: true });
    }
  }

  //map the data from server
  mapBookingDetails(PoBookingDetails: Array<PoBookingDetail>) {
    for (var poBookingItem of PoBookingDetails) {
      var bookingDetail: PoBookingDetail = {
        bookingNumber: poBookingItem.bookingNumber,
        factoryName: poBookingItem.factoryName,
        statusName: poBookingItem.statusName,
        statusColor: poBookingItem.statusColor,
        supplierName: poBookingItem.supplierName,
        serviceDateFrom: poBookingItem.serviceDateFrom,
        serviceDateTo:  poBookingItem.serviceDateTo,
      }
      this.summaryListModel.poBookingDetails.push(bookingDetail);
    }
  }

  openBookingDetail(item) {
    var poId = item.id;
    this.summaryListModel.selectedPoNo = item.pono;
    this.summaryListModel.poBookingDetails = new Array<PoBookingDetail>();
    this.getPOBookingDetailsByPoId(poId);
  }
}
