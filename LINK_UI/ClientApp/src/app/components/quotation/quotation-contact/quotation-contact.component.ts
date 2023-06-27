import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { QuotationEntityType, QuotationEntityContact, DataSource, QuotationContactListResult, AddressFactoryResult, QuotationBillPaidBy, PaymentTerm } from '../../../_Models/quotation/quotation.model';
import { QuotationService } from '../../../_Services/quotation/quotation.service';
import { JsonHelper, Validator } from '../../common';
import { SupplierService } from 'src/app/_Services/supplier/supplier.service';
@Component({
  selector: 'app-quotationContact',
  templateUrl: './quotation-contact.component.html',
  styleUrls: ['./quotation-contact.component.css']
})

export class QuotationContactComponent implements OnInit {
  @Input() type: QuotationEntityType;
  @Input() dataSource: Array<DataSource>;
  @Input() label: string;
  @Input() legalName: string;

  @Input() dropdownloading: boolean;
  @Input() data: Array<QuotationEntityContact>;
  @Input() id: DataSource;
  @Input() parentId: number;
  @Input() address: string;
  @Output() selectItem = new EventEmitter<DataSource>();
  @Input() model: Array<QuotationEntityContact>;
  @Input() jsonPathValidation: string;
  @Input() fieldtype: string;
  @Input() isInternaluser: boolean;
  @Input() quotationId: number;
  @Input() isFromBookingSummary: boolean;
  @Input() grade: string;
  @Input() billPaidBy: number;

  @Input() paymentTerm: number;

  paymentTermEnum = PaymentTerm;

  public currentValidator: Validator
  // labelLoading: boolean = false;
  contactLoading: boolean = false;

  constructor(public validator: Validator, private supplierService: SupplierService, private service: QuotationService, private translate: TranslateService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.validator.isSubmitted = false;
    this.currentValidator = Validator.getValidator(this, this.jsonPathValidation, this.validator.jsonHelper, false, this.toastr, this.translate);
  }

  ngOnChanges(changes: SimpleChanges) {
    // this.labelLoading = true;
    var keyNames = Object.keys(changes);

    if (keyNames != null && keyNames.length > 0 && keyNames[0] == "dataSource") {
      if (this.id != null)
        this.refreshData();

    }
    // this.labelLoading = false;
  }

  public async refreshData() {

    // this.labelLoading = true;
    if (this.id != null && this.id.id > 0) {
      //send request to  get contact data
      this.data = [];
      let response = await this.service.getContactList(this.id.id, this.type, this.parentId);

      // this.labelLoading = false;
      if (response != null) {

        switch (response.result) {
          case QuotationContactListResult.Success:
            //this.data = response.data;
            this.data = [...response.data];

            //type should be internal and quotation id non greater than zero
            if (this.type == QuotationEntityType.Internal && !(this.quotationId > 0)) {
              this.model = this.data.filter(x => x.customerAE);
              this.model.forEach(x => {
                x.email = true;
                x.quotation = true;
                x.invoiceEmail = true;
              });
            }
            break;
          case QuotationContactListResult.NotFound:
            // this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_COUNTRYISEMPTY');
            break;
        }
      }
      // if factory search address
      if (this.type == QuotationEntityType.Factory) {

        let resp = await this.service.getAddressFactory(this.id.id);
        if (resp.result == AddressFactoryResult.Success) {
          this.address = resp.address;
        }
      }
    }
  }

  async updateData(item: DataSource) {

    this.data = [];
    this.model = null;
    this.legalName = null;
    this.address = null;
    this.grade = '';
    if (item) {
      //send request to  get contact data
      this.contactLoading = true;
      let response = await this.service.getContactList(item.id, this.type, this.parentId);
      this.contactLoading = false;
      if (response != null) {

        switch (response.result) {
          case QuotationContactListResult.Success:
            //this.data = response.data;
            this.data = [...response.data];

            //type should be internal and quotation id non greater than zero
            if (this.type == QuotationEntityType.Internal && !(this.quotationId > 0)) {
              this.model = this.data.filter(x => x.customerAE);
              this.model.forEach(x => {
                x.email = true;
                x.quotation = true;
                x.invoiceEmail = true;
              });
            }
            break;
          case QuotationContactListResult.NotFound:
            this.showError('QUOTATION.TITLE', 'QUOTATION.MSG_CONTCATLISTISEMPTY');
            break;
        }
      }

      // put  the legal name by default
      this.legalName = item.name;

      // if factory search address
      if (this.type == QuotationEntityType.Factory) {

        let resp = await this.service.getAddressFactory(item.id);
        if (resp.result == AddressFactoryResult.Success) {
          this.address = resp.address;
        }
      }
    }
    // emit event for parent
    this.selectItem.emit(item);

  }

  private showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle);
  }


}
