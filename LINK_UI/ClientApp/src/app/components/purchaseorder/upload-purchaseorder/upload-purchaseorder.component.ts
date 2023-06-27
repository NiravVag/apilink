import { Component, NgModule, OnInit, Input, EventEmitter, Output, SimpleChanges, SimpleChange, OnChanges, ViewChild, ElementRef } from '@angular/core';
import { first, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Validator, WaitingService, JsonHelper } from '../../common'
import { LocationService } from '../../../_Services/location/location.service';
import { CustomerProduct } from '../../../_Services/customer/customerproductsummary.service'
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { PurchaseOrderService } from '../../../_Services/purchaseorder/purchaseorder.service'
import { TranslateService } from "@ngx-translate/core";
import { CustomerService } from '../../../_Services/customer/customer.service';
import { EditCustomerProductModel, ProductScreenCallType } from 'src/app/_Models/customer/editcustomerproduct.model';
import { UploadPurchaseOrderModel, AttachmentFile, ProductDetails } from 'src/app/_Models/purchaseorder/upload-purchaseorder.model';
import { APIService } from "src/app/components/common/static-data-common";

@Component({
  selector: 'app-upload-purchaseorder',
  templateUrl: './upload-purchaseorder.component.html',
  styleUrls: ['./upload-purchaseorder.component.scss']
})
export class UploadPurchaseorderComponent implements OnInit {

  public model: UploadPurchaseOrderModel;
  public customerProductModelList: Array<EditCustomerProductModel>;
  public activeIds: Array<any>;
  public exportDataLoading = false;
  public initialLoading = false;
  public saveButtonLoading = false;
  public saveProductButtonLoading = false;
  public processFileLoading = false;
  public panelnewProducts = true;
  public panelDuplicateError = true;
  public error: '';
  public existingpolist: any;
  public newpolist: any;
  public duplicatePoProducts: Array<any> = []
  public selectednewAll: boolean = false;
  public selectedAll: boolean = false;
  public selectedCustomer: any;
  public isproductListNew: boolean = false;
  public isPoExist: boolean = false;
  public isExportButton: boolean = false;
  public jsonHelper: JsonHelper;
  public poDetailValidators: Array<any> = [];
  public isFileExists: boolean = true;
  public uploadfileimage: string = "assets/images/uploaded-files.svg";
  public SmallUploadTitle = "Upload CSV File";
  public SmallUploadSubTitle = "";
  destinationCountryList: any; 
  customerList: Array<any> = [];
  productList: any;
  poProductList: Array<ProductDetails> = [];
  supplierList: any;
  poList: any;
  poSavedList: any;
  fileSize: number;
  uploadLimit: number;
  totalRecords: number;
  totalUploadRecords: number;
  totalNonUploadRecords: number;
  totalPO: number;
  uploadFileExtensions: string;
  apiServiceEnum = APIService;


