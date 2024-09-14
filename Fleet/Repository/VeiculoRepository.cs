using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository
{
    public class VeiculoRepository(ApplicationDbContext context) : IVeiculoRepository
    {

        public async Task<bool> Cadastrar(Veiculos veiculo)
        {
            await context.Veiculos.AddAsync(veiculo);                  
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Veiculos>> Listar(int workspaceId)
        {
            return await context.Veiculos.Where(v => v.WorkspaceId == workspaceId && v.Ativo == true).ToListAsync();
        }

    }
}
