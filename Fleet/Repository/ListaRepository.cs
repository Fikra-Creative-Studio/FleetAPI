using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;

namespace Fleet.Repository
{
    public class ListaRepository(ApplicationDbContext applicationDbContext) : BaseWorkspaceRepository<Listas>(applicationDbContext), IListaRepository
    {
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
