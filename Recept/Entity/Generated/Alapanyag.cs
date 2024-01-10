using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public class Alapanyag
    {
        public int Id { get; set; }
        public string Nev { get; set; } = null!;
        public int KategoriaId { get; set; }
        public bool Deleted { get; set; }

        public virtual Kategorium Kategoria { get; set; } = null!;
        public virtual ICollection<AlapanyagAllergen> AlapanyagAllergens { get; set; } = new HashSet<AlapanyagAllergen>();
        public virtual ICollection<Hozzavalo> Hozzavalos { get; set; } = new HashSet<Hozzavalo>();
    }
}
