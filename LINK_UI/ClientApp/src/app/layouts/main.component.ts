import { Component } from '@angular/core';
import { WorkerMiddlewareService } from '../_Services/common/worker.service';
import { Router, RouterEvent, RouteConfigLoadStart, RouteConfigLoadEnd } from "@angular/router";
import { NetworkCheckService } from '../components/common/network-check';

@Component({
	selector: 'app-main-layout',
	templateUrl: './main.component.html',
	styleUrls: ['./main.component.scss']
})
export class MainComponent {
	title = 'APILINK ';
	allowScrollTop: boolean;
	showScrollTop: boolean;
	public isShowingRouteLoadIndicator: boolean;
	public _IsOnline: boolean;
	//public networkservice: NetworkCheckService
	constructor(workerService: WorkerMiddlewareService, router: Router, networkservice: NetworkCheckService) {
		var asyncLoadCount = 0;
		this.isShowingRouteLoadIndicator = false;
		workerService.init();
		this.allowScrollTop = true;
		this.showScrollTop = false;

		//to show loading while navigate between routes at first time
		router.events.subscribe(
			(event: RouterEvent): void => {
				if (event instanceof RouteConfigLoadStart) {
					networkservice.createOnline$().subscribe(x => this._IsOnline = x);
					if (!this._IsOnline)
						router.navigate(['/error/0']);
					else
						asyncLoadCount++;

				} else if (event instanceof RouteConfigLoadEnd) {

					asyncLoadCount--;

				}
				this.isShowingRouteLoadIndicator = !!asyncLoadCount;
			}
		);
	}
	ngOnInit() {
		window.addEventListener('scroll', e => {
			let top = window.pageYOffset || document.documentElement.scrollTop;

			if (top > 200) {
				this.showScrollTop = true;
			} else {
				this.showScrollTop = false;
			}
		});
	}

	scrollToTop() {
		window.scrollTo({ left: 0, top: 0, behavior: 'smooth' });
	}
}
