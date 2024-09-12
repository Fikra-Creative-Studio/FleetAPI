using System.Linq.Expressions;
using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IUsuarioWorkspaceRepository
{
    Task Criar(UsuarioWorkspace usuarioWorkspace);
    Task<bool> Existe(Expression<Func<UsuarioWorkspace, bool>> exp);
    Task AtualizarPapel(int usuarioId, int workspaceId, PapelEnum papel);
    Task Remover(int usuarioId, int workspaceId);
}
