import { Component, NgModule, Input, Output, EventEmitter } from '@angular/core';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { TranslateService } from "@ngx-translate/core";
import { CustomerContactService } from '../../../_Services/customer/customercontact.service'
import { CustomerSerConfigSummaryModel, CustomerServiceConfigItemModel, customerServiceConfigToRemove } from '../../../_Models/customer/customerserviceconfig.model';
import { first } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { UserModel } from '../../../_Models/user/user.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Validator } from '../../common'
import { SummaryComponent } from '../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomerServiceConfig } from 'src/app/_Services/customer/customerserviceconfig.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-customerContactSummary',
  templateUrl: './customer-serviceconfiguration.component.html',
  styleUrls: ['./customer-serviceconfiguration.component.css'],
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

export class CustomerServiceConfigurationComponent extends SummaryComponent<CustomerSerConfigSummaryModel> {
  public model: CustomerSerConfigSummaryModel;
  public modelRemove: customerServiceConfigToRemove;
  public modelRef: NgbModalRef;
  public searchIsCustomer = false;
  public isCustomerDetails: boolean = false;
  idCurrentCustomer: number;
  customerList: Array<any> = [];
  serviceList: Array<any> = [];
  public currentUser: UserModel;
  public parentID: any;
  public customerValues: any;
  public isEditDetail:boolean;
  public customerID?:string;
  public currentRoute: Router;
  initialloading: boolean = false;
  Searchloading: boolean = false;
  public fromSummary: boolean = false;
  public paramParent: string;
  isFilterOpen: boolean;
  constructor(public service: CustomerServiceConfig,public customerService: CustomerService, public modalService: NgbModal,
    authService: AuthenticationService,public utility: UtilityService, router: Router, route: ActivatedRoute, public validator: Validator,
    public routeCurrent: ActivatedRoute,public routerCurrent: Router,translate: TranslateService) {
    super(router, validator, route,translate);
    
    this.validator.setJSON("customer/customercontactsummary.valid.json");  
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.model = new CustomerSerConfigSummaryModel();
    this.currentUser = authService.getCurrentUser();
    this.currentRoute = router;
    this.isFilterOpen = true;
  }

  onInit() {
    this.initialloading=true;
    this.customerID = this.routeCurrent.snapshot.paramMap.get("id");   
    this.routeCurrent.queryParams.subscribe(
      params => {  
        if (params != null && params['paramParent'] != null) {
          this.paramParent = params['paramParent'];
          this.fromSummary = true;
        }

      }
    ); 
    if (this.customerID) {
     
      this.service.getEditCustomerServiceConfig({ index: 1, pageSize: 10, customerID: this.customerID })
      .pipe()
      .subscribe(
        response => {
      
          if (response && response.result == 1 && response.customerList && response.customerList.length > 0) {
            this.isEditDetail=true;
           
            this.mapPageProperties(response);
            this.customerList = response.customerList;
            this.serviceList=response.serviceList;
            this.model.customerValue = Number(this.customerID);       
            
            this.model.items = response.data.map((x) => {

              var tabItem: CustomerServiceConfigItemModel = {
                id: x.id,
                service: x.service,
                productCategory: x.productCategory,
                serviceType: x.serviceType,
                customerName: x.customerName,
                samplingMethod:x.samplingMethod,
                isExpand: false,
                list: x.list == null ? [] : x.list.map(y => {
                  return {
                    id: y.id,
                    service: y.service,
                    productCategory: y.productCategory,
                    serviceType: y.serviceType,
                    customerName: y.customerName,
                    samplingMethod:y.samplingMethod
                  };
                })
              }

              return tabItem;
            });   
            this.initialloading=false;

          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this.initialloading=false;
          }
          else {
            this.error = response.result;
            this.initialloading=false;
            // TODO check error from result
          }

        },
        error => {
          this.error = error;
          this.initialloading=false;
        });
    }
    else {
    
      this.service.getCustomerServiceConfigSummary()
        .pipe()
        .subscribe(
          response => {
          
            if (response && response.result == 1) {
              this.data = response;
              if(this.data.isEdit==true){
                this.isEditDetail=true;
              }
              else{
                this.isEditDetail=false;
              }
              this.customerList = response.customerList;
              this.serviceList=response.serviceList;
              this.initialloading=false;
            }
            else {
              this.error = response.result;
              this.initialloading=false;
            }

          },
          error => {
            this.setError(error);
            this.initialloading=false;
        });

    }
   
  }

  getPathDetails(): string {
    return this.isEditDetail ? "cusserviceedit/edit-customerserviceconfig" : "cusserviceedit/view-customerserviceconfig";
  }

  NavigateDetailPage(id) {
    this.validator.isSubmitted = true;
    
    if (this.formValid()) {
      var data = Object.keys(this.model);
      var currentItem: any = {};
  
      for (let item of data) {
        if (item != 'noFound' && item != 'noFound' && item != 'items' && item != 'totalCount')
          currentItem[item] = this.model[item];
      }
      this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getPathDetails()}/${id}/${this.model.customerValue}`], {queryParams:{ paramParent: encodeURI(JSON.stringify(currentItem)) }});
    }
  }

  getData() {
    this.Searchloading=true;
    this.service.getCustomerServiceConfig({ index: this.model.index, pageSize: this.model.pageSize, customerValue: this.model.customerValue,serviceValue:this.model.serviceValue})
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {
           
            this.validator.isSubmitted = true;
            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;
            this.isEditDetail=true;
            
          
            this.model.items = response.data.map((x) => {

              var tabItem: CustomerServiceConfigItemModel = {
                id: x.id,
                service: x.service,
                productCategory: x.productCategory,
                serviceType: x.serviceType,
                customerName: x.customerName,
                samplingMethod:x.samplingMethod,
                isExpand: false,
                list: x.list == null ? [] : x.list.map(y => {
                  return {
                    id: y.id,
                    service: y.service,
                    productCategory: y.productCategory,
                    serviceType: y.serviceType,
                    customerName: y.customerName,
                    samplingMethod:y.samplingMethod
                  };
                })
              }

              if (x.typeId != 1)
                this.searchIsCustomer = true;

              return tabItem;
            });
            this.Searchloading=false;

          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
            this.Searchloading=false;
          }
          else {
            this.error = response.result;
            this.Searchloading=false;
          }

        },
        error => {
          this.error = error;
          this.Searchloading=false;
        });
  } 

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  openConfirm(id, name, content) {

    this.modelRemove = {
      id: id,
      name: name
    };

    this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
      // this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

  }

  deleteCustomerService(item: customerServiceConfigToRemove) {
   
    this.service.deleteCustomerServiceConfig(item.id)
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
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();

  }

  

  getDetail() {  

    this.validator.isSubmitted=true;

    if (this.formValid()) {
      let entity: string = this.utility.getEntityName();
      let Path="cusserviceedit/edit-customer-contact";
      let id= this.model.customerValue+'|add';
      this.routerCurrent.navigate([`/${entity}/${Path}/${id}`]);
    }
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

  formValid(): boolean {
    return this.validator.isValid('customerValue');
  }

  returnToParent(){
    let path="cussearch/customer-summary";
    let entity: string = this.utility.getEntityName();
    this.routerCurrent.navigate([`/${entity}/${path}`], {queryParams:{ param: this.paramParent }});
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }

}
