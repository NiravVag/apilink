import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from "@angular/router";
import { catchError, debounceTime, distinctUntilChanged, first, retry, switchMap, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { CustomerService } from '../../../_Services/customer/customer.service'
import { CustomerSummaryItemModel } from '../../../_Models/customer/cutomersummary.model';
import { EditCustomerModel, CustomerAddress } from '../../../_Models/customer/edit-customer.model';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { LocationService } from '../../../_Services/location/location.service';
import { ReferenceService } from '../../../_Services/reference/reference.service';
import { APIService, ListSize, RoleEnum } from "src/app/components/common/static-data-common";
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryThumbnailsComponent } from 'ngx-gallery-9';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionPickingService } from 'src/app/_Services/booking/inspectionpicking.service';
import { CommonDataSourceRequest, DataSource, ResponseResult } from 'src/app/_Models/common/common.model';
import { CustomerContactService } from 'src/app/_Services/customer/customercontact.service';
import { BehaviorSubject, of } from 'rxjs';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-editCustomer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent extends DetailComponent {
  public model: EditCustomerModel;
  public data: any;

  error: '';
  public customersbyGroup: Array<CustomerSummaryItemModel>;
  public customerSelected: Array<CustomerSummaryItemModel>;
  public selectCounryId: number = 0;
  public isDetails: boolean = true;
  public jsonHelper: JsonHelper;
  public addressValidators: Array<any> = [];
  public contactValidators: Array<any> = [];
  public selectedcustomerList: Array<any>;
  public customergroupList: Array<any>;
  public languageList: Array<any>;
  public prospectstatusList: Array<any>;
  public marketsegmentList: Array<any>;
  public businesstypeList: Array<any>;
  public invoicetypeList: Array<any>;
  public addresstypeList: Array<any>;
  public countryList: Array<any>;
  public cityList: Array<any>;
  public customerCategory: Array<any>;
  public complexityLevel: Array<any>;
  public skills: Array<any>;
  public kamList: Array<any>;
  public accountingleaderList: Array<any>;
  public salesInchargeList: Array<any>;
  public activitiesLevelList: Array<any>;
  public relationshipStatusList: Array<any>;
  public brandPriorityList: Array<any>;
  public apiServiceList: any;
  public entityList: Array<any>;
  apiServiceEnum = APIService;
  country: any;
  city: any;
  initialloading: boolean = false;
  saveloading: boolean = false;
  dbCustomerEntityIdList: number[] = [];
  public customerEntityModel: any;
  public mapCustomerContactEntityModalRef: NgbModalRef;
  public deleteCustomerEntityModalRef: NgbModalRef;
  public confirmCustomerContactEntityMapModalRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  @ViewChild('mapCustomerContactEntity') mapCustomerContactEntityModal;
  @ViewChild('deleteCustomerEntity') deleteCustomerEntityModal;
  @ViewChild('confirmEntityMapCustomerContact') confirmEntityMapCustomerContact;
  customerContacts: Array<any>;
  customerContactLoading: boolean = false;
  selectedAllCSSummary: boolean;
  requestCustomerModel: CommonDataSourceRequest;
  customerList: Array<DataSource>;
  customerInput: BehaviorSubject<string>;
  customerLoading: boolean;
  isItTeamRole: boolean;
  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public service: CustomerService,
    public locationservice: LocationService,
    public utility: UtilityService,
    private authService: AuthenticationService,
    public referenceService: ReferenceService,
    public customerContactService: CustomerContactService,
    public modalService: NgbModal) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/edit-customer.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;

    this._translate = translate;
    this._toastr = toastr;

    this.customerInput = new BehaviorSubject<string>("");
    this.requestCustomerModel = new CommonDataSourceRequest();

    const user = authService.getCurrentUser();
    this.isItTeamRole = user.roles.some(x => x.id === RoleEnum.IT_Team);
    // router.events.subscribe(data => {
    //   if (data instanceof NavigationStart) {
    //     let id = route.snapshot.paramMap.get("id");
    //     this.onInit(id);
    //   }
    // });
  }

  onInit(id?: any) {
    this.initialloading = true;
    this.getCountryList();
    this.getCustomerGroup();
    this.getLanguage();
    this.getProspectStatus();
    this.getMarketSegment();
    this.getBusinessType();
    this.getInvoiceType();
    this.getAddressType();
    this.getStaffKAMList();
    this.GetAccountingLeader();
    this.getSalesInchargeList();
    this.getActivitiesLevelList();
    this.getRelationshipStatusList();
    this.getBrandPriorityList();
    this.getAPIServices();
    this.getEntityList(id);
    this.init(id);


  }



  getCustomerGroup() {
    this.data = this.service.getCustomerGroup()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.customergroupList = data.customerGroup;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });
  }
  getLanguage() {
    this.data = this.service.getLanguage()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.languageList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  getProspectStatus() {
    this.data = this.service.getProspectStatus()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.prospectstatusList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  getMarketSegment() {
    this.data = this.service.getMarketSegment()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.marketsegmentList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  getBusinessType() {
    this.data = this.service.getBusinessType()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.businesstypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  getInvoiceType() {
    this.data = this.service.getInvoiceType()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.invoicetypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }


  GetAccountingLeader() {
    this.data = this.service.getAccountingLeader()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.accountingleaderList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getSalesInchargeList() {
    this.data = this.service.getSalesIncharge("Sales")
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.salesInchargeList = data.dataSourceList;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }
  getStaffKAMList() {
    this.data = this.service.getStaffKAMList()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.kamList = data.dataSourceList;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }

  getActivitiesLevelList() {
    this.data = this.service.getActivitiesLevel()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.activitiesLevelList = data.customerSource;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }

  getRelationshipStatusList() {
    this.data = this.service.getRelationshipStatus()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.relationshipStatusList = data.customerSource;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }

  getBrandPriorityList() {
    this.data = this.service.getBrandPriority()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.brandPriorityList = data.customerSource;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }


  getAddressType() {
    this.data = this.service.getAddressType()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.addresstypeList = data.customerSource;
          }
          else {
            this.error = data.result;
          }

          this.loading = false;

        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }
  getCountryList() {
    this.locationservice.getCountrySummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.countryList = data.countryList;
            // If Country Api dealy then set the bussiness country Item 
            if (this.model && this.model.id > 0 && this.model.businessCountry && this.model.businessCountry.length > 0) {
              this.model.businessCountryItems = this.countryList.filter(x => this.model.businessCountry.indexOf(x.id) >= 0).map(x => { return { id: x.id, countryName: x.countryName } })
            }
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          this.setError(error);
        });

  }
  getCity(item) {
    if (item.customerAddress.countryId) {
      this.data = this.locationservice.getcitybycountryid(item.customerAddress.countryId)
        .pipe()
        .subscribe(
          data => {
            if (data && data.result == 1) {
              item.customerAddress.cityList = data.data;
            }
            else {
              item.customerAddress.cityList = [];
              this.error = data.result;
            }

            this.loading = false;

          },
          error => {
            this.setError(error);
            this.loading = false;
          });
    }
  }
  getViewPath(): string {
    return "cusedit/view-customer";
  }

  getEditPath(): string {
    return "cusedit/edit-customer";
  }


  ngOnChanges(changes: SimpleChanges) {
    const id: SimpleChange = changes.currentId;
    this.init(id.currentValue);
  }

  isFactory(): boolean {
    return this.model != null && this.model.id == 1;
  }

  init(id?) {

    this.customerCategory =
      [{ name: 'A', id: 1 },
      { name: 'B', id: 2 },
      { name: 'C', id: 3 }
      ];
    this.complexityLevel =
      [{ name: '1', id: 1 },
      { name: '2', id: 2 },
      { name: '3', id: 3 },
      { name: '4', id: 4 }
      ];
    this.skills =
      [{ name: 'Basic', id: 1 },
      { name: 'Advanced', id: 2 }
      ];
    this.model = new EditCustomerModel();
    this.data = {};
    this.validator.isSubmitted = false;

    //this.waitingService.open();
    if (id == null) {
      this.model.apiServiceIds = [];
      this.model.apiServiceIds.push(this.apiServiceEnum.Inspection);

      this.model.customerEntityIds = [];
      var entityId = Number(this.utility.getEntityId());
      this.model.customerEntityIds.push(entityId);

      id = 0;
    }
    this.service.getEditCustomer(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;

            this.addressValidators = [];
            this.contactValidators = [];
            if (id) {
              this.model = this.mapModel(res.customerDetails);
            }

            if (this.model.customerAddresses == null || this.model.customerAddresses.length == 0)
              this.addAddress();

          }
          else if (res.customerDetails == null && res.result == 2) {
            this.addressValidators = [];
            this.contactValidators = [];
            this.addAddress();
          }
          else {
            this.error = res.result;

          }
          if (this.isItTeamRole)
            this.getCustomerListBySearch();
          this.initialloading = false;
        },
        error => {
          this.setError(error);
          this.initialloading = false;
        });
  }

  mapModel(customerDetails: any): EditCustomerModel {
    var model: EditCustomerModel = {
      id: customerDetails.id,
      group: customerDetails.group,
      name: customerDetails.name,
      code: customerDetails.code,
      email: customerDetails.email,
      fax: customerDetails.fax,
      phone: customerDetails.phone,
      complexityLevel: customerDetails.complexityLevel,
      startDate: customerDetails.startDate,
      website: customerDetails.website,
      others: customerDetails.others,
      prospectStatus: customerDetails.prospectStatus,
      skillsRequired: customerDetails.skillsRequired,
      kam: customerDetails.kam,
      kamItems: this.getkamValues(customerDetails.kam),
      category: customerDetails.category,
      margetSegment: customerDetails.margetSegment,
      businessCountry: customerDetails.businessCountry,
      businessCountryItems: this.getbusinessCountryValues(customerDetails.businessCountry),
      otherPhone: customerDetails.otherPhone,
      language: customerDetails.language,
      businessType: customerDetails.businessType,
      quatationName: customerDetails.quatationName,
      icRequired: customerDetails.icRequired,
      glCode: customerDetails.glCode,
      comments: customerDetails.comments,
      bookingDefaultComments: customerDetails.bookingDefaultComments,
      accountingLeader: customerDetails.accountingLeader,
      salesIncharge: customerDetails.salesIncharge,
      salesInchargeItems: this.getsalesInchargeValues(customerDetails.salesIncharge),
      activitiesLevel: customerDetails.activitiesLevel,
      relationshipStatus: customerDetails.relationshipStatus,
      brandPriority: customerDetails.brandPriority,
      brandPriorityItems: this.getbrandPriorityValues(customerDetails.brandPriority),
      directCompetitor: customerDetails.directCompetitor,
      invoiceType: customerDetails.invoiceType,
      apiServiceIds: customerDetails.apiServiceIds,
      customerEntityIds: customerDetails.customerEntityIds,
      mapCustomerContactEntityIds: [],

      sisterCompanyIds: customerDetails.sisterCompanyIds,
      companyId: customerDetails.companyId,
      customerAddresses: customerDetails.customerAddresses.map((x) => {
        var address: CustomerAddress = {
          id: x.id,
          countryId: x.countryId,
          cityId: x.cityId,
          zipCode: x.zipCode,
          addressType: x.addressType,
          boxPost: x.boxPost,
          address: x.address,
          cityList: null,
        };

        this.addressValidators.push({ customerAddress: address, validator: Validator.getValidator(address, "supplier/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

        this.refreshCities(x.countryId, address);
        return address;
      })
    };

    this.dbCustomerEntityIdList = model.customerEntityIds;
    return model;
  }


  refreshCities(countryId, item: CustomerAddress) {
    this.getCities(countryId)
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



  getCities(stateId) {
    return this.locationservice.getcitybycountryid(stateId)
      .pipe();
    // return null;
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.addressValidators)
      item.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.saveloading = true;
      //this.waitingService.open();
      this.model.kam = this.getValues(this.model.kamItems);
      this.model.businessCountry = this.getValues(this.model.businessCountryItems);
      this.model.salesIncharge = this.getValues(this.model.salesInchargeItems);
      this.model.brandPriority = this.getValues(this.model.brandPriorityItems);
      this.service.saveCustomer(this.model)
        .subscribe(
          res => {

            if (res && res.result == 1) {
              //this.waitingService.close();
              this.showSuccess('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.SAVE_OK');
              this.saveloading = false;
              if (this.confirmCustomerContactEntityMapModalRef)
                this.confirmCustomerContactEntityMapModalRef.close()
              if (this.deleteCustomerEntityModalRef)
                this.deleteCustomerEntityModalRef.close()
              if (this.mapCustomerContactEntityModalRef)
                this.mapCustomerContactEntityModalRef.close()
              if (this.fromSummary)
                this.return('cussearch/customer-summary');
              else
                this.init();
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CANNOT_ADDCUSTOMER');
                  break;
                case 3:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CURRENTSUPP_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CUSTOMER_EXISTS');
                  break;
                case 6:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_DUPLICATE_CUSTOMER_EXISTS');
                  break;
                case 7:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_DUPLICATE_GLCODE_EXISTS');
                  break;
                case 8:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_DUPLICATE_EMAIL_EXISTS');
                  break;
                case 11:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_CUSTOMER_CONTACT_ENTITY_ASSIGNED');
                  break;
                case 12:
                  this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_SAME_SISTERCOMPANY_NOTADDED');
                  break;
              }

              //this.waitingService.close();
            }
            this.saveloading = false;
          },
          error => {
            this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_UNKNONW_ERROR');
            this.saveloading = false;
            //this.waitingService.close();
          });
    }
  }


  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  removeAddress(index) {
    this.model.customerAddresses.splice(index, 1);
    this.addressValidators.splice(index, 1);
  }

  addAddress() {

    var address: CustomerAddress = {
      id: 0,
      countryId: null,
      cityId: null,
      cityList: [],
      zipCode: '',
      addressType: null,
      boxPost: '',
      address: ''
    };

    this.model.customerAddresses.push(address);
    this.addressValidators.push({ customerAddress: address, validator: Validator.getValidator(address, "supplier/edit-address.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }

  removeCustomer(index) {
    this.customerSelected = [];
    //this.model.customerParentList.splice(index, 1);
  }

  isFormValid() {
    return this.validator.isValid('name')
      && this.validator.isValid('startDate')
      && this.validator.isValid('invoiceType')
      && this.validator.isValid('businessCountryItems')
      && this.validator.isValid('glCode')
      && this.validator.isValid('apiServiceIds')
      && this.validator.isValid('entityIds')
      && this.validator.isValid('companyId')
      && this.addressValidators.every((x) =>
        x.validator.isValid('addressType')
        && x.validator.isValid('address')
        && x.validator.isValid('countryId')
        && x.validator.isValid('cityId'))
      && this.checkHeadOffice()
  }

  getbusinessCountryValues(values) {
    if (this.countryList == null || values == null)
      return [];
    return this.countryList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, countryName: x.countryName } });
  }

  getkamValues(values) {
    if (this.kamList == null || values == null)
      return [];
    return this.kamList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, name: x.name } });
  }

  getsalesInchargeValues(values) {
    if (this.salesInchargeList == null || values == null)
      return [];
    return this.salesInchargeList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, name: x.name } });
  }

  getbrandPriorityValues(values) {
    if (this.brandPriorityList == null || values == null)
      return [];
    return this.brandPriorityList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, name: x.name } });
  }





  clearCity(item) {
    item.customerAddress.cityList = null;
    item.customerAddress.cityId = null;

  }

  getEntityList(id) {
    this.referenceService.getEntityList()
      .pipe()
      .subscribe(
        response => {
          if (response.result == ResponseResult.Success) {
            this.entityList = response.dataSourceList;
            let entity: string = this.utility.getEntityName();
            if (!id || id == 0) {
              this.model.companyId = this.entityList?.find(x => x.name == entity).id;
            }
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }


  getAPIServices() {

    this.referenceService.getAPIServices()
      .pipe()
      .subscribe(
        response => {
          if (response.result == 1)
            this.apiServiceList = response.dataSourceList;
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  selectAll() {
    this.customerContacts.forEach(x => {
      x.selected = this.selectedAllCSSummary;
    });

    this.model.mapCustomerContactEntityIds = [];
    if (this.selectedAllCSSummary)
      this.model.mapCustomerContactEntityIds = this.customerContacts.filter(x => x.selected === true).map(x => x.id);
    else
      this.model.mapCustomerContactEntityIds = [];
  }

  checkIfAllSelected() {
    this.selectedAllCSSummary = this.customerContacts.every(function (item: any) {
      return item.selected == true;
    });
    this.model.mapCustomerContactEntityIds = this.customerContacts.filter(x => x.selected === true).map(x => x.id);
  }

  cancel() {
    this.mapCustomerContactEntityModalRef.dismiss();
  }

  //user select any contact with new entity map
  saveMapContact() {
    this.validator.isSubmitted = true;
    if (this.isFormValid() && this.validator.isValid('mapCustomerContactEntityIds')) {
      this.save();
    }

  }

  getNewCustomerEntityIds(): Array<number> {
    return this.model.customerEntityIds.filter(x => !this.dbCustomerEntityIdList.includes(x));
  }

  getDeletedCustomerEntityIds(): Array<number> {
    return this.dbCustomerEntityIdList.filter(x => !this.model.customerEntityIds.includes(x));;
  }

  private openConfirmCustomerContactEntityMapModal() {
    this.confirmCustomerContactEntityMapModalRef = this.modalService.open(this.confirmEntityMapCustomerContact, {
      windowClass: 'smModelWidth',
      centered: true,
      backdrop: 'static',
      keyboard: false
    });
  }

  //for open the map contact
  openContact() {
    const newCustomerEntityIds = this.getNewCustomerEntityIds();
    this.validator.isSubmitted = false;

    if (this.confirmCustomerContactEntityMapModalRef)
      this.confirmCustomerContactEntityMapModalRef.close();

    this.mapCustomerContactEntityModalRef = this.modalService.open(this.mapCustomerContactEntityModal, {
      windowClass: "lgModelWidth",
      centered: true,
      backdrop: 'static',
      keyboard: false
    });
    this.mapCustomerContactEntityModalRef.result.then(
      (result) => { },
      (reason) => { }
    );

    var newCustomerEntities = this.entityList.filter(x => newCustomerEntityIds.includes(x.id));
    this.customerEntityModel = {
      entitys: newCustomerEntities.map(x => x.name).join(", ")
    }
  }

  confirmSaveCustomer() {
    if (this.model.id && this.model.id > 0) {
      //when customer update
      this.validator.initTost();
      this.validator.isSubmitted = true;

      for (let item of this.addressValidators)
        item.validator.isSubmitted = true;

      if (this.isFormValid()) {
        var newCustomerEntityIds = this.getNewCustomerEntityIds();
        var deletedCustomerEntityIds = this.getDeletedCustomerEntityIds();


        if (newCustomerEntityIds.length > 0 && deletedCustomerEntityIds.length > 0) {
          this.showError('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_NOT_REMOVE_ENTITY_DELETE_AND_UPDATE');
          return;
        }

        if (newCustomerEntityIds.length > 0) {
          this.customerContactLoading = true;
          this.customerContactService.getCustomerContactByCustomerId(this.model.id).subscribe(res => {
            this.customerContactLoading = false;
            if (res.result == ResponseResult.Success) {
              this.customerContacts = res.customerContacts;
              this.openConfirmCustomerContactEntityMapModal();
            }
            else if (res.result === ResponseResult.NoDataFound) {
              this.save();
            }


          });

        }

        if (deletedCustomerEntityIds.length > 0) {
          this.openDeleteCustomerEntityModal(deletedCustomerEntityIds);
        }

        if (newCustomerEntityIds.length === 0 && deletedCustomerEntityIds.length === 0)
          this.save();
      }
    }
    else {
      // this is for the new customer added
      this.save();
    }

  }

  private openDeleteCustomerEntityModal(deletedCustomerEntityIds): boolean {
    var deletedCustomerEntities = this.entityList.filter(x => deletedCustomerEntityIds.includes(x.id));
    this.customerEntityModel = {
      entitys: deletedCustomerEntities.map(x => x.name).join(", ")
    }
    this.deleteCustomerEntityModalRef = this.modalService.open(this.deleteCustomerEntityModal, {
      windowClass: "smModelWidth",
      centered: true,
      backdrop: 'static',
      keyboard: false
    });
    this.deleteCustomerEntityModalRef.result.then(
      (result) => {

      },
      (reason) => { }
    );

    return true;

  }

  clearCustomer() {
    this.requestCustomerModel = new CommonDataSourceRequest();
    this.getCustomerListBySearch();
  }

  getCustomerListBySearch() {
    if (this.model.sisterCompanyIds)
      this.requestCustomerModel.idList = this.model.sisterCompanyIds;
    this.customerInput
      .pipe(
        debounceTime(200),
        distinctUntilChanged(),
        tap(() => (this.customerLoading = true)),
        switchMap((term) =>
          this.service
            .getCustomerDataSourceList(this.requestCustomerModel, term)
            .pipe(
              catchError(() => of([])), // empty list on error
              tap(() => (this.customerLoading = false))
            )
        )
      )
      .subscribe((data) => {
         if (this.model.id && this.model.id > 0) {
           data = data.filter(x => x.id != this.model.id);
         }
        this.customerList = data;

        this.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData(IsVirtual: boolean) {
    if (IsVirtual) {
      this.requestCustomerModel.searchText =
        this.customerInput.getValue();
      this.requestCustomerModel.skip = this.customerList.length;
    }

    this.customerLoading = true;
    this.service
      .getCustomerDataSourceList(this.requestCustomerModel)
      .subscribe((customerData) => {
        if (IsVirtual) {
          this.requestCustomerModel.skip = 0;
          this.requestCustomerModel.take = ListSize;
          if (customerData && customerData.length > 0) {
             if (this.model.id && this.model.id > 0) {
               customerData = customerData.filter(x => x.id != this.model.id);
             }
            this.customerList =
              this.customerList.concat(customerData);
          }
        }
        this.customerLoading = false;
      }),
      (error) => {
        this.customerLoading = false;
        this.setError(error);
      };
  }

  checkHeadOffice(): boolean {
    var isOk: boolean = true;
    if (this.model.customerAddresses == null || this.model.customerAddresses.length == 0)
      isOk = false;
    else if (this.addresstypeList && this.addresstypeList.length > 0) {
      var headOfficeId = this.addresstypeList[0].id;
      var data = this.model.customerAddresses.filter(x => x.addressType == headOfficeId);
      if (!data.length) {
        this.showWarning('EDIT_CUSTOMER.SAVE_RESULT', 'EDIT_CUSTOMER.MSG_HEADOFFICE_REQ');
        isOk = false;
      }
      else {
        isOk = true;
      }
    }
    return isOk;
  }
}



