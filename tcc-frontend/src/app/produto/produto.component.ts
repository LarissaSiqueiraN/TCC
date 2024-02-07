import { UtilitariosService } from './../services/utilitarios.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';
import { formatDate } from '@angular/common';
import { Paginacao } from 'app/models/paginacao';

declare var $: any;

@Component({
  moduleId: module.id,
  selector: 'app-produto',
  templateUrl: './produto.component.html',
  styleUrls: ['./produto.component.scss']
})

export default class ProdutoComponent implements OnInit {
  public produtos: any = []
  public formulario: FormGroup

  public valorFiltro: string = ''
  public valorInputFiltro: string = ''

  public filtro: any = {}
  public quantidadeFiltros: number = null
  public filtros = [];

  public paginacao: Paginacao

  constructor(
    private produtoService: ProdutoService,
    private utilitariosService: UtilitariosService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.createFormulario()
    this.getProdutos(this.filtro)
  }

  private createFormulario() {
    this.formulario = this.formBuilder.group({
      _valorFiltro: [{ value: null, disabled: false }, [Validators.required]],
      _valorInputFiltro: [{ value: null, disabled: false }, [Validators.required]]
    })
  }


  public pesquisarPorCampo() {
    let valorInputFiltro = this.formulario.controls['_valorInputFiltro'].value;
    let filtro = this.formulario.controls['_valorFiltro'].value;
    
    this.filtros.forEach((x) => {
      if (x.key == '' || x.value == '') {
        let posicaoFiltro = this.filtros.indexOf(x);
        this.filtros.splice(posicaoFiltro);
      }

      if (x.key.includes(filtro)) {
        let posicaoFiltro = this.filtros.indexOf(x);
        this.filtros.splice(posicaoFiltro, 1);
      }
      else if (x.value.includes(valorInputFiltro)) {
        if (x.key.includes(filtro)) {
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
    if (filtro == "Vendido") this.filtro.vendido = Boolean(valorInputFiltro)
    if (filtro == "DataVenda") this.filtro.dataVenda = valorInputFiltro

    filtro = " "

    this.getProdutos(this.filtro)
    this.limparInput()

  }

  public limparInput(){
    this.formulario.controls['_valorInputFiltro'].setValue(null)
  }

  public getProdutos(filtro) {
    this.utilitariosService.showLoading()

    console.log("Filtro")

    this.produtoService.getByFiltro(filtro)
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

        this.produtos = response.data;
        response.data = null;
        this.paginacao = response;
        this.utilitariosService.hideLoadding();
    })
  }

  public atualizaStatusProduto(produtoId: number) {
    this.produtoService.atualizaStatusProduto(produtoId).subscribe()
  }

  public Remover(filtro: any) {
    let posicao = this.filtros.indexOf(filtro)
    this.filtros.splice(posicao, 1)

    if (filtro.key.includes("Nome")) this.filtro.nome = null
    if (filtro.key.includes("Valor")) this.filtro.valor = null
    if (filtro.key.includes("Descricao")) this.filtro.descricao = null
    if (filtro.key.includes("Vendido")) this.filtro.vendido = null
    if (filtro.key.includes("DataVenda")) this.filtro.dataVenda = null
    
    this.quantidadeFiltros = this.filtros.length
    this.getProdutos(this.filtro);
  }

  public paginacaoAplicada(pagina) {
    this.filtro.pagina = pagina;
    this.getProdutos(this.filtro);
  }

  public alteracaoItensPagina(itensPagina: number) {
    this.filtro.itensPagina = itensPagina;
    this.getProdutos(this.filtro);
  }

  public setPaginacao(value){
    this.paginacao = value;
  }
}
