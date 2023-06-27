import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class SideBarService {

	// creating behaviorSubject and setting to 
	//currentSideBar variable as observable
	private toggleSideBar = new BehaviorSubject<boolean>(false);
	sideBarRequired = this.toggleSideBar.asObservable();

	constructor() { }

	// function to get updated value and pass it to app
	changeToggleSideBar(data: boolean) {
		return this.toggleSideBar.next(data);
	}
}
