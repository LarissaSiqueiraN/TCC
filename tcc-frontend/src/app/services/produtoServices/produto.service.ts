import { environment } from 'environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Produtos, Produto } from 'app/models/produto';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class ProdutoService {

    constructor(private http: HttpClient, private router: Router) { }
    
    private getHeader(): HttpHeaders {
        let token = JSON.parse(atob(localStorage.getItem('token')))
        return new HttpHeaders().set("Authorization", `Bearer ${token}`)
    }

    public getProdutos(): Observable<Produtos> {
        return this.http.get<Produtos>(`${environment.apiUrl}Produto`, {
        })
    }

    public getProdutosPorEncomenda(id: number): Observable<Produtos> {
        return this.http.get<Produtos>(`${environment.apiUrl}Produto/Encomenda/${id}`, {
            headers: this.getHeader()
        })
    }

    public getByFiltro(filtro): Observable<any> {
        console.log("Filtro: ", filtro)
        return this.http.post<any>(`${environment.apiUrl}Produto/GetByFiltro`, filtro)
    }

    public getProdutosByIds(produtoIds): Observable<any>{
        return this.http.post<any>(`${environment.apiUrl}Produto/GetProdutosByIds`, produtoIds)
    }

    public cadastrarProduto(produto): Observable<any> {
        return this.http.post<any>(environment.apiUrl + `Produto`, produto,{
            headers: this.getHeader()
        })
    }

    public atualizaProduto(id: number, produto: Produto): Observable<Produto> {
        return this.http.put<any>(environment.apiUrl + `Produto/${id
            }`, produto, {
            headers: this.getHeader()
        })
    }

    public deletaProduto(id: number): Observable<Produto> {
        return this.http.delete<any>(environment.apiUrl + `Produto/${id}`, {
            headers: this.getHeader()
        })
    }

    public atualizaStatusProduto(produtoId: number): Observable<Produto> {
        return this.http.put<Produto>(`${environment.apiUrl}Produto/status/${produtoId}`, {}, {
            headers: this.getHeader()
        })
    }

    public getInformacoesCompraByProduto(produtoId: number): Observable<any>{
        return this.http.get<any>(`${environment.apiUrl}Produto/GetInformacoesCompraByProduto/${produtoId}`);
    }

    public descontarQuantidadeProduto(dto){
        console.log("Dto: ", dto)
        return this.http.post<any>(environment.apiUrl + `Produto/DescontarQuantidadeProduto`, dto)
    }
}
