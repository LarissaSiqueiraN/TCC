export class ProdutoFiltroDto{
    id: number
    nome: string
    valor: string
    descricao: string
    vendido: boolean
    dataVenda: string
    caminhoImagem: string
}

export type ProdutosFiltroDto = Array<ProdutoFiltroDto>;