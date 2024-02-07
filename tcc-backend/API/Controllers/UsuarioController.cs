using Business.Intefaces;
using Business.Services.Interfaces;
using GestaoDePlantao.Controllers.Base;
using GestaoDePlantao.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(INotificador notificador,
                                    ILogger<AuthController> logger,
                                    IUsuarioService usuarioService) : base(notificador)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarUsuario()
        {
            try
            {
                return CustomResponse(await _usuarioService.Buscar());
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao buscar usuarios", _logger);
                return CustomResponse();
            }
        }

        //[HttpGet("combo")]
        //public async Task<ActionResult> GetCombo()
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.GetCombo());
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar getCombos de divisão", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> CadastrarUsuario(Usuario usuario)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            NotificarErro("Dado de cadastro do usuario estão incorretos ou incompletos");
        //            return CustomResponse();
        //        }
        //        string usuarioId = GetUsuarioId();
        //        return CustomResponse(await _usuarioService.Adicionar(usuario, usuarioId));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao cadastrar usuario", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> AtualizarUsuario(Usuario usuario, int id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            NotificarErro("Dado de cadastro do funcionario estão incorretos ou incompletos");
        //            return CustomResponse();
        //        }
        //        string usuarioId = GetUsuarioId();
        //        return CustomResponse(await _usuarioService.Atualizar(usuario, id, usuarioId));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Erro ao atualizar usuario", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpPut("status/{id}")]
        //public async Task<ActionResult> AtualizaStatusUsuario(int id)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.AtualizaStatus(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao atualizar status de usuario", _logger);
        //        return CustomResponse();
        //    }
        //}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarUsuario(int id)
        {
            try
            {
                return CustomResponse(await _usuarioService.Deletar(id));
            }
            catch (Exception ex)
            {
                GravaException(ex, "Falha ao deletar usuario", _logger);
                return CustomResponse();
            }
        }

        //[HttpGet("nome/{filtro}")]
        //public async Task<ActionResult> BuscarUsuarioPorNome(string filtro)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorNome(filtro));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por nome ", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpGet("cpf/{filtro}")]
        //public async Task<ActionResult> BuscarUsuarioPorCpf(DateTime filtro)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorCpf(filtro));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por cpf ", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpGet("rg/{filtro}")]
        //public async Task<ActionResult> BuscarUsuarioPorRg(string filtro)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorRg(filtro));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por rg ", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpGet("dataNascimento/{filtro}")]
        //public async Task<ActionResult> BuscarUsuarioPorDataNascimento(string filtro)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorDataNascimento(filtro));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por dataNascimento ", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpGet("email/{filtro}")]
        //public async Task<ActionResult> BuscarUsuarioPorEmail(string filtro)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorEmail(filtro));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por email ", _logger);
        //        return CustomResponse();
        //    }
        //}

        //[HttpPost("filtroComposto")]
        //public async Task<ActionResult> BuscarUsuarioPorFiltroComposto(UsuarioFiltroDto usuarioFiltroDto)
        //{
        //    try
        //    {
        //        return CustomResponse(await _usuarioService.BuscarPorFiltroComposto(usuarioFiltroDto));
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaException(ex, "Falha ao buscar usuario(s) por filtro composto", _logger);
        //        return CustomResponse();
        //    }
        //}
    }
}
