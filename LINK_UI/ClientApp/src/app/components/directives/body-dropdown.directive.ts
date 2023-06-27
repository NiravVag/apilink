import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
	selector: '[appBodyDropdown]'
})
export class BodyDropdownDirective {

	el: ElementRef

	constructor(private ele: ElementRef) {
		this.el = ele;
	}

	@HostListener('mouseenter') onMouseEnter() {
		this.appentToBody(true);
	}

	@HostListener('mouseleave') onMouseLeave() {
		this.appentToBody(false);
	}

	/**
	 * Function to remove all elements
	 */
	ngOnDestroy(){
		document.querySelectorAll(".body-append-dropdown").forEach(e => {
			e.remove();
		});
	}

	/**
	 * Function to append dropdown element to body
	 * @param boolean append
	 */
	 appentToBody(append) {

	 	if(append) {

	 		// removing existing dropdowns
	 		document.querySelectorAll(".body-append-dropdown").forEach(e => {
	 			this.moveBackElement(e);
	 			// e.remove();
	 		});

	 		let dropdown = this.el.nativeElement.querySelector('.cta-dropdown');
	 		dropdown.classList.add('body-append-dropdown');
	 		dropdown.id = this.el.nativeElement.id;
	 		document.querySelector('body').append(dropdown);

	 		let position = this.el.nativeElement.getBoundingClientRect();
	 		let totalOffset = this.cumulativeOffset(this.el.nativeElement);

	 		// setting visibility and position
	 		dropdown.style.visibility = 'hidden';
	 		dropdown.style.display = 'block';
	 		dropdown.style.top = `${totalOffset.top}px`;
	 		dropdown.style.left = `${position.x - dropdown.offsetWidth}px`;
	 		dropdown.style.visibility = 'visible';
	 		dropdown.style.zIndex = '10';

	 	}
	 	else {
	 		// remove all dropdowns which does not currently have hover
	 		document.querySelectorAll(".body-append-dropdown").forEach(e => {
	 			if(!e.matches(":hover")){
	 				this.moveBackElement(e);
	 				// e.remove();
	 			}
	 			else {
	 				this.bindEventListner(e);
	 			}
	 		});
	 	}
	 }

	/**
	 * Function to move back dropdown element to its original position
	 * @param HTMLElement e
	 */
	 moveBackElement(e) {
	 	if(!e) { return; }
	 	let originalParent = document.querySelector(`#${e.id}`);
	 	e.classList.remove('body-append-dropdown');
	 	originalParent.append(e);
	 }

	/**
	 * Function to bind mouseleave event to the dropdown to remove
	 * @param HTMLElement el
	 */
	 bindEventListner(el) {
	 	el.addEventListener('mouseleave', (e) => {
	 		this.moveBackElement(el);
	 		// el.remove();
	 	});
	 }

	/**
	 * Function to get cumulative offset from top
	 * @param HTMLElement element
	 */
	 cumulativeOffset(element) {
	 	var top = 0, left = 0;
	 	do {
	 		top += element.offsetTop  || 0;
	 		left += element.offsetLeft || 0;
	 		element = element.offsetParent;
	 	} while(element);

	 	return {
	 		top: top,
	 		left: left
	 	};
	 }

	}
