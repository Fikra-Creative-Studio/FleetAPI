using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Controllers.Model.Response.Usuario;

namespace Fleet.Interfaces.Service;

public interface IWorskpaceService
{
    Task Criar(IFormFile file,WorkspaceRequest request);
    Task Atualizar(string id);
    Task<List<UsuarioBuscarWorkspaceResponse>> BuscarUsuarios(string workspaceId);
    Task AtualizarPapel(string workspaceId,WorkspaceAtualizarPermissaoRequest request);
}
