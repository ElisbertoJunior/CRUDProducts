using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services.Interfaces;



namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            var departments = _departmentService.GetAll();
            return Ok(departments);

        }

    }
}
