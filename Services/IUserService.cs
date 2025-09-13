using Azorian.Models;

namespace Azorian.Services;

/// <summary>
/// Defines operations for managing application users.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <returns>A sequence of <see cref="User"/> objects.</returns>
    IEnumerable<User> GetAll();

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>The matching <see cref="User"/> or <c>null</c> if not found.</returns>
    User? GetById(Guid id);

    /// <summary>
    /// Creates a new user with the specified credentials.
    /// </summary>
    /// <param name="credentials">The username and plain-text password.</param>
    /// <returns>The created <see cref="User"/>.</returns>
    User Create(UserCredentials credentials);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">User information to update.</param>
    /// <returns><c>true</c> if the user exists and was updated; otherwise <c>false</c>.</returns>
    bool Update(User user);

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="id">The user's identifier.</param>
    /// <returns><c>true</c> if the user was removed; otherwise <c>false</c>.</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Attempts to authenticate a user with the given credentials.
    /// </summary>
    /// <param name="credentials">The username and password.</param>
    /// <returns>The authenticated <see cref="User"/> or <c>null</c>.</returns>
    User? Authenticate(UserCredentials credentials);

    /// <summary>
    /// Marks a user as suspended.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns><c>true</c> if the user exists and was suspended.</returns>
    bool Suspend(Guid id);

    /// <summary>
    /// Removes the suspended status from a user.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns><c>true</c> if the user exists and was unsuspended.</returns>
    bool Unsuspend(Guid id);

    /// <summary>
    /// Marks a user as banned.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns><c>true</c> if the user exists and was banned.</returns>
    bool Ban(Guid id);

    /// <summary>
    /// Removes the banned status from a user.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns><c>true</c> if the user exists and was unbanned.</returns>
    bool Unban(Guid id);
}
