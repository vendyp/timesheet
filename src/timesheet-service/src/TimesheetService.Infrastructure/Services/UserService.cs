using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;

namespace TimesheetService.Infrastructure.Services;

public class UserService : BaseEntityService<User>, IUserService
{
    private readonly ISalter _salter;

    public UserService(IDbContext dbContext, ISalter salter) : base(dbContext)
    {
        _salter = salter;
    }

    public override IQueryable<User> GetBaseQuery()
        => DbContext.Set<User>()
            .Include(e => e.UserRoles)
            .ThenInclude(e => e.Role!.RoleScopes)
            .AsQueryable();

    public override Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

    public override Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        => GetBaseQuery().ToListAsync(cancellationToken);

    public override async Task<User?> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        DbContext.Insert(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new Exception("Data not found");

        DbContext.AttachEntity(user);

        user.SetToDeleted();

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.NormalizedUsername == username.ToUpper())
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> IsUserExistAsync(string username, CancellationToken cancellationToken)
    {
        username = username.ToUpper();

        return GetBaseQuery().Where(e => e.NormalizedUsername == username)
            .AnyAsync(cancellationToken);
    }

    public bool VerifyPassword(string currentPassword, string salt, string password)
        => currentPassword == _salter.Hash(salt, password);
}