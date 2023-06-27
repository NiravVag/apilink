import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { Validator, JsonHelper } from '../../common'
import { InspectionPicking, InspectionPickingContact } from 'src/app/_Models/booking/inspectionpicking.model';
import { InspectionPickingService } from '../../../_Services/booking/inspectionpicking.service'
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { first, retry, map } from 'rxjs/operators';
import { LabType, BookingStatus } from '../../common/static-data-common'
import { ok } from 'assert';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomkpiModel } from 'src/app/_Models/kpi/customkpimodel';
@Component({
  selector: 'app-edit-inspectionpicking',
  templateUrl: './edit-inspectionpicking.component.html',
  styleUrls: ['./edit-inspectionpicking.component.scss']
})
export class EditInspectionPickingComponent extends DetailComponent {

  public pickingProductList: any;
  public poTransactionList: any;
  public productList: any;
  public poList: any;
  public labList: any;
  public labAddressList: any;
  public labContactList: any;
  public totalPickingQuantity: number;
  public jsonHelper: JsonHelper;
  public model: any;
  public customerID: number;
  public statusId: number;
  public inspectionPickingList: any;
  public saveInspectionPickingList: any;
  public inspectionId: number;
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public initialLoading = false;
  _BookingStatus = BookingStatus;
  savedataloading = false;
  _isPreview = true;
  labLoading = false;
  _labType = LabType;
  public viewImagePath: string;
  public closeImagePath: string;
  public exportDataLoading: boolean = false;
  inspectionPickingFound: boolean = false;
  constructor(
    toastr: ToastrService,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public validator: Validator,
    public service: InspectionPickingService,
    public utility: UtilityService
  ) {
    super(router, route, translate, toastr);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("booking/edit-booking-picking.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.poTransactionList = [];
    this.productList = [];
    this.poList = [];
    this.labList = [];
    this.labAddressList = [];
    this.labContactList = [];
    this.inspectionPickingList = [];
    this.saveInspectionPickingList = [];
    this._toastr = toastr;
    this._translate = translate;
    this.viewImagePath = "assets/images/cta-view.svg";
    this.closeImagePath = "assets/images/cta-close.svg";
  }

  onInit(id?: any, param?: any): void {
    this.model = new Array<InspectionPicking>();
    this.inspectionId = id;
    this.customerID = (param) ? (param.params.customerid) ? param.params.customerid : 0 : 0;
    this.statusId = (param) ? (param.params.statusId) ? param.params.statusId : 0 : 0;
    this.initialLoading = true;
    this.initialize(id, this.customerID);
  }

  getViewPath(): string {
    return "inspcombine/edit-combineorders";
  }

  getEditPath(): string {
    return "inspcombine/edit-combineorders";
  }

  initialize(id, customerID) {

    //Get the lab list from the server
    this.getlabList(this.customerID);
    this.getInspectionPickingList(id);


  }

  getPickingData() {

    let bookingId = this.inspectionId;
    this.service.GetPickingPdf(bookingId)
      .pipe()
      .subscribe(res => {
        this.downloadPDFFile(res, "application/pdf");

      },
        error => {
          this.showError('BOOKING_SUMMARY.LBL_PICKING', 'BOOKING_SUMMARY.ADDRESS_NOTFOUND');

        });
  }
  downloadPDFFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.pdf");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
  }

  // Get the inspection picking list by booking id
  getInspectionPickingList(bookingID) {
    this.service.GetInspectionPickingList(bookingID)
      .pipe()
      .subscribe(
        res => {
          this.processInspPickingSuccessResponse(res);
        },
        error => {
          this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          this.initialLoading = false;
        }
      );
  }