  constructor(
    public locationService: LocationService,
    public customerProductService: CustomerProduct,
    public supplierService: SupplierService,
    public service: PurchaseOrderService,

    public translate: TranslateService, public toastr: ToastrService,
    public waitingService: WaitingService,
    public customerService: CustomerService, public validator: Validator
  ) {
    this.validator.setJSON("purchaseorder/upload-purchaseorder.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;
    this.uploadLimit = 1;
    this.fileSize = 5000000;
    this.uploadFileExtensions = 'csv,xlsx';
    this.activeIds = ['ngb-panel-0', 'ngb-panel-1'];
  }

  @ViewChild('panel1') panel1: ElementRef;
  @ViewChild('panel2') panel2: ElementRef;

  getCountryList() {
    this.locationService.getCountrySummary()
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {
            this.destinationCountryList = data.countryList;
          }
          else {
            this.error = data.result;
          }
          this.initialLoading = false;

        },
        error => {
          // this.setError(error);
          this.initialLoading = false;
        });
  }


  toggleOnInit() {
    this.activeIds = ['ngb-panel-0', 'ngb-panel-1'];
    let element;
    this.activeIds.forEach(ele => {
      element = document.querySelector('#' + ele + '-header') as HTMLElement;
      if (element != null)
        element.querySelector('.head-arrow').style.transform = 'rotate(0deg)';
    });
  }

  toggleAccordian1(prop) {

    let element;

    if (prop.panelId == 'ngb-panel-0') {
      element = document.querySelector('#ngb-panel-0-header') as HTMLElement;

      (prop.nextState) ?
        element.querySelector('.head-arrow').style.transform = 'rotate(0deg)' :
        element.querySelector('.head-arrow').style.transform = 'rotate(180deg)';
    }

    if (prop.panelId == 'ngb-panel-1') {
      element = document.querySelector('#ngb-panel-1-header') as HTMLElement;

      (prop.nextState) ?
        element.querySelector('.head-arrow').style.transform = 'rotate(0deg)' :
        element.querySelector('.head-arrow').style.transform = 'rotate(180deg)';
    }
  }
  getProductList(customerId, callfrom) {
    this.customerProductService.getProductsByCustomer(customerId)
      .pipe()
      .subscribe(
        data => {

          if (data) {
            this.productList = data;
            if (callfrom == 2) {
              this.updateProducts();
            }
          }
          else {
            this.error = data.result;
          }

          this.saveProductButtonLoading = false;

        },
        error => {
          //this.setError(error);      
          this.saveProductButtonLoading = false;
        });
  }

  getSupplierList(customerId) {

    this.supplierService.getSuppliersbyCustomer(customerId)
      .pipe()
      .subscribe(
        data => {

          if (data && data.result == 1) {

            this.supplierList = data.data;
          }
          else {
            this.error = data.result;
          }
        },
        error => {
          //  this.setError(error);
        });
  }

  getPurchaseOrderList(customerId, callFrom) {

    this.service.getPurchaseOrderByCustomerId(customerId)
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.poList = data.data;
            if (callFrom == 2) {
              this.updateProducts();
            }
          }
          else {
            this.error = data.result;
          }
        },
        error => {

        });
  }



  savePOList() {

    var tempPoList = this.existingpolist.filter(x => x.selected == true);
    if (tempPoList.length > 0) {
      this.saveButtonLoading = true;
      this.service.savePurchaseOrderList(tempPoList)
        .subscribe(
          res => {
            if (res) {
              this.getPurchaseOrderList(this.model.customerId, 2);
              this.showSuccessMessage(res);
              this.poSavedList = res.savePurchaseOrdersResult;
              this.selectAll(null);
              this.saveButtonLoading = false;
              this.setTotalRecordsandPORecords();
              this.isExportButton = true;
            }
          },
          error => {
            this.saveButtonLoading = false;
            this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
            this.isExportButton = true;

          });
    }
    else {
      this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'PURCHASEORDER_UPLOAD.MSG_PO_SELECT');
    }

  }

  showSuccessMessage(res) {

    if (res.savePurchaseOrdersResult != null && res.savePurchaseOrdersResult.length > 0) {
      this.showSuccess('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.SAVE_OK');
    }

    if (res.bookingPurchaseOrdersResult != null && res.bookingPurchaseOrdersResult.length > 0) {
      var errormessage = 'The below items already mapped for booking ';
      res.bookingPurchaseOrdersResult.forEach(item => {
        errormessage += item.poId + "-" + item.productId + ","
      });
      errormessage = errormessage.replace(/,\s*$/, "");

      this.showWarning('EDIT_PURCHASEORDER.SAVE_RESULT', errormessage);
    }

  }

  clearCountry(item) {
    if (item) {
      item.selected = false;
    }
  }

  onChangeCountry() {
    this.selectAll(null);
  }

  clearSupplier(item) {
    if (item) {
      item.selected = false;
    }
  }

  resetPageData() {
    this.poProductList = [];
    this.newpolist = [];
    this.existingpolist = [];
    this.duplicatePoProducts = [];
    this.model.purchaseOrderAttachments = [];
    this.isproductListNew = false;

  }

  onChangeCustomer() {
    this.resetPageData();
    this.getPurchaseOrderList(this.model.customerId, 1);
    this.getProductList(this.model.customerId, 1);
    this.getSupplierList(this.model.customerId);
  }

  onChangeSupplier() {
    this.selectAll(null);
  }

  onChangePO() {
    this.selectAll(null);
  }

  onChangeProduct() {
    this.selectAll(null);
  }

  export() {
    this.exportDataLoading = true;
    this.downloadFileAsCSV(this.existingpolist, "purchase_Order" + ".csv");
  }

  selectAll(event) {
    for (var i = 0; i < this.existingpolist.length; i++) {

      // Assign customer id 
      if (this.existingpolist[i].productId == 0)
        this.existingpolist[i].productId = null;

      this.existingpolist[i].customerId = this.model.customerId;

      if (this.checkProductList(this.existingpolist[i].productId) &&
        this.checkSupplierList(this.existingpolist[i].supplierId) &&
        this.existingpolist[i].pono != ''
      ) {
        this.existingpolist[i].selected = true;
      }
      else {
        this.existingpolist[i].selected = false;
      }

      if (this.checkPoSavedList(this.existingpolist[i].id)) {
        this.existingpolist[i].purchaseOrderStatus = 1;
      }
      else {
        this.existingpolist[i].purchaseOrderStatus = 2;
      }


    }
    this.selectedAll = this.existingpolist.every(function (item: any) {
      return item.selected == true;
    });

    if (!this.selectedAll) {
      if (event != null) {
        event.target.checked = false;
      }
    }
  }
  checkIfAllSelected(item, data) {

    if (this.checkProductList(data.productId) &&
      this.checkSupplierList(data.supplierId) &&
      data.poId != ''
    ) {
    }
    else {
      item.target.checked = false;
    }

    this.selectedAll = this.existingpolist.every(function (item: any) {
      return item.selected == true;
    })
  }
  selectnewAll() {
    for (var i = 0; i < this.newpolist.length; i++) {
      this.newpolist[i].selected = this.selectednewAll;
    }
  }
  checkIfAllnewSelected() {
    this.selectednewAll = this.newpolist.every(function (item: any) {
      return item.selected == true;
    })
  }

  ngOnInit() {
    this.initialLoading = true;
    this.model = new UploadPurchaseOrderModel();
    this.getCustomerList();
    this.getCountryList();
  }



  selectFiles(event) {
    if (event && !event.error && event.files) {
      this.resetPageData();
      this.model.purchaseOrderAttachments = [];

      if (this.model.purchaseOrderAttachments.length + 1 > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else if (event.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {

        for (let file of event.files) {
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            isNew: true,
            id: 0,
            mimeType: "",
            uniqueld: this.newGuid()
          };
          this.model.purchaseOrderAttachments.push(fileItem);
        }
      }



      //event.srcElement.value = null;
    }
    else if (event && event.error && event.errorMessage) {
      this.showError('EDIT_CUSTOMER_PRODUCT.TITLE', event.errorMessage);
    }
  }

  removeAttachment(index) {
    this.model.purchaseOrderAttachments.splice(index, 1);
    this.resetPageData();
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  getFile(file: AttachmentFile) {
    if (!file.isNew) {
      this.service.getFile(file.id)
        .subscribe(res => {

          //  this.downloadFile(res, file.mimeType);
        },
          error => {
            this.showError("EDIT_PURCHASEORDER.TITLE", "EDIT_PURCHASEORDER.MSG_UNKNOWN_ERROR");
          });
    }
    else {
      const url = window.URL.createObjectURL(file.file);
      window.open(url);
    }
  }
  downloadFileAsCSV(data, fileName) {

    var headerList = ["pono", "product", "productDescription", "etd",
      "quantity", "destinationCountry", "customer", "supplier", "purchaseOrderStatus"];
    // Find the largest element
    var largestEntry = 0;
    var header;
    for (var i = 0; i < data.length; i++) {
      if (!Object.keys) {
        Object.keys = function (obj) {
          var keys = [];
          for (var i in obj) {
            if (obj.hasOwnProperty(i)) {
              keys.push(i);
            }
          }
          return keys;
        };
      }
      if (Object.keys(data[i]).length > largestEntry) {
        largestEntry = Object.keys(data[i]).length;
        header = Object.keys(data[i]);
      }
    };
    // Assemble the header
    var convertedjson = "";
    if (typeof Array.prototype.forEach != 'function') {
      Array.prototype.forEach = function (callback) {
        for (var i = 0; i < this.length; i++) {
          callback.apply(this, [this[i], i, this]);
        }
      };
    }
    header.forEach(function (heading) {
      if (headerList.indexOf(heading) !== -1) {
        if (convertedjson != "") {
          convertedjson += ",";
        }
        convertedjson += "\"";
        convertedjson += heading
        convertedjson += "\"";
      }
    });
    convertedjson += "\r\n";
    // Iterate through the header for all elements
    var dataHeader = '';
    data.forEach(function (entry) {
      header.forEach(function (heading) {
        if (headerList.indexOf(heading) !== -1) {
          // set status text for export data
          if (heading == 'purchaseOrderStatus') {
            dataHeader = (entry[heading] == 2) ? 'Not Uploaded ' : 'Uploaded'
          }
          else {
            dataHeader = entry[heading];
          }
          convertedjson += "\"";
          convertedjson += (dataHeader || "");
          convertedjson += "\"";
          convertedjson += ",";
        }
      });
      convertedjson = convertedjson.substring(0, convertedjson.length - 1);
      convertedjson += "\r\n";
    });

    var uri = 'data:text/csv;charset=utf-8,' + escape(convertedjson);
    var a = document.createElement('a');
    a.href = uri;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(uri);

    this.exportDataLoading = false;
  }

  updateProducts() {
    for (var i = 0; i < this.existingpolist.length; i++) {

      if (this.existingpolist[i].product) {

        var productInfo = this.productList.filter(x => x.productId == this.existingpolist[i].product);

        if (productInfo.length > 0) {
          this.existingpolist[i].productId = productInfo[0].id;
        }
      }

      if (this.existingpolist[i].pono) {

        var poInfo = this.poList.filter(x => x.pono == this.existingpolist[i].pono);
        if (poInfo.length > 0) {
          this.existingpolist[i].poId = poInfo[0].id;
        }
      }

    }

    // sorting by default
    this.existingpolist.sort((a, b) => (a.purchaseOrderStatus > b.purchaseOrderStatus) ? -1 : ((b.purchaseOrderStatus > a.purchaseOrderStatus) ? 1 : 0));

  }

  populateProductList(newProductPOList) {
    var products = this.unique(newProductPOList, 'product');

    this.poProductList = new Array<ProductDetails>();

    products.forEach(item => {

      var po = newProductPOList.filter(x => x.product == item);
      var ponumbers = Array.from(new Set(po.map((item: any) => item.pono)))

      this.poProductList.push({
        product: item, productDescription: po[0].productDescription, barCode: po[0].productBarcode, factoryReference: po[0].ftyRef,
        pono: ponumbers.join(",").replace(/(^,)|(,$)/g, ""), selected: true
      });
    });


  }


  processFile() {
    this.activeIds = ['ngb-panel-0', 'ngb-panel-1'];
    this.validator.isSubmitted = true;
    if (this.isFileFieldsValid()) {
      this.processFileLoading = true;
      var attachedList = [];
      if (this.model.purchaseOrderAttachments)

        attachedList = this.model.purchaseOrderAttachments.filter(x => x.isNew);
      if (attachedList.length > 0) {
        this.service.uploadPurchaseOrder(this.model.customerId, attachedList).subscribe(res => {
          this.processFileSuccessResponse(res);
        },
          error => {
            this.processFileLoading = false;
            this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.MSG_CONNECT_IT_TEAM');
          });
      }
      else {
        this.processFileLoading = false;
        this.showError('PURCHASEORDER_UPLOAD.TITLE', 'PURCHASEORDER_UPLOAD.MSG_FILEUPLOAD_REQ');
      }
    }
    else {
      this.processFileLoading = false;
    }
  }

  // To hanlde success Response after processing the file 

  processFileSuccessResponse(res) {
    if (res.result == 1) {
      this.processFileLoading = false;
      this.existingpolist = res.purchaseOrderUploadList;
      var newProductPOList = this.existingpolist.filter(item => item.isProductNew == true);
      this.checkPOhasNewProduct(newProductPOList);
      this.checkPOExist();
      this.getDuplicatesError();
      this.populateProductList(newProductPOList);
      this.setTotalRecordsandPORecords();
      this.poSavedList = [];
      // copy the actual data for further process.
      this.newpolist = JSON.parse(JSON.stringify(this.poProductList));
      this.selectednewAll = true;
      // this.selectedAll = true;
      this.selectAll(null);



    }
    else if (res.result == 3) {
      this.processFileLoading = false;
      this.showError('EDIT_PURCHASEORDER.SAVE_RESULT', 'EDIT_PURCHASEORDER.PROCESS_RESULT_DATE_FORMAT');
    }
  }

  checkPOExist() {
    if (this.existingpolist.filter(item => item.pono.trimLeft('').trimRight('') != '').length > 0) {
      this.isPoExist = true;
    }

    if ((!this.isPoExist) && (!this.isproductListNew)) {
      this.showWarning('PURCHASEORDER_UPLOAD.TITLE', 'PURCHASEORDER_UPLOAD.LBL_NO_PO_WARNING');
    }
  }

  checkPOhasNewProduct(newProductPOList) {
    if (newProductPOList && newProductPOList.length > 0) {
      this.isproductListNew = true;
    }
    else {
      this.isproductListNew = false;
    }
  }


  setTotalRecordsandPORecords() {
    this.totalRecords = this.existingpolist.length;
    this.totalPO = this.unique(this.existingpolist, 'pono').length - 1;
    this.totalUploadRecords = this.existingpolist.filter(x => x.purchaseOrderStatus == 1).length;
    this.totalNonUploadRecords = this.existingpolist.filter(x => x.purchaseOrderStatus == 2).length;
  }

  checkFileExists() {
    if (this.model.purchaseOrderAttachments) {
      if (this.model.purchaseOrderAttachments.length > 0)
        this.isFileExists = true;
      else
        this.isFileExists = false;
    }
    else {
      this.isFileExists = false;
    }
  }
  getDuplicatesError() {
    var duplicateItem = [];
    this.duplicatePoProducts = [];
    var dataList = this.existingpolist;
    var dataMainList = this.existingpolist;
    var lastSelectedPO = '';
    dataList.forEach(item => {
      var checkItemExist = dataMainList.filter(x => x.pono == item.pono);
      if (checkItemExist.length > 1) {
        if (lastSelectedPO != item.pono) {

          var productList = checkItemExist.map(function (item) { return item.product; }).join();

          // remove duplicates product
          productList = productList.split(',').filter(function (item, index, self) {
            return self.indexOf(item) == index;
          });

          duplicateItem.push({ 'poId': item.pono, 'productId': productList, 'error': 'Duplicate Item' });

          lastSelectedPO = item.pono;
        }
      }
    });
    this.duplicatePoProducts = duplicateItem;
    this.toggleOnInit();

  }



  SaveCustomerProductService() {
    this.customerProductService.saveCustomerProductList(this.customerProductModelList)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.getProductList(this.model.customerId, 2);
            this.newpolist = this.newpolist.filter(x => x.selected == false);
            if (this.newpolist.length == 0)
              this.isproductListNew = false;
            //this.waitingService.close();

            this.showSuccess('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.SAVE_OK');
          }
          else {
            switch (res.result) {
              case 2:
                this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CANNOT_ADDCUSTOMERPRODUCT');
                break;
              case 3:
                this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_PRODUCT_NOT_FOUND');
                break;
              case 4:
                this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_CUSTOMER_PRODUCT_EXISTS');
                break;
            }

            // this.waitingService.close();
            this.saveProductButtonLoading = false;
          }
        },
        error => {
          this.showError('EDIT_CUSTOMER_PRODUCT.SAVE_RESULT', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
          //  this.waitingService.close();
          this.saveProductButtonLoading = false;
        });
  }
  //Check if the produt id and description is empty
  isProductDataValid(productList) {
    var isOk = true;
    productList.forEach(element => {
      if (element.product == "" || element.product == null) {
        isOk = false;
        this.showError('PURCHASEORDER_UPLOAD.LBL_WARNING', 'PURCHASEORDER_UPLOAD.MSG_PRODUCT_DATA_REQUIRED');
      }
      else if (element.productDescription == "" || element.productDescription == null) {
        isOk = false;
        this.showError('PURCHASEORDER_UPLOAD.LBL_WARNING', 'PURCHASEORDER_UPLOAD.MSG_PRODUCT_DESC_REQUIRED');
      }
    });
    return isOk;
  }

  saveCustomerProduct() {
    var selectedList = this.newpolist.filter(x => x.selected);
    if (this.isProductDataValid(selectedList)) {
      this.saveProductButtonLoading = true;
      this.customerProductModelList = new Array<EditCustomerProductModel>();
      if (selectedList.length > 0) {
        var apiServiceIds = [];
        apiServiceIds.push(this.apiServiceEnum.Inspection);
        selectedList.forEach(element => {
          this.customerProductModelList.push({
            id: 0, productID: element.product, productDescription: element.productDescription,
            customerID: this.model.customerId, barcode: element.barCode, photo: "", productCategory: null, productSubCategory: null, productCategorySub2: null,
            remarks: null, cuProductFileAttachments: null, factoryReference: element.factoryReference, isProductBooked: false, isBooked: false, apiServiceIds: apiServiceIds,
            isNewProduct:true,isMsChart:false,isStyle:false, productCategorySub3:null, timePreparation: null, sampleSize8h: null, tpAdjustmentReason: null,
            unit: null, technicalComments: null,screenCallType:ProductScreenCallType.PoUpload,productCategoryName:null,productSubCategoryName:null,
            productCategorySub2Name:null,productCategorySub3Name:null
          });
        });

        this.SaveCustomerProductService();
      }
      else {
        this.saveProductButtonLoading = false;
        this.showError('PURCHASEORDER_UPLOAD.LBL_WARNING', 'PURCHASEORDER_UPLOAD.MSG_PRODUCT_REQUIRED');
      }
    }

  }

  unique(arr, prop) {
    return arr.map(function (e) {
      return e[prop];
    }).filter(function (e, i, a) {
      return i === a.indexOf(e);
    });
  }


  public showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle);
  }

  public showWarning(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.warning(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }


  public showSuccess(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.success(tradMessage, tradTitle);
  }
  getCustomerList() {
    this.customerService.getCustomerSummary()
      .pipe()
      .subscribe(
        response => {

          if (response && response.result == 1) {
            this.customerList = response.customerList;
          }
          else {
            this.error = response.result;
          }
          this.initialLoading = false;
        },
        error => {
          //this.setError(error);
          this.initialLoading = false;
        });
  }


  checkPOList(poId): boolean {
    if (this.poList == null || this.poList.length == 0)
      return false;
    var data = this.poList.filter(x => x.id == poId);
    return data.length == 1;
  }

  checkProductList(productId) {
    if (this.productList == null || this.productList.length == 0)
      return false;
    var data = this.productList.filter(x => x.id == productId);
    return data.length == 1;
  }

  checkCountryList(countryId) {
    if (this.destinationCountryList == null || this.destinationCountryList.length == 0)
      return false;
    var data = this.destinationCountryList.filter(x => x.id == countryId);
    return data.length == 1;
  }

  checkSupplierList(supplierId) {
    if (this.supplierList == null || this.supplierList.length == 0)
      return false;
    var data = this.supplierList.filter(x => x.id == supplierId);
    return data.length == 1;
  }

  checkPoSavedList(excelID) {
    if (this.poSavedList == null || this.poSavedList.length == 0)
      return false;
    var data = this.poSavedList.filter(x => x.id == excelID);
    return data.length == 1;
  }

  isFileFieldsValid() {

    this.checkFileExists();
    return (this.validator.isValid('customerId'));
  }

  isSaveValid() {
    if (this.model.purchaseOrderAttachments) {
      if (this.model.purchaseOrderAttachments.length > 0) {
        return true;
      }
      else if (this.model.purchaseOrderAttachments.length == 0) {
        return false;
      }
    }
  }

}
