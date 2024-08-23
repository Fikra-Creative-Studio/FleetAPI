using System;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository;

public class UsuarioWorkspaceRepository(ApplicationDbContext context)  : IUsuarioWorkspaceRepository
{
    public async Task Criar(UsuarioWorkspace usuarioWorkspace)
    {
        await context.UsuarioWorkspaces.AddAsync(usuarioWorkspace);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Existe(int usuarioId, int workspaceId)
    {
        return await context.UsuarioWorkspaces.AnyAsync(x => x.UsuarioId == usuarioId && x.WorkspaceId == workspaceId);
    }
}
