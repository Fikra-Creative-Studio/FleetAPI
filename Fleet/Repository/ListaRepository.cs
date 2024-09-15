using Fleet.Interfaces.Repository;
using Fleet.Models;

namespace Fleet.Repository
{
    public class ListaRepository(ApplicationDbContext applicationDbContext) : BaseWorkspaceRepository<Listas>(applicationDbContext), IListaRepository
    {
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
