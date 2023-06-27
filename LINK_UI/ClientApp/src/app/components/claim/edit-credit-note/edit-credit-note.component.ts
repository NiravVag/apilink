import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { CustomerService } from './../../../_Services/customer/customer.service';
import { ClaimService } from './../../../_Services/claim/claim.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { EditCreditNoteMasterModel, EditCreditNoteModel, GetCreditNoteItemModel } from 'src/app/_Models/claim/edit-credit-note.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { JsonHelper, Validator } from '../../common';
import { DetailComponent } from '../../common/detail.component';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { ResponseResult } from 'src/app/_Models/common/common.model';
import { InvoiceBankService } from 'src/app/_Services/invoice/invoicebank.service';

@Component({
  selector: 'app-edit-credit-note',
  templateUrl: './edit-credit-note.component.html',
  styleUrls: ['./edit-credit-note.component.scss']
})
export class EditCreditNoteComponent extends DetailComponent {
  masterModel: EditCreditNoteMasterModel;
  model: EditCreditNoteModel;
  title: string;
  saveloading: boolean;
  todaydate: any;
  creditNoteItemValidator: Array<any>;
  public jsonHelper: JsonHelper;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  hideSubmit=false;
  constructor(
    private invoiceBankService: InvoiceBankService,
    private claimService: ClaimService, public calendar: NgbCalendar, private customerService: CustomerService, private refService: ReferenceService,
    public validator: Validator, translate: TranslateService, toastr: ToastrService, router: Router,
    route: ActivatedRoute, private activatedRoute: ActivatedRoute, public utility: UtilityService) {
    super(router, route, translate, toastr);
    this.todaydate = this.calendar.getToday();
    this._toastr = toastr;
    this._translate = translate;
    this.validator.setJSON("claim/edit-credit-note.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
  }

  onInit(id?: any, inputparam?: ParamMap): void {
    this.jsonHelper = this.validator.jsonHelper;
    this.masterModel = new EditCreditNoteMasterModel();
    this.model = new EditCreditNoteModel();
    this.creditNoteItemValidator = [];
    if (id && id > 0) {
      this.getCreditNote(id);
      this.title = "EDIT_CREDIT_NOTE.LBL_EDIT";
    }
    else {
      this.title = "EDIT_CREDIT_NOTE.Title";
      this.activatedRoute.queryParams.subscribe(
        params => {
          //// (+) before `params.get()` turns the string into a number
          //this.selectedId = +params.get('id');
          //return this.service.getHeroes();
          if (params != null && params['paramParent'] != null) {
            this.paramParent = params['paramParent'];
            this.fromSummary = true;
          }

          if (params != null && params['claimIds'] != null) {
            const claimIds = JSON.parse(decodeURI(params['claimIds']));
            this.getClaimData(claimIds);
            this.model.creditDate = this.todaydate;
            this.model.postDate = this.todaydate;
            this.hideSubmit=false;            
          }
          else{
            this.hideSubmit=true;
            this.showWarning('EDIT_CREDIT_NOTE.Title','EDIT_CREDIT_NOTE.MSG_CLAIM_REQ')
          }
        }
      );
    }


    this.getCurrencyList();
    this.getPaymentTerms();
    this.getOfficeList();
    this.getBillingEntityList();
    this.getCreditTypeList();

  }
  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.creditNoteItemValidator)
      item.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.model.saveCreditNotes = this.creditNoteItemValidator.map(x => x.creditNote);
      this.saveloading = true;
      this.claimService.saveCreditNote(this.model).subscribe(res => {
        if (res && res.result === ResponseResult.Success) {
          this.showSuccess(
            "EDIT_CREDIT_NOTE.MSG_SAVE_RESULT",
            "EDIT_CREDIT_NOTE.MSG_SAVE_OK"
          );
          this.return("editClaim/credit-note-summary");
        }
        else if (res && res.result === ResponseResult.NoDataFound) {
          this.showError(
            "EDIT_CREDIT_NOTE.MSG_SAVE_RESULT",
            "EDIT_CREDIT_NOTE.MSG_NOT_FOUND"
          );
        }
        else if (res.result === 3) {
          this.showError(
            "EDIT_CREDIT_NOTE.MSG_SAVE_RESULT",
            "EDIT_CREDIT_NOTE.MSG_CREDIT_NO_ALREDY_EXIST"
          );
        }
        this.saveloading = false;
      })
    }
  }

  getClaimData(claimIds) {
    this.claimService.getClaimDetailByIds(claimIds).subscribe(res => {
      if (res && res.result === 1) {
        this.model.saveCreditNotes = res.data;
        this.masterModel.customerId = res.customerId;
        this.model.billedTo = res.customerName;
        this.masterModel.isAccountingCreditNoteRole=res.isAccountingCreditNoteRole;
        this.mapCreditNoteItem(res.data);
        this.getCustomerDetails(this.masterModel.customerId);
        this.getCustomerAddressList();
        this.getCustomerContactList();
      }
    })
  }

  mapCreditNoteItem(items: GetCreditNoteItemModel[]) {
    items.forEach((x) => {
      this.creditNoteItemValidator.push({
        creditNote: x,
        validator: Validator.getValidator(
          x,
          "claim/edit-credit-note-item.valid.json",
          this.jsonHelper,
          this.validator.isSubmitted,
          this._toastr,
          this._translate
        ),
      });
    });
  }

  getCustomerDetails(customerId: number) {
    this.customerService.GetCustomerByCustomerId(customerId).subscribe(res => {
      if (res && res.result === 1 && res.dataSourceList.length > 0) {
        this.model.billedTo = res.dataSourceList[0].name;
      }
    },
      error => {
        this.setError(error);
      });
  }

  getCustomerAddressList() {
    this.masterModel.customerAddressLoading = true;
    this.customerService.getCustomerAddressList(this.masterModel.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.customerAddressList = response.dataSourceList;
          this.masterModel.customerAddressLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.customerAddressLoading = false;
        });
  }

  getCustomerContactList() {
    this.masterModel.customerContactsLoading = true;
    this.customerService.getCustomerContactList(this.masterModel.customerId)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.customerContactList = response.dataSourceList;
          this.masterModel.customerContactsLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.customerContactsLoading = false;
        });
  }
  getCurrencyList() {
    this.masterModel.currencyLoading = true;

    this.refService.getCurrencyList().subscribe(
      (res) => {
        if (res.result == ResponseResult.Success) {
          this.masterModel.currencyList = res.dataSourceList;
        } else {
          this.masterModel.currencyList = [];
        }
        this.masterModel.currencyLoading = false;
      },
      (error) => {
        this.masterModel.currencyList = [];
        this.masterModel.currencyLoading = false;
      }
    );
  }

  onChangePaymentType(event) {
    if (event) {
      this.model.paymentTerms = event.name;
      this.model.paymentDuration = event.duration;
    }
  }

  getPaymentTerms() {
    this.masterModel.paymentTermLoading = true;
    this.refService
      .getInvoicePaymentTypeList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.paymentTermList = response.dataSourceList;
          this.masterModel.paymentTermLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.paymentTermLoading = false;
        }
      );
  }

  getOfficeList() {
    this.masterModel.officeLoading = true;
    this.refService
      .getInvoiceOfficeList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.officeList = response.dataSourceList;
          this.masterModel.officeLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.officeLoading = false;
        }
      );
  }
  onChangeBillingEntity(event) {
    if (event) {
      this.onClearBillingEntity();
      this.getInvoiceBankList(event.id);
    }
  }

  getBillingEntityList() {
    this.masterModel.billingLoading = true;
    this.refService
      .getBillingEntityList()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.billingEntityList = response.dataSourceList;
          this.masterModel.billingLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.billingLoading = false;
        }
      );
  }

  onClearBillingEntity() {
    this.model.bankId = null;
    this.masterModel.bankList = [];

  }
  getInvoiceBankList(billingEntity) {
    this.masterModel.bankLoading = true;
    this.refService
      .getInvoiceBankList(billingEntity)
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.bankList = response.dataSourceList;
          this.masterModel.bankLoading = false;
        },
        (error) => {
          this.setError(error);
          this.masterModel.bankLoading = false;
        }
      );
  }
  removeItem(index) {
    this.creditNoteItemValidator.splice(index, 1);
  }

  isFormValid() {
    return this.validator.isValid('creditNo')
      && this.validator.isValid('creditDate')
      && this.validator.isValid('postDate')
      && this.validator.isValid('billedTo')
      && this.validator.isValid('billedAddress')
      && this.validator.isValid('currencyId')
      && this.validator.isValid('paymentTerms')
      && this.validator.isValid('paymentDuration')
      && this.validator.isValid('officeId')
      && this.validator.isValid('bankId')
      && this.validator.isValid('billingEntity')
      && this.validator.isValid('creditTypeId')
      && this.creditNoteItemValidator.every(x => x.validator.isValid('refundAmount'));
  }

  async getInvoiceNumberExistsStatus(): Promise<boolean> {
    this.masterModel.isCreditNoLoading = true;
    this.masterModel.isCreditNoLoadingMsg = "Checking Credit Exists Or Not";
    let response: boolean;
    try {
      response = await this.claimService.checkCreditNumberExist(
        this.model.creditNo
      );
    } catch (e) {
      console.error(e);
      this.masterModel.isCreditNoLoading = false;
      this.showError(
        "EDIT_CREDIT_NOTE.LBL_TITLE",
        "COMMON.MSG_UNKNONW_ERROR"
      );
    }
    this.masterModel.isCreditNoLoading = false;
    return response;
  }
  async checkCreditNoExists() {
    let result = false;
    if (this.model.creditNo) {
      result = await this.getInvoiceNumberExistsStatus();
      if (result) {
        this.showWarning(
          "EDIT_CREDIT_NOTE.LBL_CREDIT_NOTE",
          "EDIT_CREDIT_NOTE.MSG_CREDIT_NO_ALREDY_EXIST"
        );
      } else {
        this.showSuccess(
          "EDIT_CREDIT_NOTE.LBL_CREDIT_NOTE",
          "EDIT_CREDIT_NOTE.MSG_CREDIT_NUMBER_NOT_EXISTS"
        );
      }
    } else {
      result = false;
      this.showWarning(
        "EDIT_CREDIT_NOTE.LBL_CREDIT_NOTE",
        "EDIT_CREDIT_NOTE.LBL_CREDIT_NO_CANNOT_BE_EMPTY"
      );
    }
    return result;
  }

  getCreditNote(id) {
    this.claimService.getCreditNote(id).subscribe(res => {
      if (res && res.result === ResponseResult.Success) {
        this.model = res.creditNote;
        this.masterModel.oldCreditNo = res.creditNote.creditNo;
        this.masterModel.customerId = res.creditNote.customerId;
        this.masterModel.isAccountingCreditNoteRole=res.isAccountingCreditNoteRole;
        this.mapCreditNoteItem(this.model.saveCreditNotes);
        this.getBankDetails(this.model.bankId);

        this.getCustomerAddressList();
        this.getCustomerContactList();
      }
      else {
        this.showError(
          "EDIT_CREDIT_NOTE.LBL_CREDIT_NOTE",
          "EDIT_CREDIT_NOTE.MSG_NOT_FOUND"
        );
      }
    })
  }
  getBankDetails(id: number) {
    this.invoiceBankService.getBankDetails(id).then((res) => {
      if (res && res.result === 1) {
        if (this.model.billingEntity === undefined) {
          this.model.billingEntity = res.bankDetails.billingEntity;
          this.getInvoiceBankList(this.model.billingEntity);
        }
      }
    });
  }
  numericValidation(event, length) {
    this.utility.numericValidation(event, length);
  }

  validateNegativeValue(value) {
    if (parseInt(value) < 0) return true;

    return false;
  }

  getDecimalValuewithTwoDigits(numArr) {
    if (numArr.length > 1) {
      var afterDot = numArr[1];

      if (afterDot.length > 2) {
        return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
      }
    }
  }

  validateDecimal(item, itemFeeType): void {
    let numArr: Array<string>;

    if (item[itemFeeType]) {
      if (!this.validateNegativeValue(item[itemFeeType])) {
        numArr = item[itemFeeType].toString().split(".");

        var value = this.getDecimalValuewithTwoDigits(numArr);

        setTimeout(() => {
          if (value) item[itemFeeType] = value;
        }, 10);
      } else {
        setTimeout(() => {
          item[itemFeeType] = null;
        }, 10);
      }
    }
  }
  getCreditTypeList() {
    this.masterModel.creditTypeLoading = true;
    this.claimService.getCreditTypeList().subscribe(res => {
      if (res && res.result === ResponseResult.Success) {
        this.masterModel.creditTypeList = res.dataSourceList;
      }
      else {
        this.masterModel.creditTypeList = [];
      }
      this.masterModel.creditTypeLoading = false;;
    }, error => {
      this.masterModel.creditTypeList = [];
      this.masterModel.creditTypeLoading = false;;
    })
  }
  setAddress(event) {
    if (event) {
      this.model.billedAddress = event.name;
    }

  }
}
