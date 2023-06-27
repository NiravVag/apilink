import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap } from 'rxjs/operators';
import { CommonDataSourceRequest, ProductCategorySourceRequest, ProductSubCategory2SourceRequest, ProductSubCategorySourceRequest } from 'src/app/_Models/common/common.model';
import { QCBlockEdit, QCBlockMasterData, SaveQCBlockResponseResult } from 'src/app/_Models/Schedule/editqcblock.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerService } from 'src/app/_Services/customer/customer.service';
import { CustomerProduct } from 'src/app/_Services/customer/customerproductsummary.service';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { QCBlockService } from 'src/app/_Services/Schedule/qcblock.service';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
import { JsonHelper, Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { SupplierType } from '../../common/static-data-common';

@Component({
  selector: 'app-edit-qc-block',
  templateUrl: './edit-qc-block.component.html'
})
export class EditQcBlockComponent extends DetailComponent {
  masterData: QCBlockMasterData;
  model: QCBlockEdit;
  jsonHelper: JsonHelper;

  constructor(router: Router, route: ActivatedRoute, translate: TranslateService, toastr: ToastrService,
    public modalService: NgbModal, private cusService: CustomerService, private supService: SupplierService,
    private customerProductService: CustomerProduct, private hrService: HrService,
    public utility: UtilityService,
    private qcBlockService: QCBlockService, public validator: Validator,
  ) {
    super(router, route, translate, toastr, modalService);
    this.validator.setJSON("schedule/edit-qc-block.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = validator.jsonHelper;
    this.validator.isSubmitted = false;
  }

  onInit(id?: number) {
    this.masterData = new QCBlockMasterData();
    this.model = new QCBlockEdit();

    if (id && id > 0) {
      this.edit(id);
    }
    else {
      this.getCustomerListBySearch();
      this.getSupListBySearch();
      this.getFactListBySearch();
      this.getQCListBySearch();
      this.getProductCategoryListBySearch();
    }
  }

  //fetch the first 10 customer on load
  getCustomerListBySearch() {
    if (this.model && this.model.customerIds && this.model.customerIds.length > 0)
      this.masterData.customerModelRequest.idList = this.model.customerIds;
    this.masterData.customerModelRequest.customerId = 0;
    this.masterData.customerInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.customerLoading = true),
      switchMap(term => term
        ? this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest, term)
        : this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.customerLoading = false))
      ))
      .subscribe(data => {
        this.masterData.customerList = data;
        this.masterData.customerLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCustomerData() {

    this.masterData.customerModelRequest.searchText = this.masterData.customerInput.getValue();
    this.masterData.customerModelRequest.skip = this.masterData.customerList.length;

    this.masterData.customerLoading = true;
    this.cusService.getCustomerDataSourceList(this.masterData.customerModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.customerList = this.masterData.customerList.concat(data);
        }

        this.masterData.customerModelRequest = new CommonDataSourceRequest();
        this.masterData.customerLoading = false;
      }),
      error => {
        this.masterData.customerLoading = false;
        this.setError(error);
      };
  }

  clearCustomerSelection() {
    this.getCustomerData();
  }

  //fetch the facotry data with virtual scroll
  getFactoryData() {
    this.masterData.factoryModelRequest.searchText = this.masterData.factoryInput.getValue();
    this.masterData.factoryModelRequest.skip = this.masterData.factoryList.length;

    this.masterData.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterData.factoryLoading = true;

    this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.factoryList = this.masterData.factoryList.concat(data);
        }
        this.masterData.factoryModelRequest = new CommonDataSourceRequest();
        this.masterData.factoryLoading = false;
      }),
      error => {
        this.masterData.factoryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 fact for the supplier on load
  getFactListBySearch() {
    if (this.model && this.model.factoryIds && this.model.factoryIds.length > 0)
      this.masterData.factoryModelRequest.idList = this.model.factoryIds;
    this.masterData.factoryList = null;
    this.masterData.factoryModelRequest.supplierType = SupplierType.Factory;
    this.masterData.factoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.factoryLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterData.factoryModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.factoryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.factoryList = data;
        this.masterData.factoryLoading = false;
      });
  }

  clearFactorySelection() {
    this.masterData.factoryModelRequest = new CommonDataSourceRequest();
    this.getFactoryData();
  }

  //fetch the supplier data with virtual scroll
  getSupplierData() {
    this.masterData.supplierLoading = true;

    this.masterData.supplierModelRequest.searchText = this.masterData.supplierInput.getValue();
    this.masterData.supplierModelRequest.skip = this.masterData.supplierList.length;
    this.masterData.supplierModelRequest.supplierType = SupplierType.Supplier;

    this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.supplierList = this.masterData.supplierList.concat(data);
        }
        this.masterData.supplierModelRequest = new CommonDataSourceRequest();
        this.masterData.supplierLoading = false;
      }),
      error => {
        this.masterData.supplierLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 suppliers for the customer on load
  getSupListBySearch() {
    if (this.model && this.model.supplierIds && this.model.supplierIds.length > 0)
      this.masterData.supplierModelRequest.idList = this.model.supplierIds;
    this.masterData.supplierList = null;
    this.masterData.supplierModelRequest.supplierType = SupplierType.Supplier;
    this.masterData.supplierInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.supplierLoading = true),
      switchMap(term => term
        ? this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest, term)
        : this.supService.getFactoryDataSourceList(this.masterData.supplierModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.supplierLoading = false))
      ))
      .subscribe(data => {
        this.masterData.supplierList = data;
        this.masterData.supplierLoading = false;
      });
  }

  clearSupplierSelection() {
    this.masterData.supplierModelRequest = new CommonDataSourceRequest();
    this.getSupplierData();
  }
  //#region ProductSubCategory2 Loading
  //fetch the first 10 buyers on load
  getProductSubCategory2ListBySearch() {

    if (this.model.productCategorySubIds)
      this.masterData.productCategorySub2ModelRequest.productSubCategoryIds = this.model.productCategorySubIds;
    if (this.model.productCategorySub2Ids)
      this.masterData.productCategorySub2ModelRequest.productSubCategory2Ids = this.model.productCategorySub2Ids;
    this.masterData.productCategorySub2Input.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.productCategorySub2Loading = true),
      switchMap(term => term
        ? this.customerProductService.getProductSubCategory2DataSource(this.masterData.productCategorySub2ModelRequest, term)
        : this.customerProductService.getProductSubCategory2DataSource(this.masterData.productCategorySub2ModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.productCategorySub2Loading = false))
      ))
      .subscribe(data => {
        this.masterData.productCategorySub2List = data;
        this.masterData.productCategorySub2Loading = false;
      });
  }

  //fetch the product sub category 2 data with virtual scroll
  getProductSubCategory2Data(isDefaultLoad: boolean) {
    if (this.model.productCategorySubIds)
      this.masterData.productCategorySub2ModelRequest.productSubCategoryIds = this.model.productCategorySubIds;
    if (isDefaultLoad) {
      this.masterData.productCategorySub2ModelRequest.searchText = this.masterData.productCategorySub2Input.getValue();
      this.masterData.productCategorySub2ModelRequest.skip = this.masterData.productCategorySub2List.length;
    }
    this.masterData.productCategorySub2Loading = true;
    this.customerProductService.getProductSubCategory2DataSource(this.masterData.productCategorySub2ModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.productCategorySub2List = this.masterData.productCategorySub2List.concat(data);
        }
        // if (isDefaultLoad)
        //   this.masterData.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
        this.masterData.productCategorySub2Loading = false;
      }),
      error => {
        this.masterData.productCategorySub2Loading = false;
      };
  }

  clearProductSubCategory2Selection() {
    this.masterData.productCategorySub2ModelRequest = new ProductSubCategory2SourceRequest();
    this.getProductSubCategory2ListBySearch();
  }
  //#endregion

  //#region ProductSubCategory Loading
  //fetch the first 10 buyers on load
  getProductSubCategoryListBySearch() {
    this.masterData.productCategorySubModelRequest.serviceId = 0;
    if (this.model.productCategoryIds)
      this.masterData.productCategorySubModelRequest.productCategoryIds = this.model.productCategoryIds;
    if (this.model && this.model.productCategorySubIds && this.model.productCategorySubIds)
      this.masterData.productCategorySubModelRequest.productSubCategoryIds = this.model.productCategorySubIds;
    this.masterData.productCategorySubInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.productCategorySubLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductSubCategoryDataSource(this.masterData.productCategorySubModelRequest, term)
        : this.customerProductService.getProductSubCategoryDataSource(this.masterData.productCategorySubModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.productCategorySubLoading = false))
      ))
      .subscribe(data => {
        this.masterData.productCategorySubList = data;
        this.masterData.productCategorySubLoading = false;
      });
  }

  //fetch the product sub category data with virtual scroll
  getProductSubCategoryData(isDefaultLoad: boolean) {
    this.masterData.productCategorySubModelRequest.serviceId = 0;
    if (this.model.productCategoryIds)
      this.masterData.productCategorySubModelRequest.productCategoryIds = this.model.productCategoryIds;
    //this.masterData.requestCustomerModel.take = 2;
    if (isDefaultLoad) {
      this.masterData.productCategorySubModelRequest.searchText = this.masterData.productCategorySubInput.getValue();
      this.masterData.productCategorySubModelRequest.skip = this.masterData.productCategorySubList.length;
    }
    this.masterData.productCategorySubLoading = true;
    this.customerProductService.getProductSubCategoryDataSource(this.masterData.productCategorySubModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.productCategorySubList = this.masterData.productCategorySubList.concat(data);
        }
        // if (isDefaultLoad)
        //   this.masterData.productCategorySubModelRequest = new ProductSubCategorySourceRequest();
        this.masterData.productCategorySubLoading = false;
      }),
      error => {
        this.masterData.productCategorySubLoading = false;
      };
  }

  clearProductSubCategorySelection() {
    this.masterData.productCategorySubModelRequest = new ProductSubCategorySourceRequest();
    this.masterData.productCategorySubList = [];

    this.model.productCategorySub2Ids = [];
    this.masterData.productCategorySub2List = [];

    this.getProductSubCategoryListBySearch();
  }

  changeProductSubCategorySelection(event) {
    this.model.productCategorySub2Ids = [];
    if (event && event.length > 0) {
      this.masterData.productCategorySub2List = [];

      this.getProductSubCategory2ListBySearch();

    }
    else {
      this.masterData.productCategorySub2List = [];
    }
  }
  //#endregion

  //#region ProductCategory Loading

  //fetch the first 10  on load
  getProductCategoryListBySearch() {
    if (this.model && this.model.productCategoryIds && this.model.productCategoryIds.length > 0)
      this.masterData.productCategoryModelRequest.productCategoryIds = this.model.productCategoryIds;
    this.masterData.productCategoryModelRequest.serviceId = 0;
    this.masterData.productCategoryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.productCategoryLoading = true),
      switchMap(term => term
        ? this.customerProductService.getProductCategoryDataSource(this.masterData.productCategoryModelRequest, term)
        : this.customerProductService.getProductCategoryDataSource(this.masterData.productCategoryModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.productCategoryLoading = false))
      ))
      .subscribe(data => {
        this.masterData.productCategoryList = data;
        this.masterData.productCategoryLoading = false;
      });
  }

  //fetch the product category data with virtual scroll
  getProductCategoryData(isDefaultLoad: boolean) {
    this.masterData.productCategoryModelRequest.serviceId = 0;
    if (isDefaultLoad) {
      this.masterData.productCategoryModelRequest.searchText = this.masterData.productCategoryInput.getValue();
      this.masterData.productCategoryModelRequest.skip = this.masterData.productCategoryList.length;
    }
    this.masterData.productCategoryLoading = true;
    this.customerProductService.getProductCategoryDataSource(this.masterData.productCategoryModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.productCategoryList = this.masterData.productCategoryList.concat(data);
        }
        if (isDefaultLoad)
          this.masterData.productCategoryModelRequest = new ProductCategorySourceRequest();
        this.masterData.productCategoryLoading = false;
      }),
      error => {
        this.masterData.productCategoryLoading = false;
      };
  }

  changeProductCategoryData(event) {
    this.model.productCategorySubIds = [];
    this.model.productCategorySub2Ids = [];
    this.masterData.productCategorySub2List = [];
    if (event && event.length > 0) {
      this.masterData.productCategorySubList = [];

      this.getProductSubCategoryListBySearch();
    }
    else {
      this.masterData.productCategorySubList = [];
    }
  }

  clearProductCategorySelection() {
    this.masterData.productCategoryModelRequest = new ProductCategorySourceRequest();
    this.masterData.productCategoryList = [];

    this.model.productCategorySubIds = [];
    this.masterData.productCategorySubList = [];

    this.masterData.productCategorySub2List = [];
    this.model.productCategorySub2Ids = [];
    this.getProductCategoryData(false);
  }

  //#endregion


  getQCData() {
    this.masterData.qcModelRequest.searchText = this.masterData.qcInput.getValue();
    this.masterData.qcModelRequest.skip = this.masterData.qcList.length;

    if (this.model.qcId && this.model.qcId > 0)
      this.masterData.qcModelRequest.id = this.model.qcId;

    this.masterData.qcLoading = true;
    this.hrService.getQCDataSource(this.masterData.qcModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterData.qcList = this.masterData.qcList.concat(data);
        }
        this.masterData.qcModelRequest = new CommonDataSourceRequest();
        this.masterData.qcLoading = false;
      }),
      error => {
        this.masterData.qcLoading = false;
      };
  }

  //fetch the first 10 QC on load
  getQCListBySearch() {
    this.masterData.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterData.qcLoading = true),
      switchMap(term => term
        ? this.hrService.getQCDataSource(this.masterData.qcModelRequest, term)
        : this.hrService.getQCDataSource(this.masterData.qcModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterData.qcLoading = false))
      ))
      .subscribe(data => {
        this.masterData.qcList = data;
        this.masterData.qcLoading = false;
      });
  }

  //validate the UI
  isFormValid(): boolean {
    var isOk: boolean = false;

    var isOk = this.validator.isValid('qcId');

    if (isOk) {
      if (!((this.model.customerIds && this.model.customerIds.length > 0) || (this.model.supplierIds && this.model.supplierIds.length > 0) ||
        (this.model.factoryIds && this.model.factoryIds.length > 0) || (this.model.productCategoryIds && this.model.productCategoryIds.length > 0)
        || (this.model.productCategorySubIds && this.model.productCategorySubIds.length > 0) ||
        (this.model.productCategorySub2Ids && this.model.productCategorySub2Ids.length > 0))) {
        this.showWarning('EDIT_QC_BLOCK.LBL_TITLE', 'EDIT_QC_BLOCK.LBL_QC_SELECT_ANY_ONE_FIELD_TO_BLOCK');

        isOk = false;
      }
    }

    return isOk;
  }
  //save the details
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.masterData.saveLoading = true;
      this.qcBlockService.save(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == SaveQCBlockResponseResult.Success) {
              if (this.fromSummary) {
                this.return('summary/qc-block-summary');
              }
              else {
                this.edit(response.id);
              }
              this.showSuccess("EDIT_QC_BLOCK.LBL_TITLE", "COMMON.MSG_SAVED_SUCCESS");
            }
            else if(response && response.result == SaveQCBlockResponseResult.IsExists) {
              this.showWarning("EDIT_QC_BLOCK.LBL_TITLE", "EDIT_QC_BLOCK.LBL_QC_BLOCK_ALREADY_EXISTS");
            }
            else if(response && response.result == SaveQCBlockResponseResult.SelectAnyOtherField) {
              this.showWarning('EDIT_QC_BLOCK.LBL_TITLE', 'EDIT_QC_BLOCK.LBL_QC_SELECT_ANY_ONE_FIELD_TO_BLOCK');
            }
            else {
              this.showError("EDIT_QC_BLOCK.LBL_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterData.saveLoading = false;
          },
          error => {
            if (error && error.error && error.error.errors && error.error.statusCode == 400) {
              let validationErrors: [];
              validationErrors = error.error.errors;
              this.openValidationPopup(validationErrors);
            }
            else {
              this.showError("EDIT_QC_BLOCK.LBL_TITLE", "COMMON.MSG_UNKNONW_ERROR");
            }
            this.masterData.saveLoading = false;
          });
    }
  }

  //edit the QC block details
  edit(id: number) {
    this.qcBlockService.edit(id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == SaveQCBlockResponseResult.Success) {

            this.model = response.qcBlockDetails;
            this.masterData.isQCDisabled = true;
            this.getCustomerListBySearch();
            this.getSupListBySearch();
            this.getFactListBySearch();
            this.getQCData();
            this.getProductCategoryListBySearch();
            this.getProductSubCategoryListBySearch();
            this.getProductSubCategory2ListBySearch();

          }
        },
        error => {
          this.setError(error);
        });
  }

  getEditPath() {
    return "";
  }

  getViewPath() {
    return "";
  }

}
