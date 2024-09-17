using System.Linq.Expressions;
using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IEstabelecimentoRepository
    {
        Task<bool> Cadastrar(Estabelecimentos estabelecimento);
        Task<List<Estabelecimentos>> Listar(int workspaceId);
        Task<bool> ExisteCnpj(string cnpj, int? id = null);
        Task<Estabelecimentos> Buscar(Expression<Func<Estabelecimentos,bool>> exp);
        Task Atualizar(Estabelecimentos estabelecimento);
        Task Deletar(int id);
    }
}
