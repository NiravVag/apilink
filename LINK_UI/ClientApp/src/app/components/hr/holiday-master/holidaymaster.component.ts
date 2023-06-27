import { Component, NgModule } from '@angular/core';
import { HrService } from '../../../_Services/hr/hr.service'
import { HolidayMasterModel, HolidayMasterItemModel, HolidayRequest } from '../../../_Models/hr/holidaymaster.model'
import { first } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbCalendar, NgbDate, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Validator, JsonHelper } from '../../common'
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-holidaymaster',
  templateUrl: './holidaymaster.component.html',
  styleUrls: ['./holidaymaster.component.css']
})

export class HolidaymasterComponent {

  public data: any;
  loading = false;
  error = '';
  public model: HolidayMasterModel;
  public remStaffId: number;
  public remStaffName: string;
  fromDate: NgbDate;
  toDate: NgbDate;
  hoveredDate: NgbDate;
  public editHoliday: HolidayRequest;
  public editValidator: Validator;
  private jsonHelper: JsonHelper;
  private modelRef: NgbModalRef;
  private selectedDate: NgbDate;
  public none = "none";
  public isEdit: boolean;
  public dateSelected : NgbDate;

  constructor(private service: HrService,
    private translate: TranslateService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    public validator: Validator,
    private calendar: NgbCalendar,
    private router: Router) {

    this.model = new HolidayMasterModel();
    this.model.year = (new Date()).getFullYear();
    this.fromDate = calendar.getToday();
    this.toDate = calendar.getNext(calendar.getToday(), 'd', 50);

    this.validator.setJSON("hr/holiday-master.valid.json");
    this.jsonHelper = validator.jsonHelper;
    this.validator.setModelAsync(() => this.model);
    this.validator.isSubmitted = false;

    this.editValidator = new Validator(this.jsonHelper, toastr, translate);
    this.editValidator.setJSON("hr/edit-holiday.valid.json");
    this.editValidator.setModelAsync(() => this.editHoliday);
    this.editValidator.isDebug = false;

    this.isEdit = this.router.url.indexOf("holiday-master") >= 0;


    this.data = service.getHolidaySummary()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
          }
          else {
            this.error = data.result;
            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

  refresh() {
    this.model.noFound = false;
    this.model.items = [];
    this.model.totalCount = 0;
    this.model.pageCount = 0;
    this.getData();
  }

  search() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    if (this.isFormValid()) {
      this.model.index = 1;
      this.refresh();
    }
  }

