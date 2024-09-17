using System;
using Fleet.Controllers.Model.Request.VisitaOpcao;

namespace Fleet.Controllers.Model.Request.Visita;

public class CriarVisitaRequest
{
    public string Observacao { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public string VeiculoId { get; set; } = string.Empty;
    public string EstabelecimentoId { get; set; } = string.Empty;
    public string GPS { get; set;} = string.Empty;
    public List<VisitaOpcaoRequest> Opcoes { get; set; } = new List<VisitaOpcaoRequest>();
    public List<VisitaImagensRequest> Imagens { get; set; } = new List<VisitaImagensRequest>();
}
