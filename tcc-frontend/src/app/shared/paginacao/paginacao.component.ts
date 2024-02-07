import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { Paginacao } from 'app/models/paginacao';

@Component({
    moduleId: module.id,
    selector: 'app-paginacao',
    styleUrls: ["./paginacao.component.css"],
    templateUrl: 'paginacao.component.html'
})

export class PaginacaoComponent implements OnChanges{

    @Input() public paginacao: Paginacao;
    @Input() public ocultarQuantidadeItens: boolean = false;
    @Input() public ocultarTotalRegistros: boolean = false
    public paginas = [];

    @Output() paginacaoAplicada = new EventEmitter<number>();
    @Output() alteracaoItensPagina = new EventEmitter<number>();  

    constructor() 
    {}

    ngOnChanges(){

        if(this.paginacao != null){
            var numeroDePaginas = this.paginacao.qtdPaginas;
            var paginas = [];

            if(numeroDePaginas < 1){
            numeroDePaginas = 1;
            paginas.push(1);
            }else{
            for (let index = 1; index <= numeroDePaginas; index++) {
                paginas.push(index);
            }
            }
        
            this.paginas = paginas;

            console.log("Paginas",this.paginacao)
        }
    }

    public alterarItensPagina(itensPagina){
        this.paginacao.itensPagina = parseInt(itensPagina);
        this.alteracaoItensPagina.emit(parseInt(itensPagina));
    }

    public paginar(pagina){
        this.paginacao.paginaAtual = pagina;
        this.paginacaoAplicada.emit(pagina);
    }
}
