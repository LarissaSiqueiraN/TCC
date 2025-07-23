import { EncomendaRoutes } from './encomenda.routing';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import{ JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { TagInputModule } from 'ngx-chips';
import EncomendaComponent from './encomenda.component';
import { EncomendaListaModule } from './encomenda-lista/encomenda-lista.module';
import { PaginacaoModule } from 'app/shared/paginacao/paginacao.module';

@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(EncomendaRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    TagInputModule,
    JwBootstrapSwitchNg2Module,
    EncomendaListaModule,
    PaginacaoModule
  ],
  declarations: [EncomendaComponent]

})
export class EncomendaModule { }

