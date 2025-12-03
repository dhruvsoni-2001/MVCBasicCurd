// DTOs/UserEditDto.cs
using System.ComponentModel.DataAnnotations;

namespace MVCBasicCurd.DTOs
{
    public class UserEditDto
    {
        public Guid UserId { get; set; }

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