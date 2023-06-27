import { TranslateService } from "@ngx-translate/core";
import { Injectable, Inject } from '@angular/core';
import { bookingStatusList, themeList } from 'src/app/components/common/static-data-common';
import { NgbDate } from "@ng-bootstrap/ng-bootstrap";
import * as CryptoJS from 'crypto-js';
import { environment } from "src/environments/environment";

@Injectable({ providedIn: 'root' })

export class UtilityService {
  constructor(private translate: TranslateService) {

  }
  // check the booking status limit based on the status id.
  checkBookingStatus(statusId: number): boolean {
    return bookingStatusList.filter(x => x.id == statusId).length > 0;
  }
  // localization translate text if we have to used in ts file 
  textTranslate(textTrans: string) {
    let tradMessage: string = "";
    this.translate.get(textTrans).subscribe((text: string) => { tradMessage = text });
    return tradMessage;
  }

  getContainerList(limit) {

    var containerMasterList = [];
    for (let index = 1; index <= limit; index++) {
      containerMasterList.push({ "id": index, "name": "container - " + index });
    }
    return containerMasterList;
  }

  //get the list of dates within a range
  getDatesBetweenDateRange(startDate: NgbDate, endDate: NgbDate) {
    let dates = [];
    //to avoid modifying the original date
    const theDate = new Date(startDate.year, startDate.month, startDate.day);
    const theEndDate = new Date(endDate.year, endDate.month, endDate.day);
    while (theDate <= theEndDate) {
      dates = [...dates, new Date(theDate).toString()]
      theDate.setDate(theDate.getDate() + 1)
    }
    return dates;
  }
  //numeric validation for maxItems
  numericValidation(event, length) {
    var invalidChars = [
      "-",
      "+",
      "e",
      "E"
    ];
    if (invalidChars.includes(event.key) || event.target.value.length >= length)
      event.preventDefault();
  }

  getEntityName(): string {
    var entityName = '';
    var key = CryptoJS.enc.Utf8.parse('1234567891012345');
    var iv = CryptoJS.enc.Utf8.parse('1234567891012345');
    if (localStorage.getItem('_entity')) {
      entityName = CryptoJS.AES.decrypt(localStorage.getItem('_entity'), key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      }).toString(CryptoJS.enc.Utf8);
    }
    return entityName;
  }
  getEntityId() {
    var entityId = '';
    var key = CryptoJS.enc.Utf8.parse('1234567891012345');
    var iv = CryptoJS.enc.Utf8.parse('1234567891012345');
    if (localStorage.getItem('_entity')) {
      entityId = CryptoJS.AES.decrypt(localStorage.getItem('_entityId'), key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      }).toString(CryptoJS.enc.Utf8);
    }
    return entityId;
  }

  formatDate(dateObject) {
    return dateObject.day + "/" + dateObject.month + "/" + dateObject.year;
  }

  decimalValidation(event, length) {
    var invalidChars = [
      "-",
      "+",
      "e",
      "E",
      "0"
    ];

    if (invalidChars.includes(event.key))
      event.preventDefault();
    if (event.target.value) {
      var splitedValue = event.target.value.split('.');
      if (splitedValue.length > 1 && splitedValue[1].length >= length)
        event.preventDefault();
    }
  }

  showPageField(fieldId, fieldList) {
    var showField = false;
    var field = fieldList.find(x => x.id == fieldId);
    if (field)
      showField = true;
    return showField;

  }

  //get the image path by entity and theme
  getImagePathbyEntityAndTheme(themeId) {

    //get the theme data
    var theme = themeList.find(x => x.id == themeId);
    //get the image path
    var imagePath = environment.entityImagePath;
    //replace the entity name
    imagePath = imagePath.replace("{0}", this.getEntityName().toLowerCase());
    //replace the theme
    if (theme)
      imagePath = imagePath.replace("{1}", theme.name);
    //return the imagepath
    return imagePath;
  }
}
