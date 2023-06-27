import { Component, OnInit } from '@angular/core';
import { SummaryComponent } from '../../common/summary.component';
import { InvoiceBankSummary, InvoiceBankGetAllResponse, InvoiceBankGetAllResult, bankSummaryMasterData, InvoiceBankDeleteResponse, InvoiceBankDeleteResult } from 'src/app/_Models/invoice/invoicebank';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap/modal/modal-ref';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { JsonHelper, Validator } from '../../common';
import { InvoiceBankService } from 'src/app/_Services/invoice/invoicebank.service';
import { PageSizeCommon } from '../../common/static-data-common';
import { first } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UtilityService } from 'src/app/_Services/common/utility.service';

@Component({
  selector: 'app-invoice-bank',
  templateUrl: './invoice-bank.component.html',
  styleUrls: ['./invoice-bank.component.scss']
})
export class InvoiceBankComponent extends SummaryComponent<InvoiceBankSummary>{
  
  private modelRef: NgbModalRef;
  private _translate: TranslateService;
  private _toastr: ToastrService; 
  public masterData:bankSummaryMasterData;
  public model: InvoiceBankSummary;
  public bankId:number;
  public accountNumber:string;
  public pageLoader:boolean=false;
  
  constructor(router: Router, validator: Validator, route: ActivatedRoute, translate: TranslateService,
    toastr: ToastrService, public utility: UtilityService, public bankService: InvoiceBankService,public modalService: NgbModal,) 
  {
    super(router, validator,  route, translate, toastr);
    this._toastr = toastr;
    this._translate = translate;  
  }

  onInit() 
  {
    this.pageLoader=true;
    this.model=new InvoiceBankSummary();
    this.model.pageSize=PageSizeCommon[0];
    this.model.index=0;
    this.GetSearchData();
  }

  GetSearchData() { 

  this.bankService.getAllBankDetails(this.model).
  pipe()
  .subscribe(
    response => {
      if (response) {

      this.mapPageProperties(response);

        switch (response.result) {
          case InvoiceBankGetAllResult.success:
            this.model.items=response.bankDetails;
            break;              
            case InvoiceBankGetAllResult.invoiceBankNotFound:
              this.model.items=[];
              this.model.noFound=true;
              break;   
            case InvoiceBankGetAllResult.failure:
              this.model.items=[];
              this.model.noFound=true;
              break;         
        }      
      }    
      this.pageLoader=false;
    },
    error => {
      this.pageLoader=false;
      this.setError(error);
    })
   
  }

  
 async deleteBank() {
   let response:InvoiceBankDeleteResponse
    try 
     {
      if(this.bankId)
      {
        response = await this.bankService.deleteBankDetails(this.bankId);          
      }         
       switch (response.result) 
       {
         case InvoiceBankDeleteResult.success:      
         this.modelRef.close();      
         this.showSuccess('EDIT_INV_BANK.TITLE', 'EDIT_INV_BANK.MSG_DELETE_SUCCESS');
         this.onInit();  
           break;       
       
         default:
           break;
       }
     }
     catch (e) {
       this.setError(e);
     }   

  }


  confirmBankDelete(content, bankId,accountNumber) {

    this.bankId=bankId;
    this.accountNumber=accountNumber;

    this.modelRef = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true,backdrop: 'static' });
    this.modelRef.result.then((result) => {

      this.bankId=null;
      this.accountNumber='';

    }, (reason) => {
    });
  }
  

  getEditDetails(id) {     
      this.getDetails(id);    
  }

  getPathDetails(): string
  {
    return "invoicebankedit/edit-invoice-bank";
  }

  getData(): void 
  {
    this.GetSearchData();
  }
}
