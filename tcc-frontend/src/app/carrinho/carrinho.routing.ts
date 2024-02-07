import { Routes } from '@angular/router';
import { CarrinhoComponent } from './carrinho.component';
import { FormularioComponent } from './formulario/formulario.component';

export const CarrinhoRoutes: Routes = [{
    path: '',
    children: [
      {
        path: '',
        component: CarrinhoComponent,
      },
      {
        path: 'finalizandoCompra',
        component: FormularioComponent
      }
    ],

}];
