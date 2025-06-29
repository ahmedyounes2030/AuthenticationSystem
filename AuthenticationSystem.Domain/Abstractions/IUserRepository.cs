﻿using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Domain.Abstractions;

public interface IUserRepository : IRepository<int, User>
{
    Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
}
