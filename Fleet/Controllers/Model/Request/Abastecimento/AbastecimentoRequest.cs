using Fleet.Models;

namespace Fleet.Controllers.Model.Request.Abastecimento
{
    public class AbastecimentoRequest
    {
        public string Odometro { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string VeiculosId { get; set; } = string.Empty;
        public string EstabelecimentosId { get; set; } = string.Empty;
        public List<AbastecimentoImagensRequest> Imagens { get; set; }
    }
}
