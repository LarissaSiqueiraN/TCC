import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UsuarioService } from 'app/services/usuario.service';
import { Usuario, Usuarios } from 'app/models/usuario';
import { catchError } from 'rxjs';
import Swal from 'sweetalert2';
declare var $: any;

@Component({
    moduleId: module.id,
    selector: 'app-usuario-cadastro',
    templateUrl: './usuario-cadastro.component.html'
})

export default class UsuarioCadastroComponent implements OnInit
{
    public formularioCadastroUsuario!: FormGroup
    public usuarioExiste: boolean
    public viewOnly: boolean
    @Input() public nome: string = ''
    public nomePlaceholder: string = 'nome'

    @Input() public cpf: Date
    public cpfPlaceholder: string = 'cpf'

    @Input() public rg: string = ''
    public rgPlaceholder: string = 'rg'

    @Input() public dataNascimento: string = ''
    public dataNascimentoPlaceholder: string = 'dataNascimento'

    @Input() public email: string = ''
    public emailPlaceholder: string = 'email'


    public usuario: Usuario = new Usuario()


    @Input() public usuarioId: any

    constructor(private activatedRoute: ActivatedRoute, private router: Router, private usuarioCadastroService: UsuarioService,
       private formBuilder: FormBuilder){ }

    ngOnInit(): void
     {

      if(this.activatedRoute.snapshot.params.usuarioExiste == 'true') {
            this.usuarioExiste = true
      }else{
            this.usuarioExiste = false
      }

      if (this.activatedRoute.snapshot.params.viewOnly == 'true') {
            this.viewOnly = true
      }else{
            this.viewOnly = false
      }

      if (this.usuarioExiste) {
          this.nome = this.activatedRoute.snapshot.params.usuarioNome
          this.nomePlaceholder = this.activatedRoute.snapshot.params.usuarioNome

          this.cpf = this.activatedRoute.snapshot.params.usuarioCpf
          this.cpfPlaceholder = this.activatedRoute.snapshot.params.usuarioCpf

          this.rg = this.activatedRoute.snapshot.params.usuarioRg
          this.rgPlaceholder = this.activatedRoute.snapshot.params.usuarioRg

          this.dataNascimento = this.activatedRoute.snapshot.params.usuarioDataNascimento
          this.dataNascimentoPlaceholder = this.activatedRoute.snapshot.params.usuarioDataNascimento

          this.email = this.activatedRoute.snapshot.params.usuarioEmail
          this.emailPlaceholder = this.activatedRoute.snapshot.params.usuarioEmail

          this.usuarioId = this.activatedRoute.snapshot.params.usuarioId
     }
     this.formularioCadastroUsuario = this.formBuilder.group(
      {
          _nome: [{ value: this.nome, disabled: this.viewOnly }, [Validators.required]],
          _cpf: [{ value: this.cpf, disabled: this.viewOnly }, [Validators.required]],
          _rg: [{ value: this.rg, disabled: this.viewOnly }],
          _dataNascimento: [{ value: this.dataNascimento, disabled: this.viewOnly }, [Validators.required]],
          _email: [{ value: this.email, disabled: this.viewOnly }, [Validators.required]],
      })

}
public salvar()
{
    if (!this.usuarioExiste)
    {
      this.usuario.nome = this.nome
      this.usuario.cpf = this.cpf
      this.usuario.rg = this.rg
      this.usuario.dataNascimento = this.dataNascimento
      this.usuario.email = this.email
      this.usuarioCadastroService.cadastrarUsuario(this.usuario).pipe(
        catchError((err) => {
        let message = 'Erro ao efetuar cadastro de Usuario'
        if (err.error && err.error.errors) message = err.error.errors[0]
            Swal.fire({
        icon: 'error',
            title: 'Oops...',
            text: message
          })

        throw 'Erro no Cadastro: ' + err
        })
      ).subscribe((res) => {
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

      this.router.navigate(['/usuario']);
      })
    }else{
      this.usuario.nome = this.nome
      this.usuario.cpf = this.cpf
      this.usuario.rg = this.rg
      this.usuario.dataNascimento = this.dataNascimento
      this.usuario.email = this.email
      const id = parseInt(this.usuarioId)

      this.usuarioCadastroService.atualizaUsuario(id, this.usuario).pipe(
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
        $.notify({
        message: "Atualização efetuada com sucesso!"
        }, {
        type: 'success',
          timer: 2000,
          placement:
            {
            from: 'top',
            align: 'right'
          }
        });

        this.router.navigate(['/usuario']);
      })
    }
  }
}
