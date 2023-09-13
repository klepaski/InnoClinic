using Microsoft.EntityFrameworkCore;

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
                        PasswordHash = "by[.�#[;M(�i��6g���W��ـ�e37", //string123
                        RefreshToken = "pXX3ZevB7sJMmgj4EkNH0+0YNYxZgUtNrZWkyhvQUOY=",
                        RefreshTokenExpiryTime = DateTime.Parse("2023-09-20 13:40:28.7256178"),
                        Role = Role.Patient
                    },
                    new User
                    {
                        Id = 2,
                        Email = "doctor@gmail.com",
                        PasswordHash = "by[.�#[;M(�i��6g���W��ـ�e37",//string123
                        RefreshToken = "pXX3ZevB7sJMmgj4EkNH0+0YNYxZgUtNrZWkyhvQUOY=",
                        RefreshTokenExpiryTime = DateTime.Parse("2023-09-20 13:40:28.7256178"),
                        Role = Role.Doctor
                    }
                );
        }
    }
}
