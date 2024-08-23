using System.ComponentModel.DataAnnotations;

namespace Fleet.Models
{
    public class DbEntity
    {
        public DbEntity()
        {
            Ativo = true;
        }
        public int Id { get; set; }
        public bool Ativo { get; set; }
    }
}