import { By } from '@angular/platform-browser';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Component, EventEmitter, DebugElement, SimpleChange } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Observable } from 'rxjs/Rx';

import { MetaConstantService } from '../../services/metaconstant.service';
import { UserTaskService, UserTaskHeaderViewModel } from '../../services/usertask.service';
import { UserTaskComponent } from './usertask.component';
import { App } from '../../app.component';

@Component({
	selector: 'app-host',
	template: `<usertask-list [docId]="docId"
	                [metaObjectId]="metaObjectId"
	                [objectId]="objectId"
	                [docTypeId]="docTypeId"
	                [canRead]="canRead">
	            </usertask-list>`,
    styleUrls: ['../../../css/bootstrap.css']
})
export class HostComponent {

    docId: number;
    metaObjectId: number;
    objectId: number;
    docTypeId: number;
    canRead = true;
}

const userTaskServiceStub = {
    usertaskList: [
    {
        'id': 1,
        'date': '12.01.2017 11:28:52',
        'name': 'Поручение Тест 1',
        'description': 'Поручение Тест 1',
        'authorName': 'Автор',
        'performerName': 'Исполнитель',
        'statusName': 'Черновик',
        'importanceId': 632,
        'documentTypeId': 3,
        'metaObjectId': 400,
        'objectId': 15
    },
    {
        'id': 2,
        'date': '12.01.2017 11:28:52',
        'name': 'Обращение Тест 2',
        'description': 'Обращение Тест 2',
        'authorName': 'Автор',
        'performerName': 'Исполнитель',
        'statusName': 'Черновик',
        'importanceId': 632,
        'documentTypeId': 7,
        'metaObjectId': 400,
        'objectId': 15
    },
    {
        'id': 3,
        'date': '12.01.2017 11:28:52',
        'name': 'Поручение Тест 3',
        'description': 'Поручение Тест 3',
        'authorName': 'Автор',
        'performerName': 'Исполнитель',
        'statusName': 'Черновик',
        'importanceId': 632,
        'documentTypeId': 3,
        'metaObjectId': 400,
        'objectId': 25
    }],
    getListByMOO(docId: number, metaObjectId: number, objectId: number): Observable<UserTaskHeaderViewModel> {
        let res: Observable<UserTaskHeaderViewModel>;
        res = Observable.of(this.usertaskList.filter((m: any) => m.documentTypeId === docId && m.metaObjectId === metaObjectId && m.objectId === objectId));
        return res;
    }
};

const appMock = {
    UpdateApp: new EventEmitter<any>()
};

const metaConstantServiceMock = {
    getMetaConstantId(value: string): Observable<number> {
        switch (value) {
        case 'DocumentTypes.UserTask':
            return Observable.of(3);
        case 'DocumentTypes.Support':
            return Observable.of(7);
        case 'Importance.Low':
            return Observable.of(631);
        case 'Importance.High':
            return Observable.of(633);
        }
        return Observable.of(null);
    }
};

