import { Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormularioComponent } from './formulario.component';

export const FormularioRoutes = [{
  path: '',
  children: [{
    path: '',
    component: FormularioComponent
  }
  ]
}
]
