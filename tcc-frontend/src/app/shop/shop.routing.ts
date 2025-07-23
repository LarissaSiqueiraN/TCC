import { Routes } from '@angular/router';
import ShopComponent from './shop.component';

export const ShopRoutes: Routes = [{
  path: '',
  children: [{
    path: '',
    component: ShopComponent
  }]
}]
