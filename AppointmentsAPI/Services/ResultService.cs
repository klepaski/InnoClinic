using AppointmentsAPI.Context;
using AppointmentsAPI.Contracts.Requests;
using AppointmentsAPI.Contracts.Responses;
using AppointmentsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsAPI.Services
{
    public interface IResultService
    {
        public Task<GetResultResponse?> Get(int appointmentId);
        public Task Create(CreateResultRequest req);
        public Task Update (UpdateResultRequest req);
        public Task<byte[]> DownloadPdf(int id);
    }

    public class ResultService : IResultService
    {
        private readonly AppointmentDbContext _db;

        public ResultService(AppointmentDbContext db)
        {
            _db = db;
        }
        public async Task Create(CreateResultRequest req)
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

        public async Task<GetResultResponse?> Get(int appointmentId)
        {
            var result = await _db.Results.Include(r => r.Appointment).FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
            if (result == null) return null;
            return result.ToResponse();
        }

        public async Task Update(UpdateResultRequest req)
        {
            var result = await _db.Results.FindAsync(req.Id);
            if (result == null) return;
            result.Complaints = req.Complaints;
            result.Conclusion = req.Conclusion;
            result.Recommendations = req.Recommendations;
            await _db.SaveChangesAsync();
        }

        private async Task<string> GetHtml(int resultId)
        {
            var result = (await _db.Results.Include(r => r.Appointment).FirstOrDefaultAsync(r => r.Id == resultId)).ToResponse();
            if (result == null) return "";
            return $"<img src='logo.png' />" +
                $"<h1>Patient: {result.PatientName} ({result.PatientBirthday.ToString("dd.MM.yyyy")})</h1>" +
                $"<h3>Doctor: {result.DoctorName} ({result.DoctorSpecialization})</h3>" +
                $"<h3>Date: {result.DateTime.ToString("dd.MM.yyyy")}</h3>" +
                $"<h3>Service: {result.ServiceName}</h3>" +
                $"<h3>Complaints: {result.Complaints}</h3>" +
                $"<h3>Conclusion: {result.Conclusion}</h3>" +
                $"<h3>Recommendations: {result.Recommendations}</h3>";
        }

        public async Task<byte[]> DownloadPdf(int id)
        {
            string html = await GetHtml(id);
            var Renderer = new ChromePdfRenderer();
            using var PDF = Renderer.RenderHtmlAsPdf(html);
            //PDF.SaveAs("Result.pdf");
            var OutputBytes = PDF.BinaryData;
            return OutputBytes;
        }
    }
}
