using System.Collections.Concurrent;
using Azorian.Models;
using BCrypt.Net;

namespace Azorian.Services;

/// <summary>
/// Provides an in-memory implementation of <see cref="IUserService"/>.
/// This implementation is intended for demo or testing purposes only.
/// </summary>
public class InMemoryUserService : IUserService
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    /// <inheritdoc />
    public IEnumerable<User> GetAll() => _users.Values;

    /// <inheritdoc />
    public User? GetById(Guid id) => _users.TryGetValue(id, out var user) ? user : null;

    /// <inheritdoc />
    public User Create(UserCredentials credentials)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = credentials.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(credentials.Password)
        };
        _users[user.Id] = user;
        return user;
    }

    /// <inheritdoc />
    public bool Update(User user)
    {
        if (!_users.ContainsKey(user.Id))
        {
            return false;
        }

        _users[user.Id] = user;
        return true;
    }

    /// <inheritdoc />
    public bool Delete(Guid id) => _users.TryRemove(id, out _);

    /// <inheritdoc />
    public User? Authenticate(UserCredentials credentials)
    {
        var user = _users.Values.FirstOrDefault(u => u.Username == credentials.Username);
        if (user is null)
        {
            return null;
        }

        return BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash) && !user.IsBanned && !user.IsSuspended
            ? user
            : null;
    }

    /// <inheritdoc />
    public bool Suspend(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
        {
            user.IsSuspended = true;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool Unsuspend(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
        {
            user.IsSuspended = false;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool Ban(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
        {
            user.IsBanned = true;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public bool Unban(Guid id)
    {
        if (_users.TryGetValue(id, out var user))
        {
            user.IsBanned = false;
            return true;
        }
        return false;
    }
}
