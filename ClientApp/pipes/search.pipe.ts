import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'search'
})
export class SearchPipe implements PipeTransform {

	transform(value: any, sterm: string) {
		if (!value) {
			return value;
		}

		return value.filter((item: any) => item.text.toLowerCase().indexOf(sterm.toLowerCase()) >= 0);
	}
}