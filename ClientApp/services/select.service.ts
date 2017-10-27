import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class SelectService {

	constructor(private http: Http) { }

    getSelectList(typeName: string, parentId: number, term: string): Observable<SelectItem[]> {
        let isGrid = typeName.indexOf('grid-') !== -1;
        if (isGrid) {
            return this.http.get(encodeURI(`/api/${typeName.substr(5)}/select/?parentId=${parentId}&term=${term}`))
                .map(res => res.json());
        }
		return this.http.get(encodeURI('/api/select/' + typeName + '?parentId=' + parentId + '&term=' + term))
			.map(res => res.json());
	}

    getSelectItemId(typeName: string, itemId: number): Observable<SelectItem> {
        let isGrid = typeName.indexOf('grid-') !== -1;
        if (isGrid) {
            return this.http.get(encodeURI(`/api/${typeName.substr(5)}/select/${itemId}`))
                .map(res => res.json());
        }
		return this.http.get('/api/select/' + typeName + '/' + itemId)
			.map(res => res.json());
	}

}

export class SelectItem {
	constructor(public id: number,
		public text: string,
		public sort?: number,
		public group?: SelectGroup) { }
}

export class SelectGroup {
	constructor(public id: number,
		public text: string,
		public sort?: number) { }
}
