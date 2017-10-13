import { Component, OnChanges, Input, ViewChild, DebugElement, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';
import { By } from '@angular/platform-browser';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { } from 'jasmine';

import { DropDown } from './dropdown.control';
import { SelectService, SelectItem, SelectGroup } from '../../services/select.service';
import { App } from '../../app.component';


@Component({
    selector: 'app-dd-host',
    template:
    `<div class="col-sm-6">
        <dropdown 
            [items]="items" 
            [itemType]="itemType"
            [minTerm]="minTerm"
            [disabled]="disabled"
            [required]="required"
            [allowClear]="allowClear" 
            [parentId]="parentId" 
            [term]="term" 
            [showGroups]="useGroups" 
            [(ngModel)]="selectedValue" 
            [autocomplete]="autocomplete"
            (value)="onValueChange($event)"
            class="form-control"></dropdown>
    </div>`,
    styleUrls: ['../../../css/bootstrap.css']
})
export class HostComponent {

    items: SelectItem[];
    itemType: string;
    minTerm: number;
    disabled: boolean;
    required: boolean;

    allowClear: boolean;
    parentId: number;
    term: string;
    useGroups: boolean;
    autocomplete: boolean;
    valueChange: EventEmitter<number> = new EventEmitter<number>();

    selectedValue: number;

    onValueChange(value: number) {
        this.valueChange.emit(value);
    }
}

const selectServiceStub = {
    list: [
        new SelectItem(1, 'Первый', 1, { id: 2, text: '<i>Числа</i>', sort: 1 }),

        { id: 6, text: 'Шестой', sort: 6, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 2, text: 'Второй', sort: 2, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 3, text: 'Третий', sort: 3, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 4, text: 'Четвертый', sort: 4, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 5, text: 'Пятый', sort: 5, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 7, text: 'Седьмой', sort: 7, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 8, text: 'Восьмой', sort: 8, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 9, text: 'Девятый', sort: 9, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 10, text: 'Десятый', sort: 10, group: { id: 2, text: '<i>Числа</i>', sort: 1 } },
        { id: 21, text: 'Нога' },
        { id: 22, text: 'Голова' },
        { id: 23, text: 'Палец' },
        { id: 24, text: 'Нос' },
        { id: 25, text: 'Ухо' },
        { id: 26, text: 'Рука' },
        { id: 27, text: 'Спина' },
        { id: 28, text: 'Щека' },
        { id: 29, text: 'Колено' },
        { id: 30, text: 'Лоб' },
        { id: 11, text: 'Дом', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 12, text: 'Улица', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 13, text: 'Фонарь', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 14, text: 'Аптека', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 15, text: 'Ночь', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 16, text: 'Луна', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 17, text: 'Лужа', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 18, text: 'Автомобиль', sort: 1, group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 19, text: 'Мотоцикл', group: { id: 1, text: 'Слова', sort: 2 } },
        { id: 20, text: 'Фрегат', group: { id: 1, text: 'Слова', sort: 2 } },
    ],

    list1: [
        new SelectItem(100, 'camera', 1, { id: 10, text: '<i>фото</i>', sort: 1 }),
    ],

    getSelectList(itemType: string, parentId: number, term: string): Observable<SelectItem[]> {
        let res: Observable<SelectItem[]>;
        if (itemType === 'one') {
            res = Observable.of(this.list1.filter((m: SelectItem) => m.text.toLowerCase().includes(term.toLowerCase())));
        } else {
            res = Observable.of(this.list.filter((m: SelectItem) => m.text.toLowerCase().includes(term.toLowerCase())));
        }
        return res;
    },

    getSelectItemId(itemType: string, id: number): Observable<SelectItem> {
        let res: Observable<SelectItem>;
        if (itemType === 'one') {
            res = Observable.of(this.list1.find((m: SelectItem) => m.id === id));
        }
        else {
            res = Observable.of(this.list.find((m: SelectItem) => m.id === id));
        }
        return res;
    }
};

const appMock = {
	UpdateApp: new EventEmitter<any>()
};

describe('dropdown.control', () => {

    const placeholder = 'Значение не выбрано...';

    beforeEach(async(() => {

        TestBed.configureTestingModule({
            declarations: [
                HostComponent,
                DropDown
            ],
            imports: [
                FormsModule,
                ReactiveFormsModule
            ],
            providers: [
				{ provide: SelectService, useValue: selectServiceStub },
				{ provide: App, useValue: appMock }
            ]
        }).compileComponents();

    }));

    it('проверка создания host компонента', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        expect(fixture.componentInstance).toBeTruthy();
    }));

    it('проверка создания компонента DropDown', async(() => {
        const fixture = TestBed.createComponent(DropDown);
        expect(fixture.componentInstance).toBeTruthy();
    }));

    it('проверка ошибки при установленных itemType и items', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        try {
            hostComponent.itemType = 'ItemTypes';
            hostComponent.items = [{ id: 99, text: '99', group: { id: 33, text: 'no' } }];
            fixture.detectChanges();
        } catch (error) {
            expect(error.message).toContain('Нельзя устанавливать одновременно параметр items и itemType!');
        }
    }));

    it('проверка ошибки при установленных minTerm и items', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        try {
            hostComponent.items = [{ id: 99, text: '99', group: { id: 33, text: 'no' } }];
            hostComponent.minTerm = 3;
            fixture.detectChanges();
        } catch (error) {
            expect(error.message).toContain('Параметр minTerm используется только совместно с параметром itemType!');
        }
    }));

    it('проверка надписи Значение не выбрано...', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.itemType = 'ItemTypes';
        fixture.detectChanges();
        const text = fixture.debugElement.query(By.css('.dd-text'));
        expect(text.nativeElement.textContent).toEqual(placeholder, 'Не соответствие надписи [Значение не выбрано...]');
    }));

    it('проверка наличия кнопки [v]', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        const btnSelector = '.dd-btn.btn-show';

        hostComponent.itemType = 'ItemTypes';
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css(btnSelector));
        expect(btn).toBeTruthy('Не найдена кнопка [v].');
    }));

    it('проверка отсутсвия кнопки [v] при disabled = true', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        const btnSelector = '.dd-btn.btn-show';

        hostComponent.itemType = 'ItemTypes';
        hostComponent.disabled = true;
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css(btnSelector));
        expect(btn).toBeFalsy('Найдена кнопка [v], её не должно быть.');

    }));

    it('проверка отсутсвия кнопки [x] при пустом значении', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        const btnSelector = '.dd-btn.btn-clear';

        hostComponent.itemType = 'ItemTypes';
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css(btnSelector));
        expect(btn).toBeFalsy('Найдена кнопка [х], её не должно быть.');
    }));

    it('проверка отсутсвия кнопки [x] при выбранном значении и allowClear = false', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        const btnSelector = '.dd-btn.btn-clear';

        hostComponent.itemType = 'ItemTypes';
        hostComponent.selectedValue = 4;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btn = fixture.debugElement.query(By.css(btnSelector));
            expect(btn).toBeFalsy('Найдена кнопка [х], её не должно быть.');
        });
    }));

    it('проверка наличия кнопки [x] при выбранном значении и allowClear = true', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        const btnSelector = '.dd-btn.btn-clear';

        hostComponent.itemType = 'ItemTypes';
        hostComponent.selectedValue = 4;
        hostComponent.allowClear = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btn = fixture.debugElement.query(By.css(btnSelector));
            expect(btn).toBeTruthy('Не найдена кнопка [х]');
        });
    }));

    it('проверка itemType & selectedValue = 4', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.itemType = 'ItemTypes';
        hostComponent.selectedValue = 4;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual('Четвертый', 'Неправильный текст выбранного значения');
        });
    }));

    it('проверка items & selectedValue = 8', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        hostComponent.items = [
            { id: 99, text: '99', group: { id: 33, text: 'no' } },
            { id: 88, text: '88', group: { id: 33, text: 'no' } },
            { id: 77, text: '77', group: { id: 33, text: 'no' } },
            { id: 8, text: 'Восьмой', group: { id: 33, text: 'no' } },
        ];
        hostComponent.selectedValue = 8;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual('Восьмой', 'Неправильный текст выбранного значения');
        });
    }));

    it('проверка ошибки "В списке значений items не найдено значение"', (() => {

        const fixture = TestBed.createComponent(DropDown);
        const component = fixture.componentInstance;
        component.items = [{ id: 99, text: '99', group: { id: 33, text: 'no' } }];
        // expect(component.requestValue(44)).toThrowError(Error);
        try {
            component.requestValue(44);
            fail('Не сработала ошибка "В списке значений items не найдено значение"');
        } catch (error) {
            expect(error.message).toContain('В списке значений items не найдено значение', 'Неверный текст ошибки');
            expect(component.selectedItem).toBeFalsy('Свойство selectedItem должно быть null! Оно = ' + component.selectedItem);
            expect(component.innerValue).toBeFalsy('Свойство innerValue должно быть null! Оно = ' + component.innerValue);
        }
    }));

    it('проверка ошибки "В списке значений itemType не найдено значение"', (() => {

        const fixture = TestBed.createComponent(DropDown);
        const component = fixture.componentInstance;
        component.itemType = 'ItemTypes';

        try {
            component.requestValue(44);
            fail('Не сработала ошибка "В списке значений itemType не найдено значение"');
        } catch (error) {
            expect(error.message).toContain('В списке значений itemType не найдено значение', 'Неверный текст ошибки');
            expect(component.selectedItem).toBeFalsy('Свойство selectedItem должно быть null! Оно = ' + component.selectedItem);
            expect(component.innerValue).toBeFalsy('Свойство innerValue должно быть null! Оно = ' + component.innerValue);
        }
    }));

    it('проверка надписи после клика по кнопке [x]', async(() => {

        const fixture = TestBed.createComponent(DropDown);
        const component = fixture.componentInstance;
        component.itemType = 'ItemTypes';
        component.allowClear = true;
        component.requestValue(8);
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btnClear = fixture.debugElement.query(By.css('.dd-btn.btn-clear'));
            click(btnClear);
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual(placeholder, 'Не найдена надпись: ' + placeholder);
        });
    }));

    it('проверка привязанного через ngModel значения после клика по кнопке [x]', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const component = fixture.componentInstance;
        component.itemType = 'ItemTypes';
        component.allowClear = true;
        component.selectedValue = 8;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btnClear = fixture.debugElement.query(By.css('.dd-btn.btn-clear'));
            click(btnClear);
            fixture.detectChanges();
            fixture.whenStable().then(() => {
                expect(component.selectedValue).toBeFalsy('Значение, привязанное через [(ngModel)] не очистилось!');
            });
        });
    }));

    it('проверка открытия окна списка', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.itemType = 'ItemTypes';
        hostComponent.allowClear = true;
        hostComponent.selectedValue = 8;
        hostComponent.minTerm = 0;
        hostComponent.useGroups = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btnShow = fixture.debugElement.query(By.css('.dd-btn.btn-show'));
            click(btnShow);
            fixture.detectChanges();
            fixture.whenStable().then(() => {
                const form = fixture.debugElement.query(By.css('.dd-findform'));
                expect(form).toBeTruthy('Не нашли блок .dd-findform');

                const input = fixture.debugElement.query(By.css('input'));
                expect(input).toBeTruthy('Не нашли поле ввода строки поиска input');

                const groups = fixture.debugElement.queryAll(By.css('.dd-group'));
                expect(groups.length).toEqual(3, 'Неверное кол-во групп в списке');

                const items = fixture.debugElement.queryAll(By.css('.dd-item'));
                expect(items.length).toEqual(30, 'Неверное кол-во элементов в списке');
            });
        });

    }));

    it('проверка выбора значения из списка', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.itemType = 'ItemTypes';
        hostComponent.allowClear = true;
        hostComponent.selectedValue = 4;
        hostComponent.minTerm = 0;
        hostComponent.useGroups = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btnShow = fixture.debugElement.query(By.css('.dd-btn.btn-show'));
            click(btnShow);
            fixture.detectChanges();
            fixture.whenStable().then(() => {
                const items = fixture.debugElement.queryAll(By.css('.dd-item'));
                const item = items.find(f => f.nativeElement.textContent === 'Восьмой');
                expect(item).toBeTruthy('Не нашли элемент с текстом "Восьмой"');
                click(item);
                fixture.detectChanges();
                expect(hostComponent.selectedValue).toEqual(8, 'Не правильное значение в поле selectedValue');
            });
        });
    }));

    it('проверка отсутствия автоподстановки по количеству элементов', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        hostComponent.minTerm = 0;
        hostComponent.autocomplete = true;
        hostComponent.itemType = 'ItemTypes';
        hostComponent.allowClear = true;
        fixture.detectChanges();
        const text = fixture.debugElement.query(By.css('.dd-text'));
        expect(text.nativeElement.textContent).toEqual(placeholder, 'выбрано что-то ошибочно, в коллекции > 1 элемента');
    }));

    it('проверка отсутствия автоподстановки по minTerm', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.autocomplete = true;
        hostComponent.itemType = 'one';
        hostComponent.allowClear = true;
        fixture.detectChanges();
        const text = fixture.debugElement.query(By.css('.dd-text'));
        expect(text.nativeElement.textContent).toEqual(placeholder, 'выбрано что-то ошибочно, при возможности поиска не требуется автоподстановка');
    }));

    it('проверка отсутствия автоподстановки по items.length>1', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.autocomplete = true;
        hostComponent.items = [{ id: 100, text: 'camera' },
        { id: 101, text: 'camerop' }];
        hostComponent.allowClear = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual(placeholder, 'значение выбрано не верно');
        });
    }));

    it('проверка автоподстановки по items.length=1', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;

        hostComponent.autocomplete = true;
        hostComponent.items = [{ id: 100, text: 'camera' }];
        hostComponent.allowClear = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual('camera', 'значение выбрано не верно');
        });
    }));

    it('проверка автоподстановки по itemType', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const hostComponent = fixture.componentInstance;
        hostComponent.minTerm = 0;
        hostComponent.autocomplete = true;
        hostComponent.itemType = 'one';
        hostComponent.allowClear = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const text = fixture.debugElement.query(By.css('.dd-text'));
            expect(text.nativeElement.textContent).toEqual('camera', 'значение выбрано не верно');
        });
    }));

});


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
