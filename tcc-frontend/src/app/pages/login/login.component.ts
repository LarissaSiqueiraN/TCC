import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UsuarioService } from 'app/services/usuario.service';
import { catchError } from 'rxjs';
import Swal from 'sweetalert2';
import * as _ from 'lodash';

declare var $:any;

@Component({
    moduleId:module.id,
    selector: 'login-cmp',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit{
    test : Date = new Date();

    public loginForm: FormGroup;
    public senhaVisivel: boolean = false;


    loading = false;

    constructor(
        private fb: FormBuilder,
        private userService: UsuarioService,
        private router: Router,
        private route: ActivatedRoute
        ) { console.log(userService)}
        
    checkFullPageBackgroundImage(){
        var $page = $('.full-page');
        var image_src = $page.data('image');

        if(image_src !== undefined){
            var image_container = '<div class="full-page-background" style="background-image: url(' + image_src + '); background-color: red"/>'
            $page.append(image_container);
        }
    };

    ngOnInit(){
        this.checkFullPageBackgroundImage();

        setTimeout(function(){
            // after 1000 ms we add the class animated to the login/register card
            $('.card').removeClass('card-hidden');
        }, 700);

        this.createLoginForm();
    }

    get loginLabel() {
        return 'Usuário';
    }

    createLoginForm() {
        this.loginForm = this.fb.group({
          login: this.fb.control('', [Validators.required]),
          password: this.fb.control('', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]),
          crO_UF: this.fb.control(null)
        });
    }

    getFormControlValidation(formControlName: string) {
        return this.loginForm.get(formControlName).invalid && this.loginForm.get(formControlName).touched;
    }
    
    
    loginSubmit() {

        if (this.loginForm.valid && !this.loading) {
          this.loading = true;
          const data = _.cloneDeep(this.loginForm.value);

          const request = this.userService.autenticar(data);
          request
          .pipe(
            catchError((err) => {
              this.loading = false;
              let message = 'Erro ao efetuar o login, verifique o usuário e senha.';
              if (err.error && err.error.errors) message = err.error.errors[0];
              
              Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: message,
              });
    
              throw 'Erro no Login: ' + err;
            })
          )
          .subscribe((res) => {
            this.loading = false;
    
            $.notify({
              message: "Autenticação efetuada com sucesso!"
            },{
                type: 'success',
                timer: 2000,
                placement: {
                    from: 'top',
                    align: 'right'
                }
            });
    
            this.router.navigate(['']);
          });
        } else {
          this.loginForm.markAllAsTouched();
    
          if (!this.loginForm.valid) {
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: 'Preencha todos os campos corretamente!',
            });
          }
        }
      }

      alternarVisibilidadeSenha() {
        this.senhaVisivel = !this.senhaVisivel;
      }
}
