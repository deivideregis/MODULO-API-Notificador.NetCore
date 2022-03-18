using APINotificador.NetCore.Dominio.RemetenteRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.TypeConfiguration.Remetentes
{
    public class RemetenteCorporativaTypeConfiguration : IEntityTypeConfiguration<RemetenteCorporativa>
    {
        public void Configure(EntityTypeBuilder<RemetenteCorporativa> builder)
        {
            builder.ToTable("RemetenteCorporativa");

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .HasColumnType("char(36)")
                .IsRequired();

            builder.HasKey(pk => pk.Id);

            builder.Property(p => p.PortaRemetente)
                .HasColumnName("PortaRemetente")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.SslRemetente)
                .HasColumnName("SslRemetente")
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(p => p.EUsarCredencial)
                .HasColumnName("EUsarCredencial")
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(p => p.ServidorRemetente)
                .HasColumnName("ServidorRemetente")
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(p => p.NomeCorporativa)
                .HasColumnName("NomeCorporativa")
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(p => p.MACCorporativa)
                .HasColumnName("MACCorporativa")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.EmailCorporativa)
                .HasColumnName("EmailCorporativa")
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(p => p.SenhaCorporativa)
                .HasColumnName("SenhaCorporativa")
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType("bit");
        }
    }
}
