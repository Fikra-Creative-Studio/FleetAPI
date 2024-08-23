using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IUsuarioWorkspaceRepository
{
    Task Criar(UsuarioWorkspace usuarioWorkspace);
    Task<bool> Existe(int usuarioId, int workspaceId);
}
