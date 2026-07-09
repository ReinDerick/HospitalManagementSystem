namespace HospitalManagementSystem.Models.DTO
{
    public class UserAccountDto
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string Email { get; set; }

        public required string RoleID { get; set; }

        public required string CreatedBy { get; set; }
    }
}
