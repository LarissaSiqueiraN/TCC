import { environment } from 'environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
@Injectable({
    providedIn: 'root'
})

export class EncomendaService {

    constructor(private http: HttpClient, private router: Router) { }
    
    private getHeader(): HttpHeaders {
        let token = JSON.parse(atob(localStorage.getItem('token')))
        return new HttpHeaders().set("Authorization", `Bearer ${token}`)
    }

    public getByFiltro(filtro): Observable<any>{
        console.log("Filtro: ", filtro)
        return this.http.post<any>(environment.apiUrl  + `Encomenda/GetByFiltro`, filtro, {
            headers: this.getHeader()
        })
    }

    public cadastrarEncomenda(encomenda): Observable<any>{
        return this.http.post<any>(environment.apiUrl + `Encomenda`, encomenda)
    }

    public atualizarStatus(encomendaId) {
        return this.http.put(environment.apiUrl + `Encomenda/Status/${encomendaId}`, {}, {
            headers: this.getHeader()
        })
    }
}