  getData() {

    this.service.getHolidaysData(this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {

            this.model.index = response.index;
            this.model.pageSize = response.pageSize;
            this.model.totalCount = response.totalCount;
            this.model.pageCount = response.pageCount;

            this.model.items = response.data.map((x) => {

              var tabItem: HolidayMasterItemModel = {
                id: x.id,
                name: x.name,
                recurrenceType: x.recurrenceType,
                endDate: x.endDate,
                startDate: x.startDate,
                startDay: null,
                endDay: null,
                startDayType: x.startDayType == null ? 0 : x.startDayType ,
                endDayType: x.endDayType == null ? 0 : x.endDayType,
              }

              return tabItem;
            });

          }
          else if (response && response.result == 2) {
            this.model.noFound = true;
          }
          else {
            this.error = response.result;
            this.loading = false;
            // TODO check error from result
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

  updateHolidayAll() {
    if (this.isEdit) {
      this.editHoliday.forAllIterations = true;
      this.updateHoliday();
    }
  }

  updateHolidayCurrent() {
    if (this.isEdit) {
      this.editHoliday.forAllIterations = false;
      this.updateHoliday();
    }
  }

  checkdateWithCurrent(date: NgbDate): boolean {

    if (date == null)
      return true;

    let currentDate = this.calendar.getToday();

    if (date.year > currentDate.year)
      return true;

    if (date.year == currentDate.year && date.month > currentDate.month)
      return true;

    if (date.year == currentDate.year && date.month == currentDate.month && date.day > currentDate.day)
      return true;

    return false;
  }

  updateHoliday() {

    if (this.isEdit) {
      if (this.editHoliday.startDate != null && this.editHoliday.startDate)
        this.editValidator.isSubmitted = true;

      if (this.editValidator.isFormValid()) {
        this.service.updateHoliday(this.editHoliday)
          .pipe()
          .subscribe(
            response => {
              if (response && (response.result == 1)) {

                this.showSuccess("HOLIDAY_MASTER.MSG_NOPOPUP", "HOLIDAY_MASTER.MSG_CALENDARISUPDATED");

                // refresh
                this.refresh();
              }
              else {
                this.error = response.result;

                this.loading = false;
                // TODO check error from result
              }

            },
            error => {
              this.error = error;
              this.loading = false;
            });

        this.modelRef.close();
      }
    }
  }

  deleteHolidayCurrent(id) {
    if(this.isEdit)
      this.deleteHoliday(id, false);
  }

  deleteHolidayAll(id) {
    if(this.isEdit)
      this.deleteHoliday(id, true);
  }

  deleteHoliday(id, forAllIterations: boolean) {

    if (this.isEdit) {
      this.service.deleteHoliday(id, forAllIterations)
        .pipe()
        .subscribe(
          response => {
            if (response && (response.result == 1)) {

              this.showSuccess("HOLIDAY_MASTER.MSG_NOPOPUP", "HOLIDAY_MASTER.MSG_HOLIDAY_REMOVED");

              // refresh
              this.refresh();
            }
            else {
              this.error = response.result;

              this.loading = false;
              // TODO check error from result
            }
          },
          error => {
            this.error = error;
            this.loading = false;
          });
    }
  }

  isFormValid(): boolean {
    return this.validator.isValid('countryId')
      && this.validator.isValid('year')
  }

  isDaySelected(date: NgbDate) {
    return this.model.items.some(x => (x.startDate != null && x.endDate != null && this.isBetween(date, x.startDate, x.endDate))
      || (x.startDate != null && x.startDate.year == date.year && x.startDate.month == date.month && x.startDate.day == date.day)
      || (x.endDate != null && x.endDate.year == date.year && x.endDate.month == date.month && x.endDate.day == date.day));
  }

  onDateSelection(date: NgbDate, content) {
    if (this.isEdit) {
      if (this.model.countryId == 0 || this.model.year == 0 || this.model.branchId == 0) {
        this.showError("HOLIDAY_MASTER.MSG_NOPOPUP", "HOLIDAY_MASTER.MSG_MUSTSELECTCOUNTRY");
        return;
      }
      console.log(this.model.branchId);

      this.editValidator.isSubmitted = false;

      //console.log("isSubmitted:" + this.editValidator.isSubmitted)
      console.log(this.dateSelected);
      let data = this.model.items.filter(x => (x.startDate != null && x.endDate != null && this.isBetween(this.dateSelected, x.startDate, x.endDate))
        || (x.startDate != null && x.startDate.year == this.dateSelected.year && x.startDate.month == this.dateSelected.month && x.startDate.day == this.dateSelected.day)
        || (x.endDate != null && x.endDate.year == this.dateSelected.year && x.endDate.month == this.dateSelected.month && x.endDate.day == this.dateSelected.day));

      console.log(data);
      if (data.length > 0) {

        let weekStartDay = data[0].startDate == null ? null : this.calendar.getWeekday(data[0].startDate);

        if (weekStartDay == 7)
          weekStartDay = 0;

        let weekEndDay = data[0].endDate == null ? null : this.calendar.getWeekday(data[0].endDate);

        if (weekEndDay == 7)
          weekEndDay = 0;

        this.editHoliday = {
          id: data[0].id,
          holidayName: data[0].name,
          countryId: this.model.countryId,
          officeId: this.model.branchId,
          recurrenceType: data[0].recurrenceType,
          startDate: data[0].startDate,
          endDate: data[0].endDate,
          startDay: weekStartDay,
          endDay: weekEndDay,
          endDateWeek: {},
          forAllIterations: false,
          startDayType: data[0].startDayType,
          endDayType: data[0].endDayType
        };

      }
      else {
        this.selectedDate = date;
        this.editHoliday = {
          id: 0,
          countryId: this.model.countryId,
          officeId: this.model.branchId,
          recurrenceType: 0,
          startDate: this.dateSelected,
          endDate: this.dateSelected,
          holidayName: "",
          forAllIterations: false,
          startDayType: 0,
          endDayType: 0
        };
      }

      this.modelRef = this.modalService.open(content, { windowClass: "mdModelWidth", centered: true, backdrop: 'static' });

      this.modelRef.result.then((result) => {
        this.selectedDate = null;
        // this.closeResult = `Closed with: ${result}`;
      }, (reason) => {
        this.selectedDate = null;
        //this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
    }
  }

  isHovered(date: NgbDate) {
    // return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
    return false;
  }

  isInside(date: NgbDate) {
    // return date.after(this.fromDate) && date.before(this.toDate);
    return false;
  }

  isRange(date: NgbDate, recType: number) {

    if (this.model.items == null || this.model.items.length == 0)
      return false;

    for (let item of this.model.items) {
      if (item.recurrenceType == recType) {
        if (item.startDate != null && date.equals(item.startDate))
          return true;

        if (item.endDate != null && date.equals(item.endDate))
          return true;

        if (item.startDate != null && item.endDate != null && this.isBetween(date, item.startDate, item.endDate))
          return true;
      }
    }
    return false;
  }


  showTooltip(tooltip, date: NgbDate) {


    if (!tooltip.isOpen()) {
      let data = this.model.items.filter(x => (x.startDate != null && x.endDate != null && this.isBetween(date, x.startDate, x.endDate))
        || (x.startDate != null && x.startDate.year == date.year && x.startDate.month == date.month && x.startDate.day == date.day)
        || (x.endDate != null && x.endDate.year == date.year && x.endDate.month == date.month && x.endDate.day == date.day));


      if (data.length > 0) {
        let msg: string = "";

        for (let item of data) {
          if (msg != "")
            msg += "\n";

          msg += item.name;
        }
        tooltip.open({ descript: msg });
      }
    }
  }

  hideTooltip(tooltip) {
    tooltip.close();
  }

  isBetween(date: NgbDate, startDate: NgbDate, endDdate: NgbDate): boolean {
    if (date.year < startDate.year)
      return false;

    if (date.year > endDdate.year)
      return false;

    if (date.year == startDate.year && date.month < startDate.month)
      return false;

    if (date.year == endDdate.year && date.month > endDdate.month)
      return false;

    if (date.year == startDate.year && date.month == startDate.month && date.day < startDate.day)
      return false;

    if (date.year == endDdate.year && date.month == endDdate.month && date.day > endDdate.day)
      return false;

    return true;

  }

  daysByMonth(year, month) {
    var now = new Date();
    return new Date(year, month + 1, 0).getDate();
  }

  months() {
    // TODO : to traduce
    return ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  }


  showSuccess(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.success(tradTitle, tradMessage);
  }

  showError(title: string, msg: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradTitle, tradMessage);
  }

  
  isSelected(date: NgbDate) {
    return this.selectedDate == date;
  }

}
