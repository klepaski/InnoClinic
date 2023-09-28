using ServicesAPI.Context;
using ServicesAPI.Models;
using ServicesAPI.Contracts.Requests;
using ServicesAPI.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI.Services
{
    public interface IServiceService
    {
        public Task<List<GetServiceResponse>> GetAll();
        public Task<GetServiceResponse?> GetById(int id);
        public Task<GeneralResponse> Create(CreateServiceRequest req);
        public Task<GeneralResponse> Update(UpdateServiceRequest req);
        public Task<GeneralResponse> ChangeStatus(int serviceId, bool status);
        public Task<List<ServiceCategory>> GetAllCategories();
    }

    public class ServiceService : IServiceService
    {
        private readonly ServicesDbContext _db;

        public ServiceService(ServicesDbContext db)
        {
            _db = db;
        }

        public async Task<List<GetServiceResponse>> GetAll()
        {
            //category: consultations, diagnostics, analyzes
            //group by Specialization
            return await _db.Services
                .Include(s => s.Specialization)
                .Include(x => x.ServiceCategory)
                .Where(x => x.IsActive == true && x.Specialization.IsActive == true)
                .Select(d => d.ToResponse())
                .ToListAsync();
        }

        public async Task<GetServiceResponse?> GetById(int id)
        {
            var service = await _db.Services
                .Include(x => x.ServiceCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (service is null) return null;
            return service.ToResponse();
        }

        public async Task<GeneralResponse> Create(CreateServiceRequest req)
        {
            var newService = new Service
            {
                ServiceName = req.ServiceName,
                Price = req.Price,
                IsActive = req.IsActive,
                CategoryId = req.CategoryId,
                SpecializationId = req.SpecializationId
            };
            await _db.Services.AddAsync(newService);
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Service created.");
        }

        public async Task<GeneralResponse> Update(UpdateServiceRequest req)
        {
            var service = await _db.Services.FindAsync(req.Id);
            if (service == null) return new GeneralResponse(false, "Service not found.");
            service.ServiceName = req.ServiceName;
            service.Price = req.Price;
            service.CategoryId = req.CategoryId;
            service.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Service updated.");
        }

        public async Task<GeneralResponse> ChangeStatus(int serviceId, bool status)
        {
            var service = await _db.Services.FindAsync(serviceId);
            if (service == null) return new GeneralResponse(false, "Service not found.");
            service.IsActive = status;
            await _db.SaveChangesAsync();
            return new GeneralResponse(true, "Status updated.");
        }

        public async Task<List<ServiceCategory>> GetAllCategories()
        {
            return await _db.ServiceCategories.ToListAsync();
        }
    }
}
