using AutoMapper;
using DAL.DTOs;
using DAL.DTOs.ProdutoDto;
using DAL.Models.EncomendaModels;
using DAL.Models.ProdutoModels;

namespace API.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProdutoCadastroViewModel, Produto>().ReverseMap();
            CreateMap<EncomendaCreateDto, Encomenda>().ReverseMap();
            CreateMap<EncomendaProdutoCreateDto, Produto>().ReverseMap();
            CreateMap<EncomendaProdutoCreateDto, EncomendaProduto>().ReverseMap();
        }
    }
}
