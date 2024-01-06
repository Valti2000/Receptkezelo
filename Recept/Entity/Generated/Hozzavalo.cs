using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public class Hozzavalo
    {
        public int Id { get; set; }
        public string Nev { get; set; } = null!;
        public int AlapanyagId { get; set; }
        public double Mennyiseg { get; set; }
        public string? Mertekegyseg { get; set; }
        public int CsoportId { get; set; }
        public bool Deleted { get; set; }

        public virtual Csoport Csoport { get; set; } = null!;
        public virtual Alapanyag Alapanyag { get; set; } = null!;
        public ICollection<ReceptHozzavalo> ReceptHozzavalok { get; set; } = new HashSet<ReceptHozzavalo>();
    }
}
