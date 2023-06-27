import { Component, OnInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import * as c3 from 'c3';

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

	@ViewChild('leftColumn') leftColumn: ElementRef;
	@ViewChild('sideOverlay') sideOverlay: ElementRef;
	@ViewChild('rightColumn') rightColumn: ElementRef;

	isLoading: boolean;
	graphWidth: number;
	filterState: boolean;
	overlayHeight: number;

	// set this object to change the order graph
	order = {
		claim: 40,
		complaints: 60
	}

	constructor(private cdr: ChangeDetectorRef) {
		this.isLoading = false;
		this.filterState = false;
	}

	ngOnInit() {

		// setTimeout(() => {
		// 	this.isLoading = false;
		// }, 3000);

		if(window.innerWidth > 1400) {
			this.graphWidth = 260;
		} else {
			this.graphWidth = 220;
		}
	}

	ngAfterViewInit() {

		// line chart settings
		let chart = c3.generate({
			bindto: '#chart',
			size : {
				height : 160,
				width : this.graphWidth
			},
			data: {
				columns: [
				['data1', 30, 200, 100, 200, 200, 80, 250],
				['data2', 100, 80, 200, 150, 60, 300, 180]
				],
				colors: {
					data1: '#D129E6',
					data2: '#4880FF',
				},
			},
			point: {
				show: false
			},
			axis: {
				x: {
					type: 'category',
					categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
				},
				y: {
					show: false
				}
			},
			legend: {
				show: false
			},
		});

		// bar chart settings
		let barChart = c3.generate({
			bindto: '#barChart',
			size : {
				height : 180,
				width : 130
			},
			data: {
				columns: [
				['Haufang Textile Co Ltd', 44, 32, 16, 9, 3]
				],
				type: 'bar',
				labels: {
					format:function (v, id, i, j) { return v + '%'; }
				}
			},
			bar: {
				width: {
					ratio: 1
				},
				space: 0.10
			},
			axis: {
				rotated: true,
				x: {
					show: false
				},
				y: {
					show: false
				}
			},
			legend: {
				show: false
			},
			tooltip: {
				format: {
					title: function (d) { return null; }
				}
			}
		});

		// setting lieft side overlay height as per left column
		var leftOverlayHeight = this.leftColumn.nativeElement.offsetHeight;

		this.overlayHeight = leftOverlayHeight;
		this.cdr.detectChanges();
	}


	// function to toggle filter component
	toggleFilterState(){
		this.filterState = !this.filterState;
	}

}
