import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap } from 'rxjs/operators';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { SaveQCBlockResponseResult } from 'src/app/_Models/Schedule/editqcblock.model';
import { QCBlockRequestModel, QCBlockSummaryItem, QCBlockSummaryModel } from 'src/app/_Models/Schedule/qcblocksummary.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { OfficeService } from 'src/app/_Services/office/office.service';
import { QCBlockService } from 'src/app/_Services/Schedule/qcblock.service';
import { Validator } from '../../common';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';

@Component({
  selector: 'app-qc-block-summary',
  templateUrl: './qc-block-summary.component.html',
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
export class QcBlockSummaryComponent extends SummaryComponent<QCBlockRequestModel> {

  pagesizeitems = PageSizeCommon;
  masterModel: QCBlockSummaryModel;
  selectedPageSize;
  isFilterOpen: boolean;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, private qcBlockService: QCBlockService, private hrService: HrService,
    public utility: UtilityService,
    private officeService: OfficeService) {

    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;

  }

  onInit() {
    this.model = new QCBlockRequestModel();
    this.masterModel = new QCBlockSummaryModel();
    this.selectedPageSize = PageSizeCommon[0];
  }

  ngAfterViewInit() {
    this.getQCListBySearch();
    this.getOfficeListBySearch();
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getData(): void {
    this.GetSearchData();
  }

  getPathDetails(): string {
    return "qcblock/edit-qc-block";
  }

  //get QC data list
  getQCData() {
    this.masterModel.qcModelRequest.searchText = this.masterModel.qcInput.getValue();
    this.masterModel.qcModelRequest.locationIdList = this.model.officeIds;
    this.masterModel.qcModelRequest.skip = this.masterModel.qcList.length;
    this.masterModel.qcLoading = true;
    this.hrService.getQCDataSource(this.masterModel.qcModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.qcList = this.masterModel.qcList.concat(data);
        }
        this.masterModel.qcLoading = false;
      }),
      error => {
        this.masterModel.qcLoading = false;
      };
  }

  //fetch the first 10 QC on load
  getQCListBySearch() {
    if (this.model && this.model.qcIds && this.model.qcIds.length > 0)
      this.masterModel.qcModelRequest.idList = this.model.qcIds;
    this.masterModel.qcModelRequest.locationIdList = this.model.officeIds;
    this.masterModel.qcInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.qcLoading = true),
      switchMap(term => term
        ? this.hrService.getQCDataSource(this.masterModel.qcModelRequest, term)
        : this.hrService.getQCDataSource(this.masterModel.qcModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.qcLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.qcList = data;
        this.masterModel.qcLoading = false;
      });
  }

  //get office list
  getOfficeData() {
    this.masterModel.officeModelRequest.searchText = this.masterModel.officeInput.getValue();
    this.masterModel.officeModelRequest.skip = this.masterModel.officeList.length;
    this.masterModel.officeLoading = true;
    this.officeService.getOfficeListByOfficeAccess(this.masterModel.officeModelRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.officeList = this.masterModel.officeList.concat(data);
        }
        this.masterModel.officeModelRequest = new CommonDataSourceRequest();
        this.masterModel.officeLoading = false;
      }),
      error => {
        this.masterModel.officeLoading = false;
      };
  }

  clearOffice() {
    this.masterModel.officeModelRequest = new CommonDataSourceRequest();
    this.getOfficeListBySearch();
  }

  clearQC() {
    this.masterModel.qcModelRequest = new CommonDataSourceRequest();
    this.getQCListBySearch();
  }
  //fetch the first 10 office on load
  getOfficeListBySearch() {
    if (this.model && this.model.officeIds && this.model.officeIds.length > 0)
      this.masterModel.officeModelRequest.idList = this.model.officeIds;
    this.masterModel.officeInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.officeLoading = true),
      switchMap(term => term
        ? this.officeService.getOfficeListByOfficeAccess(this.masterModel.officeModelRequest, term)
        : this.officeService.getOfficeListByOfficeAccess(this.masterModel.officeModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.officeLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.officeList = data;
        this.masterModel.officeLoading = false;
      });
  }


  Reset() {
    this.onInit();
    this.ngAfterViewInit();
  }

  redirectToEdit(id: number) {
    this.getDetails(id);
  }

  GetSearchData() {
    this.masterModel.searchLoading = true;
    this.model.noFound = false;
    this.qcBlockService.search(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == SaveQCBlockResponseResult.Success) {
            this.mapPageProperties(response);
            this.model.items = response.data.map(x => {
              var item: QCBlockSummaryItem = {
                customerNames: x.customerNames,
                supplierNames: x.supplierNames,
                factoryNames: x.factoryNames,
                productCategoryNames: x.productCategoryNames,
                productCategorySub2Names: x.productCategorySub2Names,
                productCategorySubNames: x.productCategorySubNames,
                qcName: x.qcName,
                qcBlockId: x.qcBlockId,
                isDeleteRow: false
              }
              return item;
            });
            this.masterModel.searchLoading = false;
          }

          else if (response && response.result == SaveQCBlockResponseResult.NoDataFound) {
            this.model.items = [];
            this.model.noFound = true;
            this.masterModel.searchLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.searchLoading = false;
        });

  }

  //export the details
  export() {
    this.masterModel.exportDataLoading = true;
    this.qcBlockService.export(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QCBlockSearchSummary");
      },
        error => {
          this.masterModel.exportDataLoading = false;
        });
  }

  downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName + ".xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName + ".xlsx";
      a.href = url;
      a.click();
    }
    this.masterModel.exportDataLoading = false;
  }

  //delete the QC block records
  deleteQCBlock() {
    var blockIds = this.model.items.filter(x => x.isDeleteRow).map(x => x.qcBlockId);
    this.masterModel.deleteLoading = true;
    this.qcBlockService.delete(blockIds)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == SaveQCBlockResponseResult.Success) {
            this.showSuccess("EDIT_QC_BLOCK.LBL_TITLE", "COMMON.MSG_DELETE_SUCCESS");
            this.SearchDetails();
            this.masterModel.deleteLoading = false;
            this.masterModel.isQCBlockSelected = false;
          }
          else if (response && response.result == SaveQCBlockResponseResult.NoDataFound) {
            this.masterModel.deleteLoading = false;
            this.masterModel.isQCBlockSelected = false;
          }
        },
        error => {
          this.setError(error);
          this.masterModel.deleteLoading = false;
          this.masterModel.isQCBlockSelected = false;
        });
  }

  SearchDetails() {
    if (this.model && this.model.items && this.model.items.length > 0)
      this.isRowDeleteSelected();
    else
      this.masterModel.isQCBlockSelected = false;

    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.refresh();
  }

  //check box change
  changeCheckBoxModel() {
    this.isRowDeleteSelected();
  }

  //check box change detect
  isRowDeleteSelected() {

    this.masterModel.selectAllCheckbox = this.model.items.every(function (item: any) {
      return item.isDeleteRow == true;
    });

    this.isDeleteButtonVisible();
  }

  //office change event
  changeOffice(event) {
    this.masterModel.qcModelRequest = new CommonDataSourceRequest();

    if (event && event[0] && event[0].id) {
      //office id assigned to filter QC
      this.masterModel.qcModelRequest.locationIdList = this.model.officeIds;
    }

    this.masterModel.qcList = [];
    this.model.qcIds = [];

    this.getQCData();
  }

  // delete button visible
  isDeleteButtonVisible() {
    if (this.model.items.filter(x => x.isDeleteRow) &&
      this.model.items.filter(x => x.isDeleteRow).length > 0) {
      this.masterModel.isQCBlockSelected = true;
    }
    else {
      this.masterModel.isQCBlockSelected = false;
    }
  }

  //check box header
  changeCheckBoxSelectAll() {
    for (var i = 0; i < this.model.items.length; i++) {
      this.model.items[i].isDeleteRow = this.masterModel.selectAllCheckbox;
    }
    this.isDeleteButtonVisible();
  }
}
