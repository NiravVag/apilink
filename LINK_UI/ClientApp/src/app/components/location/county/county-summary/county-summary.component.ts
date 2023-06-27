import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocationService } from '../../../../_Services/location/location.service'
import { CountySummaryModel, CountySummaryItemModel } from '../../../../_Models/location/countysummarymodel'
import { first } from 'rxjs/operators';
import { PageSizeCommon, Country } from '../../../common/static-data-common'
import { Validator } from "../../../common/validator"
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from "@ngx-translate/core";
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-county-summary',
  templateUrl: './county-summary.component.html',
  styleUrls: ['./county-summary.component.scss'],
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
export class CountySummaryComponent extends SummaryComponent<CountySummaryModel> {
  onInit(): void {
    this.Intialize();
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "county/edit-county";
  }
  public model: CountySummaryModel;
  public modelRemove: CountySummaryItemModel;
  public data: any;
  error: string = '';
  loading: boolean = false;
  searchloading: boolean = false;
  provinceLoading: boolean = false;
  cityLoading: boolean = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  public modelRef: NgbModalRef;
  cityList: Array<any>[];
  countyList: Array<any>[];
  provinceList: Array<any>[];
  _country = Country;
  isFilterOpen: boolean;
  constructor(private service: LocationService, 
    public utility: UtilityService,
    public validator: Validator, router: Router, route: ActivatedRoute
    , public modalService: NgbModal,translate: TranslateService) {
    super(router, validator, route,translate);
    this.model = new CountySummaryModel();
    this.modelRemove = new CountySummaryItemModel();
    this.validator.setJSON("location/county-summary.valid.json");
    this.validator.isSubmitted = false;
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  Intialize() {
    this.loading = false;
    this.getCountry();
  }

  getCountry() {
    this.data = this.service.getCountrySummary()
      .pipe()
      .subscribe(
        resultdata => {
          if (resultdata && resultdata.result == 1) {
            if (resultdata && resultdata.result == 1) {
              this.data = resultdata;
            }
            else {
              this.error = resultdata.result;
            }
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        }
      );
  }
  refreshprovince(countryid) {
    if (countryid) {
      this.provinceLoading = true;
      //this.Reset();
      this.service.getprovincebycountryid(countryid.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.provinceList = result.data;
            }
            else {
              this.provinceList = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.provinceList = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.provinceList = [];
    }
  }
  refreshcity(provinceId) {
    this.ResetProvince();
    if (provinceId) {
      this.cityLoading = true;
      this.service.getcitybyprovinceid(provinceId.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.cityList = result.data;
            }
            else {
              this.cityList = [];
            }
            this.cityLoading = false;
          },
          error => {
            this.cityList = [];
            this.cityLoading = false;
          });
    }
    else {
      this.cityList = [];
      this.model.CityValues = null;
    }
  }
  
  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.GetSearchData();
  }

  IsCountyValidationRequired(): boolean {  
    return this.validator.isSubmitted && this.model.CountyValues != null && this.model.CountyValues.trim() == "" ? true : false;
  }

  IsCityValidationRequired(): boolean {   
    return this.validator.isSubmitted && this.model.CityValues == null  ? true : false;
  }

  GetSearchData() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    this.model.noFound = false;
    if (this.IsFormvalid()) {
      this.searchloading = true;
      this.service.getCountySearchSummary(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1 && response.data.length > 0) {
              this.mapPageProperties(response);
              this.model.items = response.data.map((x) => {
                var tabItem: CountySummaryItemModel = {
                  id: x.id,
                  country: x.countryName,
                  countyName: x.countyName,
                  countyCode: x.countyCode,
                  cityName: x.cityName,
                  provinceName: x.provinceName
                }
                return tabItem;
              });
            }
            else if (response && response.result == 2) {
              this.model.items=[];
              this.model.noFound = true;
            }
            else {
              this.error = response.result;
            }
            this.searchloading = false;
          },
          error => {
            this.setError(error);
            this.searchloading = false;
          });
    }

  }
  openConfirm(id, countyname, content) {

    this.modelRemove.id = id;
    this.modelRemove.countyName = countyname;

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  DeleteCounty(id) {
    this.service.deleteCounty(id).pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.refresh();
            this.showSuccess('COUNTY_SUMMARY.TITLE', 'COUNTY_SUMMARY.MSG_DELETE_SUCCESS')
          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });

    this.modelRef.close();
  }

  IsFormvalid(): boolean {
    if (!this.model.CityValues && !this.model.CountyValues) {
      return this.validator.isValid('CityValues')
    }
    else return true;
  }
  Reset() {
    this.model.ProvinceValues = null;
    this.provinceList = null;
    this.model.CityValues = null;
    this.cityList = null;
    this.model.CountyValues = "";
  }
  ResetProvince() {
    this.model.CityValues = null;
    this.cityList = null;
    this.model.CountyValues = "";
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}

