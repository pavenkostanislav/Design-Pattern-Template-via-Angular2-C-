import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';

@Component({
	selector: 'paging',
	styles: [`
		ul.pagination {
			margin-top: 0;
			margin-bottom: 10px;
		}`],
	template: `
	<ul class="pagination">
                  
		<li [class.disabled]="currentPage < countButton" class="pg-btn-prev-prev">
			<a (click)="selectPage(currentPage-countButton)" title="Предыдущие {{countButton}} страниц" role="button">&nbsp;&laquo;&nbsp;</a>
		</li>
		<li [class.disabled]="currentPage === 0" class="pg-btn-prev">
			<a (click)="selectPage(currentPage-1)" title="Предыдущая страница" role="button">&nbsp;&#8249;&nbsp;</a>
		</li>
		<li *ngFor="let n of Pages" class="pg-btn-page">
			<a (click)="selectPage(n)"
				[ngClass]="{'btn-primary text-bold disabled pg-btn-page-current': currentPage == n}"
				[class.active]="currentPage == n" role="button">
				&nbsp;{{n+1}}&nbsp;
			</a>
		</li>
		<li [class.disabled]="currentPage === countPage - 1" class="pg-btn-next">
			<a (click)="selectPage(currentPage+1)" title="Следующая страница" role="button">&nbsp;&#8250;&nbsp;</a>
		</li>
		<li [class.disabled]="!hasNextPageBlock()" class="pg-btn-next-next">
			<a (click)="hasNextPageBlock() && selectPage(currentPage + countButton)" [class.disabled]="!hasNextPageBlock()" title="Следующие {{countButton}} страниц" role="button">&nbsp;&raquo;&nbsp;</a>
		</li>		
	</ul>`
})
export class Paging implements OnChanges {
	logPaging = false;

	@Input() countPage: number;
	@Input() currentPage: number;
	@Input() countButton: number;

	@Output() onChangePage: EventEmitter<number> = new EventEmitter<number>();


	Pages: number[] = [];

	constructor() { }

	ngOnChanges(changes: SimpleChanges) {
		if (this.logPaging) {
			console.log('Paging.ngOnChanges', this.countPage, this.currentPage, this.countButton);
		}

		this.refreshButtons();

	}

	refreshButtons() {

		if (this.logPaging) {
			console.log('Paging.refreshButtons', this.countPage, this.currentPage, this.countButton);
		}
		// первая кнопка
		let startPage = this.currentPage - (this.currentPage % this.countButton);

		//кол-во кнопок
		let buttonsOnPage = Math.min(this.countPage - startPage, this.countButton);

		this.Pages = Array<number>(buttonsOnPage);

		// для цикла
		let i = 0;

		for (i; i < buttonsOnPage; i++ , startPage++) {
			this.Pages[i] = startPage;
		}

	}

	hasNextPageBlock() {

		//let ret = Math.floor((this.currentPage + 3) / this.countButton) < Math.floor(this.countPage / this.countButton);
		let ret = this.currentPage < this.countPage - this.countButton;
		return ret;
	}

	selectPage(n: number) {

		if (this.logPaging) {
			console.log('Paging.selectPage', n);
		}

		if (n >= this.countPage) {
			n = this.countPage - 1;
		}
		if (n == null) {
			n = 0;
		}
		if (n < 0) {
			n = 0;
		}

		if (n === this.currentPage) {
			return;
		}

		this.onChangePage.emit(n);
	}
}

