using Fleet.Controllers.Model.Response.ListaItem;
using Fleet.Enums;
using Fleet.Models;

namespace Fleet.Controllers.Model.Response.Lista
{
    public class BuscarListaResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Padrao { get; set; }
        public TipoListasEnum Tipo { get; set; }

        public List<BuscarListaItemResponse> Items { get; set; } = new List<BuscarListaItemResponse> { };
    }
}
