import { By } from '@angular/platform-browser';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { Component, OnChanges, Input, ViewChild, DebugElement, EventEmitter, SimpleChange } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { } from 'jasmine';
//import 'jquery';
//import 'bootstrap';


import { Paging } from './paging.control';

describe('paging.control', () => {

	beforeEach(async(() => {

		TestBed.configureTestingModule({
			declarations: [
				HostComponent,
				Paging
			],
			imports: [
				FormsModule,
				ReactiveFormsModule
			],
			providers: []
		}).compileComponents();

	}));

	it('Cоздания компонента Paging', async(() => {

		const fixture = TestBed.createComponent(Paging);
		const comp = fixture.componentInstance;
		expect(comp).toBeTruthy('ошибка создания компонента Paging');

		comp.logPaging = true;
		comp.countPage = 6;
		comp.currentPage = 2;
		comp.countButton = 8;
		comp.ngOnChanges({
			['countPage']: new SimpleChange(undefined, 4, true),
			['currentPage']: new SimpleChange( undefined, 2,  true),
			['countButton']: new SimpleChange( undefined, 8, true)
		});

		let selectedPage;

		comp.onChangePage.subscribe((page: number) => { selectedPage = page; });

		comp.selectPage(-1);
		expect(selectedPage).toBe(0, 'выбранная страница должна быть = 0');

		comp.selectPage(10);
		expect(selectedPage).toBe(5, 'выбранная страница должна быть = 5');

		comp.selectPage(2);
        expect(selectedPage).toBe(5, 'при выборе страницы с номером равным текущему событие смены страницы отрабатывать не должно');

        comp.selectPage(null);
        expect(selectedPage).toBe(0, 'выбранная страница должна быть = 0');

		expect(comp.hasNextPageBlock()).toBeFalsy('следующего ряда кнопок быть не должно');

	}));

	it('Cоздания компонента HostComponent', async(() => {
		const fixture = TestBed.createComponent(HostComponent);
		expect(fixture.componentInstance).toBeTruthy();
	}));

	it('Кнопок 5, страниц 4, текущая 3, клик на кнопке 2', async(() => {
		// создаем компонент
		const fixture = TestBed.createComponent(HostComponent);
		const comp = fixture.componentInstance;
		// устанавливаем параметры компонента
		comp.countButton = 5;
		comp.countPage = 4;
		comp.currentPage = 2; // 0 based

		// запускаем механизм обработки изменений ангулара
		fixture.detectChanges();

		// дожидаемся окончания обработки всех изменений
		fixture.whenStable().then(() => {

			// проверяем результат, считаем кол-во кнопок, проверяем доступность кнопок, "кликаем" на кнопке 2

			// всего кнопок страниц должно быть 4
			const items = fixture.debugElement.queryAll(By.css('.pg-btn-page'));
			expect(items.length).toEqual(4, 'Всего кнопок страниц должно быть 4');

			// кнопка текущей страницы
			const selectedButton = fixture.debugElement.query(By.css('.pg-btn-page-current'));
			expect(selectedButton).toBeTruthy('Не найдена кнопка текущей страницы');
			expect(selectedButton.nativeElement.textContent.trim()).toEqual('3', 'Не соответствие номера на кнопке текущей страницы [3]');

			// кнопка предыдущей страницы [<]
			const prevButton = fixture.debugElement.query(By.css('.pg-btn-prev'));
			expect(prevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<]');
			expect(prevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<] должна быть доступна');

			// кнопка предыдущей страницы [<<]
			const prevprevButton = fixture.debugElement.query(By.css('.pg-btn-prev-prev'));
			expect(prevprevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<<]');
			expect(prevprevButton.classes['disabled']).toBeTruthy('Кнопка предыдущей страницы [<] должна быть не доступна');

			// кнопка следующей страницы [>]
			const nextButton = fixture.debugElement.query(By.css('.pg-btn-next'));
			expect(nextButton).toBeTruthy('Не найдена кнопка следующей страницы [>]');
			expect(nextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>] должна быть доступна');

			// кнопка следующей страницы [>>]
			const nextNextButton = fixture.debugElement.query(By.css('.pg-btn-next-next'));
			expect(nextNextButton).toBeTruthy('Не найдена кнопка следующей страницы [>>]');
			expect(nextNextButton.classes['disabled']).toBeTruthy('Кнопка следующей страницы [>>] должна быть не доступна');

			// клик на кнопке [2]
			click(items[1].query(By.css('a')));
			// опять запускаем механизм обработки изменений
			fixture.detectChanges();
			//  дожидаемся окончания обработки всех изменений
			fixture.whenStable().then(() => {

				// еще раз запустим
				fixture.detectChanges();
				// проверяем кнопки

				// проверяем переменную, куда должно было установиться номер текуще страницы
				expect(fixture.componentInstance.selectedButton).toBe(1, 'не отработало событие смены текущей страницы');
				comp.currentPage = 1;
				fixture.detectChanges();

				// всего кнопок страниц должно быть 4
				const items = fixture.debugElement.queryAll(By.css('.pg-btn-page'));
				expect(items.length).toEqual(4, 'Всего кнопок страниц должно быть 4');

				// кнопка текущей страницы
				const selectedButton = fixture.debugElement.query(By.css('.pg-btn-page-current'));
				expect(selectedButton).toBeTruthy('Не найдена кнопка текущей страницы');
				expect(selectedButton.nativeElement.textContent.trim()).toEqual('2', 'Не соответствие номера на кнопке текущей страницы [2]');

				// кнопка предыдущей страницы [<]
				const prevButton = fixture.debugElement.query(By.css('.pg-btn-prev'));
				expect(prevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<]');
				expect(prevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<] должна быть доступна');

				// кнопка предыдущей страницы [<<]
				const prevprevButton = fixture.debugElement.query(By.css('.pg-btn-prev-prev'));
				expect(prevprevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<<]');
				expect(prevprevButton.classes['disabled']).toBeTruthy('Кнопка предыдущей страницы [<] должна быть не доступна');

				// кнопка следующей страницы [>]
				const nextButton = fixture.debugElement.query(By.css('.pg-btn-next'));
				expect(nextButton).toBeTruthy('Не найдена кнопка следующей страницы [>]');
				expect(nextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>] должна быть доступна');

				// кнопка следующей страницы [>>]
				const nextNextButton = fixture.debugElement.query(By.css('.pg-btn-next-next'));
				expect(nextNextButton).toBeTruthy('Не найдена кнопка следующей страницы [>>]');
				expect(nextNextButton.classes['disabled']).toBeTruthy('Кнопка следующей страницы [>>] должна быть не доступна');

			});

		});
	}));

	it('Кнопок 5, страниц 20, текущая 3', async(() => {
		const fixture = TestBed.createComponent(HostComponent);
		const comp = fixture.componentInstance;
		comp.countButton = 5;
		comp.countPage = 20;
		comp.currentPage = 2; // 0 based
		fixture.detectChanges();
		if (fixture.isStable()) {
			fixture.detectChanges();

			// всего кнопок страниц должно быть 5
			const items = fixture.debugElement.queryAll(By.css('.pg-btn-page'));
			expect(items.length).toEqual(5, 'Всего кнопок страниц должно быть 5');

			// кнопка текущей страницы
			const selectedButton = fixture.debugElement.query(By.css('.pg-btn-page-current'));
			expect(selectedButton).toBeTruthy('Не найдена кнопка текущей страницы');
			expect(selectedButton.nativeElement.textContent.trim()).toEqual('3', 'Не соответствие номера на кнопке текущей страницы [3]');

			// кнопка предыдущей страницы [<]
			const prevButton = fixture.debugElement.query(By.css('.pg-btn-prev'));
			expect(prevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<]');
			expect(prevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<] должна быть доступна');

			// кнопка предыдущей страницы [<<]
			const prevprevButton = fixture.debugElement.query(By.css('.pg-btn-prev-prev'));
			expect(prevprevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<<]');
			expect(prevprevButton.classes['disabled']).toBeTruthy('Кнопка предыдущей страницы [<] должна быть не доступна');

			// кнопка следующей страницы [>]
			const nextButton = fixture.debugElement.query(By.css('.pg-btn-next'));
			expect(nextButton).toBeTruthy('Не найдена кнопка следующей страницы [>]');
			expect(nextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>] должна быть доступна');

			// кнопка следующей страницы [>>]
			const nextNextButton = fixture.debugElement.query(By.css('.pg-btn-next-next'));
			expect(nextNextButton).toBeTruthy('Не найдена кнопка следующей страницы [>>]');
			expect(nextNextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>>] должна быть доступна');
		}
	}));

	it('Кнопок 5, страниц 20, текущая 8', async(() => {
		const fixture = TestBed.createComponent(HostComponent);
		const comp = fixture.componentInstance;
		comp.countButton = 5;
		comp.countPage = 20;
		comp.currentPage = 7; // 0 based
		fixture.detectChanges();
		if (fixture.isStable()) {
			fixture.detectChanges();

			// всего кнопок страниц должно быть 5
			const items = fixture.debugElement.queryAll(By.css('.pg-btn-page'));
			expect(items.length).toEqual(5, 'Всего кнопок страниц должно быть 5');

			// кнопка текущей страницы. должна быть с номером 8
			const selectedButton = fixture.debugElement.query(By.css('.pg-btn-page-current'));
			expect(selectedButton).toBeTruthy('Не найдена кнопка текущей страницы');
			expect(selectedButton.nativeElement.textContent.trim()).toEqual('8', 'Не соответствие номера на кнопке текущей страницы [3]');

			// кнопка предыдущей страницы [<]
			const prevButton = fixture.debugElement.query(By.css('.pg-btn-prev'));
			expect(prevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<]');
			expect(prevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<] должна быть доступна');

			// кнопка предыдущей страницы [<<]
			const prevprevButton = fixture.debugElement.query(By.css('.pg-btn-prev-prev'));
			expect(prevprevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<<]');
			expect(prevprevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<<] должна быть доступна');

			// кнопка следующей страницы [>]
			const nextButton = fixture.debugElement.query(By.css('.pg-btn-next'));
			expect(nextButton).toBeTruthy('Не найдена кнопка следующей страницы [>]');
			expect(nextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>] должна быть доступна');

			// кнопка следующей страницы [>>]
			const nextNextButton = fixture.debugElement.query(By.css('.pg-btn-next-next'));
			expect(nextNextButton).toBeTruthy('Не найдена кнопка следующей страницы [>>]');
			expect(nextNextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>>] должна быть доступна');
		}
	}));

	it('Кнопок 5, страниц 20, текущая 19', async(() => {
		const fixture = TestBed.createComponent(HostComponent);
		const comp = fixture.componentInstance;
		comp.countButton = 5;
		comp.countPage = 20;
		comp.currentPage = 18; // 0 based
		fixture.detectChanges();
		if (fixture.isStable()) {
			fixture.detectChanges();

			// всего кнопок страниц должно быть 5
			const items = fixture.debugElement.queryAll(By.css('.pg-btn-page'));
			expect(items.length).toEqual(5, 'Всего кнопок страниц должно быть 5');

			// кнопка текущей страницы. должна быть с номером 8
			const selectedButton = fixture.debugElement.query(By.css('.pg-btn-page-current'));
			expect(selectedButton).toBeTruthy('Не найдена кнопка текущей страницы');
			expect(selectedButton.nativeElement.textContent.trim()).toEqual('19', 'Не соответствие номера на кнопке текущей страницы [3]');

			// кнопка предыдущей страницы [<]
			const prevButton = fixture.debugElement.query(By.css('.pg-btn-prev'));
			expect(prevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<]');
			expect(prevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<] должна быть доступна');

			// кнопка предыдущей страницы [<<]
			const prevprevButton = fixture.debugElement.query(By.css('.pg-btn-prev-prev'));
			expect(prevprevButton).toBeTruthy('Не найдена кнопка предыдущей страницы [<<]');
			expect(prevprevButton.classes['disabled']).toBeFalsy('Кнопка предыдущей страницы [<<] должна быть доступна');

			// кнопка следующей страницы [>]
			const nextButton = fixture.debugElement.query(By.css('.pg-btn-next'));
			expect(nextButton).toBeTruthy('Не найдена кнопка следующей страницы [>]');
			expect(nextButton.classes['disabled']).toBeFalsy('Кнопка следующей страницы [>] должна быть доступна');

			// кнопка следующей страницы [>>]
			const nextNextButton = fixture.debugElement.query(By.css('.pg-btn-next-next'));
			expect(nextNextButton).toBeTruthy('Не найдена кнопка следующей страницы [>>]');
			expect(nextNextButton.classes['disabled']).toBeTruthy('Кнопка следующей страницы [>>] должна быть не доступна');
		}
	}));

});


@Component({
	selector: 'app-host',
	template:
	`<div class="col-sm-6">
        <paging [countPage]="countPage" [currentPage]="currentPage" [countButton]="countButton" (onChangePage)="onValueChange($event)"></paging>
    </div>`
})
export class HostComponent {

	countPage: number;
	currentPage: number;
	countButton: number;

	selectedButton: number;

	//valueChange: EventEmitter<number> = new EventEmitter<number>();

	onValueChange(value: number) {
		this.selectedButton = value;
	}
}

// помощники иммитации клика на кнопке
export const ButtonClickEvents = {
	left: { button: 0 },
	right: { button: 2 }
};
export function click(el: DebugElement | HTMLElement, eventObj: any = ButtonClickEvents.left): void {
	if (el instanceof HTMLElement) {
		el.click();
	} else {
		el.triggerEventHandler('click', eventObj);
	}
}

