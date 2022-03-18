using System.Collections.Generic;

namespace APINotificador.NetCore.Dominio.Core
{
    public class SuccessRetorno<TViewModel>
        where TViewModel : class
    {
        public bool success { get; set; }
        public List<TViewModel> data { get; set; }
    }
}
