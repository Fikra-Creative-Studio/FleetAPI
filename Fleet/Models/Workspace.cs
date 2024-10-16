﻿namespace Fleet.Models
{
    public class Workspace : DbEntity
    {
        public string Cnpj { get; set; } = string.Empty;
        public string Fantasia { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;

        public virtual ICollection<UsuarioWorkspace> UsuarioWorkspaces { get; set; } = [];
    }
}   