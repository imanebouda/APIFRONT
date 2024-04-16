using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class PermissionRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int? CreatedBy { get; set; } // Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        [ForeignKey("RoleId")] // Renommez la clé étrangère pour correspondre à la clé primaire de Role
        public Role Roles { get; set; }

        [ForeignKey("CreatedBy")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        public User CreatedByUser { get; set; }

        [ForeignKey("PermissionId")] // Renommez la clé étrangère pour correspondre à la clé primaire de Permission
        public Permission Permissions { get; set; }
    }
}
