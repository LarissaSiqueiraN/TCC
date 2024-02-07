export class EncomendaFiltroDto {
    nome: string;
    email: string;
    cep: string;
    status: boolean;
    pagina: number = 1;
    itensPagina: number = 10;
}
