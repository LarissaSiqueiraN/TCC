import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class CarrinhoService {

  public listaProdutos: number[] = []

  constructor() {
    const carrinho = localStorage.getItem('carrinho');

    if (carrinho) {
      let lista = JSON.parse(carrinho);
      
      lista.forEach(valor => {
        this.listaProdutos.push(parseInt(valor))
      })
    
    }
   }

   public getProdutos(){
     const carrinho = localStorage.getItem('carrinho');

      this.listaProdutos = []

      let lista = JSON.parse(carrinho);
            
      if(lista != null){
        lista.forEach(valor => {
          this.listaProdutos.push(parseInt(valor))
        })
      
        return this.listaProdutos;
      }else{
        return null;
      }

   }

   public getProdutosById(produtoId){
    const carrinho = localStorage.getItem('carrinho');
    this.listaProdutos = []

    let lista = JSON.parse(carrinho);

    if(lista != null){
      lista.forEach(valor => {
        if(produtoId == parseInt(valor)){
          this.listaProdutos.push(parseInt(valor))
        }
      })
    
      return this.listaProdutos;
    }else{
      return null;
    }
   }

   public adicionarProduto(produtoId: number){
      this.listaProdutos.push(produtoId);

      localStorage.setItem('carrinho', JSON.stringify(this.listaProdutos));
   }

   public tirarProdutoCarrinhoById(produtoId){
     let indexProduto = this.listaProdutos.findIndex(x => x === produtoId);

     this.listaProdutos.splice(indexProduto, 1)

     localStorage.setItem('carrinho', JSON.stringify(this.listaProdutos));
   }
}