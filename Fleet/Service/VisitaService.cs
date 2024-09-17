using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Service;

public class VisitaService (ILoggedUser loggedUser,
                            IVisitaRepository visitaRepository, 
                            IUsuarioWorkspaceRepository usuarioWorkspaceRepository,
                            IBucketService bucketService,
                            IConfiguration configuration): IVisitaService
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

    public async Task Criar(Visitas visita, List<Tuple<string, string, bool>> fotos)
    {
        if (!await UsuarioPertenceWorkspace(loggedUser.UserId, visita.WorkspaceId)) throw new BussinessException("você não pode realizar esta operação");

        if (string.IsNullOrEmpty(visita.Supervior.Trim())) throw new BussinessException("Campo supervisor e obrigatorio ");

        var visitaImages = new List<VisitaImagens>();
        foreach (var foto in fotos)
        {
            var filename = SalvarFotoAsync(foto.Item1, foto.Item2, false).GetAwaiter().GetResult();
            if (filename != null)
            {
                visitaImages.Add(new VisitaImagens
                {
                    FotoAssinatura = foto.Item3,
                    Url = filename,
                });
            }
        }

        visita.Imagens = visitaImages;
        visita.UsuarioId = loggedUser.UserId;
        visita.Data = DateTime.Now;
        await visitaRepository.Criar(visita);
    }

    public async Task<List<Visitas>> Buscar(string workspaceId)
    {
        var decryptWorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("houve uma falha na busca das visitas"));

        if(!await UsuarioPertenceWorkspace(loggedUser.UserId, decryptWorkspaceId)) throw new BussinessException("você não pode realizar esta operação");

        return visitaRepository.Listar(v => v.WorkspaceId == decryptWorkspaceId && v.UsuarioId == loggedUser.UserId)
                               .Include(v => v.Estabelecimentos)
                               .ToList();
    }

    private async Task<bool> UsuarioPertenceWorkspace(int usuarioId, int workspaceId) 
        => await usuarioWorkspaceRepository.Existe(uw => uw.UsuarioId == usuarioId &&  uw.WorkspaceId == workspaceId);

    private async Task<string> SalvarFotoAsync(string base64, string extensao, bool retirada)
    {
        string NomeFoto = string.Empty;
        if (!string.IsNullOrEmpty(base64))
        {
            try
            {
                var bytes = Convert.FromBase64String(base64);
                NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), extensao, "maintenance") ?? throw new BussinessException("não foi possivel salvar a imagem");
            }
            catch (Exception)
            {
                NomeFoto = string.Empty;
            }
        }

        return NomeFoto;
    }
}