import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable, Subscriber } from 'rxjs/Rx';

@Injectable()
export class GridService {

	public controllerName: string;

	constructor(private http: Http) { }

    // Grid
    getGridRequestModel(requestModel: GridRequestModel) : Observable<GridResponseModel> {
		if ( !requestModel ) {
			return null;
		}
		let body = JSON.stringify( requestModel );
		let headers = new Headers ( {
            'Content-Type': 'application/json'
        });
        return this.http.post(`/api/${this.controllerName}/list`, body, { headers: headers }).map(
            (res: any) => {
                if (res.status && res.status == 200) {
                    return res.json();
                }
                else {
                    return false;
                }
            }
        );
    }

    getGridList(parentId: number) {
        if (parentId) {
            return this.http.get(`/api/${this.controllerName}/list/${parentId}`).map((res: any) => res.json());
        }
        else {
            return this.http.get(`/api/${this.controllerName}/list?${parentId}`).map((res: any) => res.json());
        }
	}

	getGridRowModel(id: number, mode: string) {
		return this.http.get(`/api/${this.controllerName}/${id}?mode=${mode}`).map((res: any) => res.json());
	}

	saveGridRowModel(model: any) {

		let body = JSON.stringify(model);
		let headers = new Headers({
			'Content-Type': 'application/json'
		});
		return this.http.post(`/api/${this.controllerName}`, body, { headers: headers }).map(
			(res: any) => {
				if (res.status && res.status == 200) {
					return res.json();
				}
				else {
					return false;
				}
			}
		);
	}

	deleteGridRowModel(id: number): Observable<boolean> {
		if (!id) {
			return Observable.of(false);
		}

		return this.http.delete(`/api/${this.controllerName}/${id}`).map((res: any) => res.ok);
	}
}

export class GridRequestModel {
    public currentPage: number = 0;
    public pageSize: number = 15;
    public keyId: number;
	public findModel: any;

}

export class GridResponseModel {
	public tableId: number;
	public countPage: number;
	public currentPage: number;
    public totalRowCount: number;
    public list: any[];
}
