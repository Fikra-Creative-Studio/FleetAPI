using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface ICheckListService
    {
        Task Retirar(Checklist objeto, List<Tuple<string, string>> fotos);
        Task Devolver(Checklist objeto, List<Tuple<string, string>> fotos);
    }
}
