namespace Fleet.Models
{
    public class Checklist :DbEntity
    {
        public DateTime DataRetirada{ get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string OdometroRetirada { get; set; } = string.Empty;
        public string OdometroDevolucao { get; set; } = string.Empty;   
        public string ObsRetirada { get; set; } = string.Empty;
        public string ObsDevolucao { get; set; } = string.Empty;
        public bool Avaria { get; set; }
        public string OsbAvaria { get; set; } = string.Empty;
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
        public int VeiculosId { get; set; }
        public virtual Veiculos Veiculos { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual List<ChecklistOpcao> ChecklistOpcaos { get; set; } = new List<ChecklistOpcao>();
        public virtual List<ChecklistImagens> ChecklistImagens { get; set; } = new List<ChecklistImagens>();

    }
}
