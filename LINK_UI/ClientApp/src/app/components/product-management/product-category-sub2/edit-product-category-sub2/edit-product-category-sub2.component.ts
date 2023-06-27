import { Component } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { ProductManagementService } from '../../../../_Services/productmanagement/productmanagement.service';
import { ProductCategorySub2Model } from '../../../../_Models/productmanagement/productcategorysub2.model';
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Validator } from "../../../common";
import { ToastrService } from "ngx-toastr";
import { DetailComponent } from '../../../common/detail.component';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-edit-product-category-sub2',
  templateUrl: './edit-product-category-sub2.component.html',
  styleUrls: ['./edit-product-category-sub2.component.scss']
})
export class EditProductCategorySub2Component extends DetailComponent {
  public model: ProductCategorySub2Model;
  public data: any;
  public error: '';
  public productCategoryList: Array<any> = [];
  public productSubCategoryList: Array<any> = [];
  loading = false;
  constructor(
    private service: ProductManagementService,
    route: ActivatedRoute,
    public validator: Validator,
    translate: TranslateService,
    notification: ToastrService,
    public utility: UtilityService,
    router: Router
  ) {
    super(router, route, translate, notification);
  }
  onInit(id?: any): void {
    this.validator.setJSON('productmanagement/add-product-category-sub2.valid.json');
    this.validator.setModelAsync(() => this.model);
    this.initialize(id);
  }
  getViewPath(): string {
    return "productsub2category/view-product-category-sub2"
  }
  getEditPath(): string {
    return "productsub2category/edit-product-category-sub2"
  }
  onChange(id) {
    this.model.productSubCategoryId = null;
    this.refresSubbyCategoryId(id);
  }
  refresSubbyCategoryId(id) {
    if (id != null && id > 0) {
      this.loading = true;
      this.service.getsubbycategoryid(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.productSubCategoryList = result.productSubCategoryList;
            }
            else {
              this.productSubCategoryList = [];
            }
            this.loading = false;
          },
          error => {
            this.productSubCategoryList = [];
            this.loading = false;
          });
    }
    else {
      this.productSubCategoryList = [];
    }
  }

  initialize(id?) {
    this.model = new ProductCategorySub2Model();
    this.model.active = true;
    this.data = {};
    this.validator.isSubmitted = false;
    this.service.getProductCategorySub2ById(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.data = res;
            if (id) {
              this.model = {
                id: res.productCategorySub2.id,
                name: res.productCategorySub2.name,
                active: res.productCategorySub2.active,
                productCategoryId: res.productCategorySub2.productCategory.id == null ? 0 : res.productCategorySub2.productCategory.id,
                productSubCategoryId: res.productCategorySub2.productSubCategory.id == null ? 0 : res.productCategorySub2.productSubCategory.id
              };
              this.productCategoryList = this.data.productCategoryList;
              this.refresSubbyCategoryId(this.model.productCategoryId);
            }
            else {
              this.productCategoryList = this.data.productCategoryList;
            }
          }
          else {
            this.error = res.result;
          }
        },
        error => {
          this.error = error
        }
      );

      
  }
  Formvalid(): boolean {
    return this.validator.isValid('name')
      && this.validator.isValid('productCategoryId')
      && this.validator.isValid('productSubCategoryId');
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.Formvalid()) {
      this.loading = true;
      this.service.saveProductCategorySub2(this.model)
        .subscribe(
          response => {
            if(response && response.result ==1) {
              this.showSuccess("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_SAVE_SUCCESS");
              this.initialize();
              if (this.fromSummary)
                this.return("productsub2category/product-category-sub2-summary");
            }
            else {
              switch (response.result) {
                case 2:
                  this.showError("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_CANNOT_SAVE_CATEGORY");
                  break;
                case 3:
                  this.showError("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_CURRENT_CATEGORY_NOTFOUND");
                  break;
                case 4:
                  this.showError("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_CANNOTMAPREQUEST");
                  break;
                  case 5:
                  this.showError("INTERNAL_PRODUCT.TITLE","INTERNAL_PRODUCT.MSG_DUPLICATE_NAME");
                  break;
              }
            }
            this.loading = false;
          }, error => {
            this.showError("INTERNAL_PRODUCT.TITLE", 'COMMON.MSG_UNKNONW_ERROR');
            this.loading = false;
          });
    }
  }
  Reset() {
    this.model.name = '';
    this.model.productCategoryId = null;
    this.model.productSubCategoryId = null;
  }
}