describe('usertask.component', () => {

	beforeEach(async(() => {

        TestBed
            .configureTestingModule({
                imports: [
                    HttpModule
                ],
                declarations: [
                    HostComponent,
                    UserTaskComponent
                ]
		    })
            .overrideComponent(UserTaskComponent, {
                set: {
                    providers: [
                        { provide: UserTaskService, useValue: userTaskServiceStub },
                        { provide: MetaConstantService, useValue: metaConstantServiceMock },
                        { provide: App, useValue: appMock }
                    ]
                }
            })
            .compileComponents();
    }));


    it('should create the UserTaskComponent', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance).toBeTruthy();
    }));

    it(`UserTaskComponent should have as parentId`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.parentId).toEqual(0);
    }));

    it(`UserTaskComponent should have as visible`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.visible).toEqual(true);
    }));

    it(`UserTaskComponent should have as canRead`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.canRead).toBeTruthy();
    }));

    it(`UserTaskComponent should have as docId`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.docId).toEqual(0);
    }));

    it(`UserTaskComponent should have as metaObjectId`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.metaObjectId).toEqual(0);
    }));

    it(`UserTaskComponent should have as objectId`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.objectId).toEqual(0);
    }));

    it(`UserTaskComponent should have as docTypeId`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.docTypeId).toEqual(0);
    }));

    it(`UserTaskComponent should have as docTypeIdUserTask`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.docTypeIdUserTask).toEqual(0);
    }));

    it(`UserTaskComponent should have as docTypeIdSupport`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.docTypeIdSupport).toEqual(0);
    }));

    it(`UserTaskComponent should have as importanceLow`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.importanceLow).toEqual(0);
    }));

    it(`UserTaskComponent should have as importanceHigh`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.importanceHigh).toEqual(0);
    }));

    it(`UserTaskComponent should have as selected`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.selected).toBeNull();
    }));

    it(`UserTaskComponent should have as freeze`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.freeze).toBeFalsy();
    }));

    it(`UserTaskComponent should have as usertaskList`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.usertaskList).toBeDefined();
    }));

    it(`UserTaskComponent should have as ngUnsubscribe`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        expect(fixture.debugElement.componentInstance.ngUnsubscribe).toBeDefined();
    }));

    it('UserTaskComponent should have tag div with classes: .panel.panel-primary', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        const compiled = fixture.debugElement.nativeElement;
        expect(compiled.querySelector('div.panel.panel-primary')).toBeDefined();
    }));

    it('UserTaskComponent should have as visible', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.detectChanges();
        expect(fixture.componentInstance.visible).toBe(true);
    }));


    it(`UserTaskComponent should init as docTypeIdUserTask`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.docTypeIdUserTask).toEqual(3);
    }));

    it(`UserTaskComponent should init as docTypeIdSupport`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.docTypeIdSupport).toEqual(7);
    }));

    it(`UserTaskComponent should init as importanceLow`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.importanceLow).toEqual(631);
    }));

    it(`UserTaskComponent should init as importanceHigh`, async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.importanceHigh).toEqual(633);
    }));

    it('UserTaskComponent have as selected is null after clearSelected', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.debugElement.componentInstance.selected = 'Test';
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.selected).toContain('Test');
        fixture.componentInstance.clearSelected();
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.selected).toBeNull();
    }));

    it('UserTaskComponent have as selected is null after onSelected', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.componentInstance.onSelected('Test');
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.selected).toContain('Test');
    }));

    it('UserTaskComponent have as usertaskList.length = 0 after onRefreshList', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.debugElement.componentInstance.usertaskList.push('Test');
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.usertaskList.length).toBe(1);
        fixture.componentInstance.canRead = false;
        fixture.componentInstance.onRefreshList();
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.usertaskList.length).toBeDefined();
        expect(fixture.debugElement.componentInstance.usertaskList.length).toBe(0);
    }));

    it('UserTaskComponent have as selected is null after detectChanges with params', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.debugElement.componentInstance.selected = 'Test';
        fixture.detectChanges();
        fixture.componentInstance.docId = 3;
        fixture.componentInstance.metaObjectId = 400;
        fixture.componentInstance.objectId = 15;
        fixture.componentInstance.visible = true;
        fixture.componentInstance.ngOnChanges({
            name: new SimpleChange(0, 3, false)
        });
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.selected).toBeNull();
    }));

    it('UserTaskComponent have as usertaskList.length > 0 after detectChanges with params', async(() => {
        const fixture = TestBed.createComponent(UserTaskComponent);
        fixture.componentInstance.docId = 3;
        fixture.componentInstance.metaObjectId = 400;
        fixture.componentInstance.objectId = 15;
        fixture.componentInstance.onRefreshList();
        fixture.detectChanges();
        expect(fixture.debugElement.componentInstance.usertaskList.length).toBeGreaterThan(0);
    }));

    it('Проверка создания HostComponent', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        comp.docTypeId = 3;
        comp.canRead = true;
        fixture.detectChanges();
        expect(fixture.componentInstance).toBeTruthy();
    }));

    it('Проверка наличия кнопки [Обновить список]', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        comp.docTypeId = 3;
        comp.canRead = true;
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css('table thead tr th.text-center a.btn.btn-xs.btn-primary[title="Обновить список"]'));
        expect(btn).toBeTruthy('Не найдена кнопка [Обновить список].');
    }));

    it('Проверка наличия кнопки [Добавить запись]', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        comp.docTypeId = 3;
        comp.canRead = true;
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css('table thead tr th.text-center a.btn.btn-xs.btn-primary[title="Добавить запись"]'));
        expect(btn).toBeTruthy('Не найдена кнопка [Добавить запись].');
    }));

    it('Проверка наличия таблицы в рамке', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        comp.docTypeId = 3;
        comp.canRead = true;
        fixture.detectChanges();
        const btn = fixture.debugElement.query(By.css('div.panel.panel-primary table.table.table-bordered.table-hover.table-striped.table-condensed thead tr th.text-center'));
        expect(btn).toBeTruthy('Не найдена кнопка таблица.');
    }));

    it('Проверка формирования строк таблицы', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        comp.docTypeId = 3;
        comp.canRead = true;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const de = fixture.debugElement.queryAll(By.css('table tbody tr'));
            expect(de.length).toEqual(1, 'Более одной строки "docId=3"' + 1);
        });
    }));

    it('Проверка формирования колонок таблицы поручений', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const countCol = 10;
            const th = fixture.debugElement.queryAll(By.css('table.table.table-bordered.table-hover.table-striped.table-condensed thead tr th'));
            expect(th.length).toBe(countCol);
        });
    }));

    it('Проверка формирования колонок таблицы поддержки', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 7;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const countCol = 15;
            const th = fixture.debugElement.queryAll(By.css('table.table.table-bordered.table-hover.table-striped.table-condensed thead tr th'));
            expect(th.length).toBe(countCol);
        });
    }));

    it('Проверка формирования кнопок в строках', async(() => {
        const fixture = TestBed.createComponent(HostComponent);
        const comp = fixture.componentInstance;
        comp.docId = 3;
        comp.metaObjectId = 400;
        comp.objectId = 15;
        fixture.detectChanges();
        fixture.whenStable().then(() => {
            fixture.detectChanges();
            const btn = fixture.debugElement.queryAll(By.css('a.btn.btn-xs.btn-success[title="Открыть"]'));
            expect(btn.length).toEqual(1, 'Всего кнопок "docId=3&&metaObjectId=400&&objectId=15"  должно быть ' + 1);
            btn.forEach(item => {
                expect(item.properties['href']).toContain('/usertask/edit/');
            });
            expect(btn[0].properties['href']).toContain('/usertask/edit/1');

            comp.docId = 7;
            comp.metaObjectId = 400;
            comp.objectId = 15;
            fixture.detectChanges();
            fixture.whenStable().then(() => {
                const btn = fixture.debugElement.queryAll(By.css('a.btn.btn-xs.btn-success[title="Открыть"]'));
                btn.forEach(item => {
                    expect(item.properties['href']).toContain('/usertask/edit/');
                });
                expect(btn.length).toEqual(1, 'Всего кнопок "docId=7&&metaObjectId=400&&objectId=15"  должно быть ' + 1);
                expect(btn[0].properties['href']).toContain('/usertask/edit/2');
            });
        });
    }));
});

