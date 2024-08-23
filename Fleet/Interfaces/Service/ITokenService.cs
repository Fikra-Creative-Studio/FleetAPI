using Fleet.Models;

namespace Fleet.Interfaces.Service
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);

    }
}