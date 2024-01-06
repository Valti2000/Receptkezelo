using System;
using System.Collections.Generic;

namespace Recept.Entity.Generated
{
    public class AlapanyagAllergen
    {
        public int Id { get; set; }
        public bool Tartalmaz { get; set; }
        public int AlapanyagId { get; set; }
        public int AllergenId { get; set; }

        public virtual Alapanyag Alapanyag { get; set; } = null!;
        public virtual Allergen Allergen { get; set; } = null!;
    }
}
