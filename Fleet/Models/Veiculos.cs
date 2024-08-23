using Fleet.Enums;

namespace Fleet.Models
{
    public class Veiculos : DbEntity 
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Ano { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Combustivel { get; set; } = string.Empty;
        public string Odometro { get; set; } = string.Empty;
        public VeiculoSituacaoEnum Situacao { get; set; } //Manutenção, Livre e Em_uso
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
    