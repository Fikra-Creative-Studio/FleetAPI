using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface IListaService : IBaseWorkspaceService<Listas>
    {
        void TornarPadrao(string workspaceId, string listaId);
    }
}
