using System;
using Fleet.Models;

namespace Fleet.Interfaces.Repository;

public interface IWorkspaceRepository
{
    Task<Workspace> Criar(Workspace workspace);
    Task Deletar (int id);
    Task Atualizar(int id,Workspace workspaceAtualizado);
    Task<bool> Existe(int id);
    Task<bool> ExisteCnpj(string cnpj, int? id = null);
}
