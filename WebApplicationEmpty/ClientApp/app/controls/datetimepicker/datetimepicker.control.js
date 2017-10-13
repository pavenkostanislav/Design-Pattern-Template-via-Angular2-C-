var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, forwardRef, ViewChild, ElementRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import 'eonasdan-bootstrap-datetimepicker';
var CUSTOM_VALUE_ACCESSOR = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(function () { return DateTimePicker; }),
    multi: true
};
var DateTimePicker = (function () {
    function DateTimePicker(element) {
        this.element = element;
        this.logDateTimePicker = false;
        this.format = 'DD.MM.YYYY';
        this.locale = 'ru';
        this.disabled = false;
        this.required = false;
        this.innerValue = '';
        this.doEvent = true;
        this.i9d = false;
        this.dpShow = false;
        this.icons = {
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
        this.onChange = function (_) { };
        this.onTouched = function () { };
    }
    Object.defineProperty(DateTimePicker.prototype, "value", {
        get: function () {
            if (this.logDateTimePicker) {
                console.log('datetimepicker.get value: ' + this.innerValue);
            }
            return this.innerValue;
        },
        set: function (v) {
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
        },
        enumerable: true,
        configurable: true
    });
    ;
    DateTimePicker.prototype.ngAfterViewInit = function () {
        if (this.logDateTimePicker) {
            console.log("datetimepicker.ngAfterViewInit format: " + this.format + ", locale: " + this.locale + ", icons: " + this.icons);
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
    };
    DateTimePicker.prototype.ngOnDestroy = function () {
        if (this.logDateTimePicker) {
            console.log('datetimepicker.ngOnDestroy');
        }
        if (this._group.data('DateTimePicker')) {
            this._group.data('DateTimePicker').destroy();
        }
    };
    DateTimePicker.prototype.ngOnChanges = function (changes) {
        if (this.logDateTimePicker) {
            console.log('datetimepicker.ngOnChanges: ' + JSON.stringify(changes));
        }
        if (changes['disabled']) {
            if (!this.disabled && this.i9d) {
                this._group.data('DateTimePicker').enable();
            }
        }
        if (changes['required']) {
            var el = this.element.nativeElement;
            var jqEl = $(el);
            if (this.required) {
                jqEl.attr({ required: null });
            }
            else {
                jqEl.removeAttr('required');
            }
        }
    };
    DateTimePicker.prototype.onShownCalendar = function ($event) {
        if (this.logDateTimePicker) {
            console.log('datetimepicker.onShownCalendar');
        }
        this.dpShow = true;
        this.onTouched();
    };
    DateTimePicker.prototype.onHideCalendar = function ($event) {
        if (this.logDateTimePicker) {
            console.log('datetimepicker.onHideCalendar');
        }
        this.dpShow = false;
    };
    DateTimePicker.prototype.onValueChange = function ($event) {
        if (this.logDateTimePicker) {
            console.log('datetimepicker.onValueChange: ' + ($event.date) + ' doEvent: ' + this.doEvent);
        }
        if (!this.i9d) {
            return;
        }
        if (this.doEvent) {
            var moment = $event.date;
            if (moment) {
                var ret = moment.format(this.format);
                this.onChange(ret);
            }
            else {
                this.onChange(undefined);
            }
        }
    };
    DateTimePicker.prototype.registerOnChange = function (fn) { this.onChange = fn; };
    DateTimePicker.prototype.registerOnTouched = function (fn) { this.onTouched = fn; };
    DateTimePicker.prototype.writeValue = function (val) {
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
    };
    return DateTimePicker;
}());
__decorate([
    ViewChild('group'),
    __metadata("design:type", ElementRef)
], DateTimePicker.prototype, "group", void 0);
__decorate([
    ViewChild('input'),
    __metadata("design:type", ElementRef)
], DateTimePicker.prototype, "input", void 0);
__decorate([
    Input(),
    __metadata("design:type", Object)
], DateTimePicker.prototype, "format", void 0);
__decorate([
    Input(),
    __metadata("design:type", Object)
], DateTimePicker.prototype, "locale", void 0);
__decorate([
    Input(),
    __metadata("design:type", Object)
], DateTimePicker.prototype, "disabled", void 0);
__decorate([
    Input(),
    __metadata("design:type", Object)
], DateTimePicker.prototype, "required", void 0);
DateTimePicker = __decorate([
    Component({
        selector: 'date-time',
        template: "\n\t\t<div #group class='input-group date'>\n\t\t\t<input #input [disabled]=\"disabled\" (focus)=\"onTouched()\" type='text' class=\"form-control\" />\n\t\t\t<span class=\"input-group-addon\">\n\t\t\t\t<span class=\"fa fa-calendar\"></span>\n\t\t\t</span>\n\t\t</div>",
        providers: [CUSTOM_VALUE_ACCESSOR]
    }),
    __metadata("design:paramtypes", [ElementRef])
], DateTimePicker);
export { DateTimePicker };
//# sourceMappingURL=datetimepicker.control.js.map