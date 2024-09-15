namespace Fleet.Models
{
    public class DBWorkspaceEntity: DbEntity
    {
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
