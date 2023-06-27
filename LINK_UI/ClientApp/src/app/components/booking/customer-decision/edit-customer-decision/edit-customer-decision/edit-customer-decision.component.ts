import { array } from '@amcharts/amcharts4/core';
import { getCurrencySymbol } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DetailComponent } from 'src/app/components/common/detail.component';
import { APIResult, CustomerDecisionSaveResponseResult, FbReportResultType, RoleEnum, UserType } from 'src/app/components/common/static-data-common';
import { CustomerDecisionListSaveRequest, EditCustomerDecisionModel, ResultType } from 'src/app/_Models/booking/customerdecision.model';
import { CustomerDecisionSaveRequest } from 'src/app/_Models/booking/inspectioncustomerdecision';
import { CommonDataSourceRequest, ResponseResult, DataSource } from 'src/app/_Models/common/common.model';
import { EmailSendFileDetails } from 'src/app/_Models/email-send/edit-email-send.model';
import { UserModel } from 'src/app/_Models/user/user.model';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { InspectionCustomerDecisionService } from 'src/app/_Services/booking/inspectioncustomerdecision.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-edit-customer-decision',
  templateUrl: './edit-customer-decision.component.html',
  styleUrls: ['./edit-customer-decision.component.scss']
})
export class EditCustomerDecisionComponent extends DetailComponent {

  bookingId: any;
  loading: boolean;
  componentDestroyed$: Subject<boolean> = new Subject;
  model: EditCustomerDecisionModel;
  selectedReportNo: number;
  customerDecisionModel: CustomerDecisionListSaveRequest;
  modelRef: NgbModalRef;
  selectedCusD: number;
  _apiResult: any = APIResult;
  selectAllChecked: boolean = false;
  noOfItemsSelected: number = 0;
  bookingProductMainList: any;
  editCustomerDecision: boolean = false;
  viewCustomerDecision: boolean = false;
  currentUser: UserModel;
  _roleEnum = RoleEnum;
  defaultDdlValue: string = "All";
  selectedResultId: number;
  cusDecddlSelectedId: number;
  checkedReportId: string;
  selectedDefectproductName: string;
  reportProdNameTitle: string;
  fbReportResult: any = FbReportResultType;
  apiResultValues: any = [];
  customerDecisionResultValues: any = [];
  isTablet: boolean;
  isMobile: boolean;
  showDetailSection: boolean;
  isCDpopupvisible: boolean;
  ProblematicRemarksTitle: "";
  constructor(
    route: ActivatedRoute, public pathroute: ActivatedRoute,
    router: Router, toastr: ToastrService,
    public utility: UtilityService,
    public modalService: NgbModal, private authService: AuthenticationService,
    translate: TranslateService, private service: BookingService, private cdService: InspectionCustomerDecisionService) {
    super(router, route, translate, toastr, modalService);

    this.getIsTablet();
    this.getIsMobile();
    this.showDetailSection = false;
    this.isCDpopupvisible = false;
    this.currentUser = authService.getCurrentUser();
    const isAccess = this.currentUser.roles.some(x => x.id == RoleEnum.EditInspectionCustomerDecision || x.id == RoleEnum.ViewInspectionCustomerDecision);
    if (this.currentUser.usertype != UserType.InternalUser && !isAccess)
      authService.redirectToLanding();
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.loading = true;
    this.initialize();
  }

  initialize() {
    this.bookingId = this.pathroute.snapshot.paramMap.get("bookingId");
    this.model = new EditCustomerDecisionModel();
    this.checkAndSetRoles();
    this.customerDecisionModel = new CustomerDecisionListSaveRequest();
    this.apiResultValues.push(APIResult[0]);

    this.model.productGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];

    this.model.additionalproductGalleryOptions = [
      {
        width: '800px',
        height: '500px',
        "preview": false
      },
      { "breakpoint": 500, "width": "300px", "height": "300px", "thumbnailsColumns": 3 },
      { "breakpoint": 300, "width": "100%", "height": "200px", "thumbnailsColumns": 2 }
    ];

