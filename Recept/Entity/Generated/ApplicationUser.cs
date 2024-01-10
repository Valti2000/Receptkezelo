using Microsoft.AspNetCore.Identity;
using Recept.Entity.Generated;
public class ApplicationUser : IdentityUser
{
    public string Nev { get; set; } = null!;
    public string Varos { get; set; } = null!;
    public string Orszag { get; set; } = null!;
    public string ProfilePictureUrl { get; set; } = null!;

    public ICollection<KedvencRecept> KedvencReceptek { get; set; } = new List<KedvencRecept>();
}
