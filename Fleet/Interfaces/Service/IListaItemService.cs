using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface IListaItemService: IBaseService<ListasItens>
    {
        List<ListasItens> Buscar(int workspaceId, TipoListasEnum tipoListasEnum);
        List<ListasItens> BuscarPorLista(int listaId);
    }
}
