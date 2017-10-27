import { ModuleWithProviders }          from '@angular/core';
import { Routes, RouterModule }         from '@angular/router';

import { Home }                         from './views/home/home.view';

export const appRoutes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
	{ path: 'home', component: Home, data: { objectname: 'home', currentUser: null } }
];

export const routing = RouterModule.forRoot(appRoutes);