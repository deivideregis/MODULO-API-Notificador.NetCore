using System;

namespace APINotificador.NetCore.Aplicacao.ViewModels.Base
{
    public abstract class BaseViewModelCadastro
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual bool Ativo { get; set; }

        public BaseViewModelCadastro()
        {
            DataCadastro = DateTime.Now;
            Ativo = true;
        }
    }
}
