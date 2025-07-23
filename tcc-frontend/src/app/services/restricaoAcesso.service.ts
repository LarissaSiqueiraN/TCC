import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class RestricaoAcessoService {

  constructor() { }

  public getPerfil() {
    console.log("Perfil: ", localStorage.getItem('perfil') )
    return localStorage.getItem('perfil');
  }

}
