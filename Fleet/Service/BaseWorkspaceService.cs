using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class BaseWorkspaceService<T>(IBaseWorkspaceRepository<T> baseRepository, ILoggedUser loggedUser, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IConfiguration configuration) where T : DBWorkspaceEntity, new()
    {

        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        public void Inserir(string workspaceId, T objeto)
        {
            var idWorkspace = CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("O Workspace apresentou uma falha.");
            objeto.WorkspaceId = int.Parse(idWorkspace);

            if (Validar(objeto))
                baseRepository.Inserir(objeto);
        }

        public void Atualizar(T objeto)
        {
            if (Validar(objeto))
                baseRepository.Atualizar(objeto);
        }
        public void Deletar(string workspaceId, int id)
        {
            var idWorkspace = CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("O Workspace apresentou uma falha.");
            if (Buscar(workspaceId, id) != null)
                baseRepository.Deletar(int.Parse(idWorkspace), id);
        }
        public List<T> Buscar(string workspaceId)
        {
            var idWorkspace = CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("O Workspace apresentou uma falha.");
            return baseRepository.Buscar(int.Parse(idWorkspace));
        }

        public T? Buscar(string workspaceId, int id)
        {
            var idWorkspace = CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("O Workspace apresentou uma falha.");
            return baseRepository.Buscar(int.Parse(idWorkspace), id);
        }

        public virtual bool Validar(T objeto) //Cada método sobrescreve seu validar
        => true;

        public async Task<bool> UsuarioAdministradorAsync(int workspaceId)
        {
            return await usuarioWorkspaceRepository.Existe(x => x.UsuarioId == loggedUser.UserId && x.WorkspaceId == workspaceId && x.Ativo);
        }

    }
}