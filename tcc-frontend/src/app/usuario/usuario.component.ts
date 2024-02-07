import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UsuarioService } from 'app/services/usuario.service';
import { Usuario, Usuarios } from 'app/models/usuario';
import { catchError} from 'rxjs';
import { formatDate } from '@angular/common';
import { UsuarioFiltroDto } from 'app/models/DTOs/usuarioFiltroDTOs';

declare var $:any;

declare interface DataTable {
    headerRow: string[];
    dataRows: Usuarios;
}
@Component({
    moduleId: module.id,
    selector: 'app-usuario',
    templateUrl: './usuario.component.html'
})

export default class UsuarioComponent implements OnInit
{
    public usuarios!: Usuarios
    public formulario: FormGroup

    @Input() public valorFiltro: string = ''
    @Input() public valorInputFiltro: string = ''

     public usuarioFiltroDto: UsuarioFiltroDto = new UsuarioFiltroDto()
    public quantidadeFiltros: string = 'Filtrar()'
    public filtros = [];

    public dataTable: DataTable;
    constructor(private activatedRoute: ActivatedRoute, private router: Router, private usuarioService: UsuarioService){ }

    ngOnInit(): void
     {
         this.dataTable = {
             headerRow: [
             'nome',
             'cpf',
             'rg',
             'dataNascimento',
             'email',
             'Status',
             'modificado Por',
             'ultima Modificacao'
             ],
             dataRows: this.usuarios
        }
        this.buscarUsuario()
     }

     ngAfterViewInit(){

        $('#datatables').DataTable({
            'pagingType': 'full_numbers',
            'lengthMenu': [[10, 25, 50, -1], [10, 25, 50, 'All']],
            responsive: true,
            language: {
             search: '_INPUT_',
             searchPlaceholder: 'Search records',
            }
        });
    }
    public async buscarUsuario(){
      this.usuarioService.getUsuarios()
      .pipe(
        catchError(error => {
          return error;
        })
      )
          .subscribe((response: Usuarios) => {
      response.forEach((usuario: Usuario) => {
        usuario.ultimaModificacaoString = formatDate(usuario.ultimaModificacao, "dd/MM/YYYY HH:mm:ss", 'en-US')
        usuario.ativoStr = usuario.ativo == true ? 'Ativo' : 'Inativo'
      });
        this.usuarios = response;
        this.dataTable.dataRows = response;
        $('[rel="tooltip"]').tooltip()
      })
     }

        public async pesquisarPorCampo(filtro: string, valorInputFiltro: string, filtros: any[])
        {
            filtros.forEach((x: string) => {
                if (x.includes(":"))
                {
                    if ((valorInputFiltro == "" && filtro == ""))
                    {
                        let posicaoFiltro = filtros.indexOf(x)
                        filtros.splice(posicaoFiltro)
                    }
                }

                if (x.includes(filtro))
                {
                    let posicaoFiltro = filtros.indexOf(x);
                    filtros.splice(posicaoFiltro, 1)
                }
                else if (x.includes(valorInputFiltro))
                {
                    if (x.includes(filtro))
                    {
                        let posicaoFiltro = filtros.indexOf(x);
                        filtros.splice(posicaoFiltro, 1)
                    }
                }
            })

            let tag = `${ filtro}:${ valorInputFiltro}`
            filtros.push(tag);

            this.quantidadeFiltros = `Filtrar(${ filtros.length})`
            if(filtro == "Nome")this.usuarioFiltroDto.nome = valorInputFiltro
            // if (filtro == "Data")this.usuarioFiltroDto.data =  formatDate(this.usuarioFiltroDto.data, "dd/MM/YYYY HH:mm:ss", 'en-US')

            if(filtro == "Rg")this.usuarioFiltroDto.rg = valorInputFiltro
            if(filtro == "DataNascimento")this.usuarioFiltroDto.dataNascimento = valorInputFiltro
            if(filtro == "Email")this.usuarioFiltroDto.email = valorInputFiltro
      filtro = " " 

     if (filtros.includes(":")
        || filtros.includes("Nome:")
        || filtros.includes("Cpf:")
        || filtros.includes("Rg:")
        || filtros.includes("DataNascimento:")
        || filtros.includes("Email:")
        )
      {
        this.usuarioService.getUsuarios()
        .pipe(
          catchError(error => {
              return error;
          })
        ).subscribe((response: Usuarios) => {
            this.usuarios = response;
            this.dataTable.dataRows = response;
        })
       }
     else
     {
        this.usuarioService.getUsuariosPorFiltroComposto(this.usuarioFiltroDto)
         .pipe(
          catchError(error => {
              return error;
          })
          ).subscribe((response: Usuarios) => {
      response.forEach((usuario: Usuario) => {
        usuario.ultimaModificacaoString = formatDate(usuario.ultimaModificacao, "dd/MM/YYYY HH:mm:ss", 'en-US')
        usuario.ativoStr = usuario.ativo == true ? 'Ativo' : 'Inativo'
      });
            this.usuarios = response;
            this.dataTable.dataRows = response;
        })
     }
    }
    public async atualizaStatusUsuario(usuarioId: number){
      this.usuarioService.atualizaStatusUsuario(usuarioId).subscribe()
    }

    public deletaUsuario(id: number){
    this.usuarioService.deletaUsuario(id).subscribe()

    this.buscarUsuario()
    this.router.routeReuseStrategy.shouldReuseRoute = () => false
    this.router.onSameUrlNavigation = 'reload'
    this.buscarUsuario()
    }

    public Remover(filtro: string){
      let posicao = this.filtros.indexOf(filtro)
      this.filtros.splice(posicao, 1)
      if(filtro.includes("Nome")) this.usuarioFiltroDto.nome = null
      if(filtro.includes("Cpf")) this.usuarioFiltroDto.cpf = null
      if(filtro.includes("Rg")) this.usuarioFiltroDto.rg = null
      if(filtro.includes("DataNascimento")) this.usuarioFiltroDto.dataNascimento = null
      if(filtro.includes("Email")) this.usuarioFiltroDto.email = null
      if(filtro.includes(":") || filtro.includes("Sigla:") || filtro.includes("Nome:")){
        this.usuarioFiltroDto.nome = null
        this.usuarioFiltroDto.cpf = null
        this.usuarioFiltroDto.rg = null
        this.usuarioFiltroDto.dataNascimento = null
        this.usuarioFiltroDto.email = null
      }
      this.quantidadeFiltros = `Filtrar(${ this.filtros.length})`
    }
}
