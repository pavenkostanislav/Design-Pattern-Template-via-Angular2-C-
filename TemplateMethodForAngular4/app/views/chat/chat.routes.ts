import { Routes } from '@angular/router';

import { GridRowList } from '../../controls/grid/grid.control.ts';
import { Test } from './test';

export const testRoutes: Routes = [
    { path: 'grid/test/:id', component: GridRowList, canActivate: [UserService], data: { objectname: 'Test' } } }
];