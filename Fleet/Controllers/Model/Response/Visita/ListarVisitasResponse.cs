namespace Fleet.Controllers.Model.Response.Visita;

public class ListarVisitasResponse
{
    public DateTime Data { get; set; }
    public string Estabelecimento { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
}
