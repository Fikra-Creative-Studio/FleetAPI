using System;
using Fleet.Controllers.Model.Request.Visita;
using Fleet.Controllers.Model.Request.VisitaOpcao;
using Fleet.Controllers.Model.Response.Estabelecimento;
using Fleet.Controllers.Model.Response.Veiculo;
using Fleet.Controllers.Model.Response.Visita;
using Fleet.Controllers.Model.Response.Workspace;
using Fleet.Helpers;
using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Service;

public class VisitaService (ILoggedUser loggedUser,
                            IVisitaRepository visitaRepository, 
                            IUsuarioWorkspaceRepository usuarioWorkspaceRepository,
                            IVeiculoRepository veiculoRepository,
                            IWorkspaceRepository workspaceRepository,
                            IUsuarioRepository usuarioRepository,
                            IEstabelecimentoRepository estabelecimentoRepository,
                            IBucketService bucketService,
                            IConfiguration configuration): IVisitaService
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

    private async Task<VisitaImagens> ConverteImagens(VisitaImagensRequest foto)
    {
        string NomeFoto = string.Empty;
        if (!string.IsNullOrEmpty(foto.ImagemBase64))
        {
            try
            {
                var bytes = Convert.FromBase64String(foto.ImagemBase64);
                NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), foto.ExtensaoImagem, "maintenance") ?? throw new BussinessException("não foi possivel salvar a imagem");
            }
            catch (Exception)
            {
                NomeFoto = string.Empty;
            }
        }
        var visitaImagen = new VisitaImagens
        {
            Url = NomeFoto
        };
        return visitaImagen;
    }

    private static VisitaOpcao CriarOpcao(VisitaOpcaoRequest visitaOpcao)
    {
        string DescricaoOpcao = string.Empty;
        string TituloOpcao = string.Empty;
        if (!string.IsNullOrEmpty(visitaOpcao.Titulo.Trim()))
            TituloOpcao = visitaOpcao.Titulo;

        if (!string.IsNullOrEmpty(visitaOpcao.Descricao.Trim()))
            DescricaoOpcao = visitaOpcao.Descricao;

        return new VisitaOpcao {
            Titulo = TituloOpcao,
            Descricao = DescricaoOpcao
        };
    }

    public async Task Criar(CriarVisitaRequest request)
    {
        if (string.IsNullOrEmpty(request.Supervisor.Trim()))
            throw new BussinessException("Campo supervisor e obrigatorio ");

        var decryptWorkspaceId = DecryptId(request.WorkspaceId, "WorkspaceId inválido");
        var decryptVeiculoId = DecryptId(request.VeiculoId, "VeiculoId inválido");
        var decryptVEstabelecimentoId = DecryptId(request.EstabelecimentoId, "EstabelecimentoId inválido");
        
        if (!await UsuarioPertenceWorkspace(loggedUser.UserId, decryptWorkspaceId))
            throw new BussinessException("Usuário não pertence a esse workspace");

        var usuario = await usuarioRepository.Buscar(v => v.Id == loggedUser.UserId) ?? throw new BussinessException("Usuario não foi encontrado");
        var workspace = await workspaceRepository.Buscar(v => v.Id == decryptWorkspaceId) ?? throw new BussinessException("Workspace não foi encontrado");
        var veiculo = await veiculoRepository.Buscar(v => v.Id == decryptVeiculoId) ?? throw new BussinessException("Veiculo não foi encontrado");
        var estabelecimento = await estabelecimentoRepository.Buscar(v => v.Id == decryptVEstabelecimentoId) ?? throw new BussinessException("Estabelecimento não foi encontrado");
        
        var visita = new Visitas {
            Observacao = request.Observacao,
            Supervior = request.Supervisor,
            Data = request.Data,
            VeiculoId = veiculo.Id,
            Veiculos = veiculo,
            WorkspaceId = workspace.Id,
            Workspace = workspace,
            UsuarioId = usuario.Id,
            GPS = request.GPS,
            Usuario = usuario,
            EstabelecimentoId = estabelecimento.Id,
            Estabelecimentos = estabelecimento
        };

        var tasks = request.Imagens.Select(ConverteImagens).ToList();
        var imagens = await Task.WhenAll(tasks);
        visita.Imagens = imagens.ToList();

        List<VisitaOpcao> opcoes = [];
        foreach (var opcao in request.Opcoes)
        {
            opcoes.Add(CriarOpcao(opcao));
        }

        visita.Opcoes = opcoes.ToList();
        
        await visitaRepository.Criar(visita);
    }

    public List<ListarVisitasResponse> Buscar(string workspaceId)
    {
        var decryptWorkspaceId = DecryptId(workspaceId, "WorkspaceId inválido");
        var visitas = visitaRepository.Listar(v => v.WorkspaceId == decryptWorkspaceId && v.UsuarioId == loggedUser.UserId)
                                        .Include(v => v.Veiculos)
                                        .Include(v => v.Estabelecimentos)
                                        .Include(v => v.Imagens)
                                        .Include(v => v.Opcoes)
                                        .ToList();
        
        return visitas.Select( v => 
            new ListarVisitasResponse {
                Id = CriptografiaHelper.CriptografarAes(v.Id.ToString(), Secret),
                Observacao = v.Observacao,
                Supervisor = v.Supervior,
                Estabelecimento = new  EstabelecimentoNomeEIdResponse {
                    Id = CriptografiaHelper.CriptografarAes( v.Estabelecimentos.Id.ToString(), Secret),
                    Fantasia = v.Estabelecimentos.Fantasia
                },
                Veiculo = new VeiculoResponse {
                    Id = CriptografiaHelper.CriptografarAes(v.Veiculos.Id.ToString(), Secret),
                    Marca = v.Veiculos.Marca,
                    Ano = v.Veiculos.Ano,
                    Combustivel = v.Veiculos.Combustivel,
                    Modelo = v.Veiculos.Modelo,
                    Placa = v.Veiculos.Placa,
                    Foto = v.Veiculos.Foto,
                },
                Opcoes = v.Opcoes.Select( o => new VisitaOpcaoResponse {
                    Id = CriptografiaHelper.CriptografarAes(o.Id.ToString(), Secret),
                    Descricao= o.Descricao,
                    Titulo = o.Titulo
                }).ToList(),
                Imagens = v.Imagens.Select( i => new VisitaImagensResponse {
                    Id = CriptografiaHelper.CriptografarAes(i.Id.ToString(), Secret),
                    Url = i.Url
                }).ToList()
            }
        ).ToList();
    }

    private async Task<bool> UsuarioPertenceWorkspace(int usuarioId, int workspaceId)
    {
        return await usuarioWorkspaceRepository.Existe(uw => uw.UsuarioId == usuarioId && 
                                                            uw.WorkspaceId == workspaceId);
    }

    private int DecryptId(string encrypt, string errorMessage)
    {
        var decrypt = CriptografiaHelper.DescriptografarAes(encrypt, Secret) ?? throw new BussinessException(errorMessage);
        return int.Parse(decrypt);
    }

   
}
