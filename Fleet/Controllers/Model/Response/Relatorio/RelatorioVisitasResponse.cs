using Fleet.Models;

namespace Fleet.Controllers.Model.Response.Relatorio
{
    public class RelatorioVisitasResponse
    {
        public int Id;
        public DateTime Data { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public string Supervior { get; set; } = string.Empty;
        public Models.Workspace Workspace { get; set; }
        public Veiculos Veiculos { get; set; }
        public Models.Usuario Usuario { get; set; }
        public Estabelecimentos Estabelecimentos { get; set; }
        public string GPS { get; set; } = string.Empty;
        public List<VisitaImagens> Imagens { get; set; } = [];
        public List<VisitaOpcao> Opcoes { get; set; } = [];

    }
}
