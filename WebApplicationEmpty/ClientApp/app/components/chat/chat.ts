import { Component, OnInit, OnChanges, Input, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';

import { GridService } from '../../services/grid.service';

@Component({
    selector: 'chat',
    templateUrl: 'chat.html',
	providers: [GridService],
	styles: [`
		.text-bottom{
			font-style: italic;
			font-size: xx-small;
			color: gray;
		}
		.chat-panel {
			height: 45em;
			overflow-y: scroll;
			overflow-x: hidden;
		}
		.chat-msg-parent {
			display: block;
			-webkit-margin-before: 1em;
			-webkit-margin-after: 1em;
			-webkit-margin-start: 40px;
			-webkit-margin-end: 40px;
		}
		.chat-msg-left {
			border-radius: 0 25px 25px 25px;
		}
		.chat-msg-right {
			border-radius: 25px 25px 0 25px;
		}
		.media-bottom {
			vertical-align: bottom;
		}
		.chat-msg {
		  background: #fff;
		  color: #222222;
		  padding: 20px;
		  padding-left: 50px;
		  box-sizing: border-box;
		  box-shadow: 0 2px 4px rgba(34, 34, 34, 0.12);
		  position: relative;
		  overflow: hidden;
		  min-height: 120px;
		}
		.btn {
			display: inline-block;
			font-weight: 400;
			text-align: center;
			white-space: nowrap;
			vertical-align: middle;
			-webkit-user-select: none;
			-moz-user-select: none;
			-ms-user-select: none;
			user-select: none;
			border: 1px solid transparent;
			padding: .5rem .75rem;
			font-size: 1rem;
			line-height: 1.25;
			border-radius: .25rem;
			transition: all .15s ease-in-out;
		}
		.btn-outline-primary {
			color: #007bff;
			background-color: transparent;
			background-image: none;
			border-color: #007bff;
		}
		.btn-outline-default {
			color: #868e96;
			background-color: transparent;
			background-image: none;
			border-color: #868e96;
		}
		.btn-outline-success {
			color: #28a745;
			background-color: transparent;
			background-image: none;
			border-color: #28a745;
		}
		.btn-outline-warning {
			color: #ffc107;
			background-color: transparent;
			background-image: none;
			border-color: #ffc107;
		}
		.btn-outline-info {
			color: #17a2b8;
			background-color: transparent;
			background-image: none;
			border-color: #17a2b8;
		}
		.btn-outline-danger {
			color: #d9534f;
			background-image: none;
			background-color: transparent;
			border-color: #d9534f;
		}
	`]
})
export class ChatList implements OnInit, OnChanges {

    @Input() isViewOnly = true;
    private logShow = false;

    selected: any = 0;
	freeze = false;
    GridSize;//размеры из бутстрапа

    current: any;
    currentEmp: any;
    permissions: any;
    listRow: any[] = [];
    newRowModel: any = { 'message': '' };
    //photo
	photoSrc: any;
	//attachment
    attachmentMetaObjectId: number;
	attchmentObjectId: number;

    constructor(private gridSrv: GridService) { }

    ngOnInit() {
        this.logShow = (window.location.hostname == `localhost`);
        if (this.logShow) {
            console.log(`EmployeeChatList.ngOnInit`);
        }
        
        if (this.current) {
			this.photoSrc = ''; // Указать путь к файлу
		}
    }
    ngOnChanges(changes: SimpleChanges) {
		this.gridSrv.controllerName = 'employeechat';
        this.onGenerateNewRow();
        this.onRefreshGrid();
    }
    onRefreshGrid() {
        this.freeze = true;
        this.gridSrv.getGridList(this.current ? this.current.id : 0).subscribe(
            (result: any[]) => {
                this.freeze = false;
                this.listRow = result;
                result.forEach((item: any) => {
                    if (item.employeePhotoFileName) {
                        item.photoSrc = '';//Указать путь к файлу
					}
                });
            },
            (err: any) => {
                this.freeze = false;
                console.log(err);
            }
        );
    }
    onClickAttachment(row: any) {
		if ( row && row.tableId ) {
			this.attachmentMetaObjectId = row.tableId;
			this.attchmentObjectId = row.id;
			$('#ModalAttach').modal('show');
		}
    }
    onGenerateNewRow() {
        this.freeze = true;
        this.gridSrv.getGridRowModel(0, '1').subscribe(
            (res: any) => {
                this.freeze = false;
                this.newRowModel = res;
            },
            (err: any) => {
                this.freeze = false;
                console.log(err);
            }
        );
    }
    onSaveNewRow() {
        this.freeze = true;
		if (this.current.id > 0) {
			this.newRowModel['authorId'] = this.current.id;
			this.gridSrv.saveGridRowModel(this.newRowModel).subscribe(
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
    onDownloadPhoto() {
		if (this.current) {
			return this.photoSrc;
		}
		else {
			return '';
		}
    }
    onDeleteRow(item: any) {
        if (!confirm("Удалить запись?")) {
            return;
        }

        if (item) {
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
    onSaveRow(editRowModel: any, cmd: string) {
        if (!editRowModel) {
            return;
        }
        if (editRowModel.ratedUsers && editRowModel.ratedUsers.indexOf(this.current.name) !== -1) {
            console.log('Уже лайкнули!');
            return;
		}
        switch (cmd) {
            case 'up':
				editRowModel.rating++;
			break;
			case 'down':
				editRowModel.rating--;
			break;
		}
		if ( editRowModel.ratedUsers ) {
			editRowModel.ratedUsers += ';' + this.current.name;
		}
		else {
			editRowModel.ratedUsers = this.current.name;
		}
        this.gridSrv.saveGridRowModel(editRowModel).subscribe(
			(res: any) => {
				this.onRefreshGrid();
			},
			(err: any) => {
				console.log(err);
			}
		);
    }
}