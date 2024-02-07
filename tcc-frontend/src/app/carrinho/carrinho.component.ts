import { Component, OnInit } from '@angular/core';
import { CarrinhoService } from 'app/services/carrinho.service';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-carrinho',
  templateUrl: './carrinho.component.html',
  styleUrls: ['./carrinho.component.scss']
})

export class CarrinhoComponent implements OnInit {

  constructor(
    private carrinhoService: CarrinhoService,
    private produtoService: ProdutoService) { }

  public dataTable = null;
  public valorTotal: number = 0;
  public produtos: any;

  ngOnInit() {
    this.getProdutosCarrinho();
  }

  public getProdutosCarrinho() {
    let produtosId = this.carrinhoService.getProdutos();

    console.log("Produtos id: ", produtosId)

    if (produtosId != null) {
      this.produtoService.getProdutosByIds(produtosId)
        .pipe(
          catchError(error => {
            return error;
          })
        ).subscribe((response: any) => {
          console.log("Res: ", response)

          this.produtos = response

          this.dataTable = {
            dataRows: response
          }

          this.somarProdutos();

        })
    } else {
      this.dataTable = {
        dataRows: []
      }
    }

  }

  public somarProdutos() {
    console.log("ENtrouu")
    console.log("This: ", this.produtos)

    this.produtos.forEach(produto => {
      console.log("Valor: ", produto.valor)
      this.valorTotal += produto.valor;
      console.log("Valor total: ", this.valorTotal)
    })
  }

  public tirarProdutoLista(produtoId) {
    this.carrinhoService.tirarProdutoCarrinhoById(produtoId);
    this.valorTotal = 0
    this.getProdutosCarrinho();
  }

  public setValorTotal(){
    
    localStorage.setItem("valorTotal", this.valorTotal.toString())
  }
} 