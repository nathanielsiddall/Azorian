using System.Runtime.Serialization;

namespace Azorian.Models;

/// <summary>
/// Represents the status of a user account.
/// </summary>
public enum UserStatus
{
    /// <summary>Active user.</summary>
    Active,
    /// <summary>User is suspended.</summary>
    Suspended,
    /// <summary>User is banned.</summary>
    Banned
}
