import { Pipe, Injectable, PipeTransform } from '@angular/core';

@Pipe({
	name: 'filter',
	pure: false
})
@Injectable()
export class FilterPipe implements PipeTransform {
	transform(items: any[], args: any[]): any {
		return items.filter(i => i.visible == true);
	}
}