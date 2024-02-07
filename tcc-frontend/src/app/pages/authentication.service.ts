import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RestricaoAcessoService } from 'app/services/restricaoAcesso.service';

declare var $: any;

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {

    constructor(
        private _route: Router,
        private _activatedRoute: ActivatedRoute,
        private restricaoAcessoService: RestricaoAcessoService) {
    }

    logout(): boolean {
        localStorage.clear();
        setTimeout(() => { this._route.navigate(['pages/login']); }, 500);
        return true;
    }

    isAuthenticated(): boolean {
        return this.restricaoAcessoService.getPerfil() != null;
    }

    isAuthorized(url): boolean {
        let perfil = this.restricaoAcessoService.getPerfil();

        if (this.getPerfilRoutes(perfil).filter(p => p == url).length > 0) return true;
        else return false;
    }

    private getPerfilRoutes(perfil) {
        let listaRoutes = [
            "/shop", 
            "/carrinho",
            "/carrinho/finalizandoCompra",
            "/dashboard",
            "/pages/login"];

        if(perfil != null){
            listaRoutes = [
                "/encomendas", 
                "/produto", 
                "/produto-cadastro", 
                "/shop", 
                "/carrinho",
                "/carrinho/finalizandoCompra",
                "/dashboard",
                "/pages/login"];
        }

        return listaRoutes;
    }
}
