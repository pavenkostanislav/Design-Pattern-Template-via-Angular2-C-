import { Routes } from '@angular/router';

import { UserService } from '../../services/user.service';
import { EmployeeList } from './employee.list.view';
import { EmployeeEdit } from './employee.edit.view';
import { Test } from './test.view';

export const employeeRoutes: Routes = [
    { path: 'employee', component: EmployeeList, canActivate: [UserService], data: { objectname: 'Employee' } },
    { path: 'employee/edit/:id', component: EmployeeEdit, canActivate: [UserService], data: { objectname: 'Employee' } },
    { path: 'employee/edit/:id/:mode', component: EmployeeEdit, canActivate: [UserService], data: { objectname: 'Employee' } },
    { path: 'employee/lk', component: Test, canActivate: [UserService], data: { objectname: 'Employee' } },
    { path: 'employee/lk/:id', component: Test, canActivate: [UserService], data: { objectname: 'Employee' } }
];