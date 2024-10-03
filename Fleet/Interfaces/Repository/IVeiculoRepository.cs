using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface IVeiculoRepository
    {
        Task<Veiculos> Cadastrar(Veiculos veiculo);
        Task<List<Veiculos>> Listar(int workspaceId);
        Task<Veiculos?> Buscar(Expression<Func<Veiculos, bool>> exp);
        Task AtualizaOdometro(int veiculoId, string odometro);
        Task Deletar(int veiculoId);
        Task AtualizaUso(int veiculoId, Usuario? usuario);
        Task Atualizar(Veiculos veiculo);
    }
}
