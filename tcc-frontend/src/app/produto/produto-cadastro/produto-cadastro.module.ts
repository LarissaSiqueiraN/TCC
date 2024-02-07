import { ProdutoCadastroRoutes } from './produto-cadastro.routing';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import{ JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import ProdutoCadastroComponent from './produto-cadastro.component';
import { IConfig, NgxMaskModule } from 'ngx-mask';

const maskConfigFunction: () => Partial<IConfig> = () => {
  return {
    validation: false,
  };
};

@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(ProdutoCadastroRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    JwBootstrapSwitchNg2Module,
    NgxMaskModule.forRoot(maskConfigFunction)
  ],
  declarations: [ProdutoCadastroComponent]

})
export class ProdutoCadastroModule { }

