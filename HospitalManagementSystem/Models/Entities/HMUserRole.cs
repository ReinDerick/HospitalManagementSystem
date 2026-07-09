using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class HMUserRole
    {

        [Key]
        public required string UserRoleID { get; set; }

        public Guid UserID { get; set; }

        public required string RoleID { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public required string LastUpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public HMUser? User { get; set; }

        public HMRole? Role { get; set; }

    }
}
