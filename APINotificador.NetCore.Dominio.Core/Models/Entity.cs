using System;

namespace APINotificador.NetCore.Dominio.Core.Models
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual bool Ativo { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + "[Id = " + Id + "]";
        }
    }
}
