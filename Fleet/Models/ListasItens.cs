namespace Fleet.Models
{
    public class ListasItens : DbEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int ListasId { get; set; }
        public virtual Listas Listas { get; set; }  
    }
}
