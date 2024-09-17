using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class BaseWorkspaceService<T>(IBaseWorkspaceRepository<T> baseRepository, ILoggedUser loggedUser, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IConfiguration configuration) where T : DBWorkspaceEntity, new()
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }
        public int getCryptoId(string workspaceId)
        {
            var idWorkspace = CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("O Workspace apresentou uma falha.");
            return int.Parse(idWorkspace);
        }


        public virtual void Inserir(string workspaceId, T objeto)
        {
            objeto.WorkspaceId = getCryptoId(workspaceId);

            if (Validar(objeto))
                baseRepository.Inserir(objeto);
        }
        public virtual void Atualizar(T objeto)
        {
            if (Validar(objeto))
                baseRepository.Atualizar(objeto);
        }
        public virtual void Deletar(string workspaceId, string id)
        {
            if (!UsuarioAdministradorAsync(getCryptoId(workspaceId)).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");

            if (Buscar(workspaceId, id) != null)
                baseRepository.Deletar(getCryptoId(workspaceId), getCryptoId(id));
        }
        public virtual List<T> Buscar(string workspaceId)
        {
            if (!UsuarioAdministradorAsync(getCryptoId(workspaceId)).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");
            return baseRepository.Buscar(getCryptoId(workspaceId));
        }


        public virtual T? Buscar(string workspaceId, string id)
        {
            if (!UsuarioAdministradorAsync(getCryptoId(workspaceId)).GetAwaiter().GetResult()) throw new BussinessException("Usuário sem permissão para realizar esta ação.");
            return baseRepository.Buscar(getCryptoId(workspaceId), getCryptoId(id));
        }

        public virtual bool Validar(T objeto) //Cada método sobrescreve seu validar
        => true;

        public async Task<bool> UsuarioAdministradorAsync(int workspaceId)
        {
            return await usuarioWorkspaceRepository.Existe(x => x.UsuarioId == loggedUser.UserId && x.WorkspaceId == workspaceId && x.Ativo && x.Papel == Enums.PapelEnum.Administrador);
        }

    }
}