  populatePoTransactionList(pickingProductList) {
    this.poTransactionList=[];
    this.productList=[];
    this.poList=[];

    pickingProductList.forEach(item => {
      this.poTransactionList.push(
        {
          "productID": item.productID, "productName": item.productName, "poID": item.poID, "poName": item.poName,
          "pickingQuantity": item.pickingQuantity, "poTransactionID": item.poTransactionID
        }
      );

      if (this.productList && !this.productList.find(x => x.productID == item.productID)) {
        this.productList.push(
          {
            "productID": item.productID, "productName": item.productName
          }
        );
      }

      if (this.poList && !this.poList.find(x => x.poID == item.poID)) {
        this.poList.push(
          {
            "poID": item.poID, "poName": item.poName
          }
        );
      }
    });
  }

  //If user deleted the exiting po transaction in the picking saving
  //then next time it has to come as separte products(means save data will come from pickingtransaction table
  //and deleted data comes from the potransaction table) 
  processRemovedTransactions(inspectionPickingList) {
    var removedPoTransactionIds = inspectionPickingList.map(x => x.poTranId);
    var selectedbookingProducts = this.poTransactionList.filter(x => !removedPoTransactionIds.includes(x.poTransactionID));
    //populate picking transaction list from the po transaction table
    if (selectedbookingProducts.length > 0) {
      this.addPickingListByBookingProducts(selectedbookingProducts);
    }
  }

  //process initialization success response
  processInspPickingSuccessResponse(res) {
    if (res.pickingProductList)
      //populate po transaction list,polist and product list
      this.populatePoTransactionList(res.pickingProductList);
    if (res.result == 1) {
      //populate picking transaction list from the po transaction table
      this.addPickingListByBookingProducts(res.pickingProductList);
      this.getTotalPickingData(false);
    }
    else if (res.result == 2) {
      //populate picking transaction list from the po transaction table
      //and removed data from insp picking transactions
      this.addInpsectionListByPickingProducts(res.inspectionPickingList);
      this.processRemovedTransactions(res.inspectionPickingList);
      this.getTotalPickingData(false);
      //check inspection picking data is there in db
      if (res.inspectionPickingList && res.inspectionPickingList.filter(x => x.labAddressId != null || x.cusAddressId != null).length > 0)
        this.inspectionPickingFound = true;
    }
    this.sortPickingDataList();
    this.initialLoading = false;
  }

