import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ShopRoutes } from './shop.routing';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TagInputModule } from 'ngx-chips';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import ShopComponent from './shop.component';
import { ShowcaseModule } from 'app/shared/showcase/showcase.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ShopRoutes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    TagInputModule,
    JwBootstrapSwitchNg2Module,
    ShowcaseModule
  ],
  declarations: [ShopComponent]
})
export class ShopModule { }
