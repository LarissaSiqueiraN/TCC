using Business.Intefaces;
using Business.Services.Base;
using Business.Services.ProdutoServices.Interfaces;
using DAL.DTOs;
using DAL.DTOs.ProdutosDto;
using DAL.Models.ProdutoModels;
using DAL.Repository.ProdutoRepositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services.ProdutoServices
{
    public class ProdutoFotoService : BaseService, IProdutoFotoService
    {
        private ILogger<ProdutoService> _logger;
        private IProdutoFotoRepository _produtoFotoRepository;

        public ProdutoFotoService(INotificador notificador, 
                                    IConfiguration configuration, 
                                    ILogger<ProdutoService> logger, 
                                    IProdutoFotoRepository produtoFotoRepository) : base(notificador, configuration)
        {
            _logger = logger;
            _produtoFotoRepository = produtoFotoRepository;
        }

        public async Task<ProdutoFotoRotaDto> GetImagensByProduto(int produtoId)
        {
            string nomeImagem = _produtoFotoRepository.Buscar(x => x.FK_Produto == produtoId).Result.Select(x => x.NomeArquivo).FirstOrDefault();

            return new ProdutoFotoRotaDto()
            {
                ProdutoId = produtoId,
                Nome = nomeImagem
            };
        }

        public async Task<ProdutoFotoCaminhoDto> GetImagemByProduto(int produtoId)
        {
            return await _produtoFotoRepository.GetImagemByProduto(produtoId);
        }


        public async Task<string> GetImagensByFiltro(int produtoId)
        {
            try
            {
                return _produtoFotoRepository.Buscar(x => x.FK_Produto == produtoId).Result.Select(x => x.NomeArquivo).FirstOrDefault();
            }
            catch(Exception ex) 
            {
                Notificar(ex, "Ocorreu um erro em ProdutoFotoService: GetImagensByFiltro", _logger);
                throw ex;
            }
        }

        public async Task AdicionarFoto(AdicionarFotoViewModel model)
        {
            try
            {
                _produtoFotoRepository.IniciaTransacao();

                var path = $"{_configuration["AppSettings:Caminhos:FotoProduto"]}/{model.ProdutoId}";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var nomeArquivoPasta = $"{Guid.NewGuid()}_{model.Files[0].FileName}";
                var caminhoCompleto = path + "/" + nomeArquivoPasta;
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    model.Files[0].CopyTo(stream);
                }

                var fileName = model.Files[0].FileName.Split('.')[0];

                var produtoFoto = new ProdutoFoto
                {
                    NomeArquivo = nomeArquivoPasta,
                    DisplayeName = fileName,
                    Extensao = model.Files[0].FileName.Split('.').Last(),
                    FK_Produto = model.ProdutoId
                };

                await _produtoFotoRepository.Adicionar(produtoFoto);

                _produtoFotoRepository.CommitTransacao();
            }
            catch (Exception ex)
            {
                Notificar(ex, "Ocorreu um erro em ProdutoFotoService: AdicionarFoto", _logger);
                throw ex;
            }
        }

        public async Task DeletarDiretorioImagemByProduto(int produtoId)
        {
            string filePath = $"{_configuration["AppSettings:Caminhos:FotoProduto"]}/{produtoId}";

            if (Directory.Exists(filePath))
            {
                this.DeletarArquivosByDiretorio(filePath);
                Directory.Delete(filePath);
            }
        }

        private async Task DeletarArquivosByDiretorio(string diretorio)
        {
            foreach (string arquivo in Directory.GetFiles(diretorio))
            {
                File.Delete(arquivo);
            }
        }

        public async Task DeletarImagemByProduto(int produtoId)
        {
            ProdutoFotoCaminhoDto dto = await _produtoFotoRepository.GetImagemByProduto(produtoId);
            await _produtoFotoRepository.Remover(dto.Id);

            string filePath = $"{_configuration["AppSettings:Caminhos:FotoProduto"]}/{produtoId}/{dto.Nome}";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
