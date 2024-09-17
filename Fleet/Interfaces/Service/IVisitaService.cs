using Fleet.Models;

namespace Fleet.Interfaces.Service;

public interface IVisitaService
{
    Task Criar(Visitas visita, List<Tuple<string,string,bool>> fotos);
    Task<List<Visitas>> Buscar(string workspaceId);
}
