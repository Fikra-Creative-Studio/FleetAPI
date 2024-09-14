using Fleet.Enums;

namespace Fleet.Controllers.Model.Request.Veiculo
{
    public class VeiculoRequest
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Ano { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Combustivel { get; set; } = string.Empty;
        public string Odometro { get; set; } = string.Empty;
    }
}
