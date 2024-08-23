using Fleet.Interfaces.Repository;
using Fleet.Repository;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fleet.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Criar(Usuario user)
        {
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(int id)
        {
            _context.Usuarios.Remove(await _context.Usuarios.FindAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(int id, Usuario usuarioAtualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            usuario.CPF = usuarioAtualizado.CPF;
            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Email = usuarioAtualizado.Email;
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Usuario usuarioAtualizado)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmail(string email, int? id = null)
        {
            if(id == null)
                return await _context.Usuarios.AnyAsync(x => x.Email == email);
            return  await _context.Usuarios.AnyAsync(x => x.Email == email && x.Id != id);
        }

        public async Task<bool> ExisteCpf(string cpf, int? id = null)
        {
            if(id == null)
                return await  _context.Usuarios.AnyAsync(x => x.CPF == cpf);
            return  await _context.Usuarios.AnyAsync(x => x.CPF == cpf && x.Id != id);
        }

        public async Task<Usuario?> Buscar(Expression<Func<Usuario,bool>> exp)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(exp);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Usuarios.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task AtualizarSenha(Usuario novaSenha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Token == novaSenha.Token);
            if(usuario != null)
            {
                usuario.Token = string.Empty;
                usuario.Senha = novaSenha.Senha;
                await _context.SaveChangesAsync();
            }
        }
    }
}