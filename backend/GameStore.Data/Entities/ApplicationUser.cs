using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}