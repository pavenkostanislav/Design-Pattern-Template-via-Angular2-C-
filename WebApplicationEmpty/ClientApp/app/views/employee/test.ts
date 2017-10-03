import { Component } from '@angular/core';
import { ActivatedRoute, Params, Data, Router } from '@angular/router';
import { Location } from '@angular/common';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';
import { DomSanitizer } from '@angular/platform-browser';

import { GridRowList } from './controls/grid/grid.control';



@Component({
    templateUrl: 'test.html',
    declarations: [GridRowList]
})
export class Test implements {
    constructor() { }
}