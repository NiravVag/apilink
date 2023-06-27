import { Component, NgModule } from '@angular/core';
import { first, takeUntil } from 'rxjs/operators';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { ExpenseClaimListModel, ExpenseClaimItemModel, ExpenseClaimListResult, ExpenseMasterData, ExpenseClaimUpdateStatus, PendingBookingExpenseRequest, PendingBookingExpenseResponseResult } from '../../../_Models/expense/expenseclaimlist.model';
import { ExpenseClaimStatusEnum, ExpenseClaimType } from '../../../_Models/expense/expenseclaim.model';
import { ExpenseService } from '../../../_Services/expense/expense.service';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgbModalRef, NgbModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { HeaderService } from '../../../_Services/header/header.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UserModel } from '../../../_Models/user/user.model';
import { EmployeeType, expenseClaimStatus, expenseEmployeeTypeList, ExpenseSummaryExportType, PageSizeCommon, RoleEnum } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { HROutSourceCompanyRequest } from 'src/app/_Models/hr/edit-staff.model';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { Subject } from 'rxjs';
import { ThisReceiver } from '@angular/compiler';
import { DataSourceResult } from '../../../_Models/kpi/datasource.model';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { array } from '@amcharts/amcharts4/core';

@Component({
  selector: 'app-expenseClaimList',
  templateUrl: './expense-claim-list.component.html',
  styleUrls: ['./expense-claim-list.component.css'],
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

export class ExpenseClaimListComponent extends SummaryComponent<ExpenseClaimListModel> {

  public data: any;
  loading = true;
  public expenseClaimTypeList: Array<ExpenseClaimType> = [];
  public exportDataLoading = false;
  error = '';
  public model: ExpenseClaimListModel;
  public isSupplierDetails: boolean = false;
  idCurrentSupplier: number;
  nameList: Array<any> = [];
  rejectComment: string;
  isRejectValid: boolean = true;
  rejectId: number;
  public nextStatusLabel: string;
  public nextStatusId: number = 0;
  public currentItemId: number = 0;
  public isApproveSummary: boolean = false;
  private modelRef: NgbModalRef;
  private _router: Router;
  public _searchloader: boolean = false;
  public _initialloader: boolean = false;
  public _statuslist: any;
  public isFilterOpen: boolean;
  public selectAllChecked: boolean = false;
  public showVoucherExport: boolean = false;
  public showExpenseKpi: boolean = false;
  public exportVoucherKpiLoading = false;
  public exportExpenseSummaryKpiLoading = false;
  public user: UserModel;
  public _RoleEnum = RoleEnum;
  public hasAccountingRole: boolean = false;


  employeeTypeList: any;

  hrOutSourceCompanyLoading = false;
  componentDestroyed$: Subject<boolean> = new Subject();
  hrOutSourceCompanyList: any;

  internalStaffList: any;
  isOutSourceAccountingRole = false;
  isOutSourceVisible = false;
  employeeTypeEnum = EmployeeType;
  expenseMasterData: ExpenseMasterData;
  pagesizeitems = PageSizeCommon;
  selectedPageSize: number;
  expenseIdList: Array<number> = [];
  selectedExpenseIdList: Array<number> = [];
  constructor(private service: ExpenseService, private modalService: NgbModal, public utility: UtilityService, public hrService: HrService,
    router: Router, route: ActivatedRoute, public validator: Validator,
    private headerService: HeaderService, private calendar: NgbCalendar, translate: TranslateService,
    toastr: ToastrService) {
    super(router, validator, route, translate, toastr);

    this._router = router;
    this.validator.setJSON("expense/expenseclaim-list.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.isFilterOpen = true;

    if (localStorage.getItem('currentUser'))
      this.user = JSON.parse(localStorage.getItem('currentUser'));

    this.model = new ExpenseClaimListModel();
    this.expenseMasterData = new ExpenseMasterData();
    this.selectedPageSize = PageSizeCommon[0];
    this.model.pageSize = this.selectedPageSize;
    this.getEmployeeTypeList();
    this.getHRPayrollCompanyList();

    this.model.companyIds = [];

    this.toggleOutSourceData();

    this.setBaseDetailBasedOnRole();
    this.model.exportType = ExpenseSummaryExportType.ExpenseSummaryKPI;

  }

  //take only the internal user and outsource user type
  getEmployeeTypeList() {
    this.employeeTypeList = expenseEmployeeTypeList;
  }

  toggleOutSourceData() {

    if (this.user.roles.filter(x => (x.id == this._RoleEnum.Accounting || x.id == this._RoleEnum.Management
      || x.id == this._RoleEnum.ExpenseRoleAccess || x.id == this._RoleEnum.OutSourceAccountingRole)).length > 0)
      this.isOutSourceVisible = true;
  }

  setBaseDetailBasedOnRole() {
    if (this.user.roles.filter(x => (x.id == this._RoleEnum.Accounting || x.id == this._RoleEnum.Management
      || x.id == this._RoleEnum.ExpenseRoleAccess)).length > 0)
      this.model.employeeTypeId = this.employeeTypeEnum.Permanent;
    else if (this.user.roles.filter(x => x.id == this._RoleEnum.OutSourceAccountingRole).length > 0) {
      this.isOutSourceAccountingRole = true;
      this.model.employeeTypeId = this.employeeTypeEnum.Outsource;
    }
  }

  onInit() {

    if (localStorage.getItem('currentUser')) {
      this.user = JSON.parse(localStorage.getItem('currentUser'));
      var index = this.user.roles.findIndex(x => x.id == this._RoleEnum.Accounting);
      if (index != -1) {
        this.hasAccountingRole = true;
      }

      var isExpenseRoleUserIndex = this.user.roles.findIndex(x => x.id == this._RoleEnum.ExpenseRoleAccess);

      if (isExpenseRoleUserIndex >= 0) {
        this.expenseMasterData.hasExpenseRole = true;
      }
    }

    this.isApproveSummary = this._router.url.indexOf("expenseclaim-approve") >= 0;
    this._initialloader = true;
    this.getClaimTypeList();
    this.data = this.service.getExpenseSummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            //take the internal staff list as backup
            this.internalStaffList = data.employeeList;

            //assign the hr outsource company list
            if (!this.hrOutSourceCompanyList)
              this.hrOutSourceCompanyList = data.hrOutSourceCompanyList;
            if (this.hrOutSourceCompanyList && this.hrOutSourceCompanyList.length > 0) {
              this.model.companyIds.push(this.hrOutSourceCompanyList[0].id);
              this.model.companyIds = [...this.model.companyIds];
            }


            if (this.isApproveSummary) {
              if (this.model.statusValues == null)
                this.model.statusValues = [];

              this.model.statusValues.push(this.data.statusList.filter(x => x.id == 5)[0]);// manager default show all checked status
              this.model.startDate = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
              this.model.endDate = this.calendar.getNext(this.calendar.getToday(), 'm', 6);
              this.search();
            }
          }
          else if (data) {
            switch (data.result) {
              case 2:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_ERROR_LOCATIONS');
                break;
              case 3:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_ERROR_EMPLOYEES');
                break;
              case 4:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_ERROR_STATUS');
                break;
            }
          }
          else {
            this.error = data.result;
          }

          this._initialloader = false;
          this.loading = false;
        },
        error => {
          this.setError(error);
          this._initialloader = false;
          this.loading = false;
        });
  }

  getClaimTypeList() {
    this.service.getClaimTypeList()
      .pipe()
      .subscribe(
        res => {
          if (res && res.expenseClaimTypeList) {
            this.expenseClaimTypeList = res.expenseClaimTypeList;
          }
          else {
            this.expenseClaimTypeList = [];
          }
        },
        error => {
          this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_CANNOT_GET');
        }
      );
  }

  getPathDetails(): string {
    return "expenseclaim/expense-claim";
  }

  getData() {
    this._searchloader = true;
    this.showExpenseKpi = false;
    this.selectAllChecked = false;
    this.showVoucherExport = false;

    this.service.getClaimList(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {

            if (this.model.employeeTypeId == this.employeeTypeEnum.Outsource && !this.hrOutSourceCompanyList)
              this.getHROutSourceCompanyData();

            this.model.index = response.index;
            this.model.items = response.expenseClaimGroupList;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;
            this.model.canCheck = response.canCheck;
            this.expenseIdList = response.expenseIdList;
            this.mapCheckedItem();

            this._statuslist = this.isApproveSummary && response.expenseStatusList ? response.expenseStatusList.filter(x => x.id == 5) : response.expenseStatusList;

          }
          else if (response && response.result == 7) {
            this.model.noFound = true;
          }
          else if (response) {
            let result: ExpenseClaimListResult = response.result;

            switch (result) {
              case ExpenseClaimListResult.StartDateRequired:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_STARTDATE_REQ');
                break;
              case ExpenseClaimListResult.EndDateRequired:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_ENDDATE_REQ');
                break;
              case ExpenseClaimListResult.NoAffectedLocations:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_ERROR_NOLOCATIONS');
                break;
              default:
                this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'ERROR.MSG_TITLE_GEN');
            }
          }
          this._searchloader = false;
          this.expenseMasterData.showUpdateStatusBtn = false;
        },
        error => {
          this._searchloader = false;
        });
  }


  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  getDetail(id) {
    this.isSupplierDetails = true;
    this.idCurrentSupplier = id;
  }

  addSupp() {
    this.isSupplierDetails = true;
    this.idCurrentSupplier = null;
  }

  setStatus(id: number, idstatus: number) {
    this.service.setStatus(id, idstatus, false)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.refresh();
          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
        },
        error => {
          this.setError(error);
        });

    this.modelRef.close();
    this.currentItemId = 0;
    this.nextStatusId = 0;
    this.nextStatusLabel = '';
  }
  GetStatusColor(statusid?) {
    if (this._statuslist != null && this._statuslist.length > 0 && statusid != null) {
      var result = this._statuslist.find(x => x.id == statusid);
      if (result)
        return result.labelColor;
    }
  }
  SearchByStatus(status) {
    this.model.statusValues = [];
    this.model.statusValues.push(status);
    this.getData();
  }
  public getstatus(id: number): string {
    return this.data.statusList.filter(x => x.id == id)[0].label;
  }


  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }

  downloadFile(data, mimeType) {

    const blob = new Blob([data], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "expense_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "expense_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }

  exportItem(iteminfo: ExpenseClaimItemModel) {
    this.exportDataLoading = true;
    this.service.exportClaim(iteminfo.id)
      .subscribe(res => {
        this.downloadFileWithName(res, "application/zip", `${iteminfo.claimNo}.zip`);
      },
        error => {
          this.exportDataLoading = false;
        });
  }

  reset() {
    this.model.items = [];
  }

  downloadFileWithName(data, mimeType, fileName) {

    const blob = new Blob([data], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      const a: HTMLAnchorElement = document.createElement('a') as HTMLAnchorElement;

      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();

      document.body.removeChild(a);
      URL.revokeObjectURL(url);
    }
    this.exportDataLoading = false;
  }

  openConfirm(content, id: number) {
    this.rejectId = id;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  openConfirmStatus(content, id: number, idStatus: number) {
    this.nextStatusId = idStatus;
    this.currentItemId = id;

    let status = this.data.statusList.filter(x => x.id == idStatus)[0];

    if (status != null)
      this.nextStatusLabel = status.label;

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  reject() {
    if (!this.rejectComment || this.rejectComment == "") {
      this.isRejectValid = false;
      return;
    }

    this.service.reject(this.rejectId, this.rejectComment)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();
            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.refresh();
          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
        },
        error => {
          this.setError(error);
        });

    this.modelRef.close();
    this.rejectComment = "";

  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  clearDateInput(controlName: any) {
    switch (controlName) {
      case "StartDate": {
        this.model.startDate = null;
        break;
      }
      case "EndDate": {
        this.model.endDate = null;
        break;
      }
    }
  }

  //export voucher kpi
  voucherExport() {
    this.exportVoucherKpiLoading = true;
    this.model.claimIdList = [];
    this.selectedExpenseIdList.forEach(element => {
      this.model.claimIdList.push(element);
    });
    this.service.exportVoucherKpiSummary(this.model)
      .subscribe(res => {
        this.downloadFileKpi(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "voucherKpi");
      },
        error => {
          this.exportVoucherKpiLoading = false;
        });
  }

  //export expense summary kpi
  expenseSummaryKpiExport() {
      this.exportExpenseSummaryKpiLoading = true;
      this.model.claimIdList = [];
      this.selectedExpenseIdList.forEach(element => {
        this.model.claimIdList.push(element);
      });
      this.service.exportExpenseKpiSummary(this.model)
        .subscribe(res => {
          if (this.modelRef)
            this.modelRef.close();
          if (this.model.exportType == ExpenseSummaryExportType.ExpenseSummaryKPI) {
            this.downloadFileKpi(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "expenseSummaryKpi");
          }
          else {
            this.downloadFileKpi(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "expenseSummaryDetailKpi");
          }
          this.model.exportType = ExpenseSummaryExportType.ExpenseSummaryKPI;
        },
          error => {
            this.exportExpenseSummaryKpiLoading = false;
          });

  }


  downloadFileKpi(data, mimeType, type) {

    const blob = new Blob([data], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "expense_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);

      if (type == "voucherKpi") {
        a.download = "voucher_kpi.xlsx";
      }
      else if (type == "expenseSummaryDetailKpi") {
        a.download = "expense_summary_details_kpi.xlsx";
      }
      else {
        a.download = "expense_summary_kpi.xlsx";
      }

      a.href = url;
      a.click();
    }
    type == "voucherKpi" ? this.exportVoucherKpiLoading = false : this.exportExpenseSummaryKpiLoading = false;
  }

  //check and uncheck of claims
  selectedClaimData(item) {
    let _selectedCount = 0;
    let totalItemCount = 0;

    const itemIndex = this.selectedExpenseIdList.findIndex(x=>x == item.id);
    if(item.isChecked && itemIndex < 0)
      this.selectedExpenseIdList.push(item.id);
    else if(!item.isChecked && itemIndex >= 0)
      this.selectedExpenseIdList.splice(itemIndex, 1)
    
    _selectedCount = this.selectedExpenseIdList.length;
    totalItemCount = this.expenseIdList.length;
    
    if (_selectedCount != totalItemCount)
      this.selectAllChecked = false;
    else
      this.selectAllChecked = true;

    if (this.model.employeeValues != undefined && this.model.employeeValues.length == 1 && _selectedCount > 0) {
      this.showVoucherExport = true;
      this.showExpenseKpi = true;
    }
    else if (_selectedCount > 0) {
      this.showExpenseKpi = true;
    }
    else {
      this.showVoucherExport = false;
      this.showExpenseKpi = false;
    }

    this.showOrHideUpdateStatusButton();
  }

  //on employee dropdown change
  employeeChange() {
    var items = this.model.items[0];

    var selectedCount = items.items.filter(x => x.isChecked).length;

    if (this.model.employeeValues != undefined && this.model.employeeValues.length == 1 && selectedCount > 0) {
      this.showVoucherExport = true;
    }
    else {
      this.showVoucherExport = false;
    }
  }

  //on header checkbox click
  selectAllCheckBox() {

    let _selectedCount = 0;
    var data = this.model.items;
    var selectAll = this.selectAllChecked;
    this.selectedExpenseIdList = [];
    if(selectAll){
      this.expenseIdList.forEach(element => {
        this.selectedExpenseIdList.push(element);
      });
    }
    _selectedCount = this.selectedExpenseIdList.length;
    data.forEach(element => {

      for (var i = 0; i < element.items.length; i++) {
        element.items[i].isChecked = selectAll;
      }

    });


    if (_selectedCount > 0) {

      this.showExpenseKpi = true;

      if (this.model.employeeValues != undefined && this.model.employeeValues.length == 1) {
        this.showVoucherExport = true;
      }
    }
    else {

      this.showVoucherExport = false;
      this.showExpenseKpi = false;
    }

    this.showOrHideUpdateStatusButton();
  }

  //assign the employee list based on the employee type change
  changeEmployeeType() {
    this.hrOutSourceCompanyList = [];
    this.data.employeeList = [];
    this.model.employeeValues=[];

   if (this.model.employeeTypeId == this.employeeTypeEnum.Outsource) {
      this.getHROutSourceCompanyData();
      this.getEmployeeByOutSourceCompany();
    }
    else if (this.model.employeeTypeId == this.employeeTypeEnum.Permanent)
      this.data.employeeList = this.internalStaffList;
  }

  //get the hr outsource company data
  getHROutSourceCompanyData() {

    this.hrOutSourceCompanyLoading = true;
    this.hrService.getHROutSourceCompanyDataList().
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(response => {
        this.processHROutCompanyResponse(response);
      }),
      error => {
        this.hrOutSourceCompanyLoading = false;
        this.setError(error);
      };
  }

  //process the hr outsource company response
  processHROutCompanyResponse(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.hrOutSourceCompanyList = response.hrOutSourceCompanyList;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_COMPANY_NOT_FOUND');
    }
    this.hrOutSourceCompanyLoading = false;
  }

  //get the employee by outsource company
  getEmployeeByOutSourceCompany() {

    this.model.employeeValues = [];
    this.expenseMasterData.employeeTypeLoading = true;
    this.hrService.getStaffListByCompanyIds(this.model.companyIds).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(response => {

        this.processEmployeeOutSourceCompany(response);

        this.expenseMasterData.employeeTypeLoading = false;
      }),
      error => {
        this.expenseMasterData.employeeTypeLoading = false;
        this.setError(error);
      };
  }

  processEmployeeOutSourceCompany(response) {
    if (response && response.result == DataSourceResult.Success) {
      this.data.employeeList = response.staffList;
    }
    else if (response && response.result == DataSourceResult.NotFound) {
      this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE_SUMMARY.MSG_EMPLOYEE_NOT_FOUND');
    }
  }

  //outsoruce company change event
  changeOrClearOutSourceCompany() {
    this.model.employeeValues = [];
    this.data.employeeList = [];
    this.getEmployeeByOutSourceCompany();
  }

  //get hr payroll company list
  getHRPayrollCompanyList() {
    this.expenseMasterData.payrollCompanyLoading = true;
    this.hrService.getHRPayrollCompanyList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.expenseMasterData.payrollCompanyList = response.hrPayrollCompanyList;
          this.expenseMasterData.payrollCompanyLoading = false;
        },
        error => {
          this.setError(error);
          this.expenseMasterData.payrollCompanyLoading = false;
        });
  }

  toggleSection() {
    this.expenseMasterData.toggleFormSection = !this.expenseMasterData.toggleFormSection;
  }

  //open popup
  openUpdateStatusConfirm(content) {

    var AutoclaimSelectedItems = new Array<ExpenseClaimItemModel>();
    var claimSelectedItems = new Array<ExpenseClaimItemModel>();

    var autoStatusName: string;
    var statusName: string;

    this.model.items.forEach(claimItems => {
      //get the selected expense claim
      var items = claimItems.items.filter(x => x.isChecked && !x.isAutoExpense);
      items.forEach(element => {
        claimSelectedItems.push(element);
      });
      //get the selected auto expense claim
      var items = claimItems.items.filter(x => x.isChecked && x.isAutoExpense)
      items.forEach(element => {
        AutoclaimSelectedItems.push(element);
      });
    });

    //count of auto expense
    var autoExpenseCount = AutoclaimSelectedItems && AutoclaimSelectedItems.length > 0 ? AutoclaimSelectedItems.length : 0;

    //count of normal expense
    var expenseCount = claimSelectedItems && claimSelectedItems.length > 0 ? claimSelectedItems.length : 0;

    //get and status id and auto status id
    var statusId = claimSelectedItems && claimSelectedItems.length > 0 ? claimSelectedItems[0].statusId : 0;

    var autoStatusId = AutoclaimSelectedItems && AutoclaimSelectedItems.length > 0 ? AutoclaimSelectedItems[0].statusId : 0;

    //assign the status name and auto status name
    if (statusId == ExpenseClaimStatusEnum.Pending) {
      statusName = expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Checked) ?
        expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Checked).name : "";
    }
    else if (statusId == ExpenseClaimStatusEnum.Approved) {
      statusName = expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Paid) ?
        expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Paid).name : "";
    }
    if (autoStatusId == ExpenseClaimStatusEnum.Pending) {
      autoStatusName = expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Approved) ?
        expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Approved).name : "";
    }
    else if (autoStatusId == ExpenseClaimStatusEnum.Approved) {
      autoStatusName = expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Paid) ?
        expenseClaimStatus.find(x => x.id == ExpenseClaimStatusEnum.Paid).name : "";
    }

    //frame the message
    if (autoExpenseCount > 0 && expenseCount > 0) {
      this.expenseMasterData.updatetitleMsg = this.utility.textTranslate('COMMON.MSG_YOU_ARE_ABOUT_UPDATE') +
        " " + expenseCount + " " +
        this.utility.textTranslate('EXPENSE_SUMMARY.MSG_EXPENSE_CLAIMS_STATUS') + " "
        + "<b>" + statusName + "</b>" + " and " + autoExpenseCount + " " +
        this.utility.textTranslate('EXPENSE_SUMMARY.MSG_AUTO_EXPENSE_CLAIMS_STATUS') + " " + "<b>" + autoStatusName + "</b>";
    }
    else if (autoExpenseCount > 0) {
      this.expenseMasterData.updatetitleMsg = this.utility.textTranslate('COMMON.MSG_YOU_ARE_ABOUT_UPDATE') + " " +
        autoExpenseCount + " " +
        this.utility.textTranslate('EXPENSE_SUMMARY.MSG_AUTO_EXPENSE_CLAIMS_STATUS') + " " + "<b>" + autoStatusName + "</b>";

    }
    else if (expenseCount > 0) {
      this.expenseMasterData.updatetitleMsg = this.utility.textTranslate('COMMON.MSG_YOU_ARE_ABOUT_UPDATE') +
        " " + expenseCount + " " +
        this.utility.textTranslate('EXPENSE_SUMMARY.MSG_EXPENSE_CLAIMS_STATUS') + " "
        + "<b>" + statusName + "</b>";
    }
    this.framePendingExpenseRequest(content);
  }

  //update the claim status
  updateStatusOk() {
    let updateStatusList = new Array<ExpenseClaimUpdateStatus>();

    this.model.items.forEach(ele => {
      var items = ele.items.filter(x => x.isChecked)

      items.forEach(claimElement => {

        if (claimElement.isChecked) {
          let updateStatus = new ExpenseClaimUpdateStatus();

          updateStatus.id = claimElement.id;
          updateStatus.expenseType = claimElement.isAutoExpense;
          updateStatus.currentStatusId = claimElement.statusId;

          if (claimElement.isAutoExpense && claimElement.statusId == ExpenseClaimStatusEnum.Pending) {
            updateStatus.nextStatusId = ExpenseClaimStatusEnum.Approved;
          }
          else if (!(claimElement.isAutoExpense) && claimElement.statusId == ExpenseClaimStatusEnum.Pending) {
            updateStatus.nextStatusId = ExpenseClaimStatusEnum.Checked;
          }
          else if (claimElement.statusId == ExpenseClaimStatusEnum.Approved) {
            updateStatus.nextStatusId = ExpenseClaimStatusEnum.Paid;
          }
          updateStatusList.push(updateStatus);
        }
      });
    });
    this.setStatusList(updateStatusList);
  }

  //update the status
  setStatusList(updateStatusList: Array<ExpenseClaimUpdateStatus>) {
    this.expenseMasterData.updateStatusLoading = true;
    this.service.setStatusList(updateStatusList)
      .pipe()
      .subscribe(
        data => {
          if (data) {
            this.headerService.getTasks();

            this.showSuccess('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIMLIST_STATUS');
            this.refresh();
          }
          else {
            this.showError('EXPENSE.EXPENSECLAIM_RESULT', 'EXPENSE.MSG_EXPCLAIM_ERROR_STATUS');
          }
          this.modelRef.close();
          this.expenseMasterData.updateStatusLoading = false;
        },
        error => {
          this.setError(error);
          this.modelRef.close();
          this.expenseMasterData.updateStatusLoading = false;
        });
  }

  showOrHideUpdateStatusButton() {
    //update status button show start - show only pending and approved items selected
    var isAnyChecked = this.model.items.filter(x => x.items.filter(y => y.isChecked).length >= 1).length > 0;

    if (isAnyChecked) {

      //pending item selected
      var isPendingitems = this.model.items.filter(y => y.items.filter(x => x.isChecked && x.statusId == ExpenseClaimStatusEnum.Pending).length >= 1).length > 0;
      var isApproveitems = this.model.items.filter(y => y.items.filter(x => x.isChecked && x.statusId == ExpenseClaimStatusEnum.Approved).length >= 1).length > 0;

      //other than pending or approved selected
      var isNonApproveOrPendingitems = this.model.items.filter(y => y.items.filter(x => x.isChecked && x.statusId != ExpenseClaimStatusEnum.Approved &&
        x.statusId != ExpenseClaimStatusEnum.Pending).length >= 1).length > 0;

      if (isNonApproveOrPendingitems) {
        this.expenseMasterData.showUpdateStatusBtn = false;
      }
      else {
        //pending and approved should not select together
        this.expenseMasterData.showUpdateStatusBtn = (isPendingitems || isApproveitems) &&
          !(isPendingitems && isApproveitems);
      }
    }
    else {
      this.expenseMasterData.showUpdateStatusBtn = false;
    }
    //update status button show end
  }
  //frame the pending expense request
  framePendingExpenseRequest(content) {
    let pendingExpenseRequestList = new Array<PendingBookingExpenseRequest>();

    this.model.items.forEach(ele => {
      var items = ele.items.filter(x => x.isChecked)
      items.forEach(claimElement => {

        if (claimElement.isChecked) {

          let pendingExpenseRequest = new PendingBookingExpenseRequest();

          if (claimElement.bookingIdList && claimElement.bookingIdList.length > 0 && claimElement.isAutoExpense) {

            pendingExpenseRequest.qcId = claimElement.staffId;
            pendingExpenseRequest.bookingIdList = claimElement.bookingIdList;
            pendingExpenseRequestList.push(pendingExpenseRequest);

          }
        }
      });
    });

    if (pendingExpenseRequestList.length > 0) {
      this.getPendingExpenseBookingIdList(pendingExpenseRequestList, content);
    }
    else {
      this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
    }
  }

  //get not configured booking id list
  getPendingExpenseBookingIdList(PendingBookingExpenseRequestList: Array<PendingBookingExpenseRequest>, content) {
    this.expenseMasterData.pendingExpenseExistLoading = true;

    this.service.getPendingExpenseBookingIdList(PendingBookingExpenseRequestList)
      .pipe()
      .subscribe(
        response => {
          if (response.result == PendingBookingExpenseResponseResult.configure) {
            this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
          }
          else if (response.result == PendingBookingExpenseResponseResult.notConfigure) {

            var commaSplitBookingStr = response.bookingIdList.join(", #");

            var message = this.utility.textTranslate('EXPENSE.MSG_TRAVEL_TARIFF_FOOD_ALLOWANCE_NOT_CONFIGURE') +
              "#" + commaSplitBookingStr;

            this.showWarning('EXPENSE.EXPENSECLAIM_RESULT', message);
          }
          this.expenseMasterData.pendingExpenseExistLoading = false;
        },
        error => {
          this.setError(error);
          this.expenseMasterData.pendingExpenseExistLoading = false;
        });
  }
    openExportPopup(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  searchData(){
    this.selectedExpenseIdList = [];
    this.search();
  }

  mapCheckedItem() {
    let _selectedCount = this.selectedExpenseIdList.length;
    let totalItemCount = this.expenseIdList.length;
    this.model.items.forEach(element => {
      for (const item of element.items) {
        const selectedExpense = this.selectedExpenseIdList.find(x=>x == item.id);
        if(selectedExpense)
          item.isChecked = true;
      }
    });

    if (_selectedCount != totalItemCount)
      this.selectAllChecked = false;
    else
      this.selectAllChecked = true;

    if (_selectedCount > 0) {
      this.showExpenseKpi = true;
      if (this.model.employeeValues != undefined && this.model.employeeValues.length == 1)
        this.showVoucherExport = true;
    }
    else {
      this.showVoucherExport = false;
      this.showExpenseKpi = false;
    }

    this.showOrHideUpdateStatusButton();
  }
}
