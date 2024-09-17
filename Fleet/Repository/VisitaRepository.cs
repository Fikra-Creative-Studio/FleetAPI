using System;
using System.Linq.Expressions;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository;

public class VisitaRepository(ApplicationDbContext applicationDbContext) : BaseRepository<Visitas>(applicationDbContext), IVisitaRepository
{
    public async Task Criar(Visitas visita)
    {
        await applicationDbContext.Visitas.AddAsync(visita);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> Existe(Expression<Func<Visitas, bool>> exp)
    {
        return await applicationDbContext.Visitas.AnyAsync(exp);
    }

    public async Task Atualizar(Visitas atualizarVisita)
    {
        var visita = await applicationDbContext.Visitas.FindAsync(atualizarVisita.Id);

        visita.Data = atualizarVisita.Data;
        visita.Supervior = atualizarVisita.Supervior;
        visita.Observacao = atualizarVisita.Observacao;
        visita.VeiculosId = atualizarVisita.VeiculosId;
        visita.Veiculos = atualizarVisita.Veiculos;
        visita.EstabelecimentosId = atualizarVisita.EstabelecimentosId;
        visita.Estabelecimentos = atualizarVisita.Estabelecimentos;

        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<Visitas> Buscar(Expression<Func<Visitas, bool>> exp)
    {
        return await applicationDbContext.Visitas.FirstOrDefaultAsync(exp);
    }

    public IQueryable<Visitas> Listar(Expression<Func<Visitas, bool>> exp)
    {
        return applicationDbContext.Visitas.Where(exp).AsQueryable();
    }
}
