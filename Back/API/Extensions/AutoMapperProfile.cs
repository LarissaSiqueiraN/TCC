using AutoMapper;
using DAL.DTOs;
using DAL.Models;

namespace API.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AnaliseViewModel, Analise>().ReverseMap();
            CreateMap<AnaliseDto, Analise>().ReverseMap();
            CreateMap<AnaliseLinhaDto, AnaliseLinha>().ReverseMap();
            CreateMap<AnaliseLinhaDadosDto, AnaliseLinhaDados>().ReverseMap();
        }
    }
}
