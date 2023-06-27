import { HRProfileEnum, NotificationList, RoleEnum } from './../../common/static-data-common';
import { saveAllocationStaff } from "./../../../_Models/Schedule/scheduleallocationmodel";
import { UserAccountService } from "src/app/_Services/UserAccount/useraccount.service";
import { HrService } from 'src/app/_Services/hr/hr.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import {
  EditCSAllocationMasterModel,
  EditCSAllocationModel,
  EditSelectedStaffs,
} from "./../../../_Models/customer/cs-allocation.model";
import { CustomerBrandService } from "./../../../_Services/customer/customerbrand.service";
import { ProductManagementService } from "./../../../_Services/productmanagement/productmanagement.service";
import { OfficeService } from "./../../../_Services/office/office.service";
import { CSAllocationDeleteItem, CsAllocationSearchModel } from "src/app/_Models/customer/cs-allocation-summary.model";
import { CustomerService } from "./../../../_Services/customer/customer.service";
import { CsAllocationMasterModel } from "./../../../_Models/customer/cs-allocation-summary.model";
import { Component } from "@angular/core";
import { NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { AuthenticationService } from "../../../_Services/user/authentication.service";
import { JsonHelper, Validator } from "../../common";
import { SummaryComponent } from "../../common/summary.component";
import { Router, ActivatedRoute } from "@angular/router";
import { csDeleteItem } from "src/app/_Models/customer/csconfig-summary.model";
import { CSConfigService } from "../../../_Services/customer/csconfig.service";
import { UtilityService } from "src/app/_Services/common/utility.service";
import {
  CommonDataSourceRequest,
  CountryDataSourceRequest,
  ResponseResult,
} from "src/app/_Models/common/common.model";
import {
  DefaultUserType,
  ListSize,
  PageSizeCommon,
  staffUserTypes,
} from "../../common/static-data-common";
import {
  catchError,
  debounceTime,
  distinctUntilChanged,
  first,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import { of, Subject, BehaviorSubject } from "rxjs";
import { eventTarget } from "@amcharts/amcharts4/.internal/core/utils/DOM";
import { ToastrService } from "ngx-toastr";
import { TranslateService } from "@ngx-translate/core";
import { LocationService } from 'src/app/_Services/location/location.service';

@Component({
  selector: "app-cs-allocation",
  templateUrl: "./cs-allocation.component.html",
  styleUrls: ["./cs-allocation.component.css"],
})
export class CSAllocationComponent extends SummaryComponent<CsAllocationSearchModel> {
  staffValidators: Array<any>;
  public jsonHelper: JsonHelper;
  public model: CsAllocationSearchModel;
  public modelRemove: CSAllocationDeleteItem;
  public editModel: EditCSAllocationModel;
  public masterModel: CsAllocationMasterModel;
  requestCustomerModel: CommonDataSourceRequest;
  public editMasterModel: EditCSAllocationMasterModel;
  public selectedAllCSSummary: boolean = false;
  initialLoading = false;
  LoadFirstTime = false;
  staffUserTypes: any = staffUserTypes;
  public selectedPageSize = PageSizeCommon[0];
  public pagesizeitems = PageSizeCommon;
  public popUpOpen: boolean;
  deleteBtnShow: boolean = false;
  deleteId: any[] = [];
  public modelRef: NgbModalRef;
  staffInput: BehaviorSubject<string>;
  countryInput: BehaviorSubject<string>;
  saveloading: boolean;
  componentDestroyed$: Subject<boolean> = new Subject();

  constructor(public csConfigService: CSConfigService,
    public hrService: HrService,
    public locationService: LocationService,
    public officeService: OfficeService,
    public customerBrandService: CustomerBrandService,
    public productManagementService: ProductManagementService,
    public userAccountService: UserAccountService,
    public cusService: CustomerService,
    public modalService: NgbModal,
    authService: AuthenticationService,
    public utility: UtilityService,
    router: Router,
    route: ActivatedRoute,
    public validator: Validator,
    protected translate: TranslateService,
    protected toastr: ToastrService
  ) {
    super(router, validator, route, translate, toastr, utility);
    // this.validator.setJSON("customer/csconfig-summary.valid.json");
    // this.validator.setModelAsync(() => this.model);
    // this.validator.isSubmitted = false;
    this.model = new CsAllocationSearchModel();
    this.masterModel = new CsAllocationMasterModel();
    this.masterModel.searchloading = false;
    this.masterModel.exportLoading = false;
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.jsonHelper = validator.jsonHelper;
  }
  onInit() {
    this.model.userType = DefaultUserType.CS;
    this.getCustomerListBySearch();
    this.getOfficeLocationList();
    this.getServices();
    this.getProductCategoryData();
    this.getStaffListBySearch();
    this.getCountryListBySearch();
  }

  getData() {
    this.masterModel.searchloading = true;
    this.csConfigService
      .getCustomerAllocation(this.model)
      .subscribe((response) => {
        if (response && response.result == 1) {
          this.mapPageProperties(response);
          this.model.items = response.csList;
        } else if (response && response.result == 2) {
          this.model.noFound = true;
          this.model.items = [];
        } else {
          this.error = response.result;
        }
        this.masterModel.searchloading = false;
      });
  }
  getPathDetails(): string {
    return "csconfigedit/csconfig-register";
  }
  searchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.selectedAllCSSummary = false;
    this.getData();
  }

  getCustomerListBySearch() {
    this.masterModel.customerInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.masterModel.customerLoading = true)),
        switchMap((term) =>
          this.cusService
            .getCustomerDataSourceList(this.requestCustomerModel, term)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.masterModel.customerLoading = false))
            )
        )
      )
      .subscribe((data) => {
        this.masterModel.customerList = data;
        this.masterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText =
        this.masterModel.customerInput.getValue();
      this.requestCustomerModel.skip = this.masterModel.customerList.length;
    }

    this.masterModel.customerLoading = true;
    this.cusService
      .getCustomerDataSourceList(this.requestCustomerModel)
      .subscribe((customerData) => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.masterModel.customerList =
              this.masterModel.customerList.concat(customerData);
          }
        }
        this.masterModel.customerLoading = false;
      }),
      (error) => {
        this.masterModel.customerLoading = false;
        this.setError(error);
      };
  }

  changeCustomerData(item) {
    if (item && item.id > 0) {
      this.masterModel.brandList = [];
      this.model.brandIds = [];
      this.masterModel.deptList = [];
      this.model.departmentIds = [];
      this.getBrandList(item.id);
      this.getDeptList(item.id);
    }
  }

  //get the office list
  getOfficeLocationList() {
    this.masterModel.officeLoading = true;
    this.officeService
      .getOfficeDetails()
      .pipe(first())
      .subscribe(
        (response) => {
          this.processOfficeLocationResponse(response);
        },
        (error) => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        }
      );
  }

  //process the office location response
  processOfficeLocationResponse(response) {
    if (response && response.result == ResponseResult.Success)
      this.masterModel.officeList = response.dataSourceList;
    else if (response && response.result == ResponseResult.NoDataFound)
      this.showError(
        "BOOKING_SUMMARY.TITLE",
        "BOOKING_SUMMARY.TITLE.MSG_OFFICE_NOT_FOUND"
      );
    this.masterModel.officeLoading = false;
  }

  //get the office list
  getServices() {
    this.masterModel.serviceLoading = true;
    this.csConfigService.getService().subscribe(
      (response) => {
        this.masterModel.serviceList = response.serviceList;
        this.masterModel.serviceLoading = false;
      },
      (error) => {
        this.setError(error);
        this.masterModel.serviceLoading = false;
      }
    );
  }

  getProductCategoryData() {
    this.masterModel.productCategoryListLoading = true;
    this.productManagementService
      .getProductCategorySummary()
      .subscribe((response) => {
        if (response && response.result == ResponseResult.Success) {
          this.masterModel.productCategoryList = response.productCategoryList;
        }
        this.masterModel.productCategoryListLoading = false;
      }),
      (error) => {
        this.masterModel.productCategoryListLoading = false;
      };
  }

  getBrandList(customerId, forEdit: boolean = false) {
    // this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    // this.masterModel.brandSearchRequest.skip = this.masterModel.brandList.length;

    if (forEdit) {
      this.editMasterModel.brandLoading = true;
    } else {
      this.masterModel.brandLoading = true;
    }

    this.customerBrandService
      .getEditCustomerBrand(customerId)
      .subscribe((brandData) => {
        if (forEdit) {
          if (
            brandData &&
            brandData.customerBrands &&
            brandData.customerBrands.length > 0
          ) {
            this.editMasterModel.brandList = brandData.customerBrands;
          }
          this.editMasterModel.brandLoading = false;
        } else {
          if (
            brandData &&
            brandData.customerBrands &&
            brandData.customerBrands.length > 0
          ) {
            this.masterModel.brandList = brandData.customerBrands;
          }
          this.masterModel.brandLoading = false;
        }
      }),
      (error: any) => {
        if (forEdit) {
          this.editMasterModel.brandLoading = false;
        } else {
          this.masterModel.brandLoading = false;
        }
      };
  }

  getDeptList(customerId, forEdit = false) {
    // this.masterModel.brandSearchRequest.searchText = this.masterModel.brandInput.getValue();
    // this.masterModel.brandSearchRequest.skip = this.masterModel.brandList.length;

    if (forEdit) {
      this.editMasterModel.deptLoading = true;
    } else {
      this.masterModel.deptLoading = true;
    }
    this.cusService.getCustomerDepartments(customerId).subscribe((deptData) => {
      if (forEdit) {
        if (
          deptData &&
          deptData.dataSourceList &&
          deptData.dataSourceList.length > 0
        ) {
          this.editMasterModel.deptList = deptData.dataSourceList;
        }
        // this.masterModel.brandSearchRequest = new CommonCustomerSourceRequest();
        this.editMasterModel.deptLoading = false;
      } else {
        if (
          deptData &&
          deptData.dataSourceList &&
          deptData.dataSourceList.length > 0
        ) {
          this.masterModel.deptList = deptData.dataSourceList;
        }
        // this.masterModel.brandSearchRequest = new CommonCustomerSourceRequest();
        this.masterModel.deptLoading = false;
      }
    }),
      (error: any) => {
        if (forEdit) {
          this.editMasterModel.deptLoading = false;
        } else {
          this.masterModel.deptLoading = false;
        }
      };
  }
  clearStaff() {
    this.editModel.userIds = null;
    this.getStaffListBySearch(true);
  }

  openNewPopUp(content, _isEdit) {
    this.editMasterModel = new EditCSAllocationMasterModel();
    this.requestCustomerModel = new CommonDataSourceRequest();

    this.editModel = new EditCSAllocationModel();
    if (!_isEdit) {
      this.getEditCustomerListBySearch();
      this.getStaffListBySearch(true);
      this.getCountryListBySearch(true);
    }
    this.editModel.isEdit = _isEdit;
    this.validator.setJSON("customer/cs-allocation.valid.json");
    this.validator.setModelAsync(() => this.editModel);
    this.validator.isSubmitted = false;

    // this.isEdit = _isEdit;

    this.validator.initTost();
    this.validator.isSubmitted = false;

    this.editMasterModel.productCategoryList =
      this.masterModel.productCategoryList;
    this.editMasterModel.officeList = this.masterModel.officeList;
    this.editMasterModel.serviceList = this.masterModel.serviceList;
    this.editMasterModel.staffList = this.masterModel.staffList;
    this.staffValidators = [];
    // if (!this.isEdit) {
    //   this.getEditProductSubCategory2ListBySearch();
    //   this.getEditProductSubCategory3ListBySearch();
    // }

    // this.editMasterModel.loading = false;
    this.modelRef = this.modalService.open(content, {
      windowClass: "lgModelWidth",
      size: "lg",
      centered: true,
      backdrop: "static",
    });
    this.popUpOpen = true;
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.staffValidators) item.validator.isSubmitted = true;
    if (this.formValid()) {
      this.saveloading = true;
      this.editModel.staffs = this.staffValidators.map(x => x.staff);
      this.csConfigService.saveCustomerAllocation(this.editModel)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.validator.isSubmitted = false;
              this.showSuccess('CS_ALLOCATION.MSG_SAVE_RESULT', 'CS_ALLOCATION.MSG_SAVE_OK');
              this.editModel = new EditCSAllocationModel();
              this.saveloading = false;
              this.cancel();
            }
            else if (res.result == 3) {
              this.showError('CS_ALLOCATION.MSG_SAVE_RESULT', 'CS_ALLOCATION.MSG_CS_ALREADY_CONFIGURED');
            }
            this.saveloading = false;
            this.searchDetails();
          },
          error => {
            this.saveloading = false;
          });
    }
  }

  formValid(): boolean {
    return (
      this.validator.isValid("customerId") &&
      this.validator.isValid("serviceIds") &&
      this.validator.isValid("userIds") &&
      this.staffValidators.every((x) => x.validator.isValid("profile")) &&
      this.isPrimaryCSAndReportCheckerValid()
    );
  }

  getEditCustomerListBySearch() {

    if (this.editModel.customerId) {
      this.requestCustomerModel.id = this.editModel.customerId;
    }
    this.editMasterModel.customerInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.editMasterModel.customerLoading = true)),
        switchMap((term) =>
          this.cusService
            .getCustomerDataSourceList(this.requestCustomerModel, term)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.editMasterModel.customerLoading = false))
            )
        )
      )
      .subscribe((data) => {
        this.editMasterModel.customerList = data;
        this.editMasterModel.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getEditCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText =
        this.editMasterModel.customerInput.getValue();
      this.requestCustomerModel.skip = this.editMasterModel.customerList.length;
    }

    this.editMasterModel.customerLoading = true;
    this.cusService
      .getCustomerDataSourceList(this.requestCustomerModel)
      .subscribe((customerData) => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
            this.editMasterModel.customerList =
              this.editMasterModel.customerList.concat(customerData);
          }
        }
        this.editMasterModel.customerLoading = false;
      }),
      (error) => {
        this.editMasterModel.customerLoading = false;
        this.setError(error);
      };
  }

  changeEditCustomerData(item) {
    if (item && item.id > 0) {
      this.editMasterModel.brandList = [];
      this.editModel.brandIds = [];
      this.editMasterModel.deptList = [];
      this.editModel.departmentIds = [];
      this.getDeptList(item.id, true);
      this.getBrandList(item.id, true);
    }
  }

  getStaffListBySearch(forEdit = false) {
    if (forEdit) {
      this.staffInput = this.editMasterModel.staffInput;
      this.editMasterModel.requestStaffDataSource.idList = this.editModel.userIds;
    }
    else {
      this.staffInput = this.masterModel.staffInput;
    }

    this.staffInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => {
        if (forEdit) {
          this.editMasterModel.staffLoading = true
        }
        else {
          this.masterModel.staffLoading = true
        }
      }),
      switchMap(term =>

        this.hrService.getStaffUserList(forEdit ? this.editMasterModel.requestStaffDataSource : this.masterModel.requestStaffDataSource, term)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => {
              if (forEdit) {
                this.editMasterModel.staffLoading = false
              }
              else {
                this.masterModel.staffLoading = false
              }
            }))
      ))
      .subscribe(data => {
        if (forEdit) {
          this.editMasterModel.staffList = data;
          this.editMasterModel.staffLoading = false;
        } else {
          this.masterModel.staffList = data;
          this.masterModel.staffLoading = false;
        }
      });
  }

  //fetch the customer data with virtual scroll
  getStaffData(IsVirtual: boolean, forEdit = false) {
    if (IsVirtual) {
      if (forEdit) {
        this.editMasterModel.requestStaffDataSource.idList = [];
        this.editMasterModel.requestStaffDataSource.searchText = this.editMasterModel.staffInput.getValue();
        this.editMasterModel.requestStaffDataSource.skip = this.editMasterModel.staffList.length;
        this.editMasterModel.staffLoading = true;
      } else {
        this.masterModel.requestStaffDataSource.searchText = this.masterModel.staffInput.getValue();
        this.masterModel.requestStaffDataSource.skip = this.masterModel.staffList.length;
        this.masterModel.staffLoading = true;
      }
    }
    this.hrService.getStaffUserList(forEdit ? this.editMasterModel.requestStaffDataSource : this.masterModel.requestStaffDataSource).
      subscribe(response => {
        if (IsVirtual) {
          if (forEdit) {
            this.editMasterModel.requestStaffDataSource.skip = 0;
            this.editMasterModel.requestStaffDataSource.take = ListSize;
            if (response && response.length > 0) {
              this.editMasterModel.staffList =
                this.editMasterModel.staffList.concat(response);
            }
            this.editMasterModel.staffLoading = false;
          } else {
            this.masterModel.requestStaffDataSource.skip = 0;
            this.masterModel.requestStaffDataSource.take = ListSize;
            if (response && response.length > 0) {
              this.masterModel.staffList =
                this.masterModel.staffList.concat(response);
            }
            this.masterModel.staffLoading = false;
          }
        }

      }),
      (error) => {
        this.editMasterModel.staffLoading = false;
        this.masterModel.staffLoading = false;
        this.setError(error);
      };
  }

  changeStaffData(items: Array<any>) {
    // this.editModel.staffs = this.editModel.staffs.filter(x => items.some(y => y.id == x.id));
    let staffValidators = this.staffValidators.filter((x) =>
      items.some((y) => y.id === x.staff.id)
    );
    if (!staffValidators.some((x) => x.staff.primaryCS)) {
      this.hideCheckBox(staffValidators, true, "isprimaryCSHide");
    }
    if (!staffValidators.some((x) => x.staff.primaryReportChecker)) {
      this.hideCheckBox(staffValidators, true, "isprimaryReportCheckerHide");
    }
    this.staffValidators = staffValidators;
    for (const item of items) {
      if (!staffValidators.some((x) => x.staff.id == item.id)) {
        let staff = new EditSelectedStaffs();
        staff.id = item.id;
        staff.name = item.name;
        staff.isprimaryCSHide = !staffValidators.some((x) => x.staff.primaryCS);
        staff.isprimaryReportCheckerHide = !staffValidators.some(
          (x) => x.staff.primaryReportChecker
        );

        this.staffValidators.push({
          staff: staff,
          validator: Validator.getValidator(
            staff,
            "customer/cs-allocation-stafftable.valid.json",
            this.jsonHelper,
            this.validator.isSubmitted,
            this.toastr,
            this.translate
          ),
        });
      }
    }
  }

  selectAllCS() {
    for (var i = 0; i < this.model.items.length; i++) {
      this.model.items[i].selected = this.selectedAllCSSummary;
    }
    this.deleteBtnShow = this.selectedAllCSSummary ? true : false;
    this.deleteId = [];
    if (this.selectedAllCSSummary) {
      for (var i = 0; i < this.model.items.length; i++) {
        if (this.model.items[i].selected == true)
          this.deleteId.push(this.model.items[i].id);
      }
    } else this.deleteId = [];
  }
  checkIfAllCSSelected() {
    this.selectedAllCSSummary = this.model.items.every(function (item: any) {
      return item.selected == true;
    });
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].selected == true) {
        this.deleteBtnShow = true;
        break;
      } else {
        this.deleteBtnShow = false;
      }
    }
    this.deleteId = [];
    for (var i = 0; i < this.model.items.length; i++) {
      if (this.model.items[i].selected == true) {
        this.deleteId.push(this.model.items[i].id);
      }
    }
  }

  openConfirm(content) {
    this.modelRemove = {
      daUserCustomerIds: this.deleteId,
    };
    this.modelRef = this.modalService.open(content, {
      windowClass: "smModelWidth",
      centered: true,
    });
    this.modelRef.result.then(
      (result) => { },
      (reason) => { }
    );
  }

  deleteCSAllocations(removeModel) {
    this.csConfigService
      .deleteCSAllocations(removeModel)
      .pipe()
      .subscribe(
        (response) => {
          this.deleteId = [];
          if (response && (response.result == 1)) {
            this.deleteBtnShow = false;
            this.getData();
          } else {
            this.error = response.result;
            this.loading = false;
          }
        },
        (error) => {
          this.error = error;
          this.loading = false;
        }
      );
    this.modelRef.close();
  }

  cancel() {
    this.validator.initTost();
    this.validator.isSubmitted = false;
    this.editModel = new EditCSAllocationModel();
    this.modelRef.close();
  }

  onChnagePrimaryCS(event, id) {
    console.log(event);
    if (event.target.checked) {
      this.hideCheckBox(
        this.staffValidators.filter((x) => x.staff.id != id),
        false,
        "isprimaryCSHide"
      );
    } else {
      this.hideCheckBox(this.staffValidators, true, "isprimaryCSHide");
    }
  }

  onCngePrimaryReportChecker(event, id) {
    if (event.target.checked) {
      this.hideCheckBox(
        this.staffValidators.filter((x) => x.staff.id != id),
        false,
        "isprimaryReportCheckerHide"
      );
    } else {
      this.hideCheckBox(
        this.staffValidators,
        true,
        "isprimaryReportCheckerHide"
      );
    }
  }

  hideCheckBox(staffs: Array<any>, isShow: boolean, property: string) {
    staffs.forEach((x) => {
      x.staff[property] = isShow;
    });
  }

  changeProfile(event, staff) {
    if (event.id == HRProfileEnum.CS) {
      staff.notificationList = NotificationList;
      staff.notification = NotificationList.filter(x => x.id != RoleEnum.ReportChecker).map(x => x.id);
    }
    else {
      staff.notification = [RoleEnum.ReportChecker];
      staff.notificationList = NotificationList.filter(x => x.id === RoleEnum.ReportChecker);
      staff.primaryCS = false;
    }
  }

  onEditCSAllocation(id, content) {
    this.csConfigService.getCSAllocation(id).subscribe((res) => {
      if (res.result == ResponseResult.Success) {
        this.openNewPopUp(content, true);
        this.mapEditData(res);
        this.getEditCustomerListBySearch();
        this.getStaffListBySearch(true);
        this.getCountryListBySearch(true);
        this.changeEditCustomerData({ id: this.editModel.customerId });
      } else {
        this.model.items = [];
        this.model.noFound = true;
        this.editMasterModel.saveLoading = false;
      }
    });
  }

  mapEditData(res) {
    this.editModel = res.csAllocation;
    this.editModel.isEdit = true;
    this.editModel.userIds = res.csAllocation.staffs.map((x) => {
      return x.id;
    });
    for (const staff of res.csAllocation.staffs) {
      let staffModel = new EditSelectedStaffs();
      staffModel.id = staff.id;
      staffModel.isprimaryCSHide = true;
      staffModel.isprimaryReportCheckerHide = true;
      staffModel.primaryCS = staff.primaryCS;
      staffModel.primaryReportChecker = staff.primaryReportChecker;
      staffModel.notification = staff.notification;
      staffModel.profile = staff.profile;
      staffModel.name = staff.name;
      if (staffModel.profile == HRProfileEnum.CS) {
        staffModel.notificationList = NotificationList;
      }
      else {
        staffModel.notificationList = NotificationList.filter(x => x.id === RoleEnum.ReportChecker);
        // staff.primaryCS = false;
      }
      this.staffValidators.push({
        staff: staffModel,
        validator: Validator.getValidator(
          staff,
          "customer/cs-allocation-stafftable.valid.json",
          this.jsonHelper,
          this.validator.isSubmitted,
          this.toastr,
          this.translate
        ),
      });
    }
  }
  export() {
    this.masterModel.exportLoading = true;
    this.csConfigService.exportSummary(this.model).subscribe(
      (res) => {
        this.downloadFile(
          res,
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
      },
      (error) => {
        this.masterModel.exportLoading = false;
      }
    );
  }

  downloadFile(data, mimeType) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "CSAllocations.xlsx");
    } else {
      const a = document.createElement("a");
      const url = window.URL.createObjectURL(blob);
      a.download = "CSAllocations_Summary.xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportLoading = false;
  }

  clearEditCustomer() {
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.editModel.brandIds = [];
    this.editMasterModel.brandList = []
    this.editModel.departmentIds = [];
    this.editMasterModel.deptList = [];
    this.getEditCustomerListBySearch();
  }

  clearCustomer() {
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.model.brandIds = [];
    this.masterModel.brandList = []
    this.model.departmentIds = [];
    this.masterModel.deptList = [];
    // this.getCustomerListBySearch();
  }

  isPrimaryCSAndReportCheckerValid() {
    if (this.editModel.isEdit) {
      return true;
    }

    if (this.staffValidators.some(x => x.staff.profile == this.editMasterModel.hRProfileEnum.CS)) {
      const primaryCS = this.staffValidators.some(x => x.staff.primaryCS);
      if (!primaryCS) {
        this.showWarning('CS_ALLOCATION.MSG_SAVE_RESULT', 'CS_ALLOCATION.MSG_PRIMARY_CS_REQ');
        return false;
      }
    }

    const primaryReportChecker = this.staffValidators.some(x => x.staff.primaryReportChecker);
    if (!primaryReportChecker) {
      this.showWarning('CS_ALLOCATION.MSG_SAVE_RESULT', 'CS_ALLOCATION.MSG_PRIMARY_REPORTCHECKER_REQ');
      return false;
    }
    return true;
  }

  //fetch the country data with virtual scroll
  getCountryData(IsVirtual: boolean, forEdit = false) {

    if (IsVirtual) {
      if (forEdit) {
        this.editMasterModel.countryRequest.countryIds = [];
        this.editMasterModel.countryRequest.searchText = this.editMasterModel.countryInput.getValue();
        this.editMasterModel.countryRequest.skip = this.editMasterModel.countryList.length;
        this.editMasterModel.countryLoading = true;
      } else {
        this.masterModel.countryRequest.searchText = this.masterModel.countryInput.getValue();
        this.masterModel.countryRequest.skip = this.masterModel.countryList.length;
        this.masterModel.countryLoading = true;
      }
    }

    this.locationService.getCountryDataSourceList(forEdit ? this.editMasterModel.countryRequest : this.masterModel.countryRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(countryData => {
        if (IsVirtual) {
          if (forEdit) {
            this.editMasterModel.countryRequest.skip = 0;
            this.editMasterModel.countryRequest.take = ListSize;
            if (countryData && countryData.length > 0) {
              this.editMasterModel.countryList =
                this.editMasterModel.countryList.concat(countryData);
            }
            this.editMasterModel.countryLoading = false;
          } else {
            this.masterModel.countryRequest.skip = 0;
            this.masterModel.countryRequest.take = ListSize;
            if (countryData && countryData.length > 0) {
              this.masterModel.countryList = this.masterModel.countryList.concat(countryData);
            }
            this.masterModel.countryLoading = false;
          }
        }
      }),
      error => {
        this.masterModel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch(forEdit = false) {
    if (forEdit) {
      this.countryInput = this.editMasterModel.countryInput;
      this.editMasterModel.countryRequest.countryIds = this.editModel.factoryCountryIds;
    }
    else {
      this.countryInput = this.masterModel.countryInput;
    }

    this.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => {
        if (forEdit) {
          this.editMasterModel.countryLoading = true
        }
        else {
          this.masterModel.countryLoading = true
        }
      }),
      switchMap(term => this.locationService.getCountryDataSourceList(forEdit ? this.editMasterModel.countryRequest : this.masterModel.countryRequest, term)
        .pipe(takeUntil(this.componentDestroyed$),
          catchError(() => of([])), // empty list on error
          tap(() => {
            if (forEdit) {
              this.editMasterModel.countryLoading = true
            }
            else {
              this.masterModel.countryLoading = true
            }
          }))
      ))
      .subscribe(data => {
        if (forEdit) {
          this.editMasterModel.countryList = data;
          this.editMasterModel.countryLoading = false;
        } else {
          this.masterModel.countryList = data;
          this.masterModel.countryLoading = false;
        }
      });
  }

  clearCountryData(forEdit = false) {
    if (forEdit) {
      this.editModel.factoryCountryIds = [];
    }
    else {
      this.model.factoryCountryIds = [];
    }

    this.getCountryListBySearch(forEdit);
  }
}


