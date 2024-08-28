using System;
using Fleet.Enums;

namespace Fleet.Controllers.Model.Request.Usuario;

public class UsuarioAtualizarPapelRequest
{
    public string WorkspaceId { get; set; } = string.Empty;
    public string UsuarioId { get; set; } = string.Empty;
    public PapelEnum Papel { get; set; }
}
