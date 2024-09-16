using Fleet.Interfaces.Repository;
using Fleet.Models;

namespace Fleet.Repository
{
    public class ManutencaoRepository(ApplicationDbContext context) : IManutencaoRepository
    {

        public async Task<bool> Cadastrar(Manutencao objeto)
        {
            await context.Manutencao.AddAsync(objeto);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
