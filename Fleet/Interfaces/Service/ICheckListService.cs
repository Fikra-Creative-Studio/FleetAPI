using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface ICheckListService
    {
        void Retirar(Checklist objeto, List<Tuple<string, string>> fotos);
        void Devolver(Checklist objeto, List<Tuple<string, string>> fotos);
    }
}
