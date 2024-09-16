using Fleet.Controllers.Model.Request.Checklist;
using Fleet.Controllers.Model.Request.Lista;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

namespace Fleet.Controllers
{
    [ApiController]
    public class CheckListController(ICheckListService checkListService, IConfiguration configuration, ILoggedUser loggedUser) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/Retirar")]
        public IActionResult Retirar([FromRoute] string WorkspaceId, [FromBody] ChecklistRetiradaRequest request)
        {
            var checklist = new Checklist
            {
                DataRetirada = DateTime.Now,
                ObsRetirada = request.Observacao,
                OdometroRetirada = request.Odometro,
                VeiculoId = int.Parse(CriptografiaHelper.DescriptografarAes(request.VeiculoId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                WorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(WorkspaceId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                UsuarioId = loggedUser.UserId,
                ChecklistOpcaos = request.Opcoes.Select(x => new ChecklistOpcao
                {
                    Titulo = x.Titulo,
                    Descricao = x.Descricao,
                    Opcao = x.Opcao,
                }).ToList()
            };

            var fotos = request.Images.Select(x => x.ImageBase64).ToList();
            var extensao = request.Images.FirstOrDefault()?.extensao ?? string.Empty;

            checkListService.Retirar(checklist, fotos, extensao);
            return Ok();
        }


        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/Devolver")]
        public IActionResult Devolver([FromRoute] string WorkspaceId, [FromBody] CheckListDevolucaoRequest request)
        {
            //var lista = new Listas
            //{
            //    Nome = request.Nome,
            //    Tipo = request.Veiculo ? Enums.TipoListasEnum.Checklist : Enums.TipoListasEnum.Visita
            //};

            //listaService.Inserir(WorkspaceId, lista);
            return Ok();
        }
    }
}
