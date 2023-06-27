import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap, filter } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { Country, RoleEnum, SupplierType, Url, UserType } from '../../../components/common/static-data-common'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { SupplierSummaryItemModel } from '../../../_Models/supplier/suppliersummary.model';
import { EditSuplierModel, Address, CustomerContact, Customer, SaveSupplierResult, Grade } from '../../../_Models/supplier/edit-supplier.model'
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService } from "src/app/components/common/static-data-common";
import { NgbCalendar, NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserAccountModel, UserDetail } from 'src/app/_Models/useraccount/useraccount.model';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { CommonSupplierSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { BehaviorSubject, from } from 'rxjs';
import { of } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-editSupplier',
  templateUrl: './edit-supplier.component.html',
  styleUrls: ['./edit-supplier.component.css']
})
export class EditSupplierComponent extends DetailComponent {
  public model: EditSuplierModel;
  public data: any;

  public suppliersbyCountry: Array<SupplierSummaryItemModel>;
  public suppliersSelected: Array<SupplierSummaryItemModel>;
  public selectCounryId: number = 0;
  private jsonHelper: JsonHelper;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public selectedcustomerList: Array<any>;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public count: number = 0;
  public apiServiceList: any;
  public apiEntityList: any;
  public apiEntityLoading: boolean = false;
  public existSupplierData: any;
  //public apiContactServiceList: any;
  _country = Country;
  apiServiceEnum = APIService;
  public modelRef: NgbModalRef;
  public confirmExistSuplierModalRef: NgbModalRef;
  customerContact: CustomerContact;
  userdetail: UserDetail;
  savePopupLoading: boolean;
  showPopupLoading: boolean;
  searchSupplierModel: CommonSupplierSourceRequest;
  supplierInput: BehaviorSubject<string>;
  supplierLoading: boolean;
  selectedType: string;
  isNew: boolean = false;
  @ViewChild('confirmEntityMapSupplierContact') confirmEntityMapSupplierContact;
  // @ViewChild('confirmExistSuplier') confirmExistSuplier: ElementRef;
  @ViewChild('confirmExistSuplier') confirmExistSupliermodel: TemplateRef<any>;
  public confirmSupplierContactEntityMapModalRef: NgbModalRef;
  dbSupplierEntityIds: number[] = [];
  saveSupplierContactLoading = false;
  isPopupButtonDisaible = false;
  showStatisticsVisiblityField: boolean = false;
  public grade: Grade = new Grade();

  public gradeList: Array<any> = [];
  public isGradeLoading = false;
  constructor(
    public calendar: NgbCalendar,
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public utility: UtilityService,
    private service: SupplierService,
    private authService: AuthenticationService,
    public referenceService: ReferenceService, public modalService: NgbModal,
    private userAccountService: UserAccountService) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("supplier/edit-supplier.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this._translate = translate;
    this._toastr = toastr;
    this.customerContact = new CustomerContact();
    this.userdetail = new UserDetail();
    this.supplierInput = new BehaviorSubject<string>("");
    this.searchSupplierModel = new CommonSupplierSourceRequest();
    let user = this.authService.getCurrentUser();
    if (user.roles.find(x => x.id == RoleEnum.IT_Team))
      this.showStatisticsVisiblityField = true;

    //if()
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }

  onInit(id?: any) {
    this.init(id);
  }

  getViewPath(): string {
    return "supplieredit/view-supplier";
  }

  getEditPath(): string {
    return "supplieredit/edit-supplier";
  }

  public getAddPath(): string {
    return "supplieredit/new-supplier";
  }

  isFactory(): boolean {
    return this.model != null && this.model.typeId == 1;
  }

  isTypeEmpty(): boolean {
    if (!this.model.typeId) { return false; }

    return true;
  }

  isCountryChinaCountyValidation(item: Address): boolean {
    if (this.validator.isSubmitted) {
      if (this.model.addressList == null || this.model.addressList.length == 0)
        return false;

      var data = item.countryId;
      if (data == this._country.China && this.model.typeId == 1 && item.countyId == null) return false;
    }
    return true;
  }

  isCountryChinaTownValidation(item: Address): boolean {
    if (this.validator.isSubmitted) {
      if (this.model.addressList == null || this.model.addressList.length == 0)
        return false;

      var data = item.countryId
      if (data == this._country.China && this.model.typeId == 1 && item.townId == null) return false;

      if (data == Country.China && this.model.isNewSupplier && this.model.typeId == SupplierType.Supplier && item.townId == null) return false;
    }
    return true;
  }

