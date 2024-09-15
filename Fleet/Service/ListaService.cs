using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class ListaService(IBaseWorkspaceRepository<Listas> baseRepository, ILoggedUser loggedUser, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IConfiguration configuration) 
        : BaseWorkspaceService<Listas>(baseRepository, loggedUser, usuarioWorkspaceRepository, configuration), IListaService
    {
        public override bool Validar(Listas objeto)
        {
            if (string.IsNullOrEmpty(objeto.Nome)) throw new BussinessException("Preencha o nome da lista.");

            if(!UsuarioAdministradorAsync(objeto.WorkspaceId).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");

            return true;
        }
    }
}
