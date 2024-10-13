using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface IRelatorioRepository
    {
        Task<Visitas?> Buscar(Expression<Func<Visitas, bool>> exp);
        IQueryable<Visitas> Listar(Expression<Func<Visitas, bool>> exp);
        IQueryable<VisitaImagens> ListarImagens(Expression<Func<VisitaImagens, bool>> exp);
        IQueryable<VisitaOpcao> ListarOpcoes(Expression<Func<VisitaOpcao, bool>> exp);
        IQueryable<Workspace> BuscaWorkspace(Expression<Func<Workspace, bool>> exp);
        IQueryable<Veiculos> BuscaVeiculo(Expression<Func<Veiculos, bool>> exp);
        IQueryable<Usuario> BuscaUsuario(Expression<Func<Usuario, bool>> exp);
        IQueryable<Estabelecimentos> BuscaEstabelecimento(Expression<Func<Estabelecimentos, bool>> exp);
    }
}
