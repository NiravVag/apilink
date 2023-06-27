import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { UtilityService } from "./../../../_Services/common/utility.service";
import { JsonHelper } from "./../../common/jsonHelper";
import { LocationService } from "src/app/_Services/location/location.service";
import { InvoiceDiscountService } from "./../../../_Services/invoice/invoice-discount.service";
import { CustomerService } from "./../../../_Services/customer/customer.service";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import {
  EditInvoiceDiscountPeriodModel,
  EditInvoiceDiscountModel,
} from "src/app/_Models/invoice/invoice-discount-register.model";
import { DetailComponent } from "../../common/detail.component";
import { Validator } from "../../common";
import { NgbCalendar, NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { CommonDataSourceRequest } from "src/app/_Models/common/common.model";

@Component({
  selector: "app-invoice-discount-register",
  templateUrl: "./invoice-discount-register.component.html",
  styleUrls: ["./invoice-discount-register.component.scss"],
})
export class InvoiceDiscountRegisterComponent extends DetailComponent {
  model: EditInvoiceDiscountModel;
  customerList: any;
  customerLoading: boolean;

  invDisTypeLoading: boolean;
  invDisTypeList: Array<any>;
  saveloading: boolean;

  countryList: Array<any>;
  countryLoading: boolean;

  jsonHelper: JsonHelper;
  title: string = "";
  public limitValidators: Array<any> = [];
  _translate: TranslateService;
  _toastr: ToastrService;
  onInit(id?: any, inputparam?: ParamMap): void {
    this.model = new EditInvoiceDiscountModel();
    this.validator.setJSON("invoice/invoice-discount-register.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.getCustomerList();
    this.getInvoiceDiscountTypes();
    this.jsonHelper = this.validator.jsonHelper;
    this.init(id);
  }

  mapLimits(invoiceDiscount: EditInvoiceDiscountModel) {
    invoiceDiscount.limits.forEach((x) => {
      this.limitValidators.push({
        limit: x,
        validator: Validator.getValidator(
          x,
          "invoice/invoice-discount-limit.valid.json",
          this.jsonHelper,
          this.validator.isSubmitted,
          this._toastr,
          this._translate
        ),
      });
    });
  }
  getViewPath(): string {
    return "invoicediscountsearch/invoice-discount-summary";
  }
  getEditPath(): string {
    return "invoicediscountedit/invoice-discount-edit";
  }
  numericValidation(event) {
    this.utility.numericValidation(event, 10);
  }

  getDecimalValuewithTwoDigits(numArr) {
    if (numArr.length > 1) {
      var afterDot = numArr[1];

      if (afterDot.length > 2) {
        return Number(numArr[0] + "." + afterDot.substring(0, 2)).toFixed(2);
      }
    }
  }

  validateNegativeValue(value) {
    if (parseInt(value) < 0) return true;

    return false;
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

  constructor(
    private calendar: NgbCalendar,
    public customerService: CustomerService,
    public locationService: LocationService,
    public invoiceDiscountService: InvoiceDiscountService,
    public validator: Validator,
    utility: UtilityService,
    toastr: ToastrService,
    route: ActivatedRoute,
    router: Router,
    modal: NgbModal,
    translate: TranslateService) {
    super(router, route, translate, toastr, modal, utility);
    this._translate = translate;
    this._toastr = toastr;
  }

  init(id?) {
    if (id != null && id > 0) {
      this.title='INV_DIS_REGISTER.EDIT_TITLE';
      this.invoiceDiscountService.editInvoiceDiscount(id)
        .subscribe(
          res => {
            if (res && res.result === 1) {
              this.model = res.invoiceDiscount;
              this.mapLimits(this.model);
              this.getCountryList();
            }
            else if (res.result === 2) {
              this.showError("INV_DIS_REGISTER.MSG_SAVE_RESULT","INV_DIS_REGISTER.MSG_NOT_FOUND");
            }
          },
          error => {
            this.setError(error);
          });
    }
    else {
      this.title='INV_DIS_REGISTER.ADD_TITLE';
      this.fromSummary = false;
      this.addLimit();
    }
  }
  getCustomerList() {
    this.customerLoading = true;
    this.customerService
      .getCustomerSummary()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result == 1) {
            this.customerList = response.customerList;
          } else {
            this.error = response.result;
          }
          this.customerLoading = false;
        },
        (error) => {
          this.setError(error);
          this.customerLoading = false;
        }
      );
  }

  changeCustomerData(event) {
    this.model.countryIds = [];
    this.countryList = [];
    if (event.id > 0) {
      this.getCountryList();
    }
  }
  getCountryList() {
    this.countryLoading = true;
    let request = new CommonDataSourceRequest();
    request.customerId = this.model.customerId;
    this.invoiceDiscountService
      .getCustomerBussinessCountries(request)
      .pipe()
      .subscribe(
        (data) => {
          if (data && data.result == 1) {
            this.countryList = data.dataSourceList;
          } else {
            this.error = data.result;
          }
          this.countryLoading = false;
        },
        (error) => {
          this.setError(error);
          this.countryLoading = false;
        }
      );
  }

  getInvoiceDiscountTypes() {
    this.invDisTypeLoading = true;
    this.invoiceDiscountService
      .getInvDisTypes()
      .pipe()
      .subscribe(
        (response) => {
          if (response && response.result === 1) {
            this.invDisTypeList = response.dataSourceList;
          }
          this.invDisTypeLoading = false;
        },
        (error) => {
          this.setError(error);
          this.invDisTypeLoading = false;
        }
      );
  }
  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.limitValidators) item.validator.isSubmitted = true;
    if (this.formValid()) {
      this.model.limits = this.limitValidators.map((x) => {
        return x.limit;
      });
      this.saveloading = true;
      this.invoiceDiscountService.saveInvoiceDiscount(this.model).subscribe(
        (res) => {
          if (res && res.result == 1) {
            //this.waitingService.close();
            this.showSuccess(
              "INV_DIS_REGISTER.MSG_SAVE_RESULT",
              "INV_DIS_REGISTER.MSG_SAVE_OK"
            );
            this.saveloading = false;
            this.return("invoicediscountsearch/invoice-discount-summary");
          } else {
            switch (res.result) {
              case 3:
                this.showError(
                  "INV_DIS_REGISTER.MSG_SAVE_RESULT",
                  "INV_DIS_REGISTER.MSG_PERIOD_DATE_ALREADY"
                );
                break;
            }

            //this.waitingService.close();
          }
          this.saveloading = false;
        },
        (error) => {
          this.showError(
            "INV_DIS_REGISTER.MSG_SAVE_RESULT",
            "INV_DIS_REGISTER.MSG_UNKNONW_ERROR"
          );
          this.saveloading = false;
          //this.waitingService.close();
        }
      );
    }
  }

  formValid(): boolean {
    return (
      this.validator.isValid("customerId") &&
      this.validator.isValid("discountType") &&
      this.validator.isValid("countryIds") &&
      this.validator.isValid("periodFrom") &&
      this.validator.isValid("periodTo") &&
      this.limitValidators.every(
        (x, index) =>
          x.validator.isValid("limitFrom") &&
          x.validator.isValid("limitTo") &&
          !this.limitFromValidateLimitTo(index, true)
      )
    );
  }

  limitFromValidateLimitTo(index, isSubmit: boolean = false): boolean {
    if (index <= 0)
      return false;
    const limitValidator = this.limitValidators[index];
    const lastLimitValidator = this.limitValidators[index - 1];
    const result =
      index > 0 &&
      limitValidator.validator.isSubmitted &&
      limitValidator.validator.isValid("limitFrom") &&
      lastLimitValidator.validator.isValid("limitTo") &&
      lastLimitValidator.validator.isSubmitted &&
      lastLimitValidator.limit.limitTo > 0 &&
      limitValidator.limit.limitFrom <= lastLimitValidator.limit.limitTo;
    if (result && isSubmit)
      this.showWarning(
        "Validation error",
        "INV_DIS_REGISTER.MSG_LIMITFROM_GREATER"
      );
    return result;
  }

  addLimit() {
    var limitModel: EditInvoiceDiscountPeriodModel = {
      id: 0,
      limitFrom: null,
      limitTo: null,
      notification: false,
    };
    this.model.limits.push(limitModel);
    this.limitValidators.push({
      limit: limitModel,
      validator: Validator.getValidator(
        limitModel,
        "invoice/invoice-discount-limit.valid.json",
        this.jsonHelper,
        this.validator.isSubmitted,
        this._toastr,
        this._translate
      ),
    });
  }

  removeLimit(index: number) {
    this.model.limits.splice(index, 1);
    this.limitValidators.splice(index, 1);
  }
}
