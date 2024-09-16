using Fleet.Controllers.Model.Request.Abastecimento;
using Fleet.Controllers.Model.Request.Veiculo;

namespace Fleet.Interfaces.Service
{
    public interface IAbastecimentoService
    {
        Task Cadastrar(AbastecimentoRequest request,string workspaceId);
    }
}
