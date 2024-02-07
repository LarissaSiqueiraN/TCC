
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from './authentication.service';

declare var $: any;
@Injectable({
    providedIn: 'root',
})

export class AuthenticationGuard implements CanActivate {
    constructor(private router: Router, private authenticationService: AuthenticationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let url = this.getRedirectParam();

        if (this.authenticationService.isAuthenticated()) {

            
            if (this.authenticationService.isAuthorized(state.url.split(";")[0])) {
                console.log("Entrou aqui!!!!")
                return true;
            } else {
                return false;
            }

        }
    }

    private getRedirectParam(): string {
        let urlAtual = window.location.href == undefined ? '' : window.location.href.indexOf('#') != -1 ? window.location.href.split('#')[1] : '';
        return urlAtual == undefined ? '' : urlAtual;
    }
}
