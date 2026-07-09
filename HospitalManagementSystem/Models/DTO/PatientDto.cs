namespace HospitalManagementSystem.Models.DTO
{
    public class PatientDto
    {

        public required string PatientName { get; set; }

        public required string PatientAge { get; set; }

        public required string PatientGender { get; set; }

        public required string PatientPhoneNumber { get; set; }

        public required string PatientAddress { get; set; }

        public required string TypeofCheckUp { get; set; }

        public DateTime DateTime { get; set; }

        public required string SelectDoctor { get; set; }

        public required string Status { get; set; }

        public string? DoctorID { get; set; }

        public string? NurseID { get; set; }

    }
}
