import { Component, OnInit, HostListener } from '@angular/core';
import { AuthenticationService } from '../../../_Services/user/authentication.service';
import { Router } from '@angular/router';
import { UserModel } from '../../../_Models/user/user.model';
import { MenuItemModel } from '../../../_Models/user/menuItem.model';
import { HrService } from '../../../_Services/hr/hr.service';
import { OutSourceUserType, RightTypeEnum, UserType } from '../../common/static-data-common';
import { DataServiceService } from "../../../_Services/common/data-service-service.service";
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { UserRightTypeAccess } from 'src/app/_Models/common/common.model';
import { RightType, RightTypeResult } from 'src/app/_Models/user/right-type-response.model';
@Component({
	selector: 'app-drawer-menu',
	templateUrl: './drawer-menu.component.html',
	styleUrls: ['./drawer-menu.component.scss']
})
export class DrawerMenuComponent {
	public User: UserModel;
	photourl: string
	_userTypeId;
	usergender: string;
	_internalusertype = UserType.InternalUser;
	userRightTypeAccessList: Array<UserRightTypeAccess>;
	selectedRightType: UserRightTypeAccess;
	showRightTypeList: boolean;
	rightTypeList: Array<RightType>;

	constructor(private authenticationService: AuthenticationService,
		private router: Router, private service: HrService, private ds: DataServiceService, public utility: UtilityService) {
		// Load user
		if (localStorage.getItem('currentUser')) {
			this.User = JSON.parse(localStorage.getItem('currentUser'));
			// console.log(" &&&&&&&&&&&&&&&    this.User &&&&&&&&&&&&&&&&&&&&&&&&&");
			//	console.log(this.User);
		}
		this._userTypeId = this.User.usertype;
		this.photourl = './assets/images/user-default.svg';
		this.GetUserDetails();

		//get user right type access list
		this.getUserRightTypeAccessList();
	}
	geturl(item: MenuItemModel) {
		return "/" + this.utility.getEntityName() + "/" + item.path;
	}
	@HostListener('click', ['$event']) onMouseEnter(event) {
		if (event.target.tagName == 'A') {
			if (event.target.nextSibling == null || event.target.href != "") {
				this.closeDrawer();
			}
		}
	}
	// function to open child menu in drawer
	openChild(event) {
		event.target.classList.toggle('active');
		event.target.firstElementChild.classList.toggle('active');
		event.target.nextElementSibling.classList.toggle('active');
	}

	// function to close the drawer
	closeDrawer() {
		this.ds.changeToggleDrawer(false);
		(<HTMLElement>document.querySelector('.main-wrapper')).classList.remove('drawer-active');
		(<HTMLElement>document.querySelector('.drawer-menu-container')).classList.remove('active');
	}
	public signOut() {
		this.authenticationService.logout();
		this.router.navigate(['/login']);
		setTimeout(function () {
			window.location.reload();
		}, 10);


	}
	setBlobFile(blob) {
		if (blob) {
			var reader = new FileReader();
			reader.readAsDataURL(blob); // read file as data url
			reader.onload = (event) => { // called once readAsDataURL is completed
				this.photourl = (<FileReaderEventTarget>event.target).result;
			}
		}
	}
	GetMenuParentId(menuid) {
		localStorage.setItem("_activemenuid", menuid)
	}
	GetUserDetails() {
		if (this._userTypeId == this._internalusertype) {
			this.service.GetGender()
				.subscribe((res) => {
					if (res && res.result == 1) {
						this.usergender = res.gender;
						if (res.isPhotovailable)
							this.GetPhoto();
					}
					else {
						this.photourl = './assets/images/user-default.svg';
					}
				},
					error => console.log(error)
				);

		}
		else {
			this.photourl = './assets/images/user-default.svg';
		}
	}
	GetPhoto() {
		if (this._userTypeId == this._internalusertype) {
			this.service.getPicture(this.User.staffId).subscribe(data => {
				if (data && data.result == 1) {
					this.photourl = data.fileUrl;
				}
				else {
					this.SetPhoto();
				}
			}, error => {
				this.SetPhoto();
			});
		}
	}
	SetPhoto() {
		if (this._userTypeId == this._internalusertype) {
			if (this.photourl && this.usergender == 'M')
				this.photourl = 'assets/images/male.jpg';
			else if (this.photourl && this.usergender == 'F')
				this.photourl = 'assets/images/female.jpg';
			return this.photourl;
		}

	}

	//toggle the rightType list dropdown data
	toggleRightTypeData() {
		this.showRightTypeList = !this.showRightTypeList
	}

	//assign the selected rightType and navigates to the selected menu
	changeRightType(rightType) {
		this.showRightTypeList = !this.showRightTypeList;
		if (rightType) {
			this.selectedRightType = rightType;
			//update the user data based on the selected rightType
			if (rightType.id) {
				localStorage.setItem('rightType', JSON.stringify(rightType.id));
				this.User = this.authenticationService.getCurrentUser();
				this.authenticationService.RedirectWithRightTypeAccess(rightType.id);
			}
		}

	}

	async getUserRightTypeAccessList() {
		const rightTypeResponse = await this.authenticationService.getRightTypelst();
		if (rightTypeResponse.result == RightTypeResult.Success) {
			if (this._userTypeId == this._internalusertype)
				this.rightTypeList = rightTypeResponse.rightTypeList;
			else if (this._userTypeId == UserType.OutSource)
				this.rightTypeList = rightTypeResponse.rightTypeList.filter(x => OutSourceUserType.includes(x.id));
			else
				this.rightTypeList = rightTypeResponse.rightTypeList.filter(x => x.id == RightTypeEnum.Inspection || x.id == RightTypeEnum.Audit || x.id == RightTypeEnum.TCF || x.id == RightTypeEnum.LabTesting);

			const rightTypeAccessList = this.authenticationService.getUserRightTypeAccessList(this.rightTypeList);
			if (rightTypeAccessList.length > 0) {
				this.userRightTypeAccessList = rightTypeAccessList.filter(x => x.userHasAccess);
				//read the selected rightType from the localstorage
				const rightTypeId = localStorage.getItem("rightType");
				if (rightTypeId) {
					//filter the rightType data and assign 
					const rightTypeData = this.userRightTypeAccessList.find(x => x.id == Number(rightTypeId));
					if (rightTypeData)
						this.selectedRightType = rightTypeData;
				}
			}
		}
	}
}
interface FileReaderEventTarget extends EventTarget {
	result: string
}
interface FileReaderEvent extends Event {
	target: FileReaderEventTarget;
	getMessage(): string;
}

