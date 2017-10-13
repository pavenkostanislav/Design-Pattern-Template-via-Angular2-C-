import { Component, Input, Output, forwardRef, AfterViewInit, ViewChild, SimpleChange, ElementRef, OnChanges, OnDestroy, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, FormControl } from '@angular/forms';

//import 'jquery';
//import 'moment';
import 'eonasdan-bootstrap-datetimepicker';

const CUSTOM_VALUE_ACCESSOR = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => DateTimePicker),
	multi: true
};

@Component({
	selector: 'date-time',
	template: `
		<div #group class='input-group date'>
			<input #input [disabled]="disabled" (focus)="onTouched()" type='text' class="form-control" />
			<span class="input-group-addon">
				<span class="fa fa-calendar"></span>
			</span>
		</div>`,
	providers: [CUSTOM_VALUE_ACCESSOR]
})
export class DateTimePicker implements AfterViewInit, OnDestroy, ControlValueAccessor {

	private logDateTimePicker = false;

	@ViewChild('group') group: ElementRef;
	@ViewChild('input') input: ElementRef;
	//@Input() value: Date;
	//@Input() options: any;
	@Input() format = 'DD.MM.YYYY';
	@Input() locale = 'ru';
	@Input() disabled = false;
	@Input() required = false;
	//@Output() valueChange: EventEmitter<string> = new EventEmitter<string>();

	private innerValue: any = '';
	private _group: JQuery;
	private _input: JQuery;
	private doEvent = true;
	private i9d = false;
	private dpShow = false;

	private icons = {
		time: 'fa fa-time',
		date: 'fa fa-calendar',
		up: 'fa fa-chevron-up',
		down: 'fa fa-chevron-down',
		previous: 'fa fa-chevron-left',
		next: 'fa fa-chevron-right',
		today: 'fa fa-screenshot',
		clear: 'fa fa-trash',
		close: 'fa fa-close'
	};

	constructor(private element: ElementRef) {
	}

	//get accessor
	get value(): any {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.get value: ' + this.innerValue);
		}
		return this.innerValue;
	};

	//set accessor including call the onchange callback
	set value(v: any) {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.set value: ' + v);
		}

		if (v !== this.innerValue) {
			this.innerValue = v;
			if (this.i9d && !this.dpShow) {
				this.doEvent = false;
				if (this.innerValue == undefined) {
					this.innerValue = null;
				}
				this._group.data('DateTimePicker').date(this.innerValue);
				this.doEvent = true;
			}
			this.onChange(v);
		}
	}


	ngAfterViewInit() {

		if (this.logDateTimePicker) {
			console.log(`datetimepicker.ngAfterViewInit format: ${this.format}, locale: ${this.locale}, icons: ${this.icons}`);
		}

		if (!this.i9d) {
			this._input = $(this.input.nativeElement);
			this._group = $(this.group.nativeElement);
			this._group.datetimepicker({
				format: this.format,
				locale: this.locale,
				icons: this.icons
			});

			if (this.innerValue) {
				this._group.data('DateTimePicker').date(this.innerValue);
			}
			this._group.on('dp.change', this.onValueChange.bind(this));
			this._group.on('dp.show', this.onShownCalendar.bind(this));
			this._group.on('dp.hide', this.onHideCalendar.bind(this));
			this.i9d = true;
		}
	}

	ngOnDestroy() {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.ngOnDestroy');
		}

		if (this._group.data('DateTimePicker')) {
			this._group.data('DateTimePicker').destroy();
		}
	}

	ngOnChanges(changes: { [key: string]: SimpleChange; }) {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.ngOnChanges: ' + JSON.stringify(changes));
		}

		if (changes['disabled']) {
			if (!this.disabled && this.i9d) {
				this._group.data('DateTimePicker').enable();
			}
		}

		if (changes['required']) {
			let el: HTMLDivElement = this.element.nativeElement;

			let jqEl = $(el);

			if (this.required) {
				jqEl.attr({ required: null });
			}
			else {
				jqEl.removeAttr('required');
			}
		}
	}

	onShownCalendar($event: any) {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.onShownCalendar');
		}

		this.dpShow = true;
		this.onTouched();
	}

	onHideCalendar($event: any) {
		if (this.logDateTimePicker) {
			console.log('datetimepicker.onHideCalendar');
		}
		this.dpShow = false;
	}

	onValueChange($event: any) {

		if (this.logDateTimePicker) {
			console.log('datetimepicker.onValueChange: ' + ($event.date) + ' doEvent: ' + this.doEvent);
		}
		if (!this.i9d) {
			return;
		}

		if (this.doEvent) {
			let moment = $event.date; //this._group.data('DateTimePicker').date();
			if (moment) {
				let ret = moment.format(this.format);
				this.onChange(ret);
			}
			else {
				this.onChange(undefined);
			}
		}
	}


	// функционал ngModel
	onChange = (_: any) => { };
	onTouched = () => { };

	registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }
	registerOnTouched(fn: () => void): void { this.onTouched = fn; }

	// установка значения из модели при инициализации
	writeValue(val: any) {

		this.innerValue = val;
		if (this.logDateTimePicker) {
			console.log('datetimepicker.writeValue: ' + val);
		}
		if (this.i9d) {
			if (val) {
				this.doEvent = false;
				this._group.data('DateTimePicker').date(this.innerValue);
				this.doEvent = true;
			}
			else {
				this.doEvent = false;
				this._group.data('DateTimePicker').date(null);
				this.doEvent = true;
			}
		}
	}
}
