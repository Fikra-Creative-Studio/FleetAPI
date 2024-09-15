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
            public bool Manutencao { get; set; }
            public bool Status { get; set; }   //0 - Livre  1 - Uso
            public string Foto { get; set; } = string.Empty;

    }
}
