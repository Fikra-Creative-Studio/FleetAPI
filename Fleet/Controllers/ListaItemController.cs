using Fleet.Controllers.Model.Request.ListaItem;
using Fleet.Controllers.Model.Response.ListaItem;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class ListaItemController(IConfiguration configuration, IListaItemService listaItemService) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [Authorize]
        [HttpPost("api/Lista/{listaId}/Item")]
        public IActionResult Criar([FromRoute] string listaId, [FromBody] CriarListaItemRequest request)
        {
            var lista = new ListasItens
            {
                ListasId = int.Parse(CriptografiaHelper.DescriptografarAes(listaId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem")),
                Titulo = request.Titulo,
                Descrição = request.Descricao,
            };

            listaItemService.Inserir(lista);
            return Created();
        }

        [Authorize]
        [HttpGet("api/Workspace/{workspaceId}/ListaPadrao/Veiculo/Item")]
        public IActionResult BuscarVeiculoPadrao([FromRoute] string workspaceId)
        {
            var workspace = int.Parse(CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem"));
            var itens = listaItemService.Buscar(workspace, Enums.TipoListasEnum.Checklist);
            return Ok(itens.Select(x => new BuscarListaPadraoResponse
            {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret),
                Titulo = x.Titulo,
                Descricao = x.Descrição
            }));
        }

        [Authorize]
        [HttpGet("api/Workspace/{workspaceId}/ListaPadrao/Visita/Item")]
        public IActionResult BuscarVisitaPadrao([FromRoute] string workspaceId)
        {
            var workspace = int.Parse(CriptografiaHelper.DescriptografarAes(workspaceId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem"));
            var itens = listaItemService.Buscar(workspace, Enums.TipoListasEnum.Visita);
            return Ok(itens.Select(x => new BuscarListaPadraoResponse
            {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret),
                Titulo = x.Titulo,
                Descricao = x.Descrição
            }));
        }

        [Authorize]
        [HttpGet("api/Lista/{listaId}/Item")]
        public IActionResult Buscar([FromRoute] string listaId)
        {
            var lista = int.Parse(CriptografiaHelper.DescriptografarAes(listaId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem"));
            var itens = listaItemService.BuscarPorLista(lista);

            return Ok(itens.Select(x => new BuscarListaPadraoResponse
            {
                Id = CriptografiaHelper.CriptografarAes(x.Id.ToString(), Secret),
                Titulo = x.Titulo,
                Descricao = x.Descrição
            }));
        }

        [Authorize]
        [HttpPut("api/Lista/{listaId}/Item/{listaItemId}/")]
        public IActionResult Atualizar([FromRoute] string listaId, [FromRoute] string listaItemId, [FromBody] CriarListaItemRequest request)
        {
            var item = new ListasItens
            {
                Id = int.Parse(CriptografiaHelper.DescriptografarAes(listaItemId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem")),
                ListasId = int.Parse(CriptografiaHelper.DescriptografarAes(listaId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem")),
                Titulo = request.Titulo,
                Descrição = request.Descricao,
            };

            listaItemService.Atualizar(item);
            return Ok();
        }

        [Authorize]
        [HttpDelete("api/Lista/[controller]/{listaItemId}")]
        public IActionResult Deletar([FromRoute] string listaItemId)
        {
            var id = int.Parse(CriptografiaHelper.DescriptografarAes(listaItemId, Secret) ?? throw new BussinessException("houve uma falha na criação da listagem"));
            listaItemService.Deletar(id);
            return Ok();
        }
    }
}
