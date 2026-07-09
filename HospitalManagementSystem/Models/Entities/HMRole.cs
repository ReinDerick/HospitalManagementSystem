using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities

{
    public class HMRole
    {
        [Key]
        public required string RoleID { get; set; }  

        public required string RoleName { get; set; }

        public required string RoleDescription { get; set; }

        public required int DisplayOrder { get; set; }

        public required string Category { get; set; }

        public ICollection<HMUserRole>? UserRoles { get; set; }
    }
}
