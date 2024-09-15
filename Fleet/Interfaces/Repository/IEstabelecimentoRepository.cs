using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IEstabelecimentoRepository
    {
        Task<bool> Cadastrar(Estabelecimentos estabelecimento);
        Task<List<Estabelecimentos>> Listar(int workspaceId);
        Task<bool> ExisteCnpj(string cnpj, int? id = null);
    }
}
