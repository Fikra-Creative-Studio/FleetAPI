using Fleet.Controllers.Model.Request.Usuario;
using Fleet.Controllers.Model.Response.Usuario;

namespace Fleet.Interfaces.Service
{
    public interface IUsuarioService
    {
        Task Criar(UsuarioRequest user);
        Task Atualizar(string id, UsurioPutRequest user);
        Task Deletar(string id);
        Task<List<UsuarioResponse>> Listar();
        Task UploadAsync(string id, Stream stream, string fileExtension);
    }
}