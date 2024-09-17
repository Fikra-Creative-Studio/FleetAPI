namespace Fleet.Models
{
    public class Visitas : DbEntity
    {

        public DateTime Data { get; set; }  
        public string Observacao { get; set; }    = string.Empty;
        public string Supervior { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
        public int VeiculosId { get; set; }
        public virtual Veiculos Veiculos { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int EstabelecimentosId { get; set; }
        public virtual Estabelecimentos Estabelecimentos { get; set; }
        public string GPS {get; set; } = string.Empty;
        public virtual List<VisitaImagens> Imagens { get; set; } = [];
        public virtual List<VisitaOpcao> Opcoes { get; set; } = [];
    }
}
