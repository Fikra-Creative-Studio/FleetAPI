using System;
using System.Linq.Expressions;
using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Repository;

public class UsuarioWorkspaceRepository(ApplicationDbContext context)  : IUsuarioWorkspaceRepository
{
    public async Task AtualizarPapel(int usuarioId, int workspaceId, PapelEnum papel)
    {
        var usuarioWorkspace = await context.UsuarioWorkspaces
                                            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId 
                                                                && x.WorkspaceId == workspaceId);
        usuarioWorkspace.Papel = papel;
        await context.SaveChangesAsync();
    }


    public async Task Criar(UsuarioWorkspace usuarioWorkspace)
    {
        await context.UsuarioWorkspaces.AddAsync(usuarioWorkspace);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Existe(Expression<Func<UsuarioWorkspace,bool>> exp)
    {
        return await context.UsuarioWorkspaces.AnyAsync(exp);
    }

    public async Task Remover(int usuarioId, int workspaceId)
    {
        var usuarioWorkspace = await context.UsuarioWorkspaces.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.WorkspaceId == workspaceId);
        context.UsuarioWorkspaces.Remove(usuarioWorkspace);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UsuarioWorkspaceAdmin(int usuarioId, int workspaceId)
    {
        return await context.UsuarioWorkspaces.AnyAsync(
                                            uw => uw.UsuarioId == usuarioId && 
                                            uw.WorkspaceId == workspaceId && 
                                            uw.Papel == PapelEnum.Administrador);
    }
}
