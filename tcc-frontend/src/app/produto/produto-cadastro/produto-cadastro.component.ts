import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { catchError } from 'rxjs';
import Swal from 'sweetalert2';
import { ProdutoFotoService } from 'app/services/produtoServices/produtoFoto.service';

declare var $: any;

@Component({
  moduleId: module.id,
  selector: 'app-produto-cadastro',
  templateUrl: './produto-cadastro.component.html',
  styleUrls: ['./produto-cadastro.component.scss']
})

export default class ProdutoCadastroComponent implements OnInit {

  public formularioCadastroProduto!: FormGroup

  public produtoExiste: boolean
  public viewOnly: boolean

  public fileUpload: any;
  public fileAlterado: boolean = false;

  public produto: any;

  public produtoId: any

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private produtoCadastroService: ProdutoService,
    private produtoFotoService: ProdutoFotoService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {

    this.createFormulario();

    if (this.activatedRoute.snapshot.params.produtoExiste == 'true') {
      this.produtoExiste = true
    } else {
      this.produtoExiste = false
    }

    if (this.activatedRoute.snapshot.params.viewOnly == 'true') {
      this.viewOnly = true
    } else {
      this.viewOnly = false
    }

    if (this.produtoExiste) {
      this.produtoId = this.activatedRoute.snapshot.params.produtoId
      this.getById(this.produtoId)
    }
  }  

  public getById(produtoId){
    let dto = {
      Id: produtoId
    }

    this.produtoCadastroService.getByFiltro(dto).pipe(
      catchError((err) => {
        let message = 'Erro ao efetuar cadastro de Produto'
        if (err.error && err.error.errors) message = err.error.errors[0]
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: message
        })

        throw 'Erro no Cadastro: ' + err
      })
    ).subscribe((res) => {

      console.log("Res: ", res)

      this.getImagemByProduto(produtoId)

      this.formularioCadastroProduto.get("nome").setValue(res.data[0].nome)
      this.formularioCadastroProduto.get("valor").setValue(res.data[0].valor)
      this.formularioCadastroProduto.get("quantidade").setValue(res.data[0].quantidade)
      this.formularioCadastroProduto.get("descricao").setValue(res.data[0].descricao)
      this.formularioCadastroProduto.get("vendido").setValue(res.data[0].vendido)
      this.formularioCadastroProduto.get("dataVenda").setValue(res.data[0].dataVenda)
      
    })
    
  }

  public getImagemByProduto(produtoId){
    this.produtoFotoService.getImagemByProdutoId(produtoId).pipe(
      catchError((err) => {
        let message = 'Erro ao efetuar buscar imagem de Produto.'
        if (err.error && err.error.errors) message = err.error.errors[0]
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: message
        })

        throw 'Erro no Cadastro: ' + err
      })
    ).subscribe((res) => {
      this.fileUpload = {
        nome: res.nome
      }
    })
  }

  public createFormulario(){
    this.formularioCadastroProduto = this.formBuilder.group(
      {
        nome: [{ value: null, disabled: this.viewOnly }, [Validators.required]],
        valor: [{ value: null, disabled: this.viewOnly }, [Validators.required]],
        descricao: [{ value: '', disabled: this.viewOnly }, [Validators.required]],
        vendido: [{ value: false, disabled: this.viewOnly }],
        dataVenda: [{ value: null, disabled: this.viewOnly }],
        quantidade: [{ value: null, disabled: this.viewOnly }, [Validators.required]]
      })
  }

  public salvar() {
    if (!this.produtoExiste && this.fileUpload != null) {

      this.produto = this.formularioCadastroProduto.value
      this.produto.valor = parseFloat(this.formularioCadastroProduto.get("valor").value)

      this.produtoCadastroService.cadastrarProduto(this.produto).pipe(
        catchError((err) => {
          let message = 'Erro ao efetuar cadastro de Produto'
          if (err.error && err.error.errors) message = err.error.errors[0]
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: message
          })

          throw 'Erro no Cadastro: ' + err
        })
      ).subscribe((res) => {
        console.log("Produto Id: ", res)
        this.cadastrarImagens(res)
      })
    } else if (this.fileUpload != null){

      this.produto = this.formularioCadastroProduto.value
      this.produto.valor = parseFloat(this.formularioCadastroProduto.get("valor").value)

      console.log("Produto atul: ", this.produto)


      const id = parseInt(this.produtoId)

      this.produtoCadastroService.atualizaProduto(id, this.produto).pipe(
        catchError((err) => {
          let message = 'Erro ao efetuar atualização'
          if (err.error && err.error.errors) message = err.error.errors[0]

          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: message
          })

          throw 'Erro na Atualização: ' + err
        })
      ).subscribe((res) => {

        if(this.fileAlterado){
          this.atualizarImagem(this.produtoId);
        }else{
          $.notify({
            message: 'Atualização efetuado com sucesso!'
          }, {
            type: 'success',
            timer: 2000,
            placement:
            {
              from: 'top',
              align: 'right'
            }
          });
    
          this.router.navigate(['/produto']);
        }
        
      })
    }else{
      Swal.fire({
        icon: 'error',
        title: 'Campos Incorretos!',
        text: "Preencha todos os campos corretamente."
      })
    }
  }

  private atualizarImagem(produtoId){
    let files = $('input[type=file]')[0].files[0];
    let formData = new FormData();
    formData.append("imagens", files)

    this.produtoFotoService.atualizarImagem(produtoId, formData).pipe(
      catchError(err => {
        let message = "Erro ao atualizar imagem do produto";
        if (err.error && err.error.errors) message = err.error.errors[0]
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: message
        })
        throw 'Erro no Cadastro: ' + err
      })
    ).subscribe(() => {

      $.notify({
        message: 'Atualização efetuado com sucesso!'
      }, {
        type: 'success',
        timer: 2000,
        placement:
        {
          from: 'top',
          align: 'right'
        }
      });

      this.router.navigate(['/produto']);
    })
  }

  private cadastrarImagens(id: number) {
    let files = $('input[type=file]')[0].files[0];
    let formData = new FormData();
    formData.append("imagens", files)

    this.produtoFotoService.cadastrarImagens(formData, id).pipe(
      catchError(err => {
        let message = "Erro ao cadastrar imagem do produto";
        if (err.error && err.error.errors) message = err.error.errors[0]
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: message
        })
        throw 'Erro no Cadastro: ' + err
      })
    ).subscribe(() => {

      $.notify({
        message: 'Cadastro efetuado com sucesso!'
      }, {
        type: 'success',
        timer: 2000,
        placement:
        {
          from: 'top',
          align: 'right'
        }
      });

      this.router.navigate(['/produto']);
    })
  }

  public atualizarArquivo(event) {
    this.fileAlterado = true;

    var file = event.target.files;

    let index = file[0].type.indexOf("/");
    let extensao = file[0].type.substring(index + 1);

    this.fileUpload = {
      nome: file[0].name,
      extensao: extensao
    }
  }

  public removerArquivo() {
    this.fileUpload = null;
  }
}