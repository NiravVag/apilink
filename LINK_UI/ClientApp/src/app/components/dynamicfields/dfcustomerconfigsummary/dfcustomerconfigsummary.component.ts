import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { SummaryComponent } from '../../common/summary.component';
import { first } from 'rxjs/operators';
import { DFCustomerConfigsummarymodel, DFCustomerConfigItem, DFCustomerConfigMaster,DFCustomerConfigToRemove } from '../../../_Models/dynamicfields/dfcustomerconfigsummary.model';
import { UserType } from '../../common/static-data-common'
import { UserModel } from '../../../_Models/user/user.model'
import { Router, ActivatedRoute } from '@angular/router';
import { Validator } from "../../common/validator"
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { DynamicFieldService } from '../../../_Services/dynamicfields/dynamicfield.service'
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { animate, state, style, transition, trigger } from '@angular/animations';
@Component({
    selector: 'app-dfcustomerconfigsummary',
    templateUrl: './dfcustomerconfigsummary.component.html',
    styleUrls: ['./dfcustomerconfigsummary.component.scss'],
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

export class DFCustomerConfigSummaryComponent extends SummaryComponent<DFCustomerConfigsummarymodel> {

    Initialloading: boolean = false;
    dfCustomerConfigMaster: DFCustomerConfigMaster;
    currentUser: UserModel;
    _IsInternalUser: boolean = false;
    saveloading: boolean = false;
    initialloading: boolean = false;
    searchloading: boolean = false;
    private currentRoute: Router;
    public modelRef: NgbModalRef;
    modelRemove:DFCustomerConfigToRemove;
    isFilterOpen: boolean;
    getData(): void {
        this.GetSearchData();
    }
    getPathDetails(): string {

        return "dfcustomerconfig/editdfcustomerconfig";
    }

    constructor(private customerService: CustomerService, public dynamicFieldService: DynamicFieldService, public validator: Validator, router: Router,
        route: ActivatedRoute, authserve: AuthenticationService,public modalService: NgbModal,
        public pathroute: ActivatedRoute, public utility: UtilityService,translate: TranslateService) {
        super(router, validator, route,translate);
        this.validator.setJSON("dynamicfields/dfcustomerconfigsummary.valid.json");
        this.validator.setModelAsync(() => this.model);
        this.validator.isSubmitted = false;
        this.currentUser = authserve.getCurrentUser();
        this._IsInternalUser = this.currentUser.usertype == UserType.InternalUser ? true : false;
        this.currentRoute = router;
        this.isFilterOpen = true;
    }

    onInit() {
        this.Initialize();
    }

    isFormValid(): boolean {
        return (this.validator.isValid('moduleId') && this.validator.isValid('customerDataList'));
    }

    Initialize() {
        this.model = new DFCustomerConfigsummarymodel();
        this.dfCustomerConfigMaster = new DFCustomerConfigMaster();
        this.dfCustomerConfigMaster.customerList = [];
        this.dfCustomerConfigMaster.moduleList = [];
        this.dfCustomerConfigMaster.controlTypeList = [];
        this.validator.isSubmitted = false;

        this.model.pageSize = 10;
        this.model.index = 0;
        this.data = [];
        this.getCustomerList();
        this.getControlTypesList();
        this.getModuleList();

    }


    getCustomerList() {

        this.customerService.getCustomerSummary()
            .pipe()
            .subscribe(
                data => {

                    if (data && data.result == 1) {
                        this.dfCustomerConfigMaster.customerList = data.customerList;
                    }
                    else {
                        this.error = data.result;
                    }

                },
                error => {
                    //this.setError(error);

                });
    }

    getModuleList() {
        this.dfCustomerConfigMaster.moduleList = [];
        this.dynamicFieldService.getModules()
            .pipe()
            .subscribe(
                data => {

                    if (data && data.result == 1) {
                        this.dfCustomerConfigMaster.moduleList = data.moduleList;
                    }
                    else {
                        this.error = data.result;
                    }

                },
                error => {
                    //this.setError(error);

                });
    }

    getControlTypesList() {
        this.dfCustomerConfigMaster.controlTypeList = [];
        this.dynamicFieldService.getControlTypes()
            .pipe()
            .subscribe(
                data => {

                    if (data && data.result == 1) {
                        this.dfCustomerConfigMaster.controlTypeList = data.controlTypeList;
                    }
                    else {
                        this.error = data.result;
                    }

                },
                error => {
                    //this.setError(error);

                });
    }

    GetSearchData() {
        //this.searchloading = true;
        this.validator.initTost();
        this.validator.isSubmitted=true;
        if (this.isFormValid()) {
            this.dynamicFieldService.searchDFCustomerConfigSummary(this.model)
                .subscribe(
                    response => {
                        if (response && response.result == 1) {
                            this.mapPageProperties(response);


                            this.model.items = response.data.map((x) => {

                                var item: DFCustomerConfigItem = {
                                    id: x.id,
                                    customerName: x.customerName,
                                    moduleName: x.moduleName,
                                    controlName: x.controlTypeName,
                                    label: x.label,
                                    displayOrder: x.displayOrder,
                                    isBooking:x.isBooking
                                }
                                return item;
                            }
                            );
                        }
                        else if (response && response.result == 2) {
                            this.model.noFound = true;
                        }
                        else {
                            this.error = response.result;
                        }
                    },
                    error => {
                        this.setError(error);
                    });
        }

    }

    getcustomerName(customerId): string {
        var customer = this.dfCustomerConfigMaster.customerList.filter(x => x.id == customerId);
        if (customer)
            return customer[0].name;
        return "";
    }

    getmoduleName(moduleId): string {
        var moduleData = this.dfCustomerConfigMaster.moduleList.filter(x => x.id == moduleId);
        if (moduleData)
            return moduleData[0].name;
        return "";
    }

    getcontrolTypeName(controlTypeId): string {
        var controlTypeData = this.dfCustomerConfigMaster.controlTypeList.filter(x => x.id == controlTypeId);
        if (controlTypeData)
            return controlTypeData[0].name;
        return "";
    }

    getEditDetails(id, newId) {
        this.getDetails(0);
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

      deleteDFCustomerConfiguration(id: number) { 
        this.dynamicFieldService.deleteDFCustomerConfiguration(id)
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
      toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}
