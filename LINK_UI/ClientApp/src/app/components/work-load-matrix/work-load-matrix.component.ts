import { first } from '@amcharts/amcharts4/.internal/core/utils/Array';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ProductSubCategory2SourceRequest, ProductSubCategory3SourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { EditWorkLoadMatrixModel, EditWorkLoadMatrixMasterModel, WorkLoadMatrixMasterModel, WorkLoadMatrixSummaryModel } from 'src/app/_Models/workloadmatrix/workloadmatrix.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { ProductManagementService } from 'src/app/_Services/productmanagement/productmanagement.service';
import { WorkLoadMatrixService } from 'src/app/_Services/workloadmatrix/workloadmatrix.service';
import { PageSizeCommon } from '../common/static-data-common';
import { SummaryComponent } from '../common/summary.component';
import { Validator } from '../common/validator';

@Component({
  selector: 'app-work-load-matrix',
  templateUrl: './work-load-matrix.component.html',
  styleUrls: ['./work-load-matrix.component.scss'],
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
export class WorkLoadMatrixComponent extends SummaryComponent<WorkLoadMatrixSummaryModel> {

  public componentDestroyed$: Subject<boolean> = new Subject();
  public model: WorkLoadMatrixSummaryModel;
  public masterModel: WorkLoadMatrixMasterModel;
  public editMasterModel: EditWorkLoadMatrixMasterModel;
  public editModel: EditWorkLoadMatrixModel;
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
  public showAddButton: boolean;

  constructor(
    private service: WorkLoadMatrixService,
    private prodMgmtService: ProductManagementService,
    public validator: Validator,
    public utility: UtilityService, public toastr: ToastrService,
    router: Router, route: ActivatedRoute, translate: TranslateService, public cusProdservice: CustomerProduct, public modalService: NgbModal) {
    super(router, validator, route, translate, toastr);
    this.model = new WorkLoadMatrixSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.validator.setJSON("workloadmatrix/add-workloadmatrix.valid.json");
    this.validator.setModelAsync(() => this.editModel);
    this.Intitialize();
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  Intitialize() {

    this.masterModel = new WorkLoadMatrixMasterModel();
    this.getProductCategoryList();
    this.getProductSubCategoryList();
    this.getProductSubCategory2ListBySearch();
    this.getProductSubCategory3ListBySearch();
  }

  getData(): void {
    if (this.model.productCategorySub2IdList) {
      this.isCategoryId = true;
    }
    this.masterModel.searchloading = true;
    this.service.getWorkLoadMatrixSearchSummary(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data.length > 0) {
            this.mapPageProperties(response);
            this.model.items = response.data;
            this.hasITRole = response.hasITRole;
            
            this.model.noFound = false;

            this.showAddButton = this.model.workLoadMatrixNotConfigured;
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

    this.model.productSubCategoryId = null;
    this.model.productCategorySub2IdList = null;
    this.model.productCategorySub3IdList = [];

    if (!id) {
      this.getProductSubCategoryList();
    }
    else {
      this.refresSubbyCategoryId(id);
    }
    this.getProductSubCategory2ListBySearch();
    this.getProductSubCategory3ListBySearch();
  }

  onChangeSub(id) {
    this.model.productCategorySub2IdList = null;
    this.model.productCategorySub3IdList = [];

    this.getProductSubCategory2ListBySearch();
    this.getProductSubCategory3ListBySearch();
  }

  onChangeSub2(item) {
    this.model.productCategorySub3IdList = [];
    this.getProductSubCategory3ListBySearch();
  }

  refresSubbyCategoryId(id) {
    if (id != null && id > 0) {
      this.masterModel.productSubCategoryLoading = true;
      this.prodMgmtService.getsubbycategoryid(id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.masterModel.productSubCategoryList = result.productSubCategoryList;
            }
            else {
              this.masterModel.productSubCategoryList = [];
            }
            this.masterModel.productSubCategoryLoading = false;
          },
          error => {
            this.masterModel.productSubCategoryList = [];
            this.masterModel.productSubCategoryLoading = false;
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
    this.prodMgmtService.getProductCategoryList()
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
    this.prodMgmtService.getProductSubCategoryList()
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

  //virtual scroll product category sub 2
  getProductSubCategory2ListBySearch() {
    this.masterModel.productSubCategory2Request = new ProductSubCategory2SourceRequest();
    if (this.model.productCategoryId)
      this.masterModel.productSubCategory2Request.productCategoryId = this.model.productCategoryId;
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

    if (this.model.productCategoryId)
      this.masterModel.productSubCategory2Request.productCategoryId = this.model.productCategoryId;

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
    if (this.editModel.productCategoryId)
      this.editMasterModel.productSubCategory2Request.productCategoryId = this.editModel.productCategoryId;
    this.editMasterModel.productSubCategory2Request.productSubCategoryIds = [];
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

    if (this.editModel.productCategoryId)
      this.editMasterModel.productSubCategory2Request.productCategoryId = this.editModel.productCategoryId;

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

  //get the sub category 3 virtual list for new/ edit pop up
  getProductSubCategory3ListBySearch() {
    this.masterModel.productSubCategory3Request = new ProductSubCategory3SourceRequest();

    if (this.model.productCategoryId) {
      this.masterModel.productSubCategory3Request.productCategoryId = this.model.productCategoryId;
    }
    if (this.model.productSubCategoryId) {
      this.masterModel.productSubCategory3Request.productCategoryId = this.model.productCategoryId;
    }

    if (this.model.productCategorySub2IdList && this.model.productCategorySub2IdList.length > 0)
      this.masterModel.productSubCategory3Request.productSubCategory2Ids = this.model.productCategorySub2IdList;
    if (this.model.productCategorySub3IdList && this.model.productCategorySub3IdList.length > 0)
      this.masterModel.productSubCategory3Request.productSubCategory3Ids = this.model.productCategorySub3IdList;
    this.masterModel.productSubCategory3Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.productCategorySub3Loading = true),
      switchMap(term => term
        ? this.cusProdservice.getProductSubCategory3DataSource(this.masterModel.productSubCategory3Request, term)
        : this.cusProdservice.getProductSubCategory3DataSource(this.masterModel.productSubCategory3Request)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.productCategorySub3Loading = false))
      ))
      .subscribe(data => {
        this.masterModel.productSubCategory3List = data;
        this.masterModel.productCategorySub3Loading = false;
      });
  }

  //fetch the product sub category 3 data with virtual scroll
  getProductSubCategory3Data(isDefaultLoad: boolean) {
    if (this.model.productCategoryId) {
      this.masterModel.productSubCategory3Request.productCategoryId = this.model.productCategoryId;
    }
    if (this.model.productSubCategoryId) {
      this.masterModel.productSubCategory3Request.productCategoryId = this.model.productCategoryId;
    }
    if (this.model.productCategorySub2IdList)
      this.masterModel.productSubCategory3Request.productSubCategory2Ids = this.model.productCategorySub2IdList;

    if (isDefaultLoad) {
      this.masterModel.productSubCategory3Request.searchText = this.masterModel.productSubCategory3Input.getValue();
      this.masterModel.productSubCategory3Request.skip = this.masterModel.productSubCategory3List.length;
    }
    this.masterModel.productCategorySub3Loading = true;
    this.cusProdservice.getProductSubCategory3DataSource(this.masterModel.productSubCategory3Request).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.productSubCategory3List = this.masterModel.productSubCategory3List.concat(data);
        }
        if (isDefaultLoad)
          this.masterModel.productSubCategory3Request = new ProductSubCategory3SourceRequest();
        this.masterModel.productCategorySub3Loading = false;
      }),
      error => {
        this.masterModel.productCategorySub3Loading = false;
      };
  }

  //get the sub category 3 virtual list for new/ edit pop up
  getEditProductSubCategory3ListBySearch() {
    this.editMasterModel.productSubCategory3Request = new ProductSubCategory3SourceRequest();

    if (this.editModel.productCategoryId) {
      this.editMasterModel.productSubCategory3Request.productCategoryId = this.editModel.productCategoryId;
    }
    if (this.editModel.productSubCategoryId) {
      this.editMasterModel.productSubCategory3Request.productCategoryId = this.editModel.productCategoryId;
    }
    this.editMasterModel.productSubCategory3Request.productSubCategory2Ids = [];
    if (this.editModel.productSubCategory2Id)
      this.editMasterModel.productSubCategory3Request.productSubCategory2Ids.push(this.editModel.productSubCategory2Id);
    if (this.editModel.productSubCategory3Id)
      this.editMasterModel.productSubCategory3Request.productSubCategory3Ids.push(this.editModel.productSubCategory3Id);
    this.editMasterModel.productSubCategory3Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editMasterModel.productCategorySub3Loading = true),
      switchMap(term => term
        ? this.cusProdservice.getProductSubCategory3DataSource(this.editMasterModel.productSubCategory3Request, term)
        : this.cusProdservice.getProductSubCategory3DataSource(this.editMasterModel.productSubCategory3Request)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editMasterModel.productCategorySub3Loading = false))
      ))
      .subscribe(data => {
        this.editMasterModel.productSubCategory3EditList = data;
        this.editMasterModel.productCategorySub3Loading = false;
      });
  }

  //fetch the product sub category 3 data with virtual scroll
  getEditProductSubCategory3Data(isDefaultLoad: boolean) {
    if (this.editModel.productCategoryId) {
      this.editMasterModel.productSubCategory3Request.productCategoryId = this.editModel.productCategoryId;
    }
    if (this.editModel.productSubCategoryId) {
      this.editMasterModel.productSubCategory3Request.productCategoryId = this.editModel.productCategoryId;
    }
    if (this.editModel.productSubCategory2Id)
      this.editMasterModel.productSubCategory3Request.productSubCategory2Ids.push(this.editModel.productSubCategoryId);

    if (isDefaultLoad) {
      this.editMasterModel.productSubCategory3Request.searchText = this.editMasterModel.productSubCategory3Input.getValue();
      this.editMasterModel.productSubCategory3Request.skip = this.editMasterModel.productSubCategory3EditList.length;
    }
    this.editMasterModel.productCategorySub3Loading = true;
    this.cusProdservice.getProductSubCategory3DataSource(this.editMasterModel.productSubCategory3Request).
      subscribe(data => {
        if (data && data.length > 0) {
          this.editMasterModel.productSubCategory3EditList = this.editMasterModel.productSubCategory3EditList.concat(data);
        }
        if (isDefaultLoad)
          this.editMasterModel.productSubCategory3Request = new ProductSubCategory3SourceRequest();
        this.editMasterModel.productCategorySub3Loading = false;
      }),
      error => {
        this.editMasterModel.productCategorySub3Loading = false;
      };
  }

  openDeletePopUp(content, id) {
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal", backdrop: 'static' });
    this.idToDelete = id;
  }

  openNewPopUp(content, _isEdit) {

    this.isEdit = _isEdit;
    this.editMasterModel = new EditWorkLoadMatrixMasterModel();
    this.editModel = new EditWorkLoadMatrixModel();
    this.editMasterModel.loading = true;

    this.validator.initTost();
    this.validator.isSubmitted = false;

    this.editMasterModel.productCategoryList = this.masterModel.productCategoryList;
    this.editMasterModel.productSubCategoryList = this.masterModel.productSubCategoryList;
    if (!this.isEdit) {
      this.getEditProductSubCategory2ListBySearch();
      this.getEditProductSubCategory3ListBySearch();
    }

    this.editMasterModel.loading = false;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.popUpOpen = true;

  }

  onEditCategoryChange(id) {

    this.editModel.productSubCategoryId = null;
    this.editModel.productSubCategory2Id = null;
    this.editModel.productSubCategory3Id = null;

    if (!id) {
      this.getProductSubCategoryList();
    }
    else {
      this.refresfSubbyCategoryIdEdit(id);
    }
    this.getEditProductSubCategory2ListBySearch();
    this.getEditProductSubCategory3ListBySearch();


  }

  onEditSubCategoryChangeSub(id) {
    this.editModel.productSubCategory3Id = null;
    this.editModel.productSubCategory2Id = null;
    this.getEditProductSubCategory2ListBySearch();
    this.getEditProductSubCategory3ListBySearch();
  }

  onEditCategorySub2ChangeSub2(item) {

    this.getEditProductSubCategory3ListBySearch();
    this.editModel.productSubCategory3Id = null;
  }

  refresfSubbyCategoryIdEdit(id) {
    if (id != null && id > 0) {
      this.editMasterModel.productSubCategoryLoading = true;
      this.prodMgmtService.getsubbycategoryid(id)
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

      try {
        let res = await this.service.saveWorkLoadMatrix(this.editModel);

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
              this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_DUPLICATE_NAME");
              break;
            case 5:
              this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", "INTERNAL_PRODUCT.MSG_CANNOTMAPREQUEST");
              break;
          }
        }
      }
      catch (e) {

      }
      finally {
        this.editMasterModel.saveloading = false;
        if (this.popUpOpen) {
          this.modelRef.close();
        }
      }
    }
  }


  async editWorkLoadMatrix(id, prodCat3Id, content) {
    let _id = id > 0 ? id : prodCat3Id;
    let res = await this.service.getWorkLoadMatrixById(_id, this.model.workLoadMatrixNotConfigured)
    if (res.result == ResponseResult.Success) {
      this.openNewPopUp(content, true);
      this.mapEditData(res.data);
      this.getEditProductSubCategory2ListBySearch();
      this.getEditProductSubCategory3ListBySearch();
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
      preparationTime: data.preparationTime,
      eightHourSampleSize: data.eightHourSampleSize,
      productSubCategory3Id: data.prodCategorySub3Id
    };
  }

  async delete() {
    this.deleteLoading = true;

    let res = await this.service.deleteWorkLoadMatrixById(this.idToDelete);
    this.deleteLoading = false;
    this.modelRef.close();
    if (res.result == ResponseResult.Success) {
      this.showSuccess("PRODUCT_CATEGORY_SUB3.LBL_TITLE", 'COMMON.MSG_DELETE_SUCCESS');
    }
    else {
      this.showError("PRODUCT_CATEGORY_SUB3.LBL_TITLE", 'COMMON.MSG_CANNOT_DELETE');
    }
    this.getData();
  }

  deleteWorks() {
    this.delete();
  }

  isFormValid() {
    let isOk = this.validator.isValid('productCategoryId')
      && this.validator.isValid('productSubCategoryId')
      && this.validator.isValid('productSubCategory2Id')
      && this.validator.isValid('productSubCategory3Id')
      && this.validator.isValid('preparationTime')
      && this.validator.isValid('eightHourSampleSize')

    return isOk;
  }


  cancel() {
    this.validator.initTost();
    this.validator.isSubmitted = false;
    this.editModel = new EditWorkLoadMatrixModel();
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
      windowNavigator.msSaveOrOpenBlob(blob, "Workload_Matrix.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Workload_Matrix_summary.xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportLoading = false;
  }
}
