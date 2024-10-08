namespace Fleet.Controllers.Model.Request.Manutencao
{
    public class ManutencaoIMagensRequest
    {
        public string ImagemBase64 { get; set; } = string.Empty;
        public string Extensao { get; set; } = string.Empty;
    }
}
