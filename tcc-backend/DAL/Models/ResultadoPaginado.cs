using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ResultadoPaginado<T>
    {
        public int TotalItens { get; set; }
        public List<T> Data { get; set; }
        public int PaginaAtual { get; set; }
        public int? ItensPagina { get; set; }
        public int QtdPaginas { get; set; }
        public bool NextPage { get; set; }
        public bool PrevPage { get; set; }

        public ResultadoPaginado(List<T> data, int totalItens, int paginaAtual, int? itensPorPagina = null)
        {
            var tamanho = itensPorPagina ?? 10;
            Data = data;
            TotalItens = totalItens;
            PaginaAtual = paginaAtual;
            QtdPaginas = Convert.ToInt32(Math.Ceiling((double)totalItens / tamanho));
            ItensPagina = itensPorPagina;
            PrevPage = paginaAtual != 1;
            NextPage = QtdPaginas > paginaAtual;
        }
    }
}
