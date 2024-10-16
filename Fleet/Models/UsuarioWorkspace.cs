﻿using System.ComponentModel.DataAnnotations.Schema;
using Fleet.Enums;

namespace Fleet.Models
{
    [Table("usuarioworkspace")]
    public class UsuarioWorkspace : DbEntity
    {
        public PapelEnum Papel { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
