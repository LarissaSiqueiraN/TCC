import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth/auth-layout.component';
import { AuthenticationGuard } from './pages/authentication.guard';

export const AppRoutes: Routes = [{
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
}, {
    path: '',
    component: AdminLayoutComponent,
    children: [{
        path: '',
        loadChildren: () => import('./dashboard/dashboard.module').then(x => x.DashboardModule)
    },{
        path: 'produto',
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./produto/produto.module').then(x => x.ProdutoModule)
    }, 
    {
        path: 'encomendas',
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./encomenda/encomenda.module').then(x => x.EncomendaModule)
    }, {
        path: "shop",
        loadChildren: () => import('./shop/shop.module').then(x => x.ShopModule)
    }, {
        path: "carrinho",
        loadChildren: () => import('./carrinho/carrinho.module').then(x => x.CarrinhoModule)
    },
    {
        path: "produtoSelecionado",
        loadChildren: () => import('./produto-comprar/produto-comprar.module').then(x => x.ProdutoComprarModule)
    }
    ]
}, {
    path: '',
    component: AuthLayoutComponent,
    children: [{
        path: 'pages',
        loadChildren: () => import('./pages/pages.module').then(x => x.PagesModule)
    }]
}
];
