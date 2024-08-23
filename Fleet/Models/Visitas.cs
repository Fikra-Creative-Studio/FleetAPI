namespace Fleet.Models
{
    public class Visitas : DbEntity
    {

        public DateTime Data { get; set; }  
        public string Observacao { get; set; }    = string.Empty;
        public string Supervior { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
        public int VeiculoId { get; set; }
        public virtual Veiculos Veiculos { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int EstabelecimentoId { get; set; }
        public virtual Estabelecimentos Estabelecimentos { get; set; }
    }
}
