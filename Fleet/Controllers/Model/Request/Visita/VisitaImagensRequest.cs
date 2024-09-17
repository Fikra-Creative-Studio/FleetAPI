using System;

namespace Fleet.Controllers.Model.Request.Visita;

public class VisitaImagensRequest
{
    public string ImagemBase64 { get; set; } = string.Empty;
    public string ExtensaoImagem { get; set; } = string.Empty;
    public bool Assinatura { get; set; }
}
