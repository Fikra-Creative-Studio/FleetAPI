using Fleet.Controllers.Model.Request.Relatorio;

namespace Fleet.Interfaces.Service
{
    public interface IRelatorioService
    {
        Task<string> Visitas(string workspaceId,RelatorioVisitasRequest request);
    }
}
