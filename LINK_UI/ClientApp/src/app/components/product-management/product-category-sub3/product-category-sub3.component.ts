import { first } from '@amcharts/amcharts4/.internal/core/utils/Array';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ProductSubCategory2SourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { EditProductCategorySub3MasterModel, EditProductCategorySub3Model, ProductCategorySub3MasterModel, ProductCategorySub3SummaryModel } from 'src/app/_Models/productmanagement/productcategorysub3.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-product-category-sub3',
  templateUrl: './product-category-sub3.component.html',
  styleUrls: ['./product-category-sub3.component.scss'],
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
export class ProductCategorySub3Component extends SummaryComponent<ProductCategorySub3SummaryModel> {

  public componentDestroyed$: Subject<boolean> = new Subject();
  public model: ProductCategorySub3SummaryModel;
  public masterModel: ProductCategorySub3MasterModel;
  public editMasterModel: EditProductCategorySub3MasterModel;
  public editModel: EditProductCategorySub3Model;
  public isCategoryId: boolean = false;
  public modelRef: NgbModalRef;
  public hasITRole = false;
  public pagesizeitems = PageSizeCommon;
  public selectedPageSize;
  public isFilterOpen: boolean;
  public idToDelete: number;
  public popUpOpen: boolean;
  public deleteLoading: boolean;
  public isEdit: boolean;

