namespace Fleet.Interfaces.Service
{
    public interface IBaseWorkspaceService<T> where T : class
    {
        void Inserir(string workspaceId, T objeto);
        void Atualizar(T objeto);
        void Deletar(string workspaceId, string id);
        List<T> Buscar(string workspaceId);
        T? Buscar(string workspaceId, string id);
        bool Validar(T objeto);

    }
}