using System;
using AutoMapper;
using Fleet.Controllers.Model.Request.Workspace;
using Fleet.Enums;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Validators;

namespace Fleet.Service;

public class WorkspaceService(IWorkspaceRepository workspaceRepository, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IUsuarioRepository usuarioRepository ,IMapper mapper, IConfiguration configuration) : IWorskpaceService
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }
    public Task Atualizar(string id)
    {
        throw new NotImplementedException();
    }

    public async Task Criar(string usuarioId, WorkspaceRequest request)
    {
        var userId = int.Parse(CriptografiaHelper.DescriptografarAes(usuarioId, Secret));
        var usuario = await usuarioRepository.Buscar(x => x.Id == userId) ?? throw new BussinessException("Usuario não inálido");
        var workspace = mapper.Map<Workspace>(request);
        await Validar(workspace, WorkspaceRequestEnum.Criar);
        var workspaceCriado = await workspaceRepository.Criar(workspace);
        var usuarioWorkspace = new UsuarioWorkspace {
            Usuario = usuario,
            UsuarioId = usuario.Id,
            Workspace = workspaceCriado,
            WorkspaceId = workspaceCriado.Id,
            Ativo = true
        };
        await usuarioWorkspaceRepository.Criar(usuarioWorkspace);
    }

     private async Task Validar(Workspace workspace, WorkspaceRequestEnum request)
        {
            var validator = new WorkspaceValidator(workspaceRepository, request);
            var validationResult = await validator.ValidateAsync(workspace);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BussinessException(errors);
            }
        }
}
