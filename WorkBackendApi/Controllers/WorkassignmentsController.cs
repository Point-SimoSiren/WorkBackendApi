using MauiTimeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkBackendApi.Models;

namespace WorkBackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkassignmentsController : ControllerBase
    {
        private readonly tuntidbContext db =  new tuntidbContext();


        [HttpGet]
        public ActionResult GetWa()
        {
            var wa = db.WorkAssignments.Where(e => e.Active == true);
            return Ok(wa);
        }

        // START
        [HttpPost("start")]
        public bool Start([FromBody] Operation op) {

            // Haetaan tietokannassa oleva WA rivi
            WorkAssignment? wa = db.WorkAssignments.Find(op.WorkAssignmentID);

            // Jos työtehtävä on jo käynnissä sitä ei voi aloittaa ja palautetaan false mobiilisovellukselle
            if ( wa.InProgress == true)
            {
                return false;
            }

            // Jos kaikki ok, muokataan WA riviä ja tallennetaan tietokantaan
            wa.InProgress = true;
            wa.WorkStartedAt = DateTime.Now.AddHours(1);
            db.SaveChanges();

            // Luodaan uusi TimeSheet rivi tietokantaan
            Timesheet newEntry = new Timesheet()
            {
                IdWorkAssignment = op.WorkAssignmentID,
                StartTime = DateTime.Now.AddHours(1),
                Active = true,
                IdEmployee = op.EmployeeID,
                IdCustomer = wa.IdCustomer,
                CreatedAt = DateTime.Now.AddHours(1),
                Comments = op.Comment
            };

            db.Timesheets.Add(newEntry);

            db.SaveChanges();

            return (true);

        }

    }
}
