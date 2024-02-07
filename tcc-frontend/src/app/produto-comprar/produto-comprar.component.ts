import { CarrinhoService } from '../services/carrinho.service';
import { ProdutoFotoService } from 'app/services/produtoServices/produtoFoto.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';

declare var $: any;

@Component({
  selector: 'app-produto-comprar',
  templateUrl: './produto-comprar.component.html',
  styleUrls: ['./produto-comprar.component.scss']
})

export default class ProdutoComprarComponent implements OnInit {

  public produtoId: number;
  public produto: any;
  public caminhoImagem: any;

  constructor(private activatedRoute: ActivatedRoute,
    private produtoService: ProdutoService,
    private router: Router,
    private produtoFotoService: ProdutoFotoService,
    private CarrinhoService: CarrinhoService) { }

  ngOnInit() {
    this.produtoId = this.activatedRoute.snapshot.params.id;

    this.getImagemByProduto(this.produtoId);
    this.getInformacoesCompraProduto(this.produtoId);
  }

  private getImagemByProduto(produtoId) {
    this.produtoFotoService.getImagemByProduto(produtoId)
      .subscribe((caminhoImagem: any) => {
        this.caminhoImagem = "assets/img/produtoFoto/" + caminhoImagem.produtoId + "/" + caminhoImagem.nome
      })
  }

  private getInformacoesCompraProduto(produtoId: number) {
    this.produtoService.getInformacoesCompraByProduto(produtoId)
      .pipe(
        catchError(error => {
          return error;
        })
      ).subscribe((res) => {
        this.produto = res;
      });
  }

  public adicionarAoCarrinho(produtoId: number) {

    console.log("Produto: ", this.produto)
   
    if(this.produto.quantidade > 0){
      this.CarrinhoService.adicionarProduto(produtoId);
      $.notify({
        message: 'Produto adicionado ao carrinho!'
      }, {
        type: 'success',
        timer: 2000,
        placement:
        {
          from: 'top',
          align: 'right'
        }
      });

      this.produto.quantidade--;
    }else{
      $.notify({
        message: 'Produto fora do estoque.'
      }, {
        type: 'info',
        timer: 2000,
        placement:
        {
          from: 'top',
          align: 'right'
        }
      });

      this.router.navigate(['/shop']);
    }

  }
}
