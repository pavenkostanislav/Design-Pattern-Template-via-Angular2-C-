import { Component, OnInit, OnChanges, Input, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';

import { App, GridSize } from '../../app.component';
import { GridService, GridRequestModel, GridResponseModel } from '../../services/grid.service';
import { UserService, UserViewModel, Permissions } from '../../services/user.service';


@Component({
    selector: 'grid',
    templateUrl: 'grid.html',
    providers: [GridService]
})
export class GridRowList implements OnInit, OnChanges {
    @Output() selected: EventEmitter<any> = new EventEmitter<any>();

    @Input() controllerName: string;
    @Input() listColumn: string[] = [];
    @Input() name: string;
    @Input() keyValue: number;
    @Input() keyName: string;
    @Input() urlEditPage: string;
    @Input() isViewOnly = false;

    @Input() canAdd: boolean = true;
    @Input() canRead: boolean = true;
    @Input() canDelete: boolean = true;
    @Input() canEdit: boolean = true;
    @Input() canCopy: boolean = false;
    @Input() canAttach: boolean = false;


    isShowSearch: boolean = false;
    pageSize = 15;
    private countButton = 5;
    private countPage = 10;
    requestModel: GridRequestModel = new GridRequestModel();
    responceModel: GridResponseModel = new GridResponseModel();

    test: string;

    private logShow = false;

    isAddNew: boolean = false;
    freeze = true;
    GridSize = GridSize;

    currentUser: UserViewModel = new UserViewModel();
    permissions: Permissions = new Permissions();

    listRow: any[] = [];
    attacment: any = { metaObjectId: 0, objectId: 0 };
    editRowModel: any;
    newRowModel: any;
    selectedRow: any;
    selectedIndex: number = 0;
    gridSize: GridSize = GridSize.lg;

    constructor(private app: App,
        private router: Router,
        private gridSrv: GridService) { }

    ngOnInit() {
        this.logShow = (window.location.hostname == `localhost`);
        if (this.logShow) {
            console.log(`GridEdit.ngOnInit`);
        }
        this.currentUser = this.app.currentUser;
        this.permissions = this.app.permissions;
		this.app.GridSize.subscribe (
            (res: any) => { this.gridSize = res; }
        );
    }

    ngOnChanges (changes: SimpleChanges) {
        if (this.controllerName) {
			this.doRefresh();
        }
    }
	onClickRefresh () {
		this.doRefresh();
    }
	doRefresh () {

        if (!this.gridSrv.controllerName) {
            this.gridSrv.controllerName = this.controllerName;
        }

        if (!this.gridSrv.controllerName) {
            return;
        }
        this.freeze = true;
		this.gridSrv.getGridRowModel (0, '4').subscribe (
			(res: any) => {
				this.requestModel.pageSize = this.pageSize;
				this.requestModel.currentPage = 0;
				this.requestModel.keyId = this.keyValue;
				this.requestModel.findModel = res;
				this.doRefreshRequestModel();
			},
			(err: any) => {
				this.freeze = false;
				this.app.showError(err);
			}
		);

        //this.gridSrv.getGridList(this.keyValue).subscribe(
        //    (res: any) => {
        //        this.freeze = false;
        //        this.listRow = res;
        //    },
        //    (err: any) => {
        //        this.freeze = false;
        //        this.app.showError(err);
        //    }
        //);
    }
	doRefreshRequestModel() {
		if (this.requestModel && this.requestModel.findModel) {
			this.gridSrv.getGridRequestModel(this.requestModel).subscribe(
				res => {
					this.freeze = false;
					this.responceModel = res;
					this.calculatePages();
				},
				err => {
					this.app.showError(err);
					this.freeze = false;
				}
			);
		} else {
			this.freeze = false;
			this.app.showError('Не найдены параметры фильтров! Обратитесь в поддержку!');
			return;
		}
	}
	onSelectedRow(row: any) {
        this.selectedRow = row;
        this.selected.emit(this.selectedRow);
    }

    goPage(urlroot: string) {
        this.router.navigateByUrl('/' + urlroot, { replaceUrl: true });
    }

    onSaveAttachment(item: any) {
        if (item && item.id && this.responceModel.tableId) {
            this.attacment.metaObjectId = this.responceModel.tableId;
            this.attacment.objectId = item.id;
            $('#GridModalAttachment').modal('show');
        }
    }

    onClickAddNew(itemId: number, mode: string = '1') {
        if (this.urlEditPage) {
            switch (mode) {
                case '1':
                    this.goPage(`${this.urlEditPage}/${itemId}/new`);
                    break;
                case '2':
                    this.goPage(`${this.urlEditPage}/${itemId}/copy`);
                    break;
            }
            return;
        }

        if (this.isAddNew) {
            this.isAddNew = false;
            return;
        }
        this.freeze = true;
        this.gridSrv.getGridRowModel(itemId, mode).subscribe(
            (res: any) => {
                this.listColumn.forEach((item: any) => {
                    if (item.defaultValue && (!res[item.key])) {
                        res[item.key] = item.defaultValue;
                    }
                });
                this.freeze = false;
                this.newRowModel = res;
                this.isAddNew = true;
            },
            (err: any) => {
                this.freeze = false;
                this.app.showError(err);
            }
        );
    }
	isNotValidModel(model: any) {
		if ( model && this.listColumn ) {
			return this.listColumn.some ( ( c: any ) => c.isNotNull && model[c.key] == null );
		} else {
			return false;
		}
	}

    onCancelNewRow() {
        this.isAddNew = false;
    }

	onClickSearch(cmd: string) {
		switch (cmd) {
			case 'show':
				this.isShowSearch = true;
				break;
			case 'hide':
				this.isShowSearch = false;
				break;
		}
	}

    onDeleteRow(item: any) {
        if (item) {
            this.app.showQuestion(`Удалить выбранную строку?`).subscribe(
                (res: any) => {
                    if (res) {
                        this.gridSrv.deleteGridRowModel(item.id).subscribe(
                            (res: any) => {
                                this.freeze = false;
                                this.doRefresh();
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

    onEditRow(item: any) {
        if (this.urlEditPage) {
            this.goPage(`${this.urlEditPage}/${item.id}`);
            return;
        }
        this.freeze = true;
        this.editRowModel = null;
        this.gridSrv.getGridRowModel(item.id, null).subscribe(
            (res: any) => {
                this.freeze = false;
                this.editRowModel = res;
            },
            (err: any) => {
                this.freeze = false;
                this.app.showError(err);
            }
        );
    }


    onSaveNewRow() {
        this.freeze = true;
        if (this.keyValue > 0 && this.keyName) {
            this.newRowModel[this.keyName] = this.keyValue;
        }
        this.gridSrv.saveGridRowModel(this.newRowModel).subscribe(
            (res: any) => {
                this.freeze = false;
                this.doRefresh();
                this.onCancelNewRow();
            },
            (err: any) => {
                this.freeze = false;
                this.app.showError(err);
            }
        );
    }

    onCancelRow() {
        this.editRowModel = null;
        this.doRefreshRequestModel();
    }

    onSaveRow() {
        if (!this.editRowModel) {
            return;
        }

        this.gridSrv.saveGridRowModel(this.editRowModel).subscribe(
            (res: any) => {
                this.editRowModel = null;
                this.doRefreshRequestModel();
            },
            (err: any) => {
                this.app.showError(err);
            }
        );
    }

    onClickCarouselCmd(cmd: any) {
        if (!this.selectedIndex) {
            this.selectedIndex = 0;
        }

        switch (cmd) {
            case `next`:
                if (this.selectedIndex == this.listRow.length - 1) {
                    this.selectedIndex = 0;
                } else {
                    this.selectedIndex++;
                }
                break;
            case `prev`:
                if (this.selectedIndex == 0) {
                    this.selectedIndex = this.listRow.length - 1;
                } else {
                    this.selectedIndex--;
                }
                break;
            default: {
                this.selectedIndex = cmd;
            }
        }

        $(`#myCarousel`).carousel(cmd);
        $(`#myCarousel`).carousel(`pause`);
    }
	// пейджинг
    calculatePages() {
        if (!this.responceModel || !this.requestModel) {
            return;
        }
        this.countPage = Math.ceil(this.responceModel.totalRowCount / this.requestModel.pageSize);
    }

    selectPage(n: number) {
        this.requestModel.currentPage = n;
        this.doRefreshRequestModel();
    }
}