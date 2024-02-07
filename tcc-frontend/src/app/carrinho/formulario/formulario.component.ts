import { ProdutoService } from 'app/services/produtoServices/produto.service';
import { Component, OnInit } from '@angular/core';
import swal from 'sweetalert2';
import { Validators, FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { EncomendaService } from 'app/services/encomenda.service';
import { catchError } from 'rxjs';
import Swal from 'sweetalert2';
import { CarrinhoService } from 'app/services/carrinho.service';
import { UtilitariosService } from 'app/services/utilitarios.service';
import { Router } from '@angular/router';

declare var $: any;

@Component({
  moduleId: module.id,
  selector: 'app-formularioComra',
  styleUrls: ['./formulario.component.scss'],
  templateUrl: './formulario.component.html'
})

export class FormularioComponent implements OnInit {

  public formulario!: FormGroup

  public validLocalFuro = false

  public dataTable = {
    dataRows: []
  }
  public valorTotal: number = 0;

  type: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private encomendaService: EncomendaService,
    private carrinhoService: CarrinhoService,
    private produtoService: ProdutoService,
    private utilitarioService: UtilitariosService,
    private router: Router) { }

  // isFieldValid(form: FormGroup, field: string) {
  //   return !form.get(field).valid && form.get(field).touched;
  // }

  // displayFieldCss(form: FormGroup, field: string) {
  //   return {
  //     'has-error': this.isFieldValid(form, field),
  //     'has-success': this.isFieldValid(form, field)
  //   };
  // }

  ngOnInit() {

    this.getProdutosCarrinho()
    this.createForm()
    this.setEnderecoNaoDomicilo()

    // you can also use the nav-pills-[blue | azure | green | orange | red] for a different color of wizard
    // Code for the Validato

    var $validator = $('.card-wizard form').validate({
      rules: {
        primeiroNome: {
          minlength: 3
        },
        segundoNome: {
          minlength: 3
        },
        telefoen: {
        },
        idade: {
        },
        furouAntes: {
        },
        localFuro: {
        },
        cep: {
        },
        bairro: {
        },
        rua: {
        },
        numero: {
        },
        complemento: {
        },
        email: {
        }
      },

      // highlight: function (element) {
      //   $(element).parent().addClass('has-error').removeClass('has-success');
      // },
      // success: function (element) {
      //   $(element).parent().addClass('has-success').removeClass('has-error');
      // }
    });

    $('#wizardCard').bootstrapWizard({
      tabClass: 'nav nav-pills',
      nextSelector: '.btn-next',
      previousSelector: '.btn-back',
      lastSelector: '.btn-finish',

      onNext: function (tab, navigation, index) {
        var $valid = $('.card-wizard form').valid();

        var email = $("#email").val()
        var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        if (
          emailRegex.test(email)) {
          $validator.focusInvalid();
          return false;
        }

      },
      onInit: function (tab, navigation, index) {

        //check number of tabs and fill the entire row
        var $total = navigation.find('li').length;
        var $width = 100 / $total;

        var $display_width = $(document).width();

        if ($display_width < 600 && $total > 3) {
          $width = 50;
        }

        // navigation.find('li').css('width',$width + '%');
      },
      onTabClick: function (tab, navigation, index) {
        // Disable the posibility to click on tabs
        return false;
      },
      onTabShow: function (tab, navigation, index) {
        var $total = navigation.find('li').length;
        var $current = index + 1;

        var wizard = navigation.closest('.card-wizard');

        // If it's the last tab then hide the last button and show the finish instead
        if ($current >= $total) {
          $(wizard).find('.btn-next').hide();
          $(wizard).find('.btn-finish').show();
        } else if ($current == 1) {
          $(wizard).find('.btn-back').hide();
        } else {
          $(wizard).find('.btn-back').show();
          $(wizard).find('.btn-next').show();
          $(wizard).find('.btn-finish').hide();
        }
      },
      onLast: function (tab, navigation, index) {

        var email = $("#email").val()
        var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        if (emailRegex.test(email)) {
        } else {
          swal.fire("Email incorreto!", "Por favor, insira um email.", "warning")
        }
        //here you can do something, sent the form to server via ajax and show a success message with swal
      }

    });
  }

  onFinishWizard() {
    //here you can do something, sent the form to server via ajax and show a success message with swal

    this.formulario.get("valorTotal").setValue(this.valorTotal);

    let encomenda = this.formulario.value
    this.setProdutosId(encomenda)

    
    this.utilitarioService.showLoading()


    this.encomendaService.cadastrarEncomenda(encomenda).pipe(
      catchError((err) => {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: err.error.errors[0]
        })

        throw err;
      })
    ).subscribe(() => {

      console.log("Produtos: ", encomenda.produtos)
      
      let ids = {fk_Produto: []}

      encomenda.produtos.forEach(res => {
        ids.fk_Produto.push(res.fk_Produto)
      })

      this.produtoService.descontarQuantidadeProduto(ids).pipe(
        catchError((err) => {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: err.error.errors[0]
          })

          throw err;
        })
      ).subscribe()


      this.utilitarioService.hideLoadding()
      localStorage.removeItem("carrinho");

      Swal.fire({
        icon: "success",
        showConfirmButton: true,
        title: "Formulario finalizado!",
        text: "O formulário foi entregue com sucesso."
      }).then((res) => {
        if (res.isConfirmed || res.isDismissed) {
          this.router.navigate(['/']);
        }
      })

    })
  }

  private setProdutosId(encomenda) {
    encomenda.produtos = []

    this.dataTable.dataRows.forEach(res => {
      let data = { fk_Produto: res.id }
      encomenda.produtos.push(data)
    })
    
  }

  private createForm() {
    this.formulario = this.formBuilder.group(
      {
        nome: [{ value: null, disabled: false }, [Validators.required]],
        telefone: [{ value: null, disabled: false }, [Validators.required]],
        idade: [{ value: null, disabled: false }, [Validators.required]],
        faz5DiasQueVacinou: [{ value: false, disabled: false }],
        temAlergia: [{ value: false, disabled: false }],
        alergias: [{ value: null, disabled: false }],
        furouAntes: [{ value: true, disabled: false }, [Validators.required]],
        jaFurouAntes: [{ value: null, disabled: false }],
        localFuro: [{ value: null, disabled: false }, [Validators.required]],
        noDomicio: [{ value: false, disabled: false }],
        cep: [{ value: null, disabled: false }],
        bairro: [{ value: null, disabled: false }],
        rua: [{ value: null, disabled: false }],
        numero: [{ value: null, disabled: false }],
        complemento: [{ value: null, disabled: false }],
        pagamento: [{ value: "debito", disabled: false }],
        valorTotal: [{ value: null, disabled: false }],
        email: [{ value: null, disabled: false }, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]],
        adicionarServicoFuro: [{ value: false, disabled: false }]
      })
  }

  public getProdutosCarrinho() {
    let produtosId = this.carrinhoService.getProdutos();

    if (produtosId != null) {
      this.produtoService.getProdutosByIds(produtosId)
        .pipe(
          catchError(error => {
            return error;
          })
        ).subscribe((response: any) => {

          this.dataTable = {
            dataRows: response
          }

          console.log("Datatable: ", this.dataTable)

          this.somarProdutos()

        })
    } else {
      this.dataTable = {
        dataRows: []
      }
    }

  }

  public somarProdutos() {
    this.dataTable.dataRows.forEach(produto => {
      this.valorTotal += produto.valor;
    })
  }

  public changeDomicilio(value) {
    console.log("Value: ", value)

    if (value) {
      this.formulario.get("cep").setValue(null);
      this.formulario.get("bairro").setValue(null);
      this.formulario.get("rua").setValue(null);
      this.formulario.get("numero").setValue(null);
      this.formulario.get("complemento").setValue(null);
    } else {
      this.setEnderecoNaoDomicilo();
    }
  }

  public setEnderecoNaoDomicilo() {
    this.formulario.get("cep").setValue("12240-470");
    this.formulario.get("bairro").setValue("Jardim das Indústrias");
    this.formulario.get("rua").setValue("Rua Estefania do Nascimento");
    this.formulario.get("numero").setValue(268);
    this.formulario.get("complemento").setValue("");
  }

  public carregarCep(value) {
    if (value.length == 9) {
      this.utilitarioService.showLoading();
      this.utilitarioService.getCepInfo(value).subscribe(res => {
        if (res.erro) {
          this.utilitarioService.hideLoadding()
          return
        };

        this.formulario.get("bairro").setValue(res.bairro);
        this.formulario.get("rua").setValue(res.logradouro);
        this.formulario.get("complemento").setValue(res.complemento);

        this.utilitarioService.hideLoadding();
      })
    }
  }

  public changeAdicionarServicoFuro(value) {
    if (value) {

      if (this.dataTable.dataRows.length > 0) {

        if (this.formulario.get("idade").value < 4) {
          this.valorTotal = 129.99
        } else {
          this.valorTotal = 69.99
        }

        if (this.dataTable.dataRows.length > 1) {
          var produtoPromocao = this.dataTable.dataRows.filter((valor, indice) => indice !== 0);

          produtoPromocao.forEach(data => {
            this.valorTotal += data.valor
          })

          this.arredondarValor(this.valorTotal)
        }

      }
      else {
        if (this.formulario.get("idade").value < 4) {
          this.valorTotal = 129.99
        } else {
          this.valorTotal = 39.99
        }
      }

    } else {

      if (this.dataTable.dataRows.length > 0) {
        this.valorTotal = 0;

        this.dataTable.dataRows.forEach(data => {
          this.valorTotal += data.valor
        })
      } else {
        if (this.formulario.get("idade").value < 4) {
          this.valorTotal -= 129.99
        } else {
          this.valorTotal -= 39.99
        }
      }

    }

  }

  public arredondarValor(valor) {
    let parteDecimal = valor - Math.floor(valor);

    if (parteDecimal > 0.8) {
      this.valorTotal = (this.valorTotal - parteDecimal) + 0.99;
    }
  }

  public changeValid(value) {
    this.validLocalFuro = value != null ? true : false;
  }

  public abrirInformacaoValores() {
    swal.fire({
      width: 900,
      imageUrl: '../../../assets/img/valoresFuros.png',
      imageHeight: 400,
      imageWidth: 900,
      imageAlt: 'Tabela de valores'
    })
  }

  public abrirInformacaoFuros() {
    swal.fire({
      imageUrl: '../../../assets/img/partesDaOrelha.png',
      imageHeight: 400,
      imageWidth: 500,
      imageAlt: 'Partes da orelha'
    })
  }
}

