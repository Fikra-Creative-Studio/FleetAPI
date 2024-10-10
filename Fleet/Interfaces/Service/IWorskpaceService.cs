using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Controllers.Model.Response.Usuario;
using Fleet.Models;

namespace Fleet.Interfaces.Service;

public interface IWorskpaceService
{
    Task<Workspace> Criar(WorkspaceRequest request);
    Task Atualizar(string id);
    Task<List<UsuarioBuscarWorkspaceResponse>> BuscarUsuarios(string workspaceId);
    Task AtualizarPapel(string workspaceId,WorkspaceAtualizarPermissaoRequest request);
    Task<string> ConvidarUsuario(string workspaceId, string email);
    Task ReenviarConviteUsuario(string workspaceId, string usuarioId);
    Task RemoverUsuario(string workspaceId, string usuarioId);
}
