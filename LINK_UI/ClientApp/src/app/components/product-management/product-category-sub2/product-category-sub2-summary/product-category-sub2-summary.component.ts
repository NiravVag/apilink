import { Component } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { ProductManagementService } from '../../../../_Services/productmanagement/productmanagement.service'
import { first } from 'rxjs/operators';
import { Validator } from '../../../common/validator'
import { PageSizeCommon } from '../../../common/static-data-common'
import { DeleteProductManagementResult, ProductCategorySub2SummaryModel } from '../../../../_Models/productmanagement/productcategorysub2.model'
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-product-category-sub2-summary',
  templateUrl: './product-category-sub2-summary.component.html',
  styleUrls: ['./product-category-sub2-summary.component.scss'],
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

export class ProductCategorySub2SummaryComponent extends SummaryComponent<ProductCategorySub2SummaryModel> {

  public model: ProductCategorySub2SummaryModel;
  public data: any = [];
  public productCategoryList: Array<any> = [];
  public productSubCategoryList: Array<any> = [];
  public productCategorySub2List: Array<any> = [];
  public searchloading: boolean = false;
  public isCategoryId: boolean = false;
  error = '';
  loading = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  isFilterOpen: boolean;
  modelRemove: any;
  private modelRef: NgbModalRef;
  
  constructor(
    private service: ProductManagementService,
    public validator: Validator,public toastr: ToastrService,
    public utility: UtilityService,private modalService: NgbModal,
    router: Router, route: ActivatedRoute,translate: TranslateService) {
    super(router, validator, route,translate, toastr);
    this.model = new ProductCategorySub2SummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.validator.setJSON("productmanagement/product-category-sub2.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.Intitialize();
  }

  Intitialize() {
    this.loading = true;
    this.data = this.service.getProductCategorySub2Summary()
      .pipe()
      .subscribe(
        resultdata => {
          if (resultdata && resultdata.result == 1) {
            this.data = resultdata;
            this.productCategoryList = resultdata.productCategoryList;
          }
          else {
            this.error = resultdata.result;
          }
          this.loading = false;
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }
  getData(): void {
    if(this.model.productCategoryId){
      this.isCategoryId = true;
    }
    this.searchloading = true;
    this.service.getProductCategorySub2SearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data;
          }
          else if (response && response.result == 3) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
          }
          this.searchloading = false;
        },
        error => {
          this.error = error;
          this.searchloading = false;
        });
        if(this.model.productCategoryId) {
          this.refresSubbyCategoryId(this.model.productCategoryId);
        }
        if(this.model.productSubCategoryId) {
          this.refresTypebySubCategoryId(this.model.productSubCategoryId);
        }
  }

  SearchDetails() {
    this.validator.initTost();
    this.model.pageSize = this.selectedPageSize;
    this.search();
  }

  onChange(id) {
    if(!id){
      this.isCategoryId = false;
    }
    else {
      this.isCategoryId = true;
      this.model.productCategorySub2Name = null;
    }
    this.model.productSubCategoryId = null;
    this.model.productCategorySub2Values = null;
    this.refresSubbyCategoryId(id);
  }

  onChangeSub(id) {
    this.model.productCategorySub2Values = null;
    this.refresTypebySubCategoryId(id);
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

  refresTypebySubCategoryId(id) {
    if (id != null && id > 0) {
      this.loading = true;
      this.service.gettypebysubcategoryid(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.productCategorySub2List = result.productCategorySub2List;
            }
            else {
              this.productCategorySub2List = [];
            }
            this.loading = false;
          },
          error => {
            this.productCategorySub2List = [];
            this.loading = false;
          });
    }
    else {
      this.productCategorySub2List = [];
    }
  }
  getPathDetails(): string {
    return "productsub2category/edit-product-category-sub2";
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }

  openConfirm(item,deleteProductSubCategory2) {
    this.modelRemove = {
      id: item.id,
      name: item.name
    };
    this.modelRef = this.modalService.open(deleteProductSubCategory2, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
  }

  cancelProductSubCategory2(){
    this.modelRemove = null;
    this.modelRef.close();
  }

  clickdeleteProductSubCategory2(){
    this.service.deleteProductCategorySub2ById(this.modelRemove.id)
    .pipe()
    .subscribe(
      response => {
        if (response && (response.result == DeleteProductManagementResult.Success)) {
          this.refresh();
          this.showSuccess("INTERNAL_PRODUCT.TITLE", 'COMMON.MSG_DELETE_SUCCESS');
        }
        else {
          switch (response.result) {
            case DeleteProductManagementResult.NotFound:
              this.showError("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_CURRENT_CATEGORY_NOTFOUND");
              break;
            case DeleteProductManagementResult.CannotDelete:
              this.showError("INTERNAL_PRODUCT.TITLE", "INTERNAL_PRODUCT.MSG_CANNOT_DELETE");
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
}
