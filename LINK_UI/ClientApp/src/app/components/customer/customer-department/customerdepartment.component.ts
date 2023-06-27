import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper  } from '../../common'
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { CustomerDepartmentmodel, departmentmodel, customerDepartmentToRemove } from 'src/app/_Models/customer/customerdepartment.model';
import { CustomerDepartmentService } from 'src/app/_Services/customer/customerdepartment.service';
import { TranslateService } from '@ngx-translate/core';
import { DetailComponent } from '../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { SaveCustomerDepartmentResult } from '../../common/static-data-common';


@Component({
  selector: 'app-customerContactSummary',
  templateUrl: './customerdepartment.component.html',
  styleUrls: ['./customerdepartment.component.css']
})

export class CustomerDepartmentComponent extends DetailComponent  {

  public model: CustomerDepartmentmodel;
  public modelRemove: customerDepartmentToRemove;
  customerList: Array<any> = [];
  public parentID: any;
  public customerValues: any;
  public isEditDetail:boolean=false;
  public customerID?:string;
  public departmentValidators: Array<any> = [];
  public jsonHelper: JsonHelper;
  public modelRef: NgbModalRef;

  public data: any;
  error = '';
  public isDetails: boolean = false;
  public currentId: any;
  public isEdit:boolean=false;
  public isReadyForSubmit:boolean=false;
  public isNoReturn:boolean=false;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  Initialloading: boolean = false;
  Saveloading: boolean = false;

  constructor(
    translate: TranslateService,
    toastr: ToastrService,
    public validator: Validator,
    route: ActivatedRoute,
    router: Router,
    public service: CustomerDepartmentService,
    public customerService: CustomerService,
    public utility: UtilityService,
    public modalService: NgbModal) {
    super(router, route, translate, toastr);
    this.validator.isSubmitted = false;
    this.validator.setJSON("customer/customer-department.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
    this.model = new CustomerDepartmentmodel();

    this._translate = translate;
    this._toastr = toastr;
  }

  getViewPath(): string {
    return "cusdep/customer-department";
  }

  getEditPath(): string {
    return "cusdep/customer-department";
  }

  onInit(id?: any) {
    this.init(id);
  }

  init(id?)
  {
    this.Initialloading=true;
    if(id)
    {
      this.service.getCustomerDepartment(id)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.departmentValidators = [];
              this.data = response;
              this.customerList = response.customerList;
              this.isReadyForSubmit=true;
              this.model={
                customerValue:parseInt(id),
                departmentList: response.customerDepartmentList.map((x) => {

                  var department: departmentmodel = {
                    id: x.id,
                    name: x.name,
                    code: x.code
                  };
                  this.departmentValidators.push({ department: department, validator: Validator.getValidator(department, "customer/customer-department.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
                  return department;
                }),
              }
              if(this.model.departmentList==null || this.model.departmentList.length==0)
                this.addDepartmentRow();
              this.Initialloading=false;
            }
            else {
              this.error = response.result;
              this.Initialloading=false;
            }


          },
          error => {
            //this.setError(error);
            this.Initialloading=false;
          });
    }
    else{
      this.customerService.getCustomerSummary()
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1) {
              this.data = response;
              this.isReadyForSubmit=false;
              this.isNoReturn=true;
              this.customerList = response.customerList;
              this.Initialloading=false;
            }
            else {
              this.error = response.result;
              this.Initialloading=false;
            }

          },
          error => {
            //this.setError(error);
            this.Initialloading=false;
          });
    }
  }

  selectDepartment() {
    if(this.model.customerValue!=null){
      this.isReadyForSubmit=true;
      this.init(this.model.customerValue);
    }

  }

  isFormValid() {
    return this.validator.isValid('customerValue')
      && this.departmentValidators.every((x) => x.validator.isValid('name'))
      && this.departmentValidators.length>0
  }

  addDepartmentRow() {
    let department: departmentmodel = { id: 0, name: "", code: "" };
    this.model.departmentList.push(department);
    this.departmentValidators.push({ department: department, validator: Validator.getValidator(department, "customer/customer-department.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }

  save(){
    this.validator.initTost();
    this.validator.isSubmitted = true;
    for (let item of this.departmentValidators)
      item.validator.isSubmitted = true;

    const duplicates: string[] = [];
    this.model.departmentList.forEach((c) => {
      if (this.model.departmentList.filter(x => x.name.toUpperCase() == c.name.toUpperCase()).length > 1) {
        if (duplicates.filter(x => x.toUpperCase() == c.name.toUpperCase()).length == 0) {
          duplicates.push(c.name)
        }
      }
    });

    if (duplicates != null && duplicates.length > 0) {
      let tradMessage: string;
      this._translate.get('CUSTOMER_DEPARTMENT.MSG_CUSTOMERDEPARTMENT_EXISTS').subscribe((text: string) => { tradMessage = text });
      tradMessage = tradMessage.replace("{0}", duplicates.toString());
      this.showError('CUSTOMER_DEPARTMENT.SAVE_RESULT', tradMessage);
    }

    if (this.isFormValid() && duplicates.length == 0) {
      this.Saveloading = true;
      this.service.saveDepartment(this.model)
        .subscribe(res=>{
          if (res && res.result == SaveCustomerDepartmentResult.Success) {
            this.showSuccess('CUSTOMER_DEPARTMENT.TITLE', 'CUSTOMER_DEPARTMENT.MSG_SAVE_SUCCESS');
            this.Saveloading=false;
            this.init(this.model.customerValue);
            //this.isReadyForSubmit=false;
          }
          else {
            this.Saveloading = false;
            switch (res.result) {
              case SaveCustomerDepartmentResult.CustomerDepartmentExists:
                let tradMessage: string = "";
                this._translate.get('CUSTOMER_DEPARTMENT.MSG_CUSTOMERDEPARTMENT_EXISTS').subscribe((text: string) => { tradMessage = text });
                tradMessage = tradMessage.replace("{0}", res.errorData.errorText);
                this.showError('CUSTOMER_DEPARTMENT.SAVE_RESULT', tradMessage);
                break;
            }
          }

        })


    }
  }

  openConfirm(id, name, content,index) {
    var departmentID=this.model.departmentList[index].id;
    var departmentName=this.model.departmentList[index].name;
    if(departmentID!=0){

      this.modelRemove = {
        id: departmentID,
        name: departmentName
      };

      this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });

      this.modelRef.result.then((result) => {
        // this.closeResult = `Closed with: ${result}`;
      }, (reason) => {
        //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });

    }
    else{
      this.model.departmentList.splice(index, 1);
      this.departmentValidators.splice(index, 1);
    }



  }


  deleteCustomer(item: customerDepartmentToRemove) {
    //this.openConfirm(iteminfo.id,iteminfo.name,content)
    this.service.deleteCustomerDepartment(item.id)
      .pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            var customerID=this.model.customerValue;
            this.init(customerID);
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
}
