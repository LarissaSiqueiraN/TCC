export class Produto
{
    id: number
    nome: string
    valor: number
    descricao: string
    vendido: boolean
    dataVenda: string
    ativo: boolean
    ativoStr: string
    files: FormData
    modificadoPor: string
    ultimaModificacao: Date 
    ultimaModificacaoString: string
}

export type Produtos = Array<Produto>
