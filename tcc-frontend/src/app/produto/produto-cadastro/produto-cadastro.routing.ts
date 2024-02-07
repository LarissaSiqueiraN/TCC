import { Routes } from '@angular/router';
import ProdutoCadastroComponent from './produto-cadastro.component';

export const ProdutoCadastroRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: ProdutoCadastroComponent,
      },
    ],

}];
