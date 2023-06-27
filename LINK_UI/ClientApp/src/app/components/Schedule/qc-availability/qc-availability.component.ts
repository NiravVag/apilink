import { Component, OnInit } from '@angular/core';
import { ScheduleModel,ScheduleZoneFilterMaster } from '../../../_Models/Schedule/schedulemodel';
import { SummaryComponent } from '../../common/summary.component';
import { ScheduleService } from '../../../_Services/Schedule/schedule.service';
import { Validator } from '../../common';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, tap, timestamp } from 'rxjs/operators';
import { NgbCalendar, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, VirtualTimeScheduler } from 'rxjs';
import { PageSizeCommon } from '../../common/static-data-common';
import { CommonOfficeZoneSourceRequest } from '../../../_Models/common/common.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
 

@Component({
  selector: 'app-qc-availability',
  templateUrl: './qc-availability.component.html',
  styleUrls: ['./qc-availability.component.scss']
})
export class QcAvailabilityComponent extends SummaryComponent<ScheduleModel>{
  
  exportDataLoading: boolean;
  private currentRoute: Router;
  public data: any;
  public searchData: any;
  public locationNames: any;
  public distinctDates: any;
  public staffList: any;
  public loading: boolean = false;
  public popUpLoading: boolean = false;
  public officeLoading: boolean = false;
  public officePlaceHolder: string;
  public modelNotFound: boolean = false;
  public mandayZoneFilterRequest: ScheduleZoneFilterMaster;
  public zoneRequest: CommonOfficeZoneSourceRequest;

  modelRef: any;
  pagesizeitems = PageSizeCommon;
  selectedPageSize;
  getData(): void {
    
  }
  getPathDetails(): string {
    return 'schedule/qc-availability';
  }

  
    constructor(private service: ScheduleService, public validator: Validator, router: Router,
      public utility: UtilityService,
    route: ActivatedRoute, public pathroute: ActivatedRoute,translate: TranslateService, public modalService: NgbModal,
    toastr: ToastrService, public calendar: NgbCalendar) {
    super(router, validator, route,translate,toastr);
    this.currentRoute = router;
  }

  onInit(): void {
    this.model = new ScheduleModel();
    this.selectedPageSize = PageSizeCommon[0];
    this.Initialize();
  }

  Initialize() {
    this.officeLoading = true;
    this.model.pageSize = 10;
    this.model.index = 0;
    this.model.fromdate = this.calendar.getToday();
    this.model.todate = this.calendar.getNext(this.calendar.getToday(), 'd', 20);
    this.mandayZoneFilterRequest = new ScheduleZoneFilterMaster();
   
    this.zoneRequest = new CommonOfficeZoneSourceRequest();
    this.officePlaceHolder = "All";
    this.getZoneListBySearch();
setTimeout(() => {
  this.GetSearchData();
}, 500);
  
    
  }

  IsDateValidationRequired(): boolean {
    return this.validator.isSubmitted && this.model.searchtypetext != null && this.model.searchtypetext.trim() == "" ? true : false;
  }
  getZoneData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.zoneRequest.searchText = this.mandayZoneFilterRequest.zoneInput.getValue();
      this.zoneRequest.skip = this.mandayZoneFilterRequest.zoneList.length;
    }
    this.zoneRequest.officeIds = this.model.officeidlst;
    this.mandayZoneFilterRequest.zoneLoading = true;
    this.service.getOfficeZoneDataSourceList(this.zoneRequest).
      subscribe(zoneData => {
        if (zoneData && zoneData.length > 0) {
          this.mandayZoneFilterRequest.zoneList = this.mandayZoneFilterRequest.zoneList.concat(zoneData);
        }
        if (isDefaultLoad)
          this.zoneRequest = new CommonOfficeZoneSourceRequest();
          this.mandayZoneFilterRequest.zoneLoading = false;
      }),
      error => {
        this.mandayZoneFilterRequest.zoneLoading = false;
        this.setError(error);
      };
  }
  // office changes to get zone
  getZoneChanges(zoneitem) {
    this.mandayZoneFilterRequest.zoneList=[];
    if(zoneitem.length<=0)
    {
      this.getZoneListBySearch();
      return;
    }
    this.model.officeidlst=zoneitem;
    this.getZoneData(false);
  }
  
  //fetch the first 10 Zones on load
  getZoneListBySearch() {
    this.mandayZoneFilterRequest.zoneInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.mandayZoneFilterRequest.zoneLoading = true),
      switchMap(term => term
        ? this.service.getZoneDataSourceList(this.zoneRequest, term)
        : this.service.getZoneDataSourceList(this.zoneRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.mandayZoneFilterRequest.zoneLoading = false))
      ))
      .subscribe(data => {
        this.mandayZoneFilterRequest.zoneList = data;
        this.mandayZoneFilterRequest.zoneLoading = false;
      });
  }
  
  GetSearchData() {
    this.loading = true;
    this.model.pageSize = this.selectedPageSize;
    this.service.getMandayForecast(this.model)
      .pipe()
      .subscribe(
        res => {
          if (res.result == 1) {
            this.searchData = res.data;
            this.searchData.length == 0 ? this.modelNotFound = true : this.modelNotFound = false;

            this.locationNames = res.locationName;
            this.data = res.dataSourceList;
            res.dataSourceList.length > 0 ? this.officePlaceHolder = "All" : this.officePlaceHolder = "No Data";
            this.distinctDates = this.searchData.filter(
              (thing, i, arr) => arr.findIndex(t => t.date === thing.date) === i
            );
            this.loading = false;
            this.officeLoading = false;
          }
          else {
            this.error = res.result;
            this.loading = false;
          }
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  GetMandayValue(date: string, location: any) {

    var data =  this.searchData.find(x => x.date == date && x.location == location.name);

    if(data == undefined)
    return null;

    return data.manDaycount + "/" + data.availableQcCount;
  }

  GetQconLeaveDetails(date: string, location: any) {

    var data =  this.searchData.find(x => x.date == date && x.location == location.name);

    if(data == undefined || data.qcOnLeaveCount == 0)
    return null;
    
    return " (" + data.qcOnLeaveCount + ")";

  }

  GetColor(date: string, location: any) {

    var data =  this.searchData.find(x => x.date == date && x.location == location.name);

    if(data == undefined)
    return null;

    return data.color;

  }

  OpenPopUp(content, date, location) {
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' }); 
    this.popUpLoading = true;

    this.service.getLeaveDetails(date.split('/').join('-'), location.officeId,location.zoneId)
      .pipe()
      .subscribe(
        res => {
          if (res.result == 1) {
            this.staffList = res.data;
            this.popUpLoading = false;
          }
          else {
            this.error = res.result;
            this.popUpLoading = false;
          }
        },
        error => {
          this.setError(error);
          this.popUpLoading = false;
        });
  }
  clearDateInput(controlName:any){
    switch(controlName) {
      case "Fromdate": { 
        this.model.fromdate=null;
        break; 
     } 
     case "Todate": { 
      this.model.todate=null;
        break; 
     } 
    }
  }
  export() {
    this.exportDataLoading = true;
    this.service.exportMandayForecast(this.model)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "Manday_Forecast.xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = "Manday_Forecast.xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }
}
