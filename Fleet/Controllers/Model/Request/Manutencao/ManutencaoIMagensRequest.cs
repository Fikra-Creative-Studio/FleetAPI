namespace Fleet.Controllers.Model.Request.Manutencao
{
    public class ManutencaoIMagensRequest
    {
        public string ImagemBase64 { get; set; } = string.Empty;
        public string ExtensaoImagem { get; set; } = string.Empty;
    }
}
