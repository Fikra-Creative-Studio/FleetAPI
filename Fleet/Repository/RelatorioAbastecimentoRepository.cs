using Fleet.Interfaces.Repository;
using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class RelatorioAbastecimentoRepository(ApplicationDbContext context) : IRelatorioAbastecimentoRepository
    {
        public IQueryable<Abastecimento> Listar(Expression<Func<Abastecimento, bool>> exp)
        {
            return context.Abastecimentos.Where(exp).AsQueryable();
        }
    }
}
