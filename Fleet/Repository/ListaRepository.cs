using Fleet.Interfaces.Repository;
using Fleet.Models;

namespace Fleet.Repository
{
    public class ListaRepository(ApplicationDbContext applicationDbContext) : BaseWorkspaceRepository<Listas>(applicationDbContext), IListaRepository
    {
    }
}
