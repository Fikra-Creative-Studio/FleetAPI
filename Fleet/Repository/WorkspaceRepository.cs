using System;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository;

public class WorkspaceRepository(ApplicationDbContext context) : IWorkspaceRepository
{

    public async Task<Workspace> Criar(Workspace workspace)
    {
        await context.Workspaces.AddAsync(workspace);
        await context.SaveChangesAsync();
        return workspace;
    }

    public async Task Deletar(int id)
    {
        context.Workspaces.Remove(await context.Workspaces.FirstOrDefaultAsync(x => x.Id == id));
         await context.SaveChangesAsync();
    }

    public async Task<bool> Existe(int id)
    {
       return await context.Workspaces.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExisteCnpj(string cnpj, int? id = null)
    {
        if (id != null)
            return await context.Workspaces.AnyAsync(x => x.Cnpj == cnpj && x.Id != id);
        return await context.Workspaces.AnyAsync(x => x.Cnpj == cnpj);
    }

    public async Task Atualizar(int id, Workspace workspaceAtualizado)
    {
        var workspace = await context.Workspaces.FirstOrDefaultAsync(x => x.Id == id);
        workspace.Cnpj = workspace.Cnpj;
        workspace.Fantasia = workspace.Fantasia;
        workspace.UrlImagem = workspace.UrlImagem;
        await context.SaveChangesAsync();
    }
}
