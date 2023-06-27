import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceItem, InvoiceSummaryModel, InvoiceSummaryRequestModel } from 'src/app/_Models/invoice/invoicesummary.model';
import { Validator } from '../../common';
import { SummaryComponent } from '../../common/summary.component';

import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Country, PageSizeCommon } from '../../common/static-data-common';
import { TravelTariffService } from 'src/app/_Services/traveltariff/traveltariff.service';
import { TravelTariffDeleteResponse, TravelTariffDeleteResult, TravelTariffGetAllResponse, TravelTariffGetAllResult, TravelTariffSearchRequest, TravelTariffSummary } from 'src/app/_Models/traveltariff/traveltariff';
import { LocationService } from 'src/app/_Services/location/location.service';
import { ResponseResult } from 'src/app/_Models/common/common.model';

@Component({
  selector: 'app-travel-tariff-summary',
  templateUrl: './travel-tariff.component.html',
  styleUrls: ['./travel-tariff.component.scss'],
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


export class TravelTariffComponent extends SummaryComponent<TravelTariffSearchRequest>{

  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService; 
  public model: TravelTariffSearchRequest;
  public masterModel:TravelTariffSummary;
  public pageLoader:boolean=false;
  public travelTariffId:number;
  public isFilterOpen: boolean;
  selectedPageSize;
  pagesizeitems = PageSizeCommon;

  _country = Country;


  constructor(public validator: Validator, router: Router, route: ActivatedRoute,    
              translate: TranslateService, toastr: ToastrService,private locationService: LocationService, 
              public utility: UtilityService, public service :TravelTariffService,public modalService: NgbModal) 
     {
       super(router, validator, route, translate, toastr);
       this._toastr = toastr;
       this._translate = translate;  
       this.masterModel=new TravelTariffSummary();
       this.isFilterOpen=true;
     }

  
  getData(): void {
    this.GetSearchData();
  }

  getPathDetails(): string {
    return "edittraveltariff/edit-travel-tariff";
  }

  getEditDetails(id) {    
    this.getDetails(id);    
}

  onInit() { 
    this.pageLoader=true;
    this.model=new TravelTariffSearchRequest();
    this.model.pageSize=PageSizeCommon[0];
    this.selectedPageSize = PageSizeCommon[0];
    this.model.index=0;
    this.getStartportList();
    this.getCountry();
    this.pageLoader=false;
  }

 async GetSearchData() { 
  this.pageLoader=true;
   var response= await this.service.getAllTravelTariff(this.model);
  
        if (response) {
  
        this.mapPageProperties(response);
  
          switch (response.result) {
            case TravelTariffGetAllResult.Success:
              this.model.items=response.travelTariffDetails;
              break;              
              case TravelTariffGetAllResult.NotFound:
                this.model.items=[];
                this.model.noFound=true;
                break;   
              case TravelTariffGetAllResult.Failure:
                this.model.items=[];
                this.model.noFound=true;
                break;         
          }      
        }    
        this.pageLoader=false;

     
    }

    
  SearchDetails() {
      
       this.model.pageSize = this.selectedPageSize;
       this.GetSearchData()
  }
       
             //get startport list
             async getStartportList() {
               this.masterModel.startPortLoading=true;
              var response= await this.service.getStartPortList();           
               if (response && response.result == ResponseResult.Success) {  
                 this.masterModel.startPortList = response.dataSourceList; 
              }
              this.masterModel.startPortLoading=false;
             
          }

    export() {
      this.pageLoader=true;
      this.service.exportSummary(this.model)
        .subscribe(res => {
          this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        },
          error => {
            this.pageLoader=false;
          });
    }

    toggleFilterSection() {
      this.isFilterOpen = !this.isFilterOpen;
    }

    downloadFile(data, mimeType) {
      let windowNavigator: any = window.Navigator;
      const blob = new Blob([data], { type: mimeType });
      if (windowNavigator && windowNavigator.msSaveOrOpenBlob) 
      {
        windowNavigator.msSaveOrOpenBlob(blob, "Travel_tariff_summary.xlsx");
      }
      else 
      {
        const a = document.createElement('a');
        const url = window.URL.createObjectURL(blob);
        a.download = "Travel_tariff_summary.xlsx";
        a.href = url;
        a.click();
      }
      this.pageLoader=false;
    }

    confirmDelete(content,id) {

      this.travelTariffId=id; 
  
      this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true,backdrop: 'static' });
      this.modelRef.result.then((result) => { 
 
  
      }, (reason) => {
      });
    }

    async deleteTravelTariff() {
      this.masterModel.deleteLoading=true;
      let response:TravelTariffDeleteResponse
       try 
        {
         if(this.travelTariffId)
         {
           response = await this.service.deleteTravelTariff(this.travelTariffId);     
           this.masterModel.deleteLoading=false;     
         }         
          switch (response.result) 
          {
            case TravelTariffDeleteResult.Success:      
            this.modelRef.close();      
            this.showSuccess('EDIT_TRAVEL_TARIFF.TITLE', 'EDIT_TRAVEL_TARIFF.MSG_DELETE_SUCCESS');
            this.onInit();  
              break;       
          
            default:
              break;
          }
          this.masterModel.deleteLoading=false;  
        }
        catch (e) {
          this.setError(e);
          this.masterModel.deleteLoading=false;  
        }   
   
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

     getCountry() {
      this.masterModel.countryLoading = true;
      this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        result => {
          this.masterModel.countryList = result.countryList;
          this.masterModel.countryLoading = false;
        }
      )
    }
    refreshprovince(countryid) {
      this.resetDetails();
     this.getProvince(countryid);
    }
    getProvince(countryid: number){
      if (countryid) {
        this.masterModel.provinceLoading = true;
        this.locationService.getprovincebycountryid(countryid)
          .pipe()
          .subscribe(
            result => {
              if (result && result.result == 1) {
                this.masterModel.provinceList= result.data;
              }
              else {
                this.masterModel.provinceList = [];
              }
              this.masterModel.provinceLoading = false;
            },
            error => {
              this.masterModel.provinceList = [];
              this.masterModel.provinceLoading = false;
            });
      }
      else {
        this.masterModel.provinceList = [];
      }
    }

    resetDetails() {
      this.model.provinceId = null;
      this.model.cityId = null;
      this.model.countyId = null;
      this.model.townId = null;
      this.masterModel.cityList=null;
      this.masterModel.countyList=null;
    }

    refreshcity(provinceId) {
      this.resetProvince();
      if (provinceId) {
        this.masterModel.cityLoading = true;
        this.locationService.getcitybyprovinceid(provinceId)
          .pipe()
          .subscribe(
            result => {
              if (result && result.result == 1) {
                this.masterModel.cityList = result.data;
              }
              else {
                this.masterModel.cityList = [];
              }
              this.masterModel.cityLoading = false;
            },
            error => {
              this.masterModel.cityList = [];
              this.masterModel.cityLoading = false;
            });
      }
      else {
        this.masterModel.cityList = [];
      }
    }

    resetProvince() {
      this.model.cityId = null;
      this.model.countyId = null;
      this.model.townId = null;
      this.masterModel.countyList=null;
    }

    refreshcounty(cityId) {
      this.model.countyId = null;
      this.model.townId = null;
      if (cityId) {
        this.masterModel.countyLoading = true;
        this.locationService.getcountybycity(cityId)
          .pipe()
          .subscribe(
            result => {
              if (result && result.result == 1) {
                this.masterModel.countyList = result.dataList;
              }
              else {
                this.masterModel.countyList = [];
              }
              this.masterModel.countyLoading = false;
            },
            error => {
              this.masterModel.countyList = [];
              this.masterModel.countyLoading = false;
            });
      }
      else {
        this.masterModel.countyList = [];
      }
    }

    refreshTown(countyId) {   
      this.model.townId = null;
      if (countyId) {
        this.masterModel.townLoading = true;
        this.locationService.getTownsByCountyId(countyId)
          .pipe()
          .subscribe(
            result => {
              if (result && result.result == 1) {
                this.masterModel.townList = result.dataSourceList;
              }
              else {
                this.masterModel.townList = [];
              }
              this.masterModel.townLoading = false;
            },
            error => {
              this.masterModel.townList = [];
              this.masterModel.townLoading = false;
            });
      }
      else {
        this.masterModel.townList = [];
      }
    }



 
}

