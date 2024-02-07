import { Routes, RouterModule } from '@angular/router';
import ProdutoComprarComponent from './produto-comprar.component';

export const ProdutoComprarRoutes: Routes = [{
  path: '',
  children: [
    {
      path: '',
      component: ProdutoComprarComponent
    }
  ]
}]
