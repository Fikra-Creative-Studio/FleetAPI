using Fleet.Models;

namespace Fleet.Interfaces.Repository
{
    public interface IAbastecimentoRepository
    {
        Task<bool> Cadastrar(Abastecimento objeto);
    }
}
