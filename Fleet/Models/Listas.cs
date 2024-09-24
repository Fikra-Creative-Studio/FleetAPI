using Fleet.Enums;

namespace Fleet.Models
{
    public class Listas : DBWorkspaceEntity
    {
        public string Nome { get; set; } = string.Empty;
        public TipoListasEnum Tipo { get; set; }
        public bool Padrao { get; set; } = false;

        public virtual List<ListasItens> ListasItens { get; set; } = [];
    }
}
