using Fleet.Models;
using Fleet.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository
{
    public class BaseWorkspaceRepository<T> where T : DBWorkspaceEntity
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BaseWorkspaceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Inserir(T objeto)
        {
            _applicationDbContext.Add(objeto);
            _applicationDbContext.SaveChanges();
        }

        public void Atualizar( T objeto)
        {
            var existingObj = _applicationDbContext.Set<T>().Find(objeto.Id);
            if (existingObj != null)
            {
                _applicationDbContext.Entry(existingObj).CurrentValues.SetValues(objeto);
                _applicationDbContext.SaveChanges();
            }
        }

        public void Deletar(int workspaceId, int id)
        {
            var objeto = this.Buscar(workspaceId, id);
            if (objeto != null)
            {
                objeto.Ativo = false;
                this.Atualizar(objeto);
            }
        }

        public List<T> Buscar(int workspaceId)
            => _applicationDbContext.Set<T>()
                                    .Where(x => x.Ativo && x.WorkspaceId == workspaceId)
                                    .ToList();
        public T? Buscar(int workspaceId, int id)
           => _applicationDbContext.Set<T>()
                .Where(x => x.Id == id && x.Ativo == true && x.WorkspaceId == workspaceId)
                .FirstOrDefault();


    }
}