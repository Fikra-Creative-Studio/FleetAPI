namespace Fleet.Models
{
    public class VisitaOpcao : DbEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Opcao { get; set; }
        public int VisitasId { get; set; }
        public virtual Visitas Visitas { get; set; }
    }
}
