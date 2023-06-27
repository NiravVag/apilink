import { Component, OnInit, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { CombineOrders, SaveCombineOrdersRequest, SaveCombineOrdersResult, SamplingQuantityRequest, CombineOrderProductFilter, CombineOrderProductFilterRequest, CombineMasterData } from 'src/app/_Models/booking/bookingcombineorders.model';
import { DetailComponent } from '../../common/detail.component';
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CombineOrdersService } from '../../../_Services/booking/combineorders.service'
import { first, retry } from 'rxjs/operators';
import { BookingStatus, CombineBookingTableScrollHeight, AqlType, UserType } from '../../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { CustomerServiceConfig } from 'src/app/_Services/customer/customerserviceconfig.service';
import { Validator, JsonHelper } from '../../common';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/_Services/user/authentication.service';

@Component({
  selector: 'app-edit-combineorders',
  templateUrl: './edit-combineorders.component.html',
  styleUrls: ['./edit-combineorders.component.scss']
})
export class EditCombineOrdersComponent extends DetailComponent {

  @ViewChild('changeAQLLevel') engineModal: TemplateRef<any>;
  @ViewChild('scrollableTable') scrollableTable: ElementRef;

  dialog: NgbModalRef | null;
  public combineOrdersList: Array<CombineOrders>;
  public saveCombineOrderList: Array<SaveCombineOrdersRequest>;
  public combineOrderFilter: CombineOrderProductFilter;
  public combineOrderFilterRequest: CombineOrderProductFilterRequest;
  public productList: any;
  public mappedProductList: any;
  selectedProducts: any;
  inspectionID: number;
  productIdList: any;
  initialloading: boolean = false;
  savedataloading: boolean = false;
  productChangeLoading: boolean = false;
  levelPickLoading: boolean = false;
  isCombineOrderSaveVisible: boolean = false;
  exportDataLoading: boolean = false;
  totalCombineAqlQuantity = 0;
  totalNonCombineAqlQuantity = 0;
  totalCombineProducts = 0;
  totalNonCombineProducts = 0;
  totalNumberofReports = 0;
  totalNumberofProducts = 0;
  totalCombineCount = 0;
  groupData: any;
  topAqlList: any;
  aqlType = AqlType;
  toggleFormSection: boolean;
  public jsonHelper: JsonHelper;
  public baseAqlId: number;
  public modelRef: NgbModalRef;
  public combineAQLSelectedMessage: string;
  //for new popup variables
  public userSelectedAqlId: number;
  public userAQLList: any;
  public currentCombineGroupId: number;
  public combineOrderAQLData: any;
  //for new popup variables

  combineMasterData: CombineMasterData;
  constructor(toastr: ToastrService,
    route: ActivatedRoute,
    router: Router,
    translate: TranslateService,
    public validator: Validator,
    public serviceConfig: CustomerServiceConfig,
    public modalService: NgbModal,
    public service: CombineOrdersService, public utility: UtilityService, authservice: AuthenticationService) {
    super(router, route, translate, toastr);
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("booking/edit-combineorder-filter.valid.json");
    this.validator.setModelAsync(() => this.combineOrderFilterRequest);
    this.combineOrdersList = [];
    this.productList = [];
    this.saveCombineOrderList = [];
    this.mappedProductList = [];
    this.inspectionID = 0;
    this.productIdList = [];
    this.combineOrderFilter = new CombineOrderProductFilter();
    this.combineOrderFilterRequest = new CombineOrderProductFilterRequest();
    this.combineMasterData = new CombineMasterData();
    this.combineMasterData.currentUser = authservice.getCurrentUser();
  }

  getGroupData() {
    this.groupData = [];
    for (let i = 1; i <= 50; i++) {
      this.groupData.push({ "id": i, "name": "Combine-" + i });
    }
  }

  onInit(id?: any): void {
    this.initialize(id);
  }

  getViewPath(): string {
    return "inspcombine/edit-combineorders";
  }

  getEditPath(): string {
    return "inspcombine/edit-combineorders";
  }


  getUniqueCombineOrder() {
    var combineOrderList = this.combineOrdersList.filter(x => x.combineProductId != null);
    let groupIds = combineOrderList.map(x => x.combineProductId);
    let distinctGroupIds = groupIds.filter((n, i) => groupIds.indexOf(n) === i);
    return distinctGroupIds;
  }

  // calculate total combine count
  calculateTotalCombineCount() {

    var groupIds = this.combineOrdersList.filter(x => x.combineProductId != null).map(x => x.combineProductId);

    let distinctGroupIds = groupIds.filter((n, i) => groupIds.indexOf(n) === i);

    var totalCombineCount = 0;

    distinctGroupIds.forEach(id => {

      if (groupIds.filter(x => x == id).length > 1) {
        totalCombineCount = totalCombineCount + 1;
      }

    });

    this.totalCombineCount = totalCombineCount;

  }

