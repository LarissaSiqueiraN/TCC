export class Encomenda
{
    id: number
    fK_Produto: number
    quantidade: number
    codigo: string
    ativo: boolean
    ativoStr: string
    modificadoPor: string
    ultimaModificacao: Date 
    ultimaModificacaoString: string
}

export type Encomendas = Array<Encomenda>
