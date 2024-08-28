using Fleet.Interfaces.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class UsuarioRepository(ApplicationDbContext context) : IUsuarioRepository
    {

    public async Task<List<Usuario>> BuscarPorWorkspace(int workspaceId, int loggedUserId)
    {
        return await context.Usuarios
                        .Include(u => u.UsuarioWorkspaces)
                            .ThenInclude(uw => uw.Workspace)
                        .Where(u => u.UsuarioWorkspaces.Any(uw => uw.WorkspaceId == workspaceId) && u.Id != loggedUserId)
                        .ToListAsync();
    }
        public async Task Criar(Usuario user)
        {
            await context.Usuarios.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task Deletar(int id)
        {
            context.Usuarios.Remove(await context.Usuarios.FindAsync(id));
            await context.SaveChangesAsync();
        }

        public async Task Atualizar(int id, Usuario usuarioAtualizado)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            usuario.CPF = usuarioAtualizado.CPF;
            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Email = usuarioAtualizado.Email;
            await context.SaveChangesAsync();
        }

        public async Task Atualizar(Usuario usuarioAtualizado)
        {
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmail(string email, int? id = null)
        {
            if(id == null)
                return await context.Usuarios.AnyAsync(x => x.Email == email);
            return  await context.Usuarios.AnyAsync(x => x.Email == email && x.Id != id);
        }

        public async Task<bool> ExisteCpf(string cpf, int? id = null)
        {
            if(id == null)
                return await  context.Usuarios.AnyAsync(x => x.CPF == cpf);
            return  await context.Usuarios.AnyAsync(x => x.CPF == cpf && x.Id != id);
        }

        public async Task<Usuario?> Buscar(Expression<Func<Usuario,bool>> exp)
        {
            return await context.Usuarios.FirstOrDefaultAsync(exp);
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Usuarios.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Usuario>> Listar()
        {
            return await context.Usuarios.ToListAsync();
        }

        public IQueryable<Usuario> Listar(Expression<Func<Usuario, bool>> exp)
        {
            return context.Usuarios.Where(exp).AsQueryable();
        }


        public async Task AtualizarSenha(Usuario novaSenha)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Token == novaSenha.Token);
            if(usuario != null)
            {
                usuario.Token = string.Empty;
                usuario.Senha = novaSenha.Senha;
                await context.SaveChangesAsync();
            }
        }
    }
}