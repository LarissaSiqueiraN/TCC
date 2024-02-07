import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormularioComponent } from './formulario.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TagInputModule } from 'ngx-chips';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { RouterModule } from '@angular/router';
import { FormularioRoutes } from './formulario.routing';
import {MatStepperModule} from '@angular/material/stepper';
import { IConfig, NgxMaskModule } from 'ngx-mask';

const maskConfigFunction: () => Partial<IConfig> = () => {
  return {
    validation: false,
  };
};

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forChild(FormularioRoutes),
    FormsModule,
    TagInputModule,
    NgxMaskModule.forRoot(maskConfigFunction),
    JwBootstrapSwitchNg2Module,
    MatStepperModule
    ],
  declarations: [FormularioComponent]
})
export class FormularioModule { }
