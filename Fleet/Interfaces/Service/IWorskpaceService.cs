using Fleet.Controllers.Model.Request.Workspace;

namespace Fleet.Interfaces.Service;

public interface IWorskpaceService
{
    Task Criar(IFormFile file,WorkspaceRequest request);
    Task Atualizar(string id);
}
