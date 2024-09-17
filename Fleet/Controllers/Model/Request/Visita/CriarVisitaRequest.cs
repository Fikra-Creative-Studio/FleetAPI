using System;
using Fleet.Controllers.Model.Request.VisitaOpcao;

namespace Fleet.Controllers.Model.Request.Visita;

public class CriarVisitaRequest
{
    public DateTime Data { get; set; }  
    public string Observacao { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public string VeiculoId { get; set; } = string.Empty;
    public string EstabelecimentoId { get; set; } = string.Empty;
    public string GPS { get; set;} = string.Empty;
    public List<VisitaOpcaoRequest> Opcoes  { get; set; }
    public List<VisitaImagensRequest> Imagens  { get; set; }
}
