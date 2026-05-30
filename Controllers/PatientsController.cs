using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s29769.DTO;
using PJATK_APBD_Cw8_s29769.Exceptions;
using PJATK_APBD_Cw8_s29769.Services;

namespace PJATK_APBD_Cw8_s29769.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? search)
        {
            var patients = await _patientService.GetPatientsAsync(search);
            return Ok(patients);
        }

        [HttpPost("{id}/bedassignments")]
        public async Task<IActionResult> AssignBed(string id, [FromBody] BedAssignmentPostDto dto)
        {
            try
            {
                var newAssignment = await _patientService.AssignBedAsync(id, dto);
                
                return Created($"/api/patients/{id}/bedassignments/{newAssignment.Id}", newAssignment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}