namespace Fleet.Models
{
    public class Manutencao : DbEntity
    {

        public DateTime Data { get; set; }
        public string Odometro { get; set; } = string.Empty;
        public string Servicos { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
        public int VeiculosId { get; set; }
        public virtual Veiculos Veiculos { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int EstabelecimentosId { get; set; }
        public virtual Estabelecimentos Estabelecimentos { get; set; }
        public virtual List<ManutencaoImagens> Imagens { get; set; }
    }
}
