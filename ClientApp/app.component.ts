import * as ng from '@angular/core';
import { Response } from '@angular/http';
import { Observable, Subject, BehaviorSubject } from 'rxjs/Rx';

import { SelectService } from './services/select.service';


import '../style/app.css';

let logApp = false;

export enum GridSize {
	xs,
	sm,
	md,
	lg,
	xl
}

@ng.Component({
    selector: 'app',
    templateUrl: 'app.html',
    providers: [SelectService]
})
export class App {

	private _ok = new Subject<boolean>();
	private _gridSize = new BehaviorSubject<GridSize>(GridSize.lg);

	GridSize = this._gridSize.asObservable();

    attachmentMetaObjectId: number;
    attchmentObjectId: number;

    docmailingMetaObjectId: number;
    docmailingtObjectId: number;

    UpdateApp: ng.EventEmitter<any> = new ng.EventEmitter<any>();

    // коллекция обработчиков событий
    private appEventHandlers: AppEventHandler[] = [];

    constructor() { }

    ngOnInit() {

        if (logApp) {
            console.log('App.OnInit');
        }

		this.initGridSize(window.innerWidth);
	}

	@ng.HostListener('window:resize', ['$event'])
	onResize(event: any) {
		if (logApp) {
			console.log('App.onResize', event);
		}
		this.initGridSize(event.target.innerWidth);
	}

	initGridSize(width: number) {

		if (width < 768) {
			this._gridSize.next(GridSize.xs);
		}
		else if (width < 992) {
			this._gridSize.next(GridSize.sm);
		}
		else if (width < 1400) {
			this._gridSize.next(GridSize.md);
		}
		else if (width < 1920) {
			this._gridSize.next(GridSize.lg);
		}
		else {
			this._gridSize.next(GridSize.xl);
		}
    }

    RefreshApp() {
        this.UpdateApp.emit();
    }

	/**
	 * Показывает модальное окно с сообщением об ошибке
	 * @param errorMessage - сообщение об ошибке (string, Response)
	 */
    showError(errorMessage: any) {

        if (!errorMessage) {
            return;
        }
        console.log('app.showError: ', errorMessage);

        if (errorMessage.constructor === Response) {
            let resp: Response = errorMessage;
            if (resp.text()) {
                let err = resp.json();
                if (err.show) {
                    $('#errorText').html(err.text);
                    $('#errorModal').modal('show');
                }
                if (err.description) {
                    $('#errorText').html(err.description);
                    $('#errorModal').modal('show');
                }
                if (err.wrong) {
                    let errText = '';
                    err.wrong.forEach((rr: any) => errText = errText + '<br/> ' + rr);
                    $('#errorText').html(errText);
                    $('#errorModal').modal('show');
                }
            }
        }
        else if (typeof (errorMessage) == 'string') {
            $('#errorText').html(<string>errorMessage);
            $('#errorModal').modal('show');
        }
        else {

            if (errorMessage._body) {
                let err = <Error>JSON.parse(errorMessage._body);
                if (err.show) {
                    $('#errorText').html(err.text);
                    $('#errorModal').modal('show');
                }
                if (err.description) {
                    $('#errorText').html(err.description);
                    $('#errorModal').modal('show');
                }
            }

        }

        return false;

    }

	/**
	 * Показывает модальное окно с сообщением.
	 * @param messageClass - цвет окна: default, warning, danger, success, info
	 * @param messageHeader - текст в заголовке окна
	 * @param messageBode - текст в теле окна
	 */
    showMessage(messageClass: string, messageHeader: string, messageBode: string) {


        $('#messageModal-label').html(messageHeader);
        $('#messageText').html(messageBode);
        $('#messageContent').addClass('alert-' + messageClass);

        $('#messageModal').modal('show');

    }

    onQuestCancelClick() {

        if (!this._ok.isStopped) {
            this._ok.next(false);
            this._ok.complete();
        }
    }

    onQuestOkClick() {

        if (!this._ok.isStopped) {
            this._ok.next(true);
            this._ok.complete();
        }
    }

