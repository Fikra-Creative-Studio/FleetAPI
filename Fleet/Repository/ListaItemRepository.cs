using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository
{
    public class ListaItemRepository(ApplicationDbContext applicationDbContext) : BaseRepository<ListasItens>(applicationDbContext), IListaItemRepository
    {

        public List<ListasItens> Buscar(int workspaceId, TipoListasEnum tipo)
            => applicationDbContext.ListasItens
                                    .Include(x => x.Listas)
                                    .Where(x => x.Ativo && x.Listas.WorkspaceId == workspaceId && x.Listas.Tipo == tipo && x.Listas.Padrao)
                                    .ToList();

        public List<ListasItens> BuscarPorLista(int listaId)
            => applicationDbContext.ListasItens.Where(x => x.Ativo && x.ListasId == listaId).ToList();
    }
}
