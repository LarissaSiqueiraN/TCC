import { HttpClient } from '@angular/common/http';
import {
  Injectable
} from '@angular/core';
import { Observable, take } from 'rxjs';
import Swal from 'sweetalert2';

// const apiUrlUsuario = environment.apiUrl + 'Usuario';

const KEY = 'token';

@Injectable({
  providedIn: 'root',
})
export class UtilitariosService {
  constructor(private http: HttpClient) { }

  public exibeAlerta(tipoAlerta: string, mensagem, callBack = null) {
    switch (tipoAlerta) {
      case "info":
        Swal.fire({
          icon: "error",
          showConfirmButton: true,
          title: "Ooops",
          text: mensagem
        }).then(() => {
          if (callBack != null) callBack();
        })
        break;
      case "INFO":
        Swal.fire({
          icon: "info",
          showConfirmButton: true,
          title: "Ooops",
          text: mensagem
        }).then(() => {
          if (callBack != null) callBack();
        })
        break;
      case "SUCESSO":
        Swal.fire({
          icon: "success",
          showConfirmButton: true,
          title: "Sucesso!",
          text: mensagem
        }).then(() => {
          if (callBack != null) callBack();
        })
        break;
      case "AVISO":
        Swal.fire({
          icon: "warning",
          showConfirmButton: true,
          title: "Atenção!",
          text: mensagem
        }).then(() => {
          if (callBack != null) callBack();
        })
        break;
      case "CONFIRMACAO":
        Swal.fire({
          title: "Confirme.",
          text: mensagem,
          icon: "question",
          showDenyButton: true,
          confirmButtonText: "SIM",
          denyButtonText: "NÃO",
        }).then((result) => {
          if (callBack != null) callBack(result);
        })
        break;
    }
  }

  public showLoading() {
    Swal.fire({
      allowOutsideClick: false,
      showConfirmButton: false
    })
    Swal.showLoading();
  }

  public hideLoadding() {
    Swal.hideLoading();
    Swal.clickConfirm();
  }

  public getCepInfo(cep: string): Observable<any> {
    return this.http.get<any>(`https://viacep.com.br/ws/${cep}/json/`).pipe(take(1));
  }

}