import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CustomerDashboardFilterModel, CustomerDashboardFilterMaster } from '../../../_Models/dashboard/customerdashboardfilterrequest.model';
import { BookingService } from '../../../_Services/booking/booking.service';
import { SupplierService } from '../../../_Services/supplier/supplier.service'
import { first } from 'rxjs/operators';
import { LocationService } from '../../../_Services/location/location.service';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { from } from 'rxjs';
import { DateObject } from '../../common/static-data-common'
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'app-customer-side-filter',
	templateUrl: './customer-side-filter.component.html',
	styleUrls: ['./customer-side-filter.component.scss']
})
export class CustomerSideFilterComponent implements OnInit {

	@Input() customerInfo: any;
	@Input() toggleFilter: Boolean;
	@Output() filterClosed = new EventEmitter;
	@Output() filterModel = new EventEmitter<CustomerDashboardFilterModel>();
	model: CustomerDashboardFilterModel;
	customerData: any;
	dashBoardFilterMaster: CustomerDashboardFilterMaster;
	serviceDateFrom: DateObject;

	constructor(
		public bookingService: BookingService,
		public supplierService: SupplierService,
		public locationService: LocationService,
		public calendar: NgbCalendar
	) {

	}

	ngOnInit() {
		this.customerData = [];
		this.customerData.push({ "id": this.customerInfo.id, "name": this.customerInfo.name });
		this.model = new CustomerDashboardFilterModel();
		this.model.customerId = this.customerInfo.id;
		this.dashBoardFilterMaster = new CustomerDashboardFilterMaster();
		//this.GetOffice();
		//this.GetCustomerByUserType();

		this.getSuppliersbyCustomers(this.customerInfo.id);
		this.getCountryList();
		this.model.serviceDateFrom = this.calendar.getPrev(this.calendar.getToday(), 'm', 6);
		this.model.serviceDateTo = this.calendar.getNext(this.calendar.getToday(), 'm', 1);
		
		/* this.serviceDateFrom=new DateObject();
		this.serviceDateFrom.day=31;
		this.serviceDateFrom.month=2;
		this.serviceDateFrom.year=2020;
		this.model.serviceDateFrom=this.serviceDateFrom; */
		/* var date = new Date();
		var date1 = new Date();
		//fromDate.setMonth(fromDate.getMonth()-2);
		var serviceDateFrom = { day: date1.getDate(), year: date1.getFullYear(), month: date1.getMonth() };
		var serviceDateTo = { day: date.getDate(), year: date.getFullYear(), month: date.getMonth()+1 };
		
		this.model.serviceDateTo = serviceDateTo;
		//serviceDateTo = { day: date.getDate(), year: date.getFullYear(), month: date.getMonth() };
		this.model.serviceDateFrom = serviceDateTo; */
	}

	// function to toggle filter state and emit the toggle event
	toggleFilterState() {
		this.toggleFilter = !this.toggleFilter;
		this.filterClosed.emit(false);
	}

	/*  GetCustomerByUserType() {
		this.bookingService.GetCustomerByUserType().subscribe(
			response => {
				if (response && response.result == 1) {
					this.dashBoardFilterMaster.customerList = response.customerList;
				}
			}
		);
	} */
	/*
		GetOffice() {
			this.bookingService.GetOffice().subscribe(
				response => {
					if (response && response.result == 1) {
						this.dashBoardFilterMaster.officeList = response.officeList;
					}
				}
			);
		} */

	getSuppliersbyCustomers(customerId) {

		this.supplierService.getSuppliersbyCustomer(customerId)
			.pipe()
			.subscribe(
				res => {
					if (res && res.result == 1) {
						this.dashBoardFilterMaster.supplierList = res.data;
					}
					else {
						this.dashBoardFilterMaster.supplierList = [];
					}
				},
				error => {
					this.dashBoardFilterMaster.supplierList = [];
				});
	}

	getCountryList() {

		this.locationService.getCountrySummary()
			.pipe()
			.subscribe(
				data => {

					if (data && data.result == 1) {
						this.dashBoardFilterMaster.countryList = data.countryList;
					}

				},
				error => {

				});
	}

	sendFilterModelToDashBoard() {
		this.filterModel.emit(this.model);
	}

}
