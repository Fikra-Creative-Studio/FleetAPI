namespace Fleet.Models
{
    public class VisitaOpcao : DbEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Opcao { get; set; } = false;
        public int VisitaId { get; set; }
        public virtual Visitas Visitas { get; set; }
    }
}
