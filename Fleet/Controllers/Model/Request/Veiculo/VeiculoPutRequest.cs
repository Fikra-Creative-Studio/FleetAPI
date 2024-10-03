using Fleet.Enums;

namespace Fleet.Controllers.Model.Request.Veiculo
{
    public class VeiculoPutRequest
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
        public DateTime? VencimentoSeguro { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public string ImagemBase64 { get; set; } = string.Empty;
        public string ExtensaoImagem { get; set; } = string.Empty;
        public string WorkspaceId { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
    }
}
