using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IManutencaoRepository
    {
        Task<bool> Cadastrar(Manutencao objeto);
    }
}
