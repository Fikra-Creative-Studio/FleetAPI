using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Controllers.Model.Response.Veiculo;

namespace Fleet.Interfaces.Service
{
    public interface IVeiculoService
    {
        Task<string> Cadastrar(VeiculoRequest request, string workspaceId);
        Task<List<VeiculoResponse>> Listar(string workspaceId);
        Task Deletar(string veiculoId);
        Task Atualizar(VeiculoPutRequest request, string veiculoId); 
        Task<VeiculoDatasResponse> BuscarDataUltimoUso(string veiculoId);
    }
}