namespace Fleet.Models
{
    public class Manutencao : DbEntity
    {

        public DateTime Data { get; set; }
        public string Odometro { get; set; } = string.Empty;
        public string Servicos { get; set; } = string.Empty;
        public double Valor { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public virtual Workspace Workspace { get; set; }
        public int VeiculoId { get; set; }
        public virtual Veiculos Veiculos { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int EstabelecimentoId { get; set; }
        public virtual Estabelecimentos Estabelecimentos { get; set; }
    }
}
