using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Context;
using ProfilesAPI.Models;
using SharedModels;

namespace ProfilesAPI.Consumers
{
    public class SpecializationCreatedConsumer : IConsumer<SpecializationCreated>
    {
        private readonly ProfilesDbContext _db;

        public SpecializationCreatedConsumer(ProfilesDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<SpecializationCreated> context)
        {
            var data = context.Message;
            var nameExist = await _db.DoctorSpecializations.FirstOrDefaultAsync(x => x.SpecializationName == data.SpecializationName);
            if (nameExist != null) return;
            var newSpecialization = new DoctorSpecialization()
            {
                //Id = data.Id,
                SpecializationName = data.SpecializationName,
                IsActive = data.IsActive
            };

            await _db.DoctorSpecializations.AddAsync(newSpecialization);
            await _db.SaveChangesAsync();
        }
    }
}
