using Fleet.Controllers.Model.Request.Relatorio;

namespace Fleet.Interfaces.Service
{
    public interface IRelatorioAbastecimentoService
    {
        Task<string> GeraRelatorio(string workspaceId, RelatorioAbastecimentoRequest request);
    }
}
