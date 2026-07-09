using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class Patient
    {

        [Key]
        public Guid PatientID { get; set; }

        public required string PatientName { get; set; }

        public required string PatientAge { get; set; }

        public required string PatientGender { get; set; }

        public required string PatientPhoneNumber { get; set; }

        public required string PatientAddress { get; set; }

        public required string TypeofCheckUp { get; set; }

        public DateTime DateTime { get; set; }

        public required string SelectDoctor { get; set; }

        public bool IsActive { get; set; }

        public required string Status { get; set; }

        public Guid? DoctorID { get; set; }

        public Guid? NurseID { get; set; }

        public HMUser? Nurse { get; set; }

        public HMUser? Doctor { get; set; }
    }
}