	/**
	 * Показывает модальное окно с вопросам.
	 * @param questText - текст вопроса
     * @param positiveAnswer - по умолчанию да
     * @param negativeAnswer - по умолчанию нет
	 */
    showQuestion(questText: string, positiveAnswer: string = 'Да', negativeAnswer: string = 'Нет', closeWithNegative: boolean = true): Observable<boolean> {

        this._ok = new Subject<boolean>();

        $('#questText').html(questText);
        $('#positiveAnswer').html(positiveAnswer);
        $('#negativeAnswer').html(negativeAnswer);
        $('#questModal').modal('show');
        if (closeWithNegative) {
            $('#questModal').on('hide.bs.modal', this.onQuestCancelClick.bind(this));
        }

        return this._ok.asObservable();

    }

	/**
	 * Добавить обработчика событий
	 * @param name - имя события
	 * @param call - обработчик
	 */
    addAppEventHandler(name: string, call: (param?: string) => void): AppEventHandler {

        if (logApp) {
            console.log('App.addAppEventHandler(name, call):', name, call);
        }
        if (!name || !call) {
            return;
        }

        let h = new AppEventHandler();
        h.name = name;
        h.call = call;

        this.appEventHandlers.push(h);

        return h;
    }

	/**
	 * Удалить обработчик событий
	 * @param handler - обработчик
	 */
    removeAppEventHandler(handler: AppEventHandler) {

        if (logApp) {
            console.log('App.removeAppEventHandler(handler):', handler);
        }
        if (!handler) {
            return;
        }

        let index = this.appEventHandlers.indexOf(handler, 0);
        if (index > -1) {
            this.appEventHandlers.splice(index, 1);
        }
    }

	/**
	 * Запустить событие
	 * @param name - имя события
	 * @param param - параметры события
	 */
    raiseAppEvent(name: string, param?: string) {

        if (!this.appEventHandlers) {
            return;
        }

        if (logApp) {
            console.log('App.raiseAppEvent(name, param):', name, param);
        }

        this.appEventHandlers.filter(e => e.name == name).forEach(h => {

            if (h.call) {
                if (param) {
                    h.call(param);
                }
                else {
                    h.call();
                }
            }
        });
    }

    convertToXML(columns: any, rows: Array<any>) {

        let row;
        let col;
        let xml;

        let header = `<?xml version="1.0"?>\n
           <ss:Workbook xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">\n
           <ss:Worksheet ss:Name="Sheet1">\n
           <ss:Table>\n`;

        let headerRow = '<ss:Row>\n';

        for (let colName in columns) {
            headerRow += '<ss:Cell>\n';
            headerRow += '<ss:Data ss:Type="String">';
            headerRow += columns[colName].name + '</ss:Data>\n';
            headerRow += '</ss:Cell>\n';
        }

        headerRow += '</ss:Row>\n';

        let footer = `</ss:Table>\n
           </ss:Worksheet>\n
           </ss:Workbook>\n`;


        xml = header + headerRow;

        for (row = 0; row < rows.length; row++) {

            xml += '<ss:Row>\n';

            for (col in columns) {

                let val = this.escapeCharacters(rows[row][col]);
                let valType = columns[col].type;
                if (valType == 'Boolean') {
                    valType = 'String';
                    if (val) {
                        val = 'Да';
                    }
                    else {
                        val = 'Нет';
                    }
                }

                xml += '  <ss:Cell>\n';
                xml += '    <ss:Data ss:Type="' + valType + '">';
                xml += val + '</ss:Data>\n';
                xml += '  </ss:Cell>\n';
            }

            xml += '</ss:Row>\n';
        }

        return xml + footer;

    }

    escapeCharacters(val: string) {

        if (!val) {
            return '';
        }

        if (typeof (val) != 'string') {
            return val;
        }

        return val.replace('&', '&amp;')
            .replace('<', '&lt;')
            .replace('>', '&gt;')
            .replace('"', '&quot;')
            .replace('\'', '&apos;');
    }

}

export class Error {
    show: boolean;
    text: string;
    description: string;
}

export class AppEventHandler {
    name: string;
    call: (param?: string) => void;
}

export class ColumnProperties {
    name: string;
    type: string;
    displayName: string;
}


