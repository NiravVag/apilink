import { Component, ElementRef, ViewChild } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { DetailComponent } from '../../common/detail.component';
import { NgbModal, NgbModalRef, NgbDateParserFormatter, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { ICMasterData, DataList, CustomerResult, InspectionCertificateRequest, InspectionCertificateBookingRequest, SupplierResult, InspectionCertificateBookingSearchRequest, ICBookingSearchResult, ICBookingSearchProduct, InspectionCertificateResult, DropdownResult, ICBookingSearch, ICBookingProductRequest } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { InspectionCertificateService } from 'src/app/_Services/inspectioncertificate/inspectioncertificate.service';
import { InspCertificatePopupBookingTableScrollHeight, InspCertificateProductDeleteCount, UserType, ICStatus, RoleEnum } from '../../common/static-data-common';
import { ICUserAccessData } from 'src/app/_Models/inspectioncertificate/inspectioncertificatesummary.model';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { UserAccountService } from 'src/app/_Services/UserAccount/useraccount.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { BusinessLine } from 'src/app/_Models/booking/inspectionbooking.model';
@Component({
  selector: 'app-edit-inspectioncertificate',
  templateUrl: './edit-inspectioncertificate.component.html',
  styleUrls: ['./edit-inspectioncertificate.component.scss']
})
export class EditInspectioncertificateComponent extends DetailComponent {
  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  private searchValidator: Validator;


  ICModel: ICMasterData;
  model: InspectionCertificateRequest;
  modelSearch: InspectionCertificateBookingSearchRequest;
  icuserAccessData: ICUserAccessData;
  searchTranslate: TranslateService;
  searchtoastr: ToastrService
  icStatus = ICStatus;
  roleEnum = RoleEnum;
  icProductValidators: Array<any>;
  @ViewChild('scrollableTable') scrollableTable: ElementRef;
  @ViewChild('scrollableICTable') scrollableICTable: ElementRef;
  businessLine: number;
  _businessLines = BusinessLine;
  constructor(private jsonHelper: JsonHelper, translate: TranslateService, toastr: ToastrService,
    public validator: Validator, public dateparser: NgbDateParserFormatter,
    route: ActivatedRoute, router: Router, private modalService: NgbModal,
    public utility: UtilityService,
    public bookingService: BookingService, public icService: InspectionCertificateService, private authService: AuthenticationService,
    public pathroute: ActivatedRoute, public calendar: NgbCalendar,
    private userAccountService: UserAccountService) {
    super(router, route, translate, toastr);
    this.searchTranslate = translate;
    this.searchtoastr = toastr;
    this.validator = new Validator(jsonHelper, toastr, translate);
    this.modelSearch = new InspectionCertificateBookingSearchRequest();
    this._toastr = toastr;
    this._translate = translate;
    // this.searchValidator = new Validator(jsonHelper, toastr, translate);
  }

  onInit(id?: any): void {
    this.ICModel = new ICMasterData();
    this.icuserAccessData = new ICUserAccessData();
    if (!(id) || id <= 0)
      this.getParamValue();

    this.icuserAccessData.currentUser = this.authService.getCurrentUser();
    this.icuserAccessData.isInternalUser = this.icuserAccessData.currentUser.usertype == UserType.InternalUser ? true : false;


    this.initialize();

    this.validator.setJSON("inspectioncertificate/ic-booking-save.valid.json");
    this.validator.setModelAsync(() => this.model);

    if (id && id > 0)
      this.edit(id);
  }
  getParamValue() {
    //get ic details from pending ic page
    this.pathroute.queryParams.subscribe(
      params => {
        if (params != null && params['paramParent'] != null) {
          this.ICModel.icFromPendingICList = JSON.parse(decodeURI(params['paramParent']));
          if (this.ICModel.icFromPendingICList != null) {
            this.ICModel.isFromPendingIC = true;
            this.productBookingIC();
          }
        }
      });
  }
  initialize() {
    this.model = new InspectionCertificateRequest();
    this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();
    this.validator.isSubmitted = false;

    this.icProductValidators = [];

    this.ICModel.customerLoading = true;
    this.ICModel.icTitleLoading = true;
    this.getCustomerListByUserType();
    this.getICTitleList();
    this.getRoleAccess();
  }
  //logged user ic role exists
  getRoleAccess() {
    this.userAccountService.loggedUserRoleExists(this.roleEnum.InspectionCertificate).subscribe(
      response => {
        this.ICModel.isICRoleAccess = response;
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    );
  }
  getViewPath(): string {
    return "";
  }

  getEditPath(): string {
    return "";
  }
  addBooking(content) {
    this.ICModel.noDataBooking = false;
    this.ICModel.bookingSearchList = null;
    this.modelSearch.serviceFromDate = this.calendar.getNext(this.calendar.getToday(), 'd', -7);
    this.modelSearch.serviceToDate = this.calendar.getToday();
    this.searchValidator = Validator.getValidator(this.modelSearch, "inspectioncertificate/ic-booking-search.valid.json", this.jsonHelper,
      false, this.searchtoastr, this.searchTranslate);
    this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", backdrop: 'static' });
    this.modelRef.result.then((result) => {

    }, (reason) => {
      this.modelSearch = new InspectionCertificateBookingSearchRequest()
    });
  }
  getCustomerListByUserType() {
    this.bookingService.GetCustomerByUserType().subscribe(
      response => {
        this.getCustomerListResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICModel.customerLoading = false;
      }
    );
  }
  getICTitleList() {
    this.icService.GetICTitleList().subscribe(
      response => {
        this.getICTitleResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICModel.icTitleLoading = false;
      }
    );
  }
  getICTitleResponse(response) {
    if (response) {
      if (response.result == DropdownResult.Success) {
        this.ICModel.icTitleList = response.dropDownList.map((x) => {
          var item: DataList = {
            id: x.id,
            name: x.name
          }
          return item;
        });
      }
      else if (response.result == DropdownResult.NodataFound) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGET_IC_TITLE_LIST');
      }
      this.ICModel.icTitleLoading = false;
    }
  }
  getCustomerListResponse(response) {
    if (response) {
      if (response.result == CustomerResult.Success) {
        this.ICModel.customerList = response.customerList.map((x) => {
          var item: DataList = {
            id: x.id,
            name: x.name
          }
          return item;
        });
        if (this.ICModel.isFromPendingIC) {
          this.model.customerId = this.ICModel.icFromPendingICList.selectedCustomerId;
          if (this.model.customerId && this.model.customerId > 0)
            this.getSupplierList(this.model.customerId);
        }
      }
      else if (response.result == CustomerResult.CannotGetCustomerList) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETCUSTLIST');
      }
      this.ICModel.customerLoading = false;
    }
  }
  setSupplierCustomerChange(selectedValue) {
    this.model.supplierId = null;
    this.model.beneficiaryName = null;
    this.model.supplierAddress = null;
    this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();
    this.ICModel.supplierLoading = true;

    if (selectedValue && selectedValue.id && selectedValue.id > 0) {
      this.getSupplierList(selectedValue.id);
      this.model.buyerName = selectedValue.name;
    }

    if (this.model && this.model.id && this.model.id > 0) {
      this.model.supplierId = null;
      this.ICModel.supplierList = null;
    }
  }
  getSupplierList(id) {
    this.bookingService.Getsupplierbycusid(id).subscribe(
      response => {
        this.getSupplierListResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICModel.supplierLoading = false;
      }
    );
  }
  getSupplierListResponse(response) {
    if (response) {
      if (response.result == SupplierResult.Success) {
        this.ICModel.supplierList = response.data.map((x) => {
          var item: DataList = {
            id: x.id,
            name: x.name
          }
          return item;
        });
        // redirect from pending ic page below condition will execute
        if (this.ICModel.isFromPendingIC && this.ICModel.icFromPendingICList && this.ICModel.icFromPendingICList.selectedSupplierId
          && this.ICModel.icFromPendingICList.selectedSupplierId > 0) {

          this.model.supplierId = this.ICModel.icFromPendingICList.selectedSupplierId;

          this.getSupplierAddress(this.model.supplierId);

          this.model.beneficiaryName = this.ICModel.supplierList.filter(x => x.id == this.model.supplierId).length > 0 ?
            this.ICModel.supplierList.filter(x => x.id == this.model.supplierId)[0].name : '';
        }
      }
      else if (response.result == SupplierResult.NodataFound) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOTGETSUPPLIST');
      }
      this.ICModel.supplierLoading = false;
    }
  }
  returnToSummary() {
    if (this.ICModel.isFromPendingIC) {
      this.return('inspectioncertificatepending/pending-ic');
    }
    else
      this.return('inspectioncertificatesearch/ic-summary');
  }
  setAddressSupplierChange(selectedValue) {
    this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();
    if (selectedValue && selectedValue.id && selectedValue.name && selectedValue.id > 0 && selectedValue.name != null) {
      this.model.beneficiaryName = selectedValue.name;
    }
    if (selectedValue && selectedValue.id) {
      this.getSupplierAddress(selectedValue.id);
    }
  }
  getSupplierAddress(id) {
    this.icService.SupplierHeadOfficeAddress(id).subscribe(
      response => {
        this.getSupplierAddressResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICModel.bookingSearchLoading = false;
      });
  }
  getSupplierAddressResponse(response) {
    if (response) {
      if (response.result == SupplierResult.Success) {
        this.model.supplierAddress = response.address;
      }
      else if (response.result == SupplierResult.NodataFound) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANNOT_GET_SUPP_ADDRESS');
      }
      this.ICModel.bookingSearchLoading = false;
    }
  }
  search() {
    this.ICModel.bookingSearchLoading = true;
    this.modelSearch.customerId = this.model.customerId;
    this.modelSearch.supplierId = this.model.supplierId;

    this.icService.BookingSearch(this.modelSearch).subscribe(
      response => {
        this.searchResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        this.ICModel.bookingSearchLoading = false;
      }
    );

    setTimeout(() => {
      this.limitBookingSearchTableHeight();
    }, 2000);
  }
  searchResponse(response) {
    if (response) {
      if (response.result == ICBookingSearchResult.Success) {
        this.ICModel.bookingSearchList = response.bookingList;
        this.ICModel.noDataBooking = false;
        this.ICModel.selectedAllBooking = false;

        //if there is none of booking checkbox enabled we should not show selected all checkbox
        this.ICModel.selectedAllCheckBoxEnable = this.ICModel.bookingSearchList.filter(x => x.enableCheckbox == true).length > 0;
      }
      else if (response && response.result == ICBookingSearchResult.NodataFound) {
        this.ICModel.noDataBooking = true;
        this.ICModel.bookingSearchList = null;
      }
      else if (response && response.result == ICBookingSearchResult.Failure) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_BOOKING_SEARCH_FAILURE');
      }
      else if (response && response.result == ICBookingSearchResult.RequestNotCorrectFormat) {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_REQUEST_NOT_CORRECT_FORMAT');
      }
      this.ICModel.bookingSearchLoading = false;
      setTimeout(() => {
        this.limitBookingSearchTableHeight();
      }, 2000);
    }
  }
  isDateValidationRequired(): boolean {
    let isOk = this.searchValidator.isSubmitted && this.modelSearch.bookingId != null && this.modelSearch.bookingId > 0 ? true : false;

    if (this.modelSearch.bookingId == null || !(this.modelSearch.bookingId > 0)) {
      if (!this.modelSearch.serviceFromDate)
        this.searchValidator.isValid('serviceFromDate');

      else if (this.modelSearch.serviceFromDate && !this.modelSearch.serviceToDate)
        this.searchValidator.isValid('serviceToDate');
    }
    return isOk;
  }
  selectAllBooking() {
    if (this.ICModel.bookingSearchList != null) {
      for (var i = 0; i < this.ICModel.bookingSearchList.length; i++) {
        this.ICModel.bookingSearchList[i].checked = this.ICModel.selectedAllBooking;

        var validateProduct = this.ICModel.bookingSearchList[i].productList.filter(x => x.enableCheckbox && x.remainingQty > 0);
        for (var j = 0; j < validateProduct.length; j++) {
          validateProduct[j].checked = this.ICModel.selectedAllBooking;
        }
      }
    }
  }
  //frame the product table
  selectICProduct() {
    if (!(this.model.icBookingList && this.model.icBookingList.length > 0))
      this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();

    let checkedBooking = this.ICModel.bookingSearchList.filter(x => x.checked);
    if (checkedBooking && checkedBooking.length > 0)
      this.businessLine = checkedBooking[0].businessLine;

    for (var i = 0; i < this.ICModel.bookingSearchList.length; i++) {
      for (var j = 0; j < this.ICModel.bookingSearchList[i].productList.length; j++) {
        if (this.ICModel.bookingSearchList[i].productList[j].checked) {

          var productExists = this.model.icBookingList.filter(x =>
            x.productCode == this.ICModel.bookingSearchList[i].productList[j].productCode &&
            x.poNo == this.ICModel.bookingSearchList[i].productList[j].poNo &&
            x.bookingNumber == this.ICModel.bookingSearchList[i].bookingNumber);

          if (productExists != null && productExists.length > 0 && productExists[0] && productExists[0].productCode != null) {
            this.showWarning('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAME_PRODUCT_EXISTS');
            return false;
            //+ productExists[0].productCode
          }
          else {
            var bookingreq: InspectionCertificateBookingRequest = new InspectionCertificateBookingRequest();

            bookingreq.bookingNumber = this.ICModel.bookingSearchList[i].bookingNumber;
            bookingreq.poNo = this.ICModel.bookingSearchList[i].productList[j].poNo;
            bookingreq.poId = this.ICModel.bookingSearchList[i].productList[j].poId;
            bookingreq.productCode = this.ICModel.bookingSearchList[i].productList[j].productCode;
            bookingreq.productDescription = this.ICModel.bookingSearchList[i].productList[j].productDescription;
            bookingreq.destinationCountry = this.ICModel.bookingSearchList[i].productList[j].destinationCountry;
            bookingreq.unit = this.ICModel.bookingSearchList[i].productList[j].unit;
            bookingreq.remainingQty = this.ICModel.bookingSearchList[i].productList[j].remainingQty;
            bookingreq.color = this.ICModel.bookingSearchList[i].productList[j].color;
            bookingreq.colorCode = this.ICModel.bookingSearchList[i].productList[j].colorCode;
            bookingreq.poColorId = this.ICModel.bookingSearchList[i].productList[j].poColorId;
            bookingreq.bookingProductId = this.ICModel.bookingSearchList[i].productList[j].inspPOTransactionId;
            bookingreq.presentedQty = this.ICModel.bookingSearchList[i].productList[j].presentedQty;
            bookingreq.totalICQty = this.ICModel.bookingSearchList[i].productList[j].totalICQty;
            bookingreq.shipmentQty = null;

            this.model.icBookingList.push(bookingreq);
            this.icProductValidators.push({ icBookingProduct: bookingreq, validator: Validator.getValidator(bookingreq, "inspectioncertificate/ic-booking-product.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
            this.modelRef.close();
          }
        }
      }
    }
    this.ICModel.icBookingAllList = this.model.icBookingList;
    this.mapPOList();
    setTimeout(() => {
      this.limitICTableHeight();
    }, 2000);
  }
  deleteIC(index: number) {
    this.model.icBookingList.splice(index, InspCertificateProductDeleteCount);
    this.icProductValidators.splice(index, InspCertificateProductDeleteCount);
    this.ICModel.icBookingAllList = this.model.icBookingList;

    this.mapPOList();
  }
  limitBookingSearchTableHeight() {
    let height = this.scrollableTable ?
      this.scrollableTable.nativeElement.offsetHeight : 0;
    if (height > InspCertificatePopupBookingTableScrollHeight) {
      this.scrollableTable.nativeElement.classList.add('scroll-maintable-y');
    }
  }
  limitICTableHeight() {
    let height = this.scrollableICTable ?
      this.scrollableICTable.nativeElement.offsetHeight : 0;
    if (height > InspCertificatePopupBookingTableScrollHeight) {
      this.scrollableICTable.nativeElement.classList.add('scroll-maintable-y');
    }
  }

  isFormValid() {
    let isOk = this.validator.isValid('customerId') && this.validator.isValid('supplierId') &&
      this.validator.isValid('icTitleId') &&
      this.validator.isValid('buyerName') &&
      this.isBookingICProduct() &&
      this.icProductValidators.every((x) => x.validator.isValid('shipmentQty')
      )
      // && this.isValidShipmentQty();
    return isOk;
  }
  isBookingICProduct() {
    let result: boolean = false;
    if (this.model.icBookingList != null &&
      this.model.icBookingList.length > 0)
      result = true;
    else
      this.showWarning('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SELECT_BOOKING_PRODUCT');
    return result;
  }
  //shipment qty value should not exceed with totalicqty, presentedqty
  isValidShipmentQty() {
    let result: boolean = false;
    if (this.model.icBookingList != null &&
      this.model.icBookingList.length > 0) {
      for (var i = 0; i < this.model.icBookingList.length; i++) {
        if (this.model.icBookingList[i].shipmentQty > 0) {
          //   result = true;
          if (((this.model.icBookingList[i].shipmentQty) + (this.model.icBookingList[i].totalICQty)
            <= this.model.icBookingList[i].presentedQty)
          ) {
            result = true;
          }
          else {
            result = false;
            this.showWarning('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SHIPMENT_QTY_LESS_OR_EQUAL_REMAINING_QTY');
            break;
          }
        }
        // else {
        //   result = false;
        //   this.showWarning('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_VALID_SHIPMENT_QTY');
        //   break;
        // }
      }
    }
    return result;
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    this.icProductValidators.forEach(item => {
      item.validator.isSubmitted = true;
    });

    if (this.isFormValid()) {
      this.ICModel.saveLoading = true;
      this.icService.SaveIC(this.model).subscribe(
        response => {
          this.saveResponse(response);
        },
        error => {
          this.ICModel.saveLoading = false;
          this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        }
      );
    }
  }
  saveResponse(response) {
    this.ICModel.saveLoading = false;
    if (response) {
      if (response.result == InspectionCertificateResult.Success) {

        this.showSuccess('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAVED_SUCCESSFULLY');
        if (this.fromSummary)
          this.return('inspectioncertificatesearch/ic-summary');
        else
          this.edit(response.id);
      }
      else if (response.result == InspectionCertificateResult.ICNoNotInserted)
        this.showWarning('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_IC_NO_NOT_INSERT');
      else if (response.result == InspectionCertificateResult.RequestNotCorrectFormat)
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_REQUEST_NOT_CORRECT_FORMAT');
      else if (response.result == InspectionCertificateResult.Failure)
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAVE_FAILED');
    }
  }
  edit(id: any) {
    this.icService.EditIC(id).subscribe(
      response => {
        this.editResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    );
  }
  editResponse(response) {
    if (response) {
      if (response.result == InspectionCertificateResult.Success) {

        this.model = response.editInspectionCertificate;
        //to handle old Ic's without buyer name
        this.model.buyerName = (this.model.buyerName) ? this.model.buyerName : this.ICModel.customerList ? this.ICModel.customerList.filter(x => x.id == this.model.customerId)[0].name : "";
        this.ICModel.icBookingAllList = this.model.icBookingList;
        if (this.ICModel.icBookingAllList && this.ICModel.icBookingAllList.length > 0) {
          this.businessLine = this.ICModel.icBookingAllList[0].businessLine
        }
        this.productValidatorMap();

        if (this.model && this.model.customerId && this.model.customerId > 0)
          this.getSupplierList(this.model.customerId);

        setTimeout(() => {
          this.limitICTableHeight();
        }, 1000);
      }
      else if (response.result == InspectionCertificateResult.RequestNotCorrectFormat)
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_REQUEST_NOT_CORRECT_FORMAT');
      else if (response.result == InspectionCertificateResult.Failure)
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAVE_FAILED');
    }
  }
  cancel() {
    if (this.model && this.model.id && this.model.id > 0) {
      this.ICModel.cancelLoading = true;
      this.icService.CancelIC(this.model.id).subscribe(
        response => {
          this.cancelResponse(response);
        },
        error => {
          this.ICModel.cancelLoading = false;
          this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        }
      );
    }
  }
  cancelResponse(response) {
    this.ICModel.cancelLoading = false;
    if (response.result == InspectionCertificateResult.Success) {
      if (this.fromSummary)
        this.return('inspectioncertificatesearch/ic-summary');
      else
        this.edit(response.id);

      this.showSuccess('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_CANCEL_SUCCESSFULLY');
    }
    else if (response.result == InspectionCertificateResult.RequestNotCorrectFormat)
      this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_REQUEST_NOT_CORRECT_FORMAT');
    else if (response.result == InspectionCertificateResult.Failure)
      this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAVE_FAILED');
  }

  //loading the loader icon based on isdraft condition
  previewLoader(isdraft, status) {
    if (!isdraft)
      this.ICModel.previewLoading = status;
    else
      this.ICModel.draftLoading = status;
  }
  preview(isdraft) {

    this.previewLoader(isdraft, true);

    this.icService.preview(this.model.id, isdraft)
      .subscribe(res => {
        this.downloadFile(res, "application/pdf");
        this.previewLoader(isdraft, false);
      },
        error => {
          this.previewLoader(isdraft, false);
          this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }
  //booking check box changed event
  bookingDataSelect(bookingItem: ICBookingSearch) {
    if (bookingItem && bookingItem.productList && bookingItem.productList.length > 0) {
      var validateProduct = bookingItem.productList.filter(x => x.enableCheckbox && x.remainingQty > 0);

      validateProduct.forEach(function (item: any) {
        item.checked = bookingItem.checked;
      });
    }
    this.ICModel.selectedAllBooking = this.ICModel.bookingSearchList.every(function (item: any) {
      return item.checked == true;
    });
  }
  //product check box changed event
  PODataSelect(bookingItem: ICBookingSearch) {
    if (bookingItem && bookingItem.productList && bookingItem.productList.length > 0) {
      var productList = bookingItem.productList.filter(x => x.enableCheckbox && x.remainingQty > 0);
      bookingItem.checked = productList.every(function (item: any) {
        return item.checked == true;
      });
      this.ICModel.selectedAllBooking = this.ICModel.bookingSearchList.every(function (item: any) {
        return item.checked == true;
      });
    }
  }
  //map the model request from ic pending page
  mapRequestModel(): ICBookingProductRequest {
    var productModelReq = new ICBookingProductRequest();
    productModelReq.BookingIdList = this.ICModel.icFromPendingICList.bookingIds;
    productModelReq.ProductIdList = this.ICModel.icFromPendingICList.inspPoTransactionIds;
    return productModelReq;
  }
  //pending ic page to new ic page server call to form the product table
  productBookingIC() {
    var model = this.mapRequestModel();
    this.icService.ProductBookingIC(model).subscribe(
      response => {
        this.productBookingICResponse(response);
      },
      error => {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    );
  }
  productBookingICResponse(res) {
    if (res) {
      if (res.result == ICBookingSearchResult.Success) {
        this.model.icBookingList = res.productBookingList;
        if (this.model.icBookingList && this.model.icBookingList.length > 0)
          this.businessLine = this.model.icBookingList[0].businessLine;
        this.mapPOList();
        this.productValidatorMap();

        this.ICModel.icBookingAllList = this.model.icBookingList;
      }
      else {
        this.showError('EDIT_INSPECTION_CERTIFICATE.TITLE', 'COMMON.MSG_UNKNONW_ERROR');
      }
    }
  }
  //map the po list(id,name) from ic product table
  mapPOList() {
    if (this.model.icBookingList && this.model.icBookingList.length > 0) {
      this.ICModel.poList = Array.from(new Set(this.model.icBookingList
        .map(x => x.poId)))
        .map(poId => {
          var item: DataList = {
            id: poId,
            name: this.model.icBookingList.find(x => x.poId == poId).poNo
          }
          return item;
        });
    }
    else {
      this.ICModel.poList = [];
    }
    if (this.ICModel.poId && this.ICModel.poId.length > 0) {

      var selectedPoIdFromList = this.ICModel.poList.filter(y => this.ICModel.poId.filter(x => x == y.id));

      this.ICModel.poId = [];

      for (var i = 0; i < selectedPoIdFromList.length; i++) {

        //push the selected poid
        this.ICModel.poId.push(selectedPoIdFromList[i].id);
      }
    }
  }
  //set the product table based on po selected in dropdown
  setProductPoChange(value: DataList[]) {
    var bookingList = this.ICModel.icBookingAllList;

    if (value && value.length > 0 && value[0].id > 0) {

      this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();

      for (var i = 0; i < value.length; i++) {
        //push the selected poid data to product table
        this.model.icBookingList.push.apply(this.model.icBookingList,
          bookingList.filter(x => x.poId == value[i].id));
      }
    }
    else {
      this.model.icBookingList = this.ICModel.icBookingAllList;
    }
    this.productValidatorMap();
  }
  //ic booking product list added to validator list
  productValidatorMap() {
    this.icProductValidators = [];

    this.model.icBookingList.forEach(item => {
      this.icProductValidators.push({ icBookingProduct: item, validator: Validator.getValidator(item, "inspectioncertificate/ic-booking-product.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
    });
  }

  unselectAllPO() {
    this.ICModel.poId = [];
    this.model.icBookingList = this.ICModel.icBookingAllList;
    this.productValidatorMap();
  }
  selectAllPO() {
    this.ICModel.poId = this.ICModel.poList.map(item => item.id);

    var bookingList = this.ICModel.icBookingAllList;
    if (this.ICModel.poId && this.ICModel.poId.length > 0 && this.ICModel.poId[0] > 0) {

      this.model.icBookingList = new Array<InspectionCertificateBookingRequest>();

      for (var i = 0; i < this.ICModel.poId.length; i++) {
        //push the selected poid data to product table
        this.model.icBookingList.push.apply(this.model.icBookingList,
          bookingList.filter(x => x.poId == this.ICModel.poId[i]));
      }
    }
    this.productValidatorMap();
  }
  expand(bookingId: number): void {
    this.setExpandCollapse(bookingId, true);
  }

  collapse(bookingId: number): void {
    this.setExpandCollapse(bookingId, false);
  }
  //set the expand collapse value
  setExpandCollapse(bookingId: number, isExpand: boolean) {
    if (this.ICModel.bookingSearchList &&
      this.ICModel.bookingSearchList.filter(x => x.bookingNumber == bookingId).length > 0) {
      this.ICModel.bookingSearchList.filter(x => x.bookingNumber == bookingId)[0].isExpand = isExpand;
    }
  }
  addProductButtonDisable(): boolean {
    return this.ICModel.customerLoading || this.ICModel.supplierLoading ||
      !(this.model.customerId > 0 || this.model.supplierId > 0) || this.ICModel.draftLoading ||
      this.ICModel.cancelLoading || this.ICModel.saveLoading || this.ICModel.previewLoading;
  }
  //loading dropdown and button disable the buttons
  buttonDisable(): boolean {
    return this.ICModel.supplierLoading || this.ICModel.customerLoading || this.ICModel.icTitleLoading ||
      this.ICModel.cancelLoading || this.ICModel.saveLoading || this.ICModel.previewLoading || this.ICModel.draftLoading;
  }
}
