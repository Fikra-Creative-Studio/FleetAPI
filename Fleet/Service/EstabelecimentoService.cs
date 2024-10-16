﻿using System.Text.RegularExpressions;
using Fleet.Controllers.Model.Request.Estabelecimento;
using Fleet.Controllers.Model.Response.Estabelecimento;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;


namespace Fleet.Service
{
    public class EstabelecimentoService(IEstabelecimentoRepository estabelecimentoRepository, IUsuarioWorkspaceRepository usuarioWorkspaceRepository, IConfiguration configuration, ILoggedUser loggedUser) : IEstabelecimentoService
    {
        private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

        private EstabelecimentoResponse ConverteEstabelecimentoResponse(Estabelecimentos estabelecimento)
        {
            var response = new EstabelecimentoResponse
            {
                Id = CriptografiaHelper.CriptografarAes(estabelecimento.Id.ToString(), Secret),
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
                Email = estabelecimento.Email,
                Tipo = estabelecimento.Tipo
            };

            return response;
        }
        private int DecryptId(string encrypt, string errorMessage)
        {
            var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
            return int.Parse(decrypt);
        }


        public async Task<string> Cadastrar(EstabelecimentoRequest request, string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            if (!await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == decryptId && x.UsuarioId == loggedUser.UserId && x.Papel == Enums.PapelEnum.Administrador && x.Ativo)) throw new BussinessException("Você não tem permissão para realizar esta ação");

            if (!IsValidCnpj(request.Cnpj)) throw new BussinessException("CNPJ inválido");
            try { request.Cnpj = Regex.Replace(request.Cnpj, "[^0-9]", ""); } catch { }

            if (await estabelecimentoRepository.ExisteCnpj(request.Cnpj)) throw new BussinessException("CNPJ já cadastrado");

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
                WorkspaceId = decryptId,
                Tipo = request.Tipo
            };

            var result = await estabelecimentoRepository.Cadastrar(estabelecimento);
            return CriptografiaHelper.CriptografarAes(result.Id.ToString(), Secret) ?? throw new BussinessException("houve uma falha no cadastro do estabelecimento");
        }

        public async Task<List<EstabelecimentoResponse>> Listar(string workspaceId)
        {
            var decryptId = DecryptId(workspaceId, "Workspace inválido");
            var estabelecimento = await estabelecimentoRepository.Listar(decryptId);
            var veiculosresponse = estabelecimento.Select(ConverteEstabelecimentoResponse).ToList();

            return veiculosresponse;
        }

        public async Task Atualizar(EstabelecimentoRequest request, string estabelecimentoId)
        {
            var decryptId = DecryptId(estabelecimentoId, "Id inválido");

            var estabelecimento = await estabelecimentoRepository.Buscar(x => x.Id == decryptId);
            if (!await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == estabelecimento.WorkspaceId && x.UsuarioId == loggedUser.UserId && x.Papel == Enums.PapelEnum.Administrador && x.Ativo)) throw new BussinessException("Você não tem permissão para realizar esta ação");
            if (!IsValidCnpj(request.Cnpj)) throw new BussinessException("CNPJ inválido");
            if (await estabelecimentoRepository.ExisteCnpj(request.Cnpj, decryptId)) throw new BussinessException("CNPJ já cadastrado");

            if (estabelecimento != null)
            {
                var obj = new Estabelecimentos
                {
                    Id = estabelecimento.Id,
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
                    WorkspaceId = estabelecimento.WorkspaceId
                };
                await estabelecimentoRepository.Atualizar(obj);
            }
        }

        public async Task Deletar(string estabelecimentoId)
        {
            var decryptId = DecryptId(estabelecimentoId, "falha ao deletar estabelecimento.");
            var estabelecimento = await estabelecimentoRepository.Buscar(x => x.Id == decryptId);

            if (!await usuarioWorkspaceRepository.Existe(x => x.WorkspaceId == estabelecimento.WorkspaceId && x.UsuarioId == loggedUser.UserId && x.Papel == Enums.PapelEnum.Administrador && x.Ativo)) throw new BussinessException("Você não tem permissão para realizar esta ação");

            if (estabelecimento != null)
                await estabelecimentoRepository.Deletar(decryptId);
        }

        private static bool IsValidCnpj(string cnpj)
        {
            // Remove non-numeric characters
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            // Check if the CNPJ has 14 digits
            if (cnpj.Length != 14)
                return false;

            // Validate the check digits
            int[] multipliers1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multipliers2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(cnpj[i].ToString()) * multipliers1[i];

            int remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            if (int.Parse(cnpj[12].ToString()) != remainder)
                return false;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(cnpj[i].ToString()) * multipliers2[i];

            remainder = sum % 11;
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            if (int.Parse(cnpj[13].ToString()) != remainder)
                return false;

            return true;
        }
    }
}