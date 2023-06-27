import { Component, ViewChild, ElementRef } from '@angular/core';
import { BookingStatus, EmployeeType, bookingStatusList, StaffType, Url, EntityAccess, ListSize, RoleEnum } from '../../common/static-data-common'
import { scheduleAllocationModel, allocationStaff, SaveScheduleModel, saveAllocationStaff, SaveScheduleResponseResult, QcAutoExpenseMaster, QcAutoExpense, ScheduleAllocationMasterModel, StaffSearchDataSource, StaffAllocationType } from '../../../_Models/Schedule/scheduleallocationmodel'
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { Validator, JsonHelper } from "../../common";
import { Router, ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../../_Services/Schedule/schedule.service';
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { DetailComponent } from '../../common/detail.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { CommonOfficeZoneSourceRequest, CommonZoneSourceRequest, ResponseResult, StartPortDataSourceRequest, TownDataSourceRequest } from 'src/app/_Models/common/common.model';
import { LocationService } from 'src/app/_Services/location/location.service';
import { element } from 'protractor';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { FinanceDashboardService } from 'src/app/_Services/statistics/finance-dashboard.service';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { TravelTariffService } from 'src/app/_Services/traveltariff/traveltariff.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
@Component({
  selector: 'app-schedule-allocation',
  templateUrl: './schedule-allocation.component.html',
  styleUrls: ['./schedule-allocation.component.scss']
})
export class ScheduleAllocationComponent extends DetailComponent {

  getViewPath(): string {
    return "schedule/schedule-summary"
  }
  getEditPath(): string {
    return "schedule/schedule-allocation"
  }
  public model: scheduleAllocationModel;
  public masterModel: ScheduleAllocationMasterModel;
  public saveModel: SaveScheduleModel;
  public qcAutoExpense: QcAutoExpenseMaster = new QcAutoExpenseMaster();
  public staffAllocationValidator: Array<any> = [];
  private _translate: TranslateService;
  private _toastr: ToastrService;
  private jsonHelper: JsonHelper;
  _BookingStatus = BookingStatus;
  _bookingStatusList = bookingStatusList;
  _EmployeeType = EmployeeType.Permanent;
  public ismodalOpen: boolean;
  public modelRef: NgbModalRef;
  public showAvailableManDay: boolean;
  public fbErrorMessage: any;
  public currentList: any
  public allocatedQC: Array<any>;
  public allocatedAddQC: Array<any>;
  public allocatedCS: Array<any>;
  public searchText: string;
  // public _qcList: Array<any>;
  public _addQCList: Array<any>;
  public _csList: Array<any>;
  public loading: boolean;
  public duplicateTravelAllowance: Array<any>;
  modalRef: any;
  photourl: string;
  popUpLoading: boolean;
  selectAllChecked: boolean;
  showSaveButton: boolean = false;
  showSelectAll: boolean = true;
  qcNotAvailable: any = null;
  csNotAvailable: any = null;
  staffUnavailable: Array<allocationStaff>;
  showRepeatButton: boolean = false;

  townList: any;
  townInput: BehaviorSubject<string>;
  townLoading: boolean;
  townRequest: TownDataSourceRequest;
  componentDestroyed$: Subject<boolean> = new Subject();

  startPortList: any;
  startPortInput: BehaviorSubject<string>;
  startPortLoading: boolean;
  startPortRequest: StartPortDataSourceRequest;

  tripTypeList: any;
  tripTypeLoading: boolean;

  @ViewChild('fbStatusPopup') fbStatusWindow: ElementRef;

  @ViewChild('schedulingStatus') schedulingStatus: ElementRef;

  @ViewChild('staffNotAvailablePopUp') staffNotAvailableWindow: ElementRef;

  @ViewChild('TravelExpenseModal') travelExpenseModal: ElementRef;

  toggleQCFilterSection: boolean;
  showMultiSelectDropDown: boolean = false;
  currentUser: UserModel;
  public _roleEnum = RoleEnum;
  
  constructor(private service: ScheduleService, router: Router,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    public utility: UtilityService,
    public locationService: LocationService,
    private productManagementService: ProductManagementService,
    public refService: ReferenceService,
    public hrService: HrService,
    private customerService: CustomerService,
    public travelTariffService: TravelTariffService,
    public financeDashboardService: FinanceDashboardService,
    toastr: ToastrService, public modalService: NgbModal,
    public pathroute: ActivatedRoute,
    private notification: ToastrService, authservice: AuthenticationService) {
    super(router, route, translate, notification);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this._translate = translate;
    this._toastr = toastr;
    this.townInput = new BehaviorSubject<string>("");
    this.townRequest = new TownDataSourceRequest();
    this.startPortInput = new BehaviorSubject<string>("");
    this.startPortRequest = new StartPortDataSourceRequest();

    this.masterModel = new ScheduleAllocationMasterModel();
    this.currentUser = authservice.getCurrentUser();
  }

  onInit(): void {
    var id = this.pathroute.snapshot.params.id;
    this.loading = true;
    this.validator.setJSON("schedule/schedule-allocation.valid.json");
    this.Initialize(id);
  }

  Initialize(id) {
    this.model = new scheduleAllocationModel();
    this.saveModel = new SaveScheduleModel();
    this.staffAllocationValidator = [];
    this.allocatedQC = new Array();
    this.allocatedAddQC = new Array();
    this.allocatedCS = new Array();
    // this._qcList = new Array();
    this.photourl = './assets/images/user-default.svg';
    this.getStartPortListBySearch();
    this.getTownListBySearch();
    this.getTripTypeList();
    if (id > 0)
      this.getBookingDetails(id);
  }

  reloadScheduleSummary() {

    if (this.modelRef) {
      this.modelRef.close();
    }

    this.return("schedule/schedule-summary");
  }

  //get entity list
  getTripTypeList() {
    this.tripTypeLoading = true;
    this.refService.getTripTypeList()
      .pipe(first())
      .subscribe(
        response => {
          this.tripTypeLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.tripTypeList = response.dataSourceList;
          }
        },
        error => {
          this.tripTypeLoading = false;
          this.setError(error);
        });
  }

  getBookingDetails(id) {
    this.loading = true;
    this.service.getBookingDetailsbyId(id)
      .pipe()
      .subscribe(
        result => {
          if (result) {
            if (id != null) {
              if (result.result == 3) {
                this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_QC_LIST_NOT_AVAILABLE");
                this.loading = false;
              }
              this.model = {
                productCategory: result.productCategory,
                productSubCategory: result.productSubCategory,
                productSubCategory2: result.productSubCategory2,
                bookingNo: result.bookingNo,
                countryId: result.countryId,
                serviceDateFrom: result.serviceDateFrom.slice(0, 10),
                serviceDateTo: result.serviceDateTo.slice(0, 10),
                customerName: result.customerName,
                customerId: result.customerId,
                supplierName: result.supplierName,
                factoryName: result.factoryName,
                quotationManDay: result.quotationManDay ? result.quotationManDay : 0,
                bookingStatus: result.bookingStatus,
                bookingComments: result.bookingComments == "" ? " " : result.bookingComments,
                comment: result.comment,
                countryName: result.countryName + " /" + result.provinceName + " /" + result.cityName + " /" + result.countyName + " /" + result.townName,
                provinceName: result.provinceName,
                cityName: result.cityName,
                countyName: result.countyName,
                townName: result.townName,
                townId: result.townId,
                serviceType: result.serviceType,
                factoryAddress: result.regionalAddress != null ? result.factoryAddress + " (" + result.regionalAddress + ")" : result.factoryAddress,
                regionalAddress: result.regionalAddress,
                totalProduct: result.totalProducts,
                totalReports: result.totalReports,
                totalSampleSize: result.totalSamplingSize,
                totalCombineCount: result.totalCombineCount,
                totalContainers: result.totalContainers,
                travelManday: result.travelManday,
                previousBookingNo: result.previousBookingNoList,
                actualManday: result.actualManday,
                isEntityLevelAutoQcExpenseEnabled: result.isEntityLevelAutoQcExpenseEnabled,
                isServiceTypeLevelAutoQcExpenseEnabled: result.isServiceTypeLevelAutoQcExpenseEnabled,
                isBookingInvoiced: result.isBookingInvoiced,
                suggestedManday: result.suggestedManday,
                staffAllocation: result.allocationCSQCStaff.map((x) => {

                  var tabItem: allocationStaff = {
                    serviceDate: x.serviceDate,
                    QC: x.qc == null ? [] : x.qc,
                    additionalQC: x.addtionalQC == null ? [] : x.addtionalQC,
                    CS: x.cs == null ? [] : x.cs,
                    actualManDay: x.actualManDay,
                    QCList: x.qcList == null ? [] : x.qcList,
                    additionalQCList: x.addtionalQCList == null ? [] : x.additionalQCList,
                    CSList: x.csList == null ? [] : x.csList,
                    availableManDay: x.availableManDay,
                    manDay: x.manDay,
                    remarks: x.remarks,
                    isLeader: x.isLeader,
                    qcAutoExpenses: x.qcAutoExpenseList.map((y) => {

                      var qcExpense: QcAutoExpense = {
                        countryId: result.countryId,
                        countryName: result.countryName,
                        inspectionId: result.bookingNo,
                        qcId: y.qcId,
                        qcName: y.qcName,
                        startPortId: y.startPortId,
                        startPortName: y.startPortName,
                        factoryTownId: y.factoryTownId,
                        factoryTownName: y.factoryTownName,
                        tripTypeId: y.tripTypeId,
                        tripTypeName: y.tripTypeName,
                        comments: y.comments,
                        expenseStatus:y.expenseStatus
                      };

                      return qcExpense;

                    }),
                    leaderId: x.qc == null ? null : x.qc.find(x => x.isLeader) != undefined ? x.qc.find(x => x.isLeader).staffID : null,
                    isQcVisibility: x.isQcVisibility
                  };
                  this.staffAllocationValidator.push({ tabItem: tabItem, validator: Validator.getValidator(tabItem, "schedule/schedule-allocation.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

                  //prevent the additional QC list from having the selected values in QC
                  tabItem.QC.forEach(item => {
                    if (tabItem.additionalQCList) {
                      var i = tabItem.additionalQCList.find(x => x.staffID == item);
                      let index = tabItem.additionalQCList.indexOf(i);
                      tabItem.additionalQCList.splice(index, 1);
                    }
                  });

                  //prevent the QC list from having the selected values in additional QC
                  tabItem.additionalQC.forEach(item => {
                    if (tabItem.QCList) {
                      var i = tabItem.QCList.find(x => x.staffID == item);
                      let index = tabItem.QCList.indexOf(i);
                      tabItem.QCList.splice(index, 1);
                    }
                  });
                  return tabItem;
                }),
              }
            }
            this.model.bookingStatus == this._BookingStatus.Confirmed || this.model.bookingStatus == this._BookingStatus.AllocateQC ? this.showAvailableManDay = true : this.showAvailableManDay = false;

            this.isScheduleSaveButtonShow();
          }
          this.loading = false;
        });
  }

  //schedule role user and before inspected status - show save button, 
  //hide the save - after status(inspected, validated, report sent)
  isScheduleSaveButtonShow() {
    var _bookingnotInInspectedStatus = this._bookingStatusList && this._bookingStatusList.findIndex(x => x.id == this.model.bookingStatus) == -1;

    var _isLoginUserHasScheduleRole = this.currentUser && this.currentUser.roles &&
      this.currentUser.roles.filter(x => x.id == this._roleEnum.InspectionScheduled).length > 0;

    if (_isLoginUserHasScheduleRole && _bookingnotInInspectedStatus) {
      this.showSaveButton = true;
    }
  }

  async save() {
    this.loading = true;
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.setSaveModel();
    if (this.ismodalOpen) this.modelRef.close();
    if (await this.isFormValid()) {
      this.service.saveStaffList(this.saveModel)
        .subscribe(
          res => {
            if (res.result == SaveScheduleResponseResult.Success) {
              this.showSuccess("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_SAVE_SUCCESS");
              this.validator.isSubmitted = false;
              this.model.bookingStatus = res.bookingStatus;
              if (this.fromSummary)
                this.return("schedule/schedule-summary");
              else
                this.Initialize(this.model.bookingNo);
            }
            else if (res.result == SaveScheduleResponseResult.SaveFBDataFailure) {
              this.fbErrorMessage = res.statusMessageList;
              this.modelRef = this.modalService.open(this.fbStatusWindow, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
            }
            else if (res.result == SaveScheduleResponseResult.BookingProcessAlready) {
              this.modelRef = this.modalService.open(this.schedulingStatus, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
            }
            else if (res.result == SaveScheduleResponseResult.ReportProcessedAlready) {
              this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_REPORT_PROCESSED");
            }
            else {
              this.showError("SCHEDULE_SUMMARY.TITLE", `SCHEDULE_SUMMARY.MSG_SAVE_FAIL`);
            }
            this.loading = false;
          },
          error => {
            this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_UNKNOWN_ERROR");
            this.loading = false;
          });
    }
  }

  viewScheduleBooking(bookingId) {
    if (bookingId) {
      // each row data
      let entity: string = this.utility.getEntityName();
      var editPage = entity + "/" + Url.EditSchedule + bookingId;
      window.open(editPage);
    }
  }

  setSaveModel() {
    this.saveModel.bookingId = this.model.bookingNo;
    this.saveModel.comment = this.model.comment;
    var data = new Array();
    this.model.staffAllocation.forEach((x) => {
      var dataItem = new saveAllocationStaff();
      dataItem.serviceDate = x.serviceDate;
      dataItem.actualManDay = x.actualManDay;
      dataItem.QC = x.QC;
      dataItem.additionalQC = x.additionalQC;
      dataItem.CS = x.CS;
      dataItem.isQcVisibility = x.isQcVisibility
      dataItem.qcAutoExpenses = x.qcAutoExpenses
      //dataItem.isLeader = x.isLeader;
      data.push(dataItem);
    });
    this.saveModel.allocationCSQCStaff = data;
  }

  async isFormValid() {
    var isok = true;

    if (this.model.bookingStatus != this._BookingStatus.AllocateQC) {
      //atleast one qc should select any one service date
      if (this.staffAllocationValidator.filter(item => item.tabItem.QC && item.tabItem.QC.length > 0).length <= 0) {
        isok = false;
        this.loading = false;
        this.showError("SCHEDULE_SUMMARY.TITLE", "SCHEDULE_SUMMARY.MSG_ATLEAST_ONE_QC_REQUIRED");
      }
      this.staffAllocationValidator.forEach(item => {
        if (item.tabItem.QC.length > 0 && (item.tabItem.CS == null || item.tabItem.CS.length == 0)) {
          isok = false; this.loading = false;
          this.validator.isValid('CS');
        }
      });
    }
    else if (this.model.bookingStatus == this._BookingStatus.AllocateQC) {
      this.staffAllocationValidator.forEach(item => {
        if (item.tabItem.QC.length > 0 && (item.tabItem.CS == null || item.tabItem.CS.length == 0)) {
          isok = false; this.loading = false;
          this.validator.isValid('CS');
        }
      });
    }

    if (isok) {
      this.staffAllocationValidator.forEach(qcperDate => {

        for (let index = 0; index < qcperDate.tabItem.qcAutoExpenses.length; index++) {
          const expense = qcperDate.tabItem.qcAutoExpenses[index];

          if (expense.startPortId == undefined || expense.startPortId == null) {
            isok = false;
            this.loading = false;
            this.showError("SCHEDULE_SUMMARY.TITLE", "Startport is missing for " + expense.qcName + " on " + qcperDate.tabItem.serviceDate.slice(0, 10));

            break;

          }

          else if (expense.tripTypeId == undefined || expense.tripTypeId == null) {
            isok = false;
            this.loading = false;
            this.showError("SCHEDULE_SUMMARY.TITLE", "Trip type is missing for " + expense.qcName + " on " + qcperDate.tabItem.serviceDate.slice(0, 10));
            break;
          }

        }
        if (!isok) {
          return isok;
        }

      });
    }

    if (isok) {
      // check duplicate travel allowance exist or not
      this.duplicateTravelAllowance = await this.service.getDuplicateTravelAllowance(this.saveModel);

      if (this.duplicateTravelAllowance && this.duplicateTravelAllowance.length == 0) {
        isok = true;
      }
      else {
        isok = false;
        this.loading = false;
        this.modelRef = this.modalService.open(this.travelExpenseModal, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
      }
    }
    return isok;
  }

  openPopUp(contentPopup) {
    if (this.model.bookingStatus == this._BookingStatus.AllocateQC) {
      //qc not selected - open popup
      if (this.staffAllocationValidator.filter(item => item.tabItem.QC && item.tabItem.QC.length > 0).length <= 0) {
        this.ismodalOpen = true;
        this.modelRef = this.modalService.open(contentPopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
        this.modelRef.result.then((result) => {
        }, (reason) => {
        });
      }
      else this.save();
    }
    else this.save();
  }

  // //update QCList and Additinal QC List when QC is selected
  // QCUpdateAdd(n: any, item) {
  //   //update QC dropdown based on additional QC
  //   let index = item.tabItem.QCList.findIndex(row => row.staffID == n.staffID);
  //   item.tabItem.QCList.splice(index, 1);
  //   item.tabItem.QCList = [...item.tabItem.QCList]; // component to detect the change and update in html

  //   let ind = item.tabItem.additionalQCList.findIndex(row => row.staffID == n.staffID);
  //   item.tabItem.additionalQCList.splice(ind, 1);
  //   item.tabItem.additionalQCList = [...item.tabItem.additionalQCList]; // component to detect the change and update in html
  //   //item.tabItem.QC.push(n);
  // }


  //update CSList when report checker is selected
  // CSUpdateAdd(n: any, item) {
  //   //update QC dropdown based on additional QC
  //   let index = item.tabItem.CSList.findIndex(row => row.staffID == n.staffID);
  //   item.tabItem.CSList.splice(index, 1);
  //   item.tabItem.CSList = [...item.tabItem.CSList]; // component to detect the change and update in html
  // }

  // //update allocated and QCList on removal of selected QC
  // QCUpdateRemove(n: any, item) {
  //   item.tabItem.QCList.push(n);
  //   item.tabItem.QCList = [...item.tabItem.QCList];

  //   item.tabItem.additionalQCList.push(n);
  //   item.tabItem.additionalQCList = [...item.tabItem.QCList];

  //   let index = item.tabItem.QC.findIndex(row => row.staffID == n.staffID);
  //   item.tabItem.leaderId = null;
  //   item.tabItem.QC.splice(index, 1);
  //   item.tabItem.QC = [...item.tabItem.QC]; // component to detect the change and update in html

  //   // remove from 
  //   let rowindex = item.tabItem.qcAutoExpenses.findIndex(row => row.qcId == n.staffID);
  //   if (rowindex != -1) {
  //     item.tabItem.qcAutoExpenses.splice(index, 1);
  //     item.tabItem.qcAutoExpenses = [...item.tabItem.qcAutoExpenses];
  //   }
  // }

  //update allocated and QCList on removal of selected QC
  QCUpdateRemove(qcItem: any, removeQCitem) {
    // item.tabItem.QCList.push(n);
    // item.tabItem.QCList = [...item.tabItem.QCList];

    // item.tabItem.additionalQCList.push(n);
    // item.tabItem.additionalQCList = [...item.tabItem.QCList];

    let index = removeQCitem.tabItem.QC.findIndex(row => row.staffID == qcItem.staffID);
    removeQCitem.tabItem.leaderId = null;
    removeQCitem.tabItem.QC.splice(index, 1);
    removeQCitem.tabItem.QC = [...removeQCitem.tabItem.QC]; // component to detect the change and update in html

    // remove from auto expense list
    this.removeQCAutoExpense(qcItem,removeQCitem);
  }

  //update QCList and Additinal QC List when QC is selected
  additionalQCUpdate(n: any, item) {
    // //update additional QC dropdown
    // let index = item.tabItem.additionalQCList.findIndex(row => row.staffID == n.staffID);
    // item.tabItem.additionalQCList.splice(index, 1);
    // item.tabItem.additionalQCList = [...item.tabItem.additionalQCList]; // component to detect the change and update in html

    // let ind = item.tabItem.QCList.findIndex(row => row.staffID == n.staffID);
    // item.tabItem.QCList.splice(ind, 1);
    // item.tabItem.QCList = [...item.tabItem.additionalQCList]; // component to detect the change and update in html
  }

  //Update Additional QC dropdown if QC is deselected
  additionalQCUpdateRemove(qcItem: any, removeQCitem) {
    // item.tabItem.additionalQCList.push(n.value);
    // item.tabItem.additionalQCList = [...item.tabItem.additionalQCList];

    // item.tabItem.QCList.push(n);
    // item.tabItem.QCList = [...item.tabItem.QCList];

    let index = removeQCitem.tabItem.additionalQC.findIndex(row => row.staffID == qcItem.staffID);
    removeQCitem.tabItem.additionalQC.splice(index, 1);
    removeQCitem.tabItem.additionalQC = [...removeQCitem.tabItem.additionalQC]; 
    //remove from auto expense list
    this.removeQCAutoExpense(qcItem,removeQCitem);
   
  }

  //fetch stafflist on click
  openLG(content, index, type) {
    this.popUpLoading = true;
    this.currentList = this.staffAllocationValidator[index].tabItem;
    this.currentList.QCList = [];
    this.currentList.additionalQCList = [];
    this.currentList.CSList = [];
    this.modalRef = this.modalService.open(content, { windowClass: "md1ModelWidth qc-popup", centered: true, backdrop: "static", });

    this.masterModel.staffSearchDataSource = new StaffSearchDataSource();
    this.masterModel.staffSearchDataSource.entityId = parseInt(this.utility.getEntityId());
    if (type == 'Inspector' && this.masterModel.staffSearchDataSource.entityId == EntityAccess.API)
      this.masterModel.staffSearchDataSource.employeeType = EmployeeType.Permanent;

    this.masterModel.staffInput = new BehaviorSubject<string>("");
    this.getOfficeList();
    if (type == 'Inspector') {
      this.getEmployeeTypes();
      this.getZoneListBySearch();
      this.getStartportList();
      this.getMarketSegment();
      this.getProductCategoryList();
      this.getExpertiseList();
    }
    this.getEntityList();

    this.allocatedQC = [];
    this.allocatedAddQC = [];
    this.allocatedCS = [];
    this.getQCDetails(type);
  }

  //get qc details based on type
  getQCDetails(type) {
    this.masterModel.qcSearchLoading = true;
    this.masterModel.staffSearchDataSource.serviceDate = this.currentList.serviceDate;
    this.masterModel.staffSearchDataSource.bookingId = this.model.bookingNo;
    this.masterModel.staffSearchDataSource.type = type;

    //for qc search
    this.masterModel.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.staffLoading = true),
      switchMap(term => this.service.getStaffList(this.masterModel.staffSearchDataSource, term)
        .pipe(
          catchError(() => of([])), // empty list on error
          tap(() => this.masterModel.staffLoading = false))
      )).subscribe(result => {
        if (result) {
          if (result.qcList != null) {
            //remove selected QC from the QC list
            this.currentList.QCList = result.qcList;
            this.currentList.QC.forEach(element => {
              let index = this.currentList.QCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.QCList.splice(index, 1);
            });

            //remove selected Additional QC from the QC list
            this.currentList.additionalQC.forEach(element => {
              let index = this.currentList.QCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.QCList.splice(index, 1);
            });

            // this._qcList = this.currentList.QCList;
          }

          //remove selected CS and assign the result to CSlist
          if (result.csList != null) {
            this.selectAllChecked = false;
            this.currentList.CSList = result.csList;
            this.currentList.CS.forEach(element => {
              let index = this.currentList.CSList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.CSList.splice(index, 1);
            });
            this._csList = this.currentList.CSList;
            this.currentList.CSList.length == 0 ? this.showSelectAll = false : this.showSelectAll = true;
            this.masterModel.employeeListLength = this.currentList.length;
          }
          //this.currentList.additionalQCList = result.addtionalQCList;
          if (result.addtionalQCList != null) {
            //remove selected additionalQC from the additional QC list
            this.currentList.additionalQCList = result.addtionalQCList;
            this.currentList.additionalQC.forEach(element => {
              let index = this.currentList.additionalQCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.additionalQCList.splice(index, 1);
            });

            //remove selected QC from the additional QC list
            this.currentList.QC.forEach(element => {
              let index = this.currentList.additionalQCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.additionalQCList.splice(index, 1);
            });
            this._addQCList = this.currentList.additionalQCList;
            this.masterModel.employeeListLength = this._addQCList.length;
          }

        }
        this.masterModel.qcSearchLoading = false;
        this.masterModel.staffLoading = false;
      });
  }

  setBlobFile(blob) {
    if (blob != null) {
      var reader = new FileReader();
      const blob1 = new Blob([blob], { type: "image/png" });
      reader.readAsDataURL(blob1); // read file as data url
      reader.onload = (event) => { // called once readAsDataURL is completed
        return (<FileReaderEventTarget>event.target).result;
      }
    }
  }

  assignAutoQCExpense(elementQC, allocationStaffItem) {
        // check auto qc expense option enabled
        if (elementQC.employeeType == EmployeeType.Permanent && this.model.isEntityLevelAutoQcExpenseEnabled &&
          this.model.isServiceTypeLevelAutoQcExpenseEnabled) {
        
            allocationStaffItem[0].tabItem.qcAutoExpenses.push(
            {
              inspectionId: this.model.bookingNo,
              countryId: this.model.countryId,
              qcId: elementQC.staffID,
              qcName: elementQC.staffName,
              startPortId: elementQC.startPortId,
              startPortName: elementQC.startPortName,
              factoryTownId: this.model.townId,
              factoryTownName: this.model.townName,
              tripTypeId: null,
              tripTypeName: "",
              comments: ""
            });
        }
  }

  //assign the selected values to the allocated list (model)
  addMore(staffAllocationType: StaffAllocationType) {
    this.searchText = null;
    var allocationStaffItem = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    if (this.allocatedQC.length > 0) {
      if (this.currentList.serviceDate == this.staffAllocationValidator[0].tabItem.serviceDate)
        if (this.model.serviceDateFrom != this.model.serviceDateTo)
          this.showRepeatButton = true;
           this.allocatedQC.forEach(elementQC => {
               this.assignAutoQCExpense(elementQC, allocationStaffItem);
               allocationStaffItem[0].tabItem.QC.push(elementQC);
               });
               allocationStaffItem[0].tabItem.qcAutoExpenses = [...allocationStaffItem[0].tabItem.qcAutoExpenses];
               allocationStaffItem[0].tabItem.QC = [...allocationStaffItem[0].tabItem.QC]; // component to detect the change and update in html
      this.allocatedQC = [];
    }

    else if (this.allocatedAddQC.length > 0) {
      this.allocatedAddQC.forEach(elementQC => {

        this.assignAutoQCExpense(elementQC, allocationStaffItem);
        allocationStaffItem[0].tabItem.additionalQC.push(elementQC);
      });
    }

    else if (this.allocatedCS.length > 0)
      this.allocatedCS.forEach(element => {
        allocationStaffItem[0].tabItem.CS.push(element);
      });
    this.currentList.CSList.length == 0 ? this.showSelectAll = false : this.showSelectAll = true;
    this.modalRef.close();
  }

  // //Triggers when QC checkbox is checked
  // onChange(data) {
  //   var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
  //   //this.currentList.QC.push(data);

  //   let index = this.allocatedQC.findIndex(x => x.staffID == data.staffID);


  //   if (index == -1) {
  //     this.allocatedQC.push(data);
  //     this.QCUpdateAdd(data, item[0]);
  //     let ind = this._qcList.findIndex(x => x.staffID == data.staffID);
  //     if (ind != -1)
  //       this._qcList.splice(ind, 1);

  //   }
  //   else {
  //     this.allocatedQC.splice(index, 1);
  //     item[0].tabItem.QCList.push(data);
  //     item[0].tabItem.additionalQCList.push(data);
  //     let ind = this._qcList.findIndex(x => x.staffID == data.staffID);
  //     if (ind == -1)
  //       this._qcList.push(data);

  //   }
  // }

  //on change qc dropdown
  // onChangeQC(data) {
  //   //set selected value in allocatedQC
  //   this.allocatedQC = data;
  // }

  // //Triggers when additional QC checkbox is checked
  // onChangeAddQC(data) {
  //   var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
  //   let index = this.allocatedAddQC.findIndex(x => x.staffID == data.staffID);
  //   if (index == -1) {
  //     this.allocatedAddQC.push(data);
  //     this.additionalQCUpdate(data, item[0]);

  //     let ind = this._addQCList.findIndex(x => x.staffID == data.staffID);
  //     if (ind != -1)
  //       this._addQCList.splice(ind, 1);
  //   }
  //   else {
  //     this.allocatedAddQC.splice(index, 1);
  //     item[0].tabItem.QCList.push(data);
  //     item[0].tabItem.additionalQCList.push(data);
  //     let ind = this._addQCList.findIndex(x => x.staffID == data.staffID);
  //     if (ind == -1)
  //       this._addQCList.push(data);
  //   }
  //   //this.AddQCUpdate(data, item[0]);
  // }

  //on change additional qc dropdown
  // onChangeAddQC(data) {
  //   //set selected value additional qc
  //   this.allocatedAddQC = data;
  // }

  //on change report checker
  // onChangeCS(data) {
  //   //set selected value in allocated qc
  //   this.allocatedCS = data;
  // }

  // //Triggers when CS checkbox is checked
  // onChangeCS(data) {
  //   this.selectAllChecked = false;
  //   var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);


  //   let index = this.allocatedCS.findIndex(x => x.staffID == data.staffID);
  //   if (index == -1) {
  //     this.allocatedCS.push(data);
  //     this.CSUpdateAdd(data, item[0]);

  //     let ind = this._csList.findIndex(x => x.staffID == data.staffID);
  //     if (ind != -1)
  //       this._csList.splice(ind, 1);
  //   }
  //   else {
  //     this.allocatedCS.splice(index, 1);
  //     item[0].tabItem.CSList.push(data);
  //     let ind = this._csList.findIndex(x => x.staffID == data.staffID);
  //     if (ind == -1)
  //       this._csList.push(data);
  //   }
  // }

  removeUser(parentIndex, type, data) {
    this.currentList = this.staffAllocationValidator[parentIndex].tabItem;
    var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    this.QCUpdateRemove(data, item[0]);
  }

  removeAddQC(parentIndex, type, data) {
    this.currentList = this.staffAllocationValidator[parentIndex].tabItem;
    var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    this.additionalQCUpdateRemove(data, item[0]);
  }

  removeAddCS(parentIndex, type, data) {
    this.selectAllChecked = false;
    this.showSelectAll = true;
    this.currentList = this.staffAllocationValidator[parentIndex].tabItem;
    var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    let index = item[0].tabItem.CS.findIndex(row => row.staffID == data.staffID);
    item[0].tabItem.CS.splice(index, 1);
  }

  //unused
  SearchUser(type) {
    if (type == 'qc') {
      if (this.searchText.length >= 3)
        this.currentList.QCList = this.currentList.QCList.filter(x => x.staffName.toLowerCase().includes(this.searchText.toLowerCase()));
      // else
      // this.currentList.QCList = this._qcList;
    }
    if (type == 'addqc') {
      if (this.searchText.length >= 3)
        this.currentList.additionalQCList = this.currentList.additionalQCList.filter(x => x.staffName.toLowerCase().includes(this.searchText.toLowerCase()));
      else
        this.currentList.additionalQCList = this._addQCList;
    }
    if (type == 'cs') {
      if (this.searchText.length >= 3) {
        this.showSelectAll = false;
        this.currentList.CSList = this.currentList.CSList.filter(x => x.staffName.toLowerCase().includes(this.searchText.toLowerCase()));
      }
      else {
        this.showSelectAll = true;
        this.currentList.CSList = this._csList;
      }
    }
  }

  //assign the QC Leader
  assignQCLeader(qcList, event) {
    if (qcList && event) {
      // set no qc as leader
      qcList.forEach(element => {
        element.isLeader = false;
      });

      // set selected qc as leader  
      var selectedQc = qcList.find(x => x.staffID == event.staffID);

      if (selectedQc) {
        selectedQc.leaderId = event.staffID;
        selectedQc.isLeader = true;
      }
    }

  }

  //TO-DO: commented code
  Reset(type) {
    var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    if (type == 'qc') {
      //push the selected data to the list if any
      this.allocatedQC.forEach(x => {
        item[0].tabItem.QCList.push(x);
      })
      this.allocatedQC = [];
      //if search text is not null then push the values in the search list
      // if (this.searchText != null) {
      //   this._qcList.forEach(x => {
      //     if (this.currentList.QCList.findIndex(y => y.staffID == x.staffID) == -1)
      //       this.currentList.QCList.push(x);
      //   })

      //   this._qcList = this.currentList.QCList;
      //   this.searchText = null;
      // }
    }

    if (type == 'addqc') {
      //push the selected data to the list if any
      this.allocatedAddQC.forEach(x => {
        item[0].tabItem.additionalQCList.push(x);
      })
      this.allocatedAddQC = [];
      //if search text is not null then push the values in the search list
      // if (this.searchText != null) {
      //   this._addQCList.forEach(x => {
      //     if (this.currentList.additionalQCList.findIndex(y => y.staffID == x.staffID) == -1)
      //       this.currentList.additionalQCList.push(x);
      //   })
      //   this._addQCList = this.currentList.additionalQCList;
      //   this.searchText = null;
      // }
    }

    if (type == 'cs') {
      this.showSelectAll = true;
      //push the selected data to the list if any
      // this.allocatedCS.forEach(x => {
      //   item[0].tabItem.CSList.push(x);
      // })
      this.allocatedCS = [];
      //if search text is not null then push the values in the search list
      // if (this.searchText != null) {
      //   this._csList.forEach(x => {
      //     if (this.currentList.CSList.findIndex(y => y.staffID == x.staffID) == -1)
      //       this.currentList.CSList.push(x);
      //   })
      //   this._csList = this.currentList.CSList;
      //   this.searchText = null;
      // }
    }
  }

  ReportCheckerSelectAll() {
    var item = this.staffAllocationValidator.filter(x => x.tabItem.serviceDate == this.currentList.serviceDate);
    if (this.currentList.CSList.length > 0) {
      this.selectAllChecked = true;
      this.currentList.CSList.forEach(x => {
        this.allocatedCS.push(x);
      });

      this.currentList.CSList = [];
    }

    else {
      this.selectAllChecked = false;
      this.currentList.CSList = this.allocatedCS;
      this.allocatedCS = [];
    }

  }

  repeatData() {
    //counter and total run are to make sure the server API calls are completed before showing the pop up
    let counter = 0;
    var csCounter = this.currentList.CS.length > 0 ? 1 : 0; //if CS is selected 1 API call for CS
    let staffcount = 1 + csCounter; //count the number of API calls - 1 is for QC  - total 2 for each date (QC and CS)
    let totalRun = (this.staffAllocationValidator.length - 1) * staffcount;
    this.staffUnavailable = new Array();
    //loop through all the service dates to copy the selected data except the date for which the staff is allocated
    this.staffAllocationValidator.forEach(element => {
      if (element.tabItem.serviceDate != this.currentList.serviceDate) {
        var dataItem = new allocationStaff();
        if (this.currentList.QC.length > 0 || this.currentList.additionalQC.length > 0) {
          //server API call to fetch the QC staff list
          let staffSearchDataSource = new StaffSearchDataSource();
          staffSearchDataSource.serviceDate = element.tabItem.serviceDate;
          staffSearchDataSource.bookingId = this.model.customerId;
          staffSearchDataSource.type = StaffType[StaffType.Inspector];
          this.service.getStaffList(staffSearchDataSource)
            .pipe()
            .subscribe(
              result => {
                if (result) {
                  this.qcNotAvailable = null;
                  counter++;
                  element.tabItem.QCList = result.qcList;
                  //check if the QC is available in the list for that particular date
                  this.currentList.QC.forEach(item => {
                    var index = result.qcList.findIndex(x => x.staffID == item.staffID);
                    if (index != -1) {
                      var ind = element.tabItem.QC.findIndex(x => x.staffID == item.staffID);
                      if (ind == -1)
                        element.tabItem.QC.push(item);
                    }
                    //if staff is not available in the list, push them to an array to display later
                    else {
                      this.qcNotAvailable = this.qcNotAvailable == null ? item.staffName : this.qcNotAvailable + ", " + item.staffName;
                      dataItem.QCList = this.qcNotAvailable;
                      if (dataItem.serviceDate == null)
                        dataItem.serviceDate = element.tabItem.serviceDate;
                      if (this.staffUnavailable.findIndex(x => x.serviceDate == dataItem.serviceDate) == -1)
                        this.staffUnavailable.push(dataItem);

                      else if (this.staffUnavailable.find(x => x.serviceDate == dataItem.serviceDate))
                        this.staffUnavailable.find(x => x.serviceDate == dataItem.serviceDate).QCList = this.qcNotAvailable;
                    }
                  });

                  this.currentList.additionalQC.forEach(item => {
                    var index = result.qcList.findIndex(x => x.staffID == item.staffID);
                    if (index != -1) {
                      var ind = element.tabItem.additionalQC.findIndex(x => x.staffID == item.staffID);
                      if (ind == -1)
                        element.tabItem.additionalQC.push(item);
                    }
                  });
                  if (counter == totalRun) {
                    if (this.staffUnavailable.length > 0)
                      this.modelRef = this.modalService.open(this.staffNotAvailableWindow, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
                  }
                }
              });
        }
        if (this.currentList.CS.length > 0) {
          let staffSearchDataSource = new StaffSearchDataSource();
          staffSearchDataSource.serviceDate = element.tabItem.serviceDate;
          staffSearchDataSource.bookingId = this.model.customerId;
          staffSearchDataSource.type = StaffType[StaffType.ReportChecker];
          this.service.getStaffList(staffSearchDataSource)
            .pipe()
            .subscribe(
              result => {
                if (result) {
                  this.csNotAvailable = null;
                  counter++;
                  element.tabItem.CSList = result.csList;
                  this.currentList.CS.forEach(item => {
                    var index = result.csList.findIndex(x => x.staffID == item.staffID);
                    if (index != -1) {
                      var ind = element.tabItem.CS.findIndex(x => x.staffID == item.staffID);
                      if (ind == -1)
                        element.tabItem.CS.push(item);
                    }

                    else {
                      this.csNotAvailable = this.csNotAvailable == null ? item.staffName : this.csNotAvailable + ", " + item.staffName;
                      dataItem.CSList = this.csNotAvailable;
                      if (dataItem.serviceDate == null)
                        dataItem.serviceDate = element.tabItem.serviceDate;


                      if (this.staffUnavailable.findIndex(x => x.serviceDate == dataItem.serviceDate) == -1)
                        this.staffUnavailable.push(dataItem);

                      else if (this.staffUnavailable.find(x => x.serviceDate == dataItem.serviceDate))
                        this.staffUnavailable.find(x => x.serviceDate == dataItem.serviceDate).CSList = this.csNotAvailable;
                    }
                    if (counter == totalRun) {
                      if (this.staffUnavailable.length > 0)
                        this.modelRef = this.modalService.open(this.staffNotAvailableWindow, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
                    }
                    //this.notAvailable = this.notAvailable == "" ? element.tabItem.serviceDate.slice(0,10) + " - " + item.staffName + "\n" : this.notAvailable + element.tabItem.serviceDate.slice(0,10) + " - " + item.staffName + "\n";
                  });
                }
              });
        }
        // if(typeof(dataItem.serviceDate) !== 'undefined')          
      }
    });

  }

  RedirectToEdit(bookingId) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.EditBooking + bookingId;
    window.open(editPage);
  }

  getTownListBySearch() {
    this.townInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.townLoading = true),
      switchMap(term => term
        ? this.locationService.getTownDataSourceList(this.townRequest, term)
        : this.locationService.getTownDataSourceList(this.townRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.townLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.townList = data;
        this.townLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getTownData() {
    this.townLoading = true;
    this.townRequest.searchText = this.townInput.getValue();
    this.townRequest.skip = this.townList.length;
    this.locationService.getTownDataSourceList(this.townRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.townList = this.townList.concat(data);
        }
        this.townRequest = new TownDataSourceRequest();
        this.townLoading = false;
      }),
      error => {
        this.townLoading = false;
        this.setError(error);
      };
  }

  townChange(item) {
  }


  clearTown() {
    this.townList = [];
    this.getTownListBySearch();
  }

  getStartPortListBySearch() {
    this.startPortInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.startPortLoading = true),
      switchMap(term => term
        ? this.locationService.getstartPortDataSourceList(this.startPortRequest, term)
        : this.locationService.getstartPortDataSourceList(this.startPortRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.startPortLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.startPortList = data;
        this.startPortLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getStartPortData() {
    this.startPortLoading = true;
    this.startPortRequest.searchText = this.startPortInput.getValue();
    this.startPortRequest.skip = this.startPortList.length;
    this.locationService.getstartPortDataSourceList(this.startPortRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.startPortList = this.startPortList.concat(data);
        }
        this.startPortRequest = new StartPortDataSourceRequest();
        this.startPortLoading = false;
      }),
      error => {
        this.startPortLoading = false;
        this.setError(error);
      };
  }

  startPortChange(item) {
  }

  clearStartPort() {
    this.startPortList = [];
    this.getStartPortListBySearch();
  }


  openEditExpensePopup(contentPopup, dataRow) {
    this.qcAutoExpense = new QcAutoExpenseMaster();
    this.modelRef = this.modalService.open(contentPopup,
      { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {
    }, (reason) => {
      if (reason == "apply") {
        if (this.qcAutoExpense.starPortData) {
          dataRow.startPortId = this.qcAutoExpense.starPortData.id;
          dataRow.startPortName = this.qcAutoExpense.starPortData.name;
        }

        if (this.qcAutoExpense.townData) {
          dataRow.factoryTownId = this.qcAutoExpense.townData.id;
          dataRow.factoryTownName = this.qcAutoExpense.townData.name;
        }

      }

    });
  }

  getEntityList() {
    this.refService.getEntityList().subscribe(res => {
      if (res && res.result == ResponseResult.Success) {
        this.masterModel.entityList = res.dataSourceList;
      } else {
        this.masterModel.entityList = [];
      }
    });
  }

  getEmployeeTypes() {
    this.masterModel.employeeTypeLoading = true;

    this.financeDashboardService.getEmployeeTypes()
      .pipe(first())
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.masterModel.employeeTypeList = res.dataSourceList;
        }
        else {
          this.masterModel.employeeTypeList = [];
        }
        this.masterModel.employeeTypeLoading = false;
      },
        error => {
          this.masterModel.employeeTypeLoading = false;
          this.masterModel.employeeTypeList = [];
        });
  }

  getZoneListBySearch() {

    if (this.masterModel.staffSearchDataSource.officeId && this.masterModel.staffSearchDataSource.officeId > 0) {
      this.masterModel.zoneRequest.officeIds.push(this.masterModel.staffSearchDataSource.officeId);
    }

    this.masterModel.zoneInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.zoneLoading = true),
      switchMap(term => term
        ? this.service.getOfficeZoneDataSourceList(this.masterModel.zoneRequest, term)
        : this.service.getOfficeZoneDataSourceList(this.masterModel.zoneRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.zoneLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.zoneList = data;
        this.masterModel.zoneLoading = false;
      });
  }

  getZoneData(isDefaultLoad: boolean) {

    if (isDefaultLoad) {
      this.masterModel.zoneRequest.searchText = this.masterModel.zoneInput.getValue();
      this.masterModel.zoneRequest.skip = this.masterModel.zoneList.length;
    }

    this.masterModel.zoneLoading = true;
    this.service.getOfficeZoneDataSourceList(this.masterModel.zoneRequest).
      subscribe(zoneData => {
        if (zoneData && zoneData.length > 0) {
          this.masterModel.zoneList = this.masterModel.zoneList.concat(zoneData);
        }
        if (isDefaultLoad)
          this.masterModel.zoneRequest = new CommonOfficeZoneSourceRequest();
        this.masterModel.zoneLoading = false;
      }),
      error => {
        this.masterModel.zoneLoading = false;
        this.setError(error);
      };
  }

  //show the advanced filter
  toggleQCFilter() {
    this.toggleQCFilterSection = !this.toggleQCFilterSection;
  }

  onChangeEmployeeType() {
    if (this.masterModel.staffSearchDataSource.employeeType === EmployeeType.Outsource) {
      this.getOutSourceCompanyList();
    }
  }

  getOutSourceCompanyList() {
    this.masterModel.outSourceCompanyLoading = true;
    this.hrService.getHROutSourceCompanyDataList().subscribe(res => {
      if (res.result === ResponseResult.Success) {
        this.masterModel.outSourceCompanyList = res.hrOutSourceCompanyList;
      }
      this.masterModel.outSourceCompanyLoading = false;
    })
  }

  async getStartportList() {
    this.masterModel.startPortLoading = true;
    var response = await this.travelTariffService.getStartPortList();
    if (response && response.result === ResponseResult.Success) {
      this.masterModel.startPortList = response.dataSourceList;
    }
    this.masterModel.startPortLoading = false;

  }

  getMarketSegment() {
    this.masterModel.marketSegmentLoading = true;
    this.customerService.getMarketSegment()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result === ResponseResult.Success) {
            this.masterModel.marketSegmentList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.masterModel.marketSegmentLoading = false;

        },
        error => {
          this.setError(error);
          this.masterModel.marketSegmentLoading = false;
        });
  }

  getProductCategoryList() {
    this.masterModel.productCategoryLoading = true;
    this.productManagementService.getProductCategoryList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result = ResponseResult.Success) {
          this.masterModel.productCategoryList = res.dataSourceList;
        }
        this.masterModel.productCategoryLoading = false;
      }),
      error => {
        this.masterModel.productCategoryList = [];
        this.masterModel.productCategoryLoading = false;
      }
  }

  getExpertiseList() {
    this.masterModel.expertiseLoading = true;
    this.refService.getExpertiseList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result = ResponseResult.Success) {
          this.masterModel.expertiseList = res.dataSourceList;
        }
        this.masterModel.expertiseLoading = false;
      }),
      error => {
        this.masterModel.expertiseList = [];
        this.masterModel.expertiseLoading = false;
      }
  }

  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.refService.getOfficeList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result = ResponseResult.Success) {
          this.masterModel.officeList = res.dataSourceList;
        }
        this.masterModel.officeLoading = false;
      }),
      error => {
        this.masterModel.officeList = [];
        this.masterModel.officeLoading = false;
      }
  }


  toggleQCDropDown(isShowQCDropDown) {
    this.masterModel.isShowQCDropdown = isShowQCDropDown;
  }

  toggleAdditionalQCDropDown(isShowAddQCDropDown) {
    this.masterModel.isShowAddQCDropdown = isShowAddQCDropDown;
  }
  toggleReportCheckerDropDown(isShowRepotCheckedDrop) {
    this.masterModel.isShowReportCheckerDropdown = isShowRepotCheckedDrop;
  }

  //virtual scroll method
  getStaffData(staffAllocationType) {
    this.popUpLoading = true;
    this.masterModel.qcSearchLoading = true;

    this.masterModel.staffSearchDataSource.searchText = this.masterModel.staffInput.getValue();
    this.masterModel.staffSearchDataSource.skip = this.currentList.QCList.length;
    //based on the staff allocation type get the array length
    if (staffAllocationType === StaffAllocationType.QC) {
      this.masterModel.staffSearchDataSource.skip = this.currentList.QCList.length;
    }
    else if (staffAllocationType === StaffAllocationType.AdditionalQC) {
      this.masterModel.staffSearchDataSource.skip = this.currentList.additionalQCList.length;
    }



    //get staff list  by pagignation
    this.service.getStaffList(this.masterModel.staffSearchDataSource).
      subscribe(result => {
        if (result) {
          this.masterModel.staffSearchDataSource.skip = 0;
          this.masterModel.staffSearchDataSource.take = ListSize;
          if (result.qcList != null) {
            //remove selected QC from the QC list
            this.currentList.QCList = this.currentList.QCList.concat(result.qcList);
            this.currentList.QC.forEach(element => {
              let index = this.currentList.QCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.QCList.splice(index, 1);
            });

            //remove selected Additional QC from the QC list
            this.currentList.additionalQC.forEach(element => {
              let index = this.currentList.QCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.QCList.splice(index, 1);
            });

            // this._qcList = this.currentList.QCList;
          }

          //remove selected CS and assign the result to CSlist
          if (result.csList != null) {
            this.selectAllChecked = false;
            this.currentList.CSList = this.currentList.QCList.concat(result.csList);
            this.currentList.CS.forEach(element => {
              let index = this.currentList.CSList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.CSList.splice(index, 1);
            });
            this._csList = this.currentList.CSList;
            this.currentList.CSList.length == 0 ? this.showSelectAll = false : this.showSelectAll = true;
          }
          //this.currentList.additionalQCList = result.addtionalQCList;
          if (result.addtionalQCList != null) {
            //remove selected additionalQC from the additional QC list
            this.currentList.additionalQCList = this.currentList.QCList.concat(result.addtionalQCList);
            this.currentList.additionalQC.forEach(element => {
              let index = this.currentList.additionalQCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.additionalQCList.splice(index, 1);
            });

            //remove selected QC from the additional QC list
            this.currentList.QC.forEach(element => {
              let index = this.currentList.additionalQCList.findIndex(x => x.staffID == element.staffID);
              if (index != -1)
                this.currentList.additionalQCList.splice(index, 1);
            });
            this._addQCList = this.currentList.additionalQCList;
          }
        }
        this.popUpLoading = false;
        this.masterModel.qcSearchLoading = false;
      }),
      (error) => {
        this.popUpLoading = false;
        this.setError(error);
      };
  }
  //on change office get the zone
  onChangeOffice($event) {
    this.masterModel.zoneRequest.officeIds = [];
    this.getZoneListBySearch();
  }

  onChangeOfficeCS() {
    this.getQCDetails('ReportChecker');
  }

  //filter the qc data
  filterQC(type) {
    if (this.masterModel) {
      this.masterModel.staffSearchDataSource.skip = 0;
      this.masterModel.staffSearchDataSource.take = 10;
    }

    this.getQCDetails(type);
  }
  //clear the staff search
  clearStaff(type) {
    this.masterModel.staffSearchDataSource.searchText = null;
    this.masterModel.staffInput = new BehaviorSubject<string>("");
    this.getQCDetails(type);
  }

  //remove selected qc
  removeQC(item) {
    const deletedIndex = this.allocatedQC.findIndex(x => x.id == item.id);
    if (deletedIndex != -1) {
      this.allocatedQC.splice(deletedIndex, 1);
      this.allocatedQC = [...this.allocatedQC];
    }
  }

  //remove selected additional qc
  unselectAddQC(item) {
    const deletedIndex = this.allocatedAddQC.findIndex(x => x.id == item.id);
    if (deletedIndex != -1) {
      this.allocatedAddQC.splice(deletedIndex, 1);
      this.allocatedAddQC = [...this.allocatedAddQC];
    }
  }

  //remove selected report checker
  unselectCS(item) {
    const deletedIndex = this.allocatedCS.findIndex(x => x.id == item.id);
    if (deletedIndex != -1) {
      this.allocatedCS.splice(deletedIndex, 1);
      this.allocatedCS = [...this.allocatedCS];
    }
  }
  //reset the qc filter
  resetQCFilter(type) {
    this.masterModel.staffSearchDataSource = new StaffSearchDataSource();
    this.masterModel.staffSearchDataSource.entityId = parseInt(this.utility.getEntityId());
    this.allocatedQC = [];
    this.allocatedCS = [];
    this.allocatedAddQC = [];
    this.getZoneListBySearch();
    this.getQCDetails(type);
  }

  //select all cs
  public onSelectAllCS() {
    this.allocatedCS = [...this.currentList.CSList];
  }
  //unselect all cs
  public onClearAllCS() {
    this.allocatedCS = [];
  }

  removeQCAutoExpense(qcItem: any, removeQCitem) {
    // remove from 
    let rowindex = removeQCitem.tabItem.qcAutoExpenses.findIndex(row => row.qcId == qcItem.staffID);
    if (rowindex != -1) {
      removeQCitem.tabItem.qcAutoExpenses.splice(rowindex, 1);
      removeQCitem.tabItem.qcAutoExpenses = [...removeQCitem.tabItem.qcAutoExpenses];
    }
  }
}

interface FileReaderEventTarget extends EventTarget {
  result: string
}

