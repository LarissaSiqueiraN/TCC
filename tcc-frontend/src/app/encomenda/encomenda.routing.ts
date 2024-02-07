import { Routes } from '@angular/router';
import EncomendaComponent from './encomenda.component';

export const EncomendaRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: EncomendaComponent,
      },
    ],

}];
