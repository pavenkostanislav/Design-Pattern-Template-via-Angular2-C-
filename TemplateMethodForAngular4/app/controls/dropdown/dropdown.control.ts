import {
	Component, OnInit, OnChanges, forwardRef, Input,
	Output, EventEmitter, SimpleChange, HostListener, ElementRef,
	AfterViewChecked
} from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, FormControl, FormGroup } from '@angular/forms';
import 'rxjs/Rx';
import { App } from '../../app.component';
import { SelectService, SelectItem, SelectGroup } from '../../services/select.service';

@Component({
	selector: 'dropdown',
	templateUrl: './dropdown.html',
	styleUrls: ['./dropdown.scss'],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => DropDown),
			multi: true
		}
	]
})
export class DropDown implements OnInit, OnChanges, ControlValueAccessor, AfterViewChecked {

	@Input() items: SelectItem[];
	@Input() itemType: string;
	@Input() minTerm = 3;
	@Input() disabled = false;
	@Input() required = false;
	@Input() allowClear = false;
	@Input() parentId: any;
	@Input() term: string;
	@Input() showGroups = false;
	@Input() autocomplete = false;

	@Output() valueChange: EventEmitter<number> = new EventEmitter<number>();

	showLog = false;

	innerValue: number;
	showDropDown = false;
	hoverItem: SelectItem;
	selectedItem: SelectItem;

	innerList: SelectItem[];
	groupList: SelectGroup[];
	_noGroup = new SelectGroup(-1, '...');

	// LazyQuery: any;
	SearchQuery: any;
	vall: any;
	searchValue = '';
	placeholder = 'Значение не выбрано...';

	myGroup = new FormGroup({
		termInput: new FormControl()
	});

	constructor(private element: ElementRef,
		private app: App,
		private selSrv: SelectService) {

		this.app.UpdateApp.subscribe(
			(res: any) => {
				this.refreshAll();
			}
		);
	}

	ngOnInit() {

		if (this.showLog) {
			console.log('DropDownComponent.ngOnInit');
		}

		this.SearchQuery = this.myGroup.controls['termInput'].valueChanges
			.filter((term) => {
				if (this.showLog) {
					console.log('DropDownComponent.ngOnInit.SearchQuery.filter', term);
				}
				return this.termFilter(term);
			})
			.distinctUntilChanged()
			.subscribe(
			res => {
				this.refreshList(res);
			},
			err => {
				throw new Error(err);
			});

		if (this.innerValue) {
			this.requestValue(this.innerValue);
		}
	}

	ngOnChanges(changes: { [key: string]: SimpleChange; }) {

		if (this.showLog) {
			console.log('DropDownComponent.ngOnChanges(changes)', changes);
		}

		if (this.items && this.itemType) {
			this.SearchQuery = null;
			throw new Error('Нельзя устанавливать одновременно параметр items и itemType!');
		}

		if (this.items && changes['minTerm'] && changes['minTerm'].currentValue) {
			throw new Error('Параметр minTerm используется только совместно с параметром itemType!');
		}

		if (changes['items'] && this.items) {
			this.innerList = this.items;
			if (this.innerValue) {
				this.requestValue(this.innerValue);
			}
		}

		if (changes['parentId'] && this.innerValue && !changes['parentId'].isFirstChange()) {
			this.innerValue = null;
		}

		if (changes['disabled']) {
			const el: HTMLDivElement = this.element.nativeElement;
			if (this.disabled) {
				el.setAttribute('disabled', '');
			} else {
				el.removeAttribute('disabled');
			}
		}

		if (changes['required']) {
			const el: HTMLDivElement = this.element.nativeElement;
			if (this.required) {
				el.setAttribute('required', null);
			} else {
				el.removeAttribute('required');
			}
		}
	}

	ngAfterViewChecked() {
		if (this.showLog) {
			console.log('DropDown.ngAfterViewChecked');
		}
		const htmlElement: HTMLElement = this.element.nativeElement;
		const input = htmlElement.querySelector('input');
		if (input) {
			input.focus();
		}
	}

	checkInputs() {
		if (this.showLog) {
			console.log('DropDown.checkInputs');
		}

		if (this.itemType && this.items) {
			throw new Error('Нельзя устанавливать одновременно параметр items и itemType!');
		}

		if (!this.itemType && !this.items) {
			throw new Error('Не устанавлен ни один из параметров items и itemType!');
		}
	}

