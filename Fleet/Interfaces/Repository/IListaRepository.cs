using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IListaRepository : IBaseWorkspaceRepository<Listas>
    {
        void TornarPadrao(int workspaceId, int listaId);
    }
}
