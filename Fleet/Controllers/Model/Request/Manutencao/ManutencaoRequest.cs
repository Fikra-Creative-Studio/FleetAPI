using Fleet.Controllers.Model.Request.Abastecimento;

namespace Fleet.Controllers.Model.Request.Manutencao
{
    public class ManutencaoRequest
    {
        public string Odometro { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Servicos { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public int VeiculosId { get; set; }
        public int EstabelecimentosId { get; set; }
        public List<ManutencaoIMagensRequest> Urls { get; set; }
    }
}
