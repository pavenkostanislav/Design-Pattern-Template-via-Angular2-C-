import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
	provide: NG_VALUE_ACCESSOR,
	useExisting: forwardRef(() => SwitchCheckbox),
	multi: true
};



//<label *ngFor="#moQV of moQuestionnaireValue" class="btn btn-info text-left" style= "text-align: left;float: left;"[class.active] = "moQV.id === step.answerId" >
//    <input type="radio"(click) = "step.answerId = moQV.id"[checked] = "moQV.id === step.answerId" > {{' ' + moQV.hint + ' (' + moQV.name + ')' }}
//</label>
//<div class="form-group" * ngIf="visible" >
//<label><ng-content></ng-content>
//                        <input type="checkbox" [(ngModel)]="value"
//                                class="form-control"
//                                (blur)="onBlur()"
//                        [disabled]="disabled">
//                                </label>
//                                < /div>
@Component({
	selector: 'switch-checkbox',
	template: `
                <div class="btn-group btn-toggle form-group" *ngIf="visible"> 
                    <button class="btn btn-{{size}} btn-{{value ? modifier : 'default'}}" [class.active]="value" (click)="value=true" [disabled]="disabled">Да</button>
                    <button class="btn btn-{{size}} btn-{{!value ? modifier : 'default'}}"  [class.active]="!value" (click)="value=false" [disabled]="disabled">Нет</button>
                </div>
`,
	providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})

export class SwitchCheckbox implements ControlValueAccessor {

	//The internal data model
	private innerValue: any = '';

	@Input() size = 'md';
	@Input() modifier = 'info';
	@Input() visible = true;
	@Input() disabled = false;
	@Output() valueChange: EventEmitter<any> = new EventEmitter<any>();

	//Placeholders for the callbacks which are later providesd
	//by the Control Value Accessor
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;

	//get accessor
	get value(): any {
		return this.innerValue;
	};

	//set accessor including call the onchange callback
	set value(v: any) {
		if (v !== this.innerValue) {
			this.innerValue = v;
			this.onChangeCallback(v);
		}
	}

	//Set touched on blur
	onBlur() {
		this.onTouchedCallback();
	}

	//From ControlValueAccessor interface
	writeValue(value: any) {
		if (value !== this.innerValue) {
			this.innerValue = value;
		}
	}

	//From ControlValueAccessor interface
	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	//From ControlValueAccessor interface
	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}

}