	requestValue(val: number) {

		if (this.showLog) {
			console.log('DropDown.requestValue(val)', val);
		}
		this.checkInputs();

		this.innerValue = (val ? val : null);

		if (val) {
			if (this.itemType) {
				this.selSrv.getSelectItemId(this.itemType, val).subscribe(
					res => {
						if (res) {
							this.selectedItem = res;
							if (this.showLog) {
								console.log('DropDown.requestValue.getSelectItemId.res', res);
							}
						} else {
							this.innerValue = null;
							this.selectedItem = null;
							throw new Error(`В списке значений itemType не найдено значение: ${val}!`);
						}
					},
					err => {
						throw new Error(err);
					}
				);
				return;
			}
			if (this.items && this.items.length > 0) {
				this.selectedItem = this.items.find(f => f.id === val);
				if (!this.selectedItem) {
					this.innerValue = null;
					this.selectedItem = null;
					throw new Error(`В списке значений items не найдено значение: ${val}!`);
				}
			}
		} else {
			this.innerValue = null;
			this.selectedItem = null;
		}
	}

	refreshList(term: string) {
		if (this.showLog) {
			console.log('DropDownComponent.refreshList(term)', term);
		}

		if (this.itemType) {
			this.selSrv.getSelectList(this.itemType, this.parentId, term)
				.subscribe(
				res => {
					this.innerList = res;
					this.refreshGroupList();
				},
				err => {
					throw new Error(err);
				}
				);
			return;
		}

		if (this.items) {
			this.innerList = this.items.filter(m => m.text.toLowerCase().includes(term.toLowerCase()));
			this.refreshGroupList();
		}
	}

	refreshGroupList() {

		if (this.showLog) {
			console.log('DropDownComponent.refreshGroupList()');
		}
		this.groupList = [];
		if (!this.showGroups) {
			return;
		}
		if (!this.innerList) {
			return;
		}

		this.groupList = this.innerList
			.map(item => {
				if (item.group) {
					return item.group;
				} else {
					return this._noGroup;
				}
			})
			.sort((a, b) => {

				if (!a.sort && b.sort) {
					return -1;
				}
				if (!a.text && b.text) {
					return -1;
				}

				if (a.sort && !b.sort) {
					return 1;
				}
				if (a.text && !b.text) {
					return 1;
				}

				if (a.sort && b.sort) {
					if (a.sort > b.sort) {
						return 1;
					} else {
						return -1;
					}
				}
				if (!a.sort && !b.sort && a.text && b.text) {
					if (a.text > b.text) {
						return 1;
					} else {
						return -1;
					}
				}
			})
			.filter((v, i, a) => a.indexOf(a.find(f => f.id === v.id)) === i); // хитрый фильтр, аналог distinct
	}

	searchList(groupId?: number): SelectItem[] {

		if (this.showLog) {
			console.log('DropDownComponent.searchList(groupId)', groupId);
		}
		if (!this.innerList || this.searchValue.length < this.minTerm) {
			return [];
		}

		let list: SelectItem[] = [];

		if (groupId) {
			if (groupId === -1) {
				list = this.innerList.filter(m => !m.group && m.text.toLowerCase().includes(this.searchValue.toLowerCase()));
			} else {
				list = this.innerList.filter(m => m.group
					&& m.group.id === groupId
					&& m.text.toLowerCase().includes(this.searchValue.toLowerCase()));
			}
		} else {
			list = this.innerList.filter(m => m.text.toLowerCase().includes(this.searchValue.toLowerCase()));
		}

		return list.sort((a, b) => {
			if (!a.sort && !b.sort) {
				if (a.text > b.text) {
					return 1;
				} else {
					return -1;
				}
			}
			if (!a.sort && b.sort) {
				return -1;
			}
			if (a.sort && !b.sort) {
				return 1;
			}
			if (a.sort && b.sort) {
				if (a.sort > b.sort) {
					return 1;
				} else {
					return -1;
				}
			}
		});
	}

	termFilter(term: string) {

		if (this.showLog) {
			console.log('DropDownComponent.termFilter(term)', term);
		}
		if (this.items) {
			return true;
		}
		if (this.itemType) {
			if (this.minTerm === 0) {
				return true;
			}
			if (this.itemType.startsWith('Enum-')) {
				return true;
			}
			return term.length >= this.minTerm;
		}
	}

