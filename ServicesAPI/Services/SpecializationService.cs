using ServicesAPI.Context;
using ServicesAPI.Models;
using ServicesAPI.Contracts.Requests;
using ServicesAPI.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI.Services
{
    public interface ISpecializationService
    {
        public Task<List<Specialization>> GetAll();
        public Task<GetSpecializationResponse?> GetById(int id);
        public Task<GeneralResponse> Create(CreateSpecializationRequest req);
        public Task<GeneralResponse> Update(UpdateSpecializationRequest req);
        public Task<GeneralResponse> ChangeStatus(int specializationId, bool status);
    }

    public class SpecializationService : ISpecializationService
    {
        private readonly ServicesDbContext _db;

        public SpecializationService(ServicesDbContext db)
        {
            _db = db;
        }

        public async Task<List<Specialization>> GetAll()
        {
            return await _db.Specializations.ToListAsync();
        }

        public async Task<GetSpecializationResponse?> GetById(int id)
        {
            var specialization = await _db.Specializations
                .Include(x => x.Services)
                    .ThenInclude(s => s.ServiceCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (specialization is null) return null;
            return specialization.ToResponse();
        }

        public async Task<GeneralResponse> Create(CreateSpecializationRequest req)
        {
            var newSpecialization = new Specialization
            {
                SpecializationName = req.SpecializationName,
                IsActive = req.IsActive
            };
            await _db.Specializations.AddAsync(newSpecialization);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Specialization created.");
        }

        public async Task<GeneralResponse> Update(UpdateSpecializationRequest? req)
        {
            var specialization = await _db.Specializations.FindAsync(req.Id);
            if (specialization == null) return new GeneralResponse(false, "Specialization not found.");
            specialization.SpecializationName = req.SpecializationName;
            specialization.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Specialization updated.");
        }

        public async Task<GeneralResponse> ChangeStatus(int specializationId, bool status)
        {
            var specialization = await _db.Specializations.FindAsync(specializationId);
            if (specialization == null) return new GeneralResponse(false, "Specialization not found.");
            specialization.IsActive = status;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Status updated.");
        }
    }
}
