import { Routes } from '@angular/router';
import UsuarioCadastroComponent from './usuario-cadastro.component';

export const UsuarioCadastroRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: UsuarioCadastroComponent,
      },
    ],

}];
