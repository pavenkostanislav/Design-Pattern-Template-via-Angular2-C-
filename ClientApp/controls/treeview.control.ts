﻿import { Component, Input, Output, EventEmitter } from '@angular/core';

export interface ITreeNode {
	id: number;
	name: string;
	children: Array<ITreeNode>;
	isExpanded: boolean;
	badge: number;
	parent: ITreeNode;
	isLeaf: boolean;
}

@Component({
	selector: 'tree-view',
	template: `
		<ul class="treenodes">
			<li *ngFor="let node of Nodes" class="treenode">
				<i *ngIf="!node.isLeaf" class="nodebutton fa fa-{{node.isExpanded ? 'minus' : 'plus'}}-square-o"
				   (click)="onExpand(node)">
				</i>
				<div class="nodeinfo">
					<i *ngIf="node.isLeaf" class="nodeicon fa fa-file-o"></i>
					<i *ngIf="!node.isLeaf" class="nodeicon fa fa-tags"></i>

					<span class="nodetext {{node == SelectedNode ? 'bg-info' : ''}} {{node.parent ? '' : 'text-root'}}"
						  (click)="onSelectNode(node)">
						{{node.name}}
					</span>
					<span *ngIf="node.badge > 0" class="nodebage badge">{{node.badge}}</span>

					<tree-view [Nodes]="node.children"
							   [SelectedNode]="SelectedNode"
							   (onSelectedChanged)="onSelectNode($event)"
							   (onRequestNodes)="onRequestLocal($event)"
							   *ngIf="node.isExpanded">
					</tree-view>
				</div>
			</li>
		</ul>
	`,
	styles: [
		'.treenodes {display:table; list-style-type: none; padding-left: 16px;}',
		':host .treenodes { padding-left: 0; }',
		'.treenode { display: table-row; list-style-type: none; }',
		'.nodebutton { display:table-cell; cursor: pointer; }',
		'.nodeinfo { display:table-cell; padding-left: 5px; list-style-type: none; }',
		'.nodetext { color: #31708f; padding-left: 3px; padding-right: 3px; cursor: pointer; }',
		'.nodetext.bg-info { font-weight: bold; }',
		'.nodetext.text-root { font-size: 16px; font-weight: bold; }'
	]
})
export class TreeView {

	@Input() Nodes: Array<ITreeNode>;
	@Input() SelectedNode: ITreeNode;
	@Input() isRoot: boolean;

	@Output() onSelectedChanged: EventEmitter<ITreeNode> = new EventEmitter<ITreeNode>();
	@Output() onRequestNodes: EventEmitter<ITreeNode> = new EventEmitter<ITreeNode>();

	constructor() { }

	onSelectNode(node: ITreeNode) {
		this.onSelectedChanged.emit(node);
	}

	onExpand(node: ITreeNode) {

		node.isExpanded = !node.isExpanded;

		if (node.isExpanded && (!node.children || node.children.length === 0)) {
			this.onRequestNodes.emit(node);
		}
	}

	onRequestLocal(node: ITreeNode) {

		this.onRequestNodes.emit(node);
	}
}