using Fleet.Controllers.Model.Request.Relatorio;

namespace Fleet.Interfaces.Service
{
    public interface IRelatorioVisitaService
    {
        Task<string> Visitas(string workspaceId,RelatorioVisitasRequest request);
    }
}
