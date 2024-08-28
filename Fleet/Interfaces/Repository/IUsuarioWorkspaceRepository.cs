using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IUsuarioWorkspaceRepository
{
    Task Criar(UsuarioWorkspace usuarioWorkspace);
    Task<bool> Existe(int usuarioId, int workspaceId);
    Task<bool> UsuarioWorkspaceAdmin(int usuarioId, int workspaceId);
    Task AtualizarPapel(int usuarioId, int workspaceId, PapelEnum papel);
}
