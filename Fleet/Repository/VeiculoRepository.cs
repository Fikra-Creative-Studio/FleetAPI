using Fleet.Interfaces.Repository;
using Fleet.Migrations;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class VeiculoRepository(ApplicationDbContext context) : IVeiculoRepository
    {

        public async Task<Veiculos> Cadastrar(Veiculos veiculo)
        {
            await context.Veiculos.AddAsync(veiculo);                  
            await context.SaveChangesAsync();
            return veiculo;
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

        public async Task AtualizaUso(int veiculoId, Usuario? usuario)
        {
            var veiculo = await context.Veiculos.FirstOrDefaultAsync(x => x.Id == veiculoId);
            if (veiculo != null)
            {
                veiculo.UsuariosId = usuario?.Id;
                veiculo.EmUsoPor = usuario == null ? string.Empty : usuario.Nome;
                await context.SaveChangesAsync();
            }
        }

        public async Task Deletar(int veiculoId)
        {
            var veiculo = await context.Veiculos.FirstOrDefaultAsync(x => x.Id == veiculoId);
            if (veiculo != null)
            {
                veiculo.Ativo = false;
                await context.SaveChangesAsync();
            }
        }
        public DateTime? BuscarDataUltimoCheck(Veiculos veiculo)
        {
            if (veiculo != null)
                if (!string.IsNullOrEmpty(veiculo.EmUsoPor))
                {
                    var check = context.Checklists.FirstOrDefault(x => x.DataDevolucao == null && x.VeiculosId == veiculo.Id);
                    if(check != null)
                        return check.DataRetirada;
                }
            return null;
        }
        public DateTime? BuscarDataUltimoAbastecimento(Veiculos veiculo)
        {
            if (veiculo != null)
            {
                var abastece = context.Abastecimentos.Where(x => x.VeiculosId == veiculo.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                if(abastece != null)
                    return abastece.Data;
            }
            return null;
        }
        public async Task Atualizar(Veiculos veiculo)
        {
            var existingObj = await context.Veiculos.FindAsync(veiculo.Id);
            if (existingObj != null)
            {
                context.Entry(existingObj).CurrentValues.SetValues(veiculo);
                context.SaveChanges();
            }
        }
    }
}
