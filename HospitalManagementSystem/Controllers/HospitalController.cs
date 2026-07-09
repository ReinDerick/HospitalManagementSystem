using HospitalManagementSystem.Api.Users.Infrastructure;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HospitalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly HospitalDbContext dbContext;
        private readonly TokenProvider tokenProvider;

        public HospitalController(HospitalDbContext dbContext, TokenProvider tokenProvider)
        {
            this.dbContext = dbContext;
            this.tokenProvider = tokenProvider;
        }

        [HttpGet("PatientStatus/{UserID}")]
        public IActionResult Get()
        {
            return Ok("Hello from HospitalController!");
        }

        [HttpPost("UserRegister")]
        public IActionResult CreateUser(UserAccountDto request)
        {
            try
            {
                var existingUser = dbContext.hMUsers.FirstOrDefault(p => p.UserName == request.UserName);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "Username already exists!" });
                }

                var existingEmail = dbContext.hMUsers.FirstOrDefault(p => p.Email == request.Email);

                if (existingEmail != null)
                {
                    return BadRequest(new { message = "Email already exists!" });
                }


                var user = new HMUser
                {
                    UserID = Guid.NewGuid(),
                    UserName = request.UserName,
                    Password = request.Password,
                    Email = request.Email,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                dbContext.hMUsers.Add(user);

                var userRole = new HMUserRole
                {
                    UserRoleID = Guid.NewGuid().ToString(),
                    UserID = user.UserID,  //linked to HMUser
                    RoleID = request.RoleID,  //linked to HMUser
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = request.CreatedBy,
                    IsActive = true
                };

                dbContext.hMUserRoles.Add(userRole);

                dbContext.SaveChanges();

                return Ok(new
                {
                    message = "User Added Successfully!",
                    userName = request.UserName,
                    password = request.Password,
                    email = request.Email,
                    roleID = userRole.RoleID,
                    createdby = request.CreatedBy,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpPost("UserLogin")]
        [AllowAnonymous]
        public IActionResult UserLogin(UserLogInDto userLogIn)
        {
            try
            {
                var user = dbContext.hMUsers.FirstOrDefault(p => p.UserName == userLogIn.UserName && p.Password == userLogIn.Password);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid username or password!" });
                }
                var userRole = dbContext.hMUserRoles.FirstOrDefault(p => p.UserID == user.UserID);
                if (userRole == null)
                {
                    return BadRequest(new { message = "User not found!" });
                }

                string roleName = "";
                string redirectUrl = "";

                switch (userRole.RoleID)
                {
                    case "1":
                        roleName = "Administrator";
                        redirectUrl = "";
                        break;
                    case "2":
                        roleName = "Contributor";
                        redirectUrl = "";
                        break;
                    case "3":
                        roleName = "Doctor";
                        redirectUrl = "doctor-dashboard.html";
                        break;
                    case "4":
                        roleName = "Nurse";
                        redirectUrl = "nurse-dashboard.html";
                        break;
                    case "5":
                        roleName = "Employees";
                        redirectUrl = "";
                        break;

                    default:
                        return BadRequest(new { message = "Invalid user role!" });
                }

                string token = tokenProvider.Create(user);

                return Ok(new
                {
                    message = "Login successful!",
                    token = token,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpPost("PatientInformation")]
        public IActionResult PatientInfo(PatientDto patientDto)
        {
            try
            {
                var Patients = dbContext.patients.FirstOrDefault(p => p.PatientName == patientDto.PatientName && p.IsActive == true);

                if (Patients != null)
                {
                    return BadRequest(new { message = "Patient Username already exists" });
                }

                // Parse string DoctorID from frontend DTO to a C# Guid type safely (IPAEXPLAIN OR ISEARCH DIN PANO ANG GINAGAWA NG CODE NATO)
                Guid? parsedDoctorId = null;
                if (!string.IsNullOrEmpty(patientDto.DoctorID) && Guid.TryParse(patientDto.DoctorID, out Guid tempDocGuid))
                {
                    parsedDoctorId = tempDocGuid;
                }

                // 2. FIXED: Parse NurseID from frontend DTO to a C# Guid safely (IPAEXPLAIN OR ISEARCH DIN PANO ANG GINAGAWA NG CODE NATO)
                Guid? parsedNurseId = null;
                if (!string.IsNullOrEmpty(patientDto.NurseID) && Guid.TryParse(patientDto.NurseID, out Guid tempNurseGuid))
                {
                    parsedNurseId = tempNurseGuid;
                }

                var PatientInfo = new Patient
                {
                    PatientID = Guid.NewGuid(),
                    PatientName = patientDto.PatientName,
                    PatientAge = patientDto.PatientAge,
                    PatientGender = patientDto.PatientGender,
                    PatientPhoneNumber = patientDto.PatientPhoneNumber,
                    PatientAddress = patientDto.PatientAddress,
                    TypeofCheckUp = patientDto.TypeofCheckUp,
                    DateTime = patientDto.DateTime,
                    SelectDoctor = patientDto.SelectDoctor,
                    Status = "Pending",
                    IsActive = true,

                    // FIXED: Map your parsed DoctorID right here! (IPAEXPLAIN OR ISEARCH DIN PANO ANG GINAGAWA NG CODE NATO)
                    DoctorID = parsedDoctorId,
                    NurseID = parsedNurseId
                };

                dbContext.patients.Add(PatientInfo);
                dbContext.SaveChanges();

                return Ok(new
                {
                    message = "Patient Checkup Added Successfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("PatientLogs")]
        public IActionResult GetPatientLogsByDoctor(string doctorName)
        {
            try
            {
                // (IPAEXPLAIN OR ISEARCH DIN PANO ANG GINAGAWA NG CODE NATO, SPECIFICALLY YUNG .Include) include is parang join
                var doctorLogs = dbContext.patients.Include(p => p.Nurse).AsNoTracking().Where(p => p.SelectDoctor == doctorName && p.IsActive).OrderByDescending(p => p.DateTime).ToList();

                if (!doctorLogs.Any())
                {
                    return NotFound( new{ message = "No patient logs found for this doctor."});
                }

                return Ok(new
                {
                    message = "Patient logs retrieved successfully!",
                    doctorName = doctorName,

                    logs = doctorLogs.Select(p => new
                    {
                        patientID = p.PatientID,
                        patientName = p.PatientName,
                        patientAge = p.PatientAge,
                        patientGender = p.PatientGender,
                        patientPhoneNumber = p.PatientPhoneNumber,
                        patientAddress = p.PatientAddress,
                        typeOfCheckUp = p.TypeofCheckUp,
                        dateTime = p.DateTime,
                        status = p.Status,

                        // (IPAEXPLAIN OR ISEARCH DIN PANO ANG GINAGAWA NG CODE NATO)
                        addedByNurse = p.Nurse != null //Ternary Operator
                            ? p.Nurse.UserName
                            : "System"
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("PatientLog")] // Keeps your standard resource name
        public IActionResult DeletePatientLog([FromQuery] Guid patientID)
        {
            try
            {
                var patient = dbContext.patients.FirstOrDefault(p => p.PatientID == patientID && p.IsActive == true);

                if (patient == null)
                {
                    return NotFound(new { message = "Patient log not found!" });
                }

                string assignedDoctor = patient.SelectDoctor;

                // Perform Soft Delete
                patient.IsActive = false;
                dbContext.SaveChanges();

                // Get remaining active logs for this doctor
                var remainingLogs = dbContext.patients.Where(p => p.SelectDoctor == assignedDoctor && p.IsActive == true).OrderByDescending(p => p.DateTime).ToList();

                return Ok(new
                {
                    message = "Patient log deleted successfully.",
                    logs = remainingLogs.Select(p => new
                    {
                        patientID = p.PatientID,
                        patientName = p.PatientName,
                        patientAge = p.PatientAge,
                        patientGender = p.PatientGender,
                        patientPhoneNumber = p.PatientPhoneNumber,
                        patientAddress = p.PatientAddress,
                        typeOfCheckUp = p.TypeofCheckUp,
                        dateTime = p.DateTime
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPut("PatientStatus/{patientID}")]
        public IActionResult UpdatePatientStatus(Guid patientID, [FromBody] UpdatePatientStatusDto dto)
        {
            try
            {
                var patient = dbContext.patients.FirstOrDefault(p => p.PatientID == patientID && p.IsActive == true);
                if (patient == null)
                {
                    return NotFound(new { message = "Patient log not found!" });
                }
                 
                var allowedStatuses = new List<string> { "Pending", "Done", "Cancelled" };

                if (!allowedStatuses.Contains(dto.Status))
                {
                    return BadRequest(new { message = "Invalid status value!" });
                }

                patient.Status = dto.Status;
                dbContext.SaveChanges();
                return Ok( new { message = "Patient Status Updated Successfully! "});
            
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpGet("PatientLogsByNurse")]
        public IActionResult GetPatientLogsByNurse(string nurseName)
        {
            try
            {
                var nurseLogs = dbContext.patients.Include(p => p.Doctor).AsNoTracking().Where(p => p.Nurse != null && p.Nurse.UserName == nurseName && p.IsActive).OrderByDescending(p => p.DateTime).ToList();
                if (!nurseLogs.Any())
                {
                    return NotFound(new { message = "No patient logs found for this nurse." });
                }
                return Ok(new
                {
                    message = "Patient logs retrieved successfully!",
                    nurseName = nurseName,
                    logs = nurseLogs.Select(p => new
                    {
                        patientID = p.PatientID,
                        patientName = p.PatientName,
                        patientAge = p.PatientAge,
                        patientGender = p.PatientGender,
                        patientPhoneNumber = p.PatientPhoneNumber,
                        patientAddress = p.PatientAddress,
                        typeOfCheckUp = p.TypeofCheckUp,
                        dateTime = p.DateTime,
                        status = p.Status,
                        assignedDoctor = p.Doctor != null ? p.Doctor.UserName : "Not Assigned"
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
    }
}
