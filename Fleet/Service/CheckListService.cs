﻿using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Models;

namespace Fleet.Service
{
    public class CheckListService(IBucketService bucketService, ICheckListRepository checkListRepository, ILoggedUser loggedUser, IVeiculoRepository veiculoRepository, IUsuarioRepository usuarioRepository) : ICheckListService
    {
        public async Task Retirar(Checklist objeto, List<Tuple<string, string>> fotos)
        {
            if (checkListRepository.Existe(x => x.VeiculosId == objeto.VeiculosId && x.DataDevolucao == null)) throw new BussinessException("Este veiculo já esta em uso");

            var checklistImages = new List<ChecklistImagens>();
            foreach (var foto in fotos) { 
                var filename = SalvarFotoAsync(foto.Item1, foto.Item2, true).GetAwaiter().GetResult();
                if (filename != null) {
                    checklistImages.Add(new ChecklistImagens
                    {
                        RetiradaDevolucao = true,
                        Url = filename,
                    });
                }
            }

            objeto.ChecklistImagens = checklistImages;

            checkListRepository.Inserir(objeto);
            var user = usuarioRepository.Listar(x => x.Id == loggedUser.UserId).First();
            await veiculoRepository.AtualizaUso(objeto.VeiculosId, user);
        }

        public async Task Devolver(Checklist objeto, List<Tuple<string, string>> fotos)
        {
            var checklist = checkListRepository.Buscar(objeto.WorkspaceId, objeto.VeiculosId, loggedUser.UserId);
            if (checklist == null) throw new BussinessException("Você não pode devolver este veiculo.");

            var checklistImages = new List<ChecklistImagens>();
            foreach (var foto in fotos)
            {
                var filename = SalvarFotoAsync(foto.Item1, foto.Item2, false).GetAwaiter().GetResult();
                if (filename != null)
                {
                    checklistImages.Add(new ChecklistImagens
                    {
                        RetiradaDevolucao = false,
                        Url = filename,
                    });
                }
            }

            checklist.ChecklistImagens = checklistImages;
            checklist.DataDevolucao = objeto.DataDevolucao;
            checklist.ObsDevolucao = objeto.ObsDevolucao;
            checklist.OdometroDevolucao = objeto.OdometroDevolucao;
            checklist.DataDevolucao = objeto.DataDevolucao;
            checklist.Avaria = objeto.Avaria;
            checklist.OsbAvaria = objeto.OsbAvaria;

            checkListRepository.Atualizar(checklist);
            await veiculoRepository.AtualizaOdometro(checklist.VeiculosId, checklist.OdometroDevolucao);
            await veiculoRepository.AtualizaUso(checklist.VeiculosId, null);
        }

        private async Task<string> SalvarFotoAsync(string base64, string extensao, bool retirada)
        {
            string NomeFoto = string.Empty;
            if (!string.IsNullOrEmpty(base64))
            {
                try
                {
                    var bytes = Convert.FromBase64String(base64);
                    NomeFoto = await bucketService.UploadAsync(new MemoryStream(bytes), extensao, retirada ? "withdraw" : "return") ?? throw new BussinessException("não foi possivel salvar a imagem");
                }
                catch (Exception)
                {
                    NomeFoto = string.Empty;
                }
            }

            return NomeFoto;
        }
    }
}