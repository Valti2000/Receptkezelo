using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public class Csoport
    {
        public int Id { get; set; }
        public string Nev { get; set; } = null!;

        public ICollection<Hozzavalo> Hozzavalok { get; set; } = new HashSet<Hozzavalo>();
        public bool Deleted { get; set; }
    }
}
