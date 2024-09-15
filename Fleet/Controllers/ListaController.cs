using Azure.Core;
using Fleet.Controllers.Model.Request.Lista;
using Fleet.Controllers.Model.Response.Lista;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fleet.Controllers
{
    [ApiController]
    public class ListaController(IListaService listaService, IConfiguration configuration) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/[Controller]")]
        public IActionResult Criar([FromRoute] string WorkspaceId, [FromBody] CriarListaRequest request)
        {
            var lista = new Listas
            {
                Nome = request.Nome,
                Tipo = request.Veiculo ? Enums.TipoListasEnum.Checklist : Enums.TipoListasEnum.Visita
            };

            listaService.Inserir(WorkspaceId, lista);
            return Created();
        }

        [Authorize]
        [HttpGet("api/Workspace/{WorkspaceId}/[Controller]")]
        public IActionResult Buscar([FromRoute] string WorkspaceId)
        {
            var listas = listaService.Buscar(WorkspaceId);

            return Ok(listas.Select(x => new BuscarListaResponse
            {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret) ?? throw new BussinessException("houve uma falha na busca da listagem"),
                Nome = x.Nome,
                Padrao = x.Padrao,
                Tipo = x.Tipo
            }));
        }

        [Authorize]
        [HttpPut("api/Workspace/{WorkspaceId}/[Controller]/{ListaId}")]
        public IActionResult Atualizar([FromRoute] string WorkspaceId, [FromRoute] string ListaId, [FromBody] AtualizarListaRequest request)
        {
            listaService.Atualizar(new Listas
            {
                Id = int.Parse(CriptografiaHelper.DescriptografarAes(ListaId, Secret) ?? throw new BussinessException("houve uma falha na atualizacao da listagem")),
                WorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(WorkspaceId, Secret) ?? throw new BussinessException("houve uma falha na atualizacao da listagem")),
                Nome = request.Nome,
            });
            return Ok();
        }

        [Authorize]
        [HttpPatch("api/Workspace/{WorkspaceId}/[Controller]/{ListaId}/[Action]")]
        public IActionResult Padrao([FromRoute] string WorkspaceId, [FromRoute] string ListaId)
        {
            listaService.TornarPadrao(WorkspaceId, ListaId);
            return Ok();
        }

        [Authorize]
        [HttpDelete("api/Workspace/{WorkspaceId}/[Controller]/{ListaId}")]
        public IActionResult Deletar([FromRoute] string WorkspaceId, [FromRoute] string ListaId)
        {
            listaService.Deletar(WorkspaceId, ListaId);
            return Ok();
        }

    }
}
