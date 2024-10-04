using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkBackendApi.Models;

namespace WorkBackendApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly tuntidbContext db = new tuntidbContext();

        [HttpGet]
        public ActionResult GetEmployees()
        {
            var emp = db.Employees.Where(e => e.Active == true);
            return Ok(emp);
        }

    }
}
