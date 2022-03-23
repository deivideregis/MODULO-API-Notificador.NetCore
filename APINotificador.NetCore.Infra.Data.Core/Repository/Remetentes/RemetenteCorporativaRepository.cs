using APINotificador.NetCore.Dominio.Core;
using APINotificador.NetCore.Dominio.RemetenteRoot;
using APINotificador.NetCore.Infra.Data.Core.Context;
using APINotificador.NetCore.Infra.Data.Core.Repository.Base;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Remetentes
{
    public class RemetenteCorporativaRepository : Repository<RemetenteCorporativa>, IRemetenteCorporativaRepository
    {
        /// <summary>     
        /// Vetor de bytes utilizados para a criptografia (Chave Externa)     
        /// </summary>     
        private static byte[] bIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        /// <summary>     
        /// Representação de valor em base 64 (Chave Interna)    
        /// O Valor representa a transformação para base64 de     
        /// um conjunto de 32 caracteres (8 * 32 = 256bits)    
        /// A chave é: "Criptografias com Rijndael / AES"     
        /// </summary>     
        private const string cryptoKey = "Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=";

        public string CriptografarSenha(string senha)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(senha))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = new UTF8Encoding().GetBytes(senha);

                    // Instancia a classe de criptografia Rijndael
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor criptografado:                
                    MemoryStream mStream = new MemoryStream();
                    // Instancia o encriptador                 
                    CryptoStream encryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateEncryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória
                    encryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    encryptor.FlushFinalBlock();
                    // Pega o vetor de bytes da memória e gera a string criptografada                
                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        /// <summary>     
        /// Pega um valor previamente criptografado e retorna o valor inicial 
        /// </summary>     
        /// <param name="senha">texto criptografado</param>     
        /// <returns>valor descriptografado</returns>     
        public string DescriptografarSenha(string senha)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia           
                if (!string.IsNullOrEmpty(senha))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(cryptoKey);
                    byte[] bText = Convert.FromBase64String(senha);

                    // Instancia a classe de criptografia Rijndael                
                    Rijndael rijndael = new RijndaelManaged();

                    // Define o tamanho da chave "256 = 8 * 32"                
                    // Lembre-se: chaves possíves:                
                    // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                    rijndael.KeySize = 256;

                    // Cria o espaço de memória para guardar o valor DEScriptografado:               
                    MemoryStream mStream = new MemoryStream();

                    // Instancia o Decriptador                 
                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        rijndael.CreateDecryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    // Faz a escrita dos dados criptografados no espaço de memória   
                    decryptor.Write(bText, 0, bText.Length);
                    // Despeja toda a memória.                
                    decryptor.FlushFinalBlock();
                    // Instancia a classe de codificação para que a string venha de forma correta         
                    UTF8Encoding utf8 = new UTF8Encoding();
                    // Com o vetor de bytes da memória, gera a string descritografada em UTF8       
                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }

        public string GetEnderecoMAC()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String enderecoMAC = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    // retorna endereço MAC
                    if (enderecoMAC == String.Empty)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        enderecoMAC = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return enderecoMAC;
            }
            catch
            {
                return "";
            }
        }

        public RemetenteCorporativaRepository(ContextBase context) : base(context)
        {
        }

        public override IQueryable<RemetenteCorporativa> ReturnIQueryable()
        {
            return Db.RemetenteCorporativas
                              .AsNoTracking();
        }


        public RemetenteCorporativa RetornaRemetentePorId(Guid id)
        {
            return Db.RemetenteCorporativas.Where(p => p.Id == id && p.Ativo == true).FirstOrDefault();
        }

        public RemetenteCorporativa RetornaRemetentePorId(string EmailCorporativa)
        {
            return Db.RemetenteCorporativas.Where(p => p.EmailCorporativa == EmailCorporativa && p.Ativo == true).FirstOrDefault();
        }

        public RemetenteCorporativa RetornaRemetentePorMac(string MAC)
        {
            return Db.RemetenteCorporativas.Where(p => p.MACCorporativa == MAC && p.Ativo == true).FirstOrDefault();
        }

        public async Task AtualizarRemetente(RemetenteCorporativa model)
        {
            Db.RemetenteCorporativas.Update(model);
            await Db.SaveChangesAsync();
        }

        public bool DominioExistente(string emailcorporativa)
        {
            return Db.RemetenteCorporativas.AsQueryable().Where(p => p.EmailCorporativa.Equals(emailcorporativa) && p.Ativo == true).Count() > 0;
        }

        public bool DominioExistenteMAC(string MAC)
        {
            return Db.RemetenteCorporativas.AsQueryable().Where(p => p.MACCorporativa.Equals(MAC) && p.Ativo == true).Count() > 0;
        }

        public bool PossuiRegistroRemetenteCorporativa()
        {
            return Db.RemetenteCorporativas.AsQueryable().Where(p => p.Ativo == true).Count() > 0;
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public async Task<ListaPaginada<RemetenteCorporativa>> ObterPorTodosFiltros(
            Guid? id,
            string nomecorporativa,
            string emailcorporativa,
            bool? ativo,
            int? pagina,
            int? tamanhoPagina,
            string ordem)
        {
            //filtra todo remetente corporativa somentes os ativos
            IQueryable<RemetenteCorporativa> _consulta = Db.RemetenteCorporativas.AsQueryable().Where(p => p.Ativo == true);

            if (id.HasValue)
                _consulta = _consulta.Where(p => p.Id == id.Value);

            if (!string.IsNullOrEmpty(nomecorporativa))
                _consulta = _consulta.Where(p => EF.Functions.Like(p.NomeCorporativa, nomecorporativa.ToScape()));

            if (!string.IsNullOrEmpty(emailcorporativa))
                _consulta = _consulta.Where(p => EF.Functions.Like(p.EmailCorporativa, emailcorporativa.ToScape()));

            if (ativo.HasValue)
                _consulta = _consulta.Where(e => e.Ativo == ativo.Value);

            _consulta = _consulta.OrderBy(p => p.NomeCorporativa);
            _consulta = _consulta.OrderByNew(ordem);

            _paginated = await ReturnPaginatedList(_consulta, pagina, tamanhoPagina);

            return new ListaPaginada<RemetenteCorporativa>(_paginated, _paginated.PageIndex, _paginated.TotalPages, _paginated.PageSize, _paginated.TotalItens);
        }
    }
}
