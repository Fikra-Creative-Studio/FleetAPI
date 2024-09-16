namespace Fleet.Models
{
    public class AbastecimentoImagens : DbEntity
    {
        public string Url { get; set; } = string.Empty;
        public int AbastecimentoId { get; set; }
        public virtual Abastecimento Abastecimento { get; set; }
    }
}
