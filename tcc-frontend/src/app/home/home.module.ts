import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TagInputModule } from 'ngx-chips';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import HomeComponent from './home.component';
import { RouterModule } from '@angular/router';
import { HomeRoutes } from './home.routing';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(HomeRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    TagInputModule,
    JwBootstrapSwitchNg2Module
  ],
  declarations: [HomeComponent]
})
export class HomeModule { }
