using System;

namespace Fleet.Controllers.Model.Request.Workspace;

public class WorkspaceRequest
{
    public string CNPJ { get; set; } = string.Empty;
    public string Fantasia { get; set; } = string.Empty;
}
