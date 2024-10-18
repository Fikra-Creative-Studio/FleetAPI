using Fleet.Controllers.Model.Request.Relatorio;

namespace Fleet.Interfaces.Service
{
    public interface IRelatorioVisitaService
    {
        Task<string> GeraRelatorio(string workspaceId,RelatorioVisitasRequest request);
    }
}
