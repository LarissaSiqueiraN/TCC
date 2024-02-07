using DAL.DTOs;
using DAL.Models;
using DAL.Models.EncomendaModels;
using DAL.Repository.Base;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class EncomendaRepository : Repository<Encomenda>, IEncomendaRepository
   {
        public EncomendaRepository(BancoAPIContext db) : base(db)
        {
        }

        public async Task<Encomenda> GetById(int id)
        {
            return await Db.Encomendas.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
      
        public async Task<ResultadoPaginado<EncomendaReadDto>> GetByFiltro(EncomendaFiltroDto dto)
        {
            var query = Db.Encomendas.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Nome))
                query = query.Where(x => x.Nome.Contains(dto.Nome));
            if (!string.IsNullOrEmpty(dto.Email))
                query = query.Where(x => x.Email.Contains(dto.Email));
            if (!string.IsNullOrEmpty(dto.Cep))
                query = query.Where(x => x.Cep.Contains(dto.Cep));
            if(dto.Status.HasValue)
                query = query.Where(x => x.Ativo ==  dto.Status);

            var totalItens = query.Count();
            var encomendas = Paginar(query, dto.Pagina, dto.ItensPagina);

            var encomendasLista = await encomendas.Where(x => x.Id == x.Id).Select(x => new EncomendaReadDto()
            {
                Id = x.Id,
                Cep = x.Cep,
                Email = x.Email,
                Nome = x.Nome,
                Alergias = x.Alergias,
                Bairro = x.Bairro,
                Complemento = x.Complemento,
                NoDomicilio = x.NoDomicilio,
                Numero = x.Numero,
                ValorTotal  = x.ValorTotal,
                Pagamento = x.Pagamento,
                Rua = x.Rua,
                TemAlergias = x.TemAlergias,
                DataCriacao = x.DataCriacao.ToString(),
                Status = x.Ativo,
                AdicionarServicoFuro = x.AdicionarServicoFuro,
                Produtos = x.Produtos.Select(p => new EncomendaProdutoReadDto() { Nome = p.Produto.Nome }).ToList()
            }).OrderByDescending(x => x.DataCriacao).OrderBy(x => x.Status).ToListAsync();

            return new ResultadoPaginado<EncomendaReadDto>(encomendasLista, totalItens, dto.Pagina, dto.ItensPagina);
        }

    }
}