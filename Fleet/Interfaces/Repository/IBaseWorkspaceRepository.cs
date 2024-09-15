using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IBaseWorkspaceRepository<T> where T : class
    {
        void Inserir(T objeto);
        void Atualizar(T objeto);
        void Deletar(int workspaceId, int id);
        List<T> Buscar(int workspaceId);
        T? Buscar(int workspaceId, int id);
    }
}