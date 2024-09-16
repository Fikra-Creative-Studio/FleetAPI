using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface ICheckListService
    {
        void Retirar(Checklist objeto, List<string> fotos, string extensaoFoto);
        void Devolver(string workspaceId, Checklist objeto);
    }
}
