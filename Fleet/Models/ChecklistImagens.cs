namespace Fleet.Models
{
    public class ChecklistImagens : DbEntity
    {
        public string Url { get; set; } = string.Empty;
        public bool RetiradaDevolucao { get; set; } = false;
        public int ChecklistId { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
