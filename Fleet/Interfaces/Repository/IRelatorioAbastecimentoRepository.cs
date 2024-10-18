using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface IRelatorioAbastecimentoRepository
    {
        IQueryable<Abastecimento> Listar(Expression<Func<Abastecimento, bool>> exp);
    }
}
