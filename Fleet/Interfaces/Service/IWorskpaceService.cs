using Fleet.Controllers.Model.Request.Workspace;

namespace Fleet.Interfaces.Service;

public interface IWorskpaceService
{
    Task Criar(WorkspaceRequest request);
    Task Atualizar(string id);
}
