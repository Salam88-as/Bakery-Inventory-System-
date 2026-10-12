using System.ComponentModel.DataAnnotations;

namespace BakeryInventorySystem.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Staff";

        public bool IsActive { get; set; } = true;
    }
}