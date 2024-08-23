namespace Fleet.Models
{
    public class VisitaImagens : DbEntity
    {
        public string Url { get; set; } = string.Empty;
        public bool FotoAssinatura { get; set; }  = false;  // true para foto da assintatura ou false para foto da visita de outro lugar para filtrar no relatório
        public int VisitaId { get; set; }
        public virtual Visitas Visitas { get; set; }
    }
}
