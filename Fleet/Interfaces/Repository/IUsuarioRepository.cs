﻿using Fleet.Models;
using System.Linq.Expressions;

namespace Fleet.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Criar(Usuario user);
        Task<bool> ExisteEmail(string email, int? id = null);
        Task<bool> ExisteCpf(string cpf, int? id = null);
        Task Deletar(int id);
        Task Atualizar(int id, Usuario usuarioAtualizado);
        Task Atualizar(Usuario usuarioAtualizado);
        Task<bool> Existe(int id);
        Task<List<Usuario>> Listar();
        IQueryable<Usuario> Listar(Expression<Func<Usuario, bool>> exp);
        Task AtualizarSenha(Usuario novaSenha);
        Task<Usuario?> Buscar(Expression<Func<Usuario, bool>> exp);
    }
}