using System.Linq.Expressions;
using Timesheet.Core.Abstractions;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;

namespace Timesheet.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IDbContext _dbContext;

    public RoleService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Role> GetBaseQuery()
        => _dbContext.Set<Role>()
            .Where(e => e.IsDefault == false)
            .AsQueryable();

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _dbContext.Set<Role>()
            .Where(e => e.RoleId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<Role>> GetAllAsync(CancellationToken cancellationToken)
        => _dbContext.Set<Role>()
            .ToListAsync(cancellationToken);

    public async Task<Role?> CreateAsync(Role entity, CancellationToken cancellationToken)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var role = await GetByExpressionAsync(
            e => e.RoleId == id,
            e => new Role
            {
                RoleId = e.RoleId
            }, cancellationToken);
        if (role is null)
            throw new Exception("Data role not found");

        _dbContext.AttachEntity(role);

        role.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Role?> GetByExpressionAsync(Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Code == code)
            .FirstOrDefaultAsync(cancellationToken);
}