import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CombineOrders, SaveCombineOrdersRequest, SamplingQuantityRequest } from 'src/app/_Models/booking/bookingcombineorders.model';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CombineOrdersService } from '../../../_Services/booking/combineorders.service'
import { first, retry } from 'rxjs/operators';
import { SplitBooking } from 'src/app/_Models/booking/splitbookingmodel';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BookingService } from 'src/app/_Services/booking/booking.service';
import { BookingDetail } from 'src/app/_Models/booking/bookingcancelmodel';
import { InspectionPageType, BookingSearchRedirectPage } from '../../common/static-data-common';
import { UserModel } from 'src/app/_Models/user/user.model';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { BusinessLine, InspectionPickingExistRequest } from 'src/app/_Models/booking/inspectionbooking.model';
import { BookingMasterData } from 'src/app/_Models/booking/bookingmaster.model';
@Component({
  selector: 'app-split-booking',
  templateUrl: './split-booking.component.html',
  styleUrls: ['./split-booking.component.scss']
})
export class SplitBookingComponent extends DetailComponent {

  public productList: any;
  public mappedProductList: any;
  public modelRef: NgbModalRef;
  selectedProducts: any;
  public model: SplitBooking;
  public bookingItem: BookingDetail;
  inspectionID: number;
  productIdList: any;
  initialloading: boolean = false;
  savedataloading: boolean = false;
  newBookingLoading: boolean = false;
  productChangeLoading: boolean = false;
  public selectedAllProducts: boolean = false;
  public checkBookingHasData: boolean = false;
  _inspectionPageType = InspectionPageType;
  redirectEditPath: string;
  private currentRoute: Router;
  currentUser: UserModel;
  _bookingredirectpage = BookingSearchRedirectPage;
  @ViewChild('splitnewbookingsuccess') splitnewbookingsuccess: ElementRef;
  _businessLine = BusinessLine;
  currentBookingNo: number;
  public bookingMasterData: BookingMasterData;

  constructor(toastr: ToastrService,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public modalService: NgbModal,
    public utility: UtilityService,
    public bookingservice: BookingService,
    public service: CombineOrdersService, authserve: AuthenticationService,) {
    super(router, route, translate, toastr);
    this.productList = [];
    this.mappedProductList = [];
    this.inspectionID = 0;
    this.productIdList = [];
    this.selectedAllProducts = false;
    this.model = new SplitBooking();
    this.currentUser = authserve.getCurrentUser();
    this.currentRoute = router;
    this.bookingMasterData = new BookingMasterData();
  }

  onInit(id?: any): void {
    this.model = new SplitBooking();
    this.model.splitBookingProductList = [];
    this.redirectEditPath = "inspsplit/split-booking";
    this.currentBookingNo = id;
    this.initialize(id);

  }

  getViewPath(): string {
    return "inspsplit/split-booking";
  }

  getEditPath(): string {
    return this.redirectEditPath;
  }

  Reset() {

  }

  initialize(id?) {
    if (id) {
      this.initialloading = true;
      this.inspectionID = id;
      this.getBookingOrdersById(id);
      this.GetCancelBookingDetails(id);
    }
  }

  GetCancelBookingDetails(id: any) {
    this.bookingItem = new BookingDetail();
    this.bookingservice.GetCancelBookingDetails(id, 1)
      .subscribe(
        response => {
          if (response && response.result == 1) {
            if (response.itemDetails) {
              var x = response.itemDetails;
              this.bookingItem = {
                bookingId: id,
                customerName: x.customerName,
                supplierName: x.supplierName,
                factoryName: x.factoryName,
                serviceType: x.serviceType,
                serviceDateFrom: x.serviceDateFrom,
                serviceDateTo: x.serviceDateTo,
                productCategory: x.productCategory,
                statusId: null
              }
            }
          }
        },
        error => {
          this.setError(error);
          this.initialloading = false;
        }
      )
  }

  AddOnlySelectedProducts() {

    var productList = [];

    var splitBookingProductList = this.model.splitBookingProductList.filter(x => x.selected);

    //looop through the selected product list
    splitBookingProductList.forEach(splitPoProduct => {
      //if it is softline then push the data to the product list(combination(poid,productid,colortransactionid))
      if (this.model.bookingData.businessLine == BusinessLine.SoftLine) {

        var poProduct = this.model.bookingData.inspectionProductList.find(x => x.productId == splitPoProduct.productId
          && x.poTransactionId == splitPoProduct.poTransactionId && x.colorTransactionId == splitPoProduct.colorTransactionId);

        productList.push(poProduct);

      }
      //if it is hardline then push the data to the product list(combination(poid,productid))
      else {
        var poProduct = this.model.bookingData.inspectionProductList.find(x => x.productId == splitPoProduct.productId
          && x.poTransactionId == splitPoProduct.poTransactionId);
        productList.push(poProduct);
      }
    });

    this.model.bookingData.inspectionProductList = productList;

    this.model.splitBookingProductList = this.model.splitBookingProductList.filter(x => x.selected);

  }

