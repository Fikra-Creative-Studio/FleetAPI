using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository
{
    public class ListaRepository(ApplicationDbContext applicationDbContext) : BaseWorkspaceRepository<Listas>(applicationDbContext), IListaRepository
    {
        public List<Listas> BuscarComItems(int workspaceId)
            => applicationDbContext.Listas.Include(x => x.ListasItens)
                                    .Where(x => x.Ativo && x.WorkspaceId == workspaceId)
                                    .ToList();

        public bool Existe(int workspaceId, TipoListasEnum tipo)
        {
            return applicationDbContext.Listas.Any(x => x.WorkspaceId == workspaceId && x.Tipo == tipo && x.Ativo);
        }

        public void TornarPadrao(int workspaceId, int listaId)
        {
            var listaAtual = Buscar(workspaceId, listaId);
            if (listaAtual != null)
            {
                var listaAntiga = applicationDbContext.Listas.First(x => x.Padrao == true && x.WorkspaceId == workspaceId);

                listaAntiga.Padrao = false;
                listaAtual.Padrao = true;

                applicationDbContext.SaveChanges();
            }
        }
    }
}
