using System;
using System.Linq.Expressions;
using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IWorkspaceRepository
{
    Task<Workspace> Criar(Workspace workspace);
    Task Deletar (int id);
    Task Atualizar(int id,Workspace workspaceAtualizado);
    Task<bool> Existe(int id);
    Task<bool> ExisteCnpj(string cnpj, int? id = null);
    Task<Workspace?> Buscar(Expression<Func<Workspace, bool>> exp);
}
