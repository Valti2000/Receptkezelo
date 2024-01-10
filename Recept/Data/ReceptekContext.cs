using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Recept.Entity.Generated;



namespace Recept.Data
{
    public partial class ReceptekContext : IdentityDbContext<ApplicationUser>


    {

        public ReceptekContext(DbContextOptions<ReceptekContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alapanyag> Alapanyagok { get; set; } = null!;
        public virtual DbSet<AlapanyagAllergen> AlapanyagAllergen { get; set; } = null!;
        public virtual DbSet<Allergen> Allergenek { get; set; } = null!;
        public virtual DbSet<Csoport> Csoport { get; set; } = null!;
        public virtual DbSet<Hozzavalo> Hozzavalok { get; set; } = null!;
        public virtual DbSet<Kategorium> Kategoria { get; set; } = null!;
        public virtual DbSet<Receptek> Receptek { get; set; } = null!;
        public virtual DbSet<ReceptHozzavalo> ReceptHozzavalo { get; set; } = null!;

        public virtual DbSet<KedvencRecept> KedvencRecept { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-UCKE7VR\\MSSQLSERVER01;Database=Receptek;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Alapanyag>(entity =>
            {
                entity.ToTable("Alapanyag");

                entity.HasIndex(e => e.KategoriaId, "IX_Alapanyag_KategoriaId");

                entity.HasOne(d => d.Kategoria)
                    .WithMany(p => p.Alapanyags)
                    .HasForeignKey(d => d.KategoriaId);
            });

            modelBuilder.Entity<AlapanyagAllergen>(entity =>
            {
                entity.ToTable("AlapanyagAllergen");

                entity.HasIndex(e => e.AlapanyagId, "IX_AlapanyagAllergen_AlapanyagId");

                entity.HasIndex(e => e.AllergenId, "IX_AlapanyagAllergen_AllergenId");

                entity.HasOne(d => d.Alapanyag)
                    .WithMany(p => p.AlapanyagAllergens)
                    .HasForeignKey(d => d.AlapanyagId);

                entity.HasOne(d => d.Allergen)
                    .WithMany(p => p.AlapanyagAllergens)
                    .HasForeignKey(d => d.AllergenId);
            });

            modelBuilder.Entity<ReceptHozzavalo>(entity =>
            {
                entity.HasKey(rh => rh.Id);

                entity.Property(rh => rh.HozzavaloId).IsRequired();
                entity.Property(rh => rh.ReceptId).IsRequired();

                entity.HasOne(rh => rh.Hozzavalo)
                    .WithMany(h => h.ReceptHozzavalok)
                    .HasForeignKey(rh => rh.HozzavaloId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(rh => rh.Recept)
                    .WithMany(r => r.ReceptHozzavalok)
                    .HasForeignKey(rh => rh.ReceptId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<Allergen>(entity =>
            {
                entity.ToTable("Allergen");
            });

            modelBuilder.Entity<Csoport>(entity =>
            {
                entity.ToTable("Csoport");
            });

            modelBuilder.Entity<Hozzavalo>(entity =>
            {
                entity.ToTable("Hozzavalo");

                entity.HasIndex(e => e.AlapanyagId, "IX_Hozzavalo_AlapanyagId");

                entity.HasIndex(e => e.CsoportId, "IX_Hozzavalo_CsoportId");

                entity.HasOne(d => d.Alapanyag)
                    .WithMany(p => p.Hozzavalos)
                    .HasForeignKey(d => d.AlapanyagId);

                entity.HasOne(d => d.Csoport)
                    .WithMany(p => p.Hozzavalok)
                    .HasForeignKey(d => d.CsoportId);

                entity.Ignore(d => d.ReceptHozzavalok);
            });

            modelBuilder.Entity<Receptek>(entity =>
            {
                entity.ToTable("Receptek");

                entity.HasMany(r => r.ReceptHozzavalok)
                    .WithOne(rh => rh.Recept)
                    .HasForeignKey(rh => rh.ReceptId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);
            });


                modelBuilder.Entity<Alapanyag>().HasQueryFilter(p => !p.Deleted);
                modelBuilder.Entity<Allergen>().HasQueryFilter(p => !p.Deleted);
                modelBuilder.Entity<Csoport>().HasQueryFilter(p => !p.Deleted);
                modelBuilder.Entity<Hozzavalo>().HasQueryFilter(p => !p.Deleted);
                modelBuilder.Entity<Kategorium>().HasQueryFilter(p => !p.Deleted);
                modelBuilder.Entity<Receptek>().HasQueryFilter(p => !p.Deleted);

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });

            modelBuilder.Entity<KedvencRecept>(entity =>
            {
                entity.HasKey(kr => new { kr.UserId, kr.ReceptId });

                entity.HasOne(kr => kr.User)
                    .WithMany(u => u.KedvencReceptek)
                    .HasForeignKey(kr => kr.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(kr => kr.Recept)
                    .WithMany(r => r.Kedvelok)
                    .HasForeignKey(kr => kr.ReceptId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
