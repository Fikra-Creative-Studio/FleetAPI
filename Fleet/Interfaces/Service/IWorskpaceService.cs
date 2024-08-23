using System;
using Fleet.Controllers.Model.Request.Workspace;

namespace Fleet.Interfaces.Service;

public interface IWorskpaceService
{
    Task Criar(string usuarioId, WorkspaceRequest request);
    Task Atualizar(string id);
}
