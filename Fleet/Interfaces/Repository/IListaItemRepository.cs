using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IListaItemRepository : IBaseRepository<ListasItens>
    {
        List<ListasItens> Buscar(int workspaceId, TipoListasEnum tipo);
        List<ListasItens> BuscarPorLista(int listaId);
    }
}