  constructor(
    private service: ProductManagementService,
    public validator: Validator,
    public utility: UtilityService, public toastr: ToastrService,
    router: Router, route: ActivatedRoute, translate: TranslateService, public cusProdservice: CustomerProduct, public modalService: NgbModal) {
    super(router, validator, route, translate, toastr);
    this.model = new ProductCategorySub3SummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.validator.setJSON("productmanagement/add-product-category-sub3.valid.json");
    this.validator.setModelAsync(() => this.editModel);
    this.Intitialize();
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  Intitialize() {

    this.masterModel = new ProductCategorySub3MasterModel();
    this.getProductCategoryList();
    this.getProductSubCategoryList();
    this.getProductSubCategory2ListBySearch();
  }

  getData(): void {
    if (this.model.productCategorySub2Values) {
      this.isCategoryId = true;
    }
    this.masterModel.searchloading = true;
    this.service.getProductCategorySub3SearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.hasITRole = response.hasITRole;
            this.model.noFound = false;
          }
          else {
            this.error = response.result;
            this.model.noFound = true;
            this.model.items = [];
          }
          this.masterModel.searchloading = false;
        },
        error => {
          this.error = error;
          this.masterModel.searchloading = false;
        });
    if (this.model.productCategoryId) {
      this.refresSubbyCategoryId(this.model.productCategoryId);
    }
    if (this.model.productSubCategoryId) {
      this.getProductSubCategory2ListBySearch();
    }
  }

  SearchDetails() {
    this.validator.initTost();
    this.model.pageSize = this.selectedPageSize;
    this.getData();
  }

  onChange(id) {
    if (!id) {
      this.getProductSubCategoryList();
      this.getProductSubCategory2ListBySearch();
    }
    else {
      this.model.productCategorySub3Name = null;
    }
    this.model.productSubCategoryId = null;
    this.model.productCategorySub2Values = null;
    this.refresSubbyCategoryId(id);
  }

  onChangeSub(id) {
    this.model.productCategorySub2Values = null;
    this.getProductSubCategory2ListBySearch();
  }

  onChangeSub2(item) {
    if (item) {
      this.isCategoryId = true;
    }
    else {
      this.isCategoryId = false;
    }
    this.model.productCategorySub3Name = null;
  }

  refresSubbyCategoryId(id) {
    if (id != null && id > 0) {
      this.loading = true;
      this.service.getsubbycategoryid(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.masterModel.productSubCategoryList = result.productSubCategoryList;
            }
            else {
              this.masterModel.productSubCategoryList = [];
            }
            this.loading = false;
          },
          error => {
            this.masterModel.productSubCategoryList = [];
            this.loading = false;
          });
    }
    else {
      this.masterModel.productSubCategoryList = [];
    }
  }


  getPathDetails(): string {
    return "";
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getProductCategoryList() {
    this.masterModel.productCategoryLoading = true;
    this.service.getProductCategoryList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result = ResponseResult.Success) {
          this.masterModel.productCategoryList = res.dataSourceList;
        }
        this.masterModel.productCategoryLoading = false;
      }),
      error => {
        this.masterModel.productCategoryList = [];
        this.masterModel.productCategoryLoading = false;
      }
  }

  getProductSubCategoryList() {
    this.masterModel.productSubCategoryLoading = true;
    this.service.getProductSubCategoryList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.masterModel.productSubCategoryList = res.dataSourceList;
        }
        this.masterModel.productSubCategoryLoading = false;
      }),
      error => {
        this.masterModel.productSubCategoryList = [];
        this.masterModel.productSubCategoryLoading = false;
      }
  }

  getEditProductSubCategoryList() {
    this.editMasterModel.productSubCategoryLoading = true;
    this.service.getProductSubCategoryList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.editMasterModel.productSubCategoryList = res.dataSourceList;
        }
        this.editMasterModel.productSubCategoryLoading = false;
      }),
      error => {
        this.editMasterModel.productSubCategoryList = [];
        this.editMasterModel.productSubCategoryLoading = false;
      }
  }

  //virtual scroll product category sub 2
  getProductSubCategory2ListBySearch() {
    this.masterModel.productSubCategory2Request.productSubCategoryIds = [];
    if (this.model.productSubCategoryId)
      this.masterModel.productSubCategory2Request.productSubCategoryIds.push(this.model.productSubCategoryId);
    this.masterModel.productSubCategory2Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.productCategorySub2Loading = true),
      switchMap(term => term
        ? this.cusProdservice.getProductSubCategory2DataSource(this.masterModel.productSubCategory2Request, term)
        : this.cusProdservice.getProductSubCategory2DataSource(this.masterModel.productSubCategory2Request)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.productCategorySub2Loading = false))
      ))
      .subscribe(data => {
        this.masterModel.productSubCategory2List = data;
        this.masterModel.productCategorySub2Loading = false;
      });
  }

  //fetch the product sub category 2 data with virtual scroll
  getProductSubCategory2Data(isDefaultLoad: boolean) {

    if (this.model.productSubCategoryId)
      this.masterModel.productSubCategory2Request.productSubCategoryIds.push(this.model.productSubCategoryId);

    if (isDefaultLoad) {
      this.masterModel.productSubCategory2Request.searchText = this.masterModel.productSubCategory2Input.getValue();
      this.masterModel.productSubCategory2Request.skip = this.masterModel.productSubCategory2List.length;
    }
    this.masterModel.productCategorySub2Loading = true;
    this.cusProdservice.getProductSubCategory2DataSource(this.masterModel.productSubCategory2Request).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.productSubCategory2List = this.masterModel.productSubCategory2List.concat(data);
        }
        if (isDefaultLoad)
          this.masterModel.productSubCategory2Request = new ProductSubCategory2SourceRequest();
        this.masterModel.productCategorySub2Loading = false;
      }),
      error => {
        this.masterModel.productCategorySub2Loading = false;
      };
  }

  //get the sub category 2 virtual list for new/ edit pop up
  getEditProductSubCategory2ListBySearch() {
    this.editMasterModel.productSubCategory2Request = new ProductSubCategory2SourceRequest();
    if (this.editModel.productSubCategoryId)
      this.editMasterModel.productSubCategory2Request.productSubCategoryIds.push(this.editModel.productSubCategoryId);
    if (this.editModel.productSubCategory2Id)
      this.editMasterModel.productSubCategory2Request.productSubCategory2Ids.push(this.editModel.productSubCategory2Id);
    this.editMasterModel.productSubCategory2Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editMasterModel.productCategorySub2Loading = true),
      switchMap(term => term
        ? this.cusProdservice.getProductSubCategory2DataSource(this.editMasterModel.productSubCategory2Request, term)
        : this.cusProdservice.getProductSubCategory2DataSource(this.editMasterModel.productSubCategory2Request)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editMasterModel.productCategorySub2Loading = false))
      ))
      .subscribe(data => {
        this.editMasterModel.productSubCategory2EditList = data;
        this.editMasterModel.productCategorySub2Loading = false;
      });
  }

  //fetch the product sub category 2 data with virtual scroll
  getEditProductSubCategory2Data(isDefaultLoad: boolean) {

    if (this.editModel.productSubCategoryId)
      this.editMasterModel.productSubCategory2Request.productSubCategoryIds.push(this.editModel.productSubCategoryId);

    if (isDefaultLoad) {
      this.editMasterModel.productSubCategory2Request.searchText = this.editMasterModel.productSubCategory2Input.getValue();
      this.editMasterModel.productSubCategory2Request.skip = this.editMasterModel.productSubCategory2EditList.length;
    }
    this.editMasterModel.productCategorySub2Loading = true;
    this.cusProdservice.getProductSubCategory2DataSource(this.editMasterModel.productSubCategory2Request).
      subscribe(data => {
        if (data && data.length > 0) {
          this.editMasterModel.productSubCategory2EditList = this.editMasterModel.productSubCategory2EditList.concat(data);
        }
        if (isDefaultLoad)
          this.editMasterModel.productSubCategory2Request = new ProductSubCategory2SourceRequest();
        this.editMasterModel.productCategorySub2Loading = false;
      }),
      error => {
        this.editMasterModel.productCategorySub2Loading = false;
      };
  }

  openDeletePopUp(content, id) {
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal", backdrop: 'static' });
    this.idToDelete = id;
  }

  openNewPopUp(content, _isEdit) {

    this.isEdit = _isEdit;
    this.editMasterModel = new EditProductCategorySub3MasterModel();
    this.editModel = new EditProductCategorySub3Model();
    this.editMasterModel.loading = true;

    this.validator.initTost();
    this.validator.isSubmitted = false;

    this.editMasterModel.productCategoryList = this.masterModel.productCategoryList;
    this.editMasterModel.productSubCategoryList = this.masterModel.productSubCategoryList;
    if (!this.isEdit)
      this.getEditProductSubCategory2ListBySearch();

    this.editMasterModel.loading = false;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.popUpOpen = true;

  }

  onEditCategoryChange(id) {
    if (!id) {
      this.getEditProductSubCategoryList();
      this.getEditProductSubCategory2ListBySearch();
    }
    else {
      this.editModel.name = null;
      this.refresfSubbyCategoryIdEdit(id);
    }
    this.editModel.productSubCategoryId = null;
    this.editModel.productSubCategory2Id = null;

  }

  onEditSubCategoryChangeSub(id) {
    this.editModel.name = null;
    this.editModel.productSubCategory2Id = null;
    this.getEditProductSubCategory2ListBySearch();
  }

  onEditCategorySub2ChangeSub2(item) {
    if (item) {
      this.isCategoryId = true;
    }
    else {
      this.isCategoryId = false;
    }
    this.editModel.name = null;
  }

  refresfSubbyCategoryIdEdit(id) {
    if (id != null && id > 0) {
      this.editMasterModel.productSubCategoryLoading = true;
      this.service.getsubbycategoryid(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.editMasterModel.productSubCategoryList = result.productSubCategoryList;
            }
            else {
              this.editMasterModel.productSubCategoryList = [];
            }
            this.editMasterModel.productSubCategoryLoading = false;
          },
          error => {
            this.editMasterModel.productSubCategoryList = [];
            this.editMasterModel.productSubCategoryLoading = false;
          });
    }
    else {
      this.editMasterModel.productSubCategoryList = [];
    }
  }

  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.isFormValid()) {
      this.editMasterModel.saveloading = true;

      let res = await this.service.saveProductCategorySub3(this.editModel);

      if (res.result == ResponseResult.Success) {
        this.showSuccess("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "COMMON.MSG_SAVED_SUCCESS");
        this.getData();
      }
      else {
        switch (res.result) {
          case 2:
            this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_CANNOT_SAVE_CATEGORY");
            break;
          case 3:
            this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_CURRENT_CATEGORY_NOTFOUND");
            break;
          case 4:
            this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_CANNOTMAPREQUEST");
            break;
          case 5:
            this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_DUPLICATE_NAME");
            break;
        }
      }

      this.editMasterModel.saveloading = false;
      if (this.popUpOpen) {
        this.modelRef.close();
      }
    }
  }


  async editProdCategorySub3(id, content) {
    let res = await this.service.getCategorySub3ById(id)
    if (res.result == ResponseResult.Success) {
      this.openNewPopUp(content, true);
      this.mapEditData(res.data);
      this.getEditProductSubCategory2ListBySearch();
      this.model.noFound = false;
    }
    else {
      this.model.items = [];
      this.model.noFound = true;
    }
    this.editMasterModel.saveloading = false;

  }

  mapEditData(data) {
    this.editModel = {
      id: data.id,
      productCategoryId: data.prodCategoryId,
      productSubCategoryId: data.prodSubCategoryId,
      productSubCategory2Id: data.prodCategorySub2Id,
      name: data.prodCategorySub3,
      workLoadMatrixChecked: data.workLoadMatrixChecked,
      preparationTime: data.preparationTime,
      eightHourSampleSize: data.eightHourSampleSize
    };
  }

  async delete() {
    this.deleteLoading = true;

    let res = await this.service.deleteProductCategorySub3ById(this.idToDelete);
    this.deleteLoading = false;
    this.modelRef.close();
    if (res.result == ResponseResult.Success) {
      this.showSuccess("PRODUCT_CATEGORY_SUB3.LBL_TITLE", 'COMMON.MSG_DELETE_SUCCESS');
    }
    else if(res.result == 4){
      this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", 'PRODUCT_CATEGORY_SUB3.MSG_PROD_MAPPED');
    }
    else {
      this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", 'COMMON.MSG_CANNOT_DELETE');
    }
    this.getData();
  }

  workLoadChange() {
    this.editModel.workLoadMatrixChecked = !this.editModel.workLoadMatrixChecked;
  }

  deleteWorks() {
    this.delete();
  }

  isFormValid() {
    let isOk = this.validator.isValid('productCategoryId')
      && this.validator.isValid('productSubCategoryId')
      && this.validator.isValid('productSubCategory2Id')
      && this.validator.isValid('name')

    if (isOk && this.editModel.workLoadMatrixChecked) {
      if (!this.editModel.preparationTime) {
        isOk = false;
        this.showWarning('Validation Error', 'Preparation time is required');
      }
      if (!this.editModel.eightHourSampleSize) {
        isOk = false;
        this.showWarning('Validation Error', '8h Sample Size is required');
      }
    }

    return isOk;
  }

  checkIfPreparationTimeValid() {
    if (this.editModel.workLoadMatrixChecked && this.validator.isSubmitted && !this.editModel.preparationTime)
      return true;

    return false;
  }
  checkIf8hSampleSizeValid() {
    if (this.editModel.workLoadMatrixChecked && this.validator.isSubmitted && !this.editModel.eightHourSampleSize)
      return true;

    return false;
  }

  cancel() {
    this.validator.initTost();
    this.validator.isSubmitted = false;
    this.editModel = new EditProductCategorySub3Model();
    this.modelRef.close();
  }
  export() {
    this.masterModel.exportLoading = true;
    this.service.exportSummary(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.masterModel.exportLoading = false;
        });
  }


  downloadFile(data, mimeType) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "Prod_cat_sub3_summary.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Prod_cat_sub3_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportLoading = false;
  }

  validateNumbers(event) {
    if (event.target.value < 1)
      event.target.value = null;

    if (!event.target.value)
      event.target.value = null;
  }
 
}
