import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CityDataSourceRequest, ResponseResult } from '../../../_Models/common/common.model';
import { StartingPortEditModel, StartingPortEditSummaryModel, StartingPortModel, StartingPortResult, StartingPortSummaryModel } from '../../../_Models/expense/startingport.model';
import { StartingPortService } from '../../../_Services/expense/startingport.service';
import { LocationService } from '../../../_Services/location/location.service';
import { TravelTariffService } from '../../../_Services/traveltariff/traveltariff.service';
import { PageSizeCommon } from '../../common/static-data-common';
import { SummaryComponent } from '../../common/summary.component';
import { Validator } from '../../common/validator';
import { StartingPortModule } from './starting-port.module';

@Component({
  selector: 'app-starting-port',
  templateUrl: './starting-port.component.html',
  styleUrls: ['./starting-port.component.scss'],
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
export class StartingPortComponent extends SummaryComponent<StartingPortModel> {

  public componentDestroyed$: Subject<boolean> = new Subject;
  public summaryModel: StartingPortSummaryModel;
  public model: StartingPortModel;
  public isFilterOpen: boolean;
  public toggleFormSection: boolean = false;
  public modelRef: NgbModalRef;
  public editSummaryModel: StartingPortEditSummaryModel;
  public editModel: StartingPortEditModel;
  public popUpOpen: boolean = false;
  public idToDelete: number;
  pagesizeitems = PageSizeCommon;
  
  selectedPageSize: any;

  constructor(public validator: Validator, router: Router, route: ActivatedRoute, translate: TranslateService, private locationService: LocationService,
    toastr: ToastrService, private service: StartingPortService, public modalService: NgbModal, private travelTariffService: TravelTariffService) {
    super(router, validator, route, translate, toastr);
    this.isFilterOpen = true;
  }

  onInit(): void {
    this.initialize();
    this.validator.setJSON("expense/edit-starting-port.valid.json");
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
    this.summaryModel = new StartingPortSummaryModel();
    this.model = new StartingPortModel();

    this.model.pageSize=PageSizeCommon[0];
    this.model.index=0;
    this.selectedPageSize = PageSizeCommon[0];
    
    this.getStartportList();
  }

  async getStartportList() {
    this.summaryModel.startingPortNameListLoading = true;
    var response = await this.travelTariffService.getStartPortList();
    if (response && response.result == ResponseResult.Success) {
      this.summaryModel.startingPortNameList = response.dataSourceList;
    }
    this.summaryModel.startingPortNameListLoading = false;
  }
  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }

  reset() {
    this.model.startingPortId = null;    
    this.model.noFound = false;
  }

  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.model.index = 1;
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.getSearchData();
  }

  async getSearchData() {
    this.summaryModel.searchloading = true;

    let res = await this.service.getAllStartingPort(this.model);
    if (res) {
      this.mapPageProperties(res);      
    
      if (res.result == StartingPortResult.Success) {
        this.model.items = res.data;
      }
      else {
        this.model.items = [];
        this.model.noFound = true;
      }
    }
    this.summaryModel.searchloading = false;
  }

  openNewPopUp(content, isEdit) {
    this.validator.isSubmitted = false;
    this.editSummaryModel = new StartingPortEditSummaryModel();
    this.editModel = new StartingPortEditModel();
    this.editSummaryModel.loading = true;
    if (!isEdit) {
      this.getCityListBySearch();
      this.editSummaryModel.showSaveButton = false;
    }
    else {
      this.editSummaryModel.showSaveButton = true;
    }
    this.editSummaryModel.loading = false;
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
    this.popUpOpen = true;

  }

  async save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if (this.formValid()) {
      this.editSummaryModel.saveloading = true;
      let res = await this.service.saveStartingPort(this.editModel)
      if (res.result == StartingPortResult.Success) {
        this.showSuccess('START_PORT.LBL_TITLE', 'COMMON.MSG_SAVED_SUCCESS');

      }
      else if (res.result == StartingPortResult.AlreadyExists) {
        this.showError('START_PORT.LBL_TITLE', 'START_PORT.MSG_ALREADY_EXISTS');
      }
      else {
        this.showError('START_PORT.LBL_TITLE', 'START_PORT.LBL_SAVE_FAILED');
      }
      this.editSummaryModel.saveloading = false;
      if (this.popUpOpen) {
        this.modelRef.close();
        this.popUpOpen = false;
        this.validator.isSubmitted = false;
      }
      this.SearchDetails();
      this.getStartportList();
    }
  }

  cancel() {
    if (this.popUpOpen) {
      this.validator.isSubmitted = false;
      this.modelRef.close();
    }
  }

  async editStartingPort(id, content) {

    let res = await this.service.getStartingPort(id)
    if (res.result == StartingPortResult.Success) {
      this.openNewPopUp(content, true);
      this.mapEditData(res.data);
      this.getCityListBySearch();
    }
    else {
      this.model.items = [];
      this.model.noFound = true;
    }
    this.editSummaryModel.saveloading = false;

  }

  mapEditData(response) {
    this.editModel = {
      id: response.startingPortId,
      cityId: response.cityId > 0 ? response.cityId : null,
      startPortName: response.startingPortName
    };
  }

  async delete() {
    this.editSummaryModel = new StartingPortEditSummaryModel();
    this.editSummaryModel.loading = true;
    this.editSummaryModel.deleteLoading = true;
    let res = await this.service.deleteStartingPort(this.idToDelete)
    if (res.result == StartingPortResult.Success) {
      this.showSuccess('START_PORT.LBL_TITLE', 'COMMON.MSG_DELETE_SUCCESS');
    }
    else {
      this.showError('START_PORT.LBL_TITLE', 'START_PORT.MSG_DELETE_FAILEd');
    }
    this.editSummaryModel.deleteLoading = false;
    this.modelRef.close();
    this.SearchDetails();
    this.getStartportList();
    this.editSummaryModel.loading = false;

  }

  openDeletePopUp(content, id) {
    this.modelRef = this.modalService.open(content, { windowClass: "confirmModal", backdrop: 'static'});
    this.idToDelete = id;
  }

  formValid(): boolean {
    let isOk =
      this.validator.isValid('startPortName')
      && this.validator.isValid('cityId');

    return isOk;
  }

  //fetch city dropdown list
  getCityListBySearch() {

    if (this.editModel.cityId > 0) {
      this.editSummaryModel.cityRequest.cityIds.push(this.editModel.cityId);
    }
    else {
      this.editSummaryModel.cityRequest.cityIds = [];
    }

    this.editSummaryModel.cityInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.editSummaryModel.cityLoading = true),
      switchMap(term => term
        ? this.locationService.getCityDataSourceList(this.editSummaryModel.cityRequest, term)
        : this.locationService.getCityDataSourceList(this.editSummaryModel.cityRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.editSummaryModel.cityLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.editSummaryModel.cityList = data;
        this.editSummaryModel.cityLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getCityData() {

    this.editSummaryModel.cityRequest.searchText = this.editSummaryModel.cityInput.getValue();
    this.editSummaryModel.cityRequest.skip = this.editSummaryModel.cityList.length;

    this.editSummaryModel.cityLoading = true;

    this.locationService.getCityDataSourceList(this.editSummaryModel.cityRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.editSummaryModel.cityList = this.editSummaryModel.cityList.concat(data);
        }
        this.editSummaryModel.cityRequest = new CityDataSourceRequest();
        this.editSummaryModel.cityLoading = false;
      }),
      error => {
        this.editSummaryModel.cityLoading = false;
        this.setError(error);
      };
  }

  clearCity() {
    this.getCityListBySearch();
  }

  startPortChange(item) {
    if (item) {
      this.editSummaryModel.showSaveButton = true;
    }
    else {
      this.editSummaryModel.showSaveButton = false;
    }
  }

  deleteRedirect(){
    this.delete();
  }

}

