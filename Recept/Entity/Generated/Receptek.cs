using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recept.Entity.Generated
{
    public  class Receptek
    {
        public int Id { get; set; }
        public string Cim { get; set; } = null!;
        public string? Leiras { get; set; }
        public bool Deleted { get; set; }

        public ICollection<ReceptHozzavalo> ReceptHozzavalok { get; set; } = new HashSet<ReceptHozzavalo>();

        public int ElokeszitesiIdo { get; set; } // percben

        [NotMapped] // Ez a tulajdonság nem kerül az adatbázisba
        public int ElkeszitesiIdo => ElokeszitesiIdo + FozesiIdo;

        public int FozesiIdo { get; set; }

        public ICollection<KedvencRecept> Kedvelok { get; set; } = new List<KedvencRecept>();
    }
}

