using MVCBasicCurd.Models;
using System.ComponentModel.DataAnnotations;

namespace CompleteBanking_MVC.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}

// This keeps things simple and extensible: a user can have multiple roles.
