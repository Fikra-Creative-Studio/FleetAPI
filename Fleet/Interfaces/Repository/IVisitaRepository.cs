using System;
using System.Linq.Expressions;
using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IVisitaRepository
{
    Task Criar(Visitas visita);
    Task<bool> Existe(Expression<Func<Visitas,bool>> exp);
    Task Atualizar(Visitas atualizarVisita);
    Task<Visitas> Buscar(Expression<Func<Visitas,bool>> exp);
    IQueryable<Visitas> Listar(Expression<Func<Visitas, bool>> exp);
}
