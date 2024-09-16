using Fleet.Interfaces.Repository;
using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class CheckListRepository(ApplicationDbContext applicationDbContext) : ICheckListRepository
    {
        public void Atualizar(Checklist objeto)
        {
            var existingObj = applicationDbContext.Checklists.Find(objeto.Id);
            if (existingObj != null)
            {
                applicationDbContext.Entry(existingObj).CurrentValues.SetValues(objeto);
                applicationDbContext.SaveChanges();
            }
        }

        public bool Existe(Expression<Func<Checklist, bool>> expression)
        {
            return applicationDbContext.Checklists.Any(expression);    
        }

        public void Inserir(Checklist objeto)
        {
            applicationDbContext.Add(objeto);
            applicationDbContext.SaveChanges();
        }

        public Checklist? Buscar(int workspaceId, int veiculoId, int usuarioId)
           => applicationDbContext.Checklists.FirstOrDefault(x => x.VeiculosId == veiculoId && x.Ativo == true && x.WorkspaceId == workspaceId && x.DataDevolucao == null && x.UsuarioId == usuarioId);
    }
}
