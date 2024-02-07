import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnChanges } from '@angular/core';
import { ProdutoFiltroDto } from 'app/models/DTOs/produtoFiltroDTOs';
import { CarrinhoService } from 'app/services/carrinho.service';

declare var $: any;

@Component({
  selector: 'app-showcase',
  templateUrl: './showcase.component.html',
  styleUrls: ['./showcase.component.scss']
})

export default class ShowcaseComponent implements OnChanges {

  @Input() public produto: any

  public cardIndisponivel: boolean = false
  public mostrarNome: boolean = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private carrinhoService: CarrinhoService) { }

    ngOnChanges() {
      if(this.produto.quantidade == 0){
        this.cardIndisponivel = true
      } else{
        let produtosCarrinho = this.carrinhoService.getProdutosById(this.produto.id)

        if(this.produto.quantidade <= produtosCarrinho.length){
          this.cardIndisponivel = true
        }
      }
    }

  getClick(){
  }
}
