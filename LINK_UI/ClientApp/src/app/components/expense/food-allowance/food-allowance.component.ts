import { is } from '@amcharts/amcharts4/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Reference } from '@angular/compiler/src/render3/r3_ast';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { CountryDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { FoodAllowanceEditModel, FoodAllowanceEditSummaryModel, FoodAllowanceModel, FoodAllowanceResult, FoodAllowanceSummaryModel } from 'src/app/_Models/expense/foodallowance.model';
import { FoodAllowanceService } from 'src/app/_Services/expense/foodAllowance.service';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-food-allowance',
  templateUrl: './food-allowance.component.html',
  styleUrls: ['./food-allowance.component.scss'],
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
export class FoodAllowanceComponent extends SummaryComponent<FoodAllowanceModel> {

  public componentDestroyed$: Subject<boolean> = new Subject;
  public summaryModel: FoodAllowanceSummaryModel;
  public model: FoodAllowanceModel;
  public isFilterOpen: boolean;
  public toggleFormSection: boolean = false;
  public modelRef: NgbModalRef;
  public editSummaryModel: FoodAllowanceEditSummaryModel;
  public editModel: FoodAllowanceEditModel;
  public popUpOpen: boolean = false;
  public idToDelete: number;
  pagesizeitems = PageSizeCommon;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, private refService: ReferenceService,
    toastr: ToastrService, private locationService: LocationService, private service: FoodAllowanceService, public modalService: NgbModal) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.initialize();
    this.validator.setJSON("expense/edit-foodallowance.valid.json");
    this.validator.setModelAsync(() => this.editModel);
  }

  getData(): void {
    this.getSearchData();
  }
  getPathDetails(): string {
    return "";
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  initialize() {
    this.summaryModel = new FoodAllowanceSummaryModel();
    this.model = new FoodAllowanceModel();
    this.getCountryListBySearch();
  }
  //fetch the country data with virtual scroll
  getCountryData() {
    this.summaryModel.countryRequest.searchText = this.summaryModel.countryInput.getValue();
    this.summaryModel.countryRequest.skip = this.summaryModel.countryList.length;

    this.summaryModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.summaryModel.countryList = this.summaryModel.countryList.concat(customerData);
        }
        this.summaryModel.countryLoading = false;
      }),
      error => {
        this.summaryModel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getCountryListBySearch() {
    this.summaryModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.summaryModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.summaryModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.summaryModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.summaryModel.countryList = data;
        this.summaryModel.countryLoading = false;
      });
  }

  IsDateValidationRequired(): boolean {

    let isOk = false;
    if (!this.model.startDate) {
      this.validator.isValid('startDate');
    }

    else if (this.model.startDate && !this.model.endDate) {
      this.validator.isValid('endDate');
    }
    return isOk;
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "startDate": {
        this.model.startDate = null;
        break;
      }
      case "endDate": {
        this.model.endDate = null;
        break;
      }
    }
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  reset() {
    this.model.countryId = null;
    this.model.startDate = null;
    this.model.endDate = null;
    this.model.noFound = false;
  }

  SearchDetails() {
    this.model.pageSize = this.summaryModel.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getSearchData();
  }

  getSearchData() {
    if (this.isSummaryFormValid()) {
      this.summaryModel.searchloading = true;

      this.service.getFoodAllowanceSummary(this.model)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {

          this.mapPageProperties(res);

          if (res.result == ResponseResult.Success) {
            this.model.items = res.data;
          }
          else {
            this.model.items = [];
            this.model.noFound = true;
          }
          this.summaryModel.searchloading = false;
        },
          error => {
            this.summaryModel.searchloading = false;
          })
    }
  }

  openNewPopUp(content, isEdit) {

    this.editSummaryModel = new FoodAllowanceEditSummaryModel();
    this.editModel = new FoodAllowanceEditModel();
    this.editSummaryModel.loading = true;
    if (!isEdit)
      this.getEditCountryListBySearch();
    this.getCurrencyList();

    this.editSummaryModel.loading = false;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.popUpOpen = true;

  }

  save() {

    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.editSummaryModel.saveloading = true;
      this.service.save(this.editModel)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          if (res.result == ResponseResult.Success) {
            this.showSuccess('Food Allowance', 'Save Successful');

          }
          else if (res.result == FoodAllowanceResult.AlreadyExists) {
            this.showError('FOOD_ALLOWANCE.LBL_FOOD_ALLOWANCE', 'FOOD_ALLOWANCE.MSG_ALREADY_EXISTS');
          }
          else {
            this.showError('FOOD_ALLOWANCE.LBL_FOOD_ALLOWANCE', 'FOOD_ALLOWANCE.LBL_SAVE_FAILED');
          }
          this.editSummaryModel.saveloading = false;
          if (this.popUpOpen) {
            this.modelRef.close();
            this.popUpOpen = false;
            this.validator.isSubmitted = false;
          }
          this.SearchDetails();
        },
          error => {
            this.showError('Food Allowance', 'Save failed');
            this.editSummaryModel.saveloading = false;
          });
    }
  }

  cancel() {
    if (this.popUpOpen) {
      this.validator.isSubmitted = false;
      this.modelRef.close();
    }
  }

  //fetch the country data with virtual scroll
  getEditCountryData() {
    this.editSummaryModel.countryRequest.searchText = this.editSummaryModel.countryInput.getValue();
    this.editSummaryModel.countryRequest.skip = this.editSummaryModel.countryList.length;

    this.editSummaryModel.countryLoading = true;
    this.locationService.getCountryDataSourceList(this.editSummaryModel.countryRequest).
      subscribe(customerData => {
        if (customerData && customerData.length > 0) {
          this.editSummaryModel.countryList = this.editSummaryModel.countryList.concat(customerData);
        }
        this.editSummaryModel.countryLoading = false;
      }),
      error => {
        this.editSummaryModel.countryLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 countries on load
  getEditCountryListBySearch() {

    if (this.editModel && this.editModel.countryId > 0) {
      this.editSummaryModel.countryRequest.countryIds.push(this.editModel.countryId);
    }

    this.editSummaryModel.countryInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.countryLoading = true),
      switchMap(term => term
        ? this.locationService.getCountryDataSourceList(this.editSummaryModel.countryRequest, term)
        : this.locationService.getCountryDataSourceList(this.editSummaryModel.countryRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.countryLoading = false))
      ))
      .subscribe(data => {
        this.editSummaryModel.countryList = data;
        this.editSummaryModel.countryLoading = false;
      });
  }

  getCurrencyList() {

    this.editSummaryModel.currencyLoading = true;

    this.refService.getCurrencyList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.editSummaryModel.currencyList = res.dataSourceList;
        }
        else {
          this.editSummaryModel.currencyList = [];
        }
        this.editSummaryModel.currencyLoading = false;
      },
        error => {
          this.editSummaryModel.currencyList = [];
          this.editSummaryModel.currencyLoading = false;
        })
  }

  editFoodAllowance(id, content) {

    this.service.editFoodAllowance(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.openNewPopUp(content, true);
          this.mapEditData(res.data[0]);
          this.getEditCountryListBySearch();
        }
        else {
          this.model.items = [];
          this.model.noFound = true;
        }
        this.editSummaryModel.saveloading = false;
      },
        error => {
          this.editSummaryModel.saveloading = false;
        })
  }

  mapEditData(response) {
    this.editModel = {
      id: response.id,
      countryId: response.countryId,
      startDate: response.startDate,
      endDate: response.endDate,
      currencyId: response.currencyId,
      foodAllowanceValue: response.foodAllowance
    };
  }

  delete() {
    this.editSummaryModel = new FoodAllowanceEditSummaryModel();
    this.editSummaryModel.loading = true;
    this.editSummaryModel.deleteLoading = true;
    this.service.deleteFoodAllowance(this.idToDelete)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == FoodAllowanceResult.Success) {
          this.showSuccess('FOOD_ALLOWANCE.LBL_FOOD_ALLOWANCE', 'COMMON.MSG_DELETE_SUCCESS');
        }
        else {
          this.showError('FOOD_ALLOWANCE.LBL_FOOD_ALLOWANCE', 'FOOD_ALLOWANCE.MSG_DELETE_FAILEd');
        }
        this.editSummaryModel.deleteLoading = false;
        this.modelRef.close();
        this.SearchDetails();
        this.editSummaryModel.loading = false;
      })
  }

  openDeletePopUp(content, id) {
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal" });
    this.idToDelete = id;
  }

  formValid(): boolean {
    let isOk = this.validator.isValid('endDate') &&
      this.validator.isValid('startDate')
      && this.validator.isValid('countryId')
      && this.validator.isValid('currencyId')
      && this.validator.isValid('foodAllowanceValue')

    return isOk;
  }

  IsEditDateValidationRequired(): boolean {

    let isOk = this.validator.isSubmitted ? true : false;
    if (!this.editModel.startDate) {
      this.validator.isValid('startDate');
    }

    else if (this.editModel.startDate && !this.editModel.endDate) {
      this.validator.isValid('endDate');
    }
    if (this.editModel.startDate && this.editModel.endDate) {
      isOk = true;
    }
    return isOk;
  }

  deleteWorks() {
    this.delete();
  }

  clearEditDateInput(controlName: any) {
    switch (controlName) {
      case "startDate": {
        this.editModel.startDate = null;
        break;
      }
      case "endDate": {
        this.editModel.endDate = null;
        break;
      }
    }
  }

  clearCountry() {
    this.editModel.countryId = null;
    this.editSummaryModel.countryRequest.countryIds = [];
    this.getEditCountryListBySearch();
  }

  isSummaryFormValid() {
    let isOk = true;
    if (this.model.startDate) {
      if (!this.model.endDate) {
        this.showWarning('Validation Error', 'FOOD_ALLOWANCE.MSG_END_DATE_REQUIRED');
        return isOk = false;
      }
      let sDate = new Date(this.model.startDate.year, this.model.startDate.month, this.model.startDate.day);
      let eDate = new Date(this.model.endDate.year, this.model.endDate.month, this.model.endDate.day);
      if (sDate > eDate) {
        this.showWarning('Validation Error', 'End date cannot be less than from date');
        return isOk = false;
      }
    }
    return isOk;
  }
}
