using Microsoft.EntityFrameworkCore;
using Fleet.Models;
using System.Reflection;

namespace Fleet.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<UsuarioWorkspace> UsuarioWorkspaces { get; set; }
        public DbSet<Veiculos> Veiculos { get; set; }
        public DbSet<Estabelecimentos> Estabelecimentos { get; set; }
        public DbSet<Listas> Listas { get; set; }
        public DbSet<ListasItens> ListasItens { get; set; }
        public DbSet<Abastecimento> Abastecimentos { get; set; }
        public DbSet<Manutencao> Manutencao { get; set; }
        public DbSet<Checklist> Checklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}