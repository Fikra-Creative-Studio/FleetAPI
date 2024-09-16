using Fleet.Controllers.Model.Request.Abastecimento;

namespace Fleet.Controllers.Model.Request.Manutencao
{
    public class ManutencaoRequest
    {
        public string Odometro { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Servicos { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public string VeiculosId { get; set; } = string.Empty;
        public string EstabelecimentosId { get; set; } = string.Empty;
        public List<ManutencaoIMagensRequest> Urls { get; set; }
    }
}
