namespace Fleet.Controllers.Model.Response.Abastecimento
{
    public class AbastecimentoResponse
    {
        public DateTime Data { get; set; }
        public string Odometro { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int VeiculosId { get; set; }
        public int EstabelecimentosId { get; set; }
    }
}
