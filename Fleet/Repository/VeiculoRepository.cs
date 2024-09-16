using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<Veiculos?> Buscar(Expression<Func<Veiculos, bool>> exp)
        {
            return await context.Veiculos.FirstOrDefaultAsync(exp);
        }
        public async Task AtualizaOdometro(int veiculoId, string odometro)
        {
            var veiculo = await context.Veiculos.FirstOrDefaultAsync(x => x.Id == veiculoId);
            if (veiculo != null)
            {
                veiculo.Odometro = odometro;
                await context.SaveChangesAsync();
            }
        }
    }
}
