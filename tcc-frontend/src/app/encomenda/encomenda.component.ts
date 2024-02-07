import { UtilitariosService } from 'app/services/utilitarios.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EncomendaService } from 'app/services/encomenda.service';
import { Encomendas } from 'app/models/encomenda';
import { catchError } from 'rxjs';
import { Paginacao } from 'app/models/paginacao';

declare var $: any;

@Component({
  moduleId: module.id,
  selector: 'app-encomenda',
  templateUrl: './encomenda.component.html',
  styleUrls: ['./encomenda.component.scss']
})

export default class EncomendaComponent implements OnInit {

  public encomendas: Encomendas = [];
  public formulario: FormGroup;

  public filtro: any = {}
  public filtros = [];

  public quantidadeFiltros: number = 0;
  public paginacao: Paginacao;

  public tipoFiltro: string = null

  constructor(
    private encomendaService: EncomendaService,
    private formBuilder: FormBuilder,
    private utilitariosService: UtilitariosService) { }

  ngOnInit(): void {
    this.createFormulario();
    this.getEncomendas();
  }

  public getEncomendas() {
    this.utilitariosService.showLoading();
    console.log("Filtro: ", this.filtro)
    this.encomendaService.getByFiltro(this.filtro)
      .pipe(
        catchError(error => {
          this.utilitariosService.hideLoadding();
          return error;
        })
      )
      .subscribe((response) => {

        response.data.forEach((encomenda: any) => {
          encomenda.statusStr = encomenda.status == true ? 'Ativo' : 'Inativo'
        });

        this.encomendas = response.data;
        response.data = null;
        this.paginacao = response;
        this.utilitariosService.hideLoadding();
      })
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
        let posicaoFiltro = this.filtros.indexOf(x)
        this.filtros.splice(posicaoFiltro)
      }

      if (x.key.ifiltro) {
        let posicaoFiltro = this.filtros.indexOf(x);
        this.filtros.splice(posicaoFiltro, 1)
      }
      else if (x.value === valorInputFiltro) {
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

    this.quantidadeFiltros = this.filtros.length

    if (filtro == "Nome") this.filtro.nome = valorInputFiltro
    if (filtro == "Email") this.filtro.email = valorInputFiltro
    if (filtro == "Cep") this.filtro.cep = valorInputFiltro
    if (filtro == "Status") this.filtro.status = valorInputFiltro

    filtro = ""

    this.quantidadeFiltros = this.filtros.length
    this.getEncomendas()
    this.limparInput()
  }

  public Remover(filtro) {
    let posicao = this.filtros.indexOf(filtro)
    this.filtros.splice(posicao, 1)

    if (filtro.key == "Nome") this.filtro.nome = null
    if (filtro.key == "Email") this.filtro.email = null
    if (filtro.key == "Cep") this.filtro.cep = null
    if (filtro.key == "Status") this.filtro.status = null

    this.quantidadeFiltros = this.filtros.length

    this.getEncomendas()
  }

  public paginacaoAplicada(pagina) {
    this.filtro.pagina = pagina;
    this.getEncomendas();
  }

  public alteracaoItensPagina(itensPagina: number) {
    this.filtro.itensPagina = itensPagina;
    this.getEncomendas();
  }

  public setPaginacao(value) {
    this.paginacao = value;
  }

  public limparInput(){
    this.formulario.controls['_valorInputFiltro'].setValue(null)
  }
}
