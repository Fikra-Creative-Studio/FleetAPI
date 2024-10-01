using Fleet.Enums;

namespace Fleet.Controllers.Model.Response.Veiculo
{
    public class VeiculoResponse
    {
        public string Id { get; set; } = string.Empty;
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
        public DateTime? VencimentoSeguro { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public bool Manutencao { get; set; }
        public bool EmUso { get; set; }
        public bool Comigo { get; set; }
        public string EmUsoPor { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;

    }
}
