// <auto-generated />
using System;
using APINotificador.NetCore.Infra.Data.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace APINotificador.NetCore.Infra.Data.Core.Migrations
{
    [DbContext(typeof(ContextBase))]
    partial class ContextBaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("APINotificador.NetCore.Dominio.RemetenteRoot.RemetenteCorporativa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("Id");

                    b.Property<ulong>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("Ativo");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime")
                        .HasColumnName("DataCadastro");

                    b.Property<ulong>("EUsarCredencial")
                        .HasColumnType("bit")
                        .HasColumnName("EUsarCredencial");

                    b.Property<string>("EmailCorporativa")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("EmailCorporativa");

                    b.Property<string>("MACCorporativa")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("MACCorporativa");

                    b.Property<string>("NomeCorporativa")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("NomeCorporativa");

                    b.Property<int>("PortaRemetente")
                        .HasColumnType("int")
                        .HasColumnName("PortaRemetente");

                    b.Property<string>("SenhaCorporativa")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("SenhaCorporativa");

                    b.Property<string>("ServidorRemetente")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("ServidorRemetente");

                    b.Property<ulong>("SslRemetente")
                        .HasColumnType("bit")
                        .HasColumnName("SslRemetente");

                    b.HasKey("Id");

                    b.ToTable("RemetenteCorporativa");
                });
#pragma warning restore 612, 618
        }
    }
}
