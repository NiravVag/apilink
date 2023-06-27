import { Component, OnInit, TemplateRef } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { LocationService } from '../../../../_Services/location/location.service'
import { first } from 'rxjs/operators';
import { PageSizeCommon, Country } from '../../../common/static-data-common'
import { Validator } from "../../../common/validator"
import { SummaryComponent } from '../../../common/summary.component';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TownSummaryModel, TownSummaryItemModel } from 'src/app/_Models/location/townsummarymodel';
import { ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-town-summary',
  templateUrl: './town-summary.component.html',
  styleUrls: ['./town-summary.component.scss'],
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
export class TownSummaryComponent extends SummaryComponent<TownSummaryModel> {
  onInit(): void {
    this.Intialize();
  }
  getData(): void {
    this.GetSearchData();
  }
  getPathDetails(): string {
    return "town/edit-town";
  }

  public model: TownSummaryModel;
  public modelRemove: TownSummaryItemModel;
  public data: any;
  error: string = '';
  loading: boolean = false;
  provinceLoading: boolean = false;
  searchloading: boolean = false;
  cityLoading: boolean = false;
  countyLoading: boolean = false;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  public modelRef: NgbModalRef;
  provinceList: any[];
  cityList: any[];
  countyList: any[];
  town: string;
  _country = Country;
  isFilterOpen: boolean;
  constructor(private service: LocationService, 
    public utility: UtilityService,
    public validator: Validator, router: Router, route: ActivatedRoute
    , public modalService: NgbModal,translate: TranslateService) {
    super(router, validator, route,translate);
    this.model = new TownSummaryModel();
    this.modelRemove = new TownSummaryItemModel();
    this.validator.setJSON("location/town-summary.valid.json");
    this.validator.isSubmitted = false;
    this.validator.setModelAsync(() => this.model);
    this.selectedPageSize = PageSizeCommon[0];
    this.isFilterOpen = true;
  }

  Intialize() {
    this.loading = true;
    this.getCountry();
  }
  
  getCountry() {
    this.data = this.service.getCountryforCounty(this._country.China )
      .pipe()
      .subscribe(
        resultdata => {
          if (resultdata && resultdata.result == 1) {
            this.data = resultdata;
          }
          else {
            this.error = resultdata.result;
          }

          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        }
      );
  }
  refreshprovince(countryid) {
    this.resetDetails();
    if (countryid) {
      this.provinceLoading = true;
      this.model.provinceValues = null;
      this.model.cityValues = null;
      //this.countyList = null;
      this.service.getprovincebycountryid(countryid.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.data.provinceValues = result.data;
            }
            else {
              this.data.provinceValues = [];
            }
            this.provinceLoading = false;
          },
          error => {
            this.provinceList = [];
            this.provinceLoading = false;
          });
    }
    else {
      this.data.provinceValues = [];
    }
  }
  refreshcity(provinceId) {
    this.resetProvince();
    if (provinceId) {
      this.cityLoading = true;
      //this.resetCountryDetails();
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
    }
  }
  refreshcounty(cityId) {
    this.model.countyValues = null;
    this.model.townValues = null;
    if (cityId) {
      this.countyLoading = true;
      this.service.getcountybycity(cityId.id)
        .pipe()
        .subscribe(
          result => {
            if (result && result.result == 1) {
              this.countyList = result.dataList;
            }
            else {
              this.countyList = [];
            }
            this.countyLoading = false;
          },
          error => {
            this.countyList = [];
            this.countyLoading = false;
          });
    }
    else {
      this.countyList = [];
    }
  }
  

  IsTownValidationRequired(): boolean {   
    return this.validator.isSubmitted && this.model.townValues != null && this.model.townValues.trim() == "" ? true : false;
  }

  IsCityValidationRequired(): boolean {  
    return this.validator.isSubmitted && this.model.cityValues == null  ? true : false;
  }

  SearchDetails() {
    this.model.pageSize = this.selectedPageSize;
    this.GetSearchData();
    
  }
  GetSearchData() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if(!this.model.townValues){
      if(this.Formvalid()==false)
        return false;
    }
      this.searchloading = true;
      this.service.getTownSearchSummary(this.model)
        .pipe()
        .subscribe(
          response => {
            if (response && response.result == 1 && response.dataList.length > 0) {
              this.mapPageProperties(response);
              this.model.items = response.dataList.map((x) => {
                var tabItem: TownSummaryItemModel = {
                  id: x.id,
                  country: x.countryName,
                  province: x.provinceName,
                  city: x.cityName,
                  county: x.countyName,
                  townName: x.townName,
                  townCode: x.townCode
                }
                return tabItem;
              });
            }
            else if (response && response.result == 2) {
              this.model.items = [];
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
  openConfirm(id, townName, content) {

    this.modelRemove.id = id;
    this.modelRemove.townName = townName;

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }
  DeleteTown(id) {
    this.service.deleteTown(id).pipe()
      .subscribe(
        response => {
          if (response && (response.result == 1)) {
            this.refresh();
            this.showSuccess('TOWN_SUMMARY.TITLE', 'TOWN_SUMMARY.MSG_DELETE_SUCCESS')
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
  resetDetails() {
    this.model.countryValues = null;
    this.model.provinceValues = null;
    this.model.cityValues = null;
    this.model.countyValues = null;
    this.model.townValues = "";
    this.cityList=null;
    this.countyList=null;
  }
  resetProvince() {
    this.model.cityValues = null;
    this.model.countyValues = null;
    this.model.townValues = "";
    this.countyList=null;
  }
  Formvalid(): boolean { 
    if(!this.model.cityValues && !this.model.townValues){
    return this.validator.isValid('cityValues')}
      else return true;
  }
  toggleFilterSection() {
		this.isFilterOpen = !this.isFilterOpen;
  }
}
