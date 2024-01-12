using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Recept.Pages.Read
{
    [Authorize(Roles = "Admin , ReceptIro")]
    public class HozzavalokModel : PageModel
    {
        private readonly IHozzavaloRepository _hozzavaloRepository;
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly ICsoportRepository _csoportRepository;
        private readonly ReceptekContext _dbContext;

        public HozzavalokModel(IHozzavaloRepository hozzavaloRepository, IAlapanyagRepository alapanyagRepository, ICsoportRepository csoportRepository, ReceptekContext dbContext)
        {
            _hozzavaloRepository = hozzavaloRepository;
            _alapanyagRepository = alapanyagRepository;
            _csoportRepository = csoportRepository;
            _dbContext = dbContext;
        }

        public List<Hozzavalo> Hozzavalok { get; set; } = new List<Hozzavalo>();

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var jwtToken = HttpContext.Request.Cookies["JWT"];

                // Ellenõrizd a JWT tokent és kezeld megfelelõen
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;


                var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
                
                Console.WriteLine($"A JWT token elfogadva a felhasználó számára: {userId}");

                Hozzavalok = await _hozzavaloRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Hiba a JWT token ellenõrzésekor: {ex.Message}");
                RedirectToPage("/Account/Login");
            }
        }

        public async Task OnPostAsync()
        {
            try
            {
                var jwtToken = HttpContext.Request.Cookies["JWT"];

 
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;


                var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
                
                Console.WriteLine($"A JWT token elfogadva a felhasználó számára: {userId}");

                if (IsDeleted)
                {
                    Hozzavalok = await _dbContext.Hozzavalok.IgnoreQueryFilters().ToListAsync();
                }
                else
                {
                    Hozzavalok = await _hozzavaloRepository.GetAllAsync();
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Hiba a JWT token ellenõrzésekor: {ex.Message}");
                RedirectToPage("/Account/Login");
            }
        }

        public async Task<string> GetAlapanyagbyId(int alapanyagId)
        {
            Alapanyag alapanyagnev = await _alapanyagRepository.GetByIdAsync(alapanyagId);
            return alapanyagnev.Nev;
        }

        public async Task<string> GetCsoportNevById(int csoportId)
        {
            Console.WriteLine("csoportId: " + csoportId);
            Csoport csoport = await _csoportRepository.GetByIdAsync(csoportId);
            return csoport.Nev;
        }
    }
}
