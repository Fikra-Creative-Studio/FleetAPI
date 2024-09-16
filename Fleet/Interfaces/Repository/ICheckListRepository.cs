using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface ICheckListRepository
    {
        void Inserir(Checklist objeto);
        void Atualizar(Checklist objeto);
        bool Existe(Expression<Func<Checklist,bool>> expression);
        Checklist? Buscar(int workspaceId, int veiculoId, int usuarioId);
    }
}
