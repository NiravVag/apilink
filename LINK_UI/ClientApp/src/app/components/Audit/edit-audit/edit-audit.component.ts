import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { EditAuditmodel, AttachmentFile, AuditMaster } from '../../../_Models/Audit/edit-auditmodel'
import { UserModel } from '../../../_Models/user/user.model'
import { ActivatedRoute, Router, ParamMap } from "@angular/router";
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { AuditService } from '../../../_Services/audit/audit.service'
import { first, takeUntil } from 'rxjs/operators';
import { NgbDate, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { UserType, AuditStatus, AuditSearchRedirectPage, FileContainerList, EntityAccess, AuditDefaultOffice, BookingTypes, AuditTypes, CustomerEnum } from '../../common/static-data-common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CommonDataSourceRequest, CustomerCommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Subject } from 'rxjs';
@Component({
  selector: 'app-edit-audit',
  templateUrl: './edit-audit.component.html',
  styleUrls: ['./edit-audit.component.css']
})

export class EditAuditComponent extends DetailComponent {

  panelOneReady: boolean;
  panelTwoReady: boolean;
  panelThreeReady: boolean;
  panelFourReady: boolean;
  panelFiveReady: boolean;
  isConfirmationVisible: boolean;
  showLoader: boolean;
  @ViewChild('ruleBoxContainer') ruleBoxContainer: ElementRef;
  @ViewChild('panel1') panel1: ElementRef;
  @ViewChild('panel2') panel2: ElementRef;
  @ViewChild('panel3') panel3: ElementRef;
  @ViewChild('panel4') panel4: ElementRef;
  @ViewChild('panel5') panel5: ElementRef;

  public model: EditAuditmodel;
  public data: any;
  Initialloading = false;
  cusloading: boolean = false;
  suploading: boolean = false;
  factloading: boolean = false;
  factoryloading: boolean = false;
  supplierloading: boolean = false;
  savedataloading = false;
  private jsonHelper: JsonHelper;
  IsCustomer: boolean = false;
  Issupplier: boolean = false;
  IsFactory: boolean = false;
  public IsInternalUser: boolean = false;
  public IsconfirmBtnShow = false;
  public IsSaveBtnShow = false;
  _auditBookingTypeEnum = AuditTypes;
  _userTypeId;
  leaddays: NgbDate;
  _Customerlist = [];
  _Seasonyearlist = [];
  _EvaluationRound = [];
  _Auidtorlist = [];
  _Office = [];
  _HolidayDates = [];
  _leadtime: number;
  _BookingRuleDesc: string;
  _contactinformation: string;
  _officedetails: any;
  _suppliercode: string = "";
  _factcode: string = "";
  _factaddress: string = "";
  _factRegaddress: string = "";
  _AuditType = [];
  _AuditWorkProcess = [];
  _AuditCS = [];
  _SupplierList = [];
  currentUser: UserModel;
  _servicefromdatepanel: string;
  _servicetodatepanel: string;
  _Officename: string;
  _cusname: string;
  _suppliername: string;
  _factoryname: string;
  _brand: string;
  _department: string;
  _season: string;
  _year: string;
  _servicetype: string;
  _evaluationroundname: string;
  _audittypename: string;
  _factcreateddate: string;
  _totalattachedfile: number;
  _issuccess: boolean = false;
  fileSize: number;
  uploadLimit: number;
  uploadFileExtensions: string;
  _pagetype: any;
  _auditorpreview: string;
  _cspreview: string;
  downloadloading: boolean = false;
  _ProductCategory = [];
  auditMaster: AuditMaster;
  componentDestroyed$: Subject<boolean> = new Subject();
  _customerEnum = CustomerEnum;
  constructor(toastr: ToastrService,
    private authService: AuthenticationService,
    public utility: UtilityService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public service: AuditService,
    public customerService: CustomerService,
    public refService: ReferenceService,
    public calendar: NgbCalendar,
    public modalService: NgbModal,
    public fileStoreService: FileStoreService, public bookingService: BookingService) {
    super(router, route, translate, toastr, modalService);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("audit/edit-audit.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.currentUser = authService.getCurrentUser();
    this._userTypeId = this.currentUser.usertype;
    this.IsInternalUser = this._userTypeId == UserType.InternalUser ? true : false;
    this.panelOneReady = false;
    this.panelTwoReady = false;
    this.panelThreeReady = false;
    this.panelFourReady = false;
    this.panelFiveReady = false;
    this.isConfirmationVisible = false;
    this.showLoader = false;
    this.uploadLimit = 10;
    this.fileSize = 10000000;
    this.uploadFileExtensions = 'png,jpg,jpeg,xlsx,pdf,doc,docx,xls';
    this.auditMaster = new AuditMaster();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    if (inputparam && inputparam.has("type")) {
      if (Number(inputparam.get("type")) == AuditSearchRedirectPage.Schedule_Auditor) {
        this._pagetype = AuditSearchRedirectPage.Schedule_Auditor;
      }
    }
    this.GetSeasonYear();
    this.GetEvaluationRound();
    this.GetAuditor();
    this.GetOffice();
    this.GetAuditType();
    this.GetAuditWorkProcess();
    this.getAuditServiceTypeList();
    this.Initialize(id);
    this.applicantDetails();
  }
  getViewPath(): string {
    return "auditedit/view-audit";
  }
  getEditPath(): string {
    return "auditedit";
  }
  toggleFromConfirmation(currentpanel) {
    this.isConfirmationVisible = false;
    setTimeout(() => {
      //this.toggleAccordianAuto(null, currentpanel);
      this.TooglePanel(null, currentpanel, currentpanel, true, true);
    }, 200);
  }
  TooglePanel(panelToClose, panelToOpen, currentpanelname, isprevious: boolean, isedit: boolean) {
    switch (currentpanelname) {
      case "panel1":
        {
          this.panelOneReady = isprevious ? false : true;;
          this.panelTwoReady = false;
          this.panelThreeReady = false;
          this.panelFourReady = false;
          this.panelFiveReady = false;
          break;
        }
      case "panel2":
        {
          this.panelOneReady = isprevious && !isedit ? false : true;
          this.panelTwoReady = isprevious ? false : true;
          this.panelThreeReady = false;
          this.panelFourReady = false;
          this.panelFiveReady = false;
          break;
        }
      case "panel3":
        {
          this.panelOneReady = true;
          this.panelTwoReady = isprevious && !isedit ? false : true;
          this.panelThreeReady = isprevious ? false : true;
          this.panelFourReady = false;
          this.panelFiveReady = false;
          this.setpaneldate();
          break;
        }
      case "panel4":
        {
          this.panelOneReady = true;
          this.panelTwoReady = true;
          this.panelThreeReady = isprevious && !isedit ? false : true;
          this.panelFourReady = isprevious ? false : true;
          break;
        }
      case "panel5":
        {
          this.panelFourReady = isprevious ? false : true;
          break;
        }
    }
    this.toggleAccordianAuto(panelToClose, panelToOpen);
  }
  SetPanelItem() {
    if (this._Office && this._Office.length > 0 && this.model.officeid)
      this._Officename = this._Office.filter(x => x.id == this.model.officeid).length > 0 ?
        this._Office.filter(x => x.id == this.model.officeid)[0].name : "";
    if (this._Customerlist && this._Customerlist.length > 0 && this.model.customerid)
      this._cusname = this._Customerlist.filter(x => x.id == this.model.customerid).length > 0 ?
        this._Customerlist.filter(x => x.id == this.model.customerid)[0].name : "";
    if (this.data.customerBrandList && this.data.customerBrandList.length > 0 && this.model.brandid)
      this._brand = this.data.customerBrandList.filter(x => x.id == this.model.brandid).length > 0 ?
        this.data.customerBrandList.filter(x => x.id == this.model.brandid)[0].name : "";
    if (this.data.customerDepartmentList && this.data.customerDepartmentList.length > 0 && this.model.departmentid)
      this._department = this.data.customerDepartmentList.filter(x => x.id == this.model.departmentid).length > 0 ?
        this.data.customerDepartmentList.filter(x => x.id == this.model.departmentid)[0].name : "";
    if (this.data.seasonList && this.data.seasonList.length > 0 && this.model.seasonid)
      this._season = this.data.seasonList.filter(x => x.id == this.model.seasonid).length > 0 ?
        this.data.seasonList.filter(x => x.id == this.model.seasonid)[0].name : "";
    if (this._Seasonyearlist && this._Seasonyearlist.length > 0 && this.model.seasonyearid)
      this._year = this._Seasonyearlist.filter(x => x.id == this.model.seasonyearid).length > 0 ?
        this._Seasonyearlist.filter(x => x.id == this.model.seasonyearid)[0].year : "";
    if (this.data.supplierList && this.data.supplierList.length > 0 && this.model.supplierid)
      this._suppliername = this.data.supplierList.filter(x => x.id == this.model.supplierid).length > 0 ?
        this.data.supplierList.filter(x => x.id == this.model.supplierid)[0].name : "";
    if (this.data.factoryList && this.data.factoryList.length > 0 && this.model.supplierid)
      this._factoryname = this.data.factoryList.filter(x => x.id == this.model.factoryid).length > 0 ?
        this.data.factoryList.filter(x => x.id == this.model.factoryid)[0].name : "";
    if (this.auditMaster.customerServiceList && this.auditMaster.customerServiceList.length > 0 && this.model.servicetypeid)
      this._servicetype = this.auditMaster.customerServiceList.filter(x => x.id == this.model.servicetypeid).length > 0 ?
        this.auditMaster.customerServiceList.filter(x => x.id == this.model.servicetypeid)[0].name : "";
    if (this._EvaluationRound && this._EvaluationRound.length > 0 && this.model.evaluationroundid)
      this._evaluationroundname = this._EvaluationRound.filter(x => x.id == this.model.evaluationroundid).length > 0 ?
        this._EvaluationRound.filter(x => x.id == this.model.evaluationroundid)[0].name : "";
    if (this._AuditType && this._AuditType.length > 0 && this.model.audittypeid)
      this._audittypename = this._AuditType.filter(x => x.id == this.model.audittypeid).length > 0 ?
        this._AuditType.filter(x => x.id == this.model.audittypeid)[0].name : "";
    if (this.model.attachments && this.model.attachments.length > 0)
      this._totalattachedfile = this.model.attachments.length;
    if (this._Auidtorlist && this._Auidtorlist.length > 0 && this.model.auditors) {
      this._auditorpreview = "";
      this._Auidtorlist.forEach(x => {
        if (this.model.auditors.includes(x.id)) {
          this._auditorpreview = this._auditorpreview + "," + x.name;
        }
      });
      if (this._auditorpreview.length > 0)
        this._auditorpreview = this._auditorpreview.substring(1, this._auditorpreview.length);
    }
    if (this._AuditCS && this._AuditCS.length > 0 && this.model.auditCS) {
      this._cspreview = this._AuditCS.filter(x => this.model.auditCS.includes(x.id)).map(x => x.name).join(", ");
    }

  }
  setpaneldate() {
    if (this.model.servicedatefrom) {
      var fromdate: NgbDate = this.model.servicedatefrom;
      this._servicefromdatepanel = fromdate.month + "/" + fromdate.day + "/" + fromdate.year;
    }
    if (this.model.servicedateto) {
      var todate: NgbDate = this.model.servicedateto;
      this._servicetodatepanel = todate.month + "/" + todate.day + "/" + todate.year;
    }
    if (this.model.factorycreationdate) {
      var factdate: NgbDate = this.model.servicedateto;
      this._factcreateddate = factdate.month + "/" + factdate.day + "/" + factdate.year;
    }
  }
  toggleAccordianAuto(panelToClose, panelToOpen) {
    var panelOpen;
    var panelClose;

    if (panelToOpen == 'panel1') {
      panelOpen = this.panel1;
    }
    else if (panelToOpen == 'panel2') {
      panelOpen = this.panel2
    }
    else if (panelToOpen == 'panel3') {
      panelOpen = this.panel3
    }
    else if (panelToOpen == 'panel4') {
      panelOpen = this.panel4
    }
    else if (panelToOpen == 'panel5') {
      panelOpen = this.panel5
    }

    if (panelToClose == 'panel1') {
      panelClose = this.panel1;
    }
    else if (panelToClose == 'panel2') {
      panelClose = this.panel2
    }
    else if (panelToClose == 'panel3') {
      panelClose = this.panel3
    }
    else if (panelToClose == 'panel4') {
      panelClose = this.panel4
    }
    else if (panelToClose == 'panel5') {
      panelClose = this.panel5
    }
    else {
      panelClose = null;
    }

    if (panelClose) {
      panelClose.nativeElement.classList.remove('active');
      panelClose.nativeElement.querySelector('.panel-body').classList.remove('active');

      let accordion = panelClose.nativeElement.querySelector('.panel-body').nextElementSibling;
      accordion.style.maxHeight = 0;
    }

    panelOpen.nativeElement.classList.add('active');
    panelOpen.nativeElement.querySelector('.panel-body').classList.add('active');
    let accordion = panelOpen.nativeElement.querySelector('.panel-body').nextElementSibling;

    if (accordion.style.maxHeight) {
      accordion.style.maxHeight = "unset";
    } else {
      accordion.style.height = 'auto';
      // accordion.style.maxHeight = accordion.scrollHeight + 'px';
    }
  }

  toggleRuleBox(event) {
    this.ruleBoxContainer.nativeElement.classList.toggle('active');
  }

  toggleConfirmation() {
    this.SetPanelItem();
    this.isConfirmationVisible = !this.isConfirmationVisible;
  }
  Initialize(id?) {
    this.data = {};
    this.model = new EditAuditmodel();
    this._officedetails = {};
    this.model.attachments = [];
    this.Initialloading = true;
    this.validator.isSubmitted = false;
    this.GetCustomerByUserType();
    this.service.EditAudit(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id) {
              var entity = res.auditDetails;
              this.GetCusRelatedDetailsById(entity.customerId, true);
              this.getSupRelatedDetailsById(entity.supplierId, entity.customerId, true);
              this.geFactRelatedDetailsById(entity.factoryId, entity.customerId, true);
              this.GetAuditBookingContact(entity.factoryId);
              this.GetAuditBookingRuleDetails(entity.customerId, entity.factoryId);
              this.GetAuditCSDetails(entity.factoryId, entity.customerId);
              debugger;
              this.model = {
                StatusId: entity.statusId,
                accreditations: entity.accreditations,
                adminstaff: entity.adminStaff,
                annualholidays: entity.annualHolidays,
                auditid: entity.auditId,
                auditbookingno: entity.auditBookingNo,
                customerid: entity.customerId,
                brandid: entity.brandId,
                departmentid: entity.departmentId,
                servicetypeid: entity.serviceTypeId,
                servicedatefrom: entity.serviceDateFrom,
                servicedateto: entity.serviceDateTo,
                seasonid: entity.seasonId,
                seasonyearid: entity.seasonYearId,
                evaluationroundid: entity.evaluationRoundId,
                applicantname: entity.applicantName,
                applicantemail: entity.applicantEmail,
                applicantphno: entity.applicantPhNo,
                supplierid: entity.supplierId,
                factoryid: entity.factoryId,
                factorycreationdate: entity.factoryCreationDate,
                noofcustomers: entity.noOfCustomers,
                noofsuppliers: entity.noOfSuppliers,
                annualproduction: entity.annualProduction,
                maximumcapacity: entity.maximumCapacity,
                factorysurfacearea: entity.factorySurfaceArea,
                totalcapacitybycustomer: entity.totalCapacityByCustomer,
                factoryextension: entity.factoryExtension,
                manufactureproducts: entity.manufactureProducts,
                brandsproduced: entity.brandsProduced,
                numberofsites: entity.numberOfSites,
                openhour: entity.openHour,
                productionstaff: entity.productionStaff,
                qualitystaff: entity.qualityStaff,
                totalstaff: entity.totalStaff,
                salesstaff: entity.salesStaff,
                investment: entity.investment,
                liability: entity.liability,
                tradeassociation: entity.tradeAssociation,
                customerComments: entity.customerComments,
                aPIComments: entity.apiComments,
                internalComments: entity.internalComments,
                ponumber: entity.poNumber,
                reportno: entity.reportNo,
                officeid: entity.officeId,
                customercontactlistitems: entity.customerContactListItems,
                suppliercontactlistitems: entity.supplierContactListItems,
                factorycontactlistitems: entity.factoryContactListItems,
                isemailrequired: entity.isEmailRequired,
                attachments: entity.attachments,
                createdbyUserType: entity.createdbyUserType,
                auditCS: entity.auditCS,
                auditors: entity.auditors,
                audittypeid: entity.auditTypeid,
                auditworkprocessItems: entity.auditworkprocessItems,
                customerBookingNo: entity.customerBookingNo,
                auditProductCategoryId: entity.auditProductCategoryId,
                isCustomerEmailSend:false,
                isSupplierOrFactoryEmailSend:true,              
                isEaqf:entity.isaEaqf
              };
              if (this.model.customerid > 0 && this.model.servicetypeid > 0)
                this.getProductCategory();
              else
                this._ProductCategory = [];
            }
            else {
              this.NewAuditDefaultDataByUser(res)
            }
            this.SetUserType();
          }
          else {
            this.ErrorDetails(res);
          }
          this.Initialloading = false;
        },
        error => {
          this.setError(error);
          this.Initialloading = false;
          console.log(error != null ? error : "");
        }
      );
  }
  ShowNewBooking() {
    this.Initialize();
    this._totalattachedfile = null;
  }
  NewAuditDefaultDataByUser(res) {
    switch (res.userType) {
      case UserType.Customer:
        {
          this.model.StatusId = AuditStatus.Requested;
        }
        break;
      case UserType.Supplier:
        // if supplier login assign supplierList at page load
        this.data.supplierList = res.supplierList.length > 0 ? res.supplierList : null;
        this.model.supplierid = this.model.supplierid == null && res.supplierList.length > 0 ? res.supplierList[0].id : this.model.supplierid;
        this.model.StatusId = AuditStatus.Requested;
        this.getSupRelatedDetailsById(this.model.supplierid, null, false);
        break;
      case UserType.Factory:
        {
          //factory login assign factoryList at page load
          this.data.factoryList = res.factoryList.length > 0 ? res.factoryList : null;
          this.model.factoryid = this.model.factoryid == null && res.factoryList.length > 0 ? res.factoryList[0].id : this.model.factoryid;
          this.model.StatusId = AuditStatus.Requested;
          this.geFactRelatedDetailsById(this.model.factoryid, null, false);
          this.GetAuditBookingContact(this.model.factoryid);
          break;
        }
      case UserType.InternalUser:
        this.model.StatusId = AuditStatus.Requested;
        break;
    }
  }
  SetUserType() {
    switch (this.currentUser.usertype) {
      case UserType.Customer:
        {
          this.IsCustomer = true;
          this.IsSaveBtnShow = this.model && this.model.StatusId != null && (this.model.StatusId == AuditStatus.Requested) ? true : false;
          this.toggleFromConfirmation('panel2');
        }
        break;
      case UserType.Supplier:
        {
          this.Issupplier = true;
          this.IsSaveBtnShow = this.model && this.model.StatusId != null && (this.model.StatusId == AuditStatus.Requested) ? true : false;
          this.toggleFromConfirmation('panel2');
          break;
        }

      case UserType.Factory:
        {
          this.IsFactory = true;
          this.IsSaveBtnShow = this.model && this.model.StatusId != null && (this.model.StatusId == AuditStatus.Requested) ? true : false;
          this.toggleFromConfirmation('panel2');
          break;
        }
      case UserType.InternalUser:
        {
          this.IsconfirmBtnShow = this.model && this.model.auditid != null && this.model.StatusId != null && (this.model.StatusId == AuditStatus.Requested || this.model.StatusId == AuditStatus.Rescheduled) ? true : false;
          this.IsSaveBtnShow = true;
          if (this._pagetype != AuditSearchRedirectPage.Schedule_Auditor)
            this.toggleFromConfirmation('panel1');
          else
            this.toggleFromConfirmation('panel5');
          break;
        }

    }
    if (this.model && this.model.auditid != null && !this.IsInternalUser && this.IsSaveBtnShow && this.model.createdbyUserType != this.currentUser.usertype) {
      this.IsSaveBtnShow = false;
    }
    this.Initialloading = false;
    this.isConfirmationVisible = false;
    this._issuccess = false;
  }
  getCusRelatedDetails(cusitem) {
    this.ResetCustomerChange();
    if (cusitem != null && cusitem.id != null) {
      this.GetCusRelatedDetailsById(cusitem.id, false);
    }
  }
  GetCusRelatedDetailsById(cusid, isEdit) {
    this.supplierloading = this.Issupplier ? false : true;
    this.cusloading = true;
    this.service.GetAuditDetailsByCusId(cusid)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 19) {
            this.data.customerContactList = res.customerContactList;
            this.data.customerBrandList = res.customerBrandList;
            this.data.customerDepartmentList = res.customerDepartmentList;
            //this.data.customerServiceList = res.customerServiceList;
            this.data.seasonList = res.seasonList;
            this.data.supplierList = isEdit || !this.Issupplier ?
              res.supplierList : this.data.supplierList;
            if (this.Issupplier && !isEdit) {
              this.data.supplierContactList = res.supplierContactList;
              this._suppliercode = res.supplierCode;
              // if supplier login factory details should load after customer select
              this.getSupRelatedDetailsById(this.model.supplierid, cusid, isEdit);
            }
            if (this.IsFactory && !isEdit) {
              this.data.factoryContactList = res.factoryContactList;
              this._factcode = res.factoryCode;
              this.model.officeid = res.officeId != 0 ? res.officeId : this.model.officeid;
              this.GetAuditBookingRuleDetails(cusid, this.model.factoryid);
              this.GetAuditCSDetails(this.model.factoryid, cusid);
            }
          }
          else {
            this.ErrorDetails(res);
            this.ResetSupplierDetails();
          }
          this.cusloading = false;
          this.supplierloading = false;
        },
        error => {
          this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          this.cusloading = false;
          this.supplierloading = false;
          console.log(error);
        }
      );
  }
  getSupRelatedDetails(supitem, isEdit) {
    this.RestFactorySupplier();
    if (supitem != null && supitem.id != null) {
      var cusid = this.model.customerid == null || this.model.customerid == 0 ? null : this.model.customerid;
      this.getSupRelatedDetailsById(supitem.id, cusid, isEdit);
    }
  }
  getSupRelatedDetailsById(supid, cusid, isEdit) {
    if (supid > 0 && cusid > 0) {
      this.factoryloading = this.IsFactory ? false : true;
      this.suploading = true;
      this.service.GetSupplierDetailsByCusIdSupId(supid, cusid)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 20) {
              this.data.supplierContactList = res.supplierContactList;
              this._suppliercode = res.supplierCode;
              if (isEdit || !this.IsFactory) {
                if (res.factoryList != null) {
                  this.data.factoryList = res.factoryList;
                }
                else {
                  this.data.factoryList = [];
                  this.ResetFactoryDetails();
                  this.ErrorDetails(res);
                }
              }
            }
            else if (res && res.result == 25) {
              this.ErrorDetails(res);
              this.data.factoryList = [];
            }
            else
              this.data.factoryList = [];
            this.suploading = false;
            this.factoryloading = false;
          },
          error => {
            this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
            this.suploading = false;
            this.factoryloading = false;
            console.log(error);
          }
        );
    }
  }
  geFactRelatedDetails(factitem) {
    if (factitem != null && factitem.id != null) {
      var cusid = this.model.customerid == null || this.model.customerid == 0 ? null : this.model.customerid;
      this.geFactRelatedDetailsById(factitem.id, cusid, false);
      this.GetAuditBookingContact(factitem.id);
      this.GetAuditCSDetails(factitem.id, cusid);
      if (cusid != null)
        this.GetAuditBookingRuleDetails(cusid, factitem.id);
    }
  }
  geFactRelatedDetailsById(factid, cusid, IsEdit) {
    this.factloading = true;
    this.service.GetFactoryDetailsByCusIdFactId(factid, cusid)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 23) {
            this.data.factoryContactList = res.factoryContactList;
            if (this.IsFactory)
              this.data.supplierList = IsEdit ? this.data.supplierList : res.supplierList;
            this._factcode = res.factoryCode;
            this._factaddress = res.factoryAddress;
            this._factRegaddress = res.factoryRegionalAddress;
            this.model.officeid = res.officeId != 0 && (this.model.officeid == null || this.model.officeid == 0) ? res.officeId : this.model.officeid;
          }
          else {
            this.ErrorDetails(res);
          }
          this.factloading = false;
        },
        error => {
          this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          this.factloading = false;
          console.log(error);
        }
      );
  }
  ErrorDetails(res) {
    switch (res.result) {
      case 2:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_CUSTOMER');
        break;
      case 5:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_CUSTOMER_CONTACT');
        break;
      case 7:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_CUSTOMER_SERVICETYPE');
        break;
      case 6:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANONT_GET_CUSTOMER_SEASON');
        break;
      case 13:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_SUPPLIER');
        break;
      case 21:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_SUPPLIER_CONTACT');
        break;
      case 14:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_FACTORY');
        break;
      case 25:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_FACTORY');
        break;
      case 24:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_FACTORY_CONTACT');
        break;
      case 10:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_LOCATION');
        break;
      case 28:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_BOOKINGRULE');
        break;
      default:
        this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
        break;
    }
  }
  ResetDetailsByCustomerId() {
    switch (this._userTypeId) {
      case UserType.Customer:
        {
          this.model.customercontactlistitems = [];
          this.model.brandid = null
          this.model.departmentid = null;
          this.model.servicetypeid = null;
          this.model.seasonid = null;
          this.model.supplierid = null;
          this.ResetSupplierDetails();
          this.model.servicetypeid = null;
          break;
        }
      case UserType.Supplier:
        {
          this.data.customerContactList = [];
          this.model.customercontactlistitems = [];
          this.data.customerBrandList = [];
          this.model.brandid = null
          this.data.customerDepartmentList = [];
          this.model.departmentid = null;
          this.auditMaster.customerServiceList = [];
          this.model.servicetypeid = null;
          this.data.seasonList = [];
          this.model.seasonid = null;
          this.data.supplierContactList = [];
          this.model.suppliercontactlistitems = [];
          this._suppliercode = "";
          this.ResetFactoryDetails();
          break;
        }
      case UserType.Factory:
        {
          this.data.customerContactList = [];
          this.model.customercontactlistitems = [];
          this.data.customerBrandList = [];
          this.model.brandid = null
          this.data.customerDepartmentList = [];
          this.model.departmentid = null;
          this.auditMaster.customerServiceList = [];
          this.model.servicetypeid = null;
          this.data.seasonList = [];
          this.model.seasonid = null;
          this.data.factoryContactList = [];
          this.model.factorycontactlistitems = [];
          this._factcode = "";
          this.ResetSupplierDetails();
          break;
        }
      case UserType.InternalUser:
        this.Initialize();
        this._suppliercode = "";
        this._factcode = "";
        this._factaddress = "";
        this._factRegaddress = "";
        break;
    }

  }
  ResetSupplierDetails() {

    switch (this._userTypeId) {
      case UserType.Customer:
        {
          this.data.supplierContactList = [];
          this.model.suppliercontactlistitems = [];
          this._suppliercode = "";
          this.data.factoryList = [];
          this.model.factoryid = null;
          this.ResetFactoryDetails();
          break;
        }
      case UserType.Factory:
        {
          this.model.supplierid = null;
          this.data.supplierContactList = [];
          this.model.suppliercontactlistitems = [];
          this._suppliercode = "";
          break;
        }
      case UserType.InternalUser:
        this.data.supplierContactList = [];
        this.model.suppliercontactlistitems = [];
        this._suppliercode = "";
        this.data.factoryList = [];
        this.model.factoryid = null;
        this.ResetFactoryDetails();
        break;
    }
  }
  RestFactorySupplier() {
    //clear factory data  if factory is not login
    if (!this.IsFactory) {
      this.model.factoryid = null;
      this._factcode = "";
      this._factaddress = "";
      this._factRegaddress = "";
      this.model.factorycontactlistitems = [];
      this.data.factoryContactList = [];
    }
    this._suppliercode = "";
    this.model.suppliercontactlistitems = [];
    this.data.supplierContactList = [];
  }
  //if we changed customer drop down value we need to clear related data.
  ResetCustomerChange() {
    //clear supplier data if supplier is not login
    if (!this.Issupplier)
      this.model.supplierid = null;
    this.model.customercontactlistitems = [];
    this.data.customerContactList = [];
    this.RestFactorySupplier();
    this.model.departmentid = null;
    this.data.customerDepartmentList = [];
    this.model.brandid = null;
    this.data.customerBrandList = [];
    this.model.servicetypeid = null;
    this.changeServiceType();
  }
  ResetFactoryDetails() {
    this.model.factoryid = null;
    this._factcode = "";
    this._factaddress = "";
    this._factRegaddress = "";
    this.data.factoryContactList = [];
    this.model.factorycontactlistitems = [];
  }
  onDateSelection(date: NgbDate) {
    if (this.model.servicedateto == null && this.model.servicedatefrom != null) {
      this.model.servicedateto = this.model.servicedatefrom;
    }
    if (this.model.servicedateto < this.model.servicedatefrom) {
      this.model.servicedateto = null;
    }

  }
  IsPanelDataValid(panelToClose, panelToOpen, currentpanelname, isprevious, event) {
    var isok = false;
    this.validator.initTost();
    this.validator.isSubmitted = true;
    switch (currentpanelname) {
      case "panel1":
        {
          if (UserType.InternalUser == this._userTypeId) {
            isok = this.validator.isValid('officeid');
          }
          break;
        }
      case "panel2":
        {

          isok = this.validator.isValid('customerid') &&
            this.validator.isValid('supplierid') &&
            this.validator.isValid('factoryid') &&
            this.IsCustomerContactListValid() &&
            this.IsSupplierContactListValid() &&
            this.IsfactoryContactListValid() &&
            this.validator.isValid('seasonid') &&
            this.validator.isValid('seasonyearid')
          if (isok && UserType.InternalUser != this._userTypeId) {
            isok = this.validator.isValid('applicantname') &&
              this.validator.isValid('applicantemail') &&
              this.validator.isValid('applicantphno');
          }
          break;
        }
      case "panel3":
        {

          isok = this.validator.isValid('servicetypeid') &&
            this.validator.isValid('servicedatefrom') &&
            this.validator.isValid('servicedateto') &&
            this.validator.isValid('evaluationroundid') &&
            this.validator.isValid('audittypeid') &&
            this.IsProductCategoryValid();
          if (isok)
            isok = this.validator.isValid('totalstaff');
          break;


        }
      case "panel4":
        {
          isok = true;
          break;
        }
      case "panel5":
        {
          isok = true;
          break;
        }
    }
    if (isok) {
      this.validator.isSubmitted = false;
      this.TooglePanel(panelToClose, panelToOpen, currentpanelname, isprevious, false);
      this.SetPanelItem()
    }
    else {
      this.validator.isSubmitted = true;
    }

  }
  IsFormValid() {
    switch (this._userTypeId) {
      case UserType.InternalUser:
        {
          return this.validator.isValid('customerid') &&
            this.validator.isValid('servicetypeid') &&
            this.validator.isValid('servicedatefrom') &&
            this.validator.isValid('servicedateto') &&
            this.validator.isValid('evaluationroundid') &&
            this.validator.isValid('supplierid') &&
            this.validator.isValid('factoryid') &&
            this.validator.isValid('officeid') &&
            this.validator.isValid('audittypeid') &&
            this.IsCustomerContactListValid() &&
            this.IsSupplierContactListValid() &&
            this.IsfactoryContactListValid() &&
            this.IsProductCategoryValid();
        }
      default:
        {
          return this.validator.isValid('customerid') &&
            this.validator.isValid('servicetypeid') &&
            this.validator.isValid('servicedatefrom') &&
            this.validator.isValid('servicedateto') &&
            this.validator.isValid('evaluationroundid') &&
            this.validator.isValid('supplierid') &&
            this.validator.isValid('factoryid') &&
            this.validator.isValid('factoryid') &&
            this.validator.isValid('applicantname') &&
            this.validator.isValid('applicantemail') &&
            this.validator.isValid('applicantphno') &&
            this.validator.isValid('totalstaff') &&
            this.IsCustomerContactListValid() &&
            this.IsSupplierContactListValid() &&
            this.IsfactoryContactListValid() &&
            this.IsProductCategoryValid()
        }
    }
  }
  IsProductCategoryValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this._ProductCategory && this._ProductCategory.length > 0 && this.model.customerid == this._customerEnum.Gap)
      return this.validator.isValid('auditProductCategoryId');
    else
      return true;
  }
  IsCustomerContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.model.customercontactlistitems != null && this.model.customercontactlistitems.length > 0) {
      this.validator.isValid('customercontactlistitems');
      return true;
    }
    else
      return false;
  }
  IsSupplierContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.model.suppliercontactlistitems != null && this.model.suppliercontactlistitems.length > 0) {
      this.validator.isValid('suppliercontactlistitems');
      return true;
    }
    else
      return false;
  }
  IsfactoryContactListValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.model.factorycontactlistitems != null && this.model.factorycontactlistitems.length > 0) {
      this.validator.isValid('factorycontactlistitems');
      return true;
    }
    else
      return false;
  }
  save() {
    try {
      this.validator.isSubmitted = true;
      if (this.IsFormValid()) {
        this.savedataloading = true;
        var attachedList = this.model.attachments.filter(x => x.isNew);
        this.IsScheduled();
        if (!this.model.audittypeid)
          this.model.audittypeid = null;
        this.service.saveAudit(this.model)
          .subscribe(
            res => {
              if (res && res.result == 1) {
                //if (attachedList.length > 0) {
                // this.service.uploadAttachedFiles(res.id, attachedList).subscribe(Isupload => {
                //   if (Isupload) {

                //     if (this.fromSummary) {
                //       this.showSuccess("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_SAVE_SUCCESS");
                //       this.return("auditsummary/audit-summary");
                //     }
                //     else {
                //       this.model.auditid = res.id;
                //       this._issuccess = true;
                //     }
                //   }
                //   else {
                //     this.showWarning("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_SAVE_SUCCESS_FILE_NOT_UPLOAD");
                //   }
                //   this.savedataloading = false;
                // },
                // error => { this.savedataloading = false; this.showWarning("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_SAVE_SUCCESS_FILE_NOT_UPLOAD"); });
                //}
                //else {
                // this._issuccess = true;
                if (this.fromSummary) {
                  this.showSuccess("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_SAVE_SUCCESS");
                  this.return("auditsummary/audit-summary");
                }
                else {
                  this._issuccess = true;
                  this.model.auditid = res.id;
                }
                //}
                this.savedataloading = false;
              }
              else {
                switch (res.result) {
                  case 2:
                    this.showError("EDIT_AUDIT.TITLE", 'EDIT_AUDIT.MSG_CANNOT_SAVE_AUDIT');
                    break;
                  case 3:
                    this.showError("EDIT_AUDIT.TITLE", 'EDIT_AUDIT.MSG_FORMAT_ERROR');
                    break;
                  case 4:
                    this.showError("EDIT_AUDIT.TITLE", 'EDIT_AUDIT.MSG_NO_ITEM_FOUND');
                    break;
                  case 5:
                    this.showError("EDIT_AUDIT.TITLE", 'EDIT_AUDIT.MSG_AUDIT_NOT_UPDATED');
                    break;
                }
                this.savedataloading = false;
              }
            },
            error => {
              this.showError("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_UNKNOWN_ERROR");
              this.savedataloading = false;
            });
      }
    }
    catch (e) {
      this.showError("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_UNKNOWN_ERROR");
    }
  }
  IsScheduled() {
    if (this.model.auditors && this.model.auditors.length > 0 && this.model.StatusId == AuditStatus.Confirmed && this.IsInternalUser)
      this.model.StatusId = AuditStatus.Scheduled;
    if (this.model.auditors && this.model.auditors.length <= 0 && (this.model.StatusId == AuditStatus.Scheduled || this.model.StatusId == AuditStatus.Audited) && this.IsInternalUser)
      this.model.StatusId = AuditStatus.Confirmed;
  }
  selectFiles(event) {
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.fileSize,
      uploadFileExtensions: this.uploadFileExtensions,
      uploadLimit: 10,
      containerId: FileContainerList.Audit,
      token: "",
      fileDescription: null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {
          if (this.model.attachments) {
            this.model.attachments.push(element);
          }
        });
      }

    }, (reason) => {

    });
    // if (event && !event.error && event.files) {

    //   if (event.files.length == null)
    //     this.model.attachments = [];

    //   for (let file of event.files) {
    //     let fileItem: AttachmentFile = {
    //       fileName: file.name,
    //       file: file,
    //       isNew: true,
    //       id: 0,
    //       mimeType: "",
    //       uniqueld: this.newGuid()
    //     };
    //     this.model.attachments.push(fileItem);
    //   }
    //   //event.srcElement.value = null;
    // }
    // else if (event && event.error && event.errorMessage) {
    //   this.showError('EDIT_AUDIT.TITLE', event.errorMessage);
    // }
  }
  removeAttachment(index) {
    this.model.attachments.splice(index, 1);
  }
  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  Reset() {
    this.model.customerid = null;
    this.model.attachments = [];
    this.model.applicantemail = '';
    this.model.applicantname = '';
    this.model.applicantphno = '';
    this.model.seasonyearid = null;
    this.model.evaluationroundid = null;
    this.ResetDetailsByCustomerId();
    this.model.accreditations = null;
    this.model.adminstaff = null;
    this.model.annualholidays = null;
    this.model.servicedatefrom = null;
    this.model.servicedateto = null;
    this.model.factorycreationdate = null;
    this.model.factorycreationdate = null;
    this.model.noofcustomers = null;
    this.model.noofsuppliers = null;
    this.model.annualproduction = null;
    this.model.maximumcapacity = null;
    this.model.factorysurfacearea = null;
    this.model.totalcapacitybycustomer = null;
    this.model.factoryextension = null;
    this.model.manufactureproducts = null;
    this.model.brandsproduced = null;
    this.model.numberofsites = null;
    this.model.openhour = null;
    this.model.productionstaff = null;
    this.model.qualitystaff = null;
    this.model.totalstaff = null;
    this.model.salesstaff = null;
    this.model.investment = null;
    this.model.liability = null;
    this.model.tradeassociation = null;
    this.model.customerComments = null;;
    this.model.aPIComments = null;
    this._BookingRuleDesc = null;
    this._contactinformation = null;
    this.model.isSupplierOrFactoryEmailSend=true;
    this.model.isCustomerEmailSend=false;
    this._officedetails = {};
  }
  getFile(file: AttachmentFile) {

    this.downloadloading = true;

    if (file.fileUrl) {
      this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.Audit)
        .subscribe(res => {
          this.downloadFile(res, file.mimeType, file.fileName);
        },
          error => {
            this.downloadloading = false;
            this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_UNKNOWN_ERROR');
          });
      this.downloadloading = false;
    }
  }
  downloadFile(data, mimeType, filename) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
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
  }
  // downloadFile(data, mimeType) {
  //   const blob = new Blob([data], { type: mimeType });
  //   if (window.navigator && window.navigator.msSaveOrOpenBlob) {
  //     window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
  //   }
  //   else {
  //     const url = window.URL.createObjectURL(blob);
  //     window.open(url);
  //   }
  // }
  ConfirmAudit() {
    if ((this.model.StatusId == AuditStatus.Requested || this.model.StatusId == AuditStatus.Rescheduled) && this.IsInternalUser) {
      this.model.StatusId = AuditStatus.Confirmed;
      this.save();
    }
    else {
      this.showError("EDIT_AUDIT.TITLE", "EDIT_AUDIT.MSG_Data_Error");
    }
  }
  isDisabled = (date: NgbDate, current: { month: number }) => {
    if (!this.IsInternalUser) {
      return date.before(this.calendar.getToday()) || (this._HolidayDates != null && this._HolidayDates.filter(x => date.equals(x)).length > 0)
        || this.leaddays && (date.before(this.leaddays) || date.equals(this.leaddays))
    };
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
  GetCustomerByUserType() {
    let customerDataSourceRequest = new CustomerCommonDataSourceRequest();
    customerDataSourceRequest.isVirtualScroll = false;
    this.customerService.getCustomerListByUserType(customerDataSourceRequest).subscribe(
      response => {
        if (response) {
          this._Customerlist = response;
        }
      }
    );
  }
  GetSeasonYear() {
    this.service.GetSeasonYear().subscribe(
      response => {
        if (response && response.result == 1) {
          this._Seasonyearlist = response.seasonYearList;
        }
      }
    );
  }
  GetEvaluationRound() {
    this.service.GetEvaluationRound().subscribe(
      response => {
        if (response && response.result == 1) {
          this._EvaluationRound = response.evaluationRoundList;
        }
      }
    );
  }
  GetOffice() {
    this.service.GetOffice().subscribe(
      response => {
        if (response && response.result == 1) {
          this._Office = response.officeList;

          var currentEntityId = Number(this.utility.getEntityId());

          if (currentEntityId == EntityAccess.API)
            this.model.officeid = AuditDefaultOffice;
        }
      }
    );
  }
  GetAuditor() {
    this.service.GetAuditor().subscribe(
      response => {
        if (response && response.result == 1) {
          this._Auidtorlist = response.auditors;
        }
      }
    );
  }
  GetAuditType() {
    this.service.GetAuditType().subscribe(
      response => {
        if (response && response.result == 1) {
          this._AuditType = response.auditTypes;
        }
      }
    );
  }

  GetAuditWorkProcess() {
    this.service.GetAuditWorkProcess().subscribe(
      response => {
        if (response && response.result == 1) {
          this._AuditWorkProcess = response.auditWorkProcessList;
        }
      }
    );
  }
  GetAuditBookingContact(factid) {
    this.service.GetAuditBookingContact(factid).subscribe(
      res => {
        if (res && res.result == 30) {
          this._officedetails.officeAddress = res.contactDetails.officeAddress;
          this._officedetails.officeFax = res.contactDetails.officeFax;
          this._officedetails.officeName = res.contactDetails.officeName;
          this._officedetails.officeTelNo = res.contactDetails.officeTelNo;
          this._officedetails.planningEmailTo = res.contactDetails.planningTeamEmailTo,
            this._officedetails.planningEmailCC = res.contactDetails.planningTeamEMailCC,
            this._contactinformation = res.contactDetails.contactInformation
        }
      }
    );
  }
  GetAuditBookingRuleDetails(cusid, factid) {
    this.service.GetAuditBookingRules(cusid, factid).subscribe(
      res => {
        if (res && res.result == 1) {
          this._HolidayDates = res.ruleDetails.holidays;
          this._leadtime = res.ruleDetails.leadDays;
          this._BookingRuleDesc = res.ruleDetails.ruleDescription;
          this.GetLeadDays();
        }
        else {
          this.showError('EDIT_AUDIT.TITLE', 'EDIT_AUDIT.MSG_CANNOT_GET_BOOKINGRULE');
        }
      }
    );
  }
  GetAuditCSDetails(factid, cusid) {
    this.service.GetAuditCSDetails(factid, cusid).subscribe(
      res => {
        if (res && res.result == 1) {
          this._AuditCS = res.auditCS
        }
      }
    );
  }
  GetFileExtensionIcon(file) {
    var ext = file.fileName.toUpperCase().split('.').pop();
    if (ext == "XLX" || ext == "XLSX")
      return "assets/images/uploaded-files.svg";
    else if (ext == "DOC" || ext == "DOCX")
      return "assets/images/uploaded-files-1.svg"
    else
      return "assets/images/uploaded-files-2.svg";
  }

  applicantDetails() {

    if (this.IsInternalUser) {
      this.getInternalLoginUserDetails();
    }
    else {
      this.getUserApplicantDetails();
    }
  }

  //get staff details
  getInternalLoginUserDetails() {
    this.bookingService.GetInspectionStaffDetails(this.currentUser.staffId)
      .pipe()
      .subscribe(
        data => {

          if (data) {
            this.model.applicantname = data.staffName;
            this.model.applicantphno = data.companyPhone;
            this.model.applicantemail = data.companyEmail;
          }

        },
        error => {
          this.setError(error);

        });
  }

  //get user application details
  getUserApplicantDetails() {
    this.bookingService.GetUserApplicantDetails()
      .pipe()
      .subscribe(
        data => {

          if (data) {
            this.model.applicantname = data.applicantName;
            this.model.applicantphno = data.applicantPhoneNo;
            this.model.applicantemail = data.applicantEmail;
          }

        },
        error => {
          this.setError(error);

        });
  }

  numericValidation(event) {
    var maxLength = 5;
    this.utility.numericValidation(event, maxLength);
  }

  changeServiceType() {
    if (this.model.customerid > 0 && this.model.servicetypeid > 0)
      this.getProductCategory();
    else
      this._ProductCategory = [];

    this.model.auditProductCategoryId = null;
  }

  getProductCategory() {
    this.service.getProductCategory(this.model.customerid, this.model.servicetypeid)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this._ProductCategory = res.data;
          }
          else
            this._ProductCategory = [];
        },
        error => {
          this._ProductCategory = [];
          this.setError(error);
        }
      );
  }

  getAuditServiceTypeList() {
    this.refService.getAuditServiceTypeList()
      .pipe(takeUntil(this.componentDestroyed$), first())
      .subscribe(
        res => {
          if (res && res.result == ResponseResult.Success)
            this.auditMaster.customerServiceList = res.serviceTypeList;
          else if (res && res.result == ResponseResult.NoDataFound)
            this.showWarning("EDIT_AUDIT.TITLE", "EDIT_AUDIT.LBL_SERVICE_TYPE_NOT_FOUND");
        },
        error => {
          this.setError(error);
        }
      );
  }
}

