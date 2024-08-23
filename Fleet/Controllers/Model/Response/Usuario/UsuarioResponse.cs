namespace Fleet.Controllers.Model.Response.Usuario;

public class UsuarioResponse
{
    public string Id { get; set;} = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UrlImagem { get; set; } = string.Empty;

    public UsuarioResponse(string Id, string Nome, string CPF, string Email, string UrlImagem )
    {
        this.Id = Id;
        this.CPF = CPF;
        this.Email = Email;
        this.Nome = Nome;
        this.UrlImagem = UrlImagem;
    }
    
}
