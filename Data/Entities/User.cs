namespace Azorian.Data;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Identity
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    // Profile
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }

    // Status and access
    public bool IsActive { get; set; } = true;
    public string Role { get; set; } = "User"; // e.g., Admin, Instructor, Student

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}