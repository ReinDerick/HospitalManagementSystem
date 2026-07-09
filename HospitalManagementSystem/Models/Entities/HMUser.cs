using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class HMUser
    {
        [Key]
        public Guid UserID { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string Email { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Patient>? DoctorPatient { get; set; } //MedicalPractitioner

        public ICollection<Patient>? NursePatient { get; set; }

        public ICollection<HMUserRole>? UserRoles { get; set; }
    }
}
