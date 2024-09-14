using Azure.Core;
using Fleet.Controllers.Model.Request.Veiculo;
using Fleet.Controllers.Model.Response.Veiculo;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Repository;

namespace Fleet.Service
{
    public class VeiculoService(IVeiculoRepository veiculoRepository) : IVeiculoService
    {
        public async Task Cadastrar(VeiculoRequest request, int workspaceId)
        {
            var veiculo = new Veiculos
            {
                Ativo = true,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Ano = request.Ano,
                Placa = request.Placa,
                Combustivel = request.Combustivel,
                Odometro = request.Odometro,
                Situacao = Enums.VeiculoSituacaoEnum.Livre,
                WorkspaceId = workspaceId
            };

            await veiculoRepository.Cadastrar(veiculo);
        }


        public async Task<List<VeiculoResponse>> Listar(int workspaceId)
        {
          
           var veiculos =  await veiculoRepository.Listar(workspaceId);
           var veiculosresponse = veiculos.Select(ConverteVeiculoResponse).ToList();

            return veiculosresponse;
        }

        private VeiculoResponse ConverteVeiculoResponse(Veiculos veiculo)
        {
            var response = new VeiculoResponse
            {
                Marca = veiculo.Marca,
                Modelo = veiculo.Modelo,
                Ano = veiculo.Ano,
                Placa = veiculo.Placa,
                Combustivel = veiculo.Combustivel,
                Odometro = veiculo.Odometro,
                SituacaoEnum = veiculo.Situacao
            };

            return response;
        }


    }
}
