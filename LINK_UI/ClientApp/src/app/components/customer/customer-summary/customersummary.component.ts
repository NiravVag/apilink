import { Component, NgModule } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { CustomerService } from '../../../_Services/customer/customer.service'
import { CustomerSummaryModel, CustomerSummaryItemModel, customerToRemove, CustomerSearchModel, CustomerMasterData } from '../../../_Models/customer/customersummary.model'
import { first, retry } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customerSummary',
  templateUrl: './customersummary.component.html',
  styleUrls: ['./customersummary.component.css'],
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

export class CustomerSummaryComponent extends SummaryComponent<CustomerSummaryModel> {

  public data: any;
  loading = true;
  error = '';
  public model: CustomerSummaryModel;
  public modelRemove: customerToRemove;
  public modelRef: NgbModalRef;
  public searchIsCustomer = false;
  public isCustomerDetails: boolean = false;
  public isCustomerContactDetails: boolean = false;
  public editPage: string;
  public viewPage: string;
  idCurrentCustomer: number;
  customerList: Array<any> = [];
  customergroupList: Array<any> = [];
  public currentUser: UserModel;
  Initialloading: boolean = false;
  searchloading: boolean = false;
  isFilterOpen: boolean;
  customerMasterData:CustomerMasterData;
  constructor(public service: CustomerService, public modalService: NgbModal,
    authService: AuthenticationService, router: Router, route: ActivatedRoute, public validator: Validator,
    public routerCurrent: Router, translate: TranslateService, public utility: UtilityService) {
    super(router, validator, route, translate);
    this.validator.setJSON("customer/customer-summary.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;

    this.model = new CustomerSummaryModel();
    this.customerMasterData=new CustomerMasterData();
    this.customerMasterData.entityId=parseInt(this.utility.getEntityId());

    this.currentUser = authService.getCurrentUser();
    this.isFilterOpen = true;
  }

  onInit() {
    this.Initialloading = true;
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

          this.Initialloading = false;

        },
        error => {
          this.setError(error);
          this.Initialloading = false;
        });
    this.getCustomerbyGroup(0);
  }
  getCustomerbyGroup(id) {
    this.Initialloading = true;
    this.data = this.service.getCustomerbyId(id)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
            this.customerList = data.CustomerList;
          }
          else {
            this.error = data.result;
          }

          this.Initialloading = false;

        },
        error => {
          this.setError(error);
          this.Initialloading = false;
        });
  }

  getPathDetails(): string {
    return this.data.isEdit ? this.editPage : this.viewPage;
  }

  getData() {
    this.searchloading = true;
    debugger;
    this.service.getDataSummary({ index: this.model.index, pageSize: this.model.pageSize, customerData: this.model.customerData })
      .pipe()
      .subscribe(
        response => {
          if (response) {

            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;
            if (response.data != null) {
              this.model.items = response.data.map((x) => {

                var tabItem: CustomerSummaryItemModel = {
                  id: x.id,
                  name: x.name,
                  group: x.groupName,
                  isExpand: false,
                  list: x.list == null ? [] : x.list.map(y => {
                    return {
                      id: y.id,
                      name: y.name,
                      group: y.groupName

                    };
                  })
                }
                if (x.typeId != 1)
                  this.searchIsCustomer = true;

                return tabItem;
              });
              this.searchloading = false;
            }
            else if (response && response.data == null) {
              this.model.noFound = true;
              this.searchloading = false;
            }
            else if (response && response.result == 2) {
              this.model.noFound = true;
              this.searchloading = false;
            }
            else {
              this.error = response.result;
              this.searchloading = false;
              // TODO check error from result
            }
          }

        },
        error => {
          this.error = error;
          this.searchloading = false;
        });
  }


  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  clearCustomer() {
    this.model.customerData.customerId = 0;
  }
  clearCustomerGroup() {
     this.model.customerData.groupId = 0;  
     this.getCustomerbyGroup(0);
    // this.model.customerValues=null;
    // this.data.customerList=[];
  }

  selectCustomer() {
    if (this.model.customerValues != null) {
      this.model.customerData.customerId = this.model.customerValues["id"];
    }
  }

  selectCustomerGroup() {
    this.model.customerValues = [];
    this.model.customerData.groupId = 0;
    this.model.customerData.customerId = 0;
    this.getCustomerbyGroup(this.model.groupValues["id"]);
    if (this.model.groupValues != null) {
      this.model.customerData.groupId = this.model.groupValues["id"];
    }
  }

  selectEAQF() {
    this.model.customerData.isEAQF = this.model.isEAQF;
  }

  openConfirm(id, name, content) {

    this.modelRemove = {
      id: id,
      name: name
    };

    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }


  getDetail(id) {
    this.isCustomerDetails = true;
    this.idCurrentCustomer = id;
  }

  addSupp() {
    this.isCustomerDetails = true;
    this.idCurrentCustomer = null;
  }

  expand(id) {
    let item = this.model.items.filter(x => x.id == id)[0];

    if (item != null)
      item.isExpand = true;
  }

  collapse(id) {
    let item = this.model.items.filter(x => x.id == id)[0];

    if (item != null)
      item.isExpand = false;
  }

  getEditDetails(id, type) {

    if (type == 0) {
      this.editPage = "cusedit/edit-customer";
      this.viewPage = "cusedit/view-customer";
      this.getDetails(id);
    }
    else if (type == 1) {
      this.editPage = "cuscontactsearch/customer-contact-summary";
      this.viewPage = "cuscontactsearch/customer-contact-summary";
      this.getDetails(id);
    }
    else if (type == 2) {
      this.editPage = "cusbrand/edit-customer-brand";
      this.viewPage = "cusbrand/edit-customer-brand";
      this.getDetails(id);
    }
    else if (type == 3) {
      this.editPage = "cuscontactsearch/customer-contact-summary";
      this.viewPage = "cuscontactsearch/customer-contact-summary";
      this.getDetails(id);
    }
    else if (type == 4) {
      this.editPage = "cusdep/customer-department";
      this.viewPage = "cusdep/customer-department";
      this.getDetails(id);
    }
    else if (type == 5) {
      this.editPage = "cusservicesearch/customer-serviceconfig";
      this.viewPage = "cusservicesearch/customer-serviceconfig";
      this.getDetails(id);
    }
    else if (type == 6) {
      this.editPage = "cusbuyer/customer-buyer";
      this.viewPage = "cusbuyer/customer-buyer";
      this.getDetails(id);
    }
    else if (type == 7) {
      this.editPage = "cuscheckpointedit/edit-customercheckpoint";
      this.viewPage = "cuscheckpointedit/edit-customercheckpoint";
      this.getDetails(id);
    }
    else if (type == 8) {
      this.editPage = "cuscollection/edit-customer-collection";
      this.viewPage = "cuscollection/edit-customer-collection";
      this.getDetails(id);
    }
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
}
