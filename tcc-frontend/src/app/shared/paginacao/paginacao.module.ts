import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PaginacaoComponent } from './paginacao.component';

@NgModule({
    imports: [ RouterModule, CommonModule ],
    declarations: [ PaginacaoComponent ],
    exports: [ PaginacaoComponent ]
})

export class PaginacaoModule {}
