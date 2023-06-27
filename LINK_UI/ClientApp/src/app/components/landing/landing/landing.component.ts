import { Component, HostListener, OnInit } from '@angular/core';
const mapboxgl = require('mapbox-gl');
import { ActivatedRoute, Router } from '@angular/router';
import { APIService, appThemeList, EntityAccess, HRProfileEnum, RoleEnum, Url, UserType } from '../../common/static-data-common';
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { WorkerMiddlewareService } from '../../../_Services/common/worker.service';
import { UserModel } from 'src/app/_Models/user/user.model';
import { HeaderService } from 'src/app/_Services/header/header.service';
import { DashBoardService } from 'src/app/_Services/dashboard/dashboard.service';
import { RoleModel } from 'src/app/_Models/user/role.model';
import * as CryptoJS from 'crypto-js';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { HrService } from 'src/app/_Services/hr/hr.service';
import { ResponseResult } from 'src/app/_Models/useraccount/userprofile.model';
import { UserProfileService } from 'src/app/_Services/UserAccount/userprofile.service';

@Component({
	selector: 'app-landing',
	templateUrl: './landing.component.html',
	styleUrls: ['./landing.component.scss']
})
export class LandingComponent {
	drawerIsOpen: boolean;
	photourl: string;
	currentUser: UserModel;
	returnUrl: string;
	togglePhoneContainer: boolean;
	insptasklist: any;
	apiServiceEnum = APIService
	apiEntityAccess = EntityAccess;
	entityList: any = [];
	entityAccessLoading: boolean = false;
	selectedEntity: number;
	pageLoading: boolean = false;
	year = 0;
	currentClass: string;
	appThemeList = appThemeList;
	showUserPanel: boolean = false;
	_internalusertype = UserType.InternalUser;
	_userTypeId;
	@HostListener('document:click', ['$event']) public onClick(event) {
		let userTrigger = document.querySelector('#userTrigger');
		let userContainer = document.querySelector('#userContainer');

		if (!userTrigger.contains(event.target) && !userContainer.contains(event.target)) {
			this.showUserPanel = false;
		}
	}

	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private authenticationService: AuthenticationService, private workerService: WorkerMiddlewareService,
		public headerService: HeaderService, public utility: UtilityService, private service: DashBoardService,
		private hrService: HrService, private userProfileService: UserProfileService
	) {
		this.drawerIsOpen = false;
		this.photourl = './assets/images/default-profile.svg';
		this.currentUser = authenticationService.getCurrentUser();
		this._userTypeId = this.currentUser.usertype;
		this.selectedEntity = Number(this.utility.getEntityId());
		this.applyThemeByCurrentEntity(this.selectedEntity);
		this.setPicture();
		this.headerService.getTasks();
		this.insptasklist = this.headerService.taskList;
		var d = new Date();
		this.year = d.getFullYear();
	}

	checkUserHasAccess(serivceAccess): boolean {
		return this.authenticationService.checkUserHasAccess(serivceAccess) && this.authenticationService.checkUserRightTypeAccess(serivceAccess);
	}

	Redirect(service) {
		localStorage.setItem('service', JSON.stringify(service));
		localStorage.setItem('rightType', JSON.stringify(service));
		this.authenticationService.RedirectWithServiceAccess(service);
	}

	ngOnInit() {
		this.getUserEntityAccess(this.currentUser.id);
		let currentEntityId = JSON.parse(localStorage.getItem('_entityId'));
		this.getUserEntityRoleAccess(currentEntityId);
	}

	getUserEntityAccess(userId) {
		this.entityAccessLoading = true;
		this.authenticationService.getUserEntityAccess(userId)
			.pipe()
			.subscribe(
				data => {
					if (data) {
						data.map(x => {
							if (x.id == this.apiEntityAccess.API) {
								this.entityList.push({ id: x.id, name: x.name, image: 'api-small.png' });
							}
							if (x.id == this.apiEntityAccess.SGT) {
								this.entityList.push({ id: x.id, name: x.name, image: 'sgt-small.svg' });
							}
							if (x.id == this.apiEntityAccess.AQF) {
								this.entityList.push({ id: x.id, name: x.name, image: 'aqf-small.png' });
							}

							this.entityList = [...this.entityList];
						
						});
					}

					this.entityAccessLoading = false;

				},
				error => {
					this.entityAccessLoading = false;
				});
	}


	getUserEntityRoleAccess(entityId) {
		this.pageLoading = true;
		this.authenticationService.getUserRoleEntityAccess(this.currentUser.id, entityId)
			.pipe()
			.subscribe(
				data => {
					if (data && data.result == 1) {
						let user = JSON.parse(localStorage.getItem('currentUser'));

						// overrried roles & rights and service access				
						user.serviceAccess = data.serviceAccess;
						user.roles = data.roles.map((x) => {
							var role: RoleModel = {
								id: x.id,
								roleName: x.roleName
							}
							return role;
						});
						user.rights = [];
						user.rightsDB = data.rights;
						localStorage.setItem('currentUser', JSON.stringify(user));

					}
					this.pageLoading = false;
				},
				error => {
					this.pageLoading = false;
				});
	}

	entityChange(entityEvent) {
		if (entityEvent && entityEvent.name) {
			this.selectedEntity = entityEvent.id;
			localStorage.setItem('_entity', this.encryptString(entityEvent.name));
			localStorage.setItem('_entityId', this.encryptString(entityEvent.id));
			this.applyThemeByCurrentEntity(entityEvent.id);
			this.getUserEntityRoleAccess(this.encryptString(entityEvent.id));
		}
	}

	encryptString(data): string {
		var key = CryptoJS.enc.Utf8.parse('1234567891012345');
		var iv = CryptoJS.enc.Utf8.parse('1234567891012345');
		var encryptedData = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(data), key,
			{
				keySize: 128 / 8,
				iv: iv,
				mode: CryptoJS.mode.CBC,
				padding: CryptoJS.pad.Pkcs7
			});
		return encryptedData.toString();
	}

	RedirectToLearMore() {
		if (this.selectedEntity == EntityAccess.API) {
			window.open('https://www.api-hk.com/');
		}
		else if (this.selectedEntity == EntityAccess.SGT) {
			window.open('https://www.sgtgroup.net/en/');
		}
		else if (this.selectedEntity == EntityAccess.AQF) {
			window.open('https://www.asiaqualityfocus.com/');
		}
	}

	public signOut() {
		this.authenticationService.logout();
		this.workerService.unregister();
		this.router.navigate(['/login']);
	}

	togglePhoneDetailContainer() {
		this.togglePhoneContainer == false;
	}

	redirectToProfilePage() {
		this.router.navigate(["/" + this.utility.getEntityName() + Url.UserProfile + this.currentUser.id]);
	}

	//apply theme based on current entity
	applyThemeByCurrentEntity(currentEntity) {
		//get the body element
		const bodyElement = document.body;

		//remove the current class
		if (this.currentClass)
			bodyElement.classList.remove(this.currentClass);

		//get the theme class and apply to the body
		var themeClass = appThemeList.find(x => x.id == currentEntity);
		if (themeClass && themeClass.name) {
			this.currentClass = themeClass.name;
			bodyElement.classList.add(themeClass.name);
		}
	}

	async setPicture() {
		if (this.currentUser.usertype == UserType.InternalUser) {
			this.hrService.getPicture(this.currentUser.staffId).subscribe(data => {
				if (data && data.result == 1 && data.fileUrl != null) {
					this.photourl = data.fileUrl;
				}
			}, error => {
				console.log(error);
			});
		}
		else {
			var response = await this.userProfileService.getUserProfileSummary(this.currentUser.id);
			if (response.result == ResponseResult.Success && response.data.profileImageUrl != null && response.data.profileImageUrl != '')
				this.photourl = response.data.profileImageUrl;
		}
	}
	toggle() {
		this.showUserPanel = !this.showUserPanel;
	}

}

