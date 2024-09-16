using Fleet.Controllers.Model.Request.Manutencao;

namespace Fleet.Interfaces.Service
{
    public interface IManutencaoService
    {
        Task Cadastrar(ManutencaoRequest request, string workspaceId);
    }
}
