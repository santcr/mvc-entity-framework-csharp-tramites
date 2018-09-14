using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace obligP3parte2.Models.Dominio
{
    public class obligP3parte2Context: DbContext
    {
        public DbSet<Etapa> Etapas { get; set; }
        public DbSet<EtapaCumplida> EtapasCumplidas { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Tramite> Tramites { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public obligP3parte2Context():base("con"){ }




    }
}