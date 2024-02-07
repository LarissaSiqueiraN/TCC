import { ProdutoRoutes } from './produto.routing';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import{ JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { TagInputModule } from 'ngx-chips';
import ProdutoComponent from './produto.component';
import { ProdutoListaModule } from './produto-lista/produto-lista.module';
import { PaginacaoModule } from 'app/shared/paginacao/paginacao.module';

@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(ProdutoRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    TagInputModule,
    JwBootstrapSwitchNg2Module,
    ProdutoListaModule,
    PaginacaoModule
  ],
  declarations: [ProdutoComponent]

})
export class ProdutoModule { }

