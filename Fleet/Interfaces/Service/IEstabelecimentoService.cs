using Fleet.Controllers.Model.Request.Estabelecimento;
using Fleet.Controllers.Model.Response.Estabelecimento;

namespace Fleet.Interfaces.Service
{
    public interface IEstabelecimentoService
    {
        Task Cadastrar(EstabelecimentoRequest request, string workspaceId);
        Task<List<EstabelecimentoResponse>> Listar(string workspaceId);
    }
}
