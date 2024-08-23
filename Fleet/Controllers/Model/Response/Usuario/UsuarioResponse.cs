using Fleet.Controllers.Model.Response.Workspace;

namespace Fleet.Controllers.Model.Response.Usuario;

public class UsuarioResponse
{
    public string Id { get; set;} = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UrlImagem { get; set; } = string.Empty;

    public List<WorkspaceGetResponse> Workspaces { get; set; } = [];
    
}
