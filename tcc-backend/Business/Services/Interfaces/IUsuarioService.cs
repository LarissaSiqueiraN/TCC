using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IUsuarioService
    {

        Task<IEnumerable<Usuario>> Buscar();

        //Task<IEnumerable<ComboIdIntDto>> GetCombo();

        //Task<Usuario> Adicionar(Usuario novousuario, string usuarioId);

        //Task<Usuario> Atualizar(Usuario usuario, int id, string usuarioId);

        //Task<Usuario> AtualizaStatus(int id);

        Task<IEnumerable<Usuario>> Deletar(int id);

        //Task<IEnumerable<Usuario>> BuscarPorNome(string nome);

        //Task<IEnumerable<Usuario>> BuscarPorCpf(DateTime cpf);

        //Task<IEnumerable<Usuario>> BuscarPorRg(string rg);

        //Task<IEnumerable<Usuario>> BuscarPorDataNascimento(string dataNascimento);

        //Task<IEnumerable<Usuario>> BuscarPorEmail(string email);

        //Task<IEnumerable<Usuario>> BuscarPorFiltroComposto(UsuarioFiltroDto clienteFiltroDto);
    }
}
