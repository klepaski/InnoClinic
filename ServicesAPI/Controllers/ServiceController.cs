using Microsoft.AspNetCore.Authorization;
using ServicesAPI.Services;
using ServicesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Contracts.Requests;

namespace ServicesAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceService.GetAll();
            return Ok(services);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetById(id);
            return Ok(service);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequest newService)
        {
            var result = await _serviceService.Create(newService);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceRequest newService)
        {
            var result = await _serviceService.Update(newService);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPut("{serviceId}/{isActive}")]
        public async Task<IActionResult> ChangeStatus(int serviceId, bool isActive)
        {
            var result = await _serviceService.ChangeStatus(serviceId, isActive);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _serviceService.GetAllCategories();
            return Ok(categories);
        }
    }
}