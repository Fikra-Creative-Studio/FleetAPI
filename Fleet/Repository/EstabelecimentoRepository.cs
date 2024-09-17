using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class EstabelecimentoRepository(ApplicationDbContext context) : IEstabelecimentoRepository
    {
        public async Task<bool> Cadastrar(Estabelecimentos estabelecimento)
        {
            await context.Estabelecimentos.AddAsync(estabelecimento);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Estabelecimentos>> Listar(int workspaceId)
        {
            return await context.Estabelecimentos.Where(v => v.WorkspaceId == workspaceId && v.Ativo == true).ToListAsync();
        }

        public async Task<Estabelecimentos?> Buscar(Expression<Func<Estabelecimentos, bool>> exp)
        {
            return await context.Estabelecimentos.FirstOrDefaultAsync(exp);
        }

        public async Task<bool> ExisteCnpj(string cnpj, int? id = null)
        {
            if (id != null)
                return await context.Estabelecimentos.AnyAsync(x => x.Cnpj == cnpj && x.Id != id);
            return await context.Estabelecimentos.AnyAsync(x => x.Cnpj == cnpj);
        }

        public async Task Atualizar(Estabelecimentos estabelecimento)
        {
            var existingObj = await context.Estabelecimentos.FindAsync(estabelecimento.Id);
            if (existingObj != null)
            {
                context.Entry(existingObj).CurrentValues.SetValues(estabelecimento);
                await context.SaveChangesAsync();
            }
        }

        public async Task Deletar(int id)
        {
            var obj = await context.Estabelecimentos.FindAsync(id);
            if (obj != null)
            {
                obj.Ativo = false;
                await context.SaveChangesAsync();
            }
        }
    }
}
