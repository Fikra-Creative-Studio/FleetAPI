namespace Fleet.Controllers.Model.Request.Checklist
{
    public class ChecklistRetiradaRequest
    {
        public string VeiculoId { get; set; } = string.Empty;
        public string Odometro { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;

        public List<CheckListOpcoesRequest> Opcoes { get; set; } = new List<CheckListOpcoesRequest>();
        public List<CheckListImagemRequest> Images { get; set; } = new List<CheckListImagemRequest>();
    }
}
