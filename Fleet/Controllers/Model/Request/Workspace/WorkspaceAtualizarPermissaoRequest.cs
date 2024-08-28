using System;
using Fleet.Enums;

namespace Fleet.Controllers.Model.Request.Workspace;

public class WorkspaceAtualizarPermissaoRequest
{
    public string UsuarioId { get; set; } = string.Empty;
    public PapelEnum Papel { get; set; }
}
