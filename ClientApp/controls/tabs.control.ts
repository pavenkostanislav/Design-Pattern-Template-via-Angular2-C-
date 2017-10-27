import { Component, Input, ContentChildren, Query, QueryList, AfterContentInit } from '@angular/core';

@Component({
	selector: 'tab',
	template: `
    <div *ngIf="active || !visible" class="pane tab">
      <ng-content></ng-content>
    </div>
  `,
	styles: [`
		.tab {
		padding-top: 10px;
	}`]
})
export class Tab {
	@Input('title') title: string;
	@Input() active = false;
	@Input() visible = true;
	@Input() disabled = false;
}

@Component({
	selector: 'tabs',
	template: `
	<ul class="nav nav-tabs">
      <li *ngFor="let tab of tabs" (click)="selectTab(tab)" [class.active]="tab.active" >
        <a class="cursor-pointer">{{tab.title}}</a>
      </li>
    </ul>
    <ng-content></ng-content>
	`,
	styles: [`
		li {
			cursor: pointer;
		}`]

})
export class Tabs implements AfterContentInit {

	@ContentChildren(Tab)
	tabs: QueryList<Tab>;

	_tabs: Tab[];
	//constructor( @ContentChildren(Tab) items: QueryList<Tab>) {
	//	this.tabs = items;
	//}

	ngAfterContentInit() {
		// get all active tabs
		let activeTabs = this.tabs.filter((tab) => tab.active);

		// if there is no active tab set, activate the first
		if (activeTabs.length === 0) {
			this.selectTab(this.tabs.toArray()[0]);
		}
	}

	selectTab(tab: Tab) {

		if (!tab) {
			return;
		}
		if (tab.disabled) {
			return;
		}

		// deactivate all tabs
		this.tabs.forEach(tab => tab.active = false);

		// activate the tab the user has clicked on.
		tab.active = true;
	}
}
