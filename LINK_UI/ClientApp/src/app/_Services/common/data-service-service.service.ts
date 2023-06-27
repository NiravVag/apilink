import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class DataServiceService {

	// creating behaviorSubject and setting to 
	//currentDrawerState variable as observable
	private toggleDrawer = new BehaviorSubject<boolean>(false);
	currentDrawerState = this.toggleDrawer.asObservable();

	constructor() { }

	// function to get updated value and pass it to app
	changeToggleDrawer(data: boolean) {
		return this.toggleDrawer.next(data);
	}
}
