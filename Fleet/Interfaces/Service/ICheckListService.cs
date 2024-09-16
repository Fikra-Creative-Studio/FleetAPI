using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface ICheckListService
    {
        void Retirar(Checklist objeto, Dictionary<string, string> fotos);
        void Devolver(Checklist objeto, Dictionary<string, string> fotos);
    }
}
