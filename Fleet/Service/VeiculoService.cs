using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Controllers.Model.Response.Veiculo;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Repository;

namespace Fleet.Service
{
    public class VeiculoService(IVeiculoRepository veiculoRepository, IBucketService bucketService, IConfiguration configuration, ILoggedUser loggedUser, IUsuarioWorkspaceRepository usuarioWorkspaceRepository) : IVeiculoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

        public async Task Cadastrar(VeiculoRequest request, string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            if(await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == decryptId && x.UsuarioId == loggedUser.UserId && x.Papel != Enums.PapelEnum.Administrador)) throw new BussinessException("Você não tem permissão para realizar esta ação");

            string NomeFoto = string.Empty;
            if (!string.IsNullOrEmpty(request.ImagemBase64))
            {
                try
                {
                    var bytes = Convert.FromBase64String(request.ImagemBase64);
                    NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), request.ExtensaoImagem, "car") ?? throw new BussinessException("não foi possivel salvar a imagem");
                }
                catch (Exception)
                {
                    NomeFoto = string.Empty;
                }
            }

            var veiculo = new Veiculos
            {
                Ativo = true,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Ano = request.Ano,
                Placa = request.Placa,
                Combustivel = request.Combustivel,
                Odometro = request.Odometro,
                Status = false,
                Manutencao = false,
                WorkspaceId = decryptId,
                Foto = NomeFoto
            };

            await veiculoRepository.Cadastrar(veiculo);
        }

        public async Task Deletar(string veiculoId)
        {
            var decryptId = DecryptId(veiculoId,"falha ao deletar veiculo");
            var veiculo = await veiculoRepository.Buscar(x => x.Id == decryptId);
            if(veiculo != null)
            {
                if (await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == veiculo.WorkspaceId && x.UsuarioId == loggedUser.UserId && x.Papel != Enums.PapelEnum.Administrador)) throw new BussinessException("Você não tem permissão para realizar esta ação");

                await veiculoRepository.Deletar(veiculo.Id);
            }
        }

        public async Task<List<VeiculoResponse>> Listar(string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            var veiculos = await veiculoRepository.Listar(decryptId);
            var veiculosresponse = veiculos.Select(ConverteVeiculoResponse).ToList();

            return veiculosresponse;
        }

        private VeiculoResponse ConverteVeiculoResponse(Veiculos veiculo)
        {
            var response = new VeiculoResponse
            {
                Id = CriptografiaHelper.CriptografarAes(veiculo.Id.ToString(), Secret),
                Marca = veiculo.Marca,
                Modelo = veiculo.Modelo,
                Ano = veiculo.Ano,
                Placa = veiculo.Placa,
                Combustivel = veiculo.Combustivel,
                Odometro = veiculo.Odometro,
                Status = veiculo.Status,
                Manutencao = veiculo.Manutencao,
                Foto = veiculo.Foto

            };

            return response;
        }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }

    }
}