  //push inspection picking product row by booking products
  addPickingListByBookingProducts(productList) {

    productList.forEach(item => {
      var inspectionPickingData: InspectionPicking = {
        id: 0,
        labId: null,
        labName: "",
        showLabName: false,
        labNameImgPath: this.viewImagePath,
        productId: item.productID,
        poId: item.poID,
        customerId: this.customerID,
        labAddressId: null,
        labAddress: "",
        showLabAddress: false,
        labAddressImgPath: this.viewImagePath,
        cusAddressId: null,
        poTranId: item.poTransactionID,
        pickingQuantity: item.pickingQuantity,
        remarks: "",
        active: true,
        labContactTypeItems: null,
        inspectionPickingContacts: null,
        labAddressList: null,
        labContactList: null,
        labType: null,
        labAddressLoading: false,
        labContactLoading: false
      }
      this.model.push({ inspectionPickingData: inspectionPickingData, validator: Validator.getValidator(inspectionPickingData, "booking/edit-booking-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });

    });
  }

  sortPickingDataList() {
    this.model.sort((a, b) =>
      (a.inspectionPickingData.poId != null ? a.inspectionPickingData.poId : Infinity) -
      (b.inspectionPickingData.poId != null ? b.inspectionPickingData.poId : Infinity)
      || (a.inspectionPickingData.productId != null ? a.inspectionPickingData.productId : Infinity) -
      (b.inspectionPickingData.productId != null ? b.inspectionPickingData.productId : Infinity));
  }

  //push inspection picking product row by picking products
  addInpsectionListByPickingProducts(pickingProductList) {

    pickingProductList.forEach(item => {
      var poTransaction = this.poTransactionList.find(x => x.poTransactionID == item.poTranId);

      var inspectionPickingData: InspectionPicking = {
        id: item.id,
        labId: (item.labAddressId) != null ? item.labId : (item.cusAddressId) != null ? -1 : null,
        labName: "",
        labNameImgPath: this.viewImagePath,
        showLabName: false,
        productId: poTransaction != null ? poTransaction.productID : null,
        poId: poTransaction != null ? poTransaction.poID : null,
        customerId: this.customerID,
        labAddressId: (item.labAddressId) != null ? item.labAddressId : (item.cusAddressId) != null ? item.cusAddressId : null,
        labAddress: "",
        labAddressImgPath: this.viewImagePath,
        showLabAddress: false,
        cusAddressId: (item.cusAddressId) != null ? item.cusAddressId : (item.labAddressId) != null ? item.labAddressId : null,
        poTranId: item.poTranId,
        pickingQuantity: item.pickingQuantity,
        remarks: item.remarks,
        active: item.active,
        labContactTypeItems: this.getLabContactItems(item.inspectionPickingContacts, (item.labAddressId) != null ? this._labType.Lab : this._labType.Customer),
        inspectionPickingContacts: item.inspectionPickingContacts,
        labAddressList: null,
        labContactList: null,
        labContactLoading: false,
        labAddressLoading: false,
        labType: (item.labAddressId) != null ? this._labType.Lab : (item.cusAddressId) != null ? this._labType.Customer : null,
      }
      //inspectionPickingData.labAddressList=this.getlabAddressList(inspectionPickingData.id,inspectionPickingData);
      this.model.push({ inspectionPickingData: inspectionPickingData, validator: Validator.getValidator(inspectionPickingData, "booking/edit-booking-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
      //this.model.push(inspectionPickingData);
    });


    this.model.forEach(element => {
      if (element.inspectionPickingData.labType == this._labType.Lab) {
        this.getlabAddressList(element.inspectionPickingData.labId, element);
        this.getlabContactList(element.inspectionPickingData.labId, element);
      }
      else if (element.inspectionPickingData.labType == this._labType.Customer) {
        this.getCustomerContacts(this.customerID, element);
      }

    });
  }

  showLabName(inspectionPicking) {
    if (inspectionPicking.inspectionPickingData.labId != null) {
      inspectionPicking.inspectionPickingData.labName = this.getInspLabName(inspectionPicking.inspectionPickingData.labId);
      inspectionPicking.inspectionPickingData.showLabName = !inspectionPicking.inspectionPickingData.showLabName;
      inspectionPicking.inspectionPickingData.labNameImgPath = (inspectionPicking.inspectionPickingData.showLabName) ? this.closeImagePath : this.viewImagePath;
    }
  }
  getInspLabName(labId: number) {

    let _labName = "";
    if (this.labList != null && this.labList.length > 0) {
      _labName = this.labList.filter(x => x.id == labId)[0].name;
    }
    return _labName;
  }
  showLabAddress(inspectionPicking) {
    if (inspectionPicking.inspectionPickingData.labAddressId != null) {
      inspectionPicking.inspectionPickingData.labAddress = this.getLabAddress(inspectionPicking);
      inspectionPicking.inspectionPickingData.showLabAddress = !inspectionPicking.inspectionPickingData.showLabAddress;
      inspectionPicking.inspectionPickingData.labAddressImgPath = (inspectionPicking.inspectionPickingData.showLabAddress) ? this.closeImagePath : this.viewImagePath;
    }
  }
  getLabAddress(inspectionPicking) {

    let _labAddress = "";
    if (inspectionPicking.inspectionPickingData.labAddressList != null && inspectionPicking.inspectionPickingData.labAddressList.length > 0) {
      _labAddress = inspectionPicking.inspectionPickingData.labAddressList.filter(x => x.id == inspectionPicking.inspectionPickingData.labAddressId)[0].name;
    }
    return _labAddress;
  }



  //Populate contact types from pickingcontact object(for multidropdown)
  getLabContactItems(inspectionPickingContacts, labType) {

    var contactTypes = [];
    inspectionPickingContacts.forEach(item => {
      if (labType == this._labType.Lab)
        contactTypes.push(item.labContactId);
      else if (labType == this._labType.Customer)
        contactTypes.push(item.cusContactId);
    });
    return contactTypes;
  }


  // Get the lab list from the server
  getlabList(customerID) {
    this.labLoading = true;
    this.service.GetLabList(customerID)
      .pipe()
      .subscribe(
        res => {
          if (res.result == 1) {
            //To bind the lab list fetched from the server
            if (res.labList) {
              this.labList = res.labList;
            }
          }
          else if (res.result == 2) {
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABFOUND");
          }
          this.labLoading = false;
        },
        error => {
          this.labLoading = false;
          this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
        }
      );
  }

  //Lab DropDown Change Event
  getLabRelatedDetails(lab, inspectionPicking) {
    this._isPreview = false;
    inspectionPicking.inspectionPickingData.labAddressList = null;
    inspectionPicking.inspectionPickingData.labContactList = null;
    inspectionPicking.inspectionPickingData.labAddressId = null;
    inspectionPicking.inspectionPickingData.labContactTypeItems = null;

    if (inspectionPicking.inspectionPickingData.labId != null) {
      inspectionPicking.inspectionPickingData.labName = this.getInspLabName(inspectionPicking.inspectionPickingData.labId);
    }
    else {
      inspectionPicking.inspectionPickingData.labName = "";
      inspectionPicking.inspectionPickingData.showLabName = false;
      inspectionPicking.inspectionPickingData.labNameImgPath = this.viewImagePath;
    }
    inspectionPicking.inspectionPickingData.labAddress = "";
    inspectionPicking.inspectionPickingData.showLabAddress = false;
    inspectionPicking.inspectionPickingData.labAddressImgPath = this.viewImagePath;

    inspectionPicking.inspectionPickingData.labType = lab.type;
    if (lab.type == this._labType.Lab) {
      this.getlabAddressList(lab.id, inspectionPicking);
      this.getlabContactList(lab.id, inspectionPicking);
    }
    else if (lab.type == this._labType.Customer) {
      this.getCustomerContacts(this.customerID, inspectionPicking);
    }
  }

  clearLabRelatedDetails(inspectionPicking) {
    inspectionPicking.inspectionPickingData.labAddressList = null;
    inspectionPicking.inspectionPickingData.labContactList = null;
    inspectionPicking.inspectionPickingData.labAddressId = null;
    inspectionPicking.inspectionPickingData.labContactTypeItems = null;

    inspectionPicking.inspectionPickingData.labName = "";
    inspectionPicking.inspectionPickingData.showLabName = false;
    inspectionPicking.inspectionPickingData.labNameImgPath = this.viewImagePath;

    inspectionPicking.inspectionPickingData.labAddress = "";
    inspectionPicking.inspectionPickingData.showLabAddress = false;
    inspectionPicking.inspectionPickingData.labAddressImgPath = this.viewImagePath;

  }

  // Lab address DropDown Change Event
  getLabAddressRelatedDetails(inspectionPicking) {
    this._isPreview = false;
    if (inspectionPicking.inspectionPickingData.labAddressId != null) {
      inspectionPicking.inspectionPickingData.labAddress = this.getLabAddress(inspectionPicking);
    }
    else {
      inspectionPicking.inspectionPickingData.labAddress = "";
      inspectionPicking.inspectionPickingData.showLabAddress = false;
      inspectionPicking.inspectionPickingData.labAddressImgPath = this.viewImagePath;
    }
  }
  clearLabAddressRelatedDetails(inspectionPicking) {
    inspectionPicking.inspectionPickingData.labAddress = "";
    inspectionPicking.inspectionPickingData.showLabAddress = false;
    inspectionPicking.inspectionPickingData.labAddressImgPath = this.viewImagePath;
  }

  //Fetch the lab address details by lab id
  getlabAddressList(labID, inspectionPicking) {
    inspectionPicking.inspectionPickingData.labAddressLoading = true;
    if (labID) {
      this.service.GetLabAddressList(labID)
        .pipe()
        .subscribe(
          res => {
            if (res.result == 1) {

              if (res.labAddressList) {
                inspectionPicking.inspectionPickingData.labAddressList = res.labAddressList;

                //assign the address in the selected list if there is only one address and if it's a new row
                if (res.labAddressList.length == 1 && inspectionPicking.inspectionPickingData.labAddressId == null) {
                  inspectionPicking.inspectionPickingData.labAddressId = res.labAddressList[0].id;
                }
              }
            }
            else if (res.result == 2) {
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABADDRESSFOUND");
            }
            inspectionPicking.inspectionPickingData.labAddressLoading = false;
          },
          error => {
            inspectionPicking.inspectionPickingData.labAddressLoading = false;
            this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }


  }

  //Fetch the lab contact details by lab id
  getlabContactList(labID, inspectionPicking) {
    inspectionPicking.inspectionPickingData.labContactLoading = true;
    if (labID) {
      this.service.GetLabContactList(labID, this.customerID)
        .pipe()
        .subscribe(
          res => {
            if (res.result == 1) {
              if (res.labContactList) {
                inspectionPicking.inspectionPickingData.labContactList = res.labContactList;

                //assign the contact in the selected list if there is only one contact and if it's a new row
                if (res.labContactList.length == 1 && inspectionPicking.inspectionPickingData.labContactTypeItems == null) {
                  inspectionPicking.inspectionPickingData.labContactTypeItems = []
                  inspectionPicking.inspectionPickingData.labContactTypeItems.push(res.labContactList[0].id);
                }
              }
            }
            else if (res.result == 2) {
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOLABADDRESSFOUND");
            }
            inspectionPicking.inspectionPickingData.labContactLoading = false;
          },
          error => {
            inspectionPicking.inspectionPickingData.labContactLoading = false;
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  // Get the customer contacts from the customerid
  getCustomerContacts(labId, inspectionPicking) {
    //this.labContactLoading = true;
    inspectionPicking.inspectionPickingData.labContactLoading = true;
    inspectionPicking.inspectionPickingData.labAddressLoading = true;
    if (labId) {
      this.service.GetCustomerContacts(labId)
        .pipe()
        .subscribe(
          res => {
            if (res.result == 1) {


              if (res.customerAddressList) {


                this.labAddressList = res.customerAddressList.map((y) => {
                  var lab = {
                    id: y.id,
                    name: y.address
                  };
                  return lab;
                });
                inspectionPicking.inspectionPickingData.labAddressList = this.labAddressList;

                //assign the address in the selected list if there is only one address and if it's a new row
                if (this.labAddressList.length == 1 && inspectionPicking.inspectionPickingData.labAddressId == null) {
                  inspectionPicking.inspectionPickingData.labAddressId = this.labAddressList[0].id;
                }
                inspectionPicking.inspectionPickingData.labAddressLoading = false;
              }
              if (res.customerContactList) {

                this.labContactList = res.customerContactList.map((y) => {
                  var labContact = {
                    id: y.id,
                    name: y.contactName,
                    active: y.active
                  };
                  return labContact;
                });
                inspectionPicking.inspectionPickingData.labContactList = this.labContactList;


                ////assign the contact in the selected list if there is only one contact and if it's a new row
                if (inspectionPicking.inspectionPickingData.labContactTypeItems == null) {
                  //filter for active contacts if it's a new row because we are displaying inactive contacts if selected already
                  inspectionPicking.inspectionPickingData.labContactList = this.labContactList.filter(x => x.active);
                  if (this.labContactList.length == 1) {
                    inspectionPicking.inspectionPickingData.labContactTypeItems = []
                    inspectionPicking.inspectionPickingData.labContactTypeItems.push(this.labContactList[0].id);
                  }
                }
                inspectionPicking.inspectionPickingData.labContactLoading = false;
              }
            }
            else if (res.result == 2) {
              this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.LBL_NOCUSTOMERCONTACTSFOUND");
            }
          },
          error => {
            inspectionPicking.inspectionPickingData.labContactLoading = false;
            inspectionPicking.inspectionPickingData.labAddressLoading = false;
            this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
          }
        );
    }
  }

  validatePoAndProductMapped(inspectionPicking) {
    var productName = this.poTransactionList.find(x => x.productID == inspectionPicking.inspectionPickingData.productId).productName;
    var poName = this.poTransactionList.find(x => x.poID == inspectionPicking.inspectionPickingData.poId).poName;
    var warningMessage = this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_SELECTED") + poName
      + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_ANDPRODUCT") + productName
      + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_NOTMAPPED");
    this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", warningMessage);
    return false;
  }

  changeProductNumber(product, inspectionPicking) {
    this._isPreview = false;
    if (inspectionPicking.inspectionPickingData.productId && inspectionPicking.inspectionPickingData.poId) {
      var poTransaction = this.poTransactionList.find(x => x.poID == inspectionPicking.inspectionPickingData.poId
        && x.productID == inspectionPicking.inspectionPickingData.productId);
      if (poTransaction) {
        inspectionPicking.inspectionPickingData.poTranId = poTransaction.poTransactionID;
      }
      else {
        this.validatePoAndProductMapped(inspectionPicking);
      }
    }
    this.sortPickingDataList();
  }

  save() {
    this.validator.initTost();
    //Map Customer Dropdownlist to customer contact object
    this.MapLabAddressContacts(this.model);
    this.saveInspectionPickingList = [];
    this.model.forEach(element => {
      this.saveInspectionPickingList.push(element.inspectionPickingData);
    });

    if (this.isInspectionDetailsValid() && this.validateTransactionMapped()) {
      //this.saveloading=true;
      //this.waitingService.open();
      //this.model = this.getValues(this.model.contactTypeItems);
      if (this.validatePickingQuantity()) {
        this.savedataloading = true;
        this.service.saveInspectionPicking(this.saveInspectionPickingList, this.inspectionId)
          .subscribe(
            res => {

              if (res && res.result == 1) {
                this.model = [];
                this.getInspectionPickingList(this.inspectionId);
                this.savedataloading = false;
                this._isPreview = true;
                this.showSuccess('BOOKING_INSPECTIONPICKING.LBL_SAVE_RESULT', 'BOOKING_INSPECTIONPICKING.MSG_SAVE_SUCCESS');
                //this.return('inspsummary/booking-summary');
              }
              else {
                switch (res.result) {
                  case 2:
                    this.showError('BOOKING_INSPECTIONPICKING.SAVE_RESULT', 'BOOKING_INSPECTIONPICKING.MSG_INSPECTIONPICKINGNOT_SAVED');
                    break;
                  case 3:
                    this.showError('BOOKING_INSPECTIONPICKING.SAVE_RESULT', 'BOOKING_INSPECTIONPICKING.MSG_INSPECTIONPICKINGNOT_FOUND');
                    break;
                }
                this.savedataloading = false;

              }
            },
            error => {
              this.showError("BOOKING_INSPECTIONPICKING.TITLE", "BOOKING_INSPECTIONPICKING.MSG_UNKNONW_ERROR");
              this.savedataloading = false;

            });
      }



    }
  }

  MapLabAddressContacts(inspectionPickingList) {
    inspectionPickingList.forEach(item => {

      if (item.inspectionPickingData.labType == this._labType.Customer) {

        if (item.inspectionPickingData.labContactTypeItems != null) {

          if (item.inspectionPickingData.inspectionPickingContacts != null) {
            //Remove element from contacts if it is removed from the dropdown
            item.inspectionPickingData.inspectionPickingContacts.forEach(element => {

              var labContact = item.inspectionPickingData.labContactTypeItems.find(x => x == element.cusContactId);
              if (!labContact) {
                var index = item.inspectionPickingData.inspectionPickingContacts.indexOf(element);
                item.inspectionPickingData.inspectionPickingContacts.splice(index, 1);
              }

            });

            item.inspectionPickingData.labContactTypeItems.forEach(contact => {

              var pickingContact = item.inspectionPickingData.inspectionPickingContacts.find(x => x.cusContactId == contact);

              if (!pickingContact) {
                var inspectionPickingContact: InspectionPickingContact = {

                  id: 0,
                  pickingTranId: 0,
                  labContactId: null,
                  cusContactId: contact,
                  active: true

                }
                item.inspectionPickingData.inspectionPickingContacts.push(inspectionPickingContact);
              }

            });

          }
          else {
            item.inspectionPickingData.inspectionPickingContacts = [];
            item.inspectionPickingData.labContactTypeItems.forEach(contact => {
              var inspectionPickingContact: InspectionPickingContact = {

                id: 0,
                pickingTranId: 0,
                labContactId: null,
                cusContactId: contact,
                active: true

              }
              item.inspectionPickingData.inspectionPickingContacts.push(inspectionPickingContact);
            });
          }

        }

      }
      else if (item.inspectionPickingData.labType == this._labType.Lab) {

        if (item.inspectionPickingData.labContactTypeItems != null) {

          if (item.inspectionPickingData.inspectionPickingContacts != null) {

            //Remove element from contacts if it is removed from the dropdown
            item.inspectionPickingData.inspectionPickingContacts.forEach(element => {

              var labContact = item.inspectionPickingData.labContactTypeItems.find(x => x == element.labContactId);
              if (!labContact) {
                var index = item.inspectionPickingData.inspectionPickingContacts.indexOf(element);
                item.inspectionPickingData.inspectionPickingContacts.splice(index, 1);
              }

            });

            item.inspectionPickingData.labContactTypeItems.forEach(contact => {

              var pickingContact = item.inspectionPickingData.inspectionPickingContacts.find(x => x.labContactId == contact);

              if (!pickingContact) {
                var inspectionPickingContact: InspectionPickingContact = {

                  id: 0,
                  pickingTranId: 0,
                  labContactId: contact,
                  cusContactId: null,
                  active: true

                }
                item.inspectionPickingData.inspectionPickingContacts.push(inspectionPickingContact);
              }

            });
          }
          else {
            item.inspectionPickingData.inspectionPickingContacts = [];
            item.inspectionPickingData.labContactTypeItems.forEach(contact => {
              var inspectionPickingContact: InspectionPickingContact = {

                id: 0,
                pickingTranId: 0,
                labContactId: contact,
                cusContactId: null,
                active: true

              }
              item.inspectionPickingData.inspectionPickingContacts.push(inspectionPickingContact);
            });
          }

        }

      }

    });
  }

  removeInspectionPicking(index) {
    this.model.splice(index, 1);
    this.getTotalPickingData(true);
  }

  isInspectionDetailsValid() {

    var isproductDetail = true;
    var islabDetail = true;
    for (let item of this.model)
      item.validator.isSubmitted = true;

    isproductDetail = this.model.every((x) =>
      x.validator.isValid('poId') &&
      x.validator.isValid('productId')
      && x.validator.isValid('pickingQuantity'));

    islabDetail = this.model.every((x) =>
      ((x.inspectionPickingData.labId) ? x.validator.isValid('labAddressId') : true)
      && ((x.inspectionPickingData.labId) ? x.validator.isValid('labContactTypeItems') : true));

    if (isproductDetail == true && islabDetail == true) {
      return true;
    }
    else
      return false;

  }

  validateTransactionMapped() {
    var transactionAvailable = true;
    this.model.forEach(element => {
      var poTransaction = this.poTransactionList.find(x => x.productID == element.inspectionPickingData.productId
        && x.poID == element.inspectionPickingData.poId);
      if (!poTransaction) {
        var productName = this.poTransactionList.find(x => x.productID == element.inspectionPickingData.productId).productName;
        var poName = this.poTransactionList.find(x => x.poID == element.inspectionPickingData.poId).poName;
        var warningMessage = this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_SELECTED") + poName
          + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_ANDPRODUCT") + productName
          + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_POPRODUCT_NOTMAPPED");
        this.showWarning("BOOKING_INSPECTIONPICKING.TITLE", warningMessage);
        transactionAvailable = false;
        return transactionAvailable;
      }
    });
    return transactionAvailable;
  }

  addInspectionPickingData() {
    this._isPreview = false;
    var inspectionPickingData: InspectionPicking = {
      id: 0,
      labId: null,
      labName: "",
      labNameImgPath: this.viewImagePath,
      showLabName: false,
      productId: null,
      poId: null,
      customerId: this.customerID,
      labAddressId: null,
      labAddress: "",
      labAddressImgPath: this.viewImagePath,
      showLabAddress: false,
      cusAddressId: null,
      poTranId: null,
      pickingQuantity: null,
      remarks: "",
      active: true,
      labContactTypeItems: null,
      inspectionPickingContacts: null,
      labAddressList: null,
      labContactList: null,
      labType: null,
      labAddressLoading: false,
      labContactLoading: false
    }
    this.model.push({ inspectionPickingData: inspectionPickingData, validator: Validator.getValidator(inspectionPickingData, "booking/edit-booking-picking.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }

  validatePickingQuantity() {

    var isok = true;
    var poTransactionIDs = this.saveInspectionPickingList.map(x => x.poTranId);

    var warningMessage = "";

    var uniquePoTransactionIDs = poTransactionIDs.filter((elem, i, arr) => {
      if (arr.indexOf(elem) === i) {
        return elem
      }
    });

    uniquePoTransactionIDs.forEach(poTransactionID => {
      var actualPOTransaction = this.poTransactionList.find(x => x.poTransactionID == poTransactionID);

      var pickingList = this.saveInspectionPickingList.filter(x => x.poTranId == poTransactionID);
      var inspectionpickingQty = this.getTotalPickingQuantity(pickingList);

      if (Number(actualPOTransaction.pickingQuantity) != inspectionpickingQty) {
        warningMessage = this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_PICKINGQTY_BASEQTY") + actualPOTransaction.poName
          + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_PICKINGQTY_PO") + actualPOTransaction.productName
          + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_PICKINGQTY_IS") + actualPOTransaction.pickingQuantity
          + this.utility.textTranslate("BOOKING_INSPECTIONPICKING.MSG_VALIDATE_PICKINGQTY_PLEASE_MAKESURE") + actualPOTransaction.pickingQuantity;

        isok = false;
        return;
      }

    });

    if (isok != true) {

      this.showError('EDIT_CUSTOMER_CONTACT_SUMMARY.SAVE_RESULT', warningMessage);
    }

    return isok;

  }

  getTotalPickingQuantity(pickingList): number {

    var total = 0;
    if (pickingList != null && pickingList.length > 0) {
      pickingList.forEach(element => {
        total = total + Number(element.pickingQuantity);
      });
    }
    return total;
  }


  getTotalPickingData(isChangeData: boolean) {
    if (isChangeData) {
      this._isPreview = false;
    }
    var total = 0;
    this.model.forEach(element => {
      total = total + Number(element.inspectionPickingData.pickingQuantity);
    });
    this.totalPickingQuantity = total;
  }
  changeLabContact(inspectionPicking) {
    this._isPreview = false;
  }
  changeRemark() {
    this._isPreview = false;
  }
  //export the picking data
  export() {
    this.exportDataLoading = true;
    var customkpiModel = new CustomkpiModel();
    customkpiModel.bookingNo = this.inspectionId;
    this.service.exportSummary(customkpiModel)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Inspection_Picking_Summary");
      },
        error => {
          this.exportDataLoading = false;
        });
  }
  //download the file
  downloadFile(data, mimeType, fileName) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, fileName + ".xlsx");
    }
    else {
      const a = document.createElement('a');
      const url = window.URL.createObjectURL(blob);
      a.download = fileName + ".xlsx";
      a.href = url;
      a.click();
    }
    this.exportDataLoading = false;
  }


}
