import { Routes } from '@angular/router';
import UsuarioComponent from './usuario.component';

export const UsuarioRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: UsuarioComponent,
      },
    ],

}];
