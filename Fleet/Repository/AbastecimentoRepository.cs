using Fleet.Interfaces.Repository;
using Fleet.Models;

namespace Fleet.Repository
{
    public class AbastecimentoRepository(ApplicationDbContext context) : IAbastecimentoRepository
    {

        public async Task<bool> Cadastrar(Abastecimento objeto)
        {
            await context.Abastecimentos.AddAsync(objeto);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
