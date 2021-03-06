﻿import { Directive, ElementRef, Renderer, OnDestroy, OnInit } from '@angular/core';

@Directive({
	selector: '[draggable]',
	host: {
		'(dragstart)': 'onDragStart($event)',
		'(dragend)': 'onDragEnd($event)',
		'(drag)': 'onDrag($event)'
	}
})
export class Draggable implements OnDestroy, OnInit {
	private Δx = 0;
	private Δy = 0;
	private mustBePosition: Array<string> = ['absolute', 'fixed', 'relative'];
	constructor(
		private el: ElementRef, private renderer: Renderer
	) {
		//try {
		//	if (this.mustBePosition.indexOf(this.el.nativeElement.style.position) === -1) {
		//		console.warn(this.el.nativeElement, 'Must be having position attribute set to ' + this.mustBePosition.join('|'));
		//	}
		//} catch (ex) {
		//	console.error(ex);
		//}
	}
	public ngOnInit(): void {
		this.renderer.setElementAttribute(this.el.nativeElement, 'draggable', 'true');
	}
	onDragStart(event: MouseEvent) {
		this.Δx = event.x - this.el.nativeElement.offsetLeft;
		this.Δy = event.y - this.el.nativeElement.offsetTop;
	}
	onDrag(event: MouseEvent) {
		//this.doTranslation(event.x, event.y);
	}
	onDragEnd(event: MouseEvent) {

		this.doTranslation(event.x, event.y);
		this.Δx = 0;
		this.Δy = 0;
	}
	doTranslation(x: number, y: number) {
		if (!x || !y) { return; };
		this.renderer.setElementStyle(this.el.nativeElement, 'top', (y - this.Δy) + 'px');
		this.renderer.setElementStyle(this.el.nativeElement, 'left', (x - this.Δx) + 'px');
	}
	public ngOnDestroy(): void {
		this.renderer.setElementAttribute(this.el.nativeElement, 'draggable', 'false');
	}

}
