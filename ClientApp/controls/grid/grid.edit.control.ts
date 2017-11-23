import { Component, OnInit, OnChanges, Input, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';

import { App, GridSize } from '../../app.component';
import { GridService, GridRequestModel, GridResponseModel } from '../../services/grid.service';
import { MetaConstantService } from '../../services/metaconstant.service';
import { UserService, UserViewModel, Permissions } from '../../services/user.service';


@Component({
    selector: 'grid-edit',
    templateUrl: 'grid.edit.html',
    providers: [MetaConstantService, GridService]
})
export class GridEditControl implements OnInit {

    @Input() controllerName: string;
    @Input() id: number = 0;
    @Input() listColumn: string[] = [];
    @Input() name: string;
    @Input() mode: string;
    @Input() documentTypeId: number;

    @Input() parentValue: number;
    @Input() parentName: string;

    @Input() canAdd: boolean = true;
    @Input() canCopy: boolean = false;
    @Input() canDelete: boolean = true;
    @Input() canEdit: boolean = true;
    @Input() canAttach: boolean = true;
    @Input() canReadInfo: boolean = true;

	isViewOnly: boolean = false;
    isNewOrCopy: boolean = false;

    model: any;
    @Output() modelChange = new EventEmitter<any>();

    tableId: number;
	private logShow = false;
    freeze = true;
	currentUser: UserViewModel = new UserViewModel();
    permissions: Permissions = new Permissions();
    gridSize: GridSize = GridSize.lg;

    constructor(private app: App,
        private router: Router,
        private location: Location,
        private constSrv: MetaConstantService,
        private gridSrv: GridService) { }

    ngOnInit() {
        this.logShow = (window.location.hostname == `localhost`);
        if (this.logShow) {
            console.log(`${this.controllerName}: GridEdit.ngOnInit ${this.controllerName}`);
        }
        this.currentUser = this.app.currentUser;
		this.permissions = this.app.permissions;
		this.app.GridSize.subscribe (
            (res: any) => { this.gridSize = res; }
        );
        if (this.controllerName) {
            this.constSrv.getMetaConstantId(`Table.${this.controllerName}`).subscribe(
                res => {
                    this.tableId = res;
                },
                err => {
                    this.app.showError(err);
                }
            );
        }
    }
    ngOnChanges(changes: SimpleChanges) {
        if (this.mode) {
            this.mode = this.mode.toLowerCase();
        }
        this.isViewOnly = (this.mode == 'viewonly');
        this.isNewOrCopy = (this.mode == 'copy') || (this.mode == 'new');

        this.doGetModel(this.id, this.mode);
    }

    onClickCancel(editForm: NgForm) {
        this.doCancel(editForm);
    }
    onClickRefresh() {
        this.doGetModel(this.id, this.mode);
    }
    onClickAddNew() {
		this.goPage(`/${this.controllerName.toLowerCase()}/edit/0/new`);
    }
    onClickAddCopy() {
		if (this.model.id) {
			this.goPage(`/${this.controllerName.toLowerCase()}/edit/${this.model.id}/copy`);
		}
    }
	onClickDelete() {
		this.doDelete();
	}
	onClickSave() {
		this.doSave();
	}
	onSubmit() {
		this.doSave();
	}
	onClickAttach() {
		this.doModalShow('AttachmentModalGridEdit');
	}
	onClickReadInfo() {
		this.doModalShow('InfoModalGridEdit');
	}

    doGetModel(id: number, mode: string) {
		this.setControllerName();
        this.freeze = true;
        this.gridSrv.getGridRowModel(id, mode).subscribe(
            (res: any) => {
                if (this.isNewOrCopy) {
                    this.listColumn.forEach((item: any) => {
                        if (item.defaultValue && (!res[item.key])) {
                            res[item.key] = item.defaultValue;
                        }
                    });
                }
                this.model = res;
                this.freeze = false;
            },
            (err: any) => {
                this.freeze = false;
                this.app.showError(err);
            }
        );
    }
	doDelete() {
        if (this.model && this.model.id) {
            this.app.showQuestion(`Удалить выбранную строку?`).subscribe(
                (res: any) => {
                    if (res) {
                        this.gridSrv.deleteGridRowModel(this.model.id).subscribe(
                            (res: any) => {
                                this.freeze = false;
                                this.goPage(`/${this.controllerName.toLowerCase()}`, true);
                            },
                            (err: any) => {
                                this.freeze = false;
                                this.app.showError(err);
                            }
                        );
                    }
                }
            );
        }
    }
    doSave(isBack: boolean = false) {
        this.freeze = true;
        if (this.isNewOrCopy && this.parentValue > 0 && this.parentName) {
            this.model[this.parentName] = this.parentValue;
        }
        this.gridSrv.saveGridRowModel(this.model).subscribe(
            (res: any) => {
				this.model = res;
                this.freeze = false;
				if (isBack) {
					this.location.back();
				}
                this.app.showMessage('info', 'Сохранение', 'Данные сохранены.');
                if (this.model.id) {
					this.goPage(`/${this.controllerName.toLowerCase()}/edit/${this.model.id}`, true);
				}
				else {
					this.doGetModel(this.id, this.mode);
				}
            },
            (err: any) => {
                this.freeze = false;
                this.app.showError(err);
            }
        );
    }
    doModalShow(name: string) {
		switch (name) {
			case 'AttachmentModalGridEdit':
				if (!this.id || !this.tableId) { return; }
				break;
			case 'InfoModalGridEdit':
				if (!this.id) { return; }
				break;
		}
		$(`#${name}`).modal('show');
    }
	doCancel(editForm: NgForm) {
        if (editForm.dirty && !editForm.submitted) {
            this.app.showQuestion('Данные изменены. Сохранить изменения?').subscribe(
                res => {
                    if (res) {
                        this.doSave();
                    }
                    else {
                        this.location.back();
                    }
                }
            );
        }
        else {
            this.location.back();
        }
        return false;
    }

	goPage(urlroot: string, replaceUrl: boolean = false) {
        if (!replaceUrl) {
            this.router.navigateByUrl('/' + urlroot);
        } else {
            this.router.navigateByUrl('/' + urlroot, { replaceUrl: true });
        }
    }

	isValid(): boolean {
		if (this.model && this.listColumn) {
			return !(this.listColumn.some((c: any) => c.isNotNull && this.model[c.key] == null));
		} else {
			return  true;
		}
    }
	setControllerName(){
		if (!this.gridSrv.controllerName) {
			this.gridSrv.controllerName = this.controllerName;
		}

		if (!this.gridSrv.controllerName) {
			return;
		}
	}
}