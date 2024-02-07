using AutoMapper;
using Business.Intefaces;
using Business.Services.Base;
using Business.Services.Interfaces;
using DAL.DTOs;
using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private IUsuarioRepository _usuarioRepository;
        private IMapper _mapper;

        public UsuarioService(INotificador notificador, IConfiguration configuration, IUsuarioRepository usuarioRepository,
        IMapper mapper) : base(notificador, configuration)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Usuario>> Buscar()
        {
            IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterTodos();

            return usuarios;
        }
        //public async Task<IEnumerable<ComboIdIntDto>> GetCombo()
        //{
        //    return await _usuarioRepository.GetCombo();
        //}

        //public async Task<Usuario> Adicionar(Usuario novousuario, string usuarioId)
        //{
        //    Usuario usuario = await _usuarioRepository.ObterUsuarioPorId(usuarioId);
        //    Usuario usuario = new Usuario()
        //    {

        //        Nome = novousuario.Nome,

        //        Cpf = novousuario.Cpf,

        //        Rg = novousuario.Rg,

        //        DataNascimento = novousuario.DataNascimento,

        //        Email = novousuario.Email,

        //        ModificadoPor = usuario.Nome,

        //        UltimaModificacao = DateTime.Now

        //    };
        //    await _usuarioRepository.Adicionar(usuario);
            
            
        //    return usuario;
        //}
        //public async Task<Usuario> Atualizar(Usuario usuario, int id, string usuarioId)
        //{
        //    Usuario usuario = await _usuarioRepository.ObterUsuarioPorId(usuarioId);
        //    Usuario usuarioSelecionado = await _usuarioRepository.ObterPorId(id);

        //    usuarioSelecionado.Nome = usuario.Nome;

        //    usuarioSelecionado.Cpf = usuario.Cpf;

        //    usuarioSelecionado.Rg = usuario.Rg;

        //    usuarioSelecionado.DataNascimento = usuario.DataNascimento;

        //    usuarioSelecionado.Email = usuario.Email;

        //    usuarioSelecionado.ModificadoPor = usuario.Nome;

        //    usuarioSelecionado.UltimaModificacao = DateTime.Now;

        //    await _usuarioRepository.update(usuarioSelecionado);

        //    return usuario;
        //}
        //public async Task<Usuario> AtualizaStatus(int id)
        //{
        //    Usuario usuario = await _usuarioRepository.ObterPorId(id);
        //    if(usuario != null)
        //    {
        //        usuario.Ativo = !usuario.Ativo;
        //    }

        //    await _usuarioRepository.update(usuario);

        //    return usuario;
        //}

        public async Task<IEnumerable<Usuario>> Deletar(int id)
        {
            await _usuarioRepository.Remover(id);

            IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterTodos();
            return usuarios;
        }

        //public async Task<IEnumerable<Usuario>> BuscarPorNome(string nome)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorNome(nome);


        //    return usuarios;
        //}

        //public async Task<IEnumerable<Usuario>> BuscarPorCpf(DateTime cpf)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorCpf(cpf);


        //    return usuarios;
        //}

        //public async Task<IEnumerable<Usuario>> BuscarPorRg(string rg)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorRg(rg);


        //    return usuarios;
        //}

        //public async Task<IEnumerable<Usuario>> BuscarPorDataNascimento(string dataNascimento)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorDataNascimento(dataNascimento);


        //    return usuarios;
        //}

        //public async Task<IEnumerable<Usuario>> BuscarPorEmail(string email)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorEmail(email);


        //    return usuarios;
        //}
        //public async Task<IEnumerable<Usuario>> BuscarPorFiltroComposto(UsuarioFiltroDto usuarioFiltroDto)
        //{
        //    IEnumerable<Usuario> usuarios = await _usuarioRepository.ObterPorFiltroComposto(usuarioFiltroDto);

        //    return usuarios;
        //}

    }
}
