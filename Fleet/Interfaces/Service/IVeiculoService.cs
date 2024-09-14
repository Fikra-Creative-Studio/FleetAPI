

using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Controllers.Model.Response.Veiculo;

namespace Fleet.Interfaces.Service
{
    public interface IVeiculoService
    {
        Task Cadastrar(VeiculoRequest request, int workspaceId);
        Task<List<VeiculoResponse>> Listar(int workspaceId);

    }
}
