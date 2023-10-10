using AppointmentsAPI.Context;
using AppointmentsAPI.Contracts.Requests;
using AppointmentsAPI.Contracts.Responses;
using AppointmentsAPI.Models;
using JuliaChistyakovaPackage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace AppointmentsAPI.Services
{
    public interface IAppointmentService
    {
        public Task<List<Appointment>> GetAll();
        public Task<List<Appointment>> GetByPatient(int id);
        public Task<List<Appointment>> GetByDoctor(int id);
        public Task<GetResultResponse?> GetResult(int id);
        public Task Create(CreateAppointmentRequest req);
        public Task CreateResult(CreateResultRequest req);
        public Task Approve(int id);
        public Task Delete(int id);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AppointmentService(AppointmentDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Create(CreateAppointmentRequest req)
        {
            string doctorName = "";
            string patientName = "";
            string patientPhoneNumber = "";
            DateTime patientBirthday = DateTime.Now;
            string serviceName = "";
            float servicePrice = 0;
            string specializationName = "";

            using (var client = _httpClientFactory.CreateClient())
            {
                string doctorUrl = $"{Ports.ProfilesAPI}/Doctor/GetById/{req.DoctorId}";
                HttpResponseMessage doctorRes = await client.GetAsync(doctorUrl);
                if (doctorRes.IsSuccessStatusCode)
                {
                    string responseBody = await doctorRes.Content.ReadAsStringAsync();
                    dynamic? doctor = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    doctorName = $"{doctor.firstName} {doctor.lastName} {doctor.middleName}";
                    specializationName = doctor.specialization;
                }

                string patientUrl = $"{Ports.ProfilesAPI}/Patient/GetById/{req.PatientId}";
                HttpResponseMessage patientRes = await client.GetAsync(patientUrl);
                if (patientRes.IsSuccessStatusCode)
                {
                    string responseBody = await patientRes.Content.ReadAsStringAsync();
                    dynamic? patient = JsonConvert.DeserializeObject(responseBody);
                    patientName = $"{patient.firstName} {patient.lastName} {patient.middleName}";
                    patientPhoneNumber = patient.phoneNumber;
                    patientBirthday = patient.dateOfBirth;
                }

                string serviceUrl = $"{Ports.ServicesAPI}/Service/GetById/{req.ServiceId}";
                HttpResponseMessage serviceRes = await client.GetAsync(serviceUrl);
                if (serviceRes.IsSuccessStatusCode)
                {
                    string responseBody = await serviceRes.Content.ReadAsStringAsync();
                    dynamic? service = JsonConvert.DeserializeObject(responseBody);
                    serviceName = service.serviceName;
                    servicePrice = service.price;
                }
            }

            var newAppointment = new Appointment
            {
                PatientId = req.PatientId,
                DoctorId = req.DoctorId,
                ServiceId = req.ServiceId,
                SpecializationId = req.SpecializationId,
                DateTime = req.DateTime,
                IsApproved = false,
                DoctorName = doctorName,
                PatientName = patientName,
                PatientPhoneNumber = patientPhoneNumber,
                PatientBirthday = patientBirthday,
                ServiceName = serviceName,
                ServicePrice = servicePrice,
                DoctorSpecialization = specializationName
            };
            await _db.Appointments.AddAsync(newAppointment);
            await _db.SaveChangesAsync();
        }

        public async Task Approve(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null) return;
            appointment.IsApproved = true;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null) return;
            _db.Appointments.Remove(appointment);
            await _db.SaveChangesAsync();
        }

        public async Task CreateResult(CreateResultRequest req)
        {
            var newResult = new Result
            {
                AppointmentId = req.AppointmentId,
                Complaints = req.Complaints,
                Conclusion = req.Conclusion,
                Recommendations = req.Recommendations
            };
            await _db.Results.AddAsync(newResult);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<List<Appointment>> GetByPatient(int id)
        {
            return await _db.Appointments.Where(x => x.PatientId == id).ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctor(int id)
        {
            return await _db.Appointments.Where(x => x.DoctorId == id).ToListAsync();
        }

        public async Task<GetResultResponse?> GetResult(int id)
        {
            var result = await _db.Results.FirstOrDefaultAsync(r => r.AppointmentId == id);
            if (result == null) return null;
            return result.ToResponse();
        }
    }
}
