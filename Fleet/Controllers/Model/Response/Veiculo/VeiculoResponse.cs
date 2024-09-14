using Fleet.Enums;

namespace Fleet.Controllers.Model.Response.Veiculo
{
    public class VeiculoResponse
    {
            public string Marca { get; set; } = string.Empty;
            public string Modelo { get; set; } = string.Empty;
            public string Ano { get; set; } = string.Empty;
            public string Placa { get; set; } = string.Empty;
            public string Combustivel { get; set; } = string.Empty;
            public string Odometro { get; set; } = string.Empty;
            public VeiculoSituacaoEnum SituacaoEnum { get; set; }
        
    }
}
