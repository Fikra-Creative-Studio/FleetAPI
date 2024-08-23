namespace Fleet.Models
{
    public class Usuario : DbEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        //public ICollection<UsuarioWorkspace> UsuarioWorkspaces { get; set ;} = [];
    }
}