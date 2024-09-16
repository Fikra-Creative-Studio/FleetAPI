namespace Fleet.Controllers.Model.Request.Checklist
{
    public class CheckListOpcoesRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Opcao { get; set; }
    }
}
