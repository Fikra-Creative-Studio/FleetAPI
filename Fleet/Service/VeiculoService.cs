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

        public async Task<string> Cadastrar(VeiculoRequest request, string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            if (await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == decryptId && x.UsuarioId == loggedUser.UserId && x.Papel != Enums.PapelEnum.Administrador)) throw new BussinessException("Você não tem permissão para realizar esta ação");

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
                Tipo = request.Tipo,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Ano = request.Ano,
                Placa = request.Placa,
                Chassi = request.Chassi,
                Combustivel = request.Combustivel,
                Cor = request.Cor,
                Odometro = request.Odometro,
                Renavam = request.Renavam,
                Seguradora = request.Seguradora,
                VencimentoSeguro = request.VencimentoSeguro,
                Observacao = request.Observacao,
                EmUsoPor = string.Empty,
                Manutencao = false,
                WorkspaceId = decryptId,
                Foto = NomeFoto,
            };

            var result = await veiculoRepository.Cadastrar(veiculo);
            return CriptografiaHelper.CriptografarAes(result.Id.ToString(), Secret);
        }

        public async Task Deletar(string veiculoId)
        {
            var decryptId = DecryptId(veiculoId, "falha ao deletar veiculo");
            var veiculo = await veiculoRepository.Buscar(x => x.Id == decryptId);
            if (veiculo != null)
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
                Tipo = veiculo.Tipo,
                Marca = veiculo.Marca,
                Modelo = veiculo.Modelo,
                Ano = veiculo.Ano,
                Placa = veiculo.Placa,
                Chassi = veiculo.Chassi,
                Combustivel = veiculo.Combustivel,
                Cor = veiculo.Cor,
                Odometro = veiculo.Odometro,
                Observacao = veiculo.Observacao,
                Renavam = veiculo.Renavam,
                Seguradora = veiculo.Seguradora,
                VencimentoSeguro = veiculo.VencimentoSeguro,
                EmUso = !string.IsNullOrEmpty(veiculo.EmUsoPor),
                EmUsoPor = veiculo.EmUsoPor,
                Manutencao = veiculo.Manutencao,
                Foto = veiculo.Foto,
                Comigo = veiculo.UsuariosId == null ? false : veiculo.UsuariosId == loggedUser.UserId
            };

            return response;
        }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }

        public async Task Atualizar(VeiculoPutRequest request, string veiculoId)
        {
            var decryptId = DecryptId(veiculoId, "veículo inválido");
      
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
                Id = decryptId,
                Ativo = true,
                Tipo = request.Tipo,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Ano = request.Ano,
                Placa = request.Placa,
                Chassi = request.Chassi,
                Combustivel = request.Combustivel,
                Cor = request.Cor,
                Odometro = request.Odometro,
                Renavam = request.Renavam,
                Seguradora = request.Seguradora,
                VencimentoSeguro = request.VencimentoSeguro,
                Observacao = request.Observacao,
                EmUsoPor = string.Empty,
                Manutencao = false,
                WorkspaceId = DecryptId(request.WorkspaceId, "Workspace inválido"),
                Foto = NomeFoto,
                UsuariosId = string.IsNullOrEmpty(request.UsuarioId) ? null : DecryptId(request.UsuarioId, "Usuario inválido"),
            };

            await veiculoRepository.Atualizar(veiculo);
        }

    }
}
