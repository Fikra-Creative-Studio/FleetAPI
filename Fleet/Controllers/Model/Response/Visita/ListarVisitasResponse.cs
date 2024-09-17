using System;
using Fleet.Controllers.Model.Response.Estabelecimento;
using Fleet.Controllers.Model.Response.Veiculo;
using Fleet.Controllers.Model.Response.Workspace;

namespace Fleet.Controllers.Model.Response.Visita;

public class ListarVisitasResponse
{
    public string Id { get; set; }
    public DateTime Data { get; set; }  
    public string Observacao { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public VeiculoResponse Veiculo { get; set; }
    public EstabelecimentoNomeEIdResponse Estabelecimento { get; set; }
    public string GPS { get; set;} = string.Empty;
    public List<VisitaOpcaoResponse> Opcoes  { get; set; }
    public List<VisitaImagensResponse> Imagens  { get; set; }
}
