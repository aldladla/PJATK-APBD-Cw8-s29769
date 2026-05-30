using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s29769.DTO;
using PJATK_APBD_Cw8_s29769.Exceptions;
using PJATK_APBD_Cw8_s29769.Models;

namespace PJATK_APBD_Cw8_s29769.Services
{
    public class PatientService : IPatientService
    {
        private readonly HospitalContext _context;

        public PatientService(HospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientGetDto>> GetPatientsAsync(string? search)
        {
            var query = _context.Patients
                .Include(p => p.Admissions).ThenInclude(a => a.Ward)
                .Include(p => p.BedAssignments).ThenInclude(ba => ba.Bed).ThenInclude(b => b.BedType)
                .Include(p => p.BedAssignments).ThenInclude(ba => ba.Bed).ThenInclude(b => b.Room).ThenInclude(r => r.Ward)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => 
                    EF.Functions.Like(p.FirstName, $"%{search}%") || 
                    EF.Functions.Like(p.LastName, $"%{search}%"));
            }

            return await query.Select(p => new PatientGetDto
            {
                Pesel = p.Pesel,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Sex = p.Sex ? "Male" : "Female",
                Admissions = p.Admissions.Select(a => new AdmissionDto
                {
                    Id = a.Id,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    Ward = new WardDto
                    {
                        Id = a.Ward.Id,
                        Name = a.Ward.Name,
                        Description = a.Ward.Description
                    }
                }).ToList(),
                BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentDto
                {
                    Id = ba.Id,
                    From = ba.From,
                    To = ba.To,
                    Bed = new BedDto
                    {
                        Id = ba.Bed.Id,
                        BedType = new BedTypeDto
                        {
                            Id = ba.Bed.BedType.Id,
                            Name = ba.Bed.BedType.Name,
                            Description = ba.Bed.BedType.Description
                        },
                        Room = new RoomDto
                        {
                            Id = ba.Bed.Room.Id,
                            HasTv = ba.Bed.Room.HasTv,
                            Ward = new WardDto
                            {
                                Id = ba.Bed.Room.Ward.Id,
                                Name = ba.Bed.Room.Ward.Name,
                                Description = ba.Bed.Room.Ward.Description
                            }
                        }
                    }
                }).ToList()
            }).ToListAsync();
        }

        public async Task<BedAssignmentDto> AssignBedAsync(string patientPesel, BedAssignmentPostDto dto)
        {
            var patientExists = await _context.Patients.AnyAsync(p => p.Pesel == patientPesel); 
            if (!patientExists)
            {
                throw new NotFoundException($"Nie znaleziono pacjenta o identyfikatorze: {patientPesel}");
            }

            var availableBed = await _context.Beds
                .Include(b => b.Room).ThenInclude(r => r.Ward)
                .Include(b => b.BedType)
                .Where(b => b.Room.Ward.Name == dto.Ward && b.BedType.Name == dto.BedType)
                .Where(b => !b.BedAssignments.Any(ba =>
                    (dto.To == null || ba.From < dto.To) &&
                    (ba.To == null || ba.To > dto.From)
                ))
                .FirstOrDefaultAsync();

            if (availableBed == null)
            {
                throw new NotFoundException($"Brak wolnych łóżek typu '{dto.BedType}' na oddziale '{dto.Ward}' we wskazanym terminie.");
            }

            var newAssignment = new BedAssignment
            {
                PatientPesel = patientPesel,
                BedId = availableBed.Id,
                From = dto.From,
                To = dto.To
            };

            _context.BedAssignments.Add(newAssignment);
            await _context.SaveChangesAsync();

            return new BedAssignmentDto
            {
                Id = newAssignment.Id,
                From = newAssignment.From,
                To = newAssignment.To,
                Bed = new BedDto
                {
                    Id = availableBed.Id,
                    BedType = new BedTypeDto
                    {
                        Id = availableBed.BedType.Id,
                        Name = availableBed.BedType.Name,
                        Description = availableBed.BedType.Description
                    },
                    Room = new RoomDto
                    {
                        Id = availableBed.Room.Id,
                        HasTv = availableBed.Room.HasTv,
                        Ward = new WardDto
                        {
                            Id = availableBed.Room.Ward.Id,
                            Name = availableBed.Room.Ward.Name,
                            Description = availableBed.Room.Ward.Description
                        }
                    }
                }
            };
        }
    }
}