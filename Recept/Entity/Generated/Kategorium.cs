using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public  class Kategorium
    {

        public int Id { get; set; }
        public string Nev { get; set; } = null!;

        public virtual ICollection<Alapanyag> Alapanyags { get; set; } = new HashSet<Alapanyag>();
        public bool Deleted { get; set; }
    }
}