    this.getBookingAndReportInformation(this.bookingId, 0, 0);
  }

  ngOnDestroy() {
    this.componentDestroyed$.next(true);
    this.componentDestroyed$.complete();
  }

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }

  checkAndSetRoles() {
    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.EditInspectionCustomerDecision).length > 0) {
      this.editCustomerDecision = true;
    }

    if (this.currentUser.roles.
      filter(x => x.id == this._roleEnum.ViewInspectionCustomerDecision).length > 0) {
      this.viewCustomerDecision = true;
    }
  }

  openDecisionPopup(content, type) {

    if (type == "product") {
      if (!this.noOfItemsSelected) {
        this.showError('BOOKING_SUMMARY.LBL_CUSTOMER_DECISION', 'CUSTOMER_DECISION.MSG_SELECT_REPORT');
      }
      else {
        let res = this.model.bookingProductList.filter(x => x.isCheckboxSelected).every((val, i, arr) => val.customerDecisionResultId === arr[0].customerDecisionResultId);
        let decisionComment = this.model.bookingProductList.filter(x => x.isCheckboxSelected).every((val, i, arr) => val.customerDecisionComment === arr[0].customerDecisionComment)
        if (res) {
          this.selectedCusD = this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].customerDecisionResultCusDecId;
        }
        if (decisionComment) {
          this.customerDecisionModel.comments = this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].customerDecisionComment;
        }
        else {
          this.customerDecisionModel.comments = null;
        }
        this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
      }
    }
    else {
      if (this.model.reportData.customerDecisionStatus) {
        this.selectedCusD = this.model.reportData.customerResultId;
      }

      this.customerDecisionModel.comments = this.model.reportData.customerDecisionComments;
      this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
    }

  }
  getBookingAndReportInformation(bookingId, reportId, containerId) {
    this.cdService.getCustomerDecisionBookingAndProductsList(bookingId)
      .subscribe(res => {
        if (res.result == 1) {
          this.model.bookingData = res.bookingData;
          this.model.bookingProductList = res.productList;
          this.mapResultList();
          this.bookingProductMainList = res.productList;
          this.getBookingCustomerDecision(this.model.bookingData.customerId);
          //this.selectedReportNo = this.model.bookingProductList[0].reportId;
          this.getReportData(this.model.bookingProductList[0]);
          this.getCustomerDecisionDefectInformation(this.selectedReportNo);
          this.getBookingDefectInformation(this.selectedReportNo);

          this.loading = false;
        }
      },
        error => {
          this.loading = false;
        });
  }
  getBookingCustomerDecision(customerId) {
    this.cdService.GetInspectionCustomerDecision(customerId)
      .subscribe(res => {
        if (res.result == 1) {
          this.model.inspectionCustomerDecisionList = res.customerDecisionList;
          this.mapCustomerDecisionList();
        }
      },
        error => {
          //this.pageLoader=false;
        });

  }

  getReportData(item) {
    if (this.selectedReportNo != item.reportId) {
      this.model.dataLoading = true;
      this.selectedReportNo = item.reportId;
      this.service.GetBookingAndReportDetails(this.model.bookingData.bookingId, this.selectedReportNo, item.containerId)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          if (res.result == ResponseResult.Success) {
            this.model.reportData = res.reportData;
            this.model.reportProducts = res.reportProducts;

            this.reportProdNameTitle = item.containerId > 0 ? "Container - " + item.containerId : res.reportProducts[0].productName;

            if (this.model.reportProducts && this.model.reportProducts.length > 1)
              this.trimProdDesc(this.model.reportProducts);
            this.model.dataLoading = false;
          }
        },
          error => {
            this.model.dataLoading = false;
          });
    }
  }

  trimProdDesc(productList) {
    productList.forEach(x => {

      x.productDescTrim = x.productDescription && x.productDescription.length > 40 ?
        x.productDescription.substring(0, 40) + "..." : x.productDescription;

      x.isDescTooltipShow = x.productDescription && x.productDescription.length > 40;
    });
  }

  reportOnClick(item) {
    if (this.selectedReportNo != item.reportId) {
      this.getReportData(item);
      this.getCustomerDecisionDefectInformation(this.selectedReportNo);
      this.getBookingDefectInformation(this.selectedReportNo)
    }
  }

  mapResultList() {

    const distinctThings = this.model.bookingProductList.filter(
      (thing, i, arr) => arr.findIndex(t => t.resportResultId === thing.resportResultId) === i
    ).map(x => [x.resportResultId]);

    distinctThings.forEach(element => {
      if (element[0]) {
        this.apiResultValues.push(APIResult[element[0]]);
      }
    });

    //remove duplicates
    this.apiResultValues = this.apiResultValues.filter((v, i, a) => a.findIndex(t => (t.id === v.id)) === i)
    this.apiResultValues = [... this.apiResultValues];
  }

  mapCustomerDecisionList() {
    //update customer decision list when api result list changes  
    this.customerDecisionResultValues = [];
    let list = this.model.bookingProductList;
    if (this.defaultDdlValue && this.defaultDdlValue != APIResult[0].name) {
      list = list.filter(x => x.reportResultName == this.defaultDdlValue);
    }

    //get the customer decisions of the report for the left side ddl
    const distinctCusDecision = list.filter(
      (thing, i, arr) => arr.findIndex(t => t.customerDecisionResultCusDecId === thing.customerDecisionResultCusDecId) === i
    ).map(x => [x.customerDecisionResultCusDecId]);

    this.customerDecisionResultValues = this.model.inspectionCustomerDecisionList.filter(x => distinctCusDecision.map(x => x[0]).includes(x.id));

    //remove duplicates
    this.customerDecisionResultValues = this.customerDecisionResultValues.filter((v, i, a) => a.findIndex(t => (t.id === v.id)) === i)

    this.customerDecisionResultValues = [... this.customerDecisionResultValues];
  }

  getCustomerDecisionDefectInformation(reportId) {
    this.service.GetGetInspectionSummary(reportId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == 1) {

          this.model.inspectionReportSummaryList = res.inspectionReportSummaryList;
        }
      },
        error => {
          //this.pageLoader=false;
          // this.exportDataLoading = false;
        });

  }

  getPreviewProductImage(imageList, modalcontent) {
    let list = [...imageList];
    if (this.model.reportData.reportPhoto) {
      list.splice(0, 0, this.model.reportData.reportPhoto);
    }


    this.model.additionalproductGalleryImages = [];
    list.forEach(url => {
      if (url) {
        this.model.additionalproductGalleryImages.push(
          {
            small: url,
            medium: url,
            big: url,
          });
      }

    });

    this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  getBookingDefectInformation(reportId) {
    this.service.GetInspectionDefects(reportId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == 1) {

          this.model.inspectionDefectList = res.inspectionDefectList;
          this.checkCustomerDecisionDefectavailable(this.model.inspectionDefectList);
        }
        else {
          this.model.inspectionDefectList = [];
        }
      },
        error => {
          //this.pageLoader=false;
          // this.exportDataLoading = false;
        });

  }

  checkCustomerDecisionDefectavailable(defectList) {
    this.model.isCriticalDefectavailable = false;
    this.model.isMajorDefectavailable = false;
    this.model.isMinorDefectavailable = false;

    if (defectList && defectList.filter(x => x.critical != 0 && x.critical != null).length > 0) {
      this.model.isCriticalDefectavailable = true;

      this.model.totalCriticalCount = defectList.filter(x => x.critical != 0 && x.critical != null)
        .reduce((sum, current) => sum + current.critical, 0);
    }

    if (defectList && defectList.filter(x => x.major != 0 && x.major != null).length > 0) {
      this.model.isMajorDefectavailable = true;
      this.model.totalMajorCount = defectList.filter(x => x.major != 0 && x.major != null)
        .reduce((sum, current) => sum + current.major, 0);
    }

    if (defectList && defectList.filter(x => x.minor != 0 && x.minor != null).length > 0) {
      this.model.isMinorDefectavailable = true;
      this.model.totalMinorCount = defectList.filter(x => x.minor != 0 && x.minor != null)
        .reduce((sum, current) => sum + current.minor, 0);
    }
  }

  SaveCustomerDecisionList() {

    this.model.saveLoading = true;

    this.selectedResultId = this.selectedCusD;

    this.customerDecisionModel.customerResultId = this.model.inspectionCustomerDecisionList.filter(x => x.id == this.selectedCusD).map(x => x.cusDecId)[0];
    if (this.customerDecisionModel.customerResultId > 0) {
      //this.isCustomerSaveDecisionButton=true;
      this.customerDecisionModel.bookingId = this.model.bookingData.bookingId;
      this.customerDecisionModel.reportIdList = this.model.bookingProductList.filter(x => x.isCheckboxSelected).map(x => x.reportId);

      if (this.customerDecisionModel.reportIdList == null || this.customerDecisionModel.reportIdList.length == 0) {

        this.customerDecisionModel.reportIdList.push(this.selectedReportNo);
      }

      this.cdService.SaveInspectionCustomerDecisionList(this.customerDecisionModel)
        .pipe(takeUntil(this.componentDestroyed$))
        .subscribe(res => {
          if (res.result == CustomerDecisionSaveResponseResult.success) {
            this.changeCusDecisionStatus();
            this.showSuccess('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'COMMON.MSG_SAVED_SUCCESS');
          }
          // After success save, no email send, bcz no email configuration but update the customerdecision status
          else if (res.result == CustomerDecisionSaveResponseResult.noemailconfiguration) {
            this.changeCusDecisionStatus();
            this.showWarning('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'BOOKING_SUMMARY.MSG_CU_DECISION_NO_EMAIL_CONFIGURE');
          }
          else if (res.result == CustomerDecisionSaveResponseResult.noEmailSubjectConfiguration) {
            this.changeCusDecisionStatus();
            this.showWarning('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY');
          }
          else if (res.result == CustomerDecisionSaveResponseResult.noEmailBodyConfiguration) {
            this.changeCusDecisionStatus();
            this.showWarning('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'EMAIL_SEND_PREVIEW.MSG_BODY_CANNOT_EMPTY');
          }
          else if (res.result == CustomerDecisionSaveResponseResult.noEmailRecipientsConfiguration) {
            this.changeCusDecisionStatus();
            this.showWarning('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'EMAIL_SEND_PREVIEW.MSG_TOLIST_CANNOT_EMPTY');
          }

          else if (res.result == CustomerDecisionSaveResponseResult.multipleRuleFound) {
            this.showWarning('CUSTOMER_DECISION.CUSTOMER_DECISION_RESULT', 'CUSTOMER_DECISION.MSG_MULTIPLE_EMAIL_RULE');
            this.modelRef.close();
          }

          this.model.saveLoading = false;
          this.modelRef.close();
          this.selectAllChecked = false;
          this.model.bookingProductList.map(x => x.isCheckboxSelected = false);
          this.customerDecisionModel.sendEmailToFactoryContacts = false;
          this.mapResultList();
          this.mapCustomerDecisionList();
          this.noOfItemsSelected = 0;
        },
          error => {
            this.model.saveLoading = false;
            this.modelRef.close();
            this.selectAllChecked = false;
            this.model.bookingProductList.map(x => x.isCheckboxSelected = false);
            this.customerDecisionModel.sendEmailToFactoryContacts = false;
            this.noOfItemsSelected = 0;
          });
    }
    else {
      this.model.saveLoading = false;
      this.selectAllChecked = false;
      this.model.bookingProductList.map(x => x.isCheckboxSelected = false);
      this.customerDecisionModel.sendEmailToFactoryContacts = false;
      this.showError('BOOKING_SUMMARY.LBL_CUSTOMER_DECISION', 'BOOKING_SUMMARY.MSG_SELECT_CU_DECISION');
      this.modelRef.close();
    }

  }

  changeCusDecisionStatus() {
    let decisionName = this.model.inspectionCustomerDecisionList.filter(x => x.id == this.selectedResultId).map(x => x.name)[0];

    if (this.noOfItemsSelected > 0) {

      this.model.bookingProductList.filter(x => x.isCheckboxSelected).forEach(x => {
        x.customerDecisionResultCusDecId = this.selectedResultId;
        x.customerDecisionName = decisionName;
        x.customerDecisionComment = this.customerDecisionModel.comments;
      });
    }
    else {
      this.model.bookingProductList.filter(x => x.reportId == this.selectedReportNo)[0].customerDecisionResultCusDecId = this.selectedResultId;
      this.model.bookingProductList.filter(x => x.reportId == this.selectedReportNo)[0].customerDecisionName = decisionName;
      this.model.bookingProductList.filter(x => x.reportId == this.selectedReportNo)[0].customerDecisionComment = this.customerDecisionModel.comments;
    }

    let isSelected = this.customerDecisionModel.reportIdList.find(x => x == this.selectedReportNo);
    if (isSelected) {
      this.model.reportData.customerResultId = this.selectedResultId;
      this.model.reportData.customerDecisionStatus = decisionName;
      this.model.reportData.customerDecisionComments = this.customerDecisionModel.comments;
    }
  }

  getInspectionCustomerDecisionReportsData(reportId) {
    this.cdService.GetInspectionCustomerDecisionReportsData(reportId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == 1) {
          this.customerDecisionModel.reportIdList.forEach(element => {
            this.model.bookingProductList.filter(x => x.reportId == element).map(x => x.customerDecisionName = res.status);
            this.model.bookingProductList.filter(x => x.reportId == element).map(x => x.customerDecisionResultId = res.customerResultId);
          });


          this.model.reportData.customerDecisionStatus = res.status;
          this.model.reportData.customerDecisionComments = res.comments;
          this.model.reportData.customerResultId = res.customerResultId;
        }
        //this.isCustomerDecisionButtonEnable=false;
      },
        error => {
          //this.pageLoader=false;
          // this.exportDataLoading = false;
        });

  }

  customerDecisionClick(event, item) {

    this.selectedCusD = item.id;
    this.customerDecisionModel.customerResultId = item.id;
  }

  popUpClose() {
    this.modelRef.close();
    this.customerDecisionModel.customerResultId = null;
    this.selectedCusD = null;
  }

  getAdditionalPreviewProductImage(imageList, modalcontent, type) {

    this.model.productGalleryImages = [];
    imageList.forEach(url => {
      this.model.productGalleryImages.push(
        {
          small: url,
          medium: url,
          big: url,
        });
    });

    this.modalService.open(modalcontent, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  filterByApiResult(item) {
    this.defaultDdlValue = item.name;
    this.model.bookingProductList = this.bookingProductMainList;
    if (item.id > 0) {
      this.model.bookingProductList = this.model.bookingProductList.filter(x => x.resportResultId == item.id);
      this.model.bookingProductList.filter(x => x.resportResultId == item.id).map(x => x.isCheckboxSelected = true);
    }
    else {
      this.model.bookingProductList.map(x => x.isCheckboxSelected = true);
    }
    this.mapCustomerDecisionList();
    this.noOfItemsSelected = this.model.bookingProductList.length;

    if (this.model.bookingProductList.length > 0)
      this.selectAllChecked = true;

    else {
      this.selectAllChecked = false;
    }

    this.cusDecddlSelectedId = null;
    this.mapCustomerDecisionList();
  }

  filterByCustomerDecisionResult(item) {
    this.cusDecddlSelectedId = item.id;
    this.model.bookingProductList = this.bookingProductMainList;
    if (item.id > 0) {
      this.model.bookingProductList = this.model.bookingProductList.filter(x => x.customerDecisionResultCusDecId == item.id);
      this.model.bookingProductList.filter(x => x.customerDecisionResultCusDecId == item.id).map(x => x.isCheckboxSelected = true);
    }
    else {
      this.model.bookingProductList.map(x => x.isCheckboxSelected = true);
    }

    this.noOfItemsSelected = this.model.bookingProductList.length;

    if (this.model.bookingProductList.length > 0)
      this.selectAllChecked = true;

    else {
      this.selectAllChecked = false;
    }
  }

  //check and uncheck of the checkbox
  GetreportIds(item) {
    var selectedcount = this.model.bookingProductList.filter(x => x.isCheckboxSelected).length;

    this.noOfItemsSelected = selectedcount;
    if (selectedcount > 0)
      this.checkedReportId = this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].productIdList[0].length > 13 ?
        this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].productIdList[0].substring(0, 13) + "..." : this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].productIdList[0];

    //uncheck select all even if one selected value is unchecked
    if (selectedcount < this.model.bookingProductList.length) {
      this.selectAllChecked = false;
    }
    else
      this.selectAllChecked = true;
  }

  SelectAllCheckBox() {
    var selectedcount = this.model.bookingProductList.filter(x => x.isCheckboxSelected).length;
    if (selectedcount != this.model.bookingProductList.length) {
      this.model.bookingProductList.forEach(element => {

        element.isCheckboxSelected = true;
      });
      this.selectAllChecked = true; //check the select all
      this.noOfItemsSelected = this.model.bookingProductList.length;
      this.checkedReportId = this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].reportTitle;

      //this.checkedReportId  = this.model.bookingProductList.filter(x => x.isCheckboxSelected)[0].map(x => x.report)
    }

    else {
      this.model.bookingProductList.forEach(element => {
        element.isCheckboxSelected = false;
      });
      this.selectAllChecked = false; //uncheck the select all
      this.noOfItemsSelected = 0;
    }
  }

  getProblematicRemarks(id, reportId, content, resultname) {
    this.ProblematicRemarksTitle = resultname;
    this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });

    this.cdService.GetCusDecisionProblematicRemarks(id, reportId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == CustomerDecisionSaveResponseResult.success) {
          this.model.problematicSummaryList = res.data;
        }
      },
        error => {

        });
  }

  openDefects(content, inspPoId, prodName) {
    this.selectedDefectproductName = prodName;
    this.getBookingDefectInformationbyProducts(this.selectedReportNo, inspPoId);
    this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });
  }

  // openDefectsByContainer(content,inspPoId) {
  //   this.getBookingDefectInformationbyContainer(this.currentReportId,inspPoId);
  // 	this.modalService.open(content, { windowClass : "mdModelWidth", centered: true ,backdrop: 'static'});
  // }

  getBookingDefectInformationbyProducts(reportId, inspPoId) {
    this.model.inspectionProductDefectList = [];
    this.service.GetInspectionDefectsbyProducts(reportId, inspPoId)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(res => {
        if (res.result == 1) {
          this.model.inspectionProductDefectList = res.inspectionDefectList;
          this.checkProductDefectavailable(this.model.inspectionProductDefectList);
        }
      },
        error => {
          // this.exportDataLoading = false;
        });
  }

  checkProductDefectavailable(defectList) {


    if (defectList && defectList.filter(x => x.critical != 0 && x.critical != null).length > 0) {
      this.model.isCriticalProductDefectavailable = true;
    }

    if (defectList && defectList.filter(x => x.major != 0 && x.major != null).length > 0) {
      this.model.isMajorProductDefectavailable = true;
    }

    if (defectList && defectList.filter(x => x.minor != 0 && x.minor != null).length > 0) {
      this.model.isMinorProductDefectavailable = true;
    }
  }

  GetStatusColor(result) {
    if (result.toLowerCase() == "pass") {
      return "#24c11e";
    }
    else if (result.toLowerCase() == "fail") {
      return "#f81538";
    }
    else if (result.toLowerCase() == "pending") {
      return "#ff5626";
    }
  }

  OpenReport(reportData) {
    if (reportData && reportData.finalManualReportPath)
      window.open(reportData.finalManualReportPath);
    else if (reportData && reportData.reportPath)
      window.open(reportData.reportPath);

  }
  getIsTablet() {
    if (window.innerWidth < 1024) {
      this.isTablet = true;
    } else {
      this.isTablet = false;
    }
  }
  getIsMobile() {
    if (window.innerWidth < 450) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }
  }
  toggleDetailSection(item) {
    this.isCDpopupvisible = !this.isCDpopupvisible;
    this.showDetailSection = !this.showDetailSection;
    this.reportOnClick(item)
  }
}