  getUniqueColorList() {
    var resultColorSets = ['FFC1C1', '87CEEB', 'D3D3D3', 'FAF0E6', 'FFE4C4', 'FFDEAD', 'FFFACD', 'F0FFF0',
      'E6E6FA', 'FFE4E1', 'ADD8E6', 'AFEEEE', '48D1CC', 'E0FFFF', '66CDAA', '7FFFD4',
      '8FBC8F', '98FB98', 'BDB76B', 'F0E68C', 'FAFAD2', 'EEDD82', 'BC8F8F', 'D2B48C', 'FFB6C1',
      'DDA0DD', '8EE5EE', '9AFF9A', 'EEEE00', 'FFC1C1', '9381FF', 'FFD8BE', 'FFEEDD', '9986DF', 'CCAFCF',
      'F2CEC7', 'F686BD', 'F4BBD3', 'D6D2D2', 'FE5D9F', 'F5A1C8', 'F3D0E3', 'EDF5FC', 'B8C5D6', 'A39BA8',
      '5ADA8F', 'D3DDE9', '00B295', 'C9DAEA', 'C05A74'];

    return resultColorSets;
  }

  applyGroupColor() {
    var count = 0;
    //var combinedCount = 0;

    var uniqueCombineProductList = this.getUniqueCombineOrder();
    var uniqueColorList = this.getUniqueColorList();

    uniqueCombineProductList.forEach(item => {
      var combineOrderList = this.combineOrdersList.filter(x => x.combineProductId == item);
      if (combineOrderList.length > 1) {
        combineOrderList.forEach(row => {

          if (row.combineProductId == item) {
            row.colorCode = "#" + uniqueColorList[count];
          }

        });
        count = count + 1;
      }
      else if (combineOrderList.length == 1) {
        combineOrderList[0].colorCode = "#FFFFFF";
        combineOrderList[0].combinedAqlQuantity = 0;
        combineOrderList[0].combineCount = 0;
      }
    });

    // this.combineOrdersList.sort((a, b) =>
    //   (a.combineProductId != null ? a.combineProductId : Infinity) -
    //   (b.combineProductId != null ? b.combineProductId : Infinity));

  }
  //validate the display product when changing the combine group
  validateChangeDisplayProduct(item) {
    var isValid = true;
    var filterProductIds = [];
    var productIds = this.combineOrdersList.filter(x => x.combineProductId == item.combineProductId).map(x => x.productId);
    if (productIds)
      filterProductIds = filterProductIds.concat(productIds);

    isValid = this.validateDisplayProducts(filterProductIds);
    if (!isValid) {
      setTimeout(() => {
        item.combineProductId = null;
      }, 1000);
    }
    return isValid;
  }

  //validate the display products with the filtered product ids which belongs to single combine group
  validateDisplayProducts(filterProductIds) {
    var isValid = true;
    //take the master product list from the single combine group
    var masterProducts = this.combineOrdersList.filter(x => filterProductIds.includes(x.productId) && x.isDisplayMaster);

    //if master product length is greater than one then multiple master products are combined together
    if (masterProducts && masterProducts.length > 1) {
      var productNames = masterProducts.map(x => x.productName).join(',').trim();
      //var productNames = Object.values(productNameList).join(',');
      this.showWarning('BOOKING_COMBINEORDERS.TITLE', productNames + this.utility.textTranslate('BOOKING_COMBINEORDERS.MSG_DIFF_MASTER_PRODUCTS_SINGLE_GRP'));
      isValid = false;
    }
    //if the master product length is one and ensure only their child products will get mapped in the combine group
    else if (masterProducts && masterProducts.length == 1) {
      //take the product list matches the parent productid
      var childProducts = this.combineOrdersList.filter(x => filterProductIds.includes(x.productId)
        && !x.isDisplayMaster && x.parentProductId != masterProducts[0].productId);
      //if it has data then multipe parent involved in the single combine group
      if (childProducts && childProducts.length > 0) {
        this.showWarning('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_MULTIPLE_PRODUCTS_CANNOT_COMBINE');
        isValid = false;
      }
    }
    else {
      //check the child product from multiple master should not be combined
      var parentProductIds = this.combineOrdersList.filter(x => filterProductIds.includes(x.productId) && !x.isDisplayMaster)
        .map(x => x.parentProductId);
      //take the distinct parent product ids
      let distinctparentProductIds = parentProductIds.filter((n, i) => parentProductIds.indexOf(n) === i);
      //if parent product ids greater than one then products from multiple master involved
      if (distinctparentProductIds.length > 1) {
        this.showWarning('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_MULTIPLE_PRODUCTS_CANNOT_COMBINE');
        isValid = false;
      }


      /*   if (childProducts && childProducts.length > 1) {
          //take the parent product ids from the child product list
          var parentProductIds = childProducts.map(x => x.parentProductId);
  
          if (parentProductIds) {
            //take the distinct parent product ids
            let distinctparentProductIds = parentProductIds.filter((n, i) => parentProductIds.indexOf(n) === i)
            //if parent product ids greater than one then products from multiple master involved
            if (distinctparentProductIds.length > 1) {
              this.showWarning('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_MULTIPLE_PRODUCTS_CANNOT_COMBINE');
              isValid = false;
            }
          }
        } */
    }
    return isValid;
  }

