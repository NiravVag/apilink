
import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { Validator } from "../../common/validator"
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TemplateSummaryModel, KpiTemplateListResponse, KpiTemplateListResult, KpiTemplateListRequest, DeleteTemplateResponse, DeleteTemplateResult } from '../../../_Models/kpi/template.model';
import { ModuleItem, ModuleListResponse, ModuleListResult } from '../../../_Models/kpi/module.model';
import { KpiService } from '../../../_Services/kpi/kpi.service';
import { CustomerService } from '../../../_Services/customer/customer.service'
import { first, retry, debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { InternalBusinessModel, EventProgress } from '../../../_Models/kpi/template.view.model';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions, LogLevel } from '@microsoft/signalr'; 
import { CommonCustomerSourceRequest } from '../../../_Models/common/common.model';
import { BehaviorSubject, of } from 'rxjs';
import { CustomerBrandService } from '../../../_Services/customer/customerbrand.service';
import { CustomerDepartmentService } from '../../../_Services/customer/customerdepartment.service';
import { DefaultDateType, KPIPPTtypelst } from '../../common/static-data-common';
import { number } from '@amcharts/amcharts4/core';


@Component({
  selector: 'app-internal-business',
  templateUrl: './internalbusiness.component.html',
  styleUrls: ['./internalbusiness.component.css']
})

export class InternalBusinessComponent implements OnInit{

  public model: InternalBusinessModel = {
    idCustomer: null,
    beginDate: null,
    endDate : null,
    loadDepartment: false,
    loadFactory: false,
    customer: null,
    brandList: [],
    deptList: [],
    searchDateTypeId: DefaultDateType.ServiceDate
  };

  public customerList: Array<any>;
  public loading: boolean = false;
  public downloading: boolean = false; 
  public error: string = "";
  public  hubConnection: HubConnection;
  private baseUrl: string;
  public progressPercent: number = 0;
  public progressMessage: string; 
  public brandSearchRequest: CommonCustomerSourceRequest;
  public deptSearchRequest: CommonCustomerSourceRequest;
  public brandInput: BehaviorSubject<string>;
  public deptInput: BehaviorSubject<string>;
  public brandList: Array<any>;
  public deptLoading: boolean;
  public brandLoading: boolean;
  public deptList: Array<any>;
  invoiceDateTypeList: any = KPIPPTtypelst;

  constructor(private service: KpiService, public validator: Validator, router: Router, 
    route: ActivatedRoute, private authserve: AuthenticationService, public pathroute: ActivatedRoute,
    private translate: TranslateService, private toastr: ToastrService, private modalService: NgbModal,
    private customerService: CustomerService, @Inject('BASE_URL') baseUrl: string, private brandService: CustomerBrandService, private deptService: CustomerDepartmentService) {
    this.baseUrl = baseUrl;
  }


