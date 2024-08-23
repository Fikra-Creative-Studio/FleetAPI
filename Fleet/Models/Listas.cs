using Fleet.Enums;

namespace Fleet.Models
{
    public class Listas : DbEntity
    {
        public string Nome { get; set; } = string.Empty;
        public TipoListasEnum Tipo { get; set; }
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
