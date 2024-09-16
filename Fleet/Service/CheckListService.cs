using Fleet.Interfaces.Repository;
using Fleet.Interfaces.Service;
using Fleet.Migrations;
using Fleet.Models;
using Fleet.Repository;

namespace Fleet.Service
{
    public class CheckListService(IBucketService bucketService, ICheckListRepository checkListRepository, ILoggedUser loggedUser, IVeiculoRepository veiculoRepository) : ICheckListService
    {
        public void Retirar(Checklist objeto, Dictionary<string, string> fotos)
        {
            if (checkListRepository.Existe(x => x.VeiculosId == objeto.VeiculosId && x.DataDevolucao == null)) throw new BussinessException("Este veiculo já esta em uso");

            var checklistImages = new List<ChecklistImagens>();
            foreach (var foto in fotos) { 
                var filename = SalvarFotoAsync(foto.Key, foto.Value, true).GetAwaiter().GetResult();
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
        }

        public void Devolver(Checklist objeto, Dictionary<string, string> fotos)
        {
            var checklist = checkListRepository.Buscar(objeto.WorkspaceId, objeto.VeiculosId, loggedUser.UserId);
            if (checklist == null) throw new BussinessException("Você não pode devolver este veiculo.");

            var checklistImages = new List<ChecklistImagens>();
            foreach (var foto in fotos)
            {
                var filename = SalvarFotoAsync(foto.Key, foto.Value, false).GetAwaiter().GetResult();
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
            veiculoRepository.AtualizaOdometro(checklist.VeiculosId, checklist.OdometroDevolucao);
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