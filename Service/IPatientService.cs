using PJATK_APBD_Cw8_s29769.DTO;

namespace PJATK_APBD_Cw8_s29769.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientGetDto>> GetPatientsAsync(string? search);
        Task<BedAssignmentDto> AssignBedAsync(string patientPesel, BedAssignmentPostDto dto);
    }
}