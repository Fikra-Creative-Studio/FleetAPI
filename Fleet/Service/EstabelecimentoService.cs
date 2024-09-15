using Fleet.Controllers.Model.Request.Estabelecimento;
using Fleet.Controllers.Model.Response.Estabelecimento;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;


namespace Fleet.Service
{
    public class EstabelecimentoService(IEstabelecimentoRepository estabelecimentoRepository, IConfiguration configuration) : IEstabelecimentoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

        private EstabelecimentoResponse ConverteEstabelecimentoResponse(Estabelecimentos estabelecimento)
        {
            var response = new EstabelecimentoResponse
            {
                Cnpj = estabelecimento.Cnpj,
                Razao = estabelecimento.Razao,
                Fantasia = estabelecimento.Fantasia,
                Telefone = estabelecimento.Telefone,
                Cep = estabelecimento.Cep,
                Rua = estabelecimento.Rua,
                Numero = estabelecimento.Numero,
                Bairro = estabelecimento.Bairro,
                Cidade = estabelecimento.Cidade,
                Estado = estabelecimento.Estado,
                Email = estabelecimento.Email
            };

            return response;
        }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }


        public async Task Cadastrar(EstabelecimentoRequest request, string workspaceId)
        {
            if (await estabelecimentoRepository.ExisteCnpj(request.Cnpj)) throw new BussinessException("CNPJ já cadastrado");

            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            var estabelecimento = new Estabelecimentos
            {
                Ativo = true,
                Cnpj = request.Cnpj,
                Razao = request.Razao,
                Fantasia = request.Fantasia,
                Telefone = request.Telefone,
                Cep = request.Cep,
                Rua = request.Rua,
                Numero = request.Numero,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                Estado = request.Estado,
                Email = request.Email,
                WorkspaceId = decryptId
            };

            await estabelecimentoRepository.Cadastrar(estabelecimento);
        }

        public async Task<List<EstabelecimentoResponse>> Listar(string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            var estabelecimento = await estabelecimentoRepository.Listar(decryptId);
            var veiculosresponse = estabelecimento.Select(ConverteEstabelecimentoResponse).ToList();

            return veiculosresponse;
        }

    }
}