	onChange = (_: any) => { };
	onTouched = () => { };

	registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }
	registerOnTouched(fn: () => void): void { this.onTouched = fn; }

	writeValue(val: any) {

		if (this.showLog) {
			console.log('DropDownComponent.writeValue(val)', val);
		}
		if (val === 0) {
			// console.log('DropDown.writeValue.НОЛЬ');
			this.innerValue = undefined;
			this.selectedItem = null;
			return;
		}
		this.requestValue(val);
		this.setAutocomplete(val);

	}

	setDisabledState(isDisabled: boolean) {
		if (this.showLog) {
			console.log('DropDownComponent.setDisabledState(isDisabled)', isDisabled);
		}
		this.disabled = isDisabled;
	}

	@HostListener('document: keydown', ['$event'])
	trackKey(event: KeyboardEvent) {

		if (this.showLog) {
			console.log('DropDownComponent.trackKey(event)', event);
		}

		if (this.showDropDown) {

			switch (event.keyCode) {
				// case 13:
				//     this.hoverSelect();
				//     break;
				case 27:
					this.showHide(false);
					break;
				// case 38:
				//     this.hoverUp();
				//     break;
				// case 40:
				//     this.hoverDown();
				//     break;
			}
		}
	}

	@HostListener('document: click', ['$event.target'])
	trackClick(target: HTMLElement) {

		if (this.showLog) {
			console.log('DropDownComponent.trackClick(target)', target);
		}

		if (!this.showDropDown) {
			return;
		}
		// console.log('DropDown.trackClick', target, this.element.nativeElement);
		let parentFound = false;

		while (target != null && !parentFound) {
			if (target === this.element.nativeElement) {
				parentFound = true;
			}
			target = target.parentElement;
		}

		if (!parentFound) {
			this.showHide(false);
		}

	}

	onHoverItem(item: SelectItem) {
		if (this.showLog) {
			console.log('DropDownComponent.onHoverItem(item)', item);
		}
		this.hoverItem = item;
	}

	hoverUp() {

		if (this.itemType) {

			if (!this.hoverItem) {
				return;
			}

			const list = this.searchList();

			const i = list.indexOf(this.hoverItem);
			if (i > 0) {
				this.hoverItem = list[i - 1];
			}
		}
	}

	hoverDown() {

		if (this.itemType) {
			const list = this.searchList();
			if (!this.hoverItem) {
				this.hoverItem = list[0];
			} else {
				const i = list.indexOf(this.hoverItem);
				if (i < list.length - 1) {
					this.hoverItem = list[i + 1];
				}
			}
		}
	}

	hoverSelect() {
		if (this.hoverItem && this.showDropDown) {
			this.onSelectItem(this.hoverItem);
		}
	}

	onSelectItem(item: SelectItem) {
		if (this.showLog) {
			console.log('DropDownComponent.onSelectItem(item)', item);
		}

		this.selectedItem = item;
		if (item) {
			this.onChange(item.id);
			this.valueChange.emit(item.id);
		} else {
			this.onChange(null);
			this.valueChange.emit(null);
		}
		this.showHide(false);
	}

	clearSelect() {
		if (this.showLog) {
			console.log('DropDownComponent.clearSelect()');
		}

		this.onSelectItem(undefined);
		this.onTouched();
	}


	showHide(show: boolean) {
		if (this.showLog) {
			console.log('DropDownComponent.showHide(show)', show);
		}

		this.showDropDown = show;

		if (this.showDropDown) {
            this.onTouched();
            this.refreshList('');
		} else {
			this.searchValue = '';
			this.refreshList('');
			this.hoverItem = undefined;
			if (this.itemType && this.minTerm > 0) {
				this.innerList = [];
			}
		}
	}
	refreshAll() {
		this.setAutocomplete(this.vall);
	}
	setAutocomplete(val: any) {
		if ((val === undefined || val === 0) && this.autocomplete && (this.minTerm === 0 || this.items)) {
			if (this.itemType) {

				this.selSrv.getSelectList(this.itemType, this.parentId, '').subscribe(list => {
					if (list.length === 1) {
						this.onSelectItem(list[0]);
					}
				});

			} else if (this.items) {

				if (this.items.length === 1) {
					this.onSelectItem(this.items[0]);
				}

			}
		}
	}
}
