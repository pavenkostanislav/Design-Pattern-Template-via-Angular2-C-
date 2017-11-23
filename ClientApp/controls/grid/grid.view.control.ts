import { Component, OnInit, OnChanges, Input, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';

import { App, GridSize } from '../../app.component';
import { GridService, GridRequestModel, GridResponseModel } from '../../services/grid.service';
import { UserService, UserViewModel, Permissions } from '../../services/user.service';
import { MetaConstantService } from '../../services/metaconstant.service';


@Component({
	selector: 'grid-view',
	templateUrl: 'grid.view.html',
	providers: [MetaConstantService, GridService]
})
export class GridViewControl implements OnInit, OnChanges {
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
	@Input() canReadInfo: boolean = true;
	@Input() canDelete: boolean = true;
	@Input() canEdit: boolean = true;
	@Input() canCopy: boolean = false;
	@Input() canAttach: boolean = false;
	@Input() canFilter: boolean = false;

	isShowFilter: boolean = false;
	requestModel: GridRequestModel = new GridRequestModel();
	responceModel: GridResponseModel = new GridResponseModel();
	///page
	@Input() countButtonPage = 20;
	countPage = 10;
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
	selectedRow: any = { id: 0, createdBy: '', createdDate: '00.00.0000', lastUpdatedBy: '', lastUpdatedDate: '00.00.0000' };
	selectedIndex: number = 0;
	gridSize: GridSize = GridSize.lg;
	findModel: any = {
			id: 0, createdBy: '',
			createdDateStart: '00.00.0000',
			createdDateEnd: '00.00.0000',
			lastUpdatedBy: '',
			lastUpdatedDateStart: '00.00.0000',
			lastUpdatedDateEnd: '00.00.0000'
		};
		
	isGetingFindModel: boolean = false;
	constructor(private app: App,
				private constSrv: MetaConstantService,
				private router: Router,
				private gridSrv: GridService,
				private sanitizer: DomSanitizer) {
			this.logShow = (window.location.hostname == `localhost`);
			if (this.logShow) {
				console.log(`GridView.constructor`);
			}
		}

	ngOnInit() {
		this.logShow = (window.location.hostname == `localhost`);
		if (this.logShow) {
			console.log(`${this.controllerName}: GridView.ngOnInit`);
		}
		this.currentUser = this.app.currentUser;
		this.permissions = this.app.permissions;
		this.app.GridSize.subscribe(
			(res: any) => { this.gridSize = res; }
		);
		this.setControllerName();
		this.freeze = true;
		this.isGetingFindModel = true;
		this.gridSrv.getGridRowModel(0, '4').subscribe(
			(res: any) => {
				this.findModel = res;
				this.doRefreshWithCheckController();
				this.freeze = false;
			},
			(err: any) => {
				this.freeze = false;
				this.app.showError(err);
			}
		);
	}

	ngOnChanges(changes: SimpleChanges) {
		if (this.logShow) {
			console.log(`${this.controllerName}: GridView.ngOnChanges`);
		}
		if(this.isGetingFindModel) {
			this.doRefreshWithCheckController();
		} else {
			this.isGetingFindModel = true;
			this.setControllerName();
			this.freeze = true;
			this.gridSrv.getGridRowModel(0, '4').subscribe(
				(res: any) => {
					this.findModel = res;
					this.doRefreshWithCheckController();
					this.freeze = false;
				},
				(err: any) => {
					this.freeze = false;
					this.app.showError(err);
				}
			);
		}
	}

	public onSetFindModel(findmodel: any) {
		for (let propertyName in findmodel) {
			if (findmodel[propertyName] == null) {
				this.doChangeFindModel_isValid(findmodel, propertyName);
			}
		}
		localStorage.setItem('FindModel_' + this.controllerName, JSON.stringify(findmodel));
		this.doRefresh();
	}

	onClickRefresh() {
		this.setControllerName();
		localStorage.setItem('FindModel_' + this.controllerName, JSON.stringify(this.requestModel.findModel));
		this.doRefreshRequestModel();
	}
	onClickClear() {
		this.setControllerName();
		this.doRefreshFindModel();
	}
	doRefreshFindModel() {
		this.setControllerName();
		this.freeze = true;
		this.isGetingFindModel = true;
		this.gridSrv.getGridRowModel(0, '4').subscribe(
			(res: any) => {
				this.requestModel.findModel = res;
				this.findModel = Object.assign({}, res);
				localStorage.setItem('FindModel_' + this.controllerName, JSON.stringify(this.requestModel.findModel));
				this.doRefreshRequestModel();
				this.freeze = false;
			},
			(err: any) => {
				this.freeze = false;
				this.app.showError(err);
			}
		);
	}
	setControllerName() {
		if (!this.gridSrv.controllerName) {
			this.gridSrv.controllerName = this.controllerName;
		}

		if (!this.gridSrv.controllerName) {
			return;
		}
	}
	doRefresh() {
		this.setControllerName();
		this.freeze = true;
		this.constSrv.getMetaConstantId('Table.' + this.gridSrv.controllerName).subscribe(
			(res: any) => {
				this.attacment.metaObjectId = res;

				this.requestModel.keyId = this.keyValue;
				if (localStorage.getItem('SortList_' + this.controllerName)) {
					this.requestModel.orderNamesList = JSON.parse(localStorage.getItem('SortList_' + this.controllerName));
				}
				let ls = localStorage.getItem('FindModel_' + this.controllerName);
				let rm: GridRequestModel = new GridRequestModel();
				if (ls && ls != rm.findModel) {
					this.requestModel.findModel = JSON.parse(ls);
					if (this.logShow) {
						console.log(`${this.controllerName}: GridView.doRefresh Get requestModel.findModel at localStorage`);
					}
					this.doRefreshRequestModel();
				} else {
					this.doRefreshFindModel();
				}
			},
			(err: any) => {
				this.app.showError(err);
			}
		);

	}
	doRefreshRequestModel() {
		if (this.requestModel) {
			this.gridSrv.getGridRequestModel(this.requestModel).subscribe(
				res => {
					this.freeze = false;
					this.responceModel = res;
					this.countButtonPage = this.requestModel.pageSize;
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
	doRefreshWithCheckController() {
		if (this.controllerName) {
			if (localStorage.getItem('CountButtonPage_' + this.controllerName)) {
				this.countButtonPage = Number(localStorage.getItem('CountButtonPage_' + this.controllerName));
			}
			this.requestModel.pageSize = this.countButtonPage;
			this.doRefresh();
		}
	}
	onSelectedRow(row: any) {
		this.selectedRow = row;
		this.selected.emit(this.selectedRow);
	}
	goPage(urlroot: string, replaceUrl: boolean = false) {
		if (!replaceUrl) {
			this.router.navigateByUrl('/' + urlroot);
		} else {
			this.router.navigateByUrl('/' + urlroot, { replaceUrl: true });
		}
	}
	model_isNotValid(model: any) {
		if (model && this.listColumn) {
			return this.listColumn.some((c: any) => c.isNotNull && model[c.key] == null);
		} else {
			return false;
		}
	}
	faSort_isVisible(col: any): boolean {
		let nameproperty = col.keyName ? col.keyName : col.key;
		return this.requestModel.orderNamesList.some(str => str == nameproperty || str == nameproperty + ' asc' || str == nameproperty + ' desc');
	}
	faSort_icon(col: any): string {
		let icon = 'fa-sort';
		switch (col.type) {
			case 'number':
				icon += '-numeric';
				break;
			case 'dropdown':
			case 'textarea':
			case 'text':
			case 'none':
				icon += '-alpha';
				break;
			default:
				icon += '-amount';
				break;
		}
		let nameproperty = col.keyName ? col.keyName : col.key;
		if (this.requestModel.orderNamesList.some(str => str == nameproperty || str == nameproperty + ' asc')) {
			return icon += '-asc';
		}
		if (this.requestModel.orderNamesList.some(str => str == nameproperty + ' desc')) {
			return icon += '-desc';
		}
	}
	badgeSort_indexOrderBy(col: any): number {
		let nameproperty = col.keyName ? col.keyName : col.key;
		return this.requestModel.orderNamesList.findIndex(str => str == nameproperty || str == nameproperty + ' asc' || str == nameproperty + ' desc') + 1;
	}
	onClickOrderBy(col: any) {
		this.orderNamesListAdd(col);
		localStorage.setItem('SortList_' + this.controllerName, JSON.stringify(this.requestModel.orderNamesList));
		this.doRefreshRequestModel();
	}
	orderNamesListAdd(col: any) {
		let nameproperty = col.keyName ? col.keyName : col.key;
		let nAsc: number = this.requestModel.orderNamesList.findIndex(str => str == nameproperty || str == nameproperty + ' asc');
		let nDesc: number = this.requestModel.orderNamesList.findIndex(str => str == nameproperty + ' desc');
		if (nAsc == -1 && nDesc == -1) {
			this.requestModel.orderNamesList.push(nameproperty + ' asc');
		} else {
			if (nAsc > -1) {
				this.requestModel.orderNamesList[nAsc] = nameproperty + ' desc';
			} else {
				if (nDesc > -1) {
					this.requestModel.orderNamesList.splice(nDesc, 1);
				}
			}
		}
	}
	onClickOrderByClean() {
		this.requestModel.orderNamesList = [];
		localStorage.setItem('SortList_' + this.controllerName, JSON.stringify(this.requestModel.orderNamesList));
		this.doRefreshRequestModel();
	}
	onClickOpenModalAttachment(item: any) {
		if (item && item.id && this.attacment.metaObjectId) {
			this.attacment.objectId = item.id;
			$('#AttachmentModalGridView').modal('show');
		}
	}
	onClickOpenModalReadInfo(item: any) {
		$('#InfoModalGridView').modal('show');
	}
	onClickOpenModalInfoModalFilter() {
		$('#InfoModalFilter').modal('show');
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
	onCancelNewRow() {
		this.isAddNew = false;
	}
	onClickSearch() {
		this.isShowFilter = !this.isShowFilter;
	}
	onDeleteRow(item: any) {
		if (item) {
			this.app.showQuestion(`Удалить выбранную строку?`).subscribe(
				(res: any) => {
					if (res) {
						this.gridSrv.deleteGridRowModel(item.id).subscribe(
							(res: any) => {
								this.freeze = false;
								this.doRefreshRequestModel();
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
				this.doRefreshRequestModel();
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
	doCountButtonPageChange() {
		if (this.countButtonPage && this.responceModel && this.countButtonPage >= 1) {
			if (this.countButtonPage > this.responceModel.totalRowCount) {
				this.countButtonPage = this.responceModel.totalRowCount;
			}
			this.requestModel.pageSize = this.countButtonPage;
			localStorage.setItem('CountButtonPage_' + this.controllerName, this.countButtonPage.toString());
			this.doRefreshRequestModel();
		} else {
			this.countButtonPage = this.requestModel.pageSize;
		}
	}
	    dd_OnChanged(event: number, findmodel: any, propertyName: string) {
        if (event == null) {
			this.doChangeFindModel_isValid(findmodel, propertyName);
        }
    }
	doChangeFindModel_isValid(findmodel: any, propertyName: string) {
		if (this.findModel && (this.findModel[propertyName] == 0 || this.findModel[propertyName])) {
			this.listColumn.filter((itemColumn: any) => itemColumn.key == propertyName).forEach((item: any) => {
				findmodel[propertyName] = item.defaultValue ? item.defaultValue : this.findModel[propertyName];
			});
		}
	}
}