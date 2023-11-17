using Timesheet.Core.Abstractions;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;

namespace Timesheet.Infrastructure.Services;

public class FileRepositoryService : IFileRepositoryService
{
    private readonly IDbContext _dbContext;

    public FileRepositoryService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<FileRepository> GetBaseQuery() =>
        _dbContext.Set<FileRepository>()
            .AsQueryable();

    public Task<FileRepository?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.FileRepositoryId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<FileRepository>> GetAllAsync(CancellationToken cancellationToken)
        => GetBaseQuery()
            .ToListAsync(cancellationToken);

    public async Task<FileRepository?> CreateAsync(FileRepository entity, CancellationToken cancellationToken)
    {
        _dbContext.Insert(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<FileRepository>()
            .Select(e => new FileRepository
            {
                FileRepositoryId = e.FileRepositoryId
            })
            .Where(e => e.FileRepositoryId == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(entity);

        entity.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<long> TotalUsedStorageAsync(CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.IsFileDeleted == false)
            .SumAsync(e => e.Size, cancellationToken);

    public Task<FileRepository?> GetByUniqueFileNameAsync(string fileName, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.UniqueFileName == fileName)
            .FirstOrDefaultAsync(cancellationToken);
}