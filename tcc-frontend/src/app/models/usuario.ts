export class Usuario
{
    id: number
    nome: string
    cpf: Date
    rg: string
    dataNascimento: string
    email: string
    ativo: boolean
    ativoStr: string
    modificadoPor: string
    ultimaModificacao: Date 
    ultimaModificacaoString: string
}

export type Usuarios = Array<Usuario>
