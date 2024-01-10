namespace Recept.Entity.Generated
{
    public class KedvencRecept
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int ReceptId { get; set; }
        public Receptek Recept { get; set; } = null!;
    }

}
