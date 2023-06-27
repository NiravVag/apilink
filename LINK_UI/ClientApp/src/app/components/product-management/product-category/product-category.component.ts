import { Component } from '@angular/core';
import { OnInit } from "@angular/core";
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ProductCategorySummaryModel, ProductCategoryModel } from '../../../_Models/productmanagement/productcategory.model'
import { ProductManagementService } from '../../../_Services/productmanagement/productmanagement.service'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from "ngx-toastr";
import { Validator } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
 
@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.scss'] 
})

export class ProductCategoryComponent implements OnInit
{
  public error = '';
  private modelRef: NgbModalRef;
  public modelRemove: ProductCategoryModel;
  public modelAdd: ProductCategoryModel;
  public loading: boolean = true;
  public isNewItem: boolean = true;
  public data: any;
  public modelsummary: ProductCategorySummaryModel;
  saveloading:boolean;
  public businessLineList: Array<any> = [];
  businessLineData: any;
  
  constructor(private service: ProductManagementService, private modalService: NgbModal,
    private router: Router, public validator: Validator,private translate: TranslateService,
    private toastr: ToastrService, public refService: ReferenceService) 
  {
      this.modelsummary = new ProductCategorySummaryModel();
      this.modelAdd = new ProductCategoryModel();
      this.validator.setJSON("productmanagement/add-product-category.valid.json");
      this.validator.setModelAsync(() => this.modelAdd);
      this.validator.isSubmitted = false;
     
  }

  ngOnInit() {
    this.Intitialize();
    this.getBusinessLines();
}

public Intitialize(){
  this.loading = true;
  this.isNewItem = true;
  this.getData();
}

  getData() {
    this.service.getProductCategorySummary()
    .pipe()
    .subscribe(
      response => {
        if (response && response.result == 1) {
          this.data = response;
          this.modelsummary.items = response.productCategoryList;
        }
        else {
          this.error = response.result;
        }
        this.loading = false;
      },
      error => {
        this.setError(error);
        this.loading = false;
      });
  }

refresh(){
    this.modelAdd = new ProductCategoryModel;
    this.Intitialize();
}

  Formvalid(): boolean {
    return this.validator.isValid('name')
    && this.validator.isValid('businessLineId');
  }

  openEdit(item) {
    this.modelAdd = {
      id: item.id,
      name: item.name,
      businessLineId: item.businessLineId,
      businessLine: item.businessLine      
    }
    this.isNewItem = false;
  }

  openConfirm(item,content) {
      this.modelRemove = {
        id: item.id,
        name: item.name,
        businessLineId: item.businessLineId,
        businessLine: item.businessLine
      };
      this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });
  }
  
  deleteProductCategory(item: ProductCategoryModel){
    this.service.deleteProductCategoryById(item.id)
    .pipe()
    .subscribe(
      response => {
        if (response && (response.result == 1)) {
          // refresh
          this.refresh();
        }
        else {
          switch (response.result) {
            case 2:
              this.showError("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CURRENT_CATEGORY_NOTFOUND");
              break;
            case 3:
              this.showError("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CANNOT_DELETE");
              break;
          }
        }
        this.loading = false;
      },
      error => {
        this.error = error;
        this.loading = false;
      });

  this.modelRef.close();

  }
  save() {
    this.validator.isSubmitted = true;

    if(this.Formvalid()) {
      this.saveloading = true;
      this.modelAdd.active = true;
        this.service.saveProductCategory(this.modelAdd)
        .subscribe(
          response => {
            if(response && response.result ==1) {
              this.showSuccess("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_SAVE_SUCCESS");
              this.validator.isSubmitted = false;
              this.refresh();
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CANNOT_SAVE_CATEGORY");
                  break;
                case 3:
                  this.showError("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CURRENT_CATEGORY_NOTFOUND");
                  break;
                case 4:
                  this.showError("PRODUCT_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CANNOTMAPREQUEST");
                  break;
                  case 5:
                  this.showError("PRODUCT_CATEGORY.TITLE","PRODUCT_CATEGORY.MSG_DUPLICATE_NAME");
                  break;
              }
            }
            this.saveloading = false;
          }, error => {
            this.showError("PRODUCT_CATEGORY.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
            this.saveloading = false;
          });
      }
  }

  public showSuccess(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";
  
    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });
  
    this.toastr.success( tradMessage,tradTitle);
  }
  
  public showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";
  
    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });
  
    this.toastr.error( tradMessage,tradTitle);
  }
  public showWarning(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";
  
    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });
  
    this.toastr.warning( tradMessage,tradTitle);
  }
  setError(error) {
  
    this.error = error;
  
    if (error == "Unauthorized")
      this.router.navigate(['/error/401']);
    else if (error == "NotFound")
      this.router.navigate(['/error/404']);
    else
      this.router.navigate([`/error/${error}`]);
      
    this.loading = false;
  }
  getBusinessLines() {
    this.refService.getBusinessLines()
      .subscribe(
        response => {
          this.businessLineData = response.dataSourceList;
        },
        error => {
          this.setError(error);
        }
      );
  }
 
}
