// angular
import { BrowserModule }                    from '@angular/platform-browser';
import { NgModule }                         from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule }                       from '@angular/http';
import { RouterModule }                     from '@angular/router';

// services
import { GridService } from './services/grid.service';

// views
import { App }    from './app.component';
import { Home }   from './views/home/home.view';
import { Paging } from './controls/paging.control';

import { EmployeeChatList }             from './components/employeechat/employeechat.component';

// controls
import { TreeView }       from './controls/treeview.control';
import { Tab, Tabs }      from './controls/tabs.control';
import { SwitchCheckbox } from './controls/switchcheckbox.control';
import { ShadowBox }      from './controls/shadowbox.control';
import { DateTimePicker } from './controls/datetimepicker.control';
import { DropDown }       from './controls/dropdown/dropdown.control';
import { Draggable }      from './controls/draggable.directive';
import { GridRowList }    from './controls/grid/grid.control';


// pipes
import { SearchPipe } from './pipes/search.pipe';
import { RuDatePipe } from './pipes/ruDate.pipe';


import { routing }        from './app.routing';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        RouterModule,
        routing
    ],
    declarations: [
        App
        , Home
        , TreeView
        , DropDown
        , ShadowBox
        , DateTimePicker
        , Tab
        , Tabs
        , SearchPipe
        , RuDatePipe
		, Paging

        , EmployeeChatList
    ],
    providers: [
        GridService
    ],
    bootstrap: [
        App
    ]
})
export class AppModule {

}