import { Component, Input } from '@angular/core';

@Component({
	selector: '[shadowbox]',
	template: `
		<div *ngIf="show" class="shadow-box" >
			<div class="shadow-box-body">
				<i class="fa fa-2x fa-spinner fa-pulse"></i>
			</div>
		</div>
		<ng-content></ng-content>`,
	styles: [
		`:host {
			position: relative;
		}`,
		`.shadow-box {
			position: absolute;
			top: 0;
			left: 0;
			z-index: 500;
			background-color: #000;
			opacity: .3;
			width: 100%;
			height: 100%;
			text-align: center;
		}`,
		`.shadow-box-body { 
			margin: auto;
			position: absolute;
			top: 0;
			left: 0;
			bottom: 0;
			right: 0;
			width: 50%;
			height: 50%;
			color: white;}`]

})
export class ShadowBox {
	@Input() show = false;
}