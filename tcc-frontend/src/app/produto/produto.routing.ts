import { Routes } from '@angular/router';
import ProdutoComponent from './produto.component';
import ProdutoCadastroComponent from './produto-cadastro/produto-cadastro.component';

export const ProdutoRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: ProdutoComponent,
      },
      {
        path: 'cadastro',
        component: ProdutoCadastroComponent
      }
    ],

}];
