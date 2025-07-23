import { environment } from 'environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Usuarios, Usuario } from 'app/models/usuario';
import { Observable, tap } from 'rxjs';
import { Auth } from 'app/models/auth';

@Injectable({
    providedIn: 'root'
})
export class UsuarioService{

    path = 'Auth/v1';

    constructor(private http: HttpClient, private router: Router) { }

    private getHeader(): HttpHeaders {
        let token = JSON.parse(atob(localStorage.getItem('token')))
        return new HttpHeaders().set("Authorization", `Bearer ${token}`)
    }

    public getUsuarios(): Observable<Usuarios>{
        return this.http.get<Usuarios>(`${ environment.apiUrl}Usuario`,{
            headers: this.getHeader()
        })
    }

    public getUsuariosPorFiltroComposto(usuarioFiltroDto) : Observable<Usuarios>{
        return this.http.post<Usuarios>(`${ environment.apiUrl}Usuario/filtroComposto`, usuarioFiltroDto, {
            headers: this.getHeader()
        })
    }
        public cadastrarUsuario(usuario: Usuario) : Observable<Usuario> {
        return this.http.post<any>(environment.apiUrl + `Usuario`, usuario, {
            headers: this.getHeader()
    })
    }

        public atualizaUsuario(id:number, usuario: Usuario) : Observable<Usuario>{
        return this.http.put<any>(environment.apiUrl + `Usuario/${id
    }`, usuario, {
            headers: this.getHeader()
        })
    }

    public deletaUsuario(id: number): Observable<Usuario>{
        return this.http.delete<any>(environment.apiUrl + `Usuario/${id}`, {
            headers: this.getHeader()
        })
    }

    public atualizaStatusUsuario(usuarioId: number): Observable<Usuario>{
        return this.http.put<Usuario>(`${environment.apiUrl}Usuario/status/${usuarioId}`, {}, {
            headers: this.getHeader()
        })
    }

    autenticar(data: Auth): Observable<any> {
        data.perfil = parseInt(data.perfil)
        return this.http
          .post<any>(environment.apiUrl + this.path + '/login', data)
          .pipe(
            tap((res) => {
              if (res) {
                localStorage.setItem(
                  'token',
                  btoa(JSON.stringify(res['accessToken']))
                );
                localStorage.setItem(
                  'userId',
                  btoa(JSON.stringify(res['id']))
                );
                localStorage.setItem(
                  'nome',
                  btoa(JSON.stringify(res['nome']))
                )
                localStorage.setItem(
                  'perfil',
                  btoa(JSON.stringify(res['perfil']))
                )
                localStorage.setItem(
                  'login',
                  btoa(JSON.stringify(data.login))
                );
                localStorage.setItem(
                  'loginResponse',
                  btoa(JSON.stringify(res))
                )
              }
            })
          );
      }
}
