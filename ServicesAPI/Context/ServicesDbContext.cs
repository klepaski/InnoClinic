using Microsoft.EntityFrameworkCore;
using ServicesAPI.Models;

namespace ServicesAPI.Context
{
    public class ServicesDbContext : DbContext
    {
        public ServicesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Service>()
                .HasOne(x => x.ServiceCategory)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.CategoryId);

            builder.Entity<Service>()
                .HasOne(x => x.Specialization)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.SpecializationId);


            builder.Entity<Specialization>().HasData(
                new Specialization { 
                    Id = 1,
                    SpecializationName = "Cardio",
                    IsActive = true
                },
                new Specialization
                {
                    Id = 2,
                    SpecializationName = "Oncology",
                    IsActive = false
                },
                new Specialization
                {
                    Id = 3,
                    SpecializationName = "Ophtalmology",
                    IsActive = true
                });

            builder.Entity<ServiceCategory>().HasData(
                new ServiceCategory
                {
                    Id = 1,
                    CategoryName = "Consultations",
                    TimeSlotSize = 10
                },
                new ServiceCategory
                {
                    Id = 2,
                    CategoryName = "Diagnostics",
                    TimeSlotSize = 5
                },
                new ServiceCategory
                {
                    Id = 3,
                    CategoryName = "Analyzes",
                    TimeSlotSize = 20
                });

            builder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    CategoryId = 1,
                    ServiceName = "Pressure measurement",
                    Price = 10,
                    SpecializationId = 1,
                    IsActive = true
                });
        }
    }
}