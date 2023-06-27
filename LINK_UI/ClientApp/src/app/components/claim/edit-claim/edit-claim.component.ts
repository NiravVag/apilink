import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { of, Subject } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, takeUntil, tap } from 'rxjs/operators';
import { BookingRequestModel, ClaimStatus, EditClaimMasterModel, EditClaimModel, AttachmentFile, Attachment, InvoiceDetailModel } from 'src/app/_Models/claim/edit-claim.model';
import { BookingIdDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { ClaimService } from 'src/app/_Services/claim/claim.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { JsonHelper } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { Validator } from '../../common/validator';
import { FileContainerList, Url } from '../../common/static-data-common';
import { FileInfo } from 'src/app/_Models/fileupload/fileupload';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { ClaimToCancel } from 'src/app/_Models/claim/claim-summary.model';

@Component({
  selector: 'app-edit-claim',
  templateUrl: './edit-claim.component.html',
  styleUrls: ['./edit-claim.component.scss']
})
export class EditClaimComponent extends DetailComponent {
  public _saveloader: boolean = false;
  public _analyzeloader: boolean = false;
  public _validateloader: boolean = false;
  public _closeloader: boolean = false;
  public _cancelloader: boolean = false;
  public qcControl100Goods: Array<any>;
  componentDestroyed$: Subject<boolean> = new Subject();
  masterModel: EditClaimMasterModel;
  invoiceDetailModel: InvoiceDetailModel;
  public model: EditClaimModel;
  public modelRemove: ClaimToCancel;
  isFilterOpen: boolean;
  bookingData: any;
  private jsonHelper: JsonHelper;
  public currentUser: UserModel;
  isCompensation: boolean = false;
  isFinalCompensation: boolean = false;
  isFinalDecision: boolean = false;
  public data: any;
  public modelRef: NgbModalRef;
  isCreateClaimRole: boolean = false;
  isNew: boolean = false;
  isAnalyzeClaimRole: boolean = false;
  isValidateClaimRole: boolean = false;
  isAccountingClaimRole: boolean = false;
  isClosed: boolean = false;
  isValidateTab: boolean = false;
  fileSize: number;
  uploadLimit: number;
  uploadFileExtensions: string;
  downloadloading: boolean = false;
  pageLoading: boolean = false;
  public attachment: Attachment;
  bookingInvoiceData: any;
  isSaveAnalze: boolean = false;
  isSaveValidate: boolean = false;
  isSaveClose: boolean = false;
  isSaveCancel: boolean = false;
  isCancel: boolean = false;
  isEditInspection: boolean = false;
  editTab1: boolean;
  isStatusCancelled: boolean = false;
  public intialiseloading = false;
  existingClaims: string;
  bookingId :string;
  @ViewChild('conformClaimBooking') conformClaimBooking: ElementRef;
  constructor(
    public validator: Validator, translate: TranslateService, toastr: ToastrService, router: Router,
    route: ActivatedRoute, private activatedRoute: ActivatedRoute, public utility: UtilityService,
    public refService: ReferenceService, private service: ClaimService, authservice: AuthenticationService, public modalService: NgbModal,
    public fileStoreService: FileStoreService, public calendar: NgbCalendar, public dateparser: NgbDateParserFormatter) {
    super(router, route, translate, toastr);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("claim/edit-claim.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.isFilterOpen = true;
    this.currentUser = authservice.getCurrentUser();
    this.uploadLimit = 10;
    this.fileSize = 10000000;
    this.uploadFileExtensions = 'png,jpg,jpeg,xlsx,pdf,doc,docx,xls';
  }

  getEditPath() {
    return "editClaim/edit-claim";;
  }
  getViewPath() {
    return "";
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  onInit(id?) {
    this.qcControl100Goods =
      [{ name: 'Yes', id: 1 },
      { name: 'No', id: 0 }
      ];
    this.initialize(id);
  }

  initialize(id?) {
    this.intialiseloading = true;
    this.loading = true;
    this.editTab1 = true;
    this.masterModel = new EditClaimMasterModel();
    this.model = new EditClaimModel();
    this.model.attachments = [];
    this.invoiceDetailModel = new InvoiceDetailModel();
    this.validator.isSubmitted = false;
    // this.attachment = new Attachment();
    this.getBookingIdListBySearch();
    this.getClaimFromList();
    this.getReceivedFromList();
    this.getClaimSourceList();
    this.getDefectFamilyList();
    this.getClaimDepartmentList();
    this.getCustomerRequestList();
    this.getPriorityList();
    this.getCustomerRequestRefundList();
    this.getCurrencyList();
    this.getDefectDistributionList();
    this.getClaimResultList();
    this.getFinalResultList();
    this.getFileTypeList();

    if (id != null) {
      this.pageLoading = true;
      this.service.getEditClaim(id)
        .pipe()
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.data = res;
              this.masterModel.reportList = [];
              this.masterModel.reportList = res.reportList;
              if (!this.masterModel.bookingIdList.some(x => x.id == res.claim.bookingId)) {
                this.masterModel.bookingIdList = this.masterModel.bookingIdList.concat(res.bookingIdList);
              }
              this.getInvoiceByBookingId(res.bookingIdList);
              // this.invoiceDetailModel = res.invoiceDetail;
              this.model = {
                amount: res.claim.amount,
                claimNo: res.claim.claimNo,
                analyzerFeedback: res.claim.analyzerFeedback,
                bookingId: res.claim.bookingId,
                claimCustomerRequestIdList: res.claim.claimCustomerRequestIdList,
                claimDate: res.claim.claimDate,
                claimDepartmentIdList: res.claim.claimDepartmentIdList,
                claimDescription: res.claim.claimDescription,
                claimFromId: res.claim.claimFromId,
                claimSourceId: res.claim.claimSourceId,
                color: res.claim.color,
                compareToAQL: res.claim.compareToAQL,
                currencyId: res.claim.currencyId,
                customerRequestRefundIdList: res.claim.customerRequestRefundIdList,
                customercomment: res.claim.customercomment,
                defectCartonInspected: res.claim.defectCartonInspected,
                defectDistributionId: res.claim.defectDistributionId,
                defectFamilyIdList: res.claim.defectFamilyIdList,
                defectPercentage: res.claim.defectPercentage,
                fobCurrencyId: res.claim.fobCurrencyId,
                fobPrice: res.claim.fobPrice,
                id: res.claim.id,
                noOfPieces: res.claim.noOfPieces,
                priorityId: res.claim.priorityId,
                qcControlId: res.claim.qcControlId != null ? (res.claim.qcControlId == true) ? 1 : 0 : res.claim.qcControlId,
                receivedFromId: res.claim.receivedFromId,
                reportIdList: res.claim.reportIdList,
                requestContactName: res.claim.requestContactName,
                retailCurrencyId: res.claim.retailCurrencyId,
                retailPrice: res.claim.retailPrice,
                claimResultId: res.claim.claimResultId,
                claimRemarks: res.claim.claimRemarks,
                claimRecommendation: res.claim.claimRecommendation,
                finalDecisionIdList: res.claim.finalDecisionIdList,
                finalRefundIdList: res.claim.finalRefundIdList,
                finalAmount: res.claim.finalAmount,
                finalCurrencyId: res.claim.finalCurrencyId,
                statusId: res.claim.statusId,
                statusName: res.claim.statusName,
                claimFinalRefundRemarks: res.claim.claimFinalRefundRemarks,
                realInspectionFees: res.claim.realInspectionFees,
                realInspectionCurrencyId: res.claim.realInspectionCurrencyId,
                fileTypeId: res.claim.fileTypeId,
                fileDesc: res.claim.fileDesc,
                attachments: res.claim.attachments
              }

              this.isEditInspection = true;
              var customerRequestRefundList = this.masterModel.customerRequestRefundList.filter(x => this.model.customerRequestRefundIdList.includes(x.id));
              this.ChangeCustomerRequestRefund(customerRequestRefundList);
              var finalResultList = this.masterModel.finalResultList.filter(x => this.model.finalDecisionIdList.includes(x.id));
              this.ChangeFinalDecision(finalResultList);
              var finalRefundList = this.masterModel.customerRequestRefundList.filter(x => this.model.finalRefundIdList.includes(x.id));
              this.ChangeFinalRefund(finalRefundList);
              this.getBookingData();
              this.checkAndSetRoles(id);
            }
            else {
              this.error = res.result;
              //this.waitingService.close();
            }
            this.pageLoading = false;
            this.loading = false;
            this.intialiseloading = false;
          },
          error => {
            this.setError(error);
            this.loading = false;
            this.intialiseloading = false;
          });
    }
    else {
      this.model.statusId = 0;
      this.checkAndSetRoles(id);
    }


  }

  public Edittab1() {
    this.editTab1 = !this.editTab1
  }

  checkAndSetRoles(id?) {
    var claimCreateRole = this.currentUser.roles.filter(x => x.roleName.toLocaleLowerCase() == "claimcreate");
    var claimAnalyzeRole = this.currentUser.roles.filter(x => x.roleName.toLocaleLowerCase() == "claimanalyze");
    var claimValidateRole = this.currentUser.roles.filter(x => x.roleName.toLocaleLowerCase() == "claimvalidate");
    var claimAccountingRole = this.currentUser.roles.filter(x => x.roleName.toLocaleLowerCase() == "claimaccounting");
    if (claimCreateRole && claimCreateRole.length > 0) {
      this.isNew = true;
      this.isCreateClaimRole = true;
    }
    if (this.model.statusId == ClaimStatus.Cancelled) {
      this.isStatusCancelled = true;
    }
    if (claimCreateRole && claimCreateRole.length > 0 && this.model.statusId == ClaimStatus.Registered) {
      this.isCancel = true;
      this.isNew = false;
      this.isCreateClaimRole = true;
    }
    if (claimCreateRole && claimCreateRole.length > 0 && this.model.statusId == ClaimStatus.Analyzed) {
      this.isCancel = true;
      this.isCreateClaimRole = false;
      this.isNew = false;
    }
    if (claimCreateRole && claimCreateRole.length > 0 && this.model.statusId == ClaimStatus.Validated) {
      this.isCancel = true;
      this.isCreateClaimRole = false;
      this.isNew = false;
    }

    if (claimAnalyzeRole && claimAnalyzeRole.length > 0 && this.model.statusId == ClaimStatus.Registered) {
      this.isCreateClaimRole = false;
      this.isAnalyzeClaimRole = true;
    }
    if (claimAnalyzeRole && claimAnalyzeRole.length > 0 && this.model.statusId == ClaimStatus.Analyzed) {
      this.isCreateClaimRole = false;
      this.isAnalyzeClaimRole = false;
    }
    if (claimAnalyzeRole && claimAnalyzeRole.length > 0 && this.model.statusId == ClaimStatus.Validated) {
      this.isValidateTab = true;
      this.isValidateClaimRole = false;
      this.isCreateClaimRole = false;
      this.isAccountingClaimRole = true;
    }
    if (claimValidateRole && claimValidateRole.length > 0 && this.model.statusId == ClaimStatus.Analyzed) {
      this.isCreateClaimRole = false;
      this.isAnalyzeClaimRole = false;
      this.isValidateClaimRole = true;
    }
    if (claimValidateRole && claimValidateRole.length > 0 && this.model.statusId == ClaimStatus.Validated) {
      this.isCreateClaimRole = false;
      this.isAnalyzeClaimRole = false;
      this.isValidateClaimRole = false;
      this.isValidateTab = true;
    }
    if (claimAccountingRole && claimAccountingRole.length > 0 && this.model.statusId == ClaimStatus.Validated) {
      this.isAccountingClaimRole = true;
      this.isValidateTab = true;
    }
    if (this.model.statusId == ClaimStatus.Closed) {
      this.isNew = false;
      this.isValidateTab = false;
      this.isCreateClaimRole = false;
      this.isAnalyzeClaimRole = false;
      this.isValidateClaimRole = false;
      this.isAccountingClaimRole = false;
      this.isCancel = false;
      this.isClosed = true;
    }
  }

  getBookingIdListBySearch() {
    this.masterModel.bookingIdRequest = new BookingIdDataSourceRequest();

    if (this.model.bookingId) {
      this.masterModel.bookingIdRequest.ids.push(this.model.bookingId);
    }
    this.masterModel.bookingIdInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.bookingIdLoading = true),
      switchMap(term => term
        ? this.service.getBookingIdList(this.masterModel.bookingIdRequest, term)
        : this.service.getBookingIdList(this.masterModel.bookingIdRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.bookingIdLoading = false))
      ))
      .subscribe(data => {
        this.masterModel.bookingIdList = data;
        this.masterModel.bookingIdLoading = false;
      });
  }

  getBookingIdListData() {

    this.masterModel.bookingIdRequest.searchText = this.masterModel.bookingIdInput.getValue();
    this.masterModel.bookingIdRequest.skip = this.masterModel.bookingIdList.length;

    this.masterModel.bookingIdLoading = true;
    this.service.getBookingIdList(this.masterModel.bookingIdRequest).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.bookingIdList = this.masterModel.bookingIdList.concat(data);
        }
        this.masterModel.bookingIdRequest = new BookingIdDataSourceRequest();
        this.masterModel.bookingIdLoading = false;
      }),
      error => {
        this.masterModel.bookingIdLoading = false;
      };
  }

  toggleFilterSection() {
    this.isFilterOpen = !this.isFilterOpen;
  }

  getReportTitleList(item) {
    this.masterModel.reportListLoading = true;
    this.model.reportIdList = null;
    this.service.getReportTitleList(item.id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.reportList = res.dataSourceList;
        }
        else {
          this.masterModel.reportList = [];
        }
        this.masterModel.reportListLoading = false;
      },
        error => {
          this.masterModel.reportList = [];
          this.masterModel.reportListLoading = false;;
        });
  }

  getBookingData() {
    let req = new BookingRequestModel();
    req.bookingId = this.model.bookingId;
    req.reportId = this.model.reportIdList;

    this.service.getClaimBookingData(req)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.bookingData = res.data;
          this.masterModel.showBookingDetails = true;
        }
        else {
          this.bookingData = [];
          this.masterModel.showBookingDetails = false;
        }
      },
        error => {
          this.bookingData = [];
          this.masterModel.showBookingDetails = true;
        })
  }

  changeBookingNo(item) {
    this.getClaimsByBookingId(item)
    this.getReportTitleList(item);
    this.getInvoiceByBookingId(item)
    this.getBookingData();
  }

  getClaimFromList() {
    this.masterModel.claimFromListLoading = true;
    this.service.getClaimFromList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.claimFromList = res.dataSourceList;
        }
        else {
          this.masterModel.claimFromList = [];
        }
        this.masterModel.claimFromListLoading = false;
      },
        error => {
          this.masterModel.claimFromList = [];
          this.masterModel.claimFromListLoading = false;;
        });
  }

  getReceivedFromList() {
    this.masterModel.receivedFromListLoading = true;
    this.service.getReceivedFromList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.receivedFromList = res.dataSourceList;
        }
        else {
          this.masterModel.receivedFromList = [];
        }
        this.masterModel.receivedFromListLoading = false;
      },
        error => {
          this.masterModel.receivedFromList = [];
          this.masterModel.receivedFromListLoading = false;;
        });
  }

  getClaimSourceList() {
    this.masterModel.claimSourceListLoading = true;
    this.service.getClaimSourceList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.claimSourceList = res.dataSourceList;
        }
        else {
          this.masterModel.claimSourceList = [];
        }
        this.masterModel.claimSourceListLoading = false;
      },
        error => {
          this.masterModel.claimSourceList = [];
          this.masterModel.claimSourceListLoading = false;;
        });
  }

  getDefectFamilyList() {
    this.masterModel.defectFamilyListLoading = true;
    this.service.getDefectFamilyList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.defectFamilyList = res.dataSourceList;
        }
        else {
          this.masterModel.defectFamilyList = [];
        }
        this.masterModel.defectFamilyListLoading = false;
      },
        error => {
          this.masterModel.defectFamilyList = [];
          this.masterModel.defectFamilyListLoading = false;;
        });
  }

  getClaimDepartmentList() {
    this.masterModel.claimDepartmentListLoading = true;
    this.service.getClaimDepartmentList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.claimDepartmentList = res.dataSourceList;
        }
        else {
          this.masterModel.claimDepartmentList = [];
        }
        this.masterModel.claimDepartmentListLoading = false;
      },
        error => {
          this.masterModel.claimDepartmentList = [];
          this.masterModel.claimDepartmentListLoading = false;;
        });
  }

  getCustomerRequestList() {
    this.masterModel.claimCustomerRequestListLoading = true;
    this.service.getCustomerRequestList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.claimCustomerRequestList = res.dataSourceList;
        }
        else {
          this.masterModel.claimCustomerRequestList = [];
        }
        this.masterModel.claimCustomerRequestListLoading = false;
      },
        error => {
          this.masterModel.claimCustomerRequestList = [];
          this.masterModel.claimCustomerRequestListLoading = false;;
        });
  }

  getPriorityList() {
    this.masterModel.priorityListLoading = true;
    this.service.getPriorityList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.priorityList = res.dataSourceList;
        }
        else {
          this.masterModel.priorityList = [];
        }
        this.masterModel.priorityListLoading = false;
      },
        error => {
          this.masterModel.priorityList = [];
          this.masterModel.priorityListLoading = false;;
        });
  }

  getCustomerRequestRefundList() {
    this.masterModel.customerRequestRefundListLoading = true;
    this.service.getCustomerRequestRefundList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.customerRequestRefundList = res.dataSourceList;
        }
        else {
          this.masterModel.customerRequestRefundList = [];
        }
        this.masterModel.customerRequestRefundListLoading = false;
      },
        error => {
          this.masterModel.customerRequestRefundList = [];
          this.masterModel.customerRequestRefundListLoading = false;;
        });
  }

  getCurrencyList() {
    this.masterModel.currencyListLoading = true;
    this.service.getCurrencyList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.currencyList = res.dataSourceList;
        }
        else {
          this.masterModel.currencyList = [];
        }
        this.masterModel.currencyListLoading = false;
      },
        error => {
          this.masterModel.currencyList = [];
          this.masterModel.currencyListLoading = false;;
        });
  }

  getDefectDistributionList() {
    this.masterModel.defectDistributionListLoading = true;
    this.service.getDefectDistributionList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.defectDistributionList = res.dataSourceList;
        }
        else {
          this.masterModel.defectDistributionList = [];
        }
        this.masterModel.defectDistributionListLoading = false;
      },
        error => {
          this.masterModel.defectDistributionList = [];
          this.masterModel.defectDistributionListLoading = false;;
        });
  }

  getClaimsByBookingId(item) {
    this.service.getClaimsByBookingId(item.id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          if (res.bookingClaims.length > 0) {
            this.bookingId = "#" +item.id;
            if (res.bookingClaims.length == 1) {
              var claimNo = "#" + res.bookingClaims[0].claimNo;
              // var claimId = res.bookingClaims[0].id;
              // let entity: string = this.utility.getEntityName();
              // var editPage = entity + "/" + Url.EditClaim + claimId;
              this.existingClaims = claimNo;
            } else {
              var claimNos = res.bookingClaims.map(x => x.claimNo).join(',');
              this.existingClaims = claimNos;
            }
            this.bookingClaimsPopup(this.conformClaimBooking);
          }
        }
        else {
        }
      },
        error => {

        });
  }

  bookingClaimsPopup(content) {

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
    this.modelRef.result.then((result) => {
    }, (reason) => {
    });
  }

  getInvoiceByBookingId(item) {
    this.service.getInvoiceByBookingId(item.id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == ResponseResult.Success) {
          this.invoiceDetailModel = res.invoiceDetail;
          this.model.realInspectionFees = this.invoiceDetailModel.totalInvoiceFees;
          this.model.realInspectionCurrencyId = this.invoiceDetailModel.invoiceCurrency;
        }
      },
        error => {
        });
  }

  toggleTab(event, tabTarget) {
    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
  }

  ChangeCustomerRequestRefund(item) {
    var refundType = item.filter(x => x.name.toLowerCase() == "compensation");
    if (refundType && refundType.length > 0) {
      this.isCompensation = true;
    } else {
      this.isCompensation = false;
    }
  }

  ChangeFinalDecision(item) {
    var refundType = item.filter(x => x.name.toLowerCase() == "accepted with financial impact");
    if (refundType && refundType.length > 0) {
      this.isFinalDecision = true;
    } else {
      this.model.finalRefundIdList = [];
      this.isFinalDecision = false;
      this.isFinalCompensation = false;
    }
  }

  ChangeFinalRefund(item) {
    var refundType = item.filter(x => x.name.toLowerCase() == "compensation");
    if (refundType && refundType.length > 0) {
      this.isFinalCompensation = true;
    } else {
      this.isFinalCompensation = false;
    }
  }

  getClaimResultList() {
    this.masterModel.claimResultLoading = true;
    this.service.getClaimResultList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.claimResultList = res.dataSourceList;
        }
        else {
          this.masterModel.claimResultList = [];
        }
        this.masterModel.claimResultLoading = false;
      },
        error => {
          this.masterModel.claimResultList = [];
          this.masterModel.claimResultLoading = false;;
        });
  }

  getFinalResultList() {
    this.masterModel.finalResultLoading = true;
    this.service.getFinalResultList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.finalResultList = res.dataSourceList;
        }
        else {
          this.masterModel.finalResultList = [];
        }
        this.masterModel.finalResultLoading = false;
      },
        error => {
          this.masterModel.finalResultList = [];
          this.masterModel.finalResultLoading = false;;
        });
  }

  getFileTypeList() {
    this.masterModel.fileTypeListLoading = true;
    this.service.getFileTypeList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {

        if (res.result == ResponseResult.Success) {
          this.masterModel.fileTypeList = res.dataSourceList;
        }
        else {
          this.masterModel.fileTypeList = [];
        }
        this.masterModel.fileTypeListLoading = false;
      },
        error => {
          this.masterModel.fileTypeList = [];
          this.masterModel.fileTypeListLoading = false;;
        });
  }

  saveAnalzye() {
    this.isSaveAnalze = true;
    this._analyzeloader = true;
    this.model.statusId = ClaimStatus.Analyzed;
    this.save();
  }

  saveValidate() {
    this.isSaveValidate = true;
    this._validateloader = true;
    this.model.statusId = ClaimStatus.Validated;
    this.save();
  }

  Close() {
    this.isSaveClose = true;
    this._closeloader = true;
    this.model.statusId = ClaimStatus.Closed;
    this.save();
  }

  openConfirm(content) {
    this.modelRemove = {
      id: this.model.id,
      claimNo: this.model.claimNo
    };

    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });

    this.modelRef.result.then((result) => {
    }, (reason) => {
    });

  }

  cancelClaim(item: ClaimToCancel) {
    this._cancelloader = true;
    this.service.cancelClaim(item.id)
      .pipe()
      .subscribe(
        response => {
          this._cancelloader = false;
          if (response && (response.result == 1)) {
            // refresh
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

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {
      if (!this._analyzeloader) {
        this._saveloader = true;
      } else if (!this._validateloader) {
        this._saveloader = true;
      } else if (!this._closeloader) {
        this._saveloader = true;
      }

      this.pageLoading = true;
      this.service.saveClaim(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              let loadElement = false;
              this.validator.isSubmitted = false;
              this.pageLoading = false;
              if (!loadElement) {
                if (this.isSaveAnalze) {
                  this.isSaveAnalze = false;
                  this.showSuccess('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_ANALYZE_SUCCESS');
                } else if (this.isSaveValidate) {
                  this.isSaveValidate = false;
                  this.showSuccess('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_VALIDATE_SUCCESS');
                } else if (this.isSaveClose) {
                  this.isSaveClose = false;
                  this.showSuccess('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_CLOSE_SUCCESS');
                } else {
                  this.showSuccess('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_SAVE_SUCCESS');
                }
                this.ngOnInit();
              }
            }
            else {
              this.pageLoading = false;
              this._saveloader = false;
              switch (res.result) {
                case 2:
                  this.showError('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_CANNOT_ADDSCLAIM');
                  break;
                case 8:
                  this.showError('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_CLAIM_REPORT_EXIST');
                  break;
              }
            }
            if (this._analyzeloader || this._validateloader || this._closeloader) {
              this._analyzeloader = false;
              this._validateloader = false;
              this._closeloader = false;
            }
            this._saveloader = false;
          },
          error => {
            this.showError('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_UNKNONW_ERROR');
            this._saveloader = false;
            this.pageLoading = false;
          });
    } else {
      if (this._analyzeloader || this._validateloader || this._closeloader) {
        this._analyzeloader = false;
        this._validateloader = false;
        this._closeloader = false;
      }
    }
  }

  isFormValid() {
    var todayDate = this.calendar.getToday();
    var date = this.dateparser.parse(this.bookingData.serviceFromDate);
    var result = this.validator.isValid('bookingId')
      && this.validator.isValid('claimDate')
      && this.validator.isValid('receivedFromId')
      && this.validator.isValid('claimSourceId')
      && (this.isCompensation ? this.validator.isValid('amount') : true)
      && (this.isValidateClaimRole || this.isAccountingClaimRole ? this.validator.isValid('claimResultId') : true)
      && (this.model.finalAmount != null ? this.validator.isValid('finalCurrencyId') : true)
      && (this.model.amount != null ? this.validator.isValid('currencyId') : true)
      && (this.isValidateTab ? this.validator.isValid('finalDecisionIdList') : true);
    if (result) {
      if (!todayDate.after(this.model.claimDate) && !todayDate.equals(this.model.claimDate)) {
        this.showWarning('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_CLAIM_DATE_LESS_REQ');
        result = false;
      }
    }
    return result;
  }

  selectFiles(event) {
    const modalRef = this.modalService.open(FileUploadComponent,
      {
        windowClass: "upload-image-wrapper",
        centered: true,
        backdrop: 'static'
      });

    let fileInfo: FileInfo = {
      fileSize: this.fileSize,
      uploadFileExtensions: this.uploadFileExtensions,
      uploadLimit: 10,
      containerId: FileContainerList.Claim,
      token: "",
      fileDescription:null
    }

    modalRef.componentInstance.fromParent = fileInfo;

    modalRef.result.then((result) => {
      if (Array.isArray(result)) {
        result.forEach(element => {
          if (this.model.attachments) {
            element.fileTypeId = this.model.fileTypeId;
            element.fileDesc = this.model.fileDesc;
            var fileType = this.masterModel.fileTypeList.find(x => x.id == this.model.fileTypeId);
            element.fileTypeName = fileType.name;
            this.model.attachments.push(element);
          }
        });
      }
    }, (reason) => {

    });
  }

  removeAttachment(index) {
    this.model.attachments.splice(index, 1);
  }

  getFile(file: AttachmentFile) {

    this.downloadloading = true;

    if (file.fileUrl) {
      this.fileStoreService.downloadBlobFile(file.uniqueld, FileContainerList.Claim)
        .subscribe(res => {
          this.downloadFile(res, file.mimeType, file.fileName);
        },
          error => {
            this.downloadloading = false;
            this.showError('EDIT_CLAIM.TITLE', 'EDIT_CLAIM.MSG_UNKNOWN_ERROR');
          });
      this.downloadloading = false;
    }
  }

  downloadFile(data, mimeType, filename) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename ? filename : "export";//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.downloadloading = false;
  }

  numericValidation(event) {
    this.utility.numericValidation(event, 5);
  }
}
