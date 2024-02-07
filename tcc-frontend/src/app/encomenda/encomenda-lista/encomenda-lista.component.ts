import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { EncomendaService } from 'app/services/encomenda.service';
import { catchError } from 'rxjs';

import * as $ from "jquery"

@Component({
  selector: 'app-encomenda-lista',
  templateUrl: './encomenda-lista.component.html',
  styleUrls: ['./encomenda-lista.component.scss']
})
export class EncomendaListaComponent implements OnChanges {

  @Input() public data: any = [];
  @Input() public filtro: any;
  @Input() public possuiAcessoAcoes: boolean;
  
  @Output() paginacao = new EventEmitter<any>();  

  public dataTable = null;

  constructor(private encomendaService: EncomendaService) { }

  ngOnChanges() {
    console.log("Data: ", this.data)
    this.dataTable = {
      dataRows: this.data
    }
  }

  public atualizarStatus(encomendaId){
    this.encomendaService.atualizarStatus(encomendaId)
      .pipe(
        catchError(error => {
          return error;
        })
      )
      .subscribe(() => {
        this.getEncomendas();
      })
  }

  public getEncomendas() {
    this.encomendaService.getByFiltro(this.filtro)
      .pipe(
        catchError(error => {
          return error;
        })
      )
      .subscribe((response) => {
        response.data.forEach((encomenda: any) => {
          encomenda.statusStr = encomenda.status == true ? 'Ativo' : 'Inativo'
        });

        this.dataTable = {
          dataRows: response.data
        }

        response.data = null;
        this.paginacao.emit(response);
      })
  }

  expandirMenu(id){
    console.log("Id: ", id)
    $("#"+id).slideToggle();
  }
}

