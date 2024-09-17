using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class ListaService(IListaRepository baseRepository, ILoggedUser loggedUser, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IConfiguration configuration) 
        : BaseWorkspaceService<Listas>(baseRepository, loggedUser, usuarioWorkspaceRepository, configuration), IListaService
    {

        public override void Inserir(string workspaceId, Listas objeto)
        {
            objeto.Padrao = !baseRepository.Existe(getCryptoId(workspaceId), objeto.Tipo);

            base.Inserir(workspaceId, objeto);
        }

        public override bool Validar(Listas objeto)
        {
            if (string.IsNullOrEmpty(objeto.Nome)) throw new BussinessException("Preencha o nome da lista.");

            if(!UsuarioAdministradorAsync(objeto.WorkspaceId).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");

            return true;
        }


        public void TornarPadrao(string workspaceId, string listaId)
        {
            var workpace = getCryptoId(workspaceId);
            var lista = getCryptoId(listaId);

            if (!UsuarioAdministradorAsync(workpace).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");

            baseRepository.TornarPadrao(workpace, lista);
        }

    }
}
