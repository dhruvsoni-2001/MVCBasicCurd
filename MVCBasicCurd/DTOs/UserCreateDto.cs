using System.ComponentModel.DataAnnotations;

namespace MVCBasicCurd.DTOs
{
    public class UserCreateDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;
    }
}


// Recommended (cleaner) approach — use a DTO for the Create form Better practice is to separate the input model (what the form submits) from the entity model (database entity). Create a UserCreateDto with only the fields your form needs, bind the action to it, and then map to User, generating BusinessUserId server-side.