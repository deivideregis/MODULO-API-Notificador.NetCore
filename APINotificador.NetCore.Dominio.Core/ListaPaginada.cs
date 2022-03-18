using System.Collections.Generic;

namespace APINotificador.NetCore.Dominio.Core
{
    public class ListaPaginada<TEntity> where TEntity : class
    {
        public ListaPaginada(IEnumerable<TEntity> listaRetorno, int paginaAtual, int totalPaginas, int tamanhoPagina, int totalItens)
        {
            ListaRetorno = listaRetorno;
            PaginaAtual = paginaAtual;
            TotalPaginas = totalPaginas;
            TamanhoPagina = tamanhoPagina;
            TotalItens = totalItens;
        }

        public int PaginaAtual { get; private set; }
        public int TotalPaginas { get; private set; }
        public int TamanhoPagina { get; private set; }
        public int TotalItens { get; private set; }
        public IEnumerable<TEntity> ListaRetorno { get; private set; }
    }
}
