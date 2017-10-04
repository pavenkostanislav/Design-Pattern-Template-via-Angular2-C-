import { Component, OnInit, OnChanges, Input, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';

import { App, GridSize } from '../../app.component';
import { GridService } from '../../services/grid.service';
import { UserService, UserViewModel, Permissions } from '../../services/user.service';


@Component({
	selector: 'grid',
	templateUrl: 'grid.html',
	providers: [GridService]
})
export class GridRowList implements OnInit, OnChanges {

	@Input() controllerName: string;
	@Input() listColumn: string[] = [];
	@Input() name: string;
	@Input() id: number;
	@Input() keyName: string;
	@Input() isViewOnly = true;

	test: string;

	private logShow = false;

	isAddNew: boolean = false;
	freeze = false;
	GridSize = GridSize;

	currentUser: UserViewModel = new UserViewModel();
	permissions: Permissions = new Permissions();

	listRow: any[] = [];
	editRowModel: any;
	newRowModel: any;
	selectedRow: any;
	selectedIndex: number = 0;
	gridSize: GridSize = GridSize.lg;

	constructor(private app: App,
		private gridSrv: GridService) { }

	ngOnInit() {
		this.logShow = (window.location.hostname == `localhost`);
		if (this.logShow) {
			console.log(`GridEdit.ngOnInit`);
		}
		this.currentUser = this.app.currentUser;
		this.permissions = this.app.permissions;

		this.app.GridSize.subscribe(
			(res: any) => { this.gridSize = res; }
		);
	}

	ngOnChanges(changes: SimpleChanges) {
		if (this.controllerName) {
			this.onRefreshGrid();
		}
	}

	onRefreshGrid() {

		if (!this.gridSrv.controllerName) {
			this.gridSrv.controllerName = this.controllerName;
		}

		if (!this.gridSrv.controllerName) {
			return;
		}
		this.freeze = true;
		this.gridSrv.getGridList(this.id).subscribe(
			(res: any) => {
				this.freeze = false;
				this.listRow = res;
			},
			(err: any) => {
				this.freeze = false;
				console.log(err);
			}
		);
	}

	onClickAddNew() {
		if (this.isAddNew) {
			this.isAddNew = false;
			return;
		}
		this.freeze = true;
		this.gridSrv.getGridRowModel(0, '1').subscribe(
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
				console.log(err);
			}
		);
	}

	onCancelNewRow() {
		this.isAddNew = false;
	}

	onDeleteRow(item: any) {
		this.freeze = true;
		if (item) {
			this.app.showQuestion(`Удалить выбранную строку?`).subscribe(
				(res: any) => {
					if (res) {
						this.gridSrv.deleteGridRowModel(item.id).subscribe(
							(res: any) => {
								this.freeze = false;
								this.onRefreshGrid();
							},
							(err: any) => {
								this.freeze = false;
								console.log(err);
							}
						);
					}
				}
			);
		}
	}

	onEditRow(item: any) {
		this.freeze = true;
		this.editRowModel = null;
		this.gridSrv.getGridRowModel(item.id, null).subscribe(
			(res: any) => {
				this.freeze = false;
				this.editRowModel = res;
			},
			(err: any) => {
				this.freeze = false;
				console.log(err);
			}
		);
	}

	onSaveNewRow() {
		this.freeze = true;
		if (this.id > 0 && this.keyName) {
			this.newRowModel[this.keyName] = this.id;
		}
		this.gridSrv.saveGridRowModel(this.newRowModel).subscribe(
			(res: any) => {
				this.freeze = false;
				this.onRefreshGrid();
				this.onCancelNewRow();
			},
			(err: any) => {
				this.freeze = false;
				console.log(err);
			}
		);
	}

	onCancelRow() {
		this.editRowModel = null;
		this.onRefreshGrid();
	}

	onSaveRow() {
		if (!this.editRowModel) {
			return;
		}

		this.gridSrv.saveGridRowModel(this.editRowModel).subscribe(
			(res: any) => {
				this.editRowModel = null;
				this.onRefreshGrid();
			},
			(err: any) => {
				console.log(err);
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
}