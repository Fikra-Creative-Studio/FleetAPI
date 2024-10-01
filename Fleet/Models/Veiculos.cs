using Fleet.Enums;

namespace Fleet.Models
{
    public class Veiculos : DbEntity
    {
        public TipoVeiculoEnum Tipo { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Ano { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public TipoCombustivel Combustivel { get; set; }
        public string Cor { get; set; } = string.Empty;
        public string Odometro { get; set; } = string.Empty;
        public string Renavam { get; set; } = string.Empty;
        public string Seguradora { get; set; } = string.Empty;
        public DateTime? VencimentoSeguro { get; set; } = null;
        public string Observacao { get; set; } = string.Empty;
        public string Foto {  get; set; } = string.Empty;
        public bool Manutencao { get; set; }   
        public string EmUsoPor { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
    