using Fleet.Models;

namespace Fleet.Controllers.Model.Request.Abastecimento
{
    public class AbastecimentoRequest
    {
        public string Odometro { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int VeiculosId { get; set; }
        public int EstabelecimentosId { get; set; }
        public List<AbastecimentoImagensRequest> Urls { get; set; }
    }
}