  ngOnInit() {
    
    this.model.searchDateTypeId = DefaultDateType.ServiceDate;
    this.validator.isSubmitted = false;
    this.validator.setJSON("kpi/internalbusiness.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.getCustomerList();
    this.brandSearchRequest = new CommonCustomerSourceRequest();
    this.deptSearchRequest = new CommonCustomerSourceRequest();
    this.brandInput = new BehaviorSubject<string>("");
    this.deptInput = new BehaviorSubject<string>("");
    this.brandList = [];
    this.deptList = [];
    this.deptLoading = false; 
    this.brandLoading = false; 
  }


  getCustomerList() {
    console.log('**** getCustomerList ')
    this.customerService.getCustomerSummary()
      .pipe()
      .subscribe(
      response => {
        console.log('**** response ', response)
          if (response && response.result == 1) {
            this.customerList = response.customerList;
          }
          else {
            this.error = response.result;
          }
          this.loading = false;

        },
        error => {
          //this.setError(error);
          this.loading = false;
        });
  }

  changeCustomer(cusitem) {
    if (cusitem != null && cusitem.id != null) {
      this.model.customer = cusitem;
      this.brandSearchRequest.customerId = cusitem.id;
      this.deptSearchRequest.customerId = cusitem.id;
      this.getBrandListBySearch();
      this.getDeptListBySearch();
    }
    else{
    this.model.brandList = []; 
    this.model.deptList = []; 
    this.model.customer = null;
    }
  }

  getBrandListBySearch() {
    this.brandInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.brandLoading = true),
      switchMap(term => term
        ? this.brandService.getBrandListByCustomerId(this.brandSearchRequest, term)
        : this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.brandLoading = false))
      ))
      .subscribe(data => {
        this.brandList = data;
        this.brandLoading = false;
      });
  }

  getDeptListBySearch() {
    this.deptInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.deptLoading = true),
      switchMap(term => term
        ? this.deptService.getDeptListByCustomerId(this.deptSearchRequest, term)
        : this.deptService.getDeptListByCustomerId(this.deptSearchRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.deptLoading = false))
      ))
      .subscribe(data => {
        this.deptList = data;
        this.deptLoading = false;
      });
  }


  //fetch the brand data with virtual scroll
  getDeptData() {
    this.deptSearchRequest.searchText = this.deptInput.getValue();
    this.deptSearchRequest.skip = this.deptList.length;

    this.deptLoading = true;
    this.deptService.getDeptListByCustomerId(this.deptSearchRequest).
      subscribe(deptData => {
        if (deptData && deptData.length > 0) {
          this.deptList = this.deptList.concat(deptData);
        }
        this.deptSearchRequest = new CommonCustomerSourceRequest();
        this.deptLoading = false;
      }),
      error => {
        this.deptLoading = false;
        //this.setError(error);
      };
  }

  getBrandData() {
    this.brandSearchRequest.searchText = this.brandInput.getValue();
    this.brandSearchRequest.skip = this.brandList.length;

    this.brandLoading = true;
    this.brandService.getBrandListByCustomerId(this.brandSearchRequest)
      .subscribe(brandData => {
        if (brandData && brandData.length > 0) {
          this.brandList = this.brandList.concat(brandData);
        }
        this.brandSearchRequest = new CommonCustomerSourceRequest();
        this.brandLoading = false;
      }),
      error => {
        this.brandLoading = false;
        //this.setError(error);
        console.error(error);

      };
  }

  download() {
    this.validator.initTost();
    this.validator.isSubmitted = true;
    if(this.isFormValid()){

    this.downloading = true;
    //this.model.idCustomer = this.model.customer.id

    console.log(this.model);

    this.progressPercent = 0;
    this.progressMessage = "";

    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => localStorage.getItem('_token')
    }

   // console.log("***** HubConnectionBuilder", `${this.baseUrl}userHub?userid=${this.authserve.getCurrentUser().id}` );

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.baseUrl}userHub?userid=${this.authserve.getCurrentUser().id}`, options) //transport signalR : websocket ou pooling
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('****** Connection started'))
      .catch(err => console.log('**** Error while starting connection: ' + err))

    this.hubConnection.on('progress', (response: EventProgress) => {
      console.log("****  gettinf message", response);

      this.progressPercent = response.percent;
      this.progressMessage = response.message;
    });


    this.service.exportInternal(this.model)
      .subscribe(res => {

        this.downloadFile(`KPI_${this.model.customer.name}`, res, "application/vnd.openxmlformats-officedocument.presentationml.presentation");
      },
      error => {
        this.toastr.error("Cannot  build this file because data are not complete, choose another date or / and customer", 'download file', { disableTimeOut: true });
        console.error(error);
          this.downloading = false;
        });
      }
  }

  downloadFile(filename,data, mimeType) {

    const blob = new Blob([data], { type: mimeType });
    const fullfilename = `${filename}.pptx`; 
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fullfilename);
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fullfilename;
      a.href = url;
      a.click();
    }

    this.downloading = false;
  }

  SetSearchDatetype(searchdatetype) {
    this.model.searchDateTypeId = searchdatetype;
  }


  isFormValid(){
    let isOk = this.validator.isValid('beginDate') &&
      this.validator.isValid('endDate')
      && this.validator.isValid('idCustomer')

    return isOk;
  }

  clearDateInput(controlName: any) {
    switch (controlName) {
      case "beginDate": {
        this.model.beginDate = null;
        break;
      }
      case "endDate": {
        this.model.endDate = null;
        break;
      }
    }
  }
}
