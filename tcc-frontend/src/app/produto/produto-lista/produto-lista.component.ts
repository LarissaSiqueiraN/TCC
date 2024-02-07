import { ProdutoFotoService } from 'app/services/produtoServices/produtoFoto.service';
import { UtilitariosService } from './../../services/utilitarios.service';
import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-produto-lista',
  templateUrl: './produto-lista.component.html',
  styleUrls: ['./produto-lista.component.scss']
})
export default class ProdutoListaComponent implements OnChanges {

  @Input() public data: any;
  @Input() public filtro: any;

  @Output() paginacao = new EventEmitter<any>();

  public dataTable = null;
  public produtos: any = [];

  constructor(
    private produtoService: ProdutoService,
    private ProdutoFotoService: ProdutoFotoService,
    private utilitariosService: UtilitariosService
  ) { }

  ngOnChanges() {
    console.log("Data: ", this.data)

    this.dataTable = {
      dataRows: this.data
    }
  }

  public deletaProduto(id: number) {
    this.utilitariosService.showLoading();


    Swal.fire({
      icon: "warning",
      showCancelButton: true,
      showConfirmButton: true,
      title: "Confirmação de exclusão.",
      text: "Confirme o para excluir o campo."
    }).then((res) => {

      if (res.isConfirmed) {
        this.produtoService.deletaProduto(id)
        .pipe(
          catchError(error => {
            this.utilitariosService.hideLoadding()

            Swal.fire({
              icon: "error",
              title: "Erro ao deletar produto",
              text: "O produto tem alguma encomenda pendente vinculada."
            })

            return error;
          })
        )
        .subscribe(() => {
          this.getProdutos()
          this.deletarDiretorioImagemByProduto(id)
        })

        this.utilitariosService.hideLoadding()
      }
    })
  }

  public deletarDiretorioImagemByProduto(id) {
    this.ProdutoFotoService.deletarDiretorioImagemByProduto(id)
      .pipe(
      catchError(error => {
        this.utilitariosService.hideLoadding()
        return error;
      })
    )
      .subscribe()
  }

  public getProdutos() {

    this.produtoService.getByFiltro(this.filtro)
      .pipe(
        catchError(error => {
          this.utilitariosService.hideLoadding()
          return error;
        })
      )
      .subscribe((response: any) => {


        response.data.forEach((produto: any) => {
          produto.ultimaModificacaoString = formatDate(produto.ultimaModificacao, "dd/MM/YYYY HH:mm:ss", 'en-US')
          produto.ativoStr = produto.ativo == true ? 'Ativo' : 'Inativo'
        });

        this.dataTable = {
          dataRows: response.data
        }

        response.data = null;
        this.paginacao.emit(response);
      })
  }
}
