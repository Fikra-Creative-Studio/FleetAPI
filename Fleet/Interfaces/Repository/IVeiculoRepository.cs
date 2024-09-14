using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IVeiculoRepository
    {
        Task<bool> Cadastrar(Veiculos veiculo);
        Task<List<Veiculos>> Listar(int workspaceId);
    }
}
