import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'ruDate' })
export class RuDatePipe implements PipeTransform {

	transform(value: string): string {
		if (!value) {
			return value;
		}
		return value.substr(0, 10);
	}
}