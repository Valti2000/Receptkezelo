using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public class Allergen
    {

        public int Id { get; set; }
        public string Nev { get; set; } = null!;

        public virtual ICollection<AlapanyagAllergen> AlapanyagAllergens { get; set; } = new HashSet<AlapanyagAllergen>();
        public bool Deleted { get; set; }
    }
}
