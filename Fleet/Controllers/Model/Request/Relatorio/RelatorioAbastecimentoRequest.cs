namespace Fleet.Controllers.Model.Request.Relatorio
{
    public class RelatorioAbastecimentoRequest
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public List<string> EstabelecimentosId { get; set; } = new List<string>();
        public List<string> VeiculoId { get; set; } = new List<string>();
    }
}
