import { Component, Input, OnInit, HostListener, EventEmitter, Output, ElementRef, ViewChild } from '@angular/core';

@Component({
	selector: 'app-drag-drop',
	templateUrl: './drag-drop.component.html',
	styleUrls: ['./drag-drop.component.scss']
})
export class DragDropComponent implements OnInit {

	inputFiles: any;
	dropHandler: any;
	errorFlag: boolean;
	errorMessage: string;
	fileArray: Array<any>;
	responseObject: object;
	fileCountErrorFlag: boolean;
	fileSizeErrorFlag: boolean;
	@Input() Bigupload?: boolean = true;
	@Input() SmallUploadImage?: string = "assets/images/picture.svg";
	@Input() SmallUploadTitle?: string = "Drop your file/document here";
	@Input() SmallUploadSubTitle?: string = "Select from your computer";
	@Input() SmallUploadButtonText?: string = "Add Files";
	@Input() fileLimit: number;
	@Input() fileExtension: any;
	@Input() fileSizeLimit: number;
	@Output() fileUpload = new EventEmitter();
	@ViewChild('fileHandler', { static: true }) fileHandler: ElementRef;

	constructor() {
		this.errorFlag = true;
		// this.fileSizeLimit = 104857600 ; // 100 MB
		this.fileArray = [];
		this.responseObject = {
			error: true,
			errorMessage: '',
			files: []
		}
	}

	ngOnInit() {
		console.log(this.fileLimit);
	}

	@HostListener('mouseover', ['$event']) onMouseOver(event) {
		event.preventDefault();
	}

	@HostListener('dragenter', ['$event']) onDragEnter(event) {
		event.preventDefault();
	}

	@HostListener('dragover', ['$event']) onDragOver(event) {
		// add active class on dragover
		(<HTMLElement>document.querySelector('.drag-drop-container')).classList.add('active');
		event.preventDefault();
	}

	@HostListener('dragleave', ['$event']) onDragLeave(event) {
		// remove active class on dragover
		(<HTMLElement>document.querySelector('.drag-drop-container')).classList.remove('active');
		event.preventDefault();
		event.stopPropagation();
	}

	@HostListener('drop', ['$event']) onDrop(event) {
		//check the extension available or not set the error flag
		// this.validateExtension();    
		// if (this.errorFlag) {
		// 	return;
		// }
		event.preventDefault();
		event.stopPropagation();
		this.fileDrop(event);
	}

	@HostListener('click', ['$event']) onClick(event) {
		//check the extension available or not set the error flag
		// this.validateExtension();
		// if (this.errorFlag) {
		// 	return;
		// }
		let el: HTMLElement = this.fileHandler.nativeElement as HTMLElement;
		el.click();
	}

	// function to handle input drag upload
	fileDrop(event) {
		let files = event.dataTransfer.files;
		this.saveFiles(files);
	}

	// function to handle input click upload
	inputFileUpload(event) {
		//check the extension available or not set the error flag
		// this.validateExtension();
		// if (this.errorFlag) {
		// 	return;
		// }
		let files = event.target.files;
		if (files && files.length > 0)
			this.saveFiles(files);
		event.target.value = '';
	}

	// function to validate files on various checks
	saveFiles(files) {
		if (files.length <= 0) {
			this.errorFlag = true;
			this.errorMessage = 'No file found';
			this.sendResponse();
			return;
		}
		else if (files.length > this.fileLimit) {
			this.errorFlag = true;
			this.fileCountErrorFlag = true;
			this.errorMessage = 'File limit exceeded';
			this.sendResponse();
			return;
		}
		else {
			this.errorFlag = false;
		}

		this.isValidFileExtension(files);
		this.isValidFileSize(files);

		if (!this.errorFlag) {
			for (var j = 0; j < files.length; j++) {
				this.fileArray.push(files[j]);
			}
			this.sendResponse();
			return;
		}
	}

	// function to emit response according to errorFlag
	sendResponse() {
		// if errorFlag is true send error to parent
		if (this.errorFlag) {
			this.responseObject['error'] = true;
			this.responseObject['errorMessage'] = this.errorMessage;
			this.responseObject['files'] = [];
		}
		else {
			this.responseObject['error'] = false;
			this.responseObject['errorMessage'] = '';
			this.responseObject['files'] = this.fileArray;
		}
		this.fileUpload.emit(this.responseObject);
		this.fileArray = [];
		return;
	}

	/**
	 * function to validate each file size accourding to input
	 * @param {Array} files Array of files
	 */
	isValidFileSize(files) {
		for (let f of files) {
			if (f.size > this.fileSizeLimit) {
				this.errorFlag = true;
				this.fileSizeErrorFlag = true;
				this.errorMessage = 'file Size Exceeded';
				this.sendResponse();
				break;
			}
		}
	}

	/**
	 * function to validate file extension
	 * @param {Array} files [Array of files]
	 */
	isValidFileExtension(files) {
		if (this.fileExtension) {
			// Make array of file extensions
			var extensions = (this.fileExtension.split(',')).map(function (x) { return x.toLocaleUpperCase().trim() });
			for (var i = 0; i < files.length; i++) {
				// Get file extension
				var ext = files[i].name.toUpperCase().split('.').pop() || files[i].name;
				// Check the extension exists
				var exists = extensions.includes(ext);
				if (!exists) {
					this.errorFlag = true;
					this.errorMessage = 'please upload correct file format';
					this.sendResponse();
					break;
				}
				else {
					this.errorFlag = false;
				}
			}
		}
	}

	//validate the extension is selected or not
	validateExtension() {
		//check the file extension is null then show the error
		if (!this.fileExtension) {
			this.errorFlag = true;
			this.errorMessage = 'please select file type';
			this.sendResponse();
		}
		else {
			this.errorFlag = false;
		}

	}
}
