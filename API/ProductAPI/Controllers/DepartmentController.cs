using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services.Interfaces;



namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var departments = _departmentService.GetAll();

             if (departments == null || !departments.Any())
                return NotFound("Nenhum departamento encontrado.");

            return Ok(departments);

        }

    }
}
