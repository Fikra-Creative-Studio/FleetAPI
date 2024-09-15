using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface IVeiculoRepository
    {
        Task<bool> Cadastrar(Veiculos veiculo);
        Task<List<Veiculos>> Listar(int workspaceId);
        Task<Veiculos?> Buscar(Expression<Func<Veiculos, bool>> exp);
    }
}
