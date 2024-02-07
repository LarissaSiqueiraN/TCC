import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "environments/environment";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class ProdutoFotoService {

    constructor(private http: HttpClient, private router: Router) { }

    private getHeader(): HttpHeaders {
        let token = JSON.parse(atob(localStorage.getItem('token')))
        return new HttpHeaders().set("Authorization", `Bearer ${token}`)
    }

    public cadastrarImagens(formData: any, produtoId: number): Observable<any>{
        return this.http.post<any>(`${environment.apiUrl}ProdutoFoto/?produtoId=${produtoId}`,formData,{
            headers: this.getHeader()
        })
    }

    public atualizarImagem(produtoId, formData): Observable<any>{
        return this.http.put<any>(`${environment.apiUrl}ProdutoFoto/?produtoId=${produtoId}`, formData, {
            headers: this.getHeader()
        })
    }

    public getImagemByProduto(produtoId: number): Observable<any>{
        return this.http.get<any>(`${environment.apiUrl}ProdutoFoto/${produtoId}`, {
        }) 
    }

    public getImagemByProdutoId(produtoId): Observable<any>{
        return this.http.get<any>(`${environment.apiUrl}ProdutoFoto/GetImagemByProduto/${produtoId}`, {
            headers: this.getHeader()
        })
    }

    public deletarDiretorioImagemByProduto(produtoId){
        console.log("ENTROU")
        console.log("Produto Id: ", produtoId)
        return this.http.delete(`${environment.apiUrl}ProdutoFoto/${produtoId}`, {
            headers: this.getHeader()
        })
    }
}