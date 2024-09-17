using System;
using Fleet.Controllers.Model.Request.Visita;
using Fleet.Controllers.Model.Request.VisitaOpcao;
using Fleet.Controllers.Model.Response.Visita;

namespace Fleet.Interfaces.Service;

public interface IVisitaService
{
    Task Criar(CriarVisitaRequest request);
    List<ListarVisitasResponse> Buscar(string workspaceId);
}
