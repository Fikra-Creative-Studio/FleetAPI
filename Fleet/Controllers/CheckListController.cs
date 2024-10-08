using Fleet.Controllers.Model.Request.Checklist;
using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Controllers
{
    [ApiController]
    public class CheckListController(ICheckListService checkListService, IConfiguration configuration, ILoggedUser loggedUser) : ControllerBase
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret") ?? string.Empty; }

        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/Retirar")]
        public async Task<IActionResult> RetirarAsync([FromRoute] string WorkspaceId, [FromBody] ChecklistRetiradaRequest request)
        {
            var checklist = new Checklist
            {
                DataRetirada = DateTime.Now,
                ObsRetirada = request.Observacao,
                OdometroRetirada = request.Odometro,
                VeiculosId = int.Parse(CriptografiaHelper.DescriptografarAes(request.VeiculoId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                WorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(WorkspaceId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                UsuarioId = loggedUser.UserId,
                ChecklistOpcaos = request.Opcoes.Select(x => new ChecklistOpcao
                {
                    Titulo = x.Titulo,
                    Descricao = x.Descricao,
                    Opcao = x.Opcao,
                }).ToList()
            };

            var fotos = request.Images.Select(x => new Tuple<string, string>(x.ImagemBase64, x.extensao)).ToList();

            await checkListService.Retirar(checklist, fotos);
            return Ok();
        }


        [Authorize]
        [HttpPost("api/Workspace/{WorkspaceId}/Devolver")]
        public async Task<IActionResult> DevolverAsync([FromRoute] string WorkspaceId, [FromBody] CheckListDevolucaoRequest request)
        {
            var checklist = new Checklist
            {
                DataDevolucao = DateTime.Now,
                ObsDevolucao = request.Observacao,
                OdometroDevolucao = request.Odometro,
                VeiculosId = int.Parse(CriptografiaHelper.DescriptografarAes(request.VeiculoId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                WorkspaceId = int.Parse(CriptografiaHelper.DescriptografarAes(WorkspaceId, Secret) ?? throw new BussinessException("houve uma falha na retirada do veiculo")),
                UsuarioId = loggedUser.UserId,
                Avaria = request.Avaria,
                OsbAvaria = request.ObservacaoAvaria
            };

            var fotos = request.Images.Select(x => new Tuple<string, string>(x.ImagemBase64, x.extensao)).ToList();

            await checkListService.Devolver(checklist, fotos);
            return Ok();
        }
    }
}
