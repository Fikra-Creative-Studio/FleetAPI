namespace Fleet.Controllers.Model.Request.Relatorio
{
    public class RelatorioVisitasRequest
    {
        public DateTime DataInicial { get; set; } 
        public DateTime DataFinal { get; set; } 
        public List<string> UsuariosId { get; set; } = new List<string>();
        public List<string> EstabelecimentosId { get; set; } = new List<string>();
    }
}
