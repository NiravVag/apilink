import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'app-side-filter',
	templateUrl: './side-filter.component.html',
	styleUrls: ['./side-filter.component.scss']
})
export class SideFilterComponent implements OnInit {

	@Input() toggleFilter: Boolean;
	@Output() filterClosed = new EventEmitter;

	cars: Array<any> = [];
	cars2 = [
	{ id: 1, name: 'Volvo' },
	{ id: 2, name: 'Saab' }
	];
	cars3 = [
	{ id: 1, name: 'Volvo' },
	{ id: 2, name: 'Saab' }
	];

	constructor() {
		this.cars = [
		{ id: 1, name: 'Volvo' },
		{ id: 2, name: 'Saab' }
		];
	}

	ngOnInit() {
	}

	// function to toggle filter state and emit the toggle event
	toggleFilterState(){
		this.toggleFilter = !this.toggleFilter;
		this.filterClosed.emit(false);
	}

}