  CancelBooking() {
    try {
      this.savedataloading = true;
      this.AddOnlySelectedProducts();
      this.bookingservice.cancelBooking(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              this.savedataloading = false;
              this.showSuccess("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", 'INSPECTION_CANCEL.MSG_PRODUCTS_REMOVE');
              this.modelRef.close();
              this.initialize(this.currentBookingNo);
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_CANNOT_SAVE_AUDIT');
                  break;
                case 3:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_FORMAT_ERROR');
                  break;
                case 4:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NO_ITEM_FOUND');
                  break;
                case 5:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_AUDIT_NOT_UPDATED');
                  break;
              }
              this.savedataloading = false;
              this.modelRef.close();
            }
          },
          error => {
            this.savedataloading = false;
            this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
            this.modelRef.close();

          });

    }
    catch (e) {
      this.savedataloading = false;
      this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
      this.modelRef.close();
    }
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  newBookingSuccesshandler(id: number) {
    this.model.bookingId = id;
    this.savedataloading = false;

    this.redirectEditPath = "inspedit/edit-booking";
    this.newBookingSuccessPopup(this.splitnewbookingsuccess);
  }
  NewBooking() {
    try {
      this.AddOnlySelectedProducts();
      this.savedataloading = true;
      this.newBookingLoading = true;
      this.model.bookingData.splitPreviousBookingNo = this.model.bookingData.id;
      this.model.bookingData.previousBookingNo = this.model.bookingData.previousBookingNo;
      this.model.bookingData.reinspectionType = this.model.bookingData.reinspectionType;
      this.model.bookingData.id = 0; // new booking so id is zero.
      this.model.bookingData.guidId = this.newGuid(); // new booking so set new guid;

      //popup close
      this.modelRef.close();

      //resetting the file attachment list to guid unique error
      this.model.bookingData.inspectionFileAttachmentList = [];
      this.bookingservice.newBooking(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {
              if (res.id > 0)
                this.newBookingSuccesshandler(res.id);
              else {
                this.modelRef.close();
                this.showError("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", 'COMMON.MSG_UNKNONW_ERROR');
              }
            }
            else {
              switch (res.result) {
                case 2:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_CANNOT_SAVE_AUDIT');
                  break;
                case 3:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_FORMAT_ERROR');
                  break;
                case 4:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_NO_ITEM_FOUND');
                  break;
                case 5:
                  this.showError("EDIT_BOOKING.TITLE", 'EDIT_BOOKING.MSG_AUDIT_NOT_UPDATED');
                  break;
              }
              this.savedataloading = false;
            }
            this.newBookingLoading = false;
          },
          error => {
            this.savedataloading = false;
            this.newBookingLoading = false;
            this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");

          });
    }
    catch (e) {
      this.savedataloading = false;
      this.newBookingLoading = false;
      this.showError("EDIT_BOOKING.TITLE", "EDIT_BOOKING.MSG_UNKNOWN_ERROR");
    }
  }

  async openConfirm(id, content) {

    var productList = this.model.splitBookingProductList.filter(x => x.selected);
    if (productList.length > 0) {
      if (this.selectedAllProducts) {
        if (id == "cancelBooking") {
          this.showWarning("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", 'INSPECTION_CANCEL.MSG_CANCEL_PRODUCTS_FEW');
        }
        else if (id == "newBooking") {
          this.showWarning("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", 'INSPECTION_CANCEL.MSG_CREATE_PRODUCTS_FEW');
        }
      }
      else if (productList.filter(x => x.combineGroupId).length > 0)
        this.showError("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", "INSPECTION_CANCEL.MSG_INSPECTION_COMBINE_DATA");
      else {
        var pickingExists = !await this.getInspectionPickingExists(productList);
        if (!pickingExists)
          this.showError("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", "INSPECTION_CANCEL.MSG_INSPECTION_PICKING_DATA");
        else {
          this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
          //{ ariaLabelledBy: 'modal-basic-title' }
          this.modelRef.result.then((result) => {
            this.selectedAllProducts = false;
            this.getBookingOrdersById(this.inspectionID);
          }, (reason) => {
          });
        }
      }
    }
    else {
      this.showWarning("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", 'INSPECTION_CANCEL.MSG_SELECT_ONE_PRODUCT');
    }
  }

  selectAllSplitBookingProducts() {
    for (var i = 0; i < this.model.splitBookingProductList.length; i++) {
      this.model.splitBookingProductList[i].selected = this.selectedAllProducts;
    }
  }
  checkIfAllSplitBookingProductsSelected() {
    this.selectedAllProducts = this.model.splitBookingProductList.every(function (item: any) {
      return item.selected == true;
    })
  }

  getBookingOrdersById(id) {
    //this.bookingservice.GetProductDataByBooking(id)
    this.bookingservice.EditBooking(id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {

            var entity = res.bookingDetails;

            this.model.bookingData = {
              id: entity.id,
              guidId: entity.guidId,
              isEmailRequired: false,
              isFlexibleInspectionDate: false,
              factoryId: entity.factoryId,
              supplierId: entity.supplierId,
              customerId: entity.customerId,
              statusId: entity.statusId,
              statusName: entity.statusName,
              seasonId: entity.seasonId,
              seasonYearId: entity.seasonYearId,
              officeId: entity.officeId,
              entityId: 1,
              serviceTypeId: entity.serviceTypeId,
              serviceDateFrom: entity.serviceDateFrom,
              serviceDateTo: entity.serviceDateTo,
              firstServiceDateFrom: entity.firstServiceDateFrom,
              firstServiceDateTo: entity.firstServiceDateTo,
              cusBookingComments: entity.cusBookingComments,
              apiBookingComments: entity.apiBookingComments,
              internalComments: entity.internalComments,
              qcBookingComments: entity.qcBookingComments,
              applicantName: entity.applicantName,
              applicantEmail: entity.applicantEmail,
              applicantPhoneNo: entity.applicantPhoneNo,
              inspectionProductList: entity.inspectionProductList,
              inspectionCustomerContactList: entity.inspectionCustomerContactList,
              inspectionFactoryContactList: entity.inspectionFactoryContactList,
              inspectionFileAttachmentList: entity.inspectionFileAttachmentList,
              inspectionServiceTypeList: entity.inspectionServiceTypeList,
              inspectionSupplierContactList: entity.inspectionSupplierContactList,
              inspectionCustomerBrandList: entity.inspectionCustomerBrandList,
              inspectionCustomerBuyerList: entity.inspectionCustomerBuyerList,
              inspectionCustomerDepartmentList: entity.inspectionCustomerDepartmentList,
              inspectionCustomerMerchandiserList: entity.inspectionCustomerMerchandiserList,
              isBookingInvoiced: entity.isBookingInvoiced,
              createdBy: entity.createdBy,
              previousBookingNo: entity.previousBookingNo,
              reinspectionType: entity.reinspectionType,
              bookingType: entity.bookingType,
              paymentOptions: entity.paymentOptions,
              isBookingRequestRole: res.isBookingRequestRole,
              isBookingConfirmRole: res.isBookingConfirmRole,
              isBookingVerifyRole: res.isBookingVerifyRole,
              isPickingRequired: false,
              isCombineRequired: false,
              customerBookingNo: entity.customerBookingNo,
              inspectionPageType: null,
              splitPreviousBookingNo: entity.splitPreviousBookingNo,
              isTermsChecked: false,
              holdReasonTypeId: null,
              isCustomerEmailSend: true,
              isSupplierOrFactoryEmailSend: true,
              inspectionDFTransactions: null,
              containerLimit: null,
              collectionId: null,
              priceCategoryId: null,
              customerCollectionName: "",
              customerPriceCategoryName: "",
              compassBookingNo: "",
              previousBookingNoList: null,
              csNames: "",
              isCombineDone: entity.isCombineDone,
              isPickingDone: entity.isPickingDone,
              holdReason: null,
              holdReasonType: null,
              businessLine: entity.businessLine,
              inspectionLocation: entity.inspectionLocation,
              shipmentPort: entity.shipmentPort,
              shipmentDate: entity.shipmentDate,
              ean: entity.ean,
              cuProductCategory: entity.cuProductCategory,
              shipmentTypeIds: entity.shipmentTypeIds,
              csList: entity.csList,
              draftInspectionId: null,
              isUpdateBookingDetail: false,
              isEaqf: entity.isEaqf,
              daCorrelationId: entity.gapdaCorrelation ? this.bookingMasterData.daCorrelationEnum.yes : this.bookingMasterData.daCorrelationEnum.no,
              gapDACorrelation: entity.gapdaCorrelation,
              gapDAEmail: entity.gapdaEmail,
              gapDAName: entity.gapdaName
            };

            this.model.splitBookingProductList = [];

            this.model.bookingData.inspectionProductList.forEach(item => {

              this.model.splitBookingProductList.push(
                {
                  id: item.id,
                  productId: item.productId,
                  productName: item.productName,
                  productDescription: item.productDesc,
                  selected: false,
                  bookingQuantity: item.bookingQuantity,
                  inspectionId: item.inspectionId,
                  poList: null,
                  poTransactionId: item.poTransactionId,
                  poId: item.poId,
                  poName: item.poName,
                  etd: item.etd ? this.utility.formatDate(item.etd) : "",
                  destinationCountryName: item.destinationCountryName,
                  colorTransactionId: item.colorTransactionId,
                  colorCode: item.colorCode,
                  colorName: item.colorName,
                  combineGroupId: item.combineGroupId,
                  pickingQuantity: item.pickingQuantity,
                  unitName: item.unitName
                });
            });
            this.initialloading = false;
          }
          else {
            this.initialloading = false;
          }
        });
  }

  toggleExpandRowProduct(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];

    let triggerTable = event.target.parentNode.parentNode;

    var firstElem = document.querySelector('[data-expand-id="product' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getPoListByBookingAndProduct(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }

  // get po list by booking and product
  getPoListByBookingAndProduct(rowItem) {

    this.bookingservice.GetPODetailsByBookingAndProduct(rowItem.inspectionId, rowItem.id)
      .subscribe(res => {
        if (res.result == 1) {
          rowItem.poList = res.bookingProductPoList;
          rowItem.isPlaceHolderVisible = false;
        }

      },
        error => {
          rowItem.isPlaceHolderVisible = false;

        });

  }

  async getInspectionPickingExists(productList) {
    var isPickingExists = false;
    var request = new InspectionPickingExistRequest();
    request.poTransactionIds = productList.map(x => x.poTransactionId);
    isPickingExists = await this.bookingservice.checkInspectionPickingExists(request);
    return isPickingExists;

  }


  getPickingAndCombineOrdersId(id, content) {
    this.bookingservice.GetPickingAndCombineOrders(id)
      .pipe()
      .subscribe(
        res => {
          this.checkBookingHasData = false;
          if (res && res.result == 1) {

            var productIds = res.pickingResponse.inspectionPickingList.map(x => x.productID);

            if (productIds && productIds.length > 0) {
              if (this.model.splitBookingProductList.filter(x => productIds.includes(x.productId) && x.selected).length > 0) {
                this.showError("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", "INSPECTION_CANCEL.MSG_INSPECTION_PICKING_DATA");
                this.checkBookingHasData = true;
              }
              if (this.checkBookingHasData) {
                return false;
              }
            }

            if (!this.checkBookingHasData) {
              res.combineOrderResponse.combineOrdersList.every(combineOrder => {

                if (this.model.splitBookingProductList.filter(x => x.productId == combineOrder.productId && x.selected && combineOrder.combineProductId != null).length > 0) {
                  this.showError("INSPECTION_CANCEL.TITLE_SPLIT_BOOKING", "INSPECTION_CANCEL.MSG_INSPECTION_COMBINE_DATA");

                  this.checkBookingHasData = true;

                }

                if (this.checkBookingHasData) {
                  return false;
                }

              });
            }


            if (!this.checkBookingHasData) {

              this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true });
              //{ ariaLabelledBy: 'modal-basic-title' }
              this.modelRef.result.then((result) => {
                this.selectedAllProducts = false;
                this.getBookingOrdersById(this.inspectionID);
              }, (reason) => {
              });

            }

          }

        });
  }
  newBookingSuccessPopup(content) {
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
    this.modelRef.result.then((result) => {
      this.modelRef.close();
    }, (reason) => {
    });
  }
  newBookingEditPage() {
    this.currentRoute.navigate([`/${this.utility.getEntityName()}/${this.getEditPath()}/${this.model.bookingId}/${this._bookingredirectpage.SplitBooking}`]);
    this.modelRef.close();
  }

  cancelSuccessSplitBooking() {
    this.modelRef.close();
    this.initialize(this.currentBookingNo);
  }
}
