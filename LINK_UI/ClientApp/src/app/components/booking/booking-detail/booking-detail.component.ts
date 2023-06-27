import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbAccordion, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { Validator } from "../../common/validator"
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { NgxGalleryOptions, NgxGalleryImage } from 'ngx-gallery-9';
import { DetailComponent } from '../../common/detail.component';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { CustomerDecisionSaveRequest } from 'src/app/_Models/booking/inspectioncustomerdecision';
import { RoleEnum, UserType, InspectionServiceType, CustomerDecisionSaveResponseResult } from '../../common/static-data-common';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { InspectionCustomerDecisionService } from 'src/app/_Services/booking/inspectioncustomerdecision.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-booking-detail',
  templateUrl: './booking-detail.component.html',
  styleUrls: ['./booking-detail.component.scss']
})
export class BookingDetailComponent  extends DetailComponent { 
  
  // Page variable declaration
  public  bookingData:any={};
  public  reportData:any={};
  public  reportProducts:any[]=[];
  public  reportContainers:any[]=[];
  public inspectionReportSummaryList:any[]=[];
  public inspectionDefectList:any[]=[];
  public inspectionProductDefectList:any[]=[];
  public inspectionCustomerDecisionList:any[]=[];
  public currentReportId:string;
  public productGalleryOptions: NgxGalleryOptions[];
  public productGalleryImages: NgxGalleryImage[]=[];
  public containerMasterList: any[] = [];


  public additionalproductGalleryOptions: NgxGalleryOptions[];
  public additionalproductGalleryImages: NgxGalleryImage[]=[];

  public pageLoader:boolean=false;
  public isCriticalDefectavailable=false;
  public isMajorDefectavailable=false;
  public isMinorDefectavailable=false;
  public isCriticalProductDefectavailable=false;
  public isMajorProductDefectavailable=false;
  public isMinorProductDefectavailable=false;
  public isCustomerDecisionButtonEnable=false;
  public isCustomerSaveDecisionButton=false;
  public customerDecisionModel:CustomerDecisionSaveRequest;
  public customerDecisionList:any[]=[];
  public modelRef: NgbModalRef;
  public _roleEnum = RoleEnum;
  public editCustomerDecision:boolean=false;
  public viewCustomerDecision:boolean=false;
  public currentUser: UserModel;
  public inspectionServiceType=InspectionServiceType;

  _IsInternalUser: boolean = false;
  _IsCustomerUser: boolean = false;
 

  constructor(validator: Validator, router: Router,  route: ActivatedRoute, 
      public modalService: NgbModal, translate: TranslateService,  authservice: AuthenticationService,public utility: UtilityService,
  public pathroute: ActivatedRoute,private service: BookingService,toastr: ToastrService,private cdService:  InspectionCustomerDecisionService) 
  {
    super(router, route, translate, toastr);
    this.currentUser = authservice.getCurrentUser();

    this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
    this._IsCustomerUser = this.currentUser.usertype == UserType.Customer ? true : false;
   
  }
  
  onInit(): void 
  {   
    this.Initialize();
  }
  getViewPath(): string {
    return "";
  }

  getEditPath(): string {
    return "";
  }
  Initialize():void
  {
    this.pageLoader=true;
    
    var bookingId = this.pathroute.snapshot.paramMap.get("bookingId");
    var reportId = this.pathroute.snapshot.paramMap.get("reportId");
    var containerId = this.pathroute.snapshot.paramMap.get("containerId");
    this.currentReportId=reportId;
    this.productGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];

