import { UsuarioCadastroRoutes } from './usuario-cadastro.routing';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import{ JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import UsuarioCadastroComponent from './usuario-cadastro.component';

@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(UsuarioCadastroRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    JwBootstrapSwitchNg2Module
  ],
  declarations: [UsuarioCadastroComponent]

})
export class UsuarioCadastroModule { }

