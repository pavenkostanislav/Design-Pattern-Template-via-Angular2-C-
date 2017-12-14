import { Component, Input, OnChanges, SimpleChange, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { UserTaskService } from '../../services/usertask.service';
import { MetaConstantService } from '../../services/metaconstant.service';
import { App } from '../../app.component';
import { Subject } from 'rxjs/Rx';

@Component({
    selector: 'usertask-list',
    templateUrl: 'usertask.html',
    providers: [App, UserTaskService, MetaConstantService, Router]
})
export class UserTaskComponent implements OnChanges, OnInit, OnDestroy {
    //MetaConst
    docTypeIdUserTask: number = 0;
    docTypeIdSupport: number = 0;
    importanceLow: any = 0;
    importanceHigh: any = 0;
    //MetaConst

    logShow: boolean = window.location.hostname === 'localhost';

    @Input()
    visible: boolean = true;
    @Input()
    docId: number = 0;
    @Input()
    parentId: number = 0;
    @Input()
    metaObjectId: number = 0;
    @Input()
    objectId: number = 0;
    @Input()
    docTypeId: number = 0;
    @Input()
    canRead: boolean = true;

    selected: any = null;
    freeze: boolean = false;
    usertaskList: any[] = [];

    private ngUnsubscribe = new Subject<void>();

    constructor(
        private app: App,
        private usertaskSrv: UserTaskService,
        private constSrv: MetaConstantService) {

        if (this.logShow) {
            console.log('UserTaskComponent.constructor');
        }
    }

    ngOnInit() {

        if (this.logShow) {
            console.log('UserTaskComponent.ngOnInit');
        }

        this.constSrv.getMetaConstantId('DocumentTypes.UserTask')
            .takeUntil(this.ngUnsubscribe)
            .subscribe(
                (res: any) => {
                    this.docTypeIdUserTask = res;
                },
                (err: any) => {
                    this.app.showError(err);
                }
            );
        this.constSrv.getMetaConstantId('DocumentTypes.Support')
            .takeUntil(this.ngUnsubscribe)
            .subscribe(
                (res: any) => {
                    this.docTypeIdSupport = res;
                },
                (err: any) => {
                    this.app.showError(err);
                }
            );
        this.constSrv.getMetaConstantId('Importance.Low')
            .takeUntil(this.ngUnsubscribe)
            .subscribe(
                (res: any) => {
                    this.importanceLow = res;
                },
                (err: any) => {
                    this.app.showError(err);
                }
            );
        this.constSrv.getMetaConstantId('Importance.High')
            .takeUntil(this.ngUnsubscribe)
            .subscribe(
                (res: any) => {
                    this.importanceHigh = res;
                },
                (err: any) => {
                    this.app.showError(err);
                }
            );
    }
    ngOnChanges(changes: { [key: string]: SimpleChange; }): any {

        if (this.logShow) {
            console.log('UserTaskComponent.ngOnChanges key: {key}');
        }

        if (this.docId > 0 && this.metaObjectId > 0 && this.objectId > 0) {
            this.clearSelected();
            if (this.visible) {
                this.onRefreshList();
            }
        }
    }

    ngOnDestroy() {

        if (this.logShow) {
            console.log('UserTaskComponent.ngOnDestroy');
        }

        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();

    }

    clearSelected() {
        this.selected = null;
    }

    onSelected(row: any) {
        this.selected = row;
    }

    onRefreshList() {
        if (!this.canRead) {
            this.usertaskList = [];
            return;
        }
        this.freeze = true;
        this.usertaskSrv.getListByMOO(this.docId, this.metaObjectId, this.objectId)
            .takeUntil(this.ngUnsubscribe)
            .subscribe(
                (res: any) => {
                    this.freeze = false;
                    this.usertaskList = (res);
                },
                (err: any) => {
                    this.freeze = false;
                    this.app.showError(err);
                }
            );
    }
}