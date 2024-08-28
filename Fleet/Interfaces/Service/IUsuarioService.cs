using Fleet.Controllers.Model.Request.Usuario;

namespace Fleet.Interfaces.Service
{
    public interface IUsuarioService
    {
        Task Criar(UsuarioRequest user);
        Task Atualizar(UsurioPutRequest user);
        Task UploadAsync(Stream stream, string fileExtension);
    }
}