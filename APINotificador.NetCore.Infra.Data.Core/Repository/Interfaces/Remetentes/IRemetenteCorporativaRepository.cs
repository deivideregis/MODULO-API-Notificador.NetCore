using APINotificador.NetCore.Dominio.Core;
using APINotificador.NetCore.Dominio.RemetenteRoot;
using System;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes
{
    public interface IRemetenteCorporativaRepository : IRepository<RemetenteCorporativa>
    {

        RemetenteCorporativa RetornaRemetentePorId(Guid id);

        RemetenteCorporativa RetornaRemetentePorId(string EmailCorporativa);

        Task AtualizarRemetente(RemetenteCorporativa model);

        bool DominioExistente(string EmailCorporativa);

        bool DominioExistenteMAC(string MAC);

        bool PossuiRegistroRemetenteCorporativa();

        string CriptografarSenha(string senha);

        string DescriptografarSenha(string senha);

        string GetEnderecoMAC();

        Task<ListaPaginada<RemetenteCorporativa>> ObterPorTodosFiltros(
            Guid? id,
            string NomeCorporativa,
            string EmailCorporativa,
            bool? ativo,
            int? pagina,
            int? tamanhoPagina,
            string ordem);
    }
}