    this.additionalproductGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];
    this.containerMasterList = this.utility.getContainerList(100);
    this.getBookingAndReportInformation(bookingId,reportId,containerId);
    this.getBookingSummaryInformation(reportId);
    this.getBookingDefectInformation(reportId);
    this.checkAndSetRoles();

  }

  checkAndSetRoles(){
    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.EditInspectionCustomerDecision).length > 0) 
    {
        this.editCustomerDecision=true;
    }

    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.ViewInspectionCustomerDecision).length > 0) 
    {
      this.viewCustomerDecision=true;
    }
  }

  getReplaceSpecialChar(strText){
    
    if(strText && strText!=null)
    {       
        strText=  strText.replace(/\^/g, "");    
    }
    return strText;
  }

  getBookingAndReportInformation(bookingId,reportId,containerId)
  {
    this.service.GetBookingAndReportDetails(bookingId,reportId,containerId)
    .subscribe(res => 
     {
       if(res.result==1)
       {
          this.bookingData=res.bookingData;
          this.reportData=res.reportData;
          this.reportProducts=res.reportProducts;
          this.reportContainers=res.reportContainers;

          this.getBookingCustomerDecision(this.bookingData.customerId);

          this.pageLoader=false;
       }
     },
      error => 
      {
        this.pageLoader=false;
       // this.exportDataLoading = false;
      });
  


  }

  getContainerName(containerId) {
    return this.containerMasterList.length > 0 && containerId != null && containerId != "" ?
      this.containerMasterList.filter(x => x.id == containerId)[0].name : ""
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

  // get product list by container and booking
  getProductListByContainerAndBooking(rowItem) {
    this.service.GetProductListByBookingAndContainer(rowItem.bookingId, rowItem.id)
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

  toggleExpandRowProduct(event, index, rowItem, containerId, serviceType) {
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
      this.getPoListByBookingAndProduct(rowItem, containerId);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

    // get po list by booking and product
    getPoListByBookingAndProduct(rowItem, containerId) {
     
        this.service.GetPODetailsByBookingAndConatinerAndProduct(rowItem.bookingId, containerId, rowItem.id)
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

  getBookingSummaryInformation(reportId)
  {
    this.service.GetGetInspectionSummary(reportId)
    .subscribe(res => 
     {
       if(res.result==1)
       {
       
          this.inspectionReportSummaryList=res.inspectionReportSummaryList;
       }
     },
      error => 
      {
        this.pageLoader=false;
       // this.exportDataLoading = false;
      }); 

  }

  

  getBookingDefectInformation(reportId)
  {
    this.service.GetInspectionDefects(reportId)
    .subscribe(res => 
     {
       if(res.result==1)
       {
       
          this.inspectionDefectList=res.inspectionDefectList;
          this.checkDefectavailable(this.inspectionDefectList);
       }
     },
      error => 
      {
        this.pageLoader=false;
       // this.exportDataLoading = false;
      });
  
  }

  getInspectionCustomerDecisionReportsData(reportId)
  {
    this.cdService.GetInspectionCustomerDecisionReportsData(reportId)
    .subscribe(res => 
     {
       if(res.result==1)
       {       
         this.reportData.customerDecisionStatus=res.status;
         this.reportData.customerDecisionComments=res.comments;
         this.reportData.customerResultId=res.customerResultId;
       }
       this.isCustomerDecisionButtonEnable=false;
     },
      error => 
      {
        this.pageLoader=false;
       // this.exportDataLoading = false;
      });
  
  }

  SetCustomerDecision(resultId) {
    this.customerDecisionModel.customerResultId = resultId;
  }

  getBookingCustomerDecision(customerId)
  {
    this.cdService.GetInspectionCustomerDecision(customerId)
    .subscribe(res => 
     {
       if(res.result==1)
       {       
          this.inspectionCustomerDecisionList=res.customerDecisionList;
       }
     },
      error => 
      {
        this.pageLoader=false;
      });
  
  }

  SaveCustomerDecision(){

    if(this.customerDecisionModel.customerResultId >0 )
    {
      this.isCustomerSaveDecisionButton=true;
      this.customerDecisionModel.bookingId = this.bookingData.bookingId;
      this.cdService.SaveInspectionCustomerDecision(this.customerDecisionModel)
      .subscribe(res => 
       {
         if(res.result==CustomerDecisionSaveResponseResult.success)
         {   
          this.isCustomerSaveDecisionButton=false;
          this.modelRef.close();
          this.getInspectionCustomerDecisionReportsData(this.customerDecisionModel.reportId);
         }
         // After success save, no email send, bcz no email configuration
         else if(res.result==CustomerDecisionSaveResponseResult.noemailconfiguration)         
         {
          this.showWarning('BOOKING_SUMMARY.LBL_CUSTOMER_DECISION', 'BOOKING_SUMMARY.MSG_CU_DECISION_NO_EMAIL_CONFIGURE');
          this.isCustomerSaveDecisionButton=false;
          this.modelRef.close();
          this.getInspectionCustomerDecisionReportsData(this.customerDecisionModel.reportId);
         }
         else if(res.result==CustomerDecisionSaveResponseResult.noroleconfiguration)         
         {
          this.showWarning('BOOKING_SUMMARY.LBL_CUSTOMER_DECISION', 'BOOKING_SUMMARY.MSG_CU_DECISION_NO_Role_CONFIGURE');
          this.isCustomerSaveDecisionButton=false;
          this.modelRef.close();
          this.getInspectionCustomerDecisionReportsData(this.customerDecisionModel.reportId);
         }
       },
        error => 
        {
          this.pageLoader=false;
        });
    }
    else
    {
      this.showError('BOOKING_SUMMARY.LBL_CUSTOMER_DECISION', 'BOOKING_SUMMARY.MSG_SELECT_CU_DECISION');     
    }    
  }

  cancelCustomerDecision()
  {
    this.isCustomerSaveDecisionButton=false;
    this.isCustomerDecisionButtonEnable=false;
    this.modelRef.close();
  }

  checkDefectavailable(defectList){
    this.isCriticalDefectavailable=false;
    this.isMajorDefectavailable=false;
    this.isMinorDefectavailable=false;

    if(defectList && defectList.filter(x=>x.critical!=0 && x.critical!=null).length>0)
    {
      this.isCriticalDefectavailable=true;
    }

    if(defectList && defectList.filter(x=>x.major!=0 && x.major!=null).length>0)
    {
      this.isMajorDefectavailable=true;
    }

    if(defectList && defectList.filter(x=>x.minor!=0 && x.minor!=null).length>0)
    {
      this.isMinorDefectavailable=true;
    }
  }

  checkProductDefectavailable(defectList){

    this.isCriticalProductDefectavailable=false;
    this.isMajorProductDefectavailable=false;
    this.isMinorProductDefectavailable=false;

    if(defectList && defectList.filter(x=>x.critical!=0 && x.critical!=null).length>0)
    {
      this.isCriticalProductDefectavailable=true;
    }

    if(defectList && defectList.filter(x=>x.major!=0 && x.major!=null).length>0)
    {
      this.isMajorProductDefectavailable=true;
    }

    if(defectList && defectList.filter(x=>x.minor!=0 && x.minor!=null).length>0)
    {
      this.isMinorProductDefectavailable=true;
    }
  }

  getBookingDefectInformationbyProducts(reportId,inspPoId)
  {
    this.inspectionProductDefectList=[];
    this.service.GetInspectionDefectsbyProducts(reportId,inspPoId)
    .subscribe(res => 
     {
       if(res.result==1)
       {       
          this.inspectionProductDefectList=res.inspectionDefectList;
         this.checkProductDefectavailable( this.inspectionProductDefectList);
       }
     },
      error => 
      {
        // this.exportDataLoading = false;
      });  
  }  

  getBookingDefectInformationbyContainer(reportId,inspPoId)
  {
    this.inspectionProductDefectList=[];
    this.service.GetInspectionDefectsbyContainer(reportId,inspPoId)
    .subscribe(res => 
     {
       if(res.result==1)
       {       
          this.inspectionProductDefectList=res.inspectionDefectList;
         this.checkProductDefectavailable( this.inspectionProductDefectList);
       }
     },
      error => 
      {
        // this.exportDataLoading = false;
      });  
  }  


  

  downloadReport(reportData)
  {
    if(reportData && reportData.finalManualReportPath)
      window.open(reportData.finalManualReportPath, "_blank");
    else if(reportData && reportData.reportPath)
      window.open(reportData.reportPath, "_blank");
  }

  // returnToPreviouspage() {   
  //   super.NavigatenewPath([`inspsummary/booking-summary/${this.bookingData.bookingId}`]);
  // }

  openDefects(content,inspPoId) {
    this.getBookingDefectInformationbyProducts(this.currentReportId,inspPoId);
		this.modalService.open(content, { windowClass : "mdModelWidth", centered: true ,backdrop: 'static'});
  }

  openDefectsByContainer(content,inspPoId) {
    this.getBookingDefectInformationbyContainer(this.currentReportId,inspPoId);
		this.modalService.open(content, { windowClass : "mdModelWidth", centered: true ,backdrop: 'static'});
  }

  openCustomerDecision(content) {
    this.isCustomerSaveDecisionButton=true;
    this.isCustomerDecisionButtonEnable=true;

    this.cdService.GetInspectionCustomerDecisionByReport(this.currentReportId)
    .subscribe(res => 
     {
      this.customerDecisionModel=new CustomerDecisionSaveRequest();
      this.customerDecisionModel.sendEmailToFactoryContacts=false;
      this.customerDecisionModel.reportId= Number(this.currentReportId);

       if(res.result==1)
       { 
          if(res.customerDecision!=null) 
          {
            this.customerDecisionModel.comments=res.customerDecision.comments;
            this.customerDecisionModel.customerResultId=res.customerDecision.customerResultId;
          }
          
       }
       this.isCustomerSaveDecisionButton=false;
       this.modelRef=	this.modalService.open(content, { windowClass : "smModelWidth", centered: true,backdrop: 'static' });
     },
      error => 
      {
        // this.exportDataLoading = false;
      });  
    


  }
  
  getAdditionalPreviewProductImage(imageList,modalcontent) 
  {
     this.additionalproductGalleryImages = []; 
     imageList.forEach(url => {
      this.additionalproductGalleryImages.push(
        {
          small: url,
          medium: url,
          big: url,
        });
     });
    
     this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true,backdrop: 'static' });    
  }
  
  getPreviewProductImage(imageList,modalcontent) 
  {
     this.productGalleryImages = []; 
     imageList.forEach(url => {
      this.productGalleryImages.push(
        {
          small: url,
          medium: url,
          big: url,
        });
     });
    
     this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true,backdrop: 'static' });    
  }

}
