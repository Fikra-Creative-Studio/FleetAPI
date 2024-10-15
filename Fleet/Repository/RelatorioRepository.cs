using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class RelatorioRepository(ApplicationDbContext context) : IRelatorioRepository
    {
        public async Task<Visitas?> Buscar(Expression<Func<Visitas, bool>> exp)
        {
            return await context.Visitas.FirstOrDefaultAsync(exp);
        }
        public IQueryable<Visitas> Listar(Expression<Func<Visitas, bool>> exp)
        {
            return context.Visitas.Where(exp).AsQueryable();
        }
        public IQueryable<VisitaImagens> ListarImagens(Expression<Func<VisitaImagens, bool>> exp)
        {
            return context.VisitaImagens.Where(exp).AsQueryable();
        }
        public IQueryable<VisitaOpcao> ListarOpcoes(Expression<Func<VisitaOpcao, bool>> exp)
        {
            return context.VisitaOpcoes.Where(exp).AsQueryable();
        }
        public IQueryable<Workspace> BuscaWorkspace(Expression<Func<Workspace, bool>> exp)
        {
            return context.Workspaces.Where(exp).AsQueryable();
        }
        public IQueryable<Veiculos> BuscaVeiculo(Expression<Func<Veiculos, bool>> exp)
        {
            return context.Veiculos.Where(exp).AsQueryable();
        }
        public IQueryable<Usuario> BuscaUsuario(Expression<Func<Usuario, bool>> exp)
        {
            return context.Usuarios.Where(exp).AsQueryable();
        }
        public IQueryable<Estabelecimentos> BuscaEstabelecimento(Expression<Func<Estabelecimentos, bool>> exp)
        {
            return context.Estabelecimentos.Where(exp).AsQueryable();
        }
    }
}