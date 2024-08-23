namespace Fleet.Models
{
    public class ChecklistOpcao: DbEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Opcao { get; set; }
        public int ChecklistId { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
