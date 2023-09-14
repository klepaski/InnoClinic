using Microsoft.EntityFrameworkCore;

namespace OfficesAPI.Models
{
    public class OfficesDbContext : DbContext
    {
        public OfficesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Receptionist> Receptionists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Office>()
                .HasKey(x => x.Id);

            builder.Entity<Receptionist>()
                .HasKey(x => x.Id);

            builder.Entity<Receptionist>()
                .HasOne(x => x.Office)
                .WithOne(x => x.Receptionist);

            builder.Entity<Office>().HasData(
               new Office[]
               {
                    new Office {
                        Id = 1,
                        Address = "Brest, Kolesnika street, 4 | Office: 54",
                        PhotoUrl = "https://avatars.mds.yandex.net/get-altay/4464784/2a0000017840924485aa42a8ef3d614ace76/L_height",
                        RegistryPhoneNumber = "+375333186223",
                        Status = Status.Inactive
                    },
                    new Office {
                        Id = 2,
                        Address = "Minsk, Sverdlova street, 13a | Office: 226",
                        PhotoUrl = "https://avatars.mds.yandex.net/get-altay/933207/2a00000161bf61714998e315745eea065577/orig",
                        RegistryPhoneNumber = "+375333733839",
                        Status = Status.Active
                    }
               });
        }
    }
}
