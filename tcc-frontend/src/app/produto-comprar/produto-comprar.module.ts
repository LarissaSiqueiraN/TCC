import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { NgxMaskModule } from 'ngx-mask';
import ProdutoComprarComponent from './produto-comprar.component';
import { ProdutoComprarRoutes } from './produto-comprar.routing';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ProdutoComprarRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    JwBootstrapSwitchNg2Module,
    NgxMaskModule.forRoot()
  ],
  declarations: [ProdutoComprarComponent]
})
export class ProdutoComprarModule { }
