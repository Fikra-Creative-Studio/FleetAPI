using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class CheckListService : ICheckListService
    {
        public void Retirar(Checklist objeto, List<string> fotos, string extensaoFoto)
        {
           //verificar se o veiculo ja esta em uso (se tem algum checklist com o id do veiculo e data de devolucao nula)
        }

        public void Devolver(string workspaceId, Checklist objeto)
        {
            throw new NotImplementedException();
        }
    }
}