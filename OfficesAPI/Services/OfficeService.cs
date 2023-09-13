using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficesAPI.Contracts.Requests;
using OfficesAPI.Contracts.Responses;
using OfficesAPI.Models;

namespace OfficesAPI.Services
{
    public interface IOfficeService
    {
        public Task<Office?> GetById(int id);
        public Task<List<Office>> GetAll();
        public Task<OfficeResult> Delete(int id);
        public Task<OfficeResult> Create(CreateOfficeRequest office);
        public Task<OfficeResult> Update(UpdateOfficeRequest office);
        public Task<OfficeResult> ChangeStatus(int id, string status);
    }

    public class OfficeService : IOfficeService
    {
        private readonly OfficesDbContext _db;

        public OfficeService(OfficesDbContext db)
        {
            _db = db;
        }

        public async Task<Office?> GetById(int id)
        {
            var office = await _db.Offices.FindAsync(id);
            if (office == null) return null;
            return office;
        }

        public async Task<List<Office>> GetAll()
        {
            var offices = await _db.Offices.ToListAsync();
            return offices;
        }

        public async Task<OfficeResult> Delete(int id)
        {
            var toDelete = await _db.Offices.FindAsync(id);
            if (toDelete == null)
            {
                return new OfficeResult(false, $"Office with id {id} not found.");
            }
            _db.Offices.Remove(toDelete);
            await _db.SaveChangesAsync();
            return new OfficeResult(true, "Office deleted.");
        }

        public async Task<OfficeResult> Create(CreateOfficeRequest office)
        {
            Address address = new Address()
            {
                City = office.City,
                Street = office.Street,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber
            };
            if (await _db.Offices.FirstOrDefaultAsync(x => x.Address == address.ToString()) != null)
            {
                return new OfficeResult(false, "Office with this address already exists.");
            }
            Office newOffice = new Office()
            {
                Address = address.ToString(),
                PhotoUrl = office.PhotoUrl,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status
            };
            await _db.Offices.AddAsync(newOffice);
            await _db.SaveChangesAsync();
            return new OfficeResult(true, "Office created.");
        }

        public async Task<OfficeResult> Update(UpdateOfficeRequest newOffice)
        {
            var office = await _db.Offices.FindAsync(newOffice.Id);
            if (office == null)
            {
                return new OfficeResult(false, $"Office with id {newOffice.Id} not found.");
            }
            Address address = new ()
            {
                City = newOffice.City,
                Street = newOffice.Street,
                HouseNumber = newOffice.HouseNumber,
                OfficeNumber = newOffice.OfficeNumber
            };
            office.Address = address.ToString();
            office.PhotoUrl = newOffice.PhotoUrl;
            office.RegistryPhoneNumber = newOffice.RegistryPhoneNumber;
            office.Status = newOffice.Status;
            await _db.SaveChangesAsync();
            return new OfficeResult(true, "Office updated.");
        }

        public async Task<OfficeResult> ChangeStatus(int id, string status)
        {
            var office = await _db.Offices.FindAsync(id);
            if (office == null)
            {
                return new OfficeResult(false, $"Office with id {id} not found.");
            }
            if (Enum.TryParse(status, true, out Status statusValue))
            {
                office.Status = statusValue;
            }
            else
            {
                return new OfficeResult(false, "Please, specify 'Active' or 'Inactive' status.");
            }
            await _db.SaveChangesAsync();
            return new OfficeResult(true, "Status updated.");
        }
    }
}
