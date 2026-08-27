using Microsoft.AspNetCore.Identity;

namespace QPET.Models
{
    public class AdminUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}