  init(id?) {
    this.loading = true;
    this.suppliersSelected = [];
    this.model = new EditSuplierModel();
    this.data = {};
    this.validator.isSubmitted = false;
    this.isNew = id == null ? true : false;
    this.getAPIServices();

    this.getEntityList();

    //this.waitingService.open();
    this.service.getEditSupplier(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            this.addressValidators = [];
            this.contactValidators = [];
            if (id) {
              this.model = this.mapModel(res.supplierDetails);
              this.onChangeEntity();
              this.dbSupplierEntityIds = this.model.supplierEntityIds;
            }

            if (this.model.addressList == null || this.model.addressList.length == 0)
              this.addAddress();

            if (this.model.supplierContactList == null || this.model.supplierContactList.length == 0) {
              this.addContact();
            }

            if (!id) {
              if (this.data.statusList && this.data.statusList[0]) {
                this.model.status = this.data.statusList[0].id;
              }
            }
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


  mapModel(supplierDetails: any): EditSuplierModel {
    var model: EditSuplierModel = {
      id: supplierDetails.id,
      comment: supplierDetails.comment,
      contactPersonName: supplierDetails.contactPersonName,
      dailyProduction: supplierDetails.dailyProduction,
      fax: supplierDetails.fax,
      glCode: supplierDetails.glCode,
      legalName: supplierDetails.legalName,
      levelId: supplierDetails.levelId,
      mobile: supplierDetails.mobile,
      locLanguageName: supplierDetails.locLanguageName,
      name: supplierDetails.name,
      ownerId: supplierDetails.ownerId,
      totalStaff: supplierDetails.totalStaff,
      typeId: supplierDetails.typeId == null ? 0 : supplierDetails.typeId,
      webSite: supplierDetails.webSite,
      email: supplierDetails.email,
      phone: supplierDetails.phone,
      isNewSupplier: false,
      status: supplierDetails.status,
      vatNo: supplierDetails.vatNo,
      apiServiceIds: supplierDetails.apiServiceIds,
      apiContactServiceList: [],
      isFromBookingPage: false,
      supplierEntityIds: supplierDetails.supplierEntityIds,
      mapAllSupplierContacts: false,
      companyId: supplierDetails.companyId > 0 ? supplierDetails.companyId : this.model.companyId,
      addressList: supplierDetails.addressList.map((x) => {
        var address: Address = {
          id: x.id,
          countryId: x.countryId,
          regionId: x.regionId,
          cityId: x.cityId,
          zipCode: x.zipCode,
          way: x.way,
          addressTypeId: x.addressTypeId == null ? 0 : x.addressTypeId,
          cityList: [],
          regionList: [],
          longitude: x.longitude,
          latitude: x.latitude,
          localLanguage: x.localLanguage,
          countyId: x.countyId == 0 ? null : x.countyId,
          townId: x.townId == 0 ? null : x.townId,
          countyList: [],
          townList: []
        };
        this.addressValidators.push({ address: address, validator: Validator.getValidator(address, "supplier/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

        this.refreshRegions(x.countryId, address);
        this.refreshCities(x.regionId, address);
        this.refreshCounties(x.cityId, address);
        this.refreshTown(x.countyId, address);

        return address;
      }),
      customerList: supplierDetails.customerList.map((y) => {
        var customer: Customer = {
          id: y.id,
          name: y.name,
          code: y.code,
          creditTerm: y.creditTerm,
          isStatisticsVisibility: y.isStatisticsVisibility

        };
        return customer;
      }),

      supplierContactList: supplierDetails.supplierContactList.map((x) => {

        var customerContact: CustomerContact = {
          comment: x.comment,
          contactEmail: x.contactEmail,
          contactId: x.contactId,
          contactName: x.contactName,
          entityServiceList: [],
          contactEntityList: [],
          primaryEntityList: [],
          primaryEntity: x.primaryEntity,
          entityServiceIds: x.entityServiceIds,
          customerList: x.customerList.map((y) => {
            var customer: Customer = {
              id: y.id,
              name: y.name,
              code: y.code,
              creditTerm: y.creditTerm,
              isStatisticsVisibility: y.isStatisticsVisibility
            };
            return customer;
          }),
          fax: x.fax,
          jobTitle: x.jobTitle,
          mobile: x.mobile,
          phone: x.phone,
          contactAPIServiceIds: x.contactAPIServiceIds,
          apiEntityIds: x.apiEntityIds,
          showPopupLoading: false
        };

        // this.onChnageEntityServiceList(customerContact);

        this.contactValidators.push({ customerContact: customerContact, validator: Validator.getValidator(customerContact, "supplier/edit-contact.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
        return customerContact;
      }),
      supplierParentList: supplierDetails.supplierParentList == null ? null : supplierDetails.supplierParentList.map((x) => {

        var tabItem: SupplierSummaryItemModel = {
          id: x.id,
          countryName: x.countryName,
          name: x.name,
          regionName: x.regionName,
          cityName: x.cityName,
          typeId: x.typeId,
          typeName: x.typeName,
          isExpand: false,
          canBeDeleted: x.canBeDeleted,
          list: x.list == null ? [] : x.list.map(y => {
            return {
              id: y.id,
              countryName: y.countryName,
              name: y.name,
              regionName: y.regionName,
              cityName: y.cityName,
              canBeDeleted: y.canBeDeleted
            };
          })
        }

        return tabItem;
      }),
      gradeList: supplierDetails.gradeList == null ? [] : supplierDetails.gradeList.map((x) => {
        var grade: Grade = {
          id: x.id,
          customerName: x.customerName,
          level: x.level,
          customerId: x.customerId == 0 ? null : x.customerId,
          levelId: x.levelId == 0 ? null : x.levelId,
          periodFrom: x.periodFrom,
          periodTo: x.periodTo,
          isDisable: this.calendar.getToday().after(new NgbDate(x.periodTo.year, x.periodTo.month, x.periodTo.day))
        };
        return grade;
      })

    };
    return model;
  }

  refreshRegions(countryId, item: Address) {
    this.getStates(countryId).subscribe(
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
      });
  }

  resetCountry(item: Address) {
    item.regionId = null;
    item.cityId = null;
    item.cityList = null;
    item.countyList = null;
    item.townList = null;
    item.countyId = null;
    item.townId = null;
  }

  resetRegion(item: Address) {
    item.cityId = null;
    item.countyList = null;
    item.townList = null;
    item.countyId = null;
    item.townId = null;
  }

  resetCity(item: Address) {
    item.townList = null;
    item.countyId = null;
    item.townId = null;
  }

  resetCounty(item: Address) {
    item.townId = null;
  }

  refreshCities(stateId, item: Address) {
    this.getCities(stateId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.cityList = res.data;
            //item.cityId = 0;
          }
          else {
            item.cityList = [];
          }
        },
        error => {
          item.cityList = [];
        });
  }

  refreshCounties(cityId, item: Address) {
    this.getCounties(cityId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.countyList = res.dataList;
          }
          else {
            item.countyList = [];
          }
        },
        error => {
          item.countyList = [];
        });
  }

  refreshTown(countyId, item: Address) {
    this.getTowns(countyId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            item.townList = res.dataList;
          }
          else {
            item.townList = [];
          }
        },
        error => {
          item.townList = [];
        });
  }

  getStates(countryId) {
    return this.service.getStates(countryId)
      .pipe()
  }

  getCities(stateId) {
    return this.service.getCities(stateId)
      .pipe();
  }

  getCounties(cityId) {
    return this.service.getCounties(cityId)
      .pipe();
  }

  getTowns(cityId) {
    return this.service.getTowns(cityId)
      .pipe();
  }

  save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.addressValidators) {
      // item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    for (let item of this.contactValidators) {
      //item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    if (this.isFormValid() && this.validateEntityService()) {
      this._saveloader = true;
      //  this.waitingService.open();
      this.model.isFromBookingPage = false;
      if (this.isNew) {
        this.service.getExistingSupplierDetails(this.model).subscribe(res => {
          if (res && res.result == 1) {
            this._saveloader = false;
            this.existSupplierData = res.data;
            var typeData = this.data.typeList.filter(x => x.id == this.model.typeId);
            this.selectedType = typeData[0].name;
            this.confirmExistSuplierModalRef = this.modalService.open(this.confirmExistSupliermodel,
              { windowClass: "lgModelWidth", centered: true });
            this.confirmExistSuplierModalRef.result.then((result) => {
            }, (reason) => {
            });
          } else {
            this.saveSupplier();
          }
        });
      } else {
        this.saveSupplier();
      }
    }
  }
  saveSupplier() {
    if (this.confirmExistSuplierModalRef) {
      this.confirmExistSuplierModalRef.close();
    }
    this.service.saveSupplier(this.model)
      .subscribe(
        res => {

          if (this.confirmSupplierContactEntityMapModalRef) {
            this.confirmSupplierContactEntityMapModalRef.close();
          }
          if (res && res.result == SaveSupplierResult.Success) {
            // this.waitingService.close();
            // this.modalInfo.showTraduce('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.SAVE_OK');
            this.showSuccess('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.SAVE_OK');
            if (this.fromSummary)
              this.return('suppliersearch/supplier-summary');
            else
              this.init();
          }
          else {
            switch (res.result) {
              case SaveSupplierResult.SupplierIsNotSaved:
                this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_CANNOT_ADDSUPPLIER');
                break;
              case SaveSupplierResult.SupplierIsNotFound:
                this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_CURRENTSUPP_NOTFOUND');
                break;
              case SaveSupplierResult.SupplierExists:
                this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_SUPPLIER_EXISTS');
                break;
              case SaveSupplierResult.SupplierCodeExists:
                this.showError('EDIT_SUPPLIER.SAVE_RESULT', res.errorData.errorText);
                break;
            }

            // this.waitingService.close();
          }
          this._saveloader = false;
        },
        error => {
          this._saveloader = false;
          if (error == "Unauthorized")
            this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'ERROR.MSG_MESSAGE_401');
          else
            this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_UNKNONW_ERROR');
          //this.waitingService.close();
        });
  }

  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  removeAddress(index) {
    this.model.addressList.splice(index, 1);
    this.addressValidators.splice(index, 1);
  }

  addAddress() {

    var headOfficeId = this.data.addressTypeList[0].id;
    var address: Address = {
      id: 0,
      countryId: null,
      cityId: null,
      cityList: [],
      regionId: null,
      regionList: [],
      way: '',
      zipCode: '',
      addressTypeId: headOfficeId,
      longitude: null,
      latitude: null,
      localLanguage: '',
      countyId: null,
      townId: null,
      countyList: [],
      townList: []
    };
    if (this.model.addressList.length > 0) address.addressTypeId = 0;
    this.model.addressList.push(address);
    this.addressValidators.push({ address: address, validator: Validator.getValidator(address, "supplier/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }

  addContact() {

    var customerContact: CustomerContact = {
      comment: '',
      contactEmail: '',
      contactId: 0,
      contactName: '',
      fax: '',
      jobTitle: '',
      mobile: '',
      phone: '',
      customerList: [],
      contactAPIServiceIds: [],
      apiEntityIds: [],
      entityServiceIds: [],
      entityServiceList: [],
      primaryEntityList: [],
      contactEntityList: [],
      primaryEntity: null,
      showPopupLoading: false
    };

    if (this.apiServiceList && this.apiServiceList[0] && this.apiServiceList[0].id)
      customerContact.contactAPIServiceIds.push(this.apiServiceList[0].id);

    this.model.supplierContactList.push(customerContact);
    this.contactValidators.push({ customerContact: customerContact, validator: Validator.getValidator(customerContact, "supplier/edit-contact.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
    this.onChangeEntity();
  }

  removeContact(index) {
    this.model.supplierContactList.splice(index, 1);
    this.contactValidators.splice(index, 1);

    if (this.model.supplierContactList && this.model.supplierContactList.length == 0) {
      this.addContact();
    }
  }

  getClearSuppliers() {
    this.suppliersSelected = null;
    this.model.supplierParentList = [];
  }

  newSupplierChange() {
    if (this.model.isNewSupplier) {
      this.getClearSuppliers();
    }
  }

  getSelectSuppliers(): Array<SupplierSummaryItemModel> {
    if (this.model.supplierParentList == null)
      this.model.supplierParentList = [];

    if (this.suppliersSelected != null) {
      for (let item of this.suppliersSelected) {
        if (!this.model.supplierParentList.some(x => x.id == item.id))
          this.model.supplierParentList.push(item);
      }
    }

    return this.model.supplierParentList;
  }

  removeSupplier(index) {
    this.suppliersSelected = [];
    this.model.supplierParentList.splice(index, 1);
  }

  getSuppliersbyCountry(countryId) {
    this.suppliersSelected = [];
    this.getSupplierByCountrySearch(countryId);
  }

  isContactValid(customerContactValidator) {
    customerContactValidator.validator.isSubmitted = true;
    var isok = false;
    isok = customerContactValidator.validator.isValid('contactAPIServiceIds')
      && customerContactValidator.validator.isValid('apiEntityIds')
      && customerContactValidator.validator.isValid('entityServiceIds');

    return isok;
  }

  isFormValid() {
    var isok = false;

    if (this.contactValidators && this.contactValidators.length == 0) {
      this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.LBL__ATLEAST_ONE_CONTACT_MANDATORY');
      return isok;
    }
    isok = this.validator.isValid('name')
      && this.validator.isValid('email')
      && this.validator.isValid('typeId')
      && this.validator.isValid('contactPersonName')
      && this.validator.isValid('apiServiceIds')
      && this.validator.isValid('supplierEntityIds')
      && this.validator.isValid('companyId')
      && this.checkCustomer()
      && this.checkSupplierParent(true)
      && this.checkCustomerContact()
      && this.addressValidators.every((x) =>
        x.validator.isValid('countryId') &&
        x.validator.isValidIf('localLanguage', x.address.countryId == Country.China)
        && x.validator.isValid('regionId')
        && x.validator.isValid('cityId')
        && x.validator.isValid('way')
        && x.validator.isValid('addressTypeId')
      )
      && this.contactValidators.every((x) => x.validator.isValid('contactName')
        && x.validator.isValid('contactEmail')
        && x.validator.isValid('customerList')
        && x.validator.isValid('phone')
        && x.validator.isValid('contactAPIServiceIds')
        && x.validator.isValid('apiEntityIds')
        && x.validator.isValid('primaryEntity')
        && x.validator.isValid('entityServiceIds')
        && this.checkHeadOffice()
        && this.checkAPIServiceIds(x.customerContact)
      );
    if (isok) {
      let result = this.addressValidators.every(x => x.validator.isValidIf('localLanguage', x.address.countryId == Country.China));
      if (!result) {
        this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_LOCALLANG_REQ')
      }
    }
    if (isok) {
      isok = this.addressValidators.every((x) => (x.address.countryId == this._country.China && this.model.typeId == 1) ? (x.validator.isValid('countyId')
        && x.validator.isValid('townId')) : true);

    }
    var number = 0;
    if (isok) {
      this.addressValidators.every((x) => (x.address.addressTypeId == 1) ? ++number : number);
      isok = number > 1 ? false : true;
      if (!isok)
        this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_OFFICETYPE_REQ')
    }
    return isok;
  }

  checkHeadOffice(): boolean {
    if (this.model.typeId != 1 && this.validator.isSubmitted) {
      if (this.model.addressList == null || this.model.addressList.length == 0)
        return false;

      if (this.data && this.data.addressTypeList) {
        var headOfficeId = this.data.addressTypeList[0].id;
        var data = this.model.addressList.filter(x => x.addressTypeId == headOfficeId);
        if (!data.length) this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_HEADOFFICE_REQ');
        return data.length == 1;
      }
    }
    return true;
  }

  checkAPIServiceIds(customerContact): boolean {
    if (this.model.apiServiceIds && customerContact.contactAPIServiceIds) {
      var apiServiceMatch = customerContact.contactAPIServiceIds.every(i => this.model.apiServiceIds.includes(i));
      if (!apiServiceMatch)
        this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_API_SERVICE_ID_SAME');
      return apiServiceMatch;
    }
  }

  addCustomer() {
    if (this.selectedcustomerList != null) {
      for (let item of this.selectedcustomerList) {
        if (!this.model.customerList.some(x => x.id == item.id))
          //this.model.customerList.push(item);
          this.model.customerList = [...this.model.customerList, item];
      }
    }

  }

  removeCustomer(index) {
    let removeCustomer = this.model.customerList[index];
    this.model.gradeList = this.model.gradeList.filter(x => x.customerId != removeCustomer.id);

    this.model.customerList.splice(index, 1);
    this.model.customerList = [...this.model.customerList];

    this.contactValidators.forEach(x => {
      const removeCustomers = x.customerContact.customerList.filter(x => x.id != removeCustomer.id);
      x.customerContact.customerList = [...removeCustomers];
    });
  }

  checkCustomer(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    return this.model.customerList != null && this.model.customerList.length > 0;
  }

  checkSupplierParent(isShow): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.model.typeId != 1)
      return true;

    if (this.model.isNewSupplier)
      return true;

    if (this.model.supplierParentList == null || this.model.supplierParentList.length == 0) {
      if (isShow) {
        this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_SUPPLIERPARENT_REQ');
      }
      return false;
    }

    return true;
  }

  checkCustomerContact(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    if (this.model.supplierContactList == null || this.model.customerList.length == 0)
      return true;

    if (this.model.customerList == null || this.model.customerList.length == 0)
      return false;

    let ids = this.model.customerList.map(x => x.id);

    for (let item of this.model.supplierContactList) {
      if (item.customerList.some(x => ids.indexOf(x.id) < 0))
        return false;
    }


    return true;
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

  getAPIServices() {

    this.referenceService.getAPIServices()
      .pipe()
      .subscribe(
        response => {
          if (response.result == 1) {
            this.apiServiceList = response.dataSourceList;
            if (!this.model.id) {
              this.model.apiServiceIds = [];
              if (this.apiServiceList[0] && this.apiServiceList[0].id)
                this.model.apiServiceIds.push(this.apiServiceList[0].id);
            }

          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  createUserCredentials(showUserPopup: any) {

    this.savePopupLoading = true;
    var userAccount = new UserAccountModel();

    //map the user details
    userAccount.userName = this.customerContact.contactEmail;
    userAccount.fullname = this.customerContact.contactName + ' (' + this.model.name + ')';
    userAccount.contact = this.customerContact.contactId;

    //supplier id
    userAccount.userId = this.model.id;

    //UserType - fact or supplier
    if (this.model.typeId == Number(SupplierType.Supplier)) {
      userAccount.userTypeId = UserType.Supplier;
    }
    else if (this.model.typeId == Number(SupplierType.Factory)) {
      userAccount.userTypeId = UserType.Factory;
    }
    userAccount.apiServiceIds = [];
    userAccount.apiServiceIds = this.customerContact.contactAPIServiceIds;

    this.setUserRoleEntity(userAccount);
    //save user api call
    this.saveUserCredential(userAccount, showUserPopup);

  }

  setUserRoleEntity(userAccount) {


    if (userAccount.apiServiceIds != null && userAccount.apiServiceIds.find(x => x == Number(APIService.Tcf))) {
      if (userAccount.userTypeId == Number(UserType.Supplier)) {
        userAccount.userRoleEntityList.push({ roleId: Number(RoleEnum.TCFSupplier), roleEntity: this.customerContact.apiEntityIds, roleName: Number(RoleEnum.TCFSupplier) });
      }
    }
    if (userAccount.userTypeId == Number(UserType.Supplier)) {
      userAccount.userRoleEntityList.push({ roleId: Number(RoleEnum.Supplier), roleEntity: this.customerContact.apiEntityIds, roleName: Number(RoleEnum.Supplier) });
    }
    else if (userAccount.userTypeId == Number(UserType.Factory)) {
      userAccount.userRoleEntityList.push({ roleId: Number(RoleEnum.Factory), roleEntity: this.customerContact.apiEntityIds, roleName: Number(RoleEnum.Factory) });
    }


  }


  showUserDetailsPopup(createUserPopup: any, showUserPopup: any, customerContactValidator) {
    customerContactValidator.customerContact.showPopupLoading = true;
    this.showPopupLoading = true;
    this.customerContact = customerContactValidator.customerContact;

    //UserType - fact or supplier
    var userTypeId: number;
    if (this.model.typeId == Number(SupplierType.Supplier)) {
      userTypeId = UserType.Supplier;
    }
    else if (this.model.typeId == Number(SupplierType.Factory)) {
      userTypeId = UserType.Factory;
    }
    var contactId = this.customerContact.contactId;

    //get login user details
    this.getUserDetails(createUserPopup, showUserPopup, customerContactValidator, userTypeId, contactId);
  }

  saveUserCredential(userAccount: UserAccountModel, showUserPopup: any) {
    this.userAccountService.saveUser(userAccount)
      .subscribe(
        response => {
          this.modelRef.close();
          if (response && response.result == 1) {
            this.userdetail.userName = response.userName;
            this.userdetail.password = response.password;
            this.modelRef = this.modalService.open(showUserPopup, { windowClass: "smModelWidth", centered: true });
          }
          else {
            switch (response.result) {
              case 2:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOT_SAVE");
                break;
              case 3:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CURRENT_USER_NOTFOUND");
                break;
              case 4:
                this.showError("EDIT_USER_ACCOUNT.TITLE", "EDIT_USER_ACCOUNT.MSG_CANNOTMAPREQUEST");
                break;
            }
          }
          this.savePopupLoading = false;
        }, error => {
          this.savePopupLoading = false;
          this.showError("EDIT_USER_ACCOUNT.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
          this.modelRef.close();
        });
  }

  getUserDetails(createUserPopup: any, showUserPopup: any, customerContactValidator: any, usertypeId: number, contactId: number) {
    this.userAccountService.getUserDetails(contactId, usertypeId)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.userdetail.userName = response.userName;
            this.userdetail.password = response.password;
            this.modelRef = this.modalService.open(showUserPopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
          }
          else if (response.result == 3) {

            if (this.isContactValid(customerContactValidator)) {
              this.modelRef = this.modalService.open(createUserPopup, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
            }
          }
          customerContactValidator.customerContact.showPopupLoading = false;
          this.showPopupLoading = false;
        }, error => {
          console.log(error);
          customerContactValidator.customerContact.showPopupLoading = false;
          this.showPopupLoading = false;
        });
  }

  //fetch the first 10 suppliers for the country  
  getSupplierByCountrySearch(countryID) {
    this.searchSupplierModel.countryIds = [];
    this.searchSupplierModel.countryIds.push(countryID);
    this.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.supplierLoading = true),
      switchMap(term => term
        ? this.service.getSupplierByCountryDataSource(this.searchSupplierModel, term)
        : this.service.getSupplierByCountryDataSource(this.searchSupplierModel)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.supplierLoading = false))
      ))
      .subscribe(data => {
        console.log(data);
        this.suppliersbyCountry = data;
        this.supplierLoading = false;
      });
  }

  //fetch the suppliers data with virtual scroll
  getSupplierByCountryData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.searchSupplierModel.searchText = this.supplierInput.getValue();
      this.searchSupplierModel.skip = this.suppliersbyCountry.length;
    }
    this.supplierLoading = true;
    this.service.getSupplierByCountryDataSource(this.searchSupplierModel).
      subscribe(data => {
        if (data && data.length > 0) {
          this.suppliersbyCountry = this.suppliersbyCountry.concat(data);
        }
        if (isDefaultLoad)
          this.searchSupplierModel = new CommonSupplierSourceRequest();
        this.supplierLoading = false;
      }),
      error => {
        this.supplierLoading = false;
        this.setError(error);
      };
  }
  //get entity list
  getEntityList() {
    this.apiEntityLoading = true;
    this.referenceService.getEntityList()
      .pipe(first())
      .subscribe(
        response => {
          this.apiEntityLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.apiEntityList = response.dataSourceList;
            let entity: string = this.utility.getEntityName();
            this.model.companyId = this.apiEntityList?.find(x => x.name == entity).id;
            //this.onChangeEntity();
          }
          if (!this.model.id) {
            this.model.supplierEntityIds = [];
            var entityId = Number(this.utility.getEntityId());
            this.model.supplierEntityIds.push(entityId);
          }
        },
        error => {
          this.apiEntityLoading = false;
          this.setError(error);
        });
  }


  onClearEntityList(customerContact) {
    customerContact.entityServiceIds = null;
    customerContact.primaryEntity = null;
  }

  onClearServiceList(customerContact) {
    customerContact.entityServiceIds = null;
  }

  onChnageEntityServiceList(customerContact) {
    customerContact.entityServiceList = [];
    customerContact.primaryEntityList = [];
    if (customerContact.contactAPIServiceIds && customerContact.apiEntityIds && customerContact.contactEntityList && this.apiServiceList) {
      let index = 1;

      customerContact.primaryEntityList = [];

      customerContact.primaryEntityList = customerContact.contactEntityList.filter(x => customerContact.apiEntityIds.includes(x.id));
      if (customerContact.primaryEntityList.find(x => x.id == customerContact.primaryEntity) == undefined) {
        customerContact.primaryEntity = null;
      }

      customerContact.contactAPIServiceIds.forEach(service => {
        customerContact.apiEntityIds.forEach(entity => {
          var entityName = customerContact.contactEntityList.find(x => x.id == entity).name;
          var serviceName = this.apiServiceList.find(x => x.id == service).name;
          var entityService = entityName + "(" + serviceName + ")";

          customerContact.entityServiceList.push({ id: index, entityId: entity, serviceId: service, name: entityService });
          index = index + 1;
        })
      })

      customerContact.entityServiceIds = customerContact.entityServiceIds.filter(y => customerContact.apiEntityIds.includes(y.entityId) && customerContact.contactAPIServiceIds.includes(y.serviceId))
    }
  }
  onChangeEntity() {
    // if(this.apiEntityList && this.apiEntityList.length>0)
    // {
    this.contactValidators.forEach(x => {
      x.customerContact.contactEntityList = this.apiEntityList.filter(y => this.model.supplierEntityIds.includes(y.id));

      const customerContactEntityIdList = x.customerContact.contactEntityList.map(y => y.id);
      x.customerContact.apiEntityIds = x.customerContact.apiEntityIds.filter(y => customerContactEntityIdList.includes(y));

      this.onChnageEntityServiceList(x.customerContact);
    })
    //}
  }

  validateEntityService() {
    let result = this.model.supplierContactList.every(x => {
      return x.apiEntityIds.every(y => x.entityServiceIds.map(z => z.entityId).includes(y))
    })

    if (!result) {
      this.showWarning('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_ENTITY_SERVICE_REQUIRED');
    }
    return result;
  }

  confirmSaveSupplier() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.addressValidators) {
      // item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    for (let item of this.contactValidators) {
      //item.validator.initTost();
      item.validator.isSubmitted = true;
    }

    if (!this.isFormValid() || !this.validateEntityService())
      return;
    const newSupplierEntityIds = this.model.supplierEntityIds.filter(x => !this.dbSupplierEntityIds.includes(x));
    const isNewEntityAddedInContacts = this.model.supplierContactList.map(x => x.apiEntityIds).reduce(function (a, b) { return a.concat(b); }, []).some(y => newSupplierEntityIds.includes(y))
    if (newSupplierEntityIds.length > 0 && !isNewEntityAddedInContacts) {
      if (this.contactValidators.length > 0) {
        this.confirmSupplierContactEntityMapModalRef = this.modalService.open(this.confirmEntityMapSupplierContact, {
          windowClass: 'smModelWidth',
          centered: true,
          keyboard: false,
          backdrop: 'static'
        });
      }
      else {
        this.save();
      }
    }
    else {
      this.save();
    }
  }

  saveSupplierContactMapWithNewEntity() {
    this.saveSupplierContactLoading = true;
    this.model.mapAllSupplierContacts = true;
    this.save();
  }

  EntityAccessToSupplier(data) {
    this.isPopupButtonDisaible = true;
    this._saveloader = true;
    data.customerList = this.model.customerList;
    this.service.addEntityIntoSupplier(data)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.isPopupButtonDisaible = false;
            this._saveloader = false;
            this.showSuccess('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.SAVE_ENTITY');
            data.isView = true;
          }
          else {
          }
          this._saveloader = false;
        },
        error => {
          this._saveloader = false;
        });
  }

  SaveExistSupplier() {
    this._saveloader = true;
    this.saveSupplier();
  }

  RedirectToSupplier(supplierid) {
    let entity: string = this.utility.getEntityName();
    var editPage = entity + "/" + Url.SupplierEdit + supplierid;
    window.open(editPage);
  }

  onChangeCustomer(event) {
    this.isGradeLoading = true;
    this.grade.customerName = event.name;
    this.grade.levelId = null;
    this.grade.level = null;
    this.gradeList = [];
    this.service.getSupplierLevelByCustomerId(event.id)
      .subscribe(res => {
        if (res.result === ResponseResult.Success) {
          this.gradeList = res.dataSourceList;
        }
        this.isGradeLoading = false;
      }, erorr => {
        this.isGradeLoading = false;
      })
  }

  addGrade() {
    if (this.isValidGrade()) {
      this.model.gradeList.push(this.grade);
      this.grade = new Grade();
    }

  }

  isValidGrade() {
    if (this.grade.customerId == null || this.grade.customerId == undefined || this.grade.customerId == 0) {
      this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_CUSTOMER_REQ');
      return false;
    }

    if (this.grade.levelId == null || this.grade.levelId == undefined || this.grade.levelId == 0) {
      this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_LEVEL_REQ');
      return false;
    }

    if (this.grade.periodFrom == null || this.grade.periodFrom == undefined || this.grade.periodFrom == '') {
      this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_PERIOD_FROM_REQ');
      return false;
    }

    if (this.grade.periodTo == null || this.grade.periodTo == undefined || this.grade.periodTo == '') {
      this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_PERIOD_TO_REQ');
      return false;
    }

    var currentFromDate = new NgbDate(this.grade.periodFrom.year, this.grade.periodFrom.month, this.grade.periodFrom.day);
    var currentToDate = new NgbDate(this.grade.periodTo.year, this.grade.periodTo.month, this.grade.periodTo.day);

    if (currentFromDate.after(currentToDate)) {
      this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_PERIOD_FROMDATE_GREATER_REQ');
      return false;
    }

    const grades = this.model.gradeList.filter(x => x.customerId == this.grade.customerId);
    for (let index = grades.length; index > 0; index--) {

      var grade = grades[index - 1];

      var fromDate = new NgbDate(grade.periodFrom.year, grade.periodFrom.month, grade.periodFrom.day);
      var toDate = new NgbDate(grade.periodTo.year, grade.periodTo.month, grade.periodTo.day);

      if ((currentToDate.after(fromDate) && toDate.after(currentFromDate))) {
        this.showError('EDIT_SUPPLIER.SAVE_RESULT', 'EDIT_SUPPLIER.MSG_PERIOD_DATE_ALREADY');
        return;
      }
    }


    return true;
  }

  removeGrade() {
    this.modelRef.close(true);
  }

  onChangeLevel(event) {
    this.grade.level = event.name;
  }

  convertDate(date) {
    return date.day + "/" + date.month + "/" + date.year;
  }

  confirmDeleteGrade(content, index) {
    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {
      if (result === true)
        this.model.gradeList.splice(index, 1);
    }, (reason) => {
    });
  }

  clearGraderCustomer() {
    this.grade.levelId = null;
    this.grade.level = null;
    this.gradeList = [];
  }
}


