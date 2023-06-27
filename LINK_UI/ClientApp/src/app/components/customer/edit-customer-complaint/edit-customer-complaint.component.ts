import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { first, switchMap, distinctUntilChanged, tap, catchError, debounceTime, takeUntil } from 'rxjs/operators';
import { BehaviorSubject, of, Subject,from } from 'rxjs';
import { ToastrService } from "ngx-toastr";
import { Validator,JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import {  Url } from '../../common/static-data-common'
import { DetailComponent } from '../../common/detail.component';
import { CustomerComplaintModel,EditCustomerComplaintModel,ComplaintType,
  BookingNoDataSourceRequest,ComplaintBookingProductData,ResponseResult,complaintDetail} from '../../../_Models/customer/customer-complaint.model'
import { CustomerComplaintService} from 'src/app/_Services/customer/customercomplaint.service';
import { LocationService } from '../../../_Services/location/location.service';
import { CustomerService } from '../../../_Services/customer/customer.service';
import {  APIService,InspectionServiceType } from "src/app/components/common/static-data-common";
import { CommonDataSourceRequest, CountryDataSourceRequest,UserDataSourceRequest } from 'src/app/_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
 
@Component({
  selector: 'app-edit-customer-complaint',
  templateUrl: './edit-customer-complaint.component.html',
  styleUrls: ['./edit-customer-complaint.component.scss']
})
export class EditCustomerComplaintComponent extends DetailComponent {
  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public jsonHelper: JsonHelper;

  modelsummary: CustomerComplaintModel;
  model: EditCustomerComplaintModel;
  public complaintDetailValidators: Array<any> = [];
  bookingProductDetailModel: Array<ComplaintBookingProductData>;
  savedataloading:boolean;
  loading:boolean;
  componentDestroyed$: Subject<boolean> = new Subject();
  isFromSummary: boolean = false;

  constructor(private service: CustomerComplaintService,public modalService: NgbModal,
    router: Router, public validator: Validator, translate: TranslateService,
    toastr: ToastrService, route: ActivatedRoute,public customerService:CustomerService,public utility: UtilityService,
    public locationService: LocationService) {
    super(router, route, translate, toastr);
    this.modelsummary = new CustomerComplaintModel();
    this.model = new EditCustomerComplaintModel();
    this._translate = translate;
    this._toastr = toastr;
    this.validator.isSubmitted = false;
    this.validator.setJSON("complaint/edit-complaint.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
  }
  onInit(id?: number) {
    
if(id > 0){
  this.isFromSummary = true;
}
     this.Intitialize(id);
  }
  getViewPath(): string {
    return "edit-customercomplaint";
  }
  getEditPath(): string {
    return "edit-customercomplaint";
  }
  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    if (bookingId && bookingId > 0) {
      var editPage = entity + "/" + Url.EditBooking + bookingId;
      window.open(editPage);
    }
  }
  private Intitialize(id?: number) {
    this.modelsummary = new CustomerComplaintModel();
    this.model = new EditCustomerComplaintModel();
    
   
    this.complaintDetailValidators = [];
 
    this.validator.isSubmitted = false;
    this.getComplaintTypeData();
    this.getComplaintCategory();
    this.getServiceData();
    this.getComplaintRecipientType();
    this.getComplaintDepartment();
    this.getoffice();
    
    if(id>0){
      this.getComplaintDetailsById(id);
       return;
    }
    this.model.complaintTypeId=ComplaintType.General;
    this.setByComplaintType(ComplaintType.General);
    this.getCustomerListBySearch();
    this.getUserListBySearch();
    this.getCountryListBySearch();
  }
  getComplaintDetailsById(id){
  this.service.getComplaintDetailsById(id)
  .pipe()
  .subscribe(
    res => {
      if (res && res.result== ResponseResult.Success) {
        this.processSuccessComplaint(res.data, id);
      }
      else if (res && res.result== ResponseResult.Failed){
        this.processFailedComplaint(res);
      }
      else {
        this.error = res.result;
      }
      //this.initialLoading = false;
    },
    error => {
      this.setError(error);
      //this.initialLoading = false;
    });
  }
  processFailedComplaint(complaint){
    this.model.complaintTypeId=ComplaintType.General;
    this.setByComplaintType(ComplaintType.General);
    this.getCustomerListBySearch();
    this.getUserListBySearch();
    this.getCountryListBySearch();
  }

  processSuccessComplaint(complaint, id){
    var _model: EditCustomerComplaintModel = {
      id: complaint.id,
      complaintTypeId:complaint.complaintTypeId,
      serviceId: complaint.serviceId,
      bookingNo: complaint.bookingNo,
      complaintDate :complaint.complaintDate,
      recipientTypeId: complaint.recipientTypeId,
      departmentId: complaint.departmentId,
      customerId: complaint.customerId,
      countryId:complaint.countryId,
      officeId: complaint.officeId,
      remarks:complaint.remarks,
      userIds: complaint.userIds,
      complaintDetails:null 
    };
    this.model=_model;

    this.getCustomerListBySearch();
    this.getUserListBySearch();
    this.getCountryListBySearch();
    this.setByComplaintType(this.model.complaintTypeId)
    if(this.model.serviceId && this.model.serviceId>=0){
      this.getBookingNoListBySearch() ;
      this.setBookinginfo(this.model.serviceId)
    }
     setTimeout(() => {
        this.mapComplaintDetail(complaint.complaintDetails);
      }, 2000);
    
  }
  mapComplaintDetail(data){
    this.model.complaintDetails=[];
    this.complaintDetailValidators=[];
    this.model.complaintDetails= data.map((x) => {
    var cmpDetail: complaintDetail = {
      id: x.id,
      productId: x.productId ,
      productDesc: "",
      categoryId:x.categoryId,
      description:x.description,
      correctiveAction:x.correctiveAction,
      answerDate:x.answerDate,
      remarks:x.remarks,
      title:x.title,
      productList:null,
      categoryList:null
    };
   
    if(this.modelsummary.showBookingProductDetails)
    {
      this.changeProduct(cmpDetail);
      cmpDetail.productList=this.getProductListFromProductModel();
    }
    cmpDetail.categoryList=this.getCategoryListFromCategoryModel();
    this.complaintDetailValidators.push({ complaintDetails: cmpDetail , validator: Validator.getValidator(cmpDetail, "complaint/edit-complaintdetails.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
    });
  }

  setDefaultComplaintDetails(){
    this.modelsummary.showBookingProductDetails=false;
    this.model.complaintDetails=[];
    this.complaintDetailValidators=[];
    this.addComplaintDetail();
  }

  
  getComplaintTypeData() {
    this.service.getComplaintType()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success)
            this.modelsummary.complaintTypeList = response.complaintDataList;
          else
            this.error = response.result;
          this.modelsummary.complaintTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.complaintTypeLoading = false;
        });
  }
  getComplaintCategory() {
    this.service.getComplaintCategory()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success){
            this.modelsummary.complaintCategoryList = response.complaintDataList;
            this.setDefaultComplaintDetails();
          }
          else
            this.error = response.result;
          this.modelsummary.complaintCategoryLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.complaintCategoryLoading = false;
        });
  }
  getComplaintRecipientType() {
    this.service.getComplaintRecipientType()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success)
            this.modelsummary.complaintRecipientTypeList = response.complaintDataList;
          else
            this.error = response.result;
          this.modelsummary.complaintRecipientTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.complaintRecipientTypeLoading = false;
        });
  }
  getComplaintDepartment() {
    this.service.getComplaintDepartment()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success)
            this.modelsummary.complaintDepartmentList = response.complaintDataList;
          else
            this.error = response.result;
          this.modelsummary.complaintDepartmentLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.complaintDepartmentLoading = false;
        });
  }
  getoffice() {
    this.service.getOffice()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success)
            this.modelsummary.officeList = response.officeList;
          else
            this.error = response.result;
          this.modelsummary.officeLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.officeLoading = false;
        });
  }
  
  onChangeComplaintType(id) {
    this.model.serviceId=null;
    this.model.bookingNo=null;
    this.setDefaultComplaintDetails();
    this.modelsummary.bookingNoList=[];
    this.setByComplaintType(id);
   
  }
  setByComplaintType(id){
    if(id==ComplaintType.Booking){
      this.modelsummary.showBookingControls=true;
    }
    else { 
      this.model.bookingNo=null;
      this.model.serviceId=null;
      this.modelsummary.showBookingControls=false;
      this.modelsummary.showBookingDetails=false;
      this.modelsummary.showBookingProductDetails=false;
    }
  }

  getServiceData() {
    this.service.getService()
    .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == ResponseResult.Success)
            this.modelsummary.serviceList = response.serviceList;
          else
            this.error = response.result;
          this.modelsummary.serviceLoading = false;
        },
        error => {
          this.setError(error);
          this.modelsummary.serviceLoading = false;
        });
  }
  onChangeService(id) {
    this.model.serviceId=id;
    this.model.bookingNo=null;
    this.modelsummary.bookingNoList=[];
    this.setDefaultComplaintDetails();
    this.getBookingNoListBySearch();
  }

   //fetch customer dropdown list
   getCustomerListBySearch() {
    if (this.model.customerId && this.model.customerId > 0) {
      this.modelsummary.customerRequest.id = this.model.customerId;
    }
    else {
      this.modelsummary.customerRequest.id = null;
    }
    this.modelsummary.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.modelsummary.customerLoading = true),
      switchMap(term => term
        ? this.customerService.getCustomerDataSourceList(this.modelsummary.customerRequest, term)
        : this.customerService.getCustomerDataSourceList(this.modelsummary.customerRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.modelsummary.customerLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.modelsummary.customerList = data;
        this.modelsummary.customerLoading = false;
        
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.modelsummary.customerRequest.searchText = this.modelsummary.customerInput.getValue();
    this.modelsummary.customerRequest.skip = this.modelsummary.customerList.length;


    this.modelsummary.customerLoading = true;

    this.modelsummary.customerRequest.id = 0;
    this.customerService.getCustomerDataSourceList(this.modelsummary.customerRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.modelsummary.customerList = this.modelsummary.customerList.concat(customerData);
        }
        this.modelsummary.customerRequest = new CommonDataSourceRequest();
        this.modelsummary.customerLoading = false;
      }),
      error => {
        this.modelsummary.customerLoading = false;
        this.setError(error);
      };
  }

  clearCustomer() {   
    this.model.customerId=null;    
    this.modelsummary.customerRequest.searchText ="";
    this.getCustomerListBySearch();  
}
clearCountry() {   
  this.model.countryId=null;    
  this.modelsummary.countryRequest.searchText ="";
  this.getCountryListBySearch();  
}
  getCountryData() {
    
    this.modelsummary.countryRequest.searchText = this.modelsummary.countryInput.getValue();
    this.modelsummary.countryRequest.skip = this.modelsummary.countryList.length;

    this.modelsummary.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.modelsummary.countryRequest).
      subscribe(countryData => {
        if (countryData && countryData.length > 0) {
          this.modelsummary.countryList = this.modelsummary.countryList.concat(countryData);
        }

        this.modelsummary.countryRequest = new CountryDataSourceRequest();
        this.modelsummary.countryLoading = false;
      }),
      error => {
        this.modelsummary.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    if (this.model.countryId && this.model.countryId > 0) {
      this.modelsummary.countryRequest.countryIds.push(this.model.countryId);
    }
    else {
      this.modelsummary.countryRequest.countryIds=[];
    }
    this.modelsummary.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.modelsummary.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.modelsummary.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.modelsummary.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.modelsummary.countryLoading = false))
      ))
      .subscribe(data => {
        this.modelsummary.countryList = data;
        this.modelsummary.countryLoading = false;
        
      });
  }
  getUserData() {
    this.modelsummary.userRequest.searchText = this.modelsummary.userInput.getValue();
    this.modelsummary.userRequest.skip = this.modelsummary.userList.length;

    this.modelsummary.userLoading = true;
    this.service.getUserDataSourceList(this.modelsummary.userRequest).
      subscribe(userData => {
        if (userData && userData.length > 0) {
          this.modelsummary.userList = this.modelsummary.userList.concat(userData);
        }

        this.modelsummary.userRequest = new UserDataSourceRequest();
        this.modelsummary.userLoading = false;
      }),
      error => {
        this.modelsummary.userLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 Users on load
  getUserListBySearch() {
    this.modelsummary.userInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.modelsummary.userLoading = true),
      switchMap(term => term
        ? this.service.getUserDataSourceList(this.modelsummary.userRequest, term)
        : this.service.getUserDataSourceList(this.modelsummary.userRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.modelsummary.userLoading = false))
      ))
      .subscribe(data => {
        this.modelsummary.userList = data;
        this.modelsummary.userLoading = false;
        
      });
  }
  clearPersonIncharge()
  {
    this.model.userIds=[];    
    this.modelsummary.userRequest.searchText  ="";
    this.getUserListBySearch();  
  }
   //fetch BookingNo dropdown list
   getBookingNoListBySearch() {
     this.modelsummary.showBookingDetails=false;
    this.modelsummary.bookingNoRequest.serviceId=this.model.serviceId;

    if (this.model.bookingNo && this.model.bookingNo > 0) {
      this.modelsummary.bookingNoRequest.id = this.model.bookingNo;
    }
    else {
      this.modelsummary.bookingNoRequest.id = 0;
    }
    this.modelsummary.bookingNoInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.modelsummary.bookingNoLoading = true),
      switchMap(term => term
        ? this.service.getBookingNoDataSourceList(this.modelsummary.bookingNoRequest, term)
        : this.service.getBookingNoDataSourceList(this.modelsummary.bookingNoRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.modelsummary.bookingNoLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.modelsummary.bookingNoList = data;
        this.modelsummary.bookingNoLoading = false;
        
      });
  }

  //fetch the BookingNo data with virtual scroll
  getBookingNoData() {

    this.modelsummary.bookingNoRequest.searchText = this.modelsummary.bookingNoInput.getValue();
    this.modelsummary.bookingNoRequest.skip = this.modelsummary.bookingNoList.length;
    this.modelsummary.bookingNoRequest.serviceId=this.model.serviceId;
    this.modelsummary.bookingNoRequest.id = 0;
    this.modelsummary.bookingNoLoading= true;

    this.service.getBookingNoDataSourceList(this.modelsummary.bookingNoRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(bookingNoData => {
        if (bookingNoData && bookingNoData.length > 0) {
          this.modelsummary.bookingNoList = this.modelsummary.bookingNoList.concat(bookingNoData);
        }
        this.modelsummary.bookingNoRequest = new BookingNoDataSourceRequest();
        this.modelsummary.bookingNoLoading = false;
      }),
      error => {
        this.modelsummary.bookingNoLoading = false;
        this.setError(error);
      };
  }
  clearBookingNo() {
    this.model.bookingNo = null;
    this.modelsummary.bookingNoRequest.searchText ="";
    this.getBookingNoListBySearch();
    this.setDefaultComplaintDetails();
  }
  onChangeBookingNo() {
    if(this.model.bookingNo!= null){
      this.setDefaultComplaintDetails();
      this.setBookinginfo(this.model.serviceId);
    }
    else{
      this.modelsummary.showBookingDetails=false;
    }
  }
  setBookinginfo(serviceId){
    this.modelsummary.showBookingDetails=true;
    if(serviceId==APIService.Inspection){
      this.getBookingDetails();
    }
    else if(serviceId==APIService.Audit){
      this.modelsummary.showBookingProductDetails=false;
      this.getAuditDetails();
    }
  }
  getBookingDetails()
  {
     
    this.service.getBookingDetailsbyId(this.model.bookingNo)
    .pipe(takeUntil(this.componentDestroyed$))
    .subscribe(
      response => {
        this.loading=false;
        if (response && response.result == ResponseResult.Success){
          this.modelsummary.bookingDetailModel = response.data;
         
          if( this.modelsummary.bookingDetailModel.serviceTypeId==InspectionServiceType.Container){
            this.modelsummary.showBookingProductDetails=false;
          }
          else{
            this.modelsummary.showBookingProductDetails=true;
            this.getBookingProductDetails();
            
          }
         }
      },
      error => {
        this.setError(error);
      });
  }
 
  getAuditDetails()
  {
    this.service.getAuditDetailsbyId(this.model.bookingNo)
    .pipe(takeUntil(this.componentDestroyed$))
    .subscribe(
      response => {
        this.loading=false;
        if (response && response.result == ResponseResult.Success){
           this.modelsummary.bookingDetailModel = response.data;
          }
      },
      error => {
        this.setError(error);
      });
  }
  getBookingProductDetails()
  {
    this.bookingProductDetailModel=new Array<ComplaintBookingProductData>();
    this.service.getBookingProductDetailsbyId(this.model.bookingNo)
    .pipe(takeUntil(this.componentDestroyed$))
    .subscribe(
      response => {
        this.loading=false;
        if (response && response.result == ResponseResult.Success){
           this.bookingProductDetailModel = response.data;

            if(this.model.id ||this.model.id<=0){
              this.model.complaintDetails=[];
              this.complaintDetailValidators=[];
              this.addComplaintDetail();
            }
          }
      },
      error => {
        this.setError(error);
      });

      this.modelRef.result.then((result) => {
      }, (reason) => {
      });
  }

  openProductInfoPopUp(content) {
     this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
     
  }
  addComplaintDetail() {
    var cmpDetail: complaintDetail = {
      id: 0,
      productId: null ,
      productDesc: "",
      categoryId:null,
      description:"",
      correctiveAction:"",
      answerDate:null,
      remarks:"",
      title:"",
      productList:null,
      categoryList:null
    };
    
    if(this.modelsummary.showBookingProductDetails)
    {
      cmpDetail.productList=this.getProductListFromProductModel();
    }
    cmpDetail.categoryList=this.getCategoryListFromCategoryModel();
    this.model.complaintDetails.push(cmpDetail);
    this.complaintDetailValidators.push({ complaintDetails: cmpDetail, validator: Validator.getValidator(cmpDetail, "complaint/edit-complaintdetails.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }
  getProductListFromProductModel()
  {
    if(this.bookingProductDetailModel.length>0)
      return this.bookingProductDetailModel.map(item => ({id: item.productId,name: item.productName}));

    return null;    
  }
  getCategoryListFromCategoryModel()
  {
    if(this.modelsummary.complaintCategoryList.length>0)
      return this.modelsummary.complaintCategoryList.map(item => ({id: item.id,name: item.name}));

    return null;    
  }
  changeProduct(item) {
    if(item.productId){
      if (this.bookingProductDetailModel)
        for (let product of this.bookingProductDetailModel) {
          if (product.productId == item.productId) {
            item.productDesc = product.productDescription;
          }
        }
    }
    else
      item.productDesc ="";
  }
  removeComplaintDetail(index, id, content) {
    if (id > 0) {
      var savedDetails = this.complaintDetailValidators.filter(x => x.complaintDetails.id >0);
      if(savedDetails){
        if(savedDetails.length==1){
          this.showWarning('EDIT_COMPLAINT.MSG__ATLEAST_ONE_COMPLAINT_MANDATORY', 'EDIT_COMPLAINT.MSG_CANNOT_DELETE');
          return;
        }
      }
    }
    this.modelsummary.removeDetailId=id;
    this.modelsummary.removeDetailIdIndex=index;

    if (id <= 0) {
      this.removeFromTable();
      return;
    }

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  removeFromTable(){
    this.model.complaintDetails.splice(this.modelsummary.removeDetailIdIndex, 1);
    this.complaintDetailValidators.splice(this.modelsummary.removeDetailIdIndex, 1);
  }
  removeCompDetailConfirm() {
    
    this.service.removeComplaintDetailById(this.modelsummary.removeDetailId)
      .subscribe(
        response => {
          this.loading=false;
          if (response && response.result == 1) {
            this.removeFromTable();
            this.showSuccess('EDIT_COMPLAINT.TITLE', 'COMMON.MSG_DELETE_SUCCESS')
            
          }
          else if (response && response.result == 2) {
            this.showWarning('EDIT_COMPLAINT.MSG_CANNOT_DELETE', 'EDIT_COMPLAINT.MSG_CANNOT_DELETE')
          }
          else {
            this.error = response.result;
          }
         // this.initialLoading = false;
          this.modelRef.close()
        },
        error => {
          this.setError(error);
         // this.initialLoading = false;
        });


  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "complaintDate": { 
        this.model.complaintDate=null;
        break; 
     } 
    }
  }
  save(){
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.modelsummary.savedataloading = true;

    for (let item of this.complaintDetailValidators)
      item.validator.isSubmitted = true;

    if(this.isFormValid())
    {
    var _complaintsDetails:Array<complaintDetail>=[];
    for (let item of this.complaintDetailValidators){
       _complaintsDetails.push(item.complaintDetails)
      }
     
    this.model.complaintDetails=_complaintsDetails;
    this.service.saveComplaint(this.model)
    .subscribe(
      res => {
        this.loading=false;
        this.modelsummary.savedataloading = false;
        if (res && res.result == ResponseResult.Success){
          this.showSuccess('EDIT_COMPLAINT.SAVE_RESULT', 'EDIT_COMPLAINT.SAVE_OK');
          this.Intitialize(0);
        }
        else if (res.result == 6){
         this.showWarning("EDIT_COMPLAINT.SAVE_RESULT", 'EDIT_COMPLAINT.MSG_NOTIFICATIONFAIL');
         this.Intitialize(0);
        }
        else{
          this.showError('EDIT_COMPLAINT.SAVE_RESULT', 'EDIT_COMPLAINT.MSG_SAVE_FAIL');
        }
      },
      error => {
        this.setError(error);
        this.modelsummary.savedataloading = false;
        //this.initialLoading = false;
      });
  }
  else
  {
    this.modelsummary.savedataloading = false;
  }
  }
  isFormValid() {
    
    if(this.model.complaintTypeId){
      if( this.model.complaintTypeId==ComplaintType.Booking){
        return  this.validator.isValid('complaintTypeId')
                    && this.validator.isValid('serviceId')
                    && this.validator.isValid('bookingNo')
                    && this.validator.isValid('complaintDate')
                    && this.validator.isValid('recipientTypeId')
                    && this.validator.isValid('departmentId')
                    && this.validator.isValid('userIds')
                    && this.complaintDetailValidators.every((x) => x.validator.isValid('categoryId'))
                    
         
      }
      else if(this.model.complaintTypeId==ComplaintType.General){
        
        return  this.validator.isValid('complaintTypeId')
                    && this.validator.isValid('complaintDate')
                    && this.validator.isValid('recipientTypeId')
                    && this.validator.isValid('departmentId')
                    && this.validator.isValid('userIds')
                    && this.validator.isValid('customerId')
                    && this.complaintDetailValidators.every((x) => x.validator.isValid('categoryId'))
      }
    }
    return false;
  }
  
}
