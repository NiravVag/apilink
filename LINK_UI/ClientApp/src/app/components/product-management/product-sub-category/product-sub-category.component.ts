import { Component } from '@angular/core';
import { OnInit } from "@angular/core";
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ProductSubCategorySummaryModel, ProductSubCategoryModel} from '../../../_Models/productmanagement/productsubcategory.model'
import { ProductManagementService } from '../../../_Services/productmanagement/productmanagement.service'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from "ngx-toastr";
import { Validator } from '../../common'
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-product-sub-category',
  templateUrl: './product-sub-category.component.html',
  styleUrls: ['./product-sub-category.component.scss']
})

export class ProductSubCategoryComponent implements OnInit
 {
  public error = '';
  private modelRef: NgbModalRef;
  public modelRemove: ProductSubCategoryModel;
  public modelAdd: ProductSubCategoryModel;
  public productCategoryList: Array<any> = [];
  public loading: boolean = true;
  public isNewItem: boolean = true;
  public data: any;
  public modelsummary: ProductSubCategorySummaryModel;
  saveloading:boolean;
  constructor(private service: ProductManagementService, private modalService: NgbModal,
    private router: Router, public validator: Validator,private translate: TranslateService,
    private toastr: ToastrService) 
  {
    this.modelsummary = new ProductSubCategorySummaryModel();
    this.modelAdd = new ProductSubCategoryModel();
    this.validator.setJSON("productmanagement/add-product-subcategory.valid.json");
    this.validator.setModelAsync(() => this.modelAdd);
    this.validator.isSubmitted = false;
  }

  ngOnInit() {
    this.Intitialize();
}

public Intitialize(){
  this.loading = true;
  this.isNewItem = true;
  this.getData();
}

  getData(id?) {
    this.service.getProductSubCategorySummary(id)
    .pipe()
    .subscribe(
      response => {
        if (response && response.result == 1) {
          this.data = response;
          this.productCategoryList = response.productCategoryList;
          this.modelsummary.items = response.productSubCategoryList;
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

  Formvalid(): boolean {
    return this.validator.isValid('name')
    && this.validator.isValid('productCategoryId');
  }

  refresh(){
    this.modelAdd = new ProductSubCategoryModel;
    this.Intitialize();
}
onChange(id) {
  this.loading = true;
  this.getData(id);
}
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if(this.Formvalid()) {
      this.saveloading = true;
      this.modelAdd.active = true;
      this.service.saveProductSubCategory(this.modelAdd)
        .subscribe(
          response => {
            if(response && response.result ==1) {
              this.showSuccess("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_SUB_CATEGORY.MSG_SAVE_SUCCESS");
              this.validator.isSubmitted = false;
              this.refresh();
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_CATEGORY.MSG_CANNOT_SAVE_CATEGORY");
                  break;
                case 3:
                  this.showError("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_SUB_CATEGORY.MSG_CURRENT_CATEGORY_NOTFOUND");
                  break;
                case 4:
                  this.showError("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_SUB_CATEGORY.MSG_CANNOTMAPREQUEST");
                  break;
                  case 5:
                  this.showError("PRODUCT_SUB_CATEGORY.TITLE","PRODUCT_SUB_CATEGORY.MSG_DUPLICATE_NAME");
                  break;
              }
            }
            this.saveloading = false;
          }, error => {
            this.showError("PRODUCT_SUB_CATEGORY.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
            this.saveloading = false;
          });
    }
  }
  openConfirm(item,content) {
    this.modelRemove = {
      id: item.id,
      name: item.name
    };
    this.modelRef = this.modalService.open(content, { windowClass : "smModelWidth", centered: true });
}
  delete(item: ProductSubCategoryModel){
    this.service.deleteProductSubCategoryById(item.id)
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
              this.showError("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_SUB_CATEGORY.MSG_CURRENT_CATEGORY_NOTFOUND");
              break;
            case 3:
              this.showError("PRODUCT_SUB_CATEGORY.TITLE", "PRODUCT_SUB_CATEGORY.MSG_CANNOT_DELETE");
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

  openEdit(item) {
    this.modelAdd = {
      id: item.id,
      name: item.name,
      productCategoryId: item.productCategory.id
    }
    this.isNewItem = false;
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
}

