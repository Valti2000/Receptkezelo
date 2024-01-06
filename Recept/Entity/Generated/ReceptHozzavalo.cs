using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recept.Entity.Generated
{
    public class ReceptHozzavalo
    {
        public int Id { get; set; }
        public int HozzavaloId { get; set; }
        public int ReceptId { get; set; }

        [ForeignKey("HozzavaloId")]
        public Hozzavalo Hozzavalo { get; set; } = null!;

        [ForeignKey("ReceptId")]
        public Receptek Recept { get; set; } = null!;

    }
}
