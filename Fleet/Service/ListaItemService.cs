using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class ListaItemService(IListaItemRepository baseRepository) : BaseService<ListasItens>(baseRepository), IListaItemService
    {
        public List<ListasItens> Buscar(int workspaceId, TipoListasEnum tipoListasEnum)
        {
            return baseRepository.Buscar(workspaceId, tipoListasEnum);
        }

        public List<ListasItens> BuscarPorLista(int listaId)
        {
            return baseRepository.BuscarPorLista(listaId);
        }

        public override bool Validar(ListasItens objeto)
        {
            if (string.IsNullOrEmpty(objeto.Titulo) || string.IsNullOrEmpty(objeto.Descricao)) throw new BussinessException("O Campo titulo e descrição são obrigatorios.");

            return true;
        }
    }
}
