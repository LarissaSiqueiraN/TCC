import { ProdutoFotoService } from './../services/produtoServices/produtoFoto.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProdutosFiltroDto } from 'app/models/DTOs/produtoFiltroDTOs';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})

export default class ShopComponent implements OnInit {

  public formulario: FormGroup

  public valorFiltro: string = ''
  public valorInputFiltro: string = ''

  public tipoFiltro: string = null

  public filtros = [];
  public filtro: any = {}
  public quantidadeFiltros: number = null

  public produtos: ProdutosFiltroDto = []

  constructor(
    private produtoService: ProdutoService,
    private produtoFotoService: ProdutoFotoService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getProdutos()
    this.createFormulario()
  }

  public createFormulario(){
    this.formulario = this.formBuilder.group(
      {
        _valorFiltro: [{ value: null, disabled: false }, [Validators.required]],
        _valorInputFiltro: [{ value: null, disabled: false}, [Validators.required]]
      })
  }

  public Remover(filtro: any) {
    let posicao = this.filtros.indexOf(filtro)
    this.filtros.splice(posicao, 1)

    if (filtro.key.includes("Nome")) this.filtro.nome = null
    if (filtro.key.includes("Valor")) this.filtro.valor = null
    if (filtro.key.includes("Descricao")) this.filtro.descricao = null
    
    this.quantidadeFiltros = this.filtros.length
    this.getProdutos();
  }

  public pesquisarPorCampo() {
    let valorInputFiltro = this.formulario.controls['_valorInputFiltro'].value;
    let filtro = this.formulario.controls['_valorFiltro'].value;

    this.filtros.forEach((x) => {
      if (x.key == '' || x.value == '') {
        let posicaoFiltro = this.filtros.indexOf(x);
        this.filtros.splice(posicaoFiltro);
      }

      if (x.key === filtro) {
        let posicaoFiltro = this.filtros.indexOf(x);
        this.filtros.splice(posicaoFiltro, 1);
      }
      else if (x.value ===valorInputFiltro) {
        if (x.key === filtro) {
          let posicaoFiltro = this.filtros.indexOf(x);
          this.filtros.splice(posicaoFiltro, 1)
        }
      }
    })

    let novoFiltro = {
      key: filtro,
      value: valorInputFiltro,
      label: valorInputFiltro
    }

    this.filtros.push(novoFiltro);
    this.quantidadeFiltros = this.filtros.length;

    if (filtro == "Nome") this.filtro.nome = valorInputFiltro
    if (filtro == "Valor") this.filtro.valor = valorInputFiltro
    if (filtro == "Descricao") this.filtro.descricao = valorInputFiltro
    
    filtro = " "

    this.getProdutos()
    this.limparInput()
  }

  public limparInput(){
    this.formulario.controls['_valorInputFiltro'].setValue(null)
  }

  private getProdutos() {
    this.produtoService.getByFiltro(this.filtro)
      .pipe(
        catchError(error => {
          return error;
        })
      ).subscribe((response: any) => {

        console.log("Respo: ", response)

        response.data.forEach((produto: any) => {

          this.produtoFotoService.getImagemByProduto(produto.id)
            .subscribe((caminhoImagem: any) => {
              produto.caminhoImagem = "assets/img/produtoFoto/" + caminhoImagem.produtoId + "/" + caminhoImagem.nome
            })

        });

        this.produtos = response.data;
      })
  }
}