  //validate the filter when apply data from the top panel
  isCombineFilterValid() {
    var isValid = true;
    isValid = this.validator.isValid('filterProductIds') &&
      this.validator.isValid('filterCombineProductId');

    if (isValid) {
      //push the filter productids into filter productids
      var filterProductIds = this.combineOrderFilterRequest.filterProductIds;
      //take the existing combine orders using the filtered combine product id
      var existingOrders = this.combineOrdersList.filter(x => x.combineProductId == this.combineOrderFilterRequest.filterCombineProductId);
      //if data is there push the product ids into combine productid
      if (existingOrders && existingOrders.length > 0) {
        var existingProductIds = existingOrders.map(x => x.productId);
        filterProductIds = filterProductIds.concat(existingProductIds);
      }
      //validate the display products
      isValid = this.validateDisplayProducts(filterProductIds);

    }
    return isValid;
  }

  combineProduct(item, event) {
    if (item && event) {
      if (this.validateChangeDisplayProduct(item)) {
        var aqlLevelIds = [];
        var combineOrderAQLData = [];
        //take the updated order from the list using product and po data
        var updatedOrder = this.combineOrdersList.filter(x => x.productId == item.productId)[0];
        if (updatedOrder) {
          this.currentCombineGroupId = updatedOrder.combineProductId;
          if (updatedOrder.combineProductId) {

            var updatedOrders = this.combineOrdersList.filter(x => x.combineProductId == updatedOrder.combineProductId);
            if (updatedOrders.length > 1) {

              //take the list of updated group data and pushed into combineOrderAQLData
              updatedOrders.forEach(element => {
                //dont push aql level it is display master
                if (!element.isDisplayMaster)
                  aqlLevelIds.push(element.aqlLevel);
                combineOrderAQLData.push({
                  "productId": element.productId, "combineProductId": element.combineProductId,
                  "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity,
                });
              });
            }
            updatedOrder.combineCount = 0;
          }

          if (this.mappedProductList.length > 0) {
            //check the product and po combination already mapped with any other group
            var mappedOrder = this.mappedProductList.filter(x => x.productId == item.productId)[0];
            if (mappedOrder) {
              if (mappedOrder.combineProductId) {
                //take the already mapped group data and push into combineOrderAQLData
                var existingOrders = this.combineOrdersList.filter(x => x.combineProductId == mappedOrder.combineProductId);
                existingOrders.forEach(element => {
                  //dont push aql level it is display master
                  if (!element.isDisplayMaster)
                    aqlLevelIds.push(updatedOrder.aqlLevel);
                  combineOrderAQLData.push({
                    "productId": element.productId, "combineProductId": element.combineProductId,
                    "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity,
                  });
                });
              }
            }
          }
        }
        if (aqlLevelIds && aqlLevelIds.length > 0) {
          var distinctAqlLevelIds = aqlLevelIds.filter((n, i) => aqlLevelIds.indexOf(n) === i);
          if (distinctAqlLevelIds.length == 1 && combineOrderAQLData.length > 1) {
            //if aql level is null assign zero
            if (!distinctAqlLevelIds[0])
              distinctAqlLevelIds[0] = 0;
            this.getSamplingQuantity(combineOrderAQLData, distinctAqlLevelIds[0]);
            this.mapOrdersToProductList();
            //apply group color logic
            this.applyGroupColor();
            this.mapNonCombinedProductList();
          }
          else if (distinctAqlLevelIds.length > 1) {
            if (distinctAqlLevelIds.includes(this.aqlType.AQLCustom)) {
              setTimeout(function () {
                item.combineProductId = null;

              }, 1000);

              this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_CUSTOM_AQL_NOT_APPLICABLE');
            }
            else {
              this.userAQLList = this.combineOrderFilter.aqlList.filter(x => distinctAqlLevelIds.includes(x.id));
              this.combineOrderAQLData = combineOrderAQLData;
              this.userSelectedAqlId = null;
              this.dialog = this.modalService.open(this.engineModal, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
            }
          }

        }
      }
    }
  }

  clearCombineProducts(item) {
    var combineOrderAQLData = [];
    //check cleared grouped data is available in the mapped product list
    if (this.mappedProductList.length > 0) {
      //take the mapped data with the cleared data productid and poid
      var mappedOrder = this.mappedProductList.filter(x => x.productId == item.productId)[0];
      if (mappedOrder) {
        if (mappedOrder.combineProductId) {
          //take the products belongs to the group where the cleared product belongs
          //since we need to calculate the combine aql quantity and update the 
          var existingOrders = this.combineOrdersList.filter(x => x.combineProductId == mappedOrder.combineProductId);
          existingOrders.forEach(element => {
            combineOrderAQLData.push({
              "productId": element.productId, "combineProductId": element.combineProductId,
              "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity
            });
          });
        }

      }

    }

    //remove the product from the mapped product list
    var index = this.mappedProductList.findIndex(x => x.productId == item.productId);
    if (index != null)
      this.mappedProductList.splice(index, 1);

    item.combineCount = 0;
    item.colorCode = "#FFFFFF";
    item.combinedAqlQuantity = 0;

    this.mapNonCombinedProductList();

    //calculate the sampling quantity for the existing group
    if (combineOrderAQLData) {
      if (combineOrderAQLData.length > 1) {
        this.getSamplingQuantity(combineOrderAQLData, 0);
        this.applyGroupColor();
      }

    }
    this.calculateTotalCombineCount();
  }

  initialize(id?) {
    if (id) {
      this.validator.isSubmitted = false;
      this.initialloading = true;
      this.inspectionID = id;
      this.getBookingOrdersById(id);
      this.getserviceLevelPickFirst();
    }
  }

  getBookingOrdersById(id) {
    this.service.getBookingOrders(id)
      .pipe()
      .subscribe(
        res => {
          this.productList = [];
          if (res && res.result == 1) {
            this.showRoleWarningMessage(res);
            this.processSuccessCombineOrders(res);
            this.totalNumberofReports = res.totalNumberofReports;
          }
          else {
            this.initialloading = false;
            this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_UNKNOWN_ERROR');
          }
        },
        error => {
          this.initialloading = false;
          this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_UNKNOWN_ERROR');
        }
      );
  }

  getBookingPoDetails(rowItem) {
    this.service.getBookingPoDetails(rowItem.inspectionId, rowItem.id)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            rowItem.poList = res.bookingProductPoList;
          }
        },
        error => {
          this.initialloading = false;
          this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_UNKNOWN_ERROR');
        }
      );
  }

  export() {
    this.exportDataLoading = true;
    this.service.exportSummary(this.inspectionID)
      .subscribe(res => {
        this.downloadFile(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      },
        error => {
          this.exportDataLoading = false;
        });
  }
  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    }
    this.exportDataLoading = false;
  }

  processSuccessCombineOrders(res) {
    this.combineOrdersList = res.combineOrdersList;
    this.totalNumberofProducts = this.combineOrdersList.map(x => x.productId).length;
    this.baseAqlId = this.combineOrdersList[0].aqlLevel;
    this.mapOrdersToProductList();
    this.mapNonCombinedProductList();
    this.initialloading = false;
    this.calculateTotalNonCombineAqlQty();
    this.getGroupData();
    this.applyGroupColor();
    this.applyCombineCount();
    // setTimeout(() => {
    //   this.limitTableHeight();
    // }, 1000);
  }

  setTotalNumberOfReports() {
    var totalreports = 0;
    var uniqueCombineProductList = this.getUniqueCombineOrder();
    if (uniqueCombineProductList)
      totalreports = totalreports + uniqueCombineProductList.length;

    var nonCombinedProducts = this.combineOrdersList.filter(x => x.combineProductId == null);
    if (nonCombinedProducts)
      totalreports = totalreports + nonCombinedProducts.length;

    this.totalNumberofReports = totalreports;
  }


  calculateTotalNonCombineAqlQty() {
    this.totalNonCombineAqlQuantity = 0;
    this.totalCombineAqlQuantity = 0;
    this.totalNonCombineProducts = 0;
    this.totalCombineProducts = 0;
    var uniqueCombineProductList = this.getUniqueCombineOrder();

    this.combineOrdersList.forEach(item => {
      // check non combine products
      if (item.combineProductId == null) {
        this.totalNonCombineAqlQuantity += item.samplingQuantity;
        this.totalNonCombineProducts = this.totalNonCombineProducts + 1;
      }
      else {
        var combinedProductGroup = this.combineOrdersList.filter(x => x.combineProductId == item.combineProductId);
        if (combinedProductGroup) {

          if (combinedProductGroup.length > 1) {
            this.totalCombineProducts = this.totalCombineProducts + 1;
            this.totalCombineAqlQuantity += item.combinedAqlQuantity;
          }
          else {
            this.totalNonCombineProducts = this.totalNonCombineProducts + 1;
            this.totalNonCombineAqlQuantity += item.samplingQuantity;
          }
        }
      }
    });


  }

  showRoleWarningMessage(role) {
    //if it is internal user and dont  have any roles then throw the message
    if ((this.combineMasterData.currentUser.usertype == UserType.InternalUser
      && !role.isBookingRequestRole && !role.isBookingVerifyRole && !role.isBookingConfirmRole)) {
      this.showWarning('BOOKING_COMBINEORDERS.TITLE', 'EDIT_BOOKING.MSG_COMBINE_ROLE_NOT_AVAILABLE', true);
    }
    //if it is internal user and booking status not falls under given status then throw the message
    else if ((this.combineMasterData.currentUser.usertype == UserType.InternalUser && this.combineMasterData.internalUserCombineSaveStatus.includes(role.bookingStatus)))
      this.isCombineOrderSaveVisible = true;
      //if it is external user(customer,supplier,factory) and booking status not falls under given status then throw the message
    else if ((this.combineMasterData.currentUser.usertype == UserType.Customer
            || this.combineMasterData.currentUser.usertype == UserType.Supplier
            || this.combineMasterData.currentUser.usertype == UserType.Factory)
      && this.combineMasterData.externalUserCombineSaveStatus.includes(role.bookingStatus)) {
      this.isCombineOrderSaveVisible = true;
    }
  }

  save() {
    this.savedataloading = true;

    if (this.validateCombineOrderList()) {
      this.mapCombineOrderRequest();

      this.service.saveCombineOrder(this.saveCombineOrderList, this.inspectionID)
        .subscribe(
          res => {
            if (res) {
              switch (res.result) {
                case SaveCombineOrdersResult.Success:
                  {
                    //  this.waitingService.close();
                    this.getBookingOrdersById(this.inspectionID);
                    this.showSuccess('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.MSG_SAVE_SUCCESS');
                  }
                  break;
                case SaveCombineOrdersResult.CombineAqlQuantityGreaterThanZero:
                  {
                    //  this.waitingService.close();
                    this.showWarning('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.MSG_COMBINE_AQL_QTY_NOT_NULL');
                  }
                  break;
                  case SaveCombineOrdersResult.InternalUserRoleNotMatched:
                  {
                    //  this.waitingService.close();
                    this.showWarning('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.LBL_USER_ROLE_NOT_MATCHED');
                  }
                  break;
                  case SaveCombineOrdersResult.InternalUserStatusNotMatched:
                  {
                    //  this.waitingService.close();
                    this.showWarning('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.LBL_USER_STATUS_NOT_MATCHED');
                  }
                  break;
                  case SaveCombineOrdersResult.ExternalUserStatusNotMatched:
                  {
                    //  this.waitingService.close();
                    this.showWarning('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.LBL_USER_STATUS_NOT_MATCHED');
                  }
                  break;
              }

              this.savedataloading = false;
            }
          },
          error => {
            this.savedataloading = false;
            this.showError('BOOKING_COMBINEORDERS.LBL_SAVE_RESULT', 'BOOKING_COMBINEORDERS.MSG_UNKNONW_ERROR');

          });
    }

  }

  mapCombineOrderRequest() {
    this.saveCombineOrderList = [];
    this.combineOrdersList.forEach(item => {

      if (this.combineOrdersList.filter(x => x.combineProductId == item.combineProductId).length < 2)
        item.combineProductId = null;
      var request = new SaveCombineOrdersRequest();
      request.id = item.id;
      request.aqlId = item.aqlLevel;
      request.inspectionId = item.inspectionId;
      request.combineProductId = item.combineProductId;
      request.productId = item.productId;
      request.samplingQuantity = item.samplingQuantity == null ? 0 : item.samplingQuantity;
      request.aqlQuantity = item.samplingQuantity == null ? 0 : item.samplingQuantity;
      request.combinedAqlQuantity = item.combinedAqlQuantity;

      this.saveCombineOrderList.push(request);

    });

  }

  //validate all the child products mapped for the parent products in the combine group
  validateAllChildProductsSelected(productIds) {
    var isValid = true;

    //take the master product from the filtered productids

    var masterProduct = this.combineOrdersList.find(x => productIds.includes(x.productId) && x.isDisplayMaster);

    if (masterProduct) {

      //take all the child products mapped with the master product data

      var productList = this.combineOrdersList.filter(x => x.parentProductId == masterProduct.productId && !x.isDisplayMaster);

      //take all the product list mapped with the combine product id

      var combineProductList = this.combineOrdersList.filter(x => x.combineProductId == masterProduct.combineProductId && !x.isDisplayMaster);

      //if both length are not equal then popup msg all the child products need to be added

      if (productList && combineProductList && productList.length != combineProductList.length) {
        isValid = false;
        this.showWarning('BOOKING_COMBINEORDERS.TITLE', this.utility.textTranslate('BOOKING_COMBINEORDERS.MSG_ALL_CHILD_PRODUCTS')
          + masterProduct.productName + this.utility.textTranslate('BOOKING_COMBINEORDERS.MSG_COMBINE_GROUP'));
      }
    }
    return isValid;
  }

  //chek all the master products added for each combine group
  checkMasterProductAdded(combineProductId) {
    var isValid = true;
    var displaychildProducts = this.combineOrdersList.filter(x => x.combineProductId == combineProductId && x.parentProductId != null);

    //if child data is there then check master product is mapped or not
    if (displaychildProducts && displaychildProducts.length > 0) {
      //take the master product data with combine product id null
      var productList = this.combineOrdersList.filter(x => x.isDisplayMaster && x.combineProductId == combineProductId
        && x.combineProductId != null);
      //then list greater than zero then show the popup
      if (productList && productList.length == 0) {
        isValid = false;
        this.showWarning('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_INCLUDE_ALL_MASTER_PRODUCTS');
      }
    }
    return isValid;
  }

  //valdiate each combine order group in save function
  validateCombineOrderList() {
    var isDataValid = true;
    var uniqueCombineOrderIds = this.getUniqueCombineOrder();
    //
    for (var index = 0; index < uniqueCombineOrderIds.length; index++) {
      //take the products involved in the combine group
      var productIds = this.combineOrdersList.filter(x => x.combineProductId == uniqueCombineOrderIds[index]).map(x => x.productId);
      //validate the combine order productids
      isDataValid = this.validateDisplayProducts(productIds) && this.validateAllChildProductsSelected(productIds)
        && this.checkMasterProductAdded(uniqueCombineOrderIds[index]);
      if (!isDataValid)
        break;
    }

    this.savedataloading = false;
    return isDataValid;
  }

  getSamplingQuantity(mappedProductList, aqlId) {
    this.service.getSamplingQuantity(this.inspectionID, mappedProductList, aqlId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            if (res.samplingDataList) {
              res.samplingDataList.forEach(element => {
                var combineOrder = this.combineOrdersList.filter(x => x.combineProductId == element.combineProductId
                  && x.productId == element.productId)[0];
                if (combineOrder) {
                  combineOrder.samplingQuantity = element.aqlQuantity;
                  combineOrder.combinedAqlQuantity = element.samplingQuantity;
                  combineOrder.combineCount = 0;
                }

              });

              this.applyCombineCount();
            }

            this.productChangeLoading = false;

            this.calculateTotalNonCombineAqlQty();
            this.setTotalNumberOfReports();
          }
          else if (res.result == 3) {
            this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_REPORT_VALIDATION', true);
            this.getBookingOrdersById(this.inspectionID);
          }

        },
        error => {
          this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_UNKNOWN_ERROR');
          console.log(error);
          this.productChangeLoading = false;
        }
      );

  }

  toggleCombineProductActivation() {
    var combinedProducts = this.combineOrdersList.filter(x => x.combineProductId != null);
    if (combinedProducts) {
      let combineProductIds = combinedProducts.map(x => x.combineProductId);
      let Products = this.combineOrdersList.filter(x => combineProductIds.includes(x.productId));
      Products.forEach(item => {
        item.isProductListEnable = true;
      });
      combinedProducts.forEach(item => {
        item.isProductListEnable = false;
      });
    }
  }

  getSamplingQuantityByProductList(bookingID, productList) {
    this.service.getSamplingQuantityByProductList(bookingID, productList)
      .subscribe(
        res => {
          if (res.result == 1) {
            var samplingDataList = res.samplingDataList;
            samplingDataList.forEach(item => {
              var productIndex = this.combineOrdersList.findIndex(x => x.productId == item.productId);
              this.combineOrdersList[productIndex].samplingQuantity = item.samplingQuantity;
              this.combineOrdersList[productIndex].combinedAqlQuantity = 0;
            });
            this.productChangeLoading = false;
          }
          this.productChangeLoading = false;

        },
        error => {
          this.productChangeLoading = false;
          this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_UNKNOWN_ERROR');
          console.log(error);
        }
      );
  }
  toggleSection() {
    this.toggleFormSection = !this.toggleFormSection;
  }
  mapOrdersToProductList() {
    this.mappedProductList = [];
    var combinedProductList = this.combineOrdersList.filter(x => x.combineProductId != null);
    combinedProductList.forEach(item => {
      this.mappedProductList.push({
        "combineProductId": item.combineProductId,
        "orderQuantity": item.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": item.samplingQuantity,
        "productId": item.productId, "aqlLevel": item.aqlLevel
      });
    });
  }

  applyCombineCount() {
    var uniqueCombineOrderIds = this.getUniqueCombineOrder();
    this.calculateTotalCombineCount();
    uniqueCombineOrderIds.forEach(combineProduct => {
      var groupedCombineOrderData = this.combineOrdersList.filter(x => x.combineProductId == combineProduct && x.combineProductId != null);
      if (groupedCombineOrderData) {
        var combineCount = groupedCombineOrderData.length;
        var combinedProductData = groupedCombineOrderData.filter(x => x.combineProductId == combineProduct && x.combinedAqlQuantity != 0)[0];
        if (combineCount > 1) {
          if (combinedProductData) {
            combinedProductData.combineCount = combineCount;
          }
        }
        else if (combineCount == 1 && combinedProductData) {
          if (combinedProductData) {
            combinedProductData.combineCount = 0;
            combinedProductData.combinedAqlQuantity = 0;
          }

        }
      }
    });
  }

  toggleExpandRow(event, index, rowItem) {
    rowItem.isPlaceHolderVisible = true;
    rowItem.poList = [];
    let triggerTable = event.target.parentNode.parentNode;
    var firstElem = document.querySelector('[data-expand-id="' + index + '"]');
    firstElem.classList.toggle('open');

    triggerTable.classList.toggle('active');

    if (firstElem.classList.contains('open')) {
      event.target.innerHTML = '-';
      this.getBookingPoDetails(rowItem);
    }
    else {
      event.target.innerHTML = '+';
      rowItem.isPlaceHolderVisible = false;
    }
  }
  //get the service pick1 data
  getserviceLevelPickFirst() {
    this.levelPickLoading = true;
    this.serviceConfig.getServiceLevelPickFirst()
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.combineOrderFilter.aqlList = res.levelPickList;
            this.topAqlList = this.combineOrderFilter.aqlList.map((x) => x);
            // remove last item
            this.topAqlList.pop();
            this.levelPickLoading = false;
          }
          else {
            this.error = res.result;
          }
          this.loading = false;
        },
        error => {
          this.levelPickLoading = false;
          this.setError(error);
          this.loading = false;
        });
  }
  //map the non combined products
  mapNonCombinedProductList() {
    var productList = [];
    this.combineOrderFilter.productList = [];
    var nonCombinedProductList = this.combineOrdersList.filter(x => x.combineProductId == null && x.aqlLevel != this.aqlType.AQLCustom);
    nonCombinedProductList.forEach(element => {
      productList.push({ "id": element.productId, "name": element.productName })
    });
    //var productList = nonCombinedProductList.map(x => x.productId);
    if (productList && productList.length > 0)
      this.combineOrderFilter.productList = productList;
  }


  //calculate the product sample size based on the filter data
  calculateProductsSampleSize() {
    this.validator.isSubmitted = true;
    this.currentCombineGroupId = this.combineOrderFilterRequest.filterCombineProductId;
    var aqlLevelIds = [];
    if (this.isCombineFilterValid()) {
      var combineOrderAQLData = [];
      if (this.combineOrderFilterRequest.filterCombineProductId) {
        //check if any products mapped for the combine product id
        var updatedOrders = this.combineOrdersList.filter(x => x.combineProductId == this.combineOrderFilterRequest.filterCombineProductId);
        if (updatedOrders.length > 0) {
          aqlLevelIds.push(updatedOrders[0].aqlLevel);
          //take the list of updated group data and pushed into combineOrderAQLData
          updatedOrders.forEach(element => {
            combineOrderAQLData.push({
              "productId": element.productId, "combineProductId": element.combineProductId,
              "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity
            });
          });
        }
        //updatedOrder.combineCount = 0;

        //push the selected product list into  combine order data
        if (this.combineOrderFilterRequest.filterProductIds) {
          var newproductList = this.combineOrdersList.filter(x => this.combineOrderFilterRequest.filterProductIds.includes(x.productId));
          if (newproductList.length > 0) {
            //take the list of updated group data and pushed into combineOrderAQLData
            newproductList.forEach(element => {
              element.combineProductId = this.combineOrderFilterRequest.filterCombineProductId;
              if (!element.isDisplayMaster) {
                aqlLevelIds.push(element.aqlLevel);
              }
              combineOrderAQLData.push({
                "productId": element.productId, "combineProductId": element.combineProductId,
                "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity
              });
            });
          }

          //if combine order is greater than zero
          if (combineOrderAQLData.length > 1) {
            if (this.combineOrderFilterRequest.filterAqlId) {
              var distinctAqlLevelIds = aqlLevelIds.filter((n, i) => aqlLevelIds.indexOf(n) === i);
              if (distinctAqlLevelIds.includes(this.aqlType.AQLCustom)) {
                // set combine as null since it is include with custom  
                newproductList.forEach(element => {
                  element.combineProductId = null;
                });
                this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_CUSTOM_AQL_NOT_APPLICABLE');
              }
              else {
                this.getSamplingQuantity(combineOrderAQLData, this.combineOrderFilterRequest.filterAqlId);
                this.processSuccessSampleSizeCalc();
                this.applySuccessSampleSizeAQLLevel(this.currentCombineGroupId, this.combineOrderFilterRequest.filterAqlId);
              }
            }
            else {
              var distinctAqlLevelIds = aqlLevelIds.filter((n, i) => aqlLevelIds.indexOf(n) === i);
              if (distinctAqlLevelIds.length == 1) {

                if (distinctAqlLevelIds.includes(this.aqlType.AQLCustom)) {
                  // set combine as null since it is include with custom  
                  newproductList.forEach(element => {
                    element.combineProductId = null;
                  });
                  this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_CUSTOM_AQL_NOT_APPLICABLE');
                }
                else {
                  this.getSamplingQuantity(combineOrderAQLData, distinctAqlLevelIds[0]);
                  this.processSuccessSampleSizeCalc();
                  this.applySuccessSampleSizeAQLLevel(this.currentCombineGroupId, distinctAqlLevelIds[0]);
                }
              }
              else if (distinctAqlLevelIds.length > 1) {
                if (distinctAqlLevelIds.includes(this.aqlType.AQLCustom)) {
                  // set combine as null since it is include with custom  
                  newproductList.forEach(element => {
                    element.combineProductId = null;
                  });
                  this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_CUSTOM_AQL_NOT_APPLICABLE');
                }
                else {
                  this.userAQLList = this.combineOrderFilter.aqlList.filter(x => distinctAqlLevelIds.includes(x.id));
                  this.combineOrderAQLData = combineOrderAQLData;
                  this.userSelectedAqlId = null;
                  this.dialog = this.modalService.open(this.engineModal, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
                }
              }
            }
          }
          else if (combineOrderAQLData.length == 1) {
            newproductList[0].combineProductId = null;
          }

        }

        //updates the mapped product list with already existing order

        this.combineOrderFilterRequest = new CombineOrderProductFilterRequest();
        this.validator.isSubmitted = false;
      }
    }
  }

  //apply selected aql level in the combined group
  applySuccessSampleSizeAQLLevel(currentCombineGroupId, aqlLevel) {
    var combineGroupOrders = this.combineOrdersList.filter(x => x.combineProductId == currentCombineGroupId && !x.isDisplayMaster);
    if (combineGroupOrders && combineGroupOrders.length > 1) {
      combineGroupOrders.forEach(element => {
        element.aqlLevel = aqlLevel;
      });
    }
  }

  //process the data after filtered data calculation
  processSuccessSampleSizeCalc() {
    this.mapOrdersToProductList();
    this.applyGroupColor();
    this.mapNonCombinedProductList();
  }

  //change the aql data event
  changeAQLData(item, event) {
    if (item && event && item.combineProductId != null) {
      if (event.id == this.aqlType.AQLCustom) {

        setTimeout(() => {
          item.combineProductId = null;
          item.aqlLevel = null;
          this.clearCombineProducts(item);
        }, 200);

        this.showError('BOOKING_COMBINEORDERS.TITLE', 'BOOKING_COMBINEORDERS.MSG_CUSTOM_AQL_NOT_APPLICABLE');
      }
      else {
        var combineOrderAQLData = [];
        //take the updated order from the list using product and po data
        var updatedOrder = this.combineOrdersList.filter(x => x.productId == item.productId)[0];
        var aqlLevelIds = [];
        if (updatedOrder) {
          this.currentCombineGroupId = updatedOrder.combineProductId;
          if (updatedOrder.combineProductId) {
            var updatedOrders = this.combineOrdersList.filter(x => x.combineProductId == updatedOrder.combineProductId);
            if (updatedOrders.length > 1) {
              //take the list of updated group data and pushed into combineOrderAQLData
              updatedOrders.forEach(element => {
                aqlLevelIds.push(element.aqlLevel);
                combineOrderAQLData.push({
                  "productId": element.productId, "combineProductId": element.combineProductId,
                  "orderQuantity": element.totalBookingQuantity, "samplingQuantity": 0, "aqlQuantity": element.samplingQuantity,
                });
              });
            }
            updatedOrder.combineCount = 0;
          }
        }

        var distinctAqlLevelIds = aqlLevelIds.filter((n, i) => aqlLevelIds.indexOf(n) === i);

        if (distinctAqlLevelIds.length > 1) {
          this.combineOrderAQLData = combineOrderAQLData;
          this.userAQLList = this.combineOrderFilter.aqlList.filter(x => distinctAqlLevelIds.includes(x.id));
          // this.userAQLList=this.userAQLList.slice(Math.max(this.userAQLList.length - 1, 0));
          this.combineOrderAQLData = combineOrderAQLData;
          this.userSelectedAqlId = null;
          this.dialog = this.modalService.open(this.engineModal, { ariaLabelledBy: 'modal-basic-title', centered: true, backdrop: 'static' });
        }
      }

    }

  }

  //process the popup apply button when user selects the desired aql from the popup
  setNewAQLLevel() {
    //this.productAqlId = this.newAqlId;
    var combineGroupOrders = this.combineOrdersList.filter(x => x.combineProductId == this.currentCombineGroupId);
    if (combineGroupOrders && combineGroupOrders.length > 1) {
      combineGroupOrders.forEach(element => {
        if (!element.isDisplayMaster)
          element.aqlLevel = this.userSelectedAqlId;
      });
    }
    if (this.combineOrderAQLData.length > 1)
      this.getSamplingQuantity(this.combineOrderAQLData, this.userSelectedAqlId);

    //apply group color logic
    this.applyGroupColor();

    this.mapOrdersToProductList();
    this.mapNonCombinedProductList();
    if (this.dialog) {
      this.dialog.dismiss();
      this.dialog = null;
    }
  }

  //select all option
  onSelectAll() {
    this.combineOrderFilterRequest.filterProductIds = [];
    for (var i = 0; i < this.combineOrderFilter.productList.length; i++) {
      this.combineOrderFilterRequest.filterProductIds.push(this.combineOrderFilter.productList[i].id);
    }
  }
  //unselect all option
  onClearAll() {
    this.combineOrderFilterRequest.filterProductIds = [];
  }

  // limitTableHeight() {
  //   let height = this.scrollableTable ?
  //     this.scrollableTable.nativeElement.offsetHeight : 0;
  //   if (height > CombineBookingTableScrollHeight) {
  //     this.scrollableTable.nativeElement.classList.add('scrollable');
  //   }
  // }
}
