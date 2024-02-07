import { UsuarioRoutes } from './usuario.routing';
import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import{ JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { TagInputModule } from 'ngx-chips';
import UsuarioComponent from './usuario.component';

@NgModule({

  imports: [
    CommonModule,
    RouterModule.forChild(UsuarioRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    TagInputModule,
    JwBootstrapSwitchNg2Module
  ],
  declarations: [UsuarioComponent]

})
export class UsuarioModule { }

