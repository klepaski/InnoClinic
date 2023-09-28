using Microsoft.EntityFrameworkCore;
using JuliaChistyakovaPackage;

namespace AuthAPI.Models
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = "user@gmail.com",
                        PasswordHash = "k��s�4��k�N�Z?WG���/\u001dI�\u001eRݷ�[K", //1
                        Role = Role.Patient
                    },
                    new User
                    {
                        Id = 2,
                        Email = "doctor@gmail.com",
                        PasswordHash = "k��s�4��k�N�Z?WG���/\u001dI�\u001eRݷ�[K",//1
                        Role = Role.Doctor
                    }
                );
        }
    